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
using System.Data;
using Utilities.SQL.MicroORM;
using Xunit;

namespace UnitTests.SQL
{
    public class Command
    {
        [Fact]
        public void Creation()
        {
            Utilities.SQL.MicroORM.Command Object = new Utilities.SQL.MicroORM.Command("SELECT * FROM Table", CommandType.Text, "@", 0, 1, 2, 3);
            Assert.Equal("SELECT * FROM Table", Object.SQLCommand);
            Assert.Equal(CommandType.Text, Object.CommandType);
            Assert.Equal(4, Object.Parameters.Count);
            Assert.Contains(new Parameter<object>("0", (object)0), Object.Parameters);
            Assert.Contains(new Parameter<object>("1", 1), Object.Parameters);
            Assert.Contains(new Parameter<object>("2", 2), Object.Parameters);
            Assert.Contains(new Parameter<object>("3", 3), Object.Parameters);
        }

        [Fact]
        public void Equality()
        {
            Utilities.SQL.MicroORM.Command Object1 = new Utilities.SQL.MicroORM.Command("SELECT * FROM Table", CommandType.Text, "@", 0, 1, 2, 3);
            Utilities.SQL.MicroORM.Command Object2 = new Utilities.SQL.MicroORM.Command("SELECT * FROM Table", CommandType.Text, "@", 0, 1, 2, 3);
            Utilities.SQL.MicroORM.Command Object3 = new Utilities.SQL.MicroORM.Command("SELECT * FROM Table", CommandType.Text, "@", 0, 1, 2);
            Utilities.SQL.MicroORM.Command Object4 = new Utilities.SQL.MicroORM.Command("SELECT * FROM Table", CommandType.Text, "@", 0, 1);
            Utilities.SQL.MicroORM.Command Object5 = new Utilities.SQL.MicroORM.Command("SELECT * FROM Table", CommandType.Text, "@", 0);
            Utilities.SQL.MicroORM.Command Object6 = new Utilities.SQL.MicroORM.Command("SELECT * FROM Table", CommandType.Text);
            Utilities.SQL.MicroORM.Command Object7 = new Utilities.SQL.MicroORM.Command("SELECT * FROM Table", CommandType.StoredProcedure, "@", 0, 1, 2, 3);
            Utilities.SQL.MicroORM.Command Object8 = new Utilities.SQL.MicroORM.Command("SELECT * FROM Table2", CommandType.Text, "@", 0, 1, 2, 3);
            Assert.Equal(Object1, Object2);
            Assert.NotEqual(Object1, Object3);
            Assert.NotEqual(Object1, Object4);
            Assert.NotEqual(Object1, Object5);
            Assert.NotEqual(Object1, Object6);
            Assert.NotEqual(Object1, Object7);
            Assert.NotEqual(Object1, Object8);
        }

        [Fact]
        public void Equality2()
        {
            Utilities.SQL.MicroORM.Command Object1 = new Utilities.SQL.MicroORM.Command("SELECT * FROM Table", CommandType.Text, "@", 1, 1);
            Utilities.SQL.MicroORM.Command Object2 = new Utilities.SQL.MicroORM.Command("SELECT * FROM Table", CommandType.Text, "@", 1, 2);
            Assert.NotEqual(Object1, Object2);
        }
    }
}