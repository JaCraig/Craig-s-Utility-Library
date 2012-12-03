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
using System.Linq;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.DataTypes.Patterns;
using Utilities.SQL.ExtensionMethods;
using Utilities.SQL.Interfaces;
using Utilities.SQL.MicroORM;
using Utilities.SQL.MicroORM.Interfaces;
using System.Collections.Concurrent;
using Utilities.DataTypes.Comparison;
using Utilities.Reflection.ExtensionMethods;
using Utilities.Caching;
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
        /// <param name="ParameterPrefix">Parameter prefix</param>
        /// <param name="DatabaseName">Database name (can be used later to pull connection information)</param>
        public SQLHelper(string Command, string ConnectionUsing, CommandType CommandType, string DatabaseName = "Default",
            string DbType = "System.Data.SqlClient", string ParameterPrefix = "@", bool Profile = false)
        {
            DatabaseUsing = SQLHelper.Database(ConnectionUsing, DatabaseName, DbType, ParameterPrefix, Profile);
            this.Command = new Command(Command, CommandType, ParameterPrefix);
            Factory = DbProviderFactories.GetFactory(DatabaseUsing.DbType);
            Connection = Factory.CreateConnection();
            Connection.ConnectionString = DatabaseUsing.Connection;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Command">Command to use</param>
        /// <param name="ConnectionUsing">The connection string to use</param>
        /// <param name="DbType">Database type, based on ADO.Net provider name</param>
        /// <param name="Profile">Determines if the calls should be profiled</param>
        /// <param name="ParameterPrefix">Parameter prefix</param>
        /// <param name="DatabaseName">Database name (can be used later to pull connection information)</param>
        public SQLHelper(ICommand Command, string ConnectionUsing, string DatabaseName,
            string DbType = "System.Data.SqlClient", string ParameterPrefix = "@", bool Profile = false)
        {
            this.Command = Command;
            DatabaseUsing = SQLHelper.Database(ConnectionUsing, DatabaseName, DbType, ParameterPrefix, Profile);
            Factory = DbProviderFactories.GetFactory(DatabaseUsing.DbType);
            Connection = Factory.CreateConnection();
            Connection.ConnectionString = DatabaseUsing.Connection;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Command">Stored procedure/SQL Text to use</param>
        /// <param name="CommandType">The command type of the command sent in</param>
        /// <param name="Database">Database to use</param>
        public SQLHelper(string Command, CommandType CommandType, string Database = "Default")
        {
            DatabaseUsing = GetDatabase(Database);
            this.Command = new Command(Command, CommandType, DatabaseUsing.ParameterPrefix);
            Factory = DbProviderFactories.GetFactory(DatabaseUsing.DbType);
            Connection = Factory.CreateConnection();
            Connection.ConnectionString = DatabaseUsing.Connection;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Command">Command to use</param>
        /// <param name="Database">Database name (can be used later to pull connection information)</param>
        public SQLHelper(ICommand Command, string Database = "Default")
        {
            this.Command = Command;
            DatabaseUsing = GetDatabase(Database);
            Factory = DbProviderFactories.GetFactory(DatabaseUsing.DbType);
            Connection = Factory.CreateConnection();
            Connection.ConnectionString = DatabaseUsing.Connection;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Database">Database name (can be used later to pull connection information)</param>
        public SQLHelper(string Database = "Default")
        {
            this.Command = new Command("", CommandType.Text);
            DatabaseUsing = GetDatabase(Database);
            Factory = DbProviderFactories.GetFactory(DatabaseUsing.DbType);
            Connection = Factory.CreateConnection();
            Connection.ConnectionString = DatabaseUsing.Connection;
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
        protected virtual IDataReader Reader { get; set; }

        /// <summary>
        /// List of database connections
        /// </summary>
        protected static ConcurrentDictionary<string, Database> Databases = new ConcurrentDictionary<string, Database>();

        /// <summary>
        /// Cache that is used internally
        /// </summary>
        protected static Cache<int> Cache = new Cache<int>();

        /// <summary>
        /// Database using
        /// </summary>
        protected virtual Database DatabaseUsing { get; set; }

        /// <summary>
        /// Command using
        /// </summary>
        protected virtual ICommand Command { get; set; }

        #endregion

        #region Events



        #endregion

        #region Protected Functions

        #region BeginTransaction

        /// <summary>
        /// Begins a transaction
        /// </summary>
        protected virtual SQLHelper BeginTransaction()
        {
            ExecutableCommand.BeginTransaction();
            return this;
        }

        #endregion

        #region Close

        /// <summary>
        /// Closes the connection
        /// </summary>
        protected virtual SQLHelper Close()
        {
            ExecutableCommand.Close();
            return this;
        }

        #endregion

        #region Commit

        /// <summary>
        /// Commits a transaction
        /// </summary>
        protected virtual SQLHelper Commit()
        {
            ExecutableCommand.Commit();
            return this;
        }

        #endregion

        #region GetDatabase

        /// <summary>
        /// Adds a database's info
        /// </summary>
        /// <param name="Name">Name to associate with the database (if not specified, it uses the first connection string it finds in the configuration manager)</param>
        /// <returns>The database object specified</returns>
        protected virtual Database GetDatabase(string Name = "Default")
        {
            Database TempDatabase = Databases.GetValue(Name, null);
            return TempDatabase.IsNull() ? SQLHelper.Database("", Name, "", "", false) : TempDatabase;
        }

        #endregion

        #region Open

        /// <summary>
        /// Opens the connection
        /// </summary>
        protected virtual SQLHelper Open()
        {
            ExecutableCommand.Open();
            return this;
        }

        #endregion

        #region Rollback

        /// <summary>
        /// Rolls back a transaction
        /// </summary>
        protected virtual SQLHelper Rollback()
        {
            ExecutableCommand.Rollback();
            return this;
        }

        #endregion

        #region SetupScalarCommand

        /// <summary>
        /// Sets up the scalar command
        /// </summary>
        /// <param name="Command">Command to create with</param>
        /// <param name="Parameters">Parameter list</param>
        /// <param name="Mapping">Mapping information</param>
        /// <returns>The string command</returns>
        protected virtual string SetupScalarCommand<ClassType>(string Command, Mapping<ClassType> Mapping, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            string WhereCommand = "";
            if (!Parameters.IsNullOrEmpty() && Parameters.Length > 0)
            {
                WhereCommand += " WHERE ";
                string Splitter = "";
                foreach (IParameter Parameter in Parameters)
                {
                    WhereCommand += Splitter + Parameter;
                    Splitter = " AND ";
                }
            }
            return string.Format("SELECT {0} FROM {1} {2}", Command, Mapping.TableName, WhereCommand);
        }

        #endregion

        #region SetupDeleteCommand

        /// <summary>
        /// Sets up the delete command
        /// </summary>
        /// <param name="Mapping">Mapping information</param>
        /// <returns>The command string</returns>
        protected virtual string SetupDeleteCommand<ClassType>(Mapping<ClassType> Mapping)
            where ClassType : class,new()
        {
            return string.Format("DELETE FROM {0} WHERE {1}", Mapping.TableName, Mapping.PrimaryKey + "=" + DatabaseUsing.ParameterPrefix + Mapping.PrimaryKey);
        }

        #endregion

        #region SetupInsertCommand

        /// <summary>
        /// Sets up the insert command
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <param name="Mapping">Mapping information</param>
        /// <returns>The command string</returns>
        protected virtual string SetupInsertCommand<ClassType>(Mapping<ClassType> Mapping, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            string ParameterList = "";
            string ValueList = "";
            string Splitter = "";
            foreach (string Name in Mapping.ParameterNames)
            {
                if (!Mapping.AutoIncrement || Name != Mapping.PrimaryKey)
                {
                    ParameterList += Splitter + Name;
                    ValueList += Splitter + DatabaseUsing.ParameterPrefix + Name;
                    Splitter = ",";
                }
            }
            foreach (IParameter Parameter in Parameters)
            {
                ParameterList += Splitter + Parameter.ID;
                ValueList += Splitter + DatabaseUsing.ParameterPrefix + Parameter.ID;
                Splitter = ",";
            }
            return string.Format("INSERT INTO {0}({1}) VALUES({2}) SELECT scope_identity() as [ID]", Mapping.TableName, ParameterList, ValueList);
        }

        #endregion

        #region SetupPageCountCommand

        /// <summary>
        /// Sets up the page count command
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="Parameters">Parameter list</param>
        /// <param name="Mapping">Mapping information</param>
        /// <returns>The string command</returns>
        protected virtual string SetupPageCountCommand<ClassType>(Mapping<ClassType> Mapping, int PageSize, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            string WhereCommand = "";
            if (!Parameters.IsNullOrEmpty() && Parameters.Length > 0)
            {
                WhereCommand += " WHERE ";
                string Splitter = "";
                foreach (IParameter Parameter in Parameters)
                {
                    WhereCommand += Splitter + Parameter;
                    Splitter = " AND ";
                }
            }
            return string.Format("SELECT COUNT(*) as Total FROM (SELECT {0} FROM {1} {2}) as Query", Mapping.PrimaryKey, Mapping.TableName, WhereCommand);
        }

        /// <summary>
        /// Sets up the page count command
        /// </summary>
        /// <param name="Command">Command</param>
        /// <returns>The string command</returns>
        protected virtual string SetupPageCountCommand(string Command)
        {
            return string.Format("SELECT COUNT(*) as Total FROM ({0}) as Query", Command);
        }

        #endregion

        #region SetupPagedCommand

        /// <summary>
        /// Sets up the paged select command
        /// </summary>
        /// <param name="Columns">Columns to return</param>
        /// <param name="OrderBy">Order by clause</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">Current page</param>
        /// <param name="Parameters">Parameter list</param>
        /// <param name="Mapping">Mapping information</param>
        /// <returns>The command string</returns>
        protected virtual string SetupPagedCommand<ClassType>(string Columns, string OrderBy, int PageSize, int CurrentPage,
                                                              Mapping<ClassType> Mapping, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            if (OrderBy.IsNullOrEmpty())
                OrderBy = Mapping.PrimaryKey;

            string WhereCommand = "";
            if (!Parameters.IsNullOrEmpty() && Parameters.Length > 0)
            {
                WhereCommand += " WHERE ";
                string Splitter = "";
                foreach (IParameter Parameter in Parameters)
                {
                    WhereCommand += Splitter + Parameter;
                    Splitter = " AND ";
                }
            }
            return SetupPagedCommand(string.Format("SELECT {0} FROM {1} {2}", Columns, Mapping.TableName, WhereCommand), OrderBy, PageSize, CurrentPage, Mapping);
        }

        /// <summary>
        /// Sets up the paged select command
        /// </summary>
        /// <param name="Query">Query used in getting the paged data</param>
        /// <param name="OrderBy">Order by clause</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">Current page</param>
        /// <param name="Mapping">Mapping information</param>
        /// <returns>The command string</returns>
        protected virtual string SetupPagedCommand<ClassType>(string Query, string OrderBy, int PageSize,
                                                              int CurrentPage, Mapping<ClassType> Mapping)
            where ClassType : class,new()
        {
            if (OrderBy.IsNullOrEmpty())
                OrderBy = Mapping.PrimaryKey;
            int PageStart = CurrentPage * PageSize;
            return string.Format("SELECT Paged.* FROM (SELECT ROW_NUMBER() OVER (ORDER BY {1}) AS Row, Query.* FROM ({0}) as Query) AS Paged WHERE Row>{2} AND Row<={3}",
                                    Query,
                                    OrderBy,
                                    PageStart,
                                    PageStart + PageSize);
        }

        #endregion

        #region SetupSelectCommand

        /// <summary>
        /// Sets up the select command
        /// </summary>
        /// <param name="Columns">Columns to return</param>
        /// <param name="Limit">limit on the number of items to return</param>
        /// <param name="OrderBy">Order by clause</param>
        /// <param name="Parameters">Parameter list</param>
        /// <param name="Mapping">Mapping information</param>
        /// <returns>The string command</returns>
        protected virtual string SetupSelectCommand<ClassType>(string Columns, int Limit, string OrderBy, Mapping<ClassType> Mapping,
                                                               params IParameter[] Parameters)
            where ClassType : class,new()
        {
            string Command = (Limit > 0 ? "SELECT TOP " + Limit : "SELECT") + " {0} FROM {1}";
            if (!Parameters.IsNullOrEmpty() && Parameters.Length > 0)
            {
                Command += " WHERE ";
                string Splitter = "";
                foreach (IParameter Parameter in Parameters)
                {
                    Command += Splitter + Parameter;
                    Splitter = " AND ";
                }
            }
            if (!OrderBy.IsNullOrEmpty())
                Command += OrderBy.Trim().ToLower().StartsWith("order by", StringComparison.CurrentCultureIgnoreCase) ? " " + OrderBy : " ORDER BY " + OrderBy;
            return string.Format(Command, Columns, Mapping.TableName);
        }

        #endregion

        #region SetupUpdateCommand

        /// <summary>
        /// Sets up the update command
        /// </summary>
        /// <param name="Mapping">Mapping information</param>
        /// <param name="Parameters">Parameters</param>
        /// <returns>The command string</returns>
        protected virtual string SetupUpdateCommand<ClassType>(Mapping<ClassType> Mapping, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            string ParameterList = "";
            string WhereCommand = Mapping.PrimaryKey + "=" + DatabaseUsing.ParameterPrefix + Mapping.PrimaryKey;
            string Splitter = "";
            foreach (string Name in Mapping.ParameterNames)
            {
                if (Name != Mapping.PrimaryKey)
                {
                    ParameterList += Splitter + Name + "=" + DatabaseUsing.ParameterPrefix + Name;
                    Splitter = ",";
                }
            }
            foreach (IParameter Parameter in Parameters)
            {
                ParameterList += Splitter + Parameter.ToString();
                Splitter = ",";
            }
            return string.Format("UPDATE {0} SET {1} WHERE {2}", Mapping.TableName, ParameterList, WhereCommand);
        }

        #endregion

        #endregion

        #region Public Functions

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
            this.Command.AddParameter(ID, Value, Direction);
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
            this.Command.AddParameter(ID, Type, Value, Direction);
            return this;
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
            this.Command.AddParameter(ID, Value, Direction);
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
        public virtual SQLHelper AddParameter(string ID, DbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input)
        {
            this.Command.AddParameter(ID, Type, Value, Direction);
            return this;
        }

        /// <summary>
        /// Adds parameters to the call
        /// </summary>
        /// <param name="Parameters">Parameters to add</param>
        /// <returns>This</returns>
        public virtual SQLHelper AddParameter(params IParameter[] Parameters)
        {
            this.Command.AddParameter(Parameters);
            return this;
        }

        /// <summary>
        /// Adds parameters to the call
        /// </summary>
        /// <param name="Parameters">Parameters to add</param>
        /// <returns>This</returns>
        public virtual SQLHelper AddParameter(params object[] Parameters)
        {
            this.Command.AddParameter(DatabaseUsing.ParameterPrefix, Parameters);
            return this;
        }

        #endregion

        #region All

        /// <summary>
        /// Gets a list of all objects that meet the specified criteria
        /// </summary>
        /// <param name="Command">Command to use (can be an SQL string or stored procedure)</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Objects">Objects to modify/addon to (uses primary key to determine)</param>
        /// <param name="ObjectCreator">Function used to create the indvidual objects</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <returns>A list of all objects that meet the specified criteria</returns>
        public virtual IEnumerable<ClassType> All<ClassType>(string Command, CommandType CommandType, IEnumerable<ClassType> Objects = null,
                                                            Func<ClassType> ObjectCreator = null, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Command.ThrowIfNullOrEmpty("Command");
            DatabaseUsing.Mappings.ThrowIfNull("Mappings");
            List<ClassType> Return = Objects == null ? new List<ClassType>() : Objects.ToList();
            ObjectCreator = (ObjectCreator == null) ? (() => new ClassType()) : ObjectCreator;
            this.Command = new Command(Command, CommandType, Parameters);
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            ExecuteReader();
            while (Read())
            {
                bool Add = false;
                object CurrentKey = GetParameter<object>(MappingUsing.PrimaryKey, null);
                ClassType Temp = default(ClassType);
                if (Objects != null) Temp = Objects.FirstOrDefault(x => MappingUsing.GetPrimaryKey(x).Equals(CurrentKey));
                if (Temp == default(ClassType))
                {
                    Temp = ObjectCreator();
                    Add = true;
                }
                MappingUsing.Mappings.Copy(this, Temp);
                if (Add) Return.Add(Temp);
            }
            return Return;
        }

        /// <summary>
        /// Gets a list of all objects that meet the specified criteria
        /// </summary>
        /// <param name="Columns">Columns to return</param>
        /// <param name="Limit">Limit on the number of items to return</param>
        /// <param name="OrderBy">Order by clause</param>
        /// <param name="Objects">Objects to modify/addon to (uses primary key to determine)</param>
        /// <param name="ObjectCreator">Function used to create the individual objects</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <returns>A list of all objects that meet the specified criteria</returns>
        public virtual IEnumerable<ClassType> All<ClassType>(string Columns = "*", int Limit = 0, string OrderBy = "", IEnumerable<ClassType> Objects = null,
                                                            Func<ClassType> ObjectCreator = null, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Columns.ThrowIfNullOrEmpty("Columns");
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            return All(SetupSelectCommand(Columns, Limit, OrderBy, MappingUsing, Parameters), CommandType.Text, Objects, ObjectCreator, Parameters);
        }

        #endregion

        #region Any

        /// <summary>
        /// Gets a single object that fits the criteria
        /// </summary>
        /// <param name="Columns">Columns to select</param>
        /// <param name="ObjectToReturn">Object to return (in case the object needs to be created outside this,
        /// or default value is desired in case of nothing found)</param>
        /// <param name="ObjectCreator">Function used to create the object if the ObjectToReturn is set to null
        /// (if set to null, it just creates a new object using the default constructor)</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <returns>An object fitting the criteria specified or null if none are found</returns>
        public virtual ClassType Any<ClassType>(string Columns = "*", ClassType ObjectToReturn = null, Func<ClassType> ObjectCreator = null, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Columns.ThrowIfNullOrEmpty("Columns");
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            return Any(SetupSelectCommand(Columns, 1, "", MappingUsing, Parameters), CommandType.Text, ObjectToReturn, ObjectCreator, Parameters);
        }

        /// <summary>
        /// Gets a single object that fits the criteria
        /// </summary>
        /// <param name="Command">Command to use (can be an SQL string or stored procedure name)</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="ObjectToReturn">Object to return (in case the object needs to be created outside this,
        /// or default value is desired in case of nothing found)</param>
        /// <param name="ObjectCreator">Function used to create the object if the ObjectToReturn is set to null
        /// (if set to null, it just creates a new object using the default constructor)</param>
        /// <param name="Parameters">Parameters used to search by</param>
        /// <returns>An object fitting the criteria specified or null if none are found</returns>
        public virtual ClassType Any<ClassType>(string Command, CommandType CommandType, ClassType ObjectToReturn = null, Func<ClassType> ObjectCreator = null,
                                                params IParameter[] Parameters)
            where ClassType : class,new()
        {
            DatabaseUsing.Mappings.ThrowIfNull("Mappings");
            Command.ThrowIfNullOrEmpty("Command");
            ObjectCreator = (ObjectCreator == null) ? (() => new ClassType()) : ObjectCreator;
            this.Command = new Command(Command, CommandType, Parameters);
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            ExecuteReader();
            if (Read())
            {
                ClassType Return = (ObjectToReturn == null) ? ObjectCreator() : ObjectToReturn;
                MappingUsing.Mappings.Copy(this, Return);
                return Return;
            }
            return ObjectToReturn;
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes an object from the database
        /// </summary>
        /// <param name="Command">Command to use</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Object">Object to delete</param>
        /// <returns>The number of rows deleted</returns>
        public virtual int Delete<ClassType>(string Command, CommandType CommandType, ClassType Object)
            where ClassType : class,new()
        {
            Object.ThrowIfNull("Object");
            Command.ThrowIfNullOrEmpty("Command");
            this.Command = new Command(Command, CommandType, DatabaseUsing.ParameterPrefix);
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            MappingUsing.Mappings.Copy(Object, this);
            return ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes an object from the database
        /// </summary>
        /// <param name="Object">Object to delete</param>
        /// <returns>The number of rows deleted</returns>
        public virtual int Delete<ClassType>(ClassType Object)
            where ClassType : class,new()
        {
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            return Delete(SetupDeleteCommand(MappingUsing), CommandType.Text, Object);
        }

        /// <summary>
        /// Deletes a list of objects from the database
        /// </summary>
        /// <param name="Objects">Objects to delete</param>
        /// <returns>The number of rows deleted</returns>
        public virtual int Delete<ClassType>(IEnumerable<ClassType> Objects)
            where ClassType : class,new()
        {
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            IBatchCommand Commands = Batch();
            foreach (ClassType Object in Objects)
            {
                Commands.AddCommand(SetupDeleteCommand(MappingUsing), CommandType.Text, DatabaseUsing.ParameterPrefix, MappingUsing.GetPrimaryKey(Object));
            }
            return ExecuteNonQuery();
        }

        #endregion

        #region Insert

        /// <summary>
        /// Inserts an object based on the command specified
        /// </summary>
        /// <typeparam name="DataType">Data type expected to be returned from the query (to get the ID, etc.)</typeparam>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <param name="Command">Command to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Object">Object to insert</param>
        /// <param name="Parameters">Parameters sent into the function</param>
        /// <returns>The returned object from the query (usually the newly created row's ID)</returns>
        public virtual DataType Insert<ClassType, DataType>(string Command, CommandType CommandType, ClassType Object, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Object.ThrowIfNull("Object");
            Command.ThrowIfNullOrEmpty("Command");
            this.Command = new Command(Command, CommandType, Parameters);
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            MappingUsing.Mappings.Copy(Object, this);
            return ExecuteScalar<DataType>();
        }

        /// <summary>
        /// Inserts an object into the database
        /// </summary>
        /// <typeparam name="DataType">Data type expected (should be the same type as the primary key)</typeparam>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <param name="Object">Object to insert</param>
        /// <param name="Parameters">Parameters sent into the function</param>
        /// <returns>The returned object from the query (the newly created row's ID)</returns>
        public virtual DataType Insert<ClassType, DataType>(ClassType Object, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            return Insert<ClassType, DataType>(SetupInsertCommand(MappingUsing, Parameters), CommandType.Text, Object, Parameters);
        }

        #endregion

        #region PageCount

        /// <summary>
        /// Gets the number of pages based on the specified 
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <returns>The number of pages that the table contains for the specified page size</returns>
        public virtual int PageCount<ClassType>(int PageSize = 25, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            Command = new Command(SetupPageCountCommand(MappingUsing, PageSize, Parameters), CommandType.Text, Parameters);
            ExecuteReader();
            if (Read())
            {
                int Total = GetParameter("Total", 0);
                return Total % PageSize == 0 ? Total / PageSize : (Total / PageSize) + 1;
            }
            return 0;
        }

        /// <summary>
        /// Gets the number of pages based on the specified 
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <param name="Command">Command to get the page count of</param>
        /// <returns>The number of pages that the table contains for the specified page size</returns>
        public virtual int PageCount<ClassType>(string Command, int PageSize = 25, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            this.Command = new Command(SetupPageCountCommand(Command), CommandType.Text, Parameters);
            ExecuteReader();
            if (Read())
            {
                int Total = GetParameter("Total", 0);
                return Total % PageSize == 0 ? Total / PageSize : (Total / PageSize) + 1;
            }
            return 0;
        }

        #endregion

        #region Paged

        /// <summary>
        /// Gets a paged list of objects fitting the specified criteria
        /// </summary>
        /// <param name="Columns">Columns to return</param>
        /// <param name="OrderBy">Order by clause</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">The current page (starting at 0)</param>
        /// <param name="Objects">Objects to modify/addon to (uses primary key to determine)</param>
        /// <param name="ObjectCreator">Function used to create the individual objects (if set to null, it uses the default constructor)</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <returns>A list of objects that fit the specified criteria</returns>
        public virtual IEnumerable<ClassType> Paged<ClassType>(string Columns = "*", string OrderBy = "", int PageSize = 25, int CurrentPage = 0,
                                                               IEnumerable<ClassType> Objects = null, Func<ClassType> ObjectCreator = null,
                                                               params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Columns.ThrowIfNullOrEmpty("Columns");
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            return All(SetupPagedCommand(Columns, OrderBy, PageSize, CurrentPage, MappingUsing, Parameters), CommandType.Text, Objects, ObjectCreator, Parameters);
        }

        #endregion

        #region PagedCommand

        /// <summary>
        /// Gets a paged list of objects fitting the specified criteria
        /// </summary>
        /// <param name="Command">Command to return data from</param>
        /// <param name="OrderBy">Order by clause</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">The current page (starting at 0)</param>
        /// <param name="Objects">Objects to modify/addon to (uses primary key to determine)</param>
        /// <param name="ObjectCreator">Function used to create the individual objects (if set to null, it uses the default constructor)</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <returns>A list of objects that fit the specified criteria</returns>
        public virtual IEnumerable<ClassType> PagedCommand<ClassType>(string Command, string OrderBy = "", int PageSize = 25, int CurrentPage = 0,
                                                                      IEnumerable<ClassType> Objects = null, Func<ClassType> ObjectCreator = null,
                                                                      params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Command.ThrowIfNullOrEmpty("Command");
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            return All(SetupPagedCommand(Command, OrderBy, PageSize, CurrentPage, MappingUsing), CommandType.Text, Objects, ObjectCreator, Parameters);
        }

        #endregion

        #region Save

        /// <summary>
        /// Saves (inserts/updates) an object based on the following criteria:
        /// 1) If autoincrement is set to true and the primary key is the default value, it inserts
        /// 2) If autoincrement is set to true and the primary key is not the default value, it updates
        /// 3) If autoincrement is set to false and the primary key is the default value, it inserts
        /// 4) If autoincrement is set to false and the primary key is not the default value,
        /// it does an Any call to see if the item is already in the database. If it is, it does an
        /// update. Otherwise it does an insert.
        /// On an insert, the primary key property is updated with the resulting value of the insert.
        /// </summary>
        /// <param name="Object">Object to save</param>
        /// <param name="Parameters">Extra parameters to be added to the insert/update function</param>
        public virtual void Save<ClassType, PrimaryKeyType>(ClassType Object, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            PrimaryKeyType PrimaryKeyVal = MappingUsing.GetPrimaryKey(Object).TryTo(default(PrimaryKeyType));
            GenericEqualityComparer<PrimaryKeyType> Comparer = new GenericEqualityComparer<PrimaryKeyType>();
            if (Comparer.Equals(PrimaryKeyVal, default(PrimaryKeyType)))
            {
                PrimaryKeyVal = Insert<ClassType, PrimaryKeyType>(Object, Parameters);
                MappingUsing.PrimaryKeyMapping.CopyRightToLeft(PrimaryKeyVal, Object);
                return;
            }
            if (MappingUsing.AutoIncrement)
            {
                Update<ClassType>(Object, Parameters);
                return;
            }
            IParameter Param1 = null;
            if (typeof(PrimaryKeyType).IsOfType(typeof(string)))
                Param1 = new StringParameter(MappingUsing.PrimaryKey, PrimaryKeyVal.ToString(), ParameterDirection.Input, DatabaseUsing.ParameterPrefix);
            else
                Param1 = new Parameter<PrimaryKeyType>(MappingUsing.PrimaryKey, PrimaryKeyVal, ParameterDirection.Input, DatabaseUsing.ParameterPrefix);
            ClassType TempVal = Any<ClassType>(MappingUsing.PrimaryKey, null, null, Param1);
            if (TempVal == null)
            {
                PrimaryKeyVal = Insert<ClassType, PrimaryKeyType>(Object, Parameters);
                MappingUsing.PrimaryKeyMapping.CopyRightToLeft(PrimaryKeyVal, Object);
                return;
            }
            Update(Object, Parameters);
        }

        /// <summary>
        /// Saves (inserts/updates) a list of objects based on the following criteria:
        /// 1) If autoincrement is set to true and the primary key is the default value, it inserts
        /// 2) If autoincrement is set to true and the primary key is not the default value, it updates
        /// 3) If autoincrement is set to false and the primary key is the default value, it inserts
        /// 4) If autoincrement is set to false and the primary key is not the default value,
        /// it does an Any call to see if the item is already in the database. If it is, it does an
        /// update. Otherwise it does an insert.
        /// On an insert, the primary key property is updated with the resulting value of the insert.
        /// </summary>
        /// <param name="Objects">Objects to save</param>
        /// <param name="Parameters">Extra parameters to be added to the insert/update function</param>
        public virtual void Save<ClassType, PrimaryKeyType>(IEnumerable<ClassType> Objects, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            foreach (ClassType Object in Objects)
            {
                Save<ClassType, PrimaryKeyType>(Object, Parameters);
            }
        }

        #endregion

        #region Scalar

        /// <summary>
        /// Runs a supplied scalar function and returns the result
        /// </summary>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <param name="Command">Command to get the page count of</param>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <returns>The scalar value returned by the command</returns>
        public virtual DataType Scalar<ClassType, DataType>(string Command, CommandType CommandType, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            this.Command = new Command(Command, CommandType.Text, Parameters);
            return ExecuteScalar<DataType>();
        }

        /// <summary>
        /// Runs a scalar command using the specified aggregate function
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <typeparam name="ClassType">Class type</typeparam>
        /// <param name="AggregateFunction">Aggregate function</param>
        /// <param name="Parameters">Parameters</param>
        /// <returns>The scalar value returned by the command</returns>
        public virtual DataType Scalar<ClassType, DataType>(string AggregateFunction, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            this.Command = new Command(SetupScalarCommand(AggregateFunction, MappingUsing, Parameters), CommandType.Text, Parameters);
            return ExecuteScalar<DataType>();
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates an object in the database
        /// </summary>
        /// <param name="Command">Command to use</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Object">Object to update</param>
        /// <param name="Parameters">Parameters sent into the function</param>
        /// <returns>The number of rows updated</returns>
        public virtual int Update<ClassType>(string Command, CommandType CommandType, ClassType Object, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Command.ThrowIfNullOrEmpty("Command");
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            this.Command = new Command(Command, CommandType.Text, Parameters);
            MappingUsing.Mappings.Copy(Object, this);
            return ExecuteNonQuery();
        }

        /// <summary>
        /// Updates an object in the database
        /// </summary>
        /// <param name="Object">Object to update</param>
        /// <param name="Parameters">Parameters sent into the function</param>
        /// <returns>The number of rows updated</returns>
        public virtual int Update<ClassType>(ClassType Object, params IParameter[] Parameters)
            where ClassType : class,new()
        {
            Mapping<ClassType> MappingUsing = (Mapping<ClassType>)DatabaseUsing.Mappings[typeof(ClassType)];
            return Update(SetupUpdateCommand(MappingUsing, Parameters), CommandType.Text, Object, Parameters);
        }

        #endregion

        #region Batch

        /// <summary>
        /// Used to help batch various commands together
        /// </summary>
        /// <returns>The batch object</returns>
        public virtual IBatchCommand Batch()
        {
            this.Command = new BatchCommand();
            return (IBatchCommand)this.Command;
        }

        #endregion

        #region ClearParameters

        /// <summary>
        /// Clears the parameters
        /// </summary>
        public virtual SQLHelper ClearParameters()
        {
            this.Command.ClearParameters();
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
        /// <param name="CreateTransaction">Create transaction</param>
        /// <returns>Returns this SQLHelper object</returns>
        public virtual SQLHelper ExecuteBulkCopy<T>(IEnumerable<T> Data, string DestinationTable, SqlBulkCopyOptions Options = SqlBulkCopyOptions.Default,
                                                    bool CreateTransaction=false)
        {
            using (DataTable Table = Data.ToDataTable())
            {
                return ExecuteBulkCopy(Table, DestinationTable, Options,CreateTransaction);
            }
        }

        /// <summary>
        /// Does a bulk copy of the data (only usable on SQL Server)
        /// </summary>
        /// <param name="Data">Data to copy over</param>
        /// <param name="DestinationTable">Table to copy the data to</param>
        /// <param name="Options">Options used during the copy</param>
        /// <param name="CreateTransaction">Create transaction</param>
        /// <returns>Returns this SQLHelper object</returns>
        public virtual SQLHelper ExecuteBulkCopy(DataTable Data, string DestinationTable, SqlBulkCopyOptions Options = SqlBulkCopyOptions.Default,
                                                 bool CreateTransaction=false)
        {
            if (DatabaseUsing.Profile)
            {
                using (new Profiler.SQLProfiler("ExecuteBulkCopy", Command.SQLCommand, Command.Parameters.ToArray()))
                {
                    BulkCopy(Data, DestinationTable, Options, CreateTransaction);
                    return this;
                }
            }
            BulkCopy(Data, DestinationTable, Options, CreateTransaction);
            return this;
        }

        /// <summary>
        /// Does the actual bulk copy
        /// </summary>
        /// <param name="Data">Data to copy over</param>
        /// <param name="DestinationTable">Table to copy the data to</param>
        /// <param name="Options">Options used during the copy</param>
        /// <param name="CreateTransaction">Create transaction</param>
        private void BulkCopy(DataTable Data, string DestinationTable, SqlBulkCopyOptions Options,bool CreateTransaction)
        {
            try
            {
                Setup(CreateTransaction);
                using (SqlBulkCopy Copier = new SqlBulkCopy(Connection.ConnectionString, Options))
                {
                    foreach (DataColumn Column in Data.Columns)
                        Copier.ColumnMappings.Add(Column.ColumnName, Column.ColumnName);
                    Copier.DestinationTableName = DestinationTable;
                    Copier.WriteToServer(Data);
                }
                Commit();
            }
            catch { Rollback(); throw; }
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// Executes the query and returns a data set
        /// </summary>
        /// <param name="CreateTransaction">Create transaction</param>
        /// <param name="Cache">Determines if the query should be cached for future queries</param>
        /// <returns>A dataset filled with the results of the query</returns>
        public virtual DataSet ExecuteDataSet(bool CreateTransaction = false,bool Cache=false)
        {
            try
            {
                Setup(CreateTransaction);
                DataSet ReturnValue = null;
                if (DatabaseUsing.Profile)
                {
                    using (new Profiler.SQLProfiler("ExecuteDataSet", Command.SQLCommand, Command.Parameters.ToArray()))
                    {
                        if (Cache && SQLHelper.Cache.Exists(Command.GetHashCode()))
                            ReturnValue = SQLHelper.Cache.Get<DataSet>(Command.GetHashCode());
                        else if (ExecutableCommand.IsNotNull())
                        {
                            ReturnValue = ExecutableCommand.ExecuteDataSet(Factory);
                            if (Cache)
                                SQLHelper.Cache.Add(Command.GetHashCode(), ReturnValue);
                            Commit();
                        }
                        return ReturnValue;
                    }
                }
                if (Cache && SQLHelper.Cache.Exists(Command.GetHashCode()))
                    ReturnValue = SQLHelper.Cache.Get<DataSet>(Command.GetHashCode());
                else if (ExecutableCommand.IsNotNull())
                {
                    ReturnValue = ExecutableCommand.ExecuteDataSet(Factory);
                    if (Cache)
                        SQLHelper.Cache.Add(Command.GetHashCode(), ReturnValue);
                    Commit();
                }
                return ReturnValue;
            }
            catch { Rollback(); throw; }
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// Executes the stored procedure as a non query
        /// </summary>
        /// <param name="CreateTransaction">Creates a transaction for this command</param>
        /// <returns>Number of rows effected</returns>
        public virtual int ExecuteNonQuery(bool CreateTransaction=false)
        {
            try
            {
                Setup(CreateTransaction);
                if (ExecutableCommand.IsNull())
                    return 0;
                int ReturnValue = 0;
                if (DatabaseUsing.Profile)
                {
                    using (new Profiler.SQLProfiler("ExecuteNonQuery", Command.SQLCommand, Command.Parameters.ToArray()))
                    {
                        ReturnValue = ExecutableCommand.ExecuteNonQuery();
                        Commit();
                        return ReturnValue;
                    }
                }
                ReturnValue = ExecutableCommand.ExecuteNonQuery();
                Commit();
                return ReturnValue;
            }
            catch { Rollback(); throw; }
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// Executes the stored procedure and returns a reader object
        /// </summary>
        /// <param name="CreateTransaction">Create transaction</param>
        /// <param name="Cache">Determines if the query should be cached for future queries</param>
        /// <returns>this</returns>
        public virtual SQLHelper ExecuteReader(bool CreateTransaction = false, bool Cache = false)
        {
            try
            {
                Setup(CreateTransaction);
                if (DatabaseUsing.Profile)
                {
                    using (new Profiler.SQLProfiler("ExecuteReader", Command.SQLCommand, Command.Parameters.ToArray()))
                    {
                        if (Cache && SQLHelper.Cache.Exists(Command.GetHashCode()))
                            Reader = new CacheTables(SQLHelper.Cache.Get<IDataReader>(Command.GetHashCode()));
                        else if (ExecutableCommand.IsNotNull())
                        {
                            using (IDataReader TempReader = ExecutableCommand.ExecuteReader())
                            {
                                Reader = new CacheTables(TempReader);
                                TempReader.Close();
                            }
                            if (Cache)
                                SQLHelper.Cache.Add(Command.GetHashCode(), new CacheTables(Reader));
                            Commit();
                        }
                        return this;
                    }
                }
                if (Cache && SQLHelper.Cache.Exists(Command.GetHashCode()))
                    Reader = new CacheTables(SQLHelper.Cache.Get<IDataReader>(Command.GetHashCode()));
                else if (ExecutableCommand.IsNotNull())
                {
                    using (DbDataReader TempReader = ExecutableCommand.ExecuteReader())
                    {
                        Reader = new CacheTables(TempReader);
                        TempReader.Close();
                    }
                    if (Cache)
                        SQLHelper.Cache.Add(Command.GetHashCode(), new CacheTables(Reader));
                    Commit();
                }
                return this;
            }
            catch { Rollback(); throw; }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes the stored procedure as a scalar query
        /// </summary>
        /// <typeparam name="DataType">Data type to return</typeparam>
        /// <param name="CreateTransaction">Create transaction</param>
        /// <param name="Cache">Determines if the query should be cached for future queries</param>
        /// <returns>The object of the first row and first column</returns>
        public virtual DataType ExecuteScalar<DataType>(bool CreateTransaction = false, bool Cache = false)
        {
            try
            {
                Setup(CreateTransaction);
                DataType ReturnValue = default(DataType);
                if (DatabaseUsing.Profile)
                {
                    using (new Profiler.SQLProfiler("ExecuteScalar", Command.SQLCommand, Command.Parameters.ToArray()))
                    {
                        if (Cache && SQLHelper.Cache.Exists(Command.GetHashCode()))
                            ReturnValue = SQLHelper.Cache.Get<DataType>(Command.GetHashCode());
                        else if (ExecutableCommand.IsNotNull())
                        {
                            ReturnValue = ExecutableCommand.ExecuteScalar<DataType>();
                            if (Cache)
                                SQLHelper.Cache.Add(Command.GetHashCode(), ReturnValue);
                            Commit();
                        }
                        return ReturnValue;
                    }
                }
                if (Cache && SQLHelper.Cache.Exists(Command.GetHashCode()))
                    ReturnValue = SQLHelper.Cache.Get<DataType>(Command.GetHashCode());
                else if (ExecutableCommand.IsNotNull())
                {
                    ReturnValue = ExecutableCommand.ExecuteScalar<DataType>();
                    if (Cache)
                        SQLHelper.Cache.Add(Command.GetHashCode(), ReturnValue);
                    Commit();
                }
                return ReturnValue;
            }
            catch { Rollback(); throw; }
        }

        #endregion

        #region ExecuteXmlReader

        /// <summary>
        /// Executes the query and returns an XmlReader
        /// </summary>
        /// <param name="CreateTransaction">Create transaction</param>
        /// <returns>The XmlReader filled with the data from the query</returns>
        public virtual XmlReader ExecuteXmlReader(bool CreateTransaction = false)
        {
            try
            {
                Setup(CreateTransaction);
                XmlReader ReturnValue = null;
                if (DatabaseUsing.Profile)
                {
                    using (new Profiler.SQLProfiler("ExecuteXmlReader", Command.SQLCommand, Command.Parameters.ToArray()))
                    {
                        if (ExecutableCommand.IsNotNull() && ExecutableCommand is SqlCommand)
                            ReturnValue = ((SqlCommand)ExecutableCommand).ExecuteXmlReader();
                        Commit();
                        return ReturnValue;
                    }
                }
                if (ExecutableCommand.IsNotNull() && ExecutableCommand is SqlCommand)
                    ReturnValue = ((SqlCommand)ExecutableCommand).ExecuteXmlReader();
                Commit();
                return ReturnValue;
            }
            catch { Rollback(); throw; }
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
        public Mapping<ClassType> Map<ClassType>()
            where ClassType : class,new()
        {
            return null;
            //if (!Databases[DatabaseUsing].Mappings.ContainsKey(typeof(ClassType)))
            //    throw new ArgumentOutOfRangeException(typeof(ClassType).Name + " not found");
            //return new Mapping<ClassType>((Mapping<ClassType>)Databases[DatabaseUsing].Mappings[typeof(ClassType)], this);
        }

        #endregion

        #region NextResult

        /// <summary>
        /// Goes to the next result set (used if multiple queries are sent in)
        /// </summary>
        public virtual SQLHelper NextResult()
        {
            if (Reader.IsNotNull())
                Reader.NextResult();
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
            return Reader.IsNull() ? false : Reader.Read();
        }

        #endregion

        #endregion

        #region Private Functions

        #region Setup

        /// <summary>
        /// Sets up the info for 
        /// </summary>
        /// <param name="Transaction">Should a transaction be set up for this call?</param>
        private void Setup(bool Transaction)
        {
            if (Reader != null)
            {
                Reader.Close();
                Reader.Dispose();
                Reader = null;
            }
            if (ExecutableCommand != null)
            {
                if (ExecutableCommand.Transaction != null)
                {
                    ExecutableCommand.Transaction.Dispose();
                    ExecutableCommand.Transaction = null;
                }
                ExecutableCommand.Dispose();
                ExecutableCommand = null;
            }
            ExecutableCommand = Factory.CreateCommand();
            ExecutableCommand.CommandText = Command.SQLCommand;
            ExecutableCommand.Connection = Connection;
            ExecutableCommand.CommandType = Command.CommandType;
            Command.Parameters.ForEach(x => x.AddParameter(ExecutableCommand));
            if (Transaction)
                ExecutableCommand.BeginTransaction();
            Open();
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

        #region ClearCache

        /// <summary>
        /// Clears the cache
        /// </summary>
        public static void ClearCache()
        {
            Cache.Clear();
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
                Databases[Database].Mappings.Clear();
        }

        #endregion

        #region Database

        /// <summary>
        /// Adds a database's info
        /// </summary>
        /// <param name="ConnectionString">Connection string to use for this database (if not specified, uses Name to get the connection string from the configuration manager)</param>
        /// <param name="Name">Name to associate with the database (if not specified, it uses the first connection string it finds in the configuration manager)</param>
        /// <param name="DbType">Database type, based on ADO.Net provider name (will override if it pulls info from the configuration manager)</param>
        /// <param name="Profile">Determines if the commands should be profiled (if empty, it will override with its best guess based on database type)</param>
        /// <param name="ParameterPrefix">Parameter prefix</param>
        /// <param name="Readable">Should this database be used to read data?</param>
        /// <param name="Writable">Should this database be used to write data?</param>
        /// <returns>The database object specified</returns>
        public static Database Database(string ConnectionString, string Name = "Default", string DbType = "System.Data.SqlClient",
                                        string ParameterPrefix = "@", bool Profile = false, bool Readable = true, bool Writable = true)
        {
            return Databases.AddOrUpdate(Name,
                                        new Database(ConnectionString, Name, DbType, ParameterPrefix, Profile, Readable, Writable),
                                        (x, y) => new Database(ConnectionString, Name, DbType, ParameterPrefix, Profile, Writable, Readable));
        }

        #endregion

        #region Map

        /// <summary>
        /// Creates a mapping (or returns an already created one if it exists)
        /// </summary>
        /// <typeparam name="ClassType">Class type to map</typeparam>
        /// <param name="TableName">Table name</param>
        /// <param name="PrimaryKey">Primary key</param>
        /// <param name="AutoIncrement">Auto incrementing primar key</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        /// <param name="Database">Database to use</param>
        /// <returns>The created mapping (or an already created one if it exists)</returns>
        public static Mapping<ClassType> Map<ClassType>(string TableName, string PrimaryKey, bool AutoIncrement = true,
                                                        string ParameterStarter = "@", string Database = "Default")
            where ClassType : class,new()
        {
            return (Mapping<ClassType>)Databases.GetOrAdd(Database, new Database("", Database))
                                                .Mappings
                                                .GetOrAdd(typeof(ClassType), new Mapping<ClassType>(TableName, PrimaryKey, AutoIncrement, ParameterStarter));
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
            if (Connection != null)
            {
                Connection.Dispose();
                Connection = null;
            }
            if (ExecutableCommand != null)
            {
                if (ExecutableCommand.Transaction != null)
                {
                    ExecutableCommand.Transaction.Dispose();
                    ExecutableCommand.Transaction = null;
                }
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