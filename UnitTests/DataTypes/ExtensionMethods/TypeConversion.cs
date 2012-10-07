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
using System.Linq;
using System.Text;
using Xunit;

using Utilities.DataTypes.ExtensionMethods;
using System.Data;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class TypeConversion
    {
        [Fact]
        public void TypeToDbType()
        {
            Assert.Equal(DbType.Int32, typeof(int).ToDbType());
            Assert.Equal(DbType.String, typeof(string).ToDbType());
            Assert.Equal(DbType.Single, typeof(float).ToDbType());
        }

        [Fact]
        public void TypeToSqlDbType()
        {
            Assert.Equal(SqlDbType.Int, typeof(int).ToSQLDbType());
            Assert.Equal(SqlDbType.NVarChar, typeof(string).ToSQLDbType());
            Assert.Equal(SqlDbType.Real, typeof(float).ToSQLDbType());
        }

        [Fact]
        public void DbTypeToType()
        {
            Assert.Equal(typeof(int), DbType.Int32.ToType());
            Assert.Equal(typeof(string), DbType.String.ToType());
            Assert.Equal(typeof(float), DbType.Single.ToType());
        }

        [Fact]
        public void SqlDbTypeToType()
        {
            Assert.Equal(typeof(int), SqlDbType.Int.ToType());
            Assert.Equal(typeof(string), SqlDbType.NVarChar.ToType());
            Assert.Equal(typeof(float), SqlDbType.Real.ToType());
        }

        [Fact]
        public void DbTypeToSqlDbType()
        {
            Assert.Equal(SqlDbType.Int, DbType.Int32.ToSqlDbType());
            Assert.Equal(SqlDbType.NVarChar, DbType.String.ToSqlDbType());
            Assert.Equal(SqlDbType.Real, DbType.Single.ToSqlDbType());
        }

        [Fact]
        public void SqlDbTypeToDbType()
        {
            Assert.Equal(DbType.Int32, SqlDbType.Int.ToDbType());
            Assert.Equal(DbType.String, SqlDbType.NVarChar.ToDbType());
            Assert.Equal(DbType.Single, SqlDbType.Real.ToDbType());
        }

        [Fact]
        public void FormatToString()
        {
            object TestObject = new DateTime(1999, 1, 1);
            Assert.Equal("January 1, 1999", TestObject.FormatToString("MMMM d, yyyy"));
        }

        [Fact]
        public void NullCheck()
        {
            object TestObject = new DateTime(1999, 1, 1);
            Assert.Equal(TestObject, TestObject.NullCheck());
            Assert.Same(TestObject, TestObject.NullCheck());
            TestObject = null;
            Assert.Equal(new DateTime(1999, 1, 2), TestObject.NullCheck(new DateTime(1999, 1, 2)));
        }

        [Fact]
        public void IsNull()
        {
            Assert.False(new DateTime(1999, 1, 1).IsNull());
            object TestObject = null;
            Assert.True(TestObject.IsNull());
        }

        [Fact]
        public void IsDefault()
        {
            Assert.False(new DateTime(1999, 1, 1).IsDefault());
            object TestObject = null;
            Assert.True(TestObject.IsDefault());
        }

        [Fact]
        public void IsNullOrDBNull()
        {
            Assert.False(new DateTime(1999, 1, 1).IsNull());
            object TestObject = null;
            Assert.True(TestObject.IsNull());
            Assert.True(DBNull.Value.IsNull());
        }

        [Fact]
        public void ThrowIfNull()
        {
            object TempObject = null;
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNull("TempName"));
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNull(new ArgumentNullException("TempName")));
        }

        [Fact]
        public void ThrowIfNullOrEmpty()
        {
            string TempObject = "";
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNullOrEmpty("TempName"));
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNullOrEmpty(new ArgumentNullException("TempName")));
        }

        [Fact]
        public void ThrowIfDefault()
        {
            Assert.Throws<ArgumentNullException>(() => default(DateTime).ThrowIfDefault("TempName"));
            Assert.Throws<ArgumentNullException>(() => default(DateTime).ThrowIfDefault(new ArgumentNullException("TempName")));
        }

        [Fact]
        public void ThrowIfNullOrDBNull()
        {
            Assert.Throws<ArgumentNullException>(() => DBNull.Value.ThrowIfNull("TempName"));
            object TempObject = null;
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNull("TempName"));
            Assert.Throws<ArgumentNullException>(() => DBNull.Value.ThrowIfNull(new ArgumentNullException("TempName")));
            Assert.Throws<ArgumentNullException>(() => TempObject.ThrowIfNull(new ArgumentNullException("TempName")));
        }

        [Fact]
        public void TryConvert()
        {
            Assert.Equal(1, (1.0f).TryTo(0));
            Assert.Equal("2011", (2011).TryTo(""));
            Assert.NotNull(new MyTestClass().TryTo<MyTestClass, IMyTestClass>());
            Assert.NotNull(((object)new MyTestClass()).TryTo<object, IMyTestClass>());
        }

        [Fact]
        public void ToList()
        {
            List<PreDataTable> Temp = new PreDataTable[] { new PreDataTable { ID = 1, Value = "A" }, new PreDataTable { ID = 2, Value = "B" }, new PreDataTable { ID = 3, Value = "C" } }.ToList();
            List<PreDataTable> Temp2 = Temp.ToDataTable().ToList<PreDataTable>();
            Assert.Equal(Temp, Temp2);
        }
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
