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
    /// Map class
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type</typeparam>
    public class Map<ClassType, DataType> : PropertyBase<ClassType, DataType, IMap<ClassType, DataType>>,
        IMap<ClassType, DataType>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Expression">Expression pointing to the Map</param>
        public Map(Expression<Func<ClassType, DataType>> Expression)
            : base(Expression)
        {
            if (Utilities.Reflection.Reflection.IsIEnumerable(typeof(DataType)))
                throw new ArgumentException("Expression is an IEnumerable, use ManyToOne or ManyToMany instead");
            SetDefaultValue(() => default(DataType));
            SetFieldName(typeof(ClassType).Name + "_" + Name + "_ID");
        }

        #endregion

        #region Functions


        public override IMap<ClassType, DataType> SetDefaultValue(Func<DataType> DefaultValue)
        {
            this.DefaultValue = DefaultValue;
            return (IMap<ClassType, DataType>)this;
        }

        public override IMap<ClassType, DataType> DoNotAllowNullValues()
        {
            this.NotNull = true;
            return (IMap<ClassType, DataType>)this;
        }

        public override IMap<ClassType, DataType> ThisShouldBeUnique()
        {
            this.Unique = true;
            return (IMap<ClassType, DataType>)this;
        }

        public override IMap<ClassType, DataType> TurnOnIndexing()
        {
            this.Index = true;
            return (IMap<ClassType, DataType>)this;
        }

        public override IMap<ClassType, DataType> TurnOnAutoIncrement()
        {
            this.AutoIncrement = true;
            return (IMap<ClassType, DataType>)this;
        }

        public override IMap<ClassType, DataType> SetFieldName(string FieldName)
        {
            this.FieldName = FieldName;
            return (IMap<ClassType, DataType>)this;
        }


        public override IMap<ClassType, DataType> SetTableName(string TableName)
        {
            this.TableName = TableName;
            return (IMap<ClassType, DataType>)this;
        }

        public override IMap<ClassType, DataType> TurnOnCascade()
        {
            this.Cascade = true;
            return (IMap<ClassType, DataType>)this;
        }

        public override IMap<ClassType, DataType> SetMaxLength(int MaxLength)
        {
            this.MaxLength = MaxLength;
            return (IMap<ClassType, DataType>)this;
        }

        #endregion
    }
}