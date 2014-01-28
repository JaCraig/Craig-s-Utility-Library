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
using System.Data;
using System.Linq;
using Utilities.ORM.Manager.Schema.Default.Database;
using Xunit;

namespace UnitTests.ORM.Manager.QueryProvider.Default
{
    /// <summary>
    /// Database batch
    /// </summary>
    public class DatabaseBatch : IDisposable
    {
        public DatabaseBatch()
        {
            Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch("Data Source=localhost;Integrated Security=SSPI;Pooling=false", "@", "System.Data.SqlClient");
            Temp.AddCommand(CommandType.Text, "Create Database TestDatabase")
                .Execute();

            Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "@", "System.Data.SqlClient");
            Temp.AddCommand(CommandType.Text, "Create Table TestTable(ID INT PRIMARY KEY IDENTITY,StringValue1 NVARCHAR(100),StringValue2 NVARCHAR(MAX),BigIntValue BIGINT,BitValue BIT,DecimalValue DECIMAL(12,6),FloatValue FLOAT,DateTimeValue DATETIME,GUIDValue UNIQUEIDENTIFIER)")
                .Execute();
        }

        [Fact]
        public void AddCommand()
        {
            Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch("A", "@", "System.Data.SqlClient");
            Temp.AddCommand("SELECT * FROM A", CommandType.Text, "@", new object[] { 1, "ASDF", 2.0f, Guid.NewGuid() });
            Assert.Equal(1, Temp.CommandCount);
        }

        [Fact]
        public void Create()
        {
            Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch("A", "@", "System.Data.SqlClient");
            Assert.NotNull(Temp);
            Assert.Equal(0, Temp.CommandCount);
        }

        public void Dispose()
        {
            Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch("Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;Pooling=false", "@", "System.Data.SqlClient");
            Temp.AddCommand(CommandType.Text, "ALTER DATABASE TestDatabase SET OFFLINE WITH ROLLBACK IMMEDIATE")
                    .AddCommand(CommandType.Text, "ALTER DATABASE TestDatabase SET ONLINE")
                    .AddCommand(CommandType.Text, "DROP DATABASE TestDatabase")
                    .Execute();
        }

        [Fact]
        public void Insert()
        {
            Guid TempGuid = Guid.NewGuid();
            Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "@", "System.Data.SqlClient");
            Temp.AddCommand("insert into TestTable(StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue) VALUES (@0,@1,@2,@3,@4,@5,@6,@7)", CommandType.Text,
                "Test String",
                "Test String",
                12345,
                true,
                1234.5678m,
                12345.6534f,
                new DateTime(1999, 12, 31),
                TempGuid)
                .Execute();

            Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "@", "System.Data.SqlClient");
            bool Found = false;
            foreach (dynamic Item in Temp.AddCommand(CommandType.Text, "SELECT * FROM TestTable")
                .Execute()
                .First())
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

            //using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT COUNT(*) as [ItemCount] FROM TestTable", CommandType.Text, "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false"))
            //{
            //    Helper.ExecuteReader();
            //    if (Helper.Read())
            //    {
            //        Assert.Equal(1, Helper.GetParameter<int>("ItemCount", 0));
            //    }
            //    else
            //    {
            //        Assert.False(true, "Nothing was inserted");
            //    }
            //}
        }
    }
}