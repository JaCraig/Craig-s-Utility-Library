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
using System.Linq.Expressions;
using Utilities.DataTypes.Patterns;
using Utilities.SQL.MicroORM.Enums;
using System.Data;
using System.Collections.Generic;
using Utilities.SQL.Interfaces;
#endregion

namespace Utilities.SQL.MicroORM.Interfaces
{
    /// <summary>
    /// Command interface
    /// </summary>
    public interface ICommand
    {
        #region Properties

        /// <summary>
        /// Actual SQL command
        /// </summary>
        string SQLCommand { get; }

        /// <summary>
        /// Command type
        /// </summary>
        CommandType CommandType { get; }

        /// <summary>
        /// Parameters associated with the command
        /// </summary>
        List<IParameter> Parameters { get; }

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
        void AddParameter<DataType>(string ID, DataType Parameter, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@");

        /// <summary>
        /// Adds a parameter to the command
        /// </summary>
        /// <param name="ID">Parameter ID</param>
        /// <param name="Parameter">Parameter value</param>
        /// <param name="Direction">Direction of the parameter</param>
        /// <param name="ParameterStarter">Parameter prefix</param>
        void AddParameter(string ID, string Parameter, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@");

        /// <summary>
        /// Adds a parameter to the command
        /// </summary>
        /// <param name="Parameters">Parameter values</param>
        /// <param name="ParameterStarter">Parameter starter to use</param>
        void AddParameter(string ParameterStarter,params object[] Parameters);

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        /// <param name="ParameterStarter">Parameter prefix</param>
        void AddParameter(string ID, SqlDbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@");

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        /// <param name="ParameterStarter">Parameter prefix</param>
        void AddParameter(string ID, DbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@");

        /// <summary>
        /// Adds a parameter to the command
        /// </summary>
        /// <param name="Parameters">Parameter values</param>
        void AddParameter(params IParameter[] Parameters);

        #endregion

        #region ClearParameters

        /// <summary>
        /// Clears the parameters for the command
        /// </summary>
        void ClearParameters();

        #endregion

        #endregion
    }
}