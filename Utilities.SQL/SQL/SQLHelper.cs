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
using System.Data.Common;
using System.Data.SqlClient;
using System.Xml;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.DataTypes.Patterns;
using Utilities.SQL.ExtensionMethods;
using Utilities.SQL.Interfaces;
using Utilities.SQL.MicroORM;
using Utilities.SQL.MicroORM.Interfaces;
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
        /// <param name="ConnectionUsing">The connection string to use</param>
        /// <param name="CommandType">The command type of the command sent in</param>
        /// <param name="DbType">Database type, based on ADO.Net provider name</param>
        /// <param name="Profile">Determines if the commands should be profiled</param>
        public SQLHelper(string Command, string ConnectionUsing, CommandType CommandType, string DbType = "System.Data.SqlClient", bool Profile = false)
        {
            Setup(Command, ConnectionUsing, CommandType, DbType, Profile, new List<object>());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Command">Command to use</param>
        /// <param name="ConnectionUsing">The connection string to use</param>
        /// <param name="DbType">Database type, based on ADO.Net provider name</param>
        /// <param name="Profile">Determines if the calls should be profiled</param>
        public SQLHelper(Command Command, string ConnectionUsing, string DbType = "System.Data.SqlClient", bool Profile = false)
        {
            Setup(Command.SQLCommand, ConnectionUsing, Command.CommandType, DbType, Profile, Command.Parameters);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Database">Database to use</param>
        /// <param name="DbType">Database type, based on ADO.Net provider name</param>
        /// <param name="Profile">Determines if the calls should be profiled</param>
        public SQLHelper(string Database="Default", string DbType = "System.Data.SqlClient", bool Profile = false)
        {
            Setup("", Database, System.Data.CommandType.Text, DbType, Profile, new List<object>());
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
        protected virtual DbCommand ExecutableCommand { get; set; }

        /// <summary>
        /// The data reader for the query
        /// </summary>
        protected virtual DbDataReader Reader { get; set; }

        /// <summary>
        /// The transaction associated with the query
        /// </summary>
        protected virtual DbTransaction Transaction { get; set; }

        /// <summary>
        /// Determines if the calls should be profiled or not
        /// </summary>
        protected virtual bool Profile { get; set; }

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

        /// <summary>
        /// Command text
        /// </summary>
        private string _Command = null;

        /// <summary>
        /// Parameters that are being used in the command
        /// </summary>
        protected virtual List<object> Parameters { get; set; }

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

        /// <summary>
        /// List of database connections
        /// </summary>
        protected static Dictionary<string, Database> Databases = new Dictionary<string, Database>();

        /// <summary>
        /// Mappings using
        /// </summary>
        protected virtual List<IMapping> MappingsUsing { get; set; }

        /// <summary>
        /// Database using
        /// </summary>
        protected virtual string DatabaseUsing { get; set; }

        #endregion

        #region Functions

        #region AddParameter

        /// <summary>
        /// Adds a parameter to the call (for strings only)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        /// <returns>This</returns>
        public virtual SQLHelper AddParameter(string ID, string Value = "", ParameterDirection Direction = ParameterDirection.Input)
        {
            Parameters.Add(Value);
            if (ExecutableCommand != null)
                ExecutableCommand.AddParameter(ID, Value, Direction);
            return this;
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        /// <returns>This</returns>
        public virtual SQLHelper AddParameter(string ID, SqlDbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input)
        {
            Parameters.Add(Value);
            return AddParameter(ID, Type.ToDbType(), Value, Direction);
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <typeparam name="DataType">Data type of the parameter</typeparam>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        /// <returns>This</returns>
        public virtual SQLHelper AddParameter<DataType>(string ID, DataType Value = default(DataType), ParameterDirection Direction = ParameterDirection.Input)
        {
            Parameters.Add(Value);
            return AddParameter(ID, Value.GetType().ToDbType(), Value, Direction);
        }

        /// <summary>
        /// Adds a parameter to the call (for all types other than strings)
        /// </summary>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="Value">Value to add</param>
        /// <param name="Type">SQL type of the parameter</param>
        /// <param name="Direction">Parameter direction (defaults to input)</param>
        /// <returns>This</returns>
        public virtual SQLHelper AddParameter(string ID, DbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input)
        {
            Parameters.Add(Value);
            if (ExecutableCommand != null)
                ExecutableCommand.AddParameter(ID, Type, Value, Direction);
            return this;
        }

        /// <summary>
        /// Adds parameters to the call
        /// </summary>
        /// <param name="Parameters">Parameters to add</param>
        /// <returns>This</returns>
        public virtual SQLHelper AddParameter(params IParameter[] Parameters)
        {
            Parameters.ThrowIfNull("Parameters");
            Parameters.ForEach(x => x.AddParameter(this));
            return this;
        }

        /// <summary>
        /// Adds parameters to the call
        /// </summary>
        /// <param name="Parameters">Parameters to add</param>
        /// <returns>This</returns>
        public virtual SQLHelper AddParameter(params object[] Parameters)
        {
            this.Parameters.Add(Parameters);
            for (int x = 0; x < Parameters.Length; ++x)
            {
                if (Parameters[x] == null)
                    AddParameter(this.ExecutableCommand.Parameters.Count.ToString(), default(DbType), null, ParameterDirection.Input);
                else if (Parameters[x] is string)
                    AddParameter(this.ExecutableCommand.Parameters.Count.ToString(), (string)Parameters[x], ParameterDirection.Input);
                else
                    AddParameter(this.ExecutableCommand.Parameters.Count.ToString(), Parameters[x].GetType().ToDbType(), Parameters[x], ParameterDirection.Input);
            }
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

        #region ChangeCommand

        /// <summary>
        /// Changes the command using the Command class
        /// </summary>
        /// <param name="Command">Command to use</param>
        /// <returns>This</returns>
        public virtual SQLHelper ChangeCommand(Command Command)
        {
            this.Command = Command.SQLCommand;
            this.CommandType = Command.CommandType;
            this.Parameters.Clear();
            AddParameter(Command.Parameters.ToArray());
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

        #region ExecuteBulkCopy

        /// <summary>
        /// Does a bulk copy of the data (only usable on SQL Server)
        /// </summary>
        /// <param name="Data">Data to copy over</param>
        /// <param name="DestinationTable">Table to copy the data to</param>
        /// <param name="Options">Options used during the copy</param>
        /// <returns>Returns this SQLHelper object</returns>
        public virtual SQLHelper ExecuteBulkCopy<T>(IEnumerable<T> Data, string DestinationTable, SqlBulkCopyOptions Options = SqlBulkCopyOptions.Default)
        {
            Open();
            if (Profile)
            {
                using (new Profiler.SQLProfiler("ExecuteBulkCopy", Command, Parameters.ToArray()))
                {
                    return ExecuteBulkCopy(Data.ToDataTable(), DestinationTable, Options);
                }
            }
            return ExecuteBulkCopy(Data.ToDataTable(), DestinationTable, Options);
        }

        /// <summary>
        /// Does a bulk copy of the data (only usable on SQL Server)
        /// </summary>
        /// <param name="Data">Data to copy over</param>
        /// <param name="DestinationTable">Table to copy the data to</param>
        /// <param name="Options">Options used during the copy</param>
        /// <returns>Returns this SQLHelper object</returns>
        public virtual SQLHelper ExecuteBulkCopy(DataTable Data, string DestinationTable, SqlBulkCopyOptions Options = SqlBulkCopyOptions.Default)
        {
            Open();
            if (Profile)
            {
                using (new Profiler.SQLProfiler("ExecuteBulkCopy", Command, Parameters.ToArray()))
                {
                    using (SqlBulkCopy Copier = new SqlBulkCopy(Connection.ConnectionString, Options))
                    {
                        foreach (DataColumn Column in Data.Columns)
                            Copier.ColumnMappings.Add(Column.ColumnName, Column.ColumnName);
                        Copier.DestinationTableName = DestinationTable;
                        Copier.WriteToServer(Data);
                    }
                    return this;
                }
            }
            using (SqlBulkCopy Copier = new SqlBulkCopy(Connection.ConnectionString, Options))
            {
                foreach (DataColumn Column in Data.Columns)
                    Copier.ColumnMappings.Add(Column.ColumnName, Column.ColumnName);
                Copier.DestinationTableName = DestinationTable;
                Copier.WriteToServer(Data);
            }
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
            Open();
            if (Profile)
            {
                using (new Profiler.SQLProfiler("ExecuteDataSet", Command, Parameters.ToArray()))
                {
                    return ExecutableCommand.ExecuteDataSet(Factory);
                }
            }
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
            if (Profile)
            {
                using (new Profiler.SQLProfiler("ExecuteNonQuery", Command, Parameters.ToArray()))
                {
                    return (ExecutableCommand != null) ? ExecutableCommand.ExecuteNonQuery() : 0;
                }
            }
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
            if (Profile)
            {
                using (new Profiler.SQLProfiler("ExecuteReader", Command, Parameters.ToArray()))
                {
                    if (ExecutableCommand != null)
                        Reader = ExecutableCommand.ExecuteReader();
                    return this;
                }
            }
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
            Open();
            if (Profile)
            {
                using (new Profiler.SQLProfiler("ExecuteScalar", Command, Parameters.ToArray()))
                {
                    return ExecutableCommand.ExecuteScalar<DataType>();
                }
            }
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
            if (Profile)
            {
                using (new Profiler.SQLProfiler("ExecuteXmlReader", Command, Parameters.ToArray()))
                {
                    if (ExecutableCommand != null && ExecutableCommand is SqlCommand)
                        return ((SqlCommand)ExecutableCommand).ExecuteXmlReader();
                    return null;
                }
            }
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

        #region Map

        /// <summary>
        /// Returns a specific mapping
        /// </summary>
        /// <typeparam name="ClassType">Class type to get</typeparam>
        /// <returns>The mapping specified</returns>
        public Mapping<ClassType> Map<ClassType>() where ClassType : class,new()
        {
            if (!Databases[DatabaseUsing].Mappings.ContainsKey(typeof(ClassType)))
                throw new ArgumentOutOfRangeException(typeof(ClassType).Name + " not found");
            return new Mapping<ClassType>((Mapping<ClassType>)Databases[DatabaseUsing].Mappings[typeof(ClassType)], this);
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
            Parameters = new List<object>();
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

        #region Setup

        /// <summary>
        /// Sets up the info
        /// </summary>
        /// <param name="Command">Stored procedure/SQL Text to use</param>
        /// <param name="ConnectionUsing">The connection string to use</param>
        /// <param name="CommandType">The command type of the command sent in</param>
        /// <param name="DbType">Database type, based on ADO.Net provider name</param>
        /// <param name="Profile">Determines if the commands should be profiled</param>
        /// <param name="Parameters">Parameters used in setting up the application</param>
        private void Setup(string Command, string ConnectionUsing, CommandType CommandType, string DbType, bool Profile, List<object> Parameters)
        {
            this.Parameters = new List<object>();
            this.MappingsUsing = new List<IMapping>();
            if (Databases.ContainsKey(ConnectionUsing))
            {
                DatabaseUsing = ConnectionUsing;
                ConnectionUsing = Databases[ConnectionUsing].Connection;
            }
            else
            {
                Database(ConnectionUsing);
                DatabaseUsing = "Default";
            }
            this.Profile = Profile;
            Factory = DbProviderFactories.GetFactory(DbType);
            Connection = Factory.CreateConnection();
            Connection.ConnectionString = ConnectionUsing;
            _Command = Command;
            _CommandType = CommandType;
            ExecutableCommand = Factory.CreateCommand();
            ExecutableCommand.CommandText = _Command;
            ExecutableCommand.Connection = Connection;
            ExecutableCommand.CommandType = CommandType;
            AddParameter(Parameters.ToArray());
        }

        #endregion

        #endregion

        #region Static Functions

        #region ClearAllMappings

        /// <summary>
        /// Clears all database objects of all mappings
        /// </summary>
        public static void ClearAllMappings()
        {
            foreach (string Database in Databases.Keys)
                ClearMappings(Database);
        }

        #endregion

        #region ClearMappings

        /// <summary>
        /// Clears a database object of all mappings
        /// </summary>
        /// <param name="Database">Database object to clear</param>
        public static void ClearMappings(string Database = "Default")
        {
            if (Databases.ContainsKey(Database))
            {
                foreach (Type Key in Databases[Database].Mappings.Keys)
                    Databases[Database].Mappings[Key].Dispose();
                Databases[Database].Mappings.Clear();
            }
        }

        #endregion

        #region Database

        /// <summary>
        /// Adds a database's info
        /// </summary>
        /// <param name="ConnectionString">Connection string to use for this database</param>
        /// <param name="Name">Name to associate with the database</param>
        public static void Database(string ConnectionString, string Name = "Default")
        {
            if (Databases.ContainsKey(Name))
                Databases[Name].Connection = ConnectionString;
            else
                Databases.Add(Name, new Database(ConnectionString, Name));
        }

        #endregion

        #region Map

        /// <summary>
        /// Creates a mapping
        /// </summary>
        /// <typeparam name="ClassType">Class type to map</typeparam>
        /// <param name="TableName">Table name</param>
        /// <param name="PrimaryKey">Primary key</param>
        /// <param name="AutoIncrement">Auto incrementing primar key</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        /// <param name="Database">Database to use</param>
        /// <returns>The created mapping (or an already created one if it exists</returns>
        public static Mapping<ClassType> Map<ClassType>(string TableName, string PrimaryKey, bool AutoIncrement = true, string ParameterStarter = "@", string Database = "Default") where ClassType : class,new()
        {
            if (!Databases.ContainsKey(Database))
                Databases.Add(Database, new Database("", Database));
            if (!Databases[Database].Mappings.ContainsKey(typeof(ClassType)))
                Databases[Database].Mappings.Add(typeof(ClassType), new Mapping<ClassType>(TableName, PrimaryKey, AutoIncrement, ParameterStarter));
            return (Mapping<ClassType>)Databases[Database].Mappings[typeof(ClassType)];
        }

        #endregion

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Closes the connection (if open) and disposes of the objects.
        /// </summary>
        public virtual void Dispose()
        {
            Close();
            foreach (IMapping Mapping in MappingsUsing)
            {
                Mapping.Dispose();
            }
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