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
using Utilities.ORM.QueryProviders.Interfaces;
using Utilities.SQL.MicroORM;
using System.Collections;
using Utilities.Reflection.ExtensionMethods;
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
        /// <param name="DerivedFieldName">Derived field name</param>
        public PropertyBase(Expression<Func<ClassType, DataType>> Expression, IMapping Mapping)
        {
            this.Expression = Expression;
            this.Name = Expression.GetPropertyName<ClassType, DataType>();
            this.Type = typeof(DataType);
            this.DerivedFieldName = "_" + Name + "Derived";
            this.Mapping = (IMapping)Mapping;
            this.CompiledExpression = this.Expression.Compile();
        }

        #endregion

        #region Functions

        public abstract ReturnType SetDefaultValue(Func<DataType> DefaultValue);
        public abstract ReturnType DoNotAllowNullValues();
        public abstract ReturnType ThisShouldBeUnique();
        public abstract ReturnType TurnOnIndexing();
        public abstract ReturnType TurnOnAutoIncrement();
        public abstract ReturnType SetFieldName(string FieldName);
        public abstract ReturnType SetTableName(string TableName);
        public abstract ReturnType TurnOnCascade();
        public abstract ReturnType SetMaxLength(int MaxLength);
        public abstract void AddToQueryProvider(IDatabase Database, Mapping<ClassType> Mapping);
        public abstract ReturnType LoadUsingCommand(string Command, System.Data.CommandType CommandType);
        public abstract IParameter GetAsParameter(ClassType Object);
        public abstract void CascadeSave(ClassType Object, MicroORM MicroORM);
        public abstract void CascadeDelete(ClassType Object, MicroORM MicroORM);
        public abstract void CascadeJoinsDelete(ClassType Object, MicroORM MicroORM);
        public abstract void CascadeJoinsSave(ClassType Object, MicroORM MicroORM);
        public abstract void JoinsDelete(ClassType Object, MicroORM MicroORM);
        public abstract void JoinsSave(ClassType Object, MicroORM MicroORM);
        public abstract void SetupLoadCommands();

        #endregion

        #region Properties

        public virtual Func<DataType> DefaultValue { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual Type Type { get; protected set; }
        public virtual Expression<Func<ClassType, DataType>> Expression { get; protected set; }
        public virtual string DerivedFieldName { get; protected set; }
        public virtual bool NotNull { get; protected set; }
        public virtual bool Unique { get; protected set; }
        public virtual bool AutoIncrement { get; protected set; }
        public virtual bool Cascade { get; protected set; }
        public virtual string FieldName { get; protected set; }
        public virtual bool Index { get; protected set; }
        public virtual int MaxLength { get; protected set; }
        public virtual string TableName { get; protected set; }
        public virtual IMapping ForeignKey { get; set; }
        public virtual Command CommandToLoad { get; protected set; }
        public virtual IMapping Mapping { get; protected set; }
        public virtual Func<ClassType, DataType> CompiledExpression { get; protected set; }

        #endregion
    }
}