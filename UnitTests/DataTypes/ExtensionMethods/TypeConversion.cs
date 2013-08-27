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

using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using Utilities.DataTypes;
using Xunit;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class TypeConversion
    {
        [Fact]
        public void TypeToDbType()
        {
            Assert.Equal(DbType.Int32, typeof(int).To(DbType.Int32));
            Assert.Equal(DbType.String, typeof(string).To(DbType.Int32));
            Assert.Equal(DbType.Single, typeof(float).To(DbType.Int32));
            Assert.Equal(DbType.Int32, typeof(MyEnumTest).To(DbType.Int32));
        }

        [Fact]
        public void TypeToSqlDbType()
        {
            Assert.Equal(SqlDbType.Int, typeof(int).To(SqlDbType.Int));
            Assert.Equal(SqlDbType.NVarChar, typeof(string).To(SqlDbType.Int));
            Assert.Equal(SqlDbType.Real, typeof(float).To(SqlDbType.Int));
            Assert.Equal(SqlDbType.Int, typeof(MyEnumTest).To(SqlDbType.Int));
        }

        [Fact]
        public void DbTypeToType()
        {
            Assert.Equal(typeof(int), DbType.Int32.To(typeof(int)));
            Assert.Equal(typeof(string), DbType.String.To(typeof(int)));
            Assert.Equal(typeof(float), DbType.Single.To(typeof(int)));
        }

        [Fact]
        public void SqlDbTypeToType()
        {
            Assert.Equal(typeof(int), SqlDbType.Int.To(typeof(int)));
            Assert.Equal(typeof(string), SqlDbType.NVarChar.To(typeof(int)));
            Assert.Equal(typeof(float), SqlDbType.Real.To(typeof(int)));
        }

        [Fact]
        public void DbTypeToSqlDbType()
        {
            Assert.Equal(SqlDbType.Int, DbType.Int32.To(SqlDbType.Int));
            Assert.Equal(SqlDbType.NVarChar, DbType.String.To(SqlDbType.Int));
            Assert.Equal(SqlDbType.Real, DbType.Single.To(SqlDbType.Int));
        }

        [Fact]
        public void SqlDbTypeToDbType()
        {
            Assert.Equal(DbType.Int32, SqlDbType.Int.To(DbType.Int32));
            Assert.Equal(DbType.String, SqlDbType.NVarChar.To(DbType.Int32));
            Assert.Equal(DbType.Single, SqlDbType.Real.To(DbType.Int32));
        }

        [Fact]
        public void FormatToString()
        {
            object TestObject = new DateTime(1999, 1, 1);
            Assert.Equal("January 1, 1999", TestObject.FormatToString("MMMM d, yyyy"));
        }

        [Fact]
        public void TryConvert()
        {
            Assert.Equal(1, (1.0f).To(0));
            Assert.Equal("2011", (2011).To(""));
            Assert.NotNull(new MyTestClass().To<MyTestClass, IMyTestClass>());
            Assert.NotNull(((object)new MyTestClass()).To<object, IMyTestClass>());
        }

        [Fact]
        public void ToList()
        {
            List<PreDataTable> Temp = new PreDataTable[] { new PreDataTable { ID = 1, Value = "A" }, new PreDataTable { ID = 2, Value = "B" }, new PreDataTable { ID = 3, Value = "C" } }.ToList();
            List<PreDataTable> Temp2 = Temp.To().To<PreDataTable>(() => new PreDataTable());
            Assert.Equal(Temp, Temp2);
        }

        [Fact]
        public void ToExpando()
        {
            MyTestClass TestObject = new MyTestClass();
            ExpandoObject Object = TestObject.To<MyTestClass, ExpandoObject>();
            Assert.Equal(10, ((IDictionary<string, object>)Object)["B"]);
            ((IDictionary<string, object>)Object)["B"] = 20;
            Assert.Equal(20, Object.To(new MyTestClass()).B);
        }
    }

    public enum MyEnumTest
    {
        Item1,
        Item2,
        Item3
    }

    public class MyTestClass:IMyTestClass
    {
        public MyTestClass() { B = 10; }
        public virtual MyTestClass A { get; set; }
        public virtual int B { get; set; }
    }

    public interface IMyTestClass
    {
    }
}
