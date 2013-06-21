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
using Utilities.ORM.Mapping.Interfaces;
using Utilities.ORM.QueryProviders.Interfaces;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.SQL;
using Utilities.SQL.Interfaces;
using Utilities.SQL.MicroORM;
#endregion

namespace Utilities.ORM.Mapping.BaseClasses
{
    /// <summary>
    /// Property base class
    /// </summary>
    public abstract class PropertyBase<ClassType, DataType, ReturnType> : IProperty<ClassType, DataType, ReturnType>,
        IProperty<ClassType, DataType>, IProperty<ClassType>, IProperty where ClassType : class,new()
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Expression">Expression used to point to the property</param>
        /// <param name="Mapping">Mapping the StringID is added to</param>
        protected PropertyBase(Expression<Func<ClassType, DataType>> Expression, IMapping Mapping)
        {
            this.Expression = Expression;
            this.Name = Expression.GetPropertyName();
            this.Type = typeof(DataType);
            this.DerivedFieldName = "_" + Name + "Derived";
            this.Mapping = (IMapping)Mapping;
            this.CompiledExpression = this.Expression.Compile();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Sets the default value
        /// </summary>
        /// <param name="DefaultValue">Default value</param>
        /// <returns>This</returns>
        public abstract ReturnType SetDefaultValue(Func<DataType> DefaultValue);

        /// <summary>
        /// Prevents null values from being saved
        /// </summary>
        /// <returns>This</returns>
        public abstract ReturnType DoNotAllowNullValues();

        /// <summary>
        /// Should this be unique
        /// </summary>
        /// <returns>This</returns>
        public abstract ReturnType ThisShouldBeUnique();

        /// <summary>
        /// Turn on indexing
        /// </summary>
        /// <returns>This</returns>
        public abstract ReturnType TurnOnIndexing();

        /// <summary>
        /// Turns on auto increment
        /// </summary>
        /// <returns>This</returns>
        public abstract ReturnType TurnOnAutoIncrement();

        /// <summary>
        /// Sets the field name
        /// </summary>
        /// <param name="FieldName">Field name</param>
        /// <returns>This</returns>
        public abstract ReturnType SetFieldName(string FieldName);

        /// <summary>
        /// Sets the table name
        /// </summary>
        /// <param name="TableName">Table name</param>
        /// <returns>This</returns>
        public abstract ReturnType SetTableName(string TableName);

        /// <summary>
        /// Turn on cascade
        /// </summary>
        /// <returns>This</returns>
        public abstract ReturnType TurnOnCascade();

        /// <summary>
        /// Set max length
        /// </summary>
        /// <param name="MaxLength">Max length</param>
        /// <returns>This</returns>
        public abstract ReturnType SetMaxLength(int MaxLength);

        /// <summary>
        /// Adds to query provider
        /// </summary>
        /// <param name="Database">Database</param>
        /// <param name="Mapping">Mapping</param>
        public abstract void AddToQueryProvider(IDatabase Database, Mapping<ClassType> Mapping);

        /// <summary>
        /// Loads the object using command
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>This</returns>
        public abstract ReturnType LoadUsingCommand(string Command, System.Data.CommandType CommandType);

        /// <summary>
        /// Gets it as a parameter
        /// </summary>
        /// <param name="Object">Object</param>
        /// <returns>The property as a parameter</returns>
        public abstract IParameter GetAsParameter(ClassType Object);

        /// <summary>
        /// Gets it as an object
        /// </summary>
        /// <param name="Object">Object</param>
        /// <returns>The property as an object</returns>
        public abstract object GetAsObject(ClassType Object);

        /// <summary>
        /// Gets it as an object
        /// </summary>
        /// <param name="Object">Object</param>
        /// <returns>The property as an object</returns>
        public object GetAsObject(object Object)
        {
            return GetAsObject((ClassType)Object);
        }

        /// <summary>
        /// Cascade save
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">ORM object</param>
        public abstract void CascadeSave(ClassType Object, SQLHelper MicroORM);

        /// <summary>
        /// Cascade delete
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">ORM object</param>
        public abstract void CascadeDelete(ClassType Object, SQLHelper MicroORM);

        /// <summary>
        /// Cascade join delete
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">ORM object</param>
        /// <returns>List of commands</returns>
        public abstract IEnumerable<Command> CascadeJoinsDelete(ClassType Object, SQLHelper MicroORM);

        /// <summary>
        /// Cascade join save
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">ORM object</param>
        /// <returns>List of commands</returns>
        public abstract IEnumerable<Command> CascadeJoinsSave(ClassType Object, SQLHelper MicroORM);

        /// <summary>
        /// Join delete
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">ORM object</param>
        /// <returns>List of commands</returns>
        public abstract IEnumerable<Command> JoinsDelete(ClassType Object, SQLHelper MicroORM);

        /// <summary>
        /// Join save
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">ORM object</param>
        /// <returns>List of commands</returns>
        public abstract IEnumerable<Command> JoinsSave(ClassType Object, SQLHelper MicroORM);

        /// <summary>
        /// Setup load commands
        /// </summary>
        public abstract void SetupLoadCommands();

        #endregion

        #region Properties

        /// <summary>
        /// Default value
        /// </summary>
        public virtual Func<DataType> DefaultValue { get; protected set; }

        /// <summary>
        /// Name of the property
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Property type
        /// </summary>
        public virtual Type Type { get; protected set; }

        /// <summary>
        /// Expression used to get the property
        /// </summary>
        public virtual Expression<Func<ClassType, DataType>> Expression { get; protected set; }

        /// <summary>
        /// Derived field name
        /// </summary>
        public virtual string DerivedFieldName { get; protected set; }

        /// <summary>
        /// Not null
        /// </summary>
        public virtual bool NotNull { get; protected set; }

        /// <summary>
        /// Unique
        /// </summary>
        public virtual bool Unique { get; protected set; }

        /// <summary>
        /// Auto increment
        /// </summary>
        public virtual bool AutoIncrement { get; protected set; }

        /// <summary>
        /// Cascade
        /// </summary>
        public virtual bool Cascade { get; protected set; }

        /// <summary>
        /// Field name
        /// </summary>
        public virtual string FieldName { get; protected set; }

        /// <summary>
        /// Index
        /// </summary>
        public virtual bool Index { get; protected set; }

        /// <summary>
        /// Max length
        /// </summary>
        public virtual int MaxLength { get; protected set; }

        /// <summary>
        /// Table name
        /// </summary>
        public virtual string TableName { get; protected set; }

        /// <summary>
        /// Foreign key
        /// </summary>
        public virtual IMapping ForeignKey { get; set; }

        /// <summary>
        /// Command to load
        /// </summary>
        public virtual Command CommandToLoad { get; protected set; }

        /// <summary>
        /// Mapping
        /// </summary>
        public virtual IMapping Mapping { get; protected set; }

        /// <summary>
        /// Compiled expression
        /// </summary>
        public virtual Func<ClassType, DataType> CompiledExpression { get; protected set; }

        #endregion
    }
}