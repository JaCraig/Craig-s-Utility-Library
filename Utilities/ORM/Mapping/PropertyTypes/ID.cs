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
using Utilities.ORM.QueryProviders.Interfaces;
using Utilities.SQL.MicroORM;
using Utilities.SQL.MicroORM.Enums;
#endregion

namespace Utilities.ORM.Mapping.PropertyTypes
{
    /// <summary>
    /// ID class
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type</typeparam>
    public class ID<ClassType, DataType> : PropertyBase<ClassType, DataType, IID<ClassType, DataType>>,
        IID<ClassType, DataType>
        where ClassType : class,new()
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Expression">Expression pointing to the ID</param>
        public ID(Expression<Func<ClassType, DataType>> Expression)
            : base(Expression)
        {
            SetDefaultValue(() => default(DataType));
            SetFieldName(Name + "_");
        }

        #endregion

        #region Functions

        public override IID<ClassType, DataType> LoadUsingCommand(string Command, System.Data.CommandType CommandType)
        {
            this.CommandToLoad = new Command(Command, CommandType);
            return (IID<ClassType, DataType>)this;
        }

        public override void AddToQueryProvider(IDatabase Database, Mapping<ClassType> Mapping)
        {
            Mode Mode=Mode.Neither;
            if(Database.Readable)
                Mode|=Mode.Read;
            if(Database.Writable)
                Mode|=Mode.Write;
            Mapping.Map<DataType>(this.Expression, this.FieldName, DefaultValue(), Mode);
        }

        public override IID<ClassType, DataType> SetDefaultValue(Func<DataType> DefaultValue)
        {
            this.DefaultValue = DefaultValue;
            return (IID<ClassType, DataType>)this;
        }

        public override IID<ClassType, DataType> DoNotAllowNullValues()
        {
            this.NotNull = true;
            return (IID<ClassType, DataType>)this;
        }

        public override IID<ClassType, DataType> ThisShouldBeUnique()
        {
            this.Unique = true;
            return (IID<ClassType, DataType>)this;
        }

        public override IID<ClassType, DataType> TurnOnIndexing()
        {
            this.Index = true;
            return (IID<ClassType, DataType>)this;
        }

        public override IID<ClassType, DataType> TurnOnAutoIncrement()
        {
            this.AutoIncrement = true;
            return (IID<ClassType, DataType>)this;
        }

        public override IID<ClassType, DataType> SetFieldName(string FieldName)
        {
            this.FieldName = FieldName;
            return (IID<ClassType, DataType>)this;
        }


        public override IID<ClassType, DataType> SetTableName(string TableName)
        {
            this.TableName = TableName;
            return (IID<ClassType, DataType>)this;
        }

        public override IID<ClassType, DataType> TurnOnCascade()
        {
            this.Cascade = true;
            return (IID<ClassType, DataType>)this;
        }

        public override IID<ClassType, DataType> SetMaxLength(int MaxLength)
        {
            this.MaxLength = MaxLength;
            return (IID<ClassType, DataType>)this;
        }

        #endregion
    }
}