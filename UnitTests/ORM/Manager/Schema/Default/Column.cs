/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

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

using System.Data;
using Utilities.ORM.Manager.Schema.Default.Database;
using Xunit;

namespace UnitTests.SQL.DataClasses
{
    public class Column
    {
        [Fact]
        public void Creation()
        {
            var Column = new Column<int>("Column1", System.Data.DbType.Int32, 0, false, false, false, false, false, "", "", 0, false, false, false, null);
            Assert.Equal("Column1", Column.Name);
            Assert.Equal("", Column.Default);
            Assert.Equal(DbType.Int32, Column.DataType);
            Assert.Equal(false, Column.Nullable);
            Assert.Equal(false, Column.AutoIncrement);
            Assert.Equal(false, Column.Index);
            Assert.Equal(0, Column.Length);
            Assert.Equal(false, Column.PrimaryKey);
            Assert.Equal(false, Column.Unique);
        }
    }
}