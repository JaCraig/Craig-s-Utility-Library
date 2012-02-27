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
using Utilities.SQL.ExtensionMethods;
using Utilities.SQL.Interfaces;
using Utilities.DataTypes.Patterns;
#endregion

namespace Utilities.SQL
{
    /// <summary>
    /// SQL Helper class
    /// </summary>
    public class SQLHelper : IDisposable, IFluentInterface
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Command">Stored procedure/SQL Text to use</param>
        /// <param name="ConnectionUsing">The connection string to user</param>
        /// <param name="CommandType">The command type of the command sent in</param>
        /// <param name="DbType">Database type, based on ADO.Net provider name</param>
        public SQLHelper(string Command, string ConnectionUsing, CommandType CommandType, string DbType = "System.Data.SqlClient")
        {
            Factory = DbProviderFactories.GetFactory(DbType);
            Connection = Factory.CreateConnection();
            Connection.ConnectionString = ConnectionUsing;
            _Command = Command;
            _CommandType = CommandType;
            ExecutableCommand = Factory.CreateCommand();
            ExecutableCommand.CommandText = _Command;
            ExecutableCommand.Connection = Connection;
            ExecutableCommand.CommandType = CommandType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Db provider factory (creates connections, etc.)
        /// </summary>
        protected virtual DbProviderFactory Factory { get; set; }

        /// <summary>
        /// Connection to the database
        /// </summary>
        protected virtual DbConnection Connection { get; set; }

        /// <summary>
        /// The executable command
        /// </summary>
        protected DbCommand ExecutableCommand { get; set; }

        /// <summary>
        /// The data reader for the query
        /// </summary>
        protected DbDataReader Reader { get; set; }

        /// <summary>
        /// The transaction associated with the query
        /// </summary>
        protected DbTransaction Transaction { get; set; }

        /// <summary>
        /// Stored procedure's name or SQL Text
        /// </summary>
        public virtual string Command
        {
            get { return _Command; }
            set
            {
                _Command = value;
                RecreateConnection();
            }
        }

        private string _Command = null;


        /// <summary>
        /// Command Type
        /// </summary>
        public virtual CommandType CommandType
        {
            get { return _CommandType; }
            set
            {
                _CommandType = value;
                RecreateConnection();
            }
        }
        private CommandType _CommandType;

        #endregion

        #region Functions

        #region AddParameter

        /// <summary>
        /// Adds a parameter to the call (for strings only)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Length">Size of the string(either -1 or greater than 4000 should be used to indicate nvarchar(max))</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        public virtual SQLHelper AddParameter(string ID, int Length, string Value = "", ParameterDirection Direction = ParameterDirection.Input)
        {
            if (ExecutableCommand != null)
                ExecutableCommand.AddParameter(ID, Length, Value, Direction);
            return this;
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        public virtual SQLHelper AddParameter(string ID, SqlDbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input)
        {
            return AddParameter(ID, Type.ToDbType(), Value, Direction);
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <typeparam name="DataType">Data type of the parameter</typeparam>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        public virtual SQLHelper AddParameter<DataType>(string ID, DataType Value = default(DataType), ParameterDirection Direction = ParameterDirection.Input)
        {
            return AddParameter(ID, Value.GetType().ToDbType(), Value, Direction);
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        public virtual SQLHelper AddParameter(string ID, DbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input)
        {
            if (ExecutableCommand != null)
                ExecutableCommand.AddParameter(ID, Type, Value, Direction);
            return this;
        }

        /// <summary>
        /// Adds parameters to the call
        /// </summary>
        /// <param name="Parameters">Parameters to add</param>
        public virtual SQLHelper AddParameter(params IParameter[] Parameters)
        {
            Parameters.ThrowIfNull("Parameters");
            Parameters.ForEach(x => x.AddParameter(this));
            return this;
        }

        #endregion

        #region BeginTransaction

        /// <summary>
        /// Begins a transaction
        /// </summary>
        public virtual SQLHelper BeginTransaction()
        {
            Transaction = ExecutableCommand.BeginTransaction();
            Command = _Command;
            return this;
        }

        #endregion

        #region ClearParameters

        /// <summary>
        /// Clears the parameters
        /// </summary>
        public virtual SQLHelper ClearParameters()
        {
            ExecutableCommand.ClearParameters();
            return this;
        }

        #endregion

        #region Close

        /// <summary>
        /// Closes the connection
        /// </summary>
        public virtual SQLHelper Close()
        {
            ExecutableCommand.Close();
            return this;
        }

        #endregion

        #region Commit

        /// <summary>
        /// Commits a transaction
        /// </summary>
        public virtual SQLHelper Commit()
        {
            ExecutableCommand.Commit();
            return this;
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// Executes the query and returns a data set
        /// </summary>
        /// <returns>A dataset filled with the results of the query</returns>
        public virtual DataSet ExecuteDataSet()
        {
            return ExecutableCommand.ExecuteDataSet(Factory);
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// Executes the stored procedure as a non query
        /// </summary>
        /// <returns>Number of rows effected</returns>
        public virtual int ExecuteNonQuery()
        {
            Open();
            return (ExecutableCommand != null) ? ExecutableCommand.ExecuteNonQuery() : 0;
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// Executes the stored procedure and returns a reader object
        /// </summary>
        public virtual SQLHelper ExecuteReader()
        {
            Open();
            if (ExecutableCommand != null)
                Reader = ExecutableCommand.ExecuteReader();
            return this;
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes the stored procedure as a scalar query
        /// </summary>
        /// <typeparam name="DataType">Data type to return</typeparam>
        /// <returns>The object of the first row and first column</returns>
        public virtual DataType ExecuteScalar<DataType>()
        {
            return ExecutableCommand.ExecuteScalar<DataType>();
        }

        #endregion

        #region ExecuteXmlReader

        /// <summary>
        /// Executes the query and returns an XmlReader
        /// </summary>
        /// <returns>The XmlReader filled with the data from the query</returns>
        public virtual XmlReader ExecuteXmlReader()
        {
            Open();
            if (ExecutableCommand != null && ExecutableCommand is SqlCommand)
                return ((SqlCommand)ExecutableCommand).ExecuteXmlReader();
            return null;
        }

        #endregion

        #region GetParameter

        /// <summary>
        /// Returns a parameter's value
        /// </summary>
        /// <typeparam name="DataType">Data type of the object</typeparam>
        /// <param name="ID">Parameter name</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public virtual DataType GetParameter<DataType>(string ID, DataType Default = default(DataType), ParameterDirection Direction = ParameterDirection.Input)
        {
            if (Direction == ParameterDirection.Output)
                return ExecutableCommand.GetOutputParameter<DataType>(ID, Default);
            return Reader.GetParameter<DataType>(ID, Default);
        }

        /// <summary>
        /// Returns a parameter's value
        /// </summary>
        /// <typeparam name="DataType">Data type of the object</typeparam>
        /// <param name="Position">Position in the row</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public virtual DataType GetParameter<DataType>(int Position, DataType Default = default(DataType))
        {
            return Reader.GetParameter<DataType>(Position, Default);
        }

        #endregion

        #region NextResult

        /// <summary>
        /// Goes to the next result set (used if multiple queries are sent in)
        /// </summary>
        public virtual SQLHelper NextResult()
        {
            if (Reader != null)
                Reader.NextResult();
            return this;
        }

        #endregion

        #region Open

        /// <summary>
        /// Opens the connection
        /// </summary>
        public virtual SQLHelper Open()
        {
            ExecutableCommand.Open();
            return this;
        }

        #endregion

        #region Read

        /// <summary>
        /// Is there more information?
        /// </summary>
        /// <returns>True if there is more rows, false otherwise</returns>
        public virtual bool Read()
        {
            return (Reader.IsNotNull()) ? Reader.Read() : false;
        }

        #endregion

        #region RecreateConnection

        private void RecreateConnection()
        {
            if (Reader != null)
            {
                Reader.Close();
                Reader.Dispose();
                Reader = null;
            }
            if (ExecutableCommand != null)
            {
                ExecutableCommand.Dispose();
                ExecutableCommand = null;
            }
            ExecutableCommand = Factory.CreateCommand();
            ExecutableCommand.CommandText = _Command;
            ExecutableCommand.Connection = Connection;
            ExecutableCommand.CommandType = CommandType;
            if (Transaction != null)
                ExecutableCommand.Transaction = Transaction;
        }

        #endregion

        #region Rollback

        /// <summary>
        /// Rolls back a transaction
        /// </summary>
        public virtual SQLHelper Rollback()
        {
            ExecutableCommand.Rollback();
            return this;
        }

        #endregion

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            Close();
            if (Connection != null)
            {
                Connection.Dispose();
                Connection = null;
            }
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }
            if (ExecutableCommand != null)
            {
                ExecutableCommand.Dispose();
                ExecutableCommand = null;
            }
            if (Reader != null)
            {
                Reader.Dispose();
                Reader = null;
            }
        }

        #endregion
    }
}