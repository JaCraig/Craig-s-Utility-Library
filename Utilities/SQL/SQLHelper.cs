/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
#endregion

namespace Utilities.SQL
{
    /// <summary>
    /// SQL Helper class
    /// </summary>
    public class SQLHelper : IDisposable
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Command">Stored procedure/SQL Text to use</param>
        /// <param name="ConnectionUsing">The connection string to user</param>
        /// <param name="CommandType">The command type of the command sent in</param>
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

        #region AddOutputParameter

        /// <summary>
        /// Adds an output parameter
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Type">SQL type of the parameter</param>
        public virtual void AddOutputParameter(string ID, SqlDbType Type)
        {
            AddOutputParameter(ID, Utilities.DataTypes.DataTypeConversion.SqlDbTypeToDbType(Type));
        }

        /// <summary>
        /// Adds an output parameter
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Type">SQL type of the parameter</param>
        public virtual void AddOutputParameter(string ID, DbType Type)
        {
            if (ExecutableCommand != null)
            {
                DbParameter Parameter = null;
                if (ExecutableCommand.Parameters.Contains(ID))
                    Parameter = ExecutableCommand.Parameters[ID];
                else
                {
                    Parameter = ExecutableCommand.CreateParameter();
                    ExecutableCommand.Parameters.Add(Parameter);
                }
                Parameter.ParameterName = ID;
                Parameter.Value = null;
                Parameter.DbType = Type;
                Parameter.Direction = ParameterDirection.Output;
            }
        }

        /// <summary>
        /// Adds an output parameter
        /// </summary>
        /// <typeparam name="DataType">Data type of the parameter</typeparam>
        /// <param name="ID">ID associated with the output parameter</param>
        public virtual void AddOutputParameter<DataType>(string ID)
        {
            AddOutputParameter(ID, Utilities.DataTypes.DataTypeConversion.NetTypeToDbType(typeof(DataType)));
        }

        /// <summary>
        /// Adds an output parameter
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Length">Length of the string (either -1 or 5000 should be used to indicate nvarchar(max))</param>
        public virtual void AddOutputParameter(string ID, int Length)
        {
            if (Length == 5000)
                Length = -1;
            if (ExecutableCommand != null)
            {
                DbParameter Parameter = null;
                if (ExecutableCommand.Parameters.Contains(ID))
                    Parameter = ExecutableCommand.Parameters[ID];
                else
                {
                    Parameter = ExecutableCommand.CreateParameter();
                    ExecutableCommand.Parameters.Add(Parameter);
                }
                Parameter.ParameterName = ID;
                Parameter.Value = null;
                Parameter.DbType = Utilities.DataTypes.DataTypeConversion.NetTypeToDbType(typeof(string));
                Parameter.Direction = ParameterDirection.Output;
                Parameter.Size = Length;
            }
        }

        #endregion

        #region AddParameter

