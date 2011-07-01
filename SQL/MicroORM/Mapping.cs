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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.DataMapper;
using System.Linq.Expressions;
using Utilities.SQL.MicroORM.Interfaces;
using System.Data;
using System.Reflection;
using Utilities.DataTypes.Comparison;
using Utilities.SQL.MicroORM.Enums;
#endregion

namespace Utilities.SQL.MicroORM
{
    /// <summary>
    /// Class that acts as a mapping within the micro ORM
    /// </summary>
    /// <typeparam name="ClassType">Class type that this will accept</typeparam>
    public class Mapping<ClassType> : IMapping, IMapping<ClassType> where ClassType : class,new()
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Connection">Connection string</param>
        /// <param name="TableName">Table name</param>
        /// <param name="PrimaryKey">Primary key in the table</param>
        /// <param name="AutoIncrement">Is the primary key set to auto increment?</param>
        /// <param name="ParameterStarter">What the database expects as the
        /// parameter starting string ("@" for SQL Server, ":" for Oracle, etc.)</param>
        /// <param name="DbType">DbType for this connection</param>
        public Mapping(string Connection, string TableName, string PrimaryKey, bool AutoIncrement = true, string ParameterStarter = "@", string DbType = "System.Data.SqlClient")
        {
            Helper = new SQLHelper("", Connection, System.Data.CommandType.Text, DbType);
            Mappings = new TypeMapping<ClassType, SQLHelper>();
            ParameterNames = new List<string>();
            this.TableName = TableName;
            this.PrimaryKey = PrimaryKey;
            this.AutoIncrement = AutoIncrement;
            this.ParameterStarter = ParameterStarter;
        }

        /// <summary>
        /// Constructor (can be used if supplying own SQLHelper)
        /// </summary>
        /// <param name="TableName">Table name</param>
        /// <param name="PrimaryKey">Primary key</param>
        /// <param name="AutoIncrement">Is the primary key set to auto increment?</param>
        /// <param name="ParameterStarter">What the database expects as the
        /// parameter starting string ("@" for SQL Server, ":" for Oracle, etc.)</param>
        public Mapping(string TableName, string PrimaryKey, bool AutoIncrement = true, string ParameterStarter = "@")
        {
            Mappings = new TypeMapping<ClassType, SQLHelper>();
            ParameterNames = new List<string>();
            this.TableName = TableName;
            this.PrimaryKey = PrimaryKey;
            this.AutoIncrement = AutoIncrement;
            this.ParameterStarter = ParameterStarter;
        }

        /// <summary>
        /// Constructor (used internally to create instance versions of static mappings
        /// </summary>
        /// <param name="MappingToCopyFrom">Mapping to copy from</param>
        /// <param name="Helper">Helper class to use for queries</param>
        public Mapping(Mapping<ClassType> MappingToCopyFrom, SQLHelper Helper)
        {
            this.Mappings = MappingToCopyFrom.Mappings;
            this.TableName = MappingToCopyFrom.TableName;
            this.PrimaryKey = MappingToCopyFrom.PrimaryKey;
            this.AutoIncrement = MappingToCopyFrom.AutoIncrement;
            this.ParameterStarter = MappingToCopyFrom.ParameterStarter;
            this.ParameterNames = MappingToCopyFrom.ParameterNames;
            this.Helper = Helper;
            this.GetPrimaryKey = MappingToCopyFrom.GetPrimaryKey;
            this.PrimaryKeyMapping = MappingToCopyFrom.PrimaryKeyMapping;
        }

        #endregion

        #region Properties

        /// <summary>
        /// SQL Helper
        /// </summary>
        public virtual SQLHelper Helper { get; set; }

        /// <summary>
        /// Mapper used to map properties to SQLHelper
        /// </summary>
        public virtual TypeMapping<ClassType, SQLHelper> Mappings { get; set; }

        /// <summary>
        /// Table name
        /// </summary>
        protected virtual string TableName { get; set; }

        /// <summary>
        /// Primary key
        /// </summary>
        protected virtual string PrimaryKey { get; set; }

        /// <summary>
        /// Used to get/set primary key from an object
        /// </summary>
        protected virtual Mapping<ClassType, object> PrimaryKeyMapping { get; set; }

        /// <summary>
        /// Gets the primary key from an object
        /// </summary>
        protected virtual Func<ClassType, object> GetPrimaryKey { get; set; }

        /// <summary>
        /// Auto increment?
        /// </summary>
        protected virtual bool AutoIncrement { get; set; }

        /// <summary>
        /// Parameter starter
        /// </summary>
        protected virtual string ParameterStarter { get; set; }

        /// <summary>
        /// Parameter names
        /// </summary>
        public virtual List<string> ParameterNames { get; set; }

        #endregion

        #region Public Functions

        #region All

        /// <summary>
        /// Gets a list of all objects that meet the specified criteria
        /// </summary>
        /// <param name="Command">Command to use (can be an SQL string or stored procedure)</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Objects">Objects to modify/addon to (uses primary key to determine)</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <returns>A list of all objects that meet the specified criteria</returns>
        public virtual IEnumerable<ClassType> All(string Command, CommandType CommandType, IEnumerable<ClassType> Objects = null, params IParameter[] Parameters)
        {
            Check(Command, "Command");
            Check(Helper, "Helper");
            Check(Mappings, "Mappings");
            List<ClassType> Return = Objects == null ? new List<ClassType>() : Objects.ToList();
            SetupCommand(Command, CommandType, Parameters);
            Helper.ExecuteReader();
            while (Helper.Read())
            {
                bool Add = false;
                object CurrentKey = Helper.GetParameter(PrimaryKey, null);
                ClassType Temp = default(ClassType);
                if (Objects != null) Temp = Objects.FirstOrDefault(x => GetPrimaryKey(x).Equals(CurrentKey));
                if (Temp == default(ClassType))
                {
                    Temp = new ClassType();
                    Add = true;
                }
                Mappings.Copy(Helper, Temp);
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
        /// <param name="Parameters">Parameters to search by</param>
        /// <returns>A list of all objects that meet the specified criteria</returns>
        public virtual IEnumerable<ClassType> All(string Columns = "*", int Limit = 0, string OrderBy = "", IEnumerable<ClassType> Objects = null, params IParameter[] Parameters)
        {
            Check(Columns, "Columns");
            return All(SetupSelectCommand(Columns, Limit, OrderBy, Parameters), CommandType.Text, Objects, Parameters);
        }

        #endregion

        #region Any

        /// <summary>
        /// Gets a single object that fits the criteria
        /// </summary>
        /// <param name="Columns">Columns to select</param>
        /// <param name="ObjectToReturn">Object to return (in case the object needs to be created outside this,
        /// or default value is desired in case of nothing found)</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <returns>An object fitting the criteria specified or null if none are found</returns>
        public virtual ClassType Any(string Columns = "*", ClassType ObjectToReturn = null, params IParameter[] Parameters)
        {
            Check(Columns, "Columns");
            return Any(SetupSelectCommand(Columns, 1, "", Parameters), CommandType.Text, ObjectToReturn, Parameters);
        }

        /// <summary>
        /// Gets a single object that fits the criteria
        /// </summary>
        /// <param name="Command">Command to use (can be an SQL string or stored procedure name)</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="ObjectToReturn">Object to return (in case the object needs to be created outside this,
        /// or default value is desired in case of nothing found)</param>
        /// <param name="Parameters">Parameters used to search by</param>
        /// <returns>An object fitting the criteria specified or null if none are found</returns>
        public virtual ClassType Any(string Command, CommandType CommandType, ClassType ObjectToReturn = null, params IParameter[] Parameters)
        {
            Check(Mappings, "Mappings");
            Check(Command, "Command");
            Check(Helper, "Helper");
            SetupCommand(Command, CommandType, Parameters);
            Helper.ExecuteReader();
            if (Helper.Read())
            {
                ClassType Return = (ObjectToReturn == null) ? new ClassType() : ObjectToReturn;
                Mappings.Copy(Helper, Return);
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
        public virtual void Delete(string Command, CommandType CommandType, ClassType Object)
        {
            Check(Object, "Object");
            Check(Command, "Command");
            Check(Helper, "Helper");
            Check(Mappings, "Mappings");
            SetupCommand(Command, CommandType, null);
            Mappings.Copy(Object, Helper);
            Helper.ExecuteNonQuery();
        }

        /// <summary>
        /// Deletes an object from the database
        /// </summary>
        /// <param name="Object">Object to delete</param>
        public virtual void Delete(ClassType Object)
        {
            Delete(SetupDeleteCommand(), CommandType.Text, Object);
        }

        /// <summary>
        /// Deletes a list of objects from the database
        /// </summary>
        /// <param name="Objects">Objects to delete</param>
        public virtual void Delete(IEnumerable<ClassType> Objects)
        {
            string Command=SetupDeleteCommand();
            foreach (ClassType Object in Objects)
                Delete(Command, CommandType.Text, Object);
        }

        #endregion

        #region Insert

        /// <summary>
        /// Inserts an object based on the command specified
        /// </summary>
        /// <typeparam name="DataType">Data type expected to be returned from the query (to get the ID, etc.)</typeparam>
        /// <param name="Command">Command to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Object">Object to insert</param>
        /// <returns>The returned object from the query (usually the newly created row's ID)</returns>
        public virtual DataType Insert<DataType>(string Command, CommandType CommandType, ClassType Object, params IParameter[] Parameters)
        {
            Check(Object, "Object");
            Check(Command, "Command");
            Check(Helper, "Helper");
            Check(Mappings, "Mappings");
            SetupCommand(Command, CommandType, Parameters);
            Mappings.Copy(Object, Helper);
            return (DataType)Convert.ChangeType(Helper.ExecuteScalar(), typeof(DataType));
        }

        /// <summary>
        /// Inserts an object into the database
        /// </summary>
        /// <typeparam name="DataType">Data type expected (should be the same type as the primary key)</typeparam>
        /// <param name="Object">Object to insert</param>
        /// <returns>The returned object from the query (the newly created row's ID)</returns>
        public virtual DataType Insert<DataType>(ClassType Object, params IParameter[] Parameters)
        {
            return Insert<DataType>(SetupInsertCommand(Parameters), CommandType.Text, Object, Parameters);
        }

        #endregion

        #region Map

        public virtual IMapping<ClassType> Map<DataType>(Expression<Func<ClassType, DataType>> Property, string DatabasePropertyName="", Mode Mode = Mode.Read|Mode.Write)
        {
            Check(Property, "Property");
            Check(Mappings, "Mappings");
            if (string.IsNullOrEmpty(DatabasePropertyName))
                DatabasePropertyName = Utilities.Reflection.Reflection.GetPropertyName(Property);
            Expression Convert = Expression.Convert(Property.Body, typeof(object));
            Expression<Func<ClassType, object>> PropertyExpression = Expression.Lambda<Func<ClassType, object>>(Convert, Property.Parameters);
            Mappings.AddMapping(PropertyExpression,
                ((Mode & Mode.Read) == Mode.Read) ? new Func<SQLHelper, object>((x) => x.GetParameter(DatabasePropertyName, default(DataType))) : null,
                ((Mode & Mode.Write) == Mode.Write) ? new Action<SQLHelper, object>((x, y) => x.AddParameter(DatabasePropertyName, y)) : null);
            ParameterNames.Add(DatabasePropertyName);
            if (DatabasePropertyName == PrimaryKey)
            {
                PrimaryKeyMapping = new Mapping<ClassType, object>(PropertyExpression, x => x, (x, y) => x = y);
                GetPrimaryKey = PropertyExpression.Compile();
            }
            return this;
        }

        public virtual IMapping<ClassType> Map(Expression<Func<ClassType, string>> Property, string DatabasePropertyName="", int Length=64, Mode Mode = Mode.Read|Mode.Write)
        {
            Check(Property, "Property");
            Check(Mappings, "Mappings");
            if (string.IsNullOrEmpty(DatabasePropertyName))
                DatabasePropertyName = Utilities.Reflection.Reflection.GetPropertyName(Property);
            Expression Convert = Expression.Convert(Property.Body, typeof(object));
            Expression<Func<ClassType, object>> PropertyExpression = Expression.Lambda<Func<ClassType, object>>(Convert, Property.Parameters);
            Mappings.AddMapping(PropertyExpression,
                ((Mode & Mode.Read) == Mode.Read) ? new Func<SQLHelper, object>((x) => x.GetParameter(DatabasePropertyName, "")) : null,
                ((Mode & Mode.Write) == Mode.Write) ? new Action<SQLHelper, object>((x, y) => x.AddParameter(DatabasePropertyName, (string)y, Length)) : null);
            ParameterNames.Add(DatabasePropertyName);
            if (DatabasePropertyName == PrimaryKey)
            {
                PrimaryKeyMapping = new Mapping<ClassType, object>(PropertyExpression, x => x, (x, y) => x = y);
                GetPrimaryKey = PropertyExpression.Compile();
            }
            return this;
        }

        #endregion

        #region PageCount

        /// <summary>
        /// Gets the number of pages based on the specified 
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="Parameters">Parameters to search by</param>
        /// <returns>The number of pages that the table contains for the specified page size</returns>
        public virtual int PageCount(int PageSize = 25, params IParameter[] Parameters)
        {
            Check(Helper, "Helper");
            SetupCommand(SetupPageCountCommand(PageSize, Parameters), CommandType.Text, Parameters);
            Helper.ExecuteReader();
            if (Helper.Read())
            {
                int Total = Helper.GetParameter("Total", 0);
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
        /// <param name="Parameters">Parameters to search by</param>
        /// <returns>A list of objects that fit the specified criteria</returns>
        public virtual IEnumerable<ClassType> Paged(string Columns = "*", string OrderBy = "", int PageSize = 25, int CurrentPage = 0, IEnumerable<ClassType> Objects = null, params IParameter[] Parameters)
        {
            Check(Columns, "Columns");
            return All(SetupPagedCommand(Columns, OrderBy, PageSize, CurrentPage, Parameters), CommandType.Text, Objects, Parameters);
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
        public virtual void Save<PrimaryKeyType>(ClassType Object, params IParameter[] Parameters)
        {
            PrimaryKeyType PrimaryKeyVal = (PrimaryKeyType)GetPrimaryKey(Object);
            GenericEqualityComparer<PrimaryKeyType> Comparer = new GenericEqualityComparer<PrimaryKeyType>();
            if (Comparer.Equals(PrimaryKeyVal, default(PrimaryKeyType)))
            {
                PrimaryKeyVal = Insert<PrimaryKeyType>(Object, Parameters);
                PrimaryKeyMapping.CopyRightToLeft(PrimaryKeyVal, Object);
                return;
            }
            if (AutoIncrement)
            {
                Update(Object, Parameters);
                return;
            }
            Parameter<PrimaryKeyType> Param1 = new Parameter<PrimaryKeyType>(PrimaryKeyVal, PrimaryKey, ParameterStarter);
            ClassType TempVal = Any(PrimaryKey, null, Param1);
            if (TempVal == null)
            {
                PrimaryKeyVal = Insert<PrimaryKeyType>(Object, Parameters);
                PrimaryKeyMapping.CopyRightToLeft(PrimaryKeyVal, Object);
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
        public virtual void Save<PrimaryKeyType>(IEnumerable<ClassType> Objects, params IParameter[] Parameters)
        {
            foreach (ClassType Object in Objects)
            {
                Save<PrimaryKeyType>(Object, Parameters);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates an object in the database
        /// </summary>
        /// <param name="Command">Command to use</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Object">Object to update</param>
        public virtual void Update(string Command, CommandType CommandType, ClassType Object, params IParameter[] Parameters)
        {
            Check(Helper, "Helper");
            Check(Mappings, "Mappings");
            Check(Command, "Command");
            SetupCommand(Command, CommandType, Parameters);
            Mappings.Copy(Object, Helper);
            Helper.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates an object in the database
        /// </summary>
        /// <param name="Object">Object to update</param>
        public virtual void Update(ClassType Object, params IParameter[] Parameters)
        {
            Update(SetupUpdateCommand(Parameters), CommandType.Text, Object, Parameters);
        }

        #endregion

        #endregion

        #region Protected Functions

        #region Check

        /// <summary>
        /// Checks if an object is null, throwing an exception if it is
        /// </summary>
        /// <param name="Object">Object to check</param>
        /// <param name="Name">Parameter name</param>
        protected virtual void Check(object Object, string Name)
        {
            if (Object == null)
                throw new ArgumentNullException(Name);
        }

        /// <summary>
        /// Checks if a string is null/empty, throwing an exception if it is
        /// </summary>
        /// <param name="String">String to check</param>
        /// <param name="Name">Parameter name</param>
        protected virtual void Check(string String, string Name)
        {
            if (string.IsNullOrEmpty(String))
                throw new ArgumentNullException(Name);
        }

        #endregion

        #region SetupCommand

        /// <summary>
        /// Sets up a command
        /// </summary>
        /// <param name="Command">Command to add to the SQL Helper</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameter list</param>
        protected virtual void SetupCommand(string Command, CommandType CommandType, IParameter[] Parameters)
        {
            Check(Helper, "Helper");
            Check(Command, "Command");
            Helper.Command = Command;
            Helper.CommandType = CommandType;
            if (Parameters != null)
            {
                foreach (IParameter Parameter in Parameters)
                {
                    Parameter.AddParameter(Helper);
                }
            }
        }

        #endregion

        #region SetupDeleteCommand

        /// <summary>
        /// Sets up the delete command
        /// </summary>
        /// <returns>The command string</returns>
        protected virtual string SetupDeleteCommand()
        {
            return string.Format("DELETE FROM {0} WHERE {1}", TableName, PrimaryKey + "=" + ParameterStarter + PrimaryKey);
        }

        #endregion

        #region SetupInsertCommand

        /// <summary>
        /// Sets up the insert command
        /// </summary>
        /// <returns>The command string</returns>
        protected virtual string SetupInsertCommand(IParameter[] Parameters)
        {
            string ParameterList = "";
            string ValueList = "";
            string Splitter = "";
            foreach (string Name in ParameterNames)
            {
                if (!AutoIncrement || Name != PrimaryKey)
                {
                    ParameterList += Splitter + Name;
                    ValueList += Splitter + ParameterStarter + Name;
                    Splitter = ",";
                }
            }
            foreach (IParameter Parameter in Parameters)
            {
                ParameterList += Splitter + Parameter.ID;
                ValueList += Splitter + ParameterStarter + Parameter.ID;
                Splitter = ",";
            }
            return string.Format("INSERT INTO {0}({1}) VALUES({2}) SELECT scope_identity() as [ID]", TableName, ParameterList, ValueList);
        }

        #endregion

        #region SetupPageCountCommand

        /// <summary>
        /// Sets up the page count command
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="Parameters">Parameter list</param>
        /// <returns>The string command</returns>
        protected virtual string SetupPageCountCommand(int PageSize, IParameter[] Parameters)
        {
            string WhereCommand = "";
            if (Parameters != null && Parameters.Length > 0)
            {
                WhereCommand += " WHERE ";
                string Splitter = "";
                foreach (IParameter Parameter in Parameters)
                {
                    WhereCommand += Splitter + Parameter;
                    Splitter = " AND ";
                }
            }
            return string.Format("SELECT COUNT({0}) as Total FROM {1} {2}", PrimaryKey, TableName, WhereCommand);
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
        /// <returns>The command string</returns>
        protected virtual string SetupPagedCommand(string Columns, string OrderBy, int PageSize, int CurrentPage, IParameter[] Parameters)
        {
            if (string.IsNullOrEmpty(OrderBy))
                OrderBy = PrimaryKey;

            string WhereCommand = "";
            if (Parameters != null && Parameters.Length > 0)
            {
                WhereCommand += " WHERE ";
                string Splitter = "";
                foreach (IParameter Parameter in Parameters)
                {
                    WhereCommand += Splitter + Parameter;
                    Splitter = " AND ";
                }
            }
            string Command = string.Format("SELECT {0} FROM (SELECT ROW_NUMBER() OVER (ORDER BY {1}) AS Row, {0} FROM {2} {3}) AS Paged ", Columns, OrderBy, TableName, WhereCommand);
            int PageStart = CurrentPage * PageSize;
            Command += string.Format(" WHERE Row>{0} AND Row<={1}", PageStart, PageStart + PageSize);
            return Command;
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
        /// <returns>The string command</returns>
        protected virtual string SetupSelectCommand(string Columns, int Limit, string OrderBy, IParameter[] Parameters)
        {
            string Command = (Limit > 0 ? "SELECT TOP " + Limit : "SELECT") + " {0} FROM {1}";
            if (Parameters != null && Parameters.Length > 0)
            {
                Command += " WHERE ";
                string Splitter = "";
                foreach (IParameter Parameter in Parameters)
                {
                    Command += Splitter + Parameter;
                    Splitter = " AND ";
                }
            }
            if (!string.IsNullOrEmpty(OrderBy))
                Command += OrderBy.Trim().ToLower().StartsWith("order by", StringComparison.CurrentCultureIgnoreCase) ? " " + OrderBy : " ORDER BY " + OrderBy;
            return string.Format(Command, Columns, TableName);
        }

        #endregion

        #region SetupUpdateCommand

        /// <summary>
        /// Sets up the update command
        /// </summary>
        /// <returns>The command string</returns>
        protected virtual string SetupUpdateCommand(IParameter[] Parameters)
        {
            string ParameterList = "";
            string WhereCommand = PrimaryKey + "=" + ParameterStarter + PrimaryKey;
            string Splitter = "";
            foreach (string Name in ParameterNames)
            {
                if (Name != PrimaryKey)
                {
                    ParameterList += Splitter + Name + "=" + ParameterStarter + Name;
                    Splitter = ",";
                }
            }
            foreach (IParameter Parameter in Parameters)
            {
                ParameterList += Splitter + Parameter.ToString();
                Splitter = ",";
            }
            return string.Format("UPDATE {0} SET {1} WHERE {2}", TableName, ParameterList, WhereCommand);
        }

        #endregion

        #endregion

        #region IDisposable

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (Helper != null)
            {
                Helper = null;
            }
        }

        #endregion
    }
}