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
using System.Linq;
using Utilities.SQL.DataClasses;
using Xunit;

namespace UnitTests.SQL.SQLServer
{
    [Collection("DatabaseCollection")]
    public class SQLServer : IDisposable
    {
        public SQLServer()
        {
        }

        [Fact]
        public void CreateDatabase()
        {
            Database Database = new Database("TestDatabase");
            Table TestTable = Database.AddTable("TestTable");
            TestTable.AddColumn<string>("ID_", DbType.Int32);
            TestTable.AddColumn<string>("Value1", DbType.String, 100);
            TestTable.AddColumn<string>("Value2", DbType.Double);
            Utilities.SQL.SQLServer.SQLServer.CreateDatabase(Database, "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("insert into TestTable(ID_,Value1,Value2) VALUES (@ID_,@Value1,@Value2)", CommandType.Text, "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.AddParameter<int>("@ID_", 1);
                Helper.AddParameter<string>("@Value1", "Test String");
                Helper.AddParameter<float>("@Value2", 3.0f);
                Assert.Equal(1, Helper.ExecuteNonQuery());
            }
        }

        public void Dispose()
        {
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("", CommandType.Text, "Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.Batch().AddCommand("ALTER DATABASE TestDatabase SET OFFLINE WITH ROLLBACK IMMEDIATE", CommandType.Text)
                    .AddCommand("ALTER DATABASE TestDatabase SET ONLINE", CommandType.Text)
                    .AddCommand("DROP DATABASE TestDatabase", CommandType.Text);
                Helper.ExecuteNonQuery();
            }
        }

        [Fact]
        public void GetDatabaseStructure()
        {
            Database Database = new Database("TestDatabase");
            Table TestTable = Database.AddTable("TestTable");
            TestTable.AddColumn<string>("ID_", DbType.Int32);
            TestTable.AddColumn<string>("Value1", DbType.String, 100);
            TestTable.AddColumn<string>("Value2", DbType.Double);
            Utilities.SQL.SQLServer.SQLServer.CreateDatabase(Database, "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Database Database2 = Utilities.SQL.SQLServer.SQLServer.GetDatabaseStructure("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Assert.Equal(Database.Tables.First().Name, Database2.Tables.First().Name);
            Assert.Equal(Database.Tables.First().Columns.Count, Database2.Tables.First().Columns.Count);
            Assert.Equal(DbType.Int32, Database2.Tables.First().Columns.First(x => x.Name == "ID_").DataType);
            Assert.Equal(DbType.String, Database2.Tables.First().Columns.First(x => x.Name == "Value1").DataType);
            Assert.Equal(DbType.Double, Database2.Tables.First().Columns.First(x => x.Name == "Value2").DataType);
            Assert.Equal(100, Database2.Tables.First().Columns.First(x => x.Name == "Value1").Length);
            Assert.Equal(4, Database2.Tables.First().Columns.First(x => x.Name == "ID_").Length);
            Assert.Equal(8, Database2.Tables.First().Columns.First(x => x.Name == "Value2").Length);
        }

        [Fact]
        public void UpdateDatabase()
        {
            Database Database = new Database("TestDatabase");
            Table TestTable = Database.AddTable("TestTable");
            TestTable.AddColumn<string>("ID_", DbType.Int32);
            TestTable.AddColumn<string>("Value1", DbType.String, 100);
            TestTable.AddColumn<string>("Value2", DbType.Double);
            Utilities.SQL.SQLServer.SQLServer.CreateDatabase(Database, "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Database Database2 = new Database("TestDatabase");
            TestTable = Database2.AddTable("TestTable");
            TestTable.AddColumn<string>("ID_", DbType.Int32);
            TestTable.AddColumn<string>("Value1", DbType.String, 100);
            TestTable.AddColumn<string>("Value2", DbType.Double);
            TestTable.AddColumn<string>("Value3", DbType.Boolean);
            Utilities.SQL.SQLServer.SQLServer.UpdateDatabase(Database2, Database, "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("insert into TestTable(ID_,Value1,Value2,Value3) VALUES (@ID_,@Value1,@Value2,@Value3)", CommandType.Text, "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false"))
            {
                Helper.AddParameter<int>("@ID_", 1);
                Helper.AddParameter<string>("@Value1", "Test String");
                Helper.AddParameter<float>("@Value2", 3.0f);
                Helper.AddParameter<bool>("@Value3", true);
                Assert.Equal(1, Helper.ExecuteNonQuery());
            }
            Database Database3 = Utilities.SQL.SQLServer.SQLServer.GetDatabaseStructure("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Assert.Equal(Database2.Tables.First().Name, Database3.Tables.First().Name);
            Assert.Equal(Database2.Tables.First().Columns.Count, Database3.Tables.First().Columns.Count);
            Assert.Equal(DbType.Int32, Database3.Tables.First().Columns.First(x => x.Name == "ID_").DataType);
            Assert.Equal(DbType.String, Database3.Tables.First().Columns.First(x => x.Name == "Value1").DataType);
            Assert.Equal(DbType.Double, Database3.Tables.First().Columns.First(x => x.Name == "Value2").DataType);
            Assert.Equal(100, Database3.Tables.First().Columns.First(x => x.Name == "Value1").Length);
            Assert.Equal(4, Database3.Tables.First().Columns.First(x => x.Name == "ID_").Length);
            Assert.Equal(8, Database3.Tables.First().Columns.First(x => x.Name == "Value2").Length);
        }
    }
}