/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.SQL.Interfaces;
using Utilities.SQL.MicroORM.Interfaces;
#endregion

namespace Utilities.SQL.MicroORM
{
    /// <summary>
    /// Holds information for a set of commands
    /// </summary>
    public class BatchCommand : ICommand,IBatchCommand
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Commands">Commands that are being merged for batching</param>
        public BatchCommand(params Command[] Commands)
        {
            this._SQLCommand = "";
            this._Parameters = new List<IParameter>();
            this.CommandType = CommandType.Text;
            this.Commands = new List<ICommand>();
            this.Batched = false;
            AddCommands(Commands);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Number of commands being batched
        /// </summary>
        public virtual int CommandCount { get { return Commands.Count; } }

        /// <summary>
        /// Actual SQL command
        /// </summary>
        public virtual string SQLCommand
        {
            get
            {
                if (!Batched)
                    Batch();
                return _SQLCommand;
            }
        }

        /// <summary>
        /// Batched SQL Command
        /// </summary>
        protected string _SQLCommand = "";

        /// <summary>
        /// Command type
        /// </summary>
        public virtual CommandType CommandType { get; protected set; }

        /// <summary>
        /// Parameters associated with the command
        /// </summary>
        public virtual List<IParameter> Parameters
        {
            get
            {
                if (!Batched)
                    Batch();
                return _Parameters;
            }
        }

        /// <summary>
        /// Batched parameter list
        /// </summary>
        protected List<IParameter> _Parameters = new List<IParameter>();

        /// <summary>
        /// Used to parse SQL commands to find parameters (when batching)
        /// </summary>
        protected static Regex ParameterRegex = new Regex(@"[^@](?<ParamStart>[:@?])(?<ParamName>\w+)", RegexOptions.Compiled);

        /// <summary>
        /// Commands to batch
        /// </summary>
        protected virtual List<ICommand> Commands { get; set; }

        /// <summary>
        /// Has this been batched?
        /// </summary>
        protected virtual bool Batched { get; set; }

        #endregion

        #region Functions

        #region AddCommand

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="ParameterPrefix">Parameter prefix to use</param>
        /// <param name="Parameters">Parameters to add</param>
        /// <returns>This</returns>
        public IBatchCommand AddCommand(string Command, CommandType CommandType, string ParameterPrefix, params object[] Parameters)
        {
            return AddCommands(new Command(Command, CommandType, ParameterPrefix, Parameters));
        }

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters to add</param>
        /// <returns>This</returns>
        public IBatchCommand AddCommand(string Command, CommandType CommandType, params IParameter[] Parameters)
        {
            return AddCommands(new Command(Command, CommandType, Parameters));
        }

        #endregion

        #region AddCommands

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Commands">Commands to add</param>
        /// <returns>This</returns>
        public virtual IBatchCommand AddCommands(params Command[] Commands)
        {
            if (Commands != null)
            {
                Batched = false;
                this.Commands.AddRange(Commands);
            }
            return this;
        }

        #endregion

        #region AddParameter

        /// <summary>
        /// Adds a parameter to the command
        /// </summary>
        /// <param name="ID">Parameter ID</param>
        /// <param name="Parameter">Parameter value</param>
        /// <typeparam name="DataType">Data type of the parameter</typeparam>
        /// <param name="Direction">Direction of the parameter</param>
        /// <param name="ParameterStarter">Parameter prefix</param>
        public virtual void AddParameter<DataType>(string ID, DataType Parameter, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
        {
        }

        /// <summary>
        /// Adds a parameter to the command
        /// </summary>
        /// <param name="ID">Parameter ID</param>
        /// <param name="Parameter">Parameter value</param>
        /// <param name="Direction">Parameter direction</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        public virtual void AddParameter(string ID, string Parameter, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
        {
        }

        /// <summary>
        /// Adds a parameter to the command
        /// </summary>
        /// <param name="Parameters">Parameter values</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        public virtual void AddParameter(string ParameterStarter, params object[] Parameters)
        {
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        /// <param name="ParameterStarter">Parameter prefix</param>
        public virtual void AddParameter(string ID, SqlDbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
        {
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        /// <param name="ParameterStarter">Parameter prefix</param>
        public virtual void AddParameter(string ID, DbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
        {
        }

        /// <summary>
        /// Adds a parameter to the command
        /// </summary>
        /// <param name="Parameters">Parameter values</param>
        public virtual void AddParameter(params IParameter[] Parameters)
        {
        }

        #endregion

        #region ClearParameters

        /// <summary>
        /// Clears the parameters for the command
        /// </summary>
        public virtual void ClearParameters()
        {
        }

        #endregion

        #region Batch

        /// <summary>
        /// Batches the data
        /// </summary>
        protected virtual void Batch()
        {
            Batched = true;
            this._Parameters = new List<IParameter>();
            this._SQLCommand = "";
            int Count = 0;
            if (Commands.Count == 1)
            {
                this._SQLCommand = Commands[0].SQLCommand;
                this._Parameters = Commands[0].Parameters;
                return;
            }
            foreach (ICommand Command in Commands)
            {
                if (Command.CommandType == System.Data.CommandType.Text)
                {
                    this._SQLCommand += Command.SQLCommand.IsNullOrEmpty() ?
                                        "" :
                                        ParameterRegex.Replace(Command.SQLCommand, x =>
                                        {
                                            if (!x.Value.StartsWith("@@"))
                                                return x.Value + "Command" + Count.ToString();
                                            return x.Value;
                                        }) + System.Environment.NewLine;
                    foreach (IParameter Parameter in Command.Parameters)
                    {
                        this._Parameters.Add(Parameter.CreateCopy("Command" + Count.ToString()));
                    }
                }
                else
                {
                    this._SQLCommand += Command.SQLCommand + System.Environment.NewLine;
                    foreach (IParameter Parameter in Command.Parameters)
                    {
                        this._Parameters.Add(Parameter.CreateCopy(""));
                    }
                }
                ++Count;
            }
        }

        #endregion

        #region Equals

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Determines if the commands are equal</returns>
        public override bool Equals(object obj)
        {
            Command OtherCommand = obj as Command;
            if (OtherCommand == null)
                return false;

            if (OtherCommand.SQLCommand != SQLCommand
                || OtherCommand.CommandType != CommandType
                || Parameters.Count != OtherCommand.Parameters.Count)
                return false;

            foreach (IParameter Parameter in Parameters)
                if (!OtherCommand.Parameters.Contains(Parameter))
                    return false;

            foreach (IParameter Parameter in OtherCommand.Parameters)
                if (!Parameters.Contains(Parameter))
                    return false;

            return true;
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Returns the hash code for the command
        /// </summary>
        /// <returns>The hash code for the object</returns>
        public override int GetHashCode()
        {
            int ParameterTotal = Parameters.Sum(x => x.GetHashCode());
            if (ParameterTotal > 0)
                return (SQLCommand.GetHashCode() * 23 + CommandType.GetHashCode()) * 23 + ParameterTotal;
            return SQLCommand.GetHashCode() * 23 + CommandType.GetHashCode();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of the command
        /// </summary>
        /// <returns>The string representation of the command</returns>
        public override string ToString()
        {
            return SQLCommand;
        }

        #endregion

        #endregion
    }
}