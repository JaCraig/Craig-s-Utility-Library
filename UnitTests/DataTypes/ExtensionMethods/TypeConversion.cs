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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonUnit;
using MoonUnit.Attributes;
using Utilities.DataTypes.ExtensionMethods;
using System.Data;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class TypeConversion
    {
        [Test]
        public void TypeToDbType()
        {
            Assert.Equal(DbType.Int32,typeof(int).ToDbType());
            Assert.Equal(DbType.String, typeof(string).ToDbType());
            Assert.Equal(DbType.Single, typeof(float).ToDbType());
        }

        [Test]
        public void TypeToSqlDbType()
        {
            Assert.Equal(SqlDbType.Int, typeof(int).ToSQLDbType());
            Assert.Equal(SqlDbType.NVarChar, typeof(string).ToSQLDbType());
            Assert.Equal(SqlDbType.Real, typeof(float).ToSQLDbType());
        }

        [Test]
        public void DbTypeToType()
        {
            Assert.Equal(typeof(int), DbType.Int32.ToType());
            Assert.Equal(typeof(string), DbType.String.ToType());
            Assert.Equal(typeof(float), DbType.Single.ToType());
        }

        [Test]
        public void SqlDbTypeToType()
        {
            Assert.Equal(typeof(int), SqlDbType.Int.ToType());
            Assert.Equal(typeof(string), SqlDbType.NVarChar.ToType());
            Assert.Equal(typeof(float), SqlDbType.Real.ToType());
        }

        [Test]
        public void DbTypeToSqlDbType()
        {
            Assert.Equal(SqlDbType.Int, DbType.Int32.ToSqlDbType());
            Assert.Equal(SqlDbType.NVarChar, DbType.String.ToSqlDbType());
            Assert.Equal(SqlDbType.Real, DbType.Single.ToSqlDbType());
        }

        [Test]
        public void SqlDbTypeToDbType()
        {
            Assert.Equal(DbType.Int32, SqlDbType.Int.ToDbType());
            Assert.Equal(DbType.String, SqlDbType.NVarChar.ToDbType());
            Assert.Equal(DbType.Single, SqlDbType.Real.ToDbType());
        }
    }
}
