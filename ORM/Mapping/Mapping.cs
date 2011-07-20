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
using Utilities.ORM.Mapping.Interfaces;
using System.Linq.Expressions;
using Utilities.ORM.Mapping.PropertyTypes;
using Utilities.ORM.QueryProviders.Interfaces;
using Utilities.SQL.MicroORM;
#endregion

namespace Utilities.ORM.Mapping
{
    /// <summary>
    /// Class mapping
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DatabaseType">Database type</typeparam>
    public class Mapping<ClassType, DatabaseType> : IMapping<ClassType>, IMapping
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
        }

        #endregion

        #region Functions

        #region AddToQueryProvider

        public void AddToQueryProvider(IDatabase Database)
        {
            Mapping<ClassType> Map = MicroORM.Map<ClassType>(TableName, IDProperty.FieldName, IDProperty.AutoIncrement, Database.ParameterStarter, Database.Name);
            ((IProperty<ClassType>)IDProperty).AddToQueryProvider(Database, Map);
            foreach (IProperty Property in Properties)
                ((IProperty<ClassType>)Property).AddToQueryProvider(Database, Map);
        }

        #endregion

        #region All

        public virtual void All(string Command, System.Data.CommandType CommandType)
        {
            AllCommand = new Command(Command, CommandType);
        }

        #endregion

        #region Any

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

        public IID<ClassType, DataType> ID<DataType>(Expression<Func<ClassType, DataType>> Expression)
        {
            if (IDProperty != null)
                throw new NotSupportedException("Multiple IDs are not currently supported");
            Setup();
            ID<ClassType, DataType> Return = new ID<ClassType, DataType>(Expression,this);
            IDProperty = Return;
            return Return;
        }

        public IID<ClassType, string> ID(Expression<Func<ClassType, string>> Expression)
        {
            if (IDProperty != null)
                throw new NotSupportedException("Multiple IDs are not currently supported");
            Setup();
            StringID<ClassType> Return = new StringID<ClassType>(Expression,this);
            IDProperty = Return;
            return Return;
        }

        #endregion

        #region Reference

        public IReference<ClassType, DataType> Reference<DataType>(Expression<Func<ClassType, DataType>> Expression)
        {
            Setup();
            Reference<ClassType, DataType> Return = new Reference<ClassType, DataType>(Expression,this);
            Properties.Add(Return);
            return Return;
        }

        public IReference<ClassType, string> Reference(Expression<Func<ClassType, string>> Expression)
        {
            Setup();
            StringReference<ClassType> Return = new StringReference<ClassType>(Expression,this);
            Properties.Add(Return);
            return Return;
        }

        #endregion

        #region Map

        public IMap<ClassType, DataType> Map<DataType>(Expression<Func<ClassType, DataType>> Expression) where DataType : class,new()
        {
            Setup();
            Map<ClassType, DataType> Return = new Map<ClassType, DataType>(Expression, this);
            Properties.Add(Return);
            return Return;
        }

        #endregion

        #region ManyToOne

        public IManyToOne<ClassType, DataType> ManyToOne<DataType>(Expression<Func<ClassType, DataType>> Expression) where DataType : class,new()
        {
            Setup();
            ManyToOne<ClassType, DataType> Return = new ManyToOne<ClassType, DataType>(Expression,this);
            Properties.Add(Return);
            return Return;
        }

        public IIEnumerableManyToOne<ClassType, DataType> ManyToOne<DataType>(Expression<Func<ClassType, IEnumerable<DataType>>> Expression) where DataType : class,new()
        {
            Setup();
            IEnumerableManyToOne<ClassType, DataType> Return = new IEnumerableManyToOne<ClassType, DataType>(Expression,this);
            Properties.Add(Return);
            return Return;
        }

        #endregion

        #region ManyToMany

        public IManyToMany<ClassType, DataType> ManyToMany<DataType>(Expression<Func<ClassType, IEnumerable<DataType>>> Expression) where DataType : class,new()
        {
            Setup();
            ManyToMany<ClassType, DataType> Return = new ManyToMany<ClassType, DataType>(Expression, this);
            Properties.Add(Return);
            return Return;
        }

        #endregion

        #endregion

        #region Properties

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
        public virtual string Suffix { get; private set; }
        public virtual string Prefix { get; private set; }
        public virtual Type DatabaseConfigType { get { return typeof(DatabaseType); } }
        public virtual List<IProperty> Properties { get; private set; }
        public virtual IProperty IDProperty { get; set; }
        public virtual IMappingManager Manager { get; set; }
        public virtual Type ObjectType { get {return typeof(ClassType); } }
        public virtual Command AnyCommand { get; set; }
        public virtual Command AllCommand { get; set; }

        #endregion
    }
}