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

using System;
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
        /// <param name="Source">Source info</param>
        public DatabaseBatch(ISourceInfo Source)
        {
            Commands = new List<Command>();
            this.Source = Source;
        }

        /// <summary>
        /// Command count
        /// </summary>
        public int CommandCount { get { return Commands.Count; } }

        /// <summary>
        /// Commands to batch
        /// </summary>
        protected IList<Command> Commands { get; private set; }

        /// <summary>
        /// Connection string
        /// </summary>
        protected ISourceInfo Source { get; set; }

        /// <summary>
        /// Used to parse SQL commands to find parameters (when batching)
        /// </summary>
        private static readonly Regex ParameterRegex = new Regex(@"[^@](?<ParamStart>[:@?])(?<ParamName>\w+)", RegexOptions.Compiled);

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="CallBack">Callback action</param>
        /// <param name="Object">Object used in the callback action</param>
        /// <returns>This</returns>
        public IBatch AddCommand(Action<Command, IList<dynamic>> CallBack, object Object, CommandType CommandType, string Command)
        {
            Commands.Add(new Command(CallBack, Object, Command, CommandType, null));
            return this;
        }

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters to add</param>
        /// <param name="CallBack">Callback action</param>
        /// <param name="Object">Object used in the callback action</param>
        /// <returns>This</returns>
        public IBatch AddCommand(Action<Command, IList<dynamic>> CallBack, object Object, string Command, CommandType CommandType, params object[] Parameters)
        {
            Commands.Add(new Command(CallBack, Object, Command, CommandType, Source.ParameterPrefix, Parameters));
            return this;
        }

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters to add</param>
        /// <param name="CallBack">Callback action</param>
        /// <param name="Object">Object used in the callback action</param>
        /// <returns>This</returns>
        public IBatch AddCommand(Action<Command, IList<dynamic>> CallBack, object Object, string Command, CommandType CommandType, params IParameter[] Parameters)
        {
            Commands.Add(new Command(CallBack, Object, Command, CommandType, Parameters));
            return this;
        }

        /// <summary>
        /// Adds a batch's commands to the current batch
        /// </summary>
        /// <param name="Batch">Batch to add</param>
        /// <returns>This</returns>
        public IBatch AddCommand(IBatch Batch)
        {
            var TempValue = Batch as DatabaseBatch;
            if (TempValue == null)
                return this;
            Commands.Add(TempValue.Commands);
            return this;
        }

        /// <summary>
        /// Executes the commands and returns the results
        /// </summary>
        /// <returns>The results of the batched commands</returns>
        public IList<IList<dynamic>> Execute()
        {
            return ExecuteCommands();
        }

        /// <summary>
        /// Removes duplicate commands from the batch
        /// </summary>
        /// <returns>This</returns>
        public IBatch RemoveDuplicateCommands()
        {
            Commands = Commands.Distinct().ToList();
            return this;
        }

        /// <summary>
        /// Converts the batch to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Commands.ToString(x => x.ToString(), Environment.NewLine);
        }

        private static IList<dynamic> GetValues(DbDataReader TempReader)
        {
            Contract.Requires<ArgumentNullException>(TempReader != null, "TempReader");
            var ReturnValue = new List<dynamic>();
            string[] FieldNames = new string[TempReader.FieldCount];
            for (int x = 0; x < TempReader.FieldCount; ++x)
            {
                FieldNames[x] = TempReader.GetName(x);
            }
            while (TempReader.Read())
            {
                var Value = new Dynamo();
                for (int x = 0; x < TempReader.FieldCount; ++x)
                {
                    Value.Add(FieldNames[x], TempReader[x]);
                }
                ReturnValue.Add(Value);
            }
            return ReturnValue;
        }

        private IList<IList<dynamic>> ExecuteCommands()
        {
            Contract.Requires(Source != null);
            if (Commands == null)
                return new List<IList<dynamic>>();
            var ReturnValue = new List<IList<dynamic>>();
            if (Commands.Count == 0)
            {
                ReturnValue.Add(new List<dynamic>());
                return ReturnValue;
            }
            var Factory = DbProviderFactories.GetFactory(Source.SourceType);
            using (DbConnection Connection = Factory.CreateConnection())
            {
                Connection.ConnectionString = Source.Connection;
                using (DbCommand ExecutableCommand = Factory.CreateCommand())
                {
                    ExecutableCommand.Connection = Connection;
                    ExecutableCommand.CommandType = CommandType.Text;
                    if (Commands.Count > 1
                        && !Commands.Any(x => x.SQLCommand.Contains("ALTER DATABASE"))
                        && !Commands.Any(x => x.SQLCommand.Contains("CREATE DATABASE")))
                        ExecutableCommand.BeginTransaction();
                    ExecutableCommand.Open();

                    try
                    {
                        int Count = 0;
                        while (true)
                        {
                            var FinalParameters = new List<IParameter>();
                            bool Finalizable = false;
                            string FinalSQLCommand = "";
                            int ParameterTotal = 0;
                            ExecutableCommand.Parameters.Clear();
                            for (int y = Count; y < Commands.Count; ++y)
                            {
                                ICommand Command = Commands[y];
                                if (ParameterTotal + Command.Parameters.Count > 2100)
                                    break;
                                ParameterTotal += Command.Parameters.Count;
                                Finalizable |= Commands[y].Finalizable;
                                if (Command.CommandType == CommandType.Text)
                                {
                                    FinalSQLCommand += string.IsNullOrEmpty(Command.SQLCommand) ?
                                                        "" :
                                                        ParameterRegex.Replace(Command.SQLCommand, x =>
                                                        {
                                                            if (!x.Value.StartsWith("@@", StringComparison.Ordinal))
                                                                return x.Value + "Command" + Count.ToString(CultureInfo.InvariantCulture);
                                                            return x.Value;
                                                        }) + Environment.NewLine;
                                    foreach (IParameter Parameter in Command.Parameters)
                                    {
                                        FinalParameters.Add(Parameter.CreateCopy("Command" + Count.ToString(CultureInfo.InvariantCulture)));
                                    }
                                }
                                else
                                {
                                    FinalSQLCommand += Command.SQLCommand + Environment.NewLine;
                                    foreach (IParameter Parameter in Command.Parameters)
                                    {
                                        FinalParameters.Add(Parameter.CreateCopy(""));
                                    }
                                }
                                ++Count;
                            }

                            ExecutableCommand.CommandText = FinalSQLCommand;
                            FinalParameters.ForEach(x => x.AddParameter(ExecutableCommand));

                            using (DbDataReader TempReader = ExecutableCommand.ExecuteReader())
                            {
                                if (Finalizable)
                                {
                                    ReturnValue.Add(GetValues(TempReader));
                                    while (TempReader.NextResult())
                                    {
                                        ReturnValue.Add(GetValues(TempReader));
                                    }
                                }
                            }
                            if (Count >= CommandCount)
                                break;
                        }
                        ExecutableCommand.Commit();
                    }
                    catch { ExecutableCommand.Rollback(); throw; }
                    finally { ExecutableCommand.Close(); }
                }
            }
            for (int x = 0, y = 0; x < Commands.Count(); ++x)
            {
                if (Commands[x].Finalizable)
                {
                    Commands[x].Finalize(ReturnValue[y]);
                    ++y;
                }
                else
                    Commands[x].Finalize(new List<dynamic>());
            }
            return ReturnValue;
        }
    }
}