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
using Utilities.ORM.Mapping.BaseClasses;
#endregion

namespace Utilities.ORM.Mapping.PropertyTypes
{
    /// <summary>
    /// Many to one class
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type</typeparam>
    public class IEnumerableManyToOne<ClassType, DataType> : PropertyBase<ClassType, IEnumerable<DataType>, IIEnumerableManyToOne<ClassType, DataType>>,
        IIEnumerableManyToOne<ClassType, DataType>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Expression">Expression pointing to the many to one</param>
        public IEnumerableManyToOne(Expression<Func<ClassType, IEnumerable<DataType>>> Expression)
            : base(Expression)
        {
            SetDefaultValue(() => new List<DataType>());
            string Class1 = typeof(ClassType).Name;
            string Class2 = typeof(DataType).Name;
            if (Class1.CompareTo(Class2) < 0)
                SetTableName(Class1 + "_" + Class2);
            else
                SetTableName(Class2 + "_" + Class1);
        }

        #endregion

        #region Functions

        public override IIEnumerableManyToOne<ClassType, DataType> SetDefaultValue(Func<IEnumerable<DataType>> DefaultValue)
        {
            this.DefaultValue = DefaultValue;
            return (IIEnumerableManyToOne<ClassType, DataType>)this;
        }

        public override IIEnumerableManyToOne<ClassType, DataType> DoNotAllowNullValues()
        {
            this.NotNull = true;
            return (IIEnumerableManyToOne<ClassType, DataType>)this;
        }

        public override IIEnumerableManyToOne<ClassType, DataType> ThisShouldBeUnique()
        {
            this.Unique = true;
            return (IIEnumerableManyToOne<ClassType, DataType>)this;
        }

        public override IIEnumerableManyToOne<ClassType, DataType> TurnOnIndexing()
        {
            this.Index = true;
            return (IIEnumerableManyToOne<ClassType, DataType>)this;
        }

        public override IIEnumerableManyToOne<ClassType, DataType> TurnOnAutoIncrement()
        {
            this.AutoIncrement = true;
            return (IIEnumerableManyToOne<ClassType, DataType>)this;
        }

        public override IIEnumerableManyToOne<ClassType, DataType> SetFieldName(string FieldName)
        {
            this.FieldName = FieldName;
            return (IIEnumerableManyToOne<ClassType, DataType>)this;
        }


        public override IIEnumerableManyToOne<ClassType, DataType> SetTableName(string TableName)
        {
            this.TableName = TableName;
            return (IIEnumerableManyToOne<ClassType, DataType>)this;
        }

        public override IIEnumerableManyToOne<ClassType, DataType> TurnOnCascade()
        {
            this.Cascade = true;
            return (IIEnumerableManyToOne<ClassType, DataType>)this;
        }

        public override IIEnumerableManyToOne<ClassType, DataType> SetMaxLength(int MaxLength)
        {
            this.MaxLength = MaxLength;
            return (IIEnumerableManyToOne<ClassType, DataType>)this;
        }

        #endregion
    }
}