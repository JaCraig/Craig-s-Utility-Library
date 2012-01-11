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
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Data.Common;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.DataTypes.Comparison;
#endregion

namespace Utilities.SQL.ExtensionMethods
{
    /// <summary>
    /// Extension methods for DbCommand objects
    /// </summary>
    public static class DbCommandExtensions
    {
        #region Functions

        #region AddParameter

        /// <summary>
        /// Adds a parameter to the call (for strings only)
        /// </summary>
        /// <param name="Command">Command object</param>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Direction">Direction that the parameter goes (in or out)</param>
        /// <param name="Length">Size of the string(either -1 or greater than 4000 should be used to indicate nvarchar(max))</param>
        public static void AddParameter(this DbCommand Command, string ID, int Length, string Value = "",
            ParameterDirection Direction = ParameterDirection.Input)
        {
            if (Length > 4000 || Length < -1)
                Length = -1;
            Command.ThrowIfNull("Command");
            ID.ThrowIfNullOrEmpty("ID");
            DbParameter Parameter = null;
            if (Command.Parameters.Contains(ID))
                Parameter = Command.Parameters[ID];
            else
            {
                Parameter = Command.CreateParameter();
                Command.Parameters.Add(Parameter);
            }
            Parameter.ParameterName = ID;
            Parameter.Value = (string.IsNullOrEmpty(Value)) ? System.DBNull.Value : (object)Value;
            Parameter.IsNullable = (string.IsNullOrEmpty(Value));
            Parameter.DbType = typeof(string).ToDbType();
            Parameter.Direction = Direction;
            Parameter.Size = Length;
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Direction">Direction that the parameter goes (in or out)</param>
        /// <param name="Command">Command object</param>
        /// <param name="Type">SQL type of the parameter</param>
        public static void AddParameter(this DbCommand Command, string ID, SqlDbType Type,
            object Value = null, ParameterDirection Direction = ParameterDirection.Input)
        {
            Command.ThrowIfNull("Command");
            ID.ThrowIfNullOrEmpty("ID");
            Command.AddParameter(ID, Type.ToDbType(), Value, Direction);
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <typeparam name="DataType">Data type of the parameter</typeparam>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Direction">Direction that the parameter goes (in or out)</param>
        /// <param name="Command">Command object</param>
        /// <param name="Value">Value to add</param>
        public static void AddParameter<DataType>(this DbCommand Command, string ID, DataType Value = default(DataType),
            ParameterDirection Direction = ParameterDirection.Input)
        {
            Command.ThrowIfNull("Command");
            ID.ThrowIfNullOrEmpty("ID");
            Command.AddParameter(ID,
                new GenericEqualityComparer<DataType>().Equals(Value, default(DataType)) ? typeof(DataType).ToDbType() : Value.GetType().ToDbType(),
                Value, Direction);
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Direction">Direction that the parameter goes (in or out)</param>
        /// <param name="Command">Command object</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        public static void AddParameter(this DbCommand Command, string ID, DbType Type, object Value = null,
            ParameterDirection Direction = ParameterDirection.Input)
        {
            Command.ThrowIfNull("Command");
            ID.ThrowIfNullOrEmpty("ID");
            Type.ThrowIfNull("Type");
            DbParameter Parameter = null;
            if (Command.Parameters.Contains(ID))
                Parameter = Command.Parameters[ID];
            else
            {
                Parameter = Command.CreateParameter();
                Command.Parameters.Add(Parameter);
            }
            Parameter.ParameterName = ID;
            Parameter.Value = (Value == null) ? System.DBNull.Value : Value;
            Parameter.IsNullable = (Value == null);
            Parameter.DbType = Type;
            Parameter.Direction = Direction;
        }

        #endregion

        #region BeginTransaction

        /// <summary>
        /// Begins a transaction
        /// </summary>
        /// <param name="Command">Command object</param>
        public static DbTransaction BeginTransaction(this DbCommand Command)
        {
            Command.Open();
            return Command.Connection.BeginTransaction();
        }

        #endregion

        #region ClearParameters

        /// <summary>
        /// Clears the parameters
        /// </summary>
        /// <param name="Command">Command object</param>
        public static void ClearParameters(this DbCommand Command)
        {
            if (Command.IsNotNull())
                Command.Parameters.Clear();
        }

        #endregion

        #region Close

        /// <summary>
        /// Closes the connection
        /// </summary>
        /// <param name="Command">Command object</param>
        public static void Close(this DbCommand Command)
        {
            if (Command.IsNotNull()
                && Command.Connection.IsNotNull()
                && Command.Connection.State != ConnectionState.Closed)
                Command.Connection.Close();
        }

        #endregion

        #region Commit

        /// <summary>
        /// Commits a transaction
        /// </summary>
        /// <param name="Command">Command object</param>
        public static void Commit(this DbCommand Command)
        {
            if (Command.IsNotNull() && Command.Transaction.IsNotNull())
                Command.Transaction.Commit();
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// Executes the query and returns a data set
        /// </summary>
        /// <param name="Command">Command object</param>
        /// <param name="Factory">DbProviderFactory being used</param>
        /// <returns>A dataset filled with the results of the query</returns>
        public static DataSet ExecuteDataSet(this DbCommand Command, DbProviderFactory Factory)
        {
            if (Command.IsNull())
                return null;
            Command.Open();
            using (DbDataAdapter Adapter = Factory.CreateDataAdapter())
            {
                Adapter.SelectCommand = Command;
                DataSet ReturnSet = new DataSet();
                Adapter.Fill(ReturnSet);
                return ReturnSet;
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes the stored procedure as a scalar query
        /// </summary>
        /// <param name="Command">Command object</param>
        /// <returns>The object of the first row and first column</returns>
        public static DataType ExecuteScalar<DataType>(this DbCommand Command)
        {
            Command.Open();
            object Value = Command.IsNotNull() ? Command.ExecuteScalar() : default(DataType);
            return Value.TryTo<object, DataType>();
        }

        #endregion

        #region GetOutputParameter

        /// <summary>
        /// Returns an output parameter's value
        /// </summary>
        /// <typeparam name="DataType">Data type of the object</typeparam>
        /// <param name="ID">Parameter name</param>
        /// <param name="Command">Command object</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public static DataType GetOutputParameter<DataType>(this DbCommand Command, string ID, DataType Default)
        {
            return Command.IsNotNull() && Command.Parameters[ID].IsNotNullOrDBNull() ?
                Command.Parameters[ID].Value.TryTo<object, DataType>() : Default;
        }

        #endregion

        #region Open

        /// <summary>
        /// Opens the connection
        /// </summary>
        /// <param name="Command">Command object</param>
        public static void Open(this DbCommand Command)
        {
            if (Command.IsNotNull()
                && Command.Connection.IsNotNull()
                && Command.Connection.State != ConnectionState.Open)
                Command.Connection.Open();
        }

        #endregion

        #region Rollback

        /// <summary>
        /// Rolls back a transaction
        /// </summary>
        /// <param name="Command">Command object</param>
        public static void Rollback(this DbCommand Command)
        {
            if (Command.IsNotNull() && Command.Transaction.IsNotNull())
                Command.Transaction.Rollback();
        }

        #endregion

        #endregion
    }
}