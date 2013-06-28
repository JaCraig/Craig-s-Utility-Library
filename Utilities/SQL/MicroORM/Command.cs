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
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.SQL.Interfaces;
using Utilities.SQL.MicroORM.Interfaces;
#endregion

namespace Utilities.SQL.MicroORM
{
    /// <summary>
    /// Holds information for an individual SQL command
    /// </summary>
    public class Command : ICommand
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SQLCommand">Actual SQL command</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters</param>
        /// <param name="ParameterStarter">Parameter prefix</param>
        public Command(string SQLCommand, CommandType CommandType, string ParameterStarter = "@", params object[] Parameters)
        {
            this.CommandType = CommandType;
            this.SQLCommand = SQLCommand;
            this.Parameters = new List<IParameter>();
            AddParameter(ParameterStarter, Parameters);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SQLCommand">Actual SQL command</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters</param>
        public Command(string SQLCommand, CommandType CommandType, params IParameter[] Parameters)
        {
            this.CommandType = CommandType;
            this.SQLCommand = SQLCommand;
            this.Parameters = new List<IParameter>();
            AddParameter(Parameters);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Actual SQL command
        /// </summary>
        public string SQLCommand { get; protected set; }

        /// <summary>
        /// Command type
        /// </summary>
        public CommandType CommandType { get; protected set; }

        /// <summary>
        /// Parameters associated with the command
        /// </summary>
        public ICollection<IParameter> Parameters { get; private set; }

        #endregion

        #region Functions

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
            this.Parameters.Add(new Parameter<DataType>(ID, Parameter, Direction, ParameterStarter));
        }

        /// <summary>
        /// Adds a parameter to the command
        /// </summary>
        /// <param name="ID">Parameter ID</param>
        /// <param name="Parameter">Parameter value</param>
        /// <param name="Direction">Direction of the parameter</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        public virtual void AddParameter(string ID, string Parameter, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
        {
            this.Parameters.Add(new StringParameter(ID, Parameter, Direction, ParameterStarter));
        }

        /// <summary>
        /// Adds a parameter to the command
        /// </summary>
        /// <param name="Parameters">Parameter values</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        public virtual void AddParameter(string ParameterStarter = "@", params object[] Parameters)
        {
            if (Parameters != null)
            {
                foreach (object Parameter in Parameters)
                {
                    string TempParameter = Parameter as string;
                    if (Parameter == null)
                        this.Parameters.Add(new Parameter<object>(this.Parameters.Count.ToString(CultureInfo.InvariantCulture), default(DbType), null, ParameterDirection.Input, ParameterStarter));
                    else if (TempParameter != null)
                        this.Parameters.Add(new StringParameter(this.Parameters.Count.ToString(CultureInfo.InvariantCulture), TempParameter, ParameterDirection.Input, ParameterStarter));
                    else
                        this.Parameters.Add(new Parameter<object>(this.Parameters.Count.ToString(CultureInfo.InvariantCulture), Parameter, ParameterDirection.Input, ParameterStarter));
                }
            }
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        public virtual void AddParameter(string ID, SqlDbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
        {
            AddParameter(ID, Type.To(DbType.Int32), Value, Direction, ParameterStarter);
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        public virtual void AddParameter(string ID, DbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
        {
            Parameters.Add(new Parameter<object>(ID, Type, Value, Direction, ParameterStarter));
        }

        /// <summary>
        /// Adds a parameter to the command
        /// </summary>
        /// <param name="Parameters">Parameter values</param>
        public virtual void AddParameter(params IParameter[] Parameters)
        {
            if (Parameters != null)
                foreach (IParameter Parameter in Parameters)
                    this.Parameters.Add(Parameter);
        }

        #endregion

        #region ClearParameters

        /// <summary>
        /// Clears the parameters for the command
        /// </summary>
        public virtual void ClearParameters()
        {
            this.Parameters.Clear();
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