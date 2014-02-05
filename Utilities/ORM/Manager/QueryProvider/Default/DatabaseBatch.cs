/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.DataTypes;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

#endregion Usings

namespace Utilities.ORM.Manager.QueryProvider.Default
{
    /// <summary>
    /// Database batch class
    /// </summary>
    public class DatabaseBatch : IBatch
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseBatch()
            : base()
        {
            this.Commands = new List<ICommand>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Source">Source info</param>
        public DatabaseBatch(ISourceInfo Source)
            : this()
        {
            this.Source = Source;
        }

        /// <summary>
        /// Command count
        /// </summary>
        public int CommandCount { get { return Commands.Count; } }

        /// <summary>
        /// Commands to batch
        /// </summary>
        protected ICollection<ICommand> Commands { get; private set; }

        /// <summary>
        /// Connection string
        /// </summary>
        protected ISourceInfo Source { get; set; }

        /// <summary>
        /// Used to parse SQL commands to find parameters (when batching)
        /// </summary>
        private static Regex ParameterRegex = new Regex(@"[^@](?<ParamStart>[:@?])(?<ParamName>\w+)", RegexOptions.Compiled);

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>This</returns>
        public IBatch AddCommand(CommandType CommandType, string Command)
        {
            Commands.Add(new Command(Command, CommandType, null));
            return this;
        }

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters to add</param>
        /// <returns>This</returns>
        public IBatch AddCommand(string Command, CommandType CommandType, params object[] Parameters)
        {
            Commands.Add(new Command(Command, CommandType, Source.ParameterPrefix, Parameters));
            return this;
        }

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters to add</param>
        /// <returns>This</returns>
        public IBatch AddCommand(string Command, CommandType CommandType, params IParameter[] Parameters)
        {
            Commands.Add(new Command(Command, CommandType, Parameters));
            return this;
        }

        /// <summary>
        /// Adds a batch's commands to the current batch
        /// </summary>
        /// <param name="Batch">Batch to add</param>
        /// <returns>This</returns>
        public IBatch AddCommand(IBatch Batch)
        {
            DatabaseBatch TempValue = Batch as DatabaseBatch;
            if (TempValue == null)
                return this;
            Commands.Add(TempValue.Commands);
            return this;
        }

        /// <summary>
        /// Executes the commands and returns the results
        /// </summary>
        /// <returns>The results of the batched commands</returns>
        public IEnumerable<IEnumerable<dynamic>> Execute()
        {
            List<IParameter> FinalParameters = new List<IParameter>();
            string FinalSQLCommand = "";
            BatchCommands(ref FinalParameters, ref FinalSQLCommand);
            return ExecuteCommands(FinalSQLCommand, FinalParameters);
        }

        private static IEnumerable<dynamic> GetValues(DbDataReader TempReader)
        {
            Contract.Requires<ArgumentNullException>(TempReader != null, "TempReader");
            List<dynamic> ReturnValue = new List<dynamic>();
            string[] FieldNames = new string[TempReader.FieldCount];
            for (int x = 0; x < TempReader.FieldCount; ++x)
            {
                FieldNames[x] = TempReader.GetName(x);
            }
            while (TempReader.Read())
            {
                IDictionary<string, object> Value = new Dynamo();
                for (int x = 0; x < TempReader.FieldCount; ++x)
                {
                    Value.Add(FieldNames[x], TempReader[x]);
                }
                ReturnValue.Add(Value);
            }
            return ReturnValue;
        }

        private void BatchCommands(ref List<IParameter> FinalParameters, ref string FinalSQLCommand)
        {
            int Count = 0;
            if (Commands.Count == 1)
            {
                FinalSQLCommand = Commands.FirstOrDefault().SQLCommand;
                FinalParameters = Commands.FirstOrDefault().Parameters.ToList();
            }
            else
            {
                foreach (ICommand Command in Commands)
                {
                    if (Command.CommandType == System.Data.CommandType.Text)
                    {
                        FinalSQLCommand += string.IsNullOrEmpty(Command.SQLCommand) ?
                                            "" :
                                            ParameterRegex.Replace(Command.SQLCommand, x =>
                                            {
                                                if (!x.Value.StartsWith("@@", StringComparison.Ordinal))
                                                    return x.Value + "Command" + Count.ToString(CultureInfo.InvariantCulture);
                                                return x.Value;
                                            }) + System.Environment.NewLine;
                        foreach (IParameter Parameter in Command.Parameters)
                        {
                            FinalParameters.Add(Parameter.CreateCopy("Command" + Count.ToString(CultureInfo.InvariantCulture)));
                        }
                    }
                    else
                    {
                        FinalSQLCommand += Command.SQLCommand + System.Environment.NewLine;
                        foreach (IParameter Parameter in Command.Parameters)
                        {
                            FinalParameters.Add(Parameter.CreateCopy(""));
                        }
                    }
                    ++Count;
                }
            }
        }

        private IEnumerable<IEnumerable<dynamic>> ExecuteCommands(string FinalSQLCommand, List<IParameter> FinalParameters)
        {
            List<IEnumerable<dynamic>> ReturnValue = new List<IEnumerable<dynamic>>();
            if (string.IsNullOrEmpty(FinalSQLCommand))
            {
                ReturnValue.Add(new List<dynamic>());
                return ReturnValue;
            }
            DbProviderFactory Factory = DbProviderFactories.GetFactory(Source.SourceType);
            using (DbConnection Connection = Factory.CreateConnection())
            {
                Connection.ConnectionString = Source.Connection;
                using (DbCommand ExecutableCommand = Factory.CreateCommand())
                {
                    ExecutableCommand.CommandText = FinalSQLCommand;
                    ExecutableCommand.Connection = Connection;
                    ExecutableCommand.CommandType = CommandType.Text;
                    FinalParameters.ForEach(x => x.AddParameter(ExecutableCommand));
                    if (Commands.Count > 1
                        && !FinalSQLCommand.Contains("ALTER DATABASE")
                        && !FinalSQLCommand.Contains("CREATE DATABASE"))
                        ExecutableCommand.BeginTransaction();
                    ExecutableCommand.Open();
                    try
                    {
                        using (DbDataReader TempReader = ExecutableCommand.ExecuteReader())
                        {
                            ReturnValue.Add(GetValues(TempReader));
                            while (TempReader.NextResult())
                            {
                                ReturnValue.Add(GetValues(TempReader));
                            }
                        }
                        ExecutableCommand.Commit();
                    }
                    catch { ExecutableCommand.Rollback(); throw; }
                    finally { ExecutableCommand.Close(); }
                }
            }
            return ReturnValue;
        }
    }
}