        /// <summary>
        /// Adds a parameter to the call (for strings only)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Length">Size of the string(either -1 or 5000 should be used to indicate nvarchar(max))</param>
        public virtual void AddParameter(string ID, string Value, int Length)
        {
            if (Length == 5000)
                Length = -1;
            if (ExecutableCommand != null)
            {
                DbParameter Parameter = null;
                if (ExecutableCommand.Parameters.Contains(ID))
                    Parameter = ExecutableCommand.Parameters[ID];
                else
                {
                    Parameter = ExecutableCommand.CreateParameter();
                    ExecutableCommand.Parameters.Add(Parameter);
                }
                Parameter.ParameterName = ID;
                Parameter.Value = (string.IsNullOrEmpty(Value)) ? System.DBNull.Value : (object)Value;
                Parameter.IsNullable = (string.IsNullOrEmpty(Value));
                Parameter.DbType = Utilities.DataTypes.DataTypeConversion.NetTypeToDbType(typeof(string));
                Parameter.Direction = ParameterDirection.Input;
                Parameter.Size = Length;
            }
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        public virtual void AddParameter(string ID, object Value, SqlDbType Type)
        {
            AddParameter(ID, Value, Utilities.DataTypes.DataTypeConversion.SqlDbTypeToDbType(Type));
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <typeparam name="DataType">Data type of the parameter</typeparam>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        public virtual void AddParameter<DataType>(string ID, DataType Value)
        {
            AddParameter(ID, Value, Utilities.DataTypes.DataTypeConversion.NetTypeToDbType(Value.GetType()));
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        public virtual void AddParameter(string ID, object Value, DbType Type)
        {
            if (ExecutableCommand != null)
            {
                DbParameter Parameter = null;
                if (ExecutableCommand.Parameters.Contains(ID))
                    Parameter = ExecutableCommand.Parameters[ID];
                else
                {
                    Parameter = ExecutableCommand.CreateParameter();
                    ExecutableCommand.Parameters.Add(Parameter);
                }
                Parameter.ParameterName = ID;
                Parameter.Value = (Value == null) ? System.DBNull.Value : Value;
                Parameter.IsNullable = (Value == null);
                Parameter.DbType = Type;
                Parameter.Direction = ParameterDirection.Input;
            }
        }

        #endregion

        #region BeginTransaction

        /// <summary>
        /// Begins a transaction
        /// </summary>
        public virtual void BeginTransaction()
        {
            Open();
            Transaction = Connection.BeginTransaction();
            Command = _Command;
        }

        #endregion

        #region ClearParameters

        /// <summary>
        /// Clears the parameters
        /// </summary>
        public virtual void ClearParameters()
        {
            if (ExecutableCommand != null)
                ExecutableCommand.Parameters.Clear();
        }

        #endregion

        #region Close

        /// <summary>
        /// Closes the connection
        /// </summary>
        public virtual void Close()
        {
            if (ExecutableCommand != null
                && ExecutableCommand.Connection != null
                && ExecutableCommand.Connection.State != ConnectionState.Closed)
                ExecutableCommand.Connection.Close();
        }

        #endregion

        #region Commit

        /// <summary>
        /// Commits a transaction
        /// </summary>
        public virtual void Commit()
        {
            if (Transaction != null)
                Transaction.Commit();
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// Executes the query and returns a data set
        /// </summary>
        /// <returns>A dataset filled with the results of the query</returns>
        public virtual DataSet ExecuteDataSet()
        {
            Open();
            if (ExecutableCommand != null)
            {
                DbDataAdapter Adapter = Factory.CreateDataAdapter();
                Adapter.SelectCommand = ExecutableCommand;
                DataSet ReturnSet = new DataSet();
                Adapter.Fill(ReturnSet);
                return ReturnSet;
            }
            return null;
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
            if (ExecutableCommand != null)
                return ExecutableCommand.ExecuteNonQuery();
            return 0;
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// Executes the stored procedure and returns a reader object
        /// </summary>
        public virtual void ExecuteReader()
        {
            Open();
            if (ExecutableCommand != null)
                Reader = ExecutableCommand.ExecuteReader();
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes the stored procedure as a scalar query
        /// </summary>
        /// <returns>The object of the first row and first column</returns>
        public virtual object ExecuteScalar()
        {
            Open();
            if (ExecutableCommand != null)
            {
                object Value = ExecutableCommand.ExecuteScalar();
                if (!Convert.IsDBNull(Value))
                    return Value;
            }
            return null;
        }

        /// <summary>
        /// Executes the stored procedure as a scalar query
        /// </summary>
        /// <returns>The object of the first row and first column</returns>
        public virtual DataType ExecuteScalar<DataType>()
        {
            Open();
            if (ExecutableCommand != null)
            {
                object Value = ExecutableCommand.ExecuteScalar();
                if (!Convert.IsDBNull(Value))
                    return (DataType)Convert.ChangeType(Value, typeof(DataType));
            }
            return default(DataType);
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
        /// <param name="ID">Parameter name</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public virtual object GetParameter(string ID, object Default)
        {
            if (Reader == null)
                return Default;
            bool Found = false;
            for (int x = 0; x < Reader.FieldCount; ++x)
            {
                if (Reader.GetName(x) == ID)
                {
                    Found = true;
                    break;
                }
            }
            if (Found && !Convert.IsDBNull(Reader[ID]))
                return Reader[ID];
            return Default;
        }

        /// <summary>
        /// Returns a parameter's value
        /// </summary>
        /// <typeparam name="DataType">Data type of the object</typeparam>
        /// <param name="ID">Parameter name</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public virtual DataType GetParameter<DataType>(string ID, DataType Default)
        {
            if (Reader == null)
                return Default;
            bool Found = false;
            for (int x = 0; x < Reader.FieldCount; ++x)
            {
                if (Reader.GetName(x) == ID)
                {
                    Found = true;
                    break;
                }
            }
            if (Found && !Convert.IsDBNull(Reader[ID]))
                return (DataType)Convert.ChangeType(Reader[ID], typeof(DataType));
            return Default;
        }

        /// <summary>
        /// Returns a parameter's value
        /// </summary>
        /// <param name="Position">Position in the row</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public virtual object GetParameter(int Position, object Default)
        {
            if (Reader != null && !Convert.IsDBNull(Reader[Position]))
                return Reader[Position];
            return Default;
        }

        /// <summary>
        /// Returns a parameter's value
        /// </summary>
        /// <typeparam name="DataType">Data type of the object</typeparam>
        /// <param name="Position">Position in the row</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public virtual DataType GetParameter<DataType>(int Position, DataType Default)
        {
            if (Reader != null && !Convert.IsDBNull(Reader[Position]))
                return (DataType)Convert.ChangeType(Reader[Position], typeof(DataType));
            return Default;
        }

        #endregion

        #region GetOutputParameter

        /// <summary>
        /// Returns an output parameter's value
        /// </summary>
        /// <param name="ID">Parameter name</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public virtual object GetOutputParameter(string ID, object Default)
        {
            if (ExecutableCommand != null && !Convert.IsDBNull(ExecutableCommand.Parameters[ID]))
                return ExecutableCommand.Parameters[ID].Value;
            return Default;
        }

        /// <summary>
        /// Returns an output parameter's value
        /// </summary>
        /// <typeparam name="DataType">Data type of the object</typeparam>
        /// <param name="ID">Parameter name</param>
        /// <param name="Default">Default value for the parameter</param>
        /// <returns>if the parameter exists (and isn't null or empty), it returns the parameter's value. Otherwise the default value is returned.</returns>
        public virtual DataType GetOutputParameter<DataType>(string ID, DataType Default)
        {
            if (ExecutableCommand != null && !Convert.IsDBNull(ExecutableCommand.Parameters[ID]))
                return (DataType)Convert.ChangeType(ExecutableCommand.Parameters[ID].Value, typeof(DataType));
            return Default;
        }

        #endregion

        #region NextResult

        /// <summary>
        /// Goes to the next result set (used if multiple queries are sent in)
        /// </summary>
        public virtual void NextResult()
        {
            if (Reader != null)
                Reader.NextResult();
        }

        #endregion

        #region Open

        /// <summary>
        /// Opens the connection
        /// </summary>
        public virtual void Open()
        {
            if (ExecutableCommand != null
                && ExecutableCommand.Connection != null
                && ExecutableCommand.Connection.State != ConnectionState.Open)
                ExecutableCommand.Connection.Open();
        }

        #endregion

        #region Read

        /// <summary>
        /// Is there more information?
        /// </summary>
        /// <returns>True if there is more rows, false otherwise</returns>
        public virtual bool Read()
        {
            return (Reader != null) ? Reader.Read() : false;
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
        public virtual void Rollback()
        {
            if (Transaction != null)
                Transaction.Rollback();
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
