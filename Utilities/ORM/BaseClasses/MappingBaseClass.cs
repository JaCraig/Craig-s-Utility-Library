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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Utilities.DataTypes;
using Utilities.DataTypes.Patterns.BaseClasses;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.Mapper.Default;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.Schema.Interfaces;

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
        protected MappingBaseClass(string TableName = "", string Suffix = "_", string Prefix = "")
        {
            if (string.IsNullOrEmpty(TableName))
                TableName = Prefix + typeof(ClassType).Name + Suffix;
            this.TableName = TableName;
            this.Suffix = Suffix;
            this.Prefix = Prefix;
            this.Properties = new List<IProperty>();
        }

        /// <summary>
        /// Database config type
        /// </summary>
        public Type DatabaseConfigType { get { return typeof(ClassType); } }

        /// <summary>
        /// ID Property
        /// </summary>
        public IProperty IDProperty { get; private set; }

        /// <summary>
        /// Mapping manager
        /// </summary>
        public Manager.Mapper.Manager Manager { get; private set; }

        /// <summary>
        /// Object type
        /// </summary>
        public Type ObjectType { get { return typeof(ClassType); } }

        /// <summary>
        /// The order in which the mappings are initialized
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
        /// Suffix used to define names of properties/table name
        /// </summary>
        public string Suffix { get; private set; }

        /// <summary>
        /// Table name
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Creates an ID object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>ID object</returns>
        public ID<ClassType, DataType> ID<DataType>(System.Linq.Expressions.Expression<Func<ClassType, DataType>> Expression)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a many to many object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to many object</returns>
        public ManyToMany<ClassType, DataType> ManyToMany<DataType>(System.Linq.Expressions.Expression<Func<ClassType, IEnumerable<DataType>>> Expression) where DataType : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a many to many object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to many object</returns>
        public ListManyToMany<ClassType, DataType> ManyToMany<DataType>(System.Linq.Expressions.Expression<Func<ClassType, List<DataType>>> Expression) where DataType : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to one object</returns>
        public ManyToOne<ClassType, DataType> ManyToOne<DataType>(System.Linq.Expressions.Expression<Func<ClassType, DataType>> Expression) where DataType : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to one object</returns>
        public IEnumerableManyToOne<ClassType, DataType> ManyToOne<DataType>(System.Linq.Expressions.Expression<Func<ClassType, IEnumerable<DataType>>> Expression) where DataType : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to one object</returns>
        public ListManyToOne<ClassType, DataType> ManyToOne<DataType>(System.Linq.Expressions.Expression<Func<ClassType, List<DataType>>> Expression) where DataType : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a map object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The map object</returns>
        public Map<ClassType, DataType> Map<DataType>(System.Linq.Expressions.Expression<Func<ClassType, DataType>> Expression) where DataType : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a reference object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>A reference object</returns>
        public Reference<ClassType, DataType> Reference<DataType>(System.Linq.Expressions.Expression<Func<ClassType, DataType>> Expression)
        {
            throw new NotImplementedException();
        }
    }
}