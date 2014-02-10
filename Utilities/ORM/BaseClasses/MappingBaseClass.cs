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

#region Usings

using System;
using System.Collections.Generic;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.Mapper.Default;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

#endregion Usings

namespace Utilities.ORM.BaseClasses
{
    /// <summary>
    /// Mapping base class
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DatabaseType">Database type</typeparam>
    public abstract class MappingBaseClass<ClassType, DatabaseType> : IMapping, IMapping<ClassType>
        where DatabaseType : IDatabase
        where ClassType : class,new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TableName">Table name</param>
        /// <param name="Suffix">Suffix used to define names of properties/table name</param>
        /// <param name="Prefix">Prefix used to define names of properties/table name</param>
        /// <param name="Order">
        /// The order in which the mappings are initialized (lower items are initialized prior to
        /// higher number items)
        /// </param>
        protected MappingBaseClass(string TableName = "", string Suffix = "_", string Prefix = "", int Order = 10)
        {
            if (string.IsNullOrEmpty(TableName))
                TableName = Prefix + typeof(ClassType).Name + Suffix;
            this.TableName = TableName;
            this.Suffix = Suffix;
            this.Prefix = Prefix;
            this.Properties = new List<IProperty>();
            this.IDProperties = new List<IProperty>();
            this.Order = Order;
        }

        /// <summary>
        /// Database config type
        /// </summary>
        public Type DatabaseConfigType { get { return typeof(DatabaseType); } }

        /// <summary>
        /// Delete command
        /// </summary>
        public string DeleteCommand { get; private set; }

        /// <summary>
        /// Delete command type
        /// </summary>
        public System.Data.CommandType DeleteCommandType { get; private set; }

        /// <summary>
        /// ID Properties
        /// </summary>
        public ICollection<IProperty> IDProperties { get; private set; }

        /// <summary>
        /// Insert command
        /// </summary>
        public string InsertCommand { get; private set; }

        /// <summary>
        /// Insert command type
        /// </summary>
        public System.Data.CommandType InsertCommandType { get; private set; }

        /// <summary>
        /// Object type
        /// </summary>
        public Type ObjectType { get { return typeof(ClassType); } }

        /// <summary>
        /// The order in which the mappings are initialized (lower items are initialized prior to
        /// higher number items)
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// Prefix used to define names of properties/table name
        /// </summary>
        public string Prefix { get; private set; }

        /// <summary>
        /// List of properties
        /// </summary>
        public ICollection<IProperty> Properties { get; private set; }

        /// <summary>
        /// Select all command
        /// </summary>
        public string SelectAllCommand { get; private set; }

        /// <summary>
        /// Select all command type
        /// </summary>
        public System.Data.CommandType SelectAllCommandType { get; private set; }

        /// <summary>
        /// Select any command
        /// </summary>
        public string SelectAnyCommand { get; private set; }

        /// <summary>
        /// Select any command type
        /// </summary>
        public System.Data.CommandType SelectAnyCommandType { get; private set; }

        /// <summary>
        /// Suffix used to define names of properties/table name
        /// </summary>
        public string Suffix { get; private set; }

        /// <summary>
        /// Table name
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Update command
        /// </summary>
        public string UpdateCommand { get; private set; }

        /// <summary>
        /// Update command type
        /// </summary>
        public System.Data.CommandType UpdateCommandType { get; private set; }

        /// <summary>
        /// Creates an ID object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>ID object</returns>
        public ID<ClassType, DataType> ID<DataType>(System.Linq.Expressions.Expression<Func<ClassType, DataType>> Expression)
        {
            if (Expression == null)
                throw new ArgumentNullException("Expression");
            ID<ClassType, DataType> ReturnValue = new ID<ClassType, DataType>(Expression, this);
            IDProperties.Add(ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// Creates a many to many object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to many object</returns>
        public ManyToMany<ClassType, DataType> ManyToMany<DataType>(System.Linq.Expressions.Expression<Func<ClassType, IEnumerable<DataType>>> Expression)
            where DataType : class, new()
        {
            if (Expression == null)
                throw new ArgumentNullException("Expression");
            ManyToMany<ClassType, DataType> ReturnValue = new ManyToMany<ClassType, DataType>(Expression, this);
            Properties.Add(ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// Creates a many to many object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to many object</returns>
        public ListManyToMany<ClassType, DataType> ManyToMany<DataType>(System.Linq.Expressions.Expression<Func<ClassType, List<DataType>>> Expression)
            where DataType : class, new()
        {
            if (Expression == null)
                throw new ArgumentNullException("Expression");
            ListManyToMany<ClassType, DataType> ReturnValue = new ListManyToMany<ClassType, DataType>(Expression, this);
            Properties.Add(ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// Creates a many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to one object</returns>
        public ManyToOne<ClassType, DataType> ManyToOne<DataType>(System.Linq.Expressions.Expression<Func<ClassType, DataType>> Expression)
            where DataType : class, new()
        {
            if (Expression == null)
                throw new ArgumentNullException("Expression");
            ManyToOne<ClassType, DataType> ReturnValue = new ManyToOne<ClassType, DataType>(Expression, this);
            Properties.Add(ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// Creates a many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to one object</returns>
        public IEnumerableManyToOne<ClassType, DataType> ManyToOne<DataType>(System.Linq.Expressions.Expression<Func<ClassType, IEnumerable<DataType>>> Expression)
            where DataType : class, new()
        {
            if (Expression == null)
                throw new ArgumentNullException("Expression");
            IEnumerableManyToOne<ClassType, DataType> ReturnValue = new IEnumerableManyToOne<ClassType, DataType>(Expression, this);
            Properties.Add(ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// Creates a many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to one object</returns>
        public ListManyToOne<ClassType, DataType> ManyToOne<DataType>(System.Linq.Expressions.Expression<Func<ClassType, List<DataType>>> Expression)
            where DataType : class, new()
        {
            if (Expression == null)
                throw new ArgumentNullException("Expression");
            ListManyToOne<ClassType, DataType> ReturnValue = new ListManyToOne<ClassType, DataType>(Expression, this);
            Properties.Add(ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// Creates a map object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The map object</returns>
        public Map<ClassType, DataType> Map<DataType>(System.Linq.Expressions.Expression<Func<ClassType, DataType>> Expression)
            where DataType : class, new()
        {
            if (Expression == null)
                throw new ArgumentNullException("Expression");
            Map<ClassType, DataType> ReturnValue = new Map<ClassType, DataType>(Expression, this);
            Properties.Add(ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// Creates a reference object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>A reference object</returns>
        public Reference<ClassType, DataType> Reference<DataType>(System.Linq.Expressions.Expression<Func<ClassType, DataType>> Expression)
        {
            if (Expression == null)
                throw new ArgumentNullException("Expression");
            Reference<ClassType, DataType> ReturnValue = new Reference<ClassType, DataType>(Expression, this);
            Properties.Add(ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// Sets the default delete command
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>This</returns>
        public IMapping SetDeleteCommand(string Command, System.Data.CommandType CommandType)
        {
            DeleteCommand = Command;
            DeleteCommandType = CommandType;
            return this;
        }

        /// <summary>
        /// Sets the default insert command
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>This</returns>
        public IMapping SetInsertCommand(string Command, System.Data.CommandType CommandType)
        {
            InsertCommand = Command;
            InsertCommandType = CommandType;
            return this;
        }

        /// <summary>
        /// Sets the default select all command
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>This</returns>
        public IMapping SetSelectAllCommand(string Command, System.Data.CommandType CommandType)
        {
            SelectAllCommand = Command;
            SelectAllCommandType = CommandType;
            return this;
        }

        /// <summary>
        /// Sets the default select any command
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>This</returns>
        public IMapping SetSelectAnyCommand(string Command, System.Data.CommandType CommandType)
        {
            SelectAnyCommand = Command;
            SelectAnyCommandType = CommandType;
            return this;
        }

        /// <summary>
        /// Sets up the mapping
        /// </summary>
        /// <param name="Source">Source info</param>
        /// <param name="QueryProvider">Query provider</param>
        public void Setup(ISourceInfo Source, Utilities.ORM.Manager.QueryProvider.Manager QueryProvider)
        {
            QueryProvider.Generate<ClassType>(Source, this)
                         .SetupCommands(this);
        }

        /// <summary>
        /// Sets the default update command
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>This</returns>
        public IMapping SetUpdateCommand(string Command, System.Data.CommandType CommandType)
        {
            UpdateCommand = Command;
            UpdateCommandType = CommandType;
            return this;
        }

        /// <summary>
        /// Converts the mapping to a string
        /// </summary>
        /// <returns>The table name</returns>
        public override string ToString()
        {
            return this.TableName;
        }
    }
}