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
using System.Linq.Expressions;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.ORM.Mapping.Interfaces;
using Utilities.ORM.Mapping.PropertyTypes;
using Utilities.ORM.QueryProviders.Interfaces;
using Utilities.SQL.MicroORM;
using Utilities.Validation;
using Utilities.SQL;
#endregion

namespace Utilities.ORM.Mapping
{
    /// <summary>
    /// Class mapping
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DatabaseType">Database type</typeparam>
    public abstract class Mapping<ClassType, DatabaseType> : IMapping<ClassType>, IMapping
        where DatabaseType : IDatabase
        where ClassType : class,new()
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="TableName">Table name</param>
        /// <param name="Suffix">Suffix used to define names of properties/table name</param>
        /// <param name="Prefix">Prefix used to define names of properties/table name</param>
        public Mapping(string TableName = "", string Suffix = "_", string Prefix = "")
        {
            if (!string.IsNullOrEmpty(TableName))
                this.TableName = TableName;
            this.Suffix = Suffix;
            this.Prefix = Prefix;
            Setup();
            SetupValidation(ValidationManager.GetValidator<ClassType>());
        }

        #endregion

        #region Functions

        #region AddToQueryProvider

        /// <summary>
        /// Add to query provider
        /// </summary>
        /// <param name="Database">Database object</param>
        public void AddToQueryProvider(IDatabase Database)
        {
            Mapping<ClassType> Map = MicroORM.Map<ClassType>(TableName, IDProperty.Chain(x => x.FieldName), IDProperty.Chain(x => x.AutoIncrement), Database.Chain(x => x.ParameterStarter), Database.Chain(x => x.Name));
            ((IProperty<ClassType>)IDProperty).Chain(x => x.AddToQueryProvider(Database, Map));
            foreach (IProperty Property in Properties)
                ((IProperty<ClassType>)Property).AddToQueryProvider(Database, Map);
        }

        #endregion

        #region All

        /// <summary>
        /// Sets a command for default all function
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        public virtual void All(string Command, System.Data.CommandType CommandType)
        {
            AllCommand = new Command(Command, CommandType);
        }

        #endregion

        #region Any

        /// <summary>
        /// Sets a command for default any function
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        public virtual void Any(string Command, System.Data.CommandType CommandType)
        {
            AnyCommand = new Command(Command, CommandType);
        }

        #endregion

        #region Setup

        /// <summary>
        /// Sets up the mapping
        /// </summary>
        private void Setup()
        {
            if (Properties == null)
                Properties = new List<IProperty>();
        }

        #endregion

        #region ID

        /// <summary>
        /// Creates an ID object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>ID object</returns>
        public IID<ClassType, DataType> ID<DataType>(Expression<Func<ClassType, DataType>> Expression)
        {
            if (IDProperty != null)
                throw new NotSupportedException("Multiple IDs are not currently supported");
            Setup();
            ID<ClassType, DataType> Return = new ID<ClassType, DataType>(Expression, this);
            IDProperty = Return;
            return Return;
        }

        /// <summary>
        /// Creates an ID object
        /// </summary>
        /// <param name="Expression">Expression</param>
        /// <returns>ID object</returns>
        public IID<ClassType, string> ID(Expression<Func<ClassType, string>> Expression)
        {
            if (IDProperty != null)
                throw new NotSupportedException("Multiple IDs are not currently supported");
            Setup();
            StringID<ClassType> Return = new StringID<ClassType>(Expression, this);
            IDProperty = Return;
            return Return;
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Should be overwritten to initialize values in the 
        /// database. This is run after the initial setup but prior to
        /// returning to the user.
        /// </summary>
        public virtual void Initialize() { }

        #endregion

        #region Reference

        /// <summary>
        /// Creates a reference object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>A reference object</returns>
        public IReference<ClassType, DataType> Reference<DataType>(Expression<Func<ClassType, DataType>> Expression)
        {
            Setup();
            Reference<ClassType, DataType> Return = new Reference<ClassType, DataType>(Expression, this);
            Properties.Add(Return);
            return Return;
        }

        /// <summary>
        /// Creates a reference object
        /// </summary>
        /// <param name="Expression">Expression</param>
        /// <returns>A reference object</returns>
        public IReference<ClassType, string> Reference(Expression<Func<ClassType, string>> Expression)
        {
            Setup();
            StringReference<ClassType> Return = new StringReference<ClassType>(Expression, this);
            Properties.Add(Return);
            return Return;
        }

        #endregion

        #region Map

        /// <summary>
        /// Creates a map object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The map object</returns>
        public IMap<ClassType, DataType> Map<DataType>(Expression<Func<ClassType, DataType>> Expression) where DataType : class,new()
        {
            Setup();
            Map<ClassType, DataType> Return = new Map<ClassType, DataType>(Expression, this);
            Properties.Add(Return);
            return Return;
        }

        #endregion

        #region ManyToOne

        /// <summary>
        /// Creates a many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to one object</returns>
        public IManyToOne<ClassType, DataType> ManyToOne<DataType>(Expression<Func<ClassType, DataType>> Expression) where DataType : class,new()
        {
            Setup();
            ManyToOne<ClassType, DataType> Return = new ManyToOne<ClassType, DataType>(Expression, this);
            Properties.Add(Return);
            return Return;
        }

        /// <summary>
        /// Creates a many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to one object</returns>
        public IIEnumerableManyToOne<ClassType, DataType> ManyToOne<DataType>(Expression<Func<ClassType, IEnumerable<DataType>>> Expression) where DataType : class,new()
        {
            Setup();
            IEnumerableManyToOne<ClassType, DataType> Return = new IEnumerableManyToOne<ClassType, DataType>(Expression, this);
            Properties.Add(Return);
            return Return;
        }

        /// <summary>
        /// Creates a many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to one object</returns>
        public IListManyToOne<ClassType, DataType> ManyToOne<DataType>(Expression<Func<ClassType, List<DataType>>> Expression) where DataType : class,new()
        {
            Setup();
            ListManyToOne<ClassType, DataType> Return = new ListManyToOne<ClassType, DataType>(Expression, this);
            Properties.Add(Return);
            return Return;
        }

        #endregion

        #region ManyToMany

        /// <summary>
        /// Creates a many to many object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to many object</returns>
        public IManyToMany<ClassType, DataType> ManyToMany<DataType>(Expression<Func<ClassType, IEnumerable<DataType>>> Expression) where DataType : class,new()
        {
            Setup();
            ManyToMany<ClassType, DataType> Return = new ManyToMany<ClassType, DataType>(Expression, this);
            Properties.Add(Return);
            return Return;
        }

        /// <summary>
        /// Creates a many to many object
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression</param>
        /// <returns>The many to many object</returns>
        public IListManyToMany<ClassType, DataType> ManyToMany<DataType>(Expression<Func<ClassType, List<DataType>>> Expression) where DataType : class,new()
        {
            Setup();
            ListManyToMany<ClassType, DataType> Return = new ListManyToMany<ClassType, DataType>(Expression, this);
            Properties.Add(Return);
            return Return;
        }

        #endregion

        #region SetupValidation

        /// <summary>
        /// Used to set up validation, using the class used internally by the system
        /// </summary>
        /// <param name="Validator">Validator</param>
        public virtual void SetupValidation(Validator<ClassType> Validator) { }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Table name
        /// </summary>
        public virtual string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(_TableName))
                {
                    return Prefix + typeof(ClassType).Name + Suffix;
                }
                return _TableName;
            }
            private set { _TableName = value; }
        }

        private string _TableName = "";

        /// <summary>
        /// Suffix used
        /// </summary>
        public virtual string Suffix { get; private set; }

        /// <summary>
        /// Prefix used
        /// </summary>
        public virtual string Prefix { get; private set; }

        /// <summary>
        /// Database config type
        /// </summary>
        public virtual Type DatabaseConfigType { get { return typeof(DatabaseType); } }

        /// <summary>
        /// List of properties
        /// </summary>
        public virtual List<IProperty> Properties { get; private set; }

        /// <summary>
        /// ID property
        /// </summary>
        public virtual IProperty IDProperty { get; set; }

        /// <summary>
        /// Mapping manager
        /// </summary>
        public virtual IMappingManager Manager { get; set; }

        /// <summary>
        /// Object type
        /// </summary>
        public virtual Type ObjectType { get { return typeof(ClassType); } }

        /// <summary>
        /// Default any command
        /// </summary>
        public virtual Command AnyCommand { get; set; }

        /// <summary>
        /// Default all command
        /// </summary>
        public virtual Command AllCommand { get; set; }

        /// <summary>
        /// The order in which the mappings are initialized
        /// </summary>
        public virtual int Order { get; set; }

        #endregion
    }
}