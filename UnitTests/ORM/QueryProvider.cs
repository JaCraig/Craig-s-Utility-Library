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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.ORM
{
    public class QueryProvider : DatabaseBaseClass
    {
        [Fact]
        public void All()
        {
            Guid TempGuid = Guid.NewGuid();
            var Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(TestDatabaseSource);
            Temp.AddCommand(null, null, "insert into TestTable(StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue) VALUES (@0,@1,@2,@3,@4,@5,@6,@7)", CommandType.Text,
                "Test String",
                "Test String",
                12345,
                true,
                1234.5678m,
                12345.6534f,
                new DateTime(1999, 12, 31),
                TempGuid)
                .Execute();

            bool Found = false;
            foreach (dynamic Item in Utilities.ORM.QueryProvider.All("SELECT * FROM TestTable", CommandType.Text, TestDatabaseSource.Name))
            {
                Assert.Equal("Test String", Item.StringValue1);
                Assert.Equal("Test String", Item.StringValue2);
                Assert.Equal(12345, Item.BigIntValue);
                Assert.Equal(true, Item.BitValue);
                Assert.Equal(1234.5678m, Item.DecimalValue);
                Assert.Equal(12345.6534f, Item.FloatValue);
                Assert.Equal(TempGuid, Item.GUIDValue);
                Assert.Equal(new DateTime(1999, 12, 31), Item.DateTimeValue);
                Found = true;
            }
            if (!Found)
            {
                Assert.False(true, "Nothing was inserted");
            }

            Found = false;
            Assert.Equal(1, Utilities.ORM.QueryProvider.All("SELECT * FROM TestTable", CommandType.Text, TestDatabaseSource.Name).Count());
        }

        [Fact]
        public void Any()
        {
            Guid TempGuid = Guid.NewGuid();
            var Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(TestDatabaseSource);
            Temp.AddCommand(null, null, "insert into TestTable(StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue) VALUES (@0,@1,@2,@3,@4,@5,@6,@7)", CommandType.Text,
                "Test String",
                "Test String",
                12345,
                true,
                1234.5678m,
                12345.6534f,
                new DateTime(1999, 12, 31),
                TempGuid)
                .Execute();

            bool Found = false;
            dynamic Item = Utilities.ORM.QueryProvider.Any("SELECT * FROM TestTable", CommandType.Text, TestDatabaseSource.Name);
            if (Item != null)
            {
                Assert.Equal("Test String", Item.StringValue1);
                Assert.Equal("Test String", Item.StringValue2);
                Assert.Equal(12345, Item.BigIntValue);
                Assert.Equal(true, Item.BitValue);
                Assert.Equal(1234.5678m, Item.DecimalValue);
                Assert.Equal(12345.6534f, Item.FloatValue);
                Assert.Equal(TempGuid, Item.GUIDValue);
                Assert.Equal(new DateTime(1999, 12, 31), Item.DateTimeValue);
                Found = true;
            }
            if (!Found)
            {
                Assert.False(true, "Nothing was inserted");
            }
        }
    }
}