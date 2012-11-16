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
using Utilities.SQL;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;
using System.Data;
using Utilities.DataTypes.ExtensionMethods;

namespace UnitTests.SQL
{
    public class SQLHelper : IDisposable
    {
        public SQLHelper()
        {
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("Create Database TestDatabase", "Data Source=localhost;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteNonQuery();

            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("Create Table TestTable(ID INT PRIMARY KEY IDENTITY,StringValue1 NVARCHAR(100),StringValue2 NVARCHAR(MAX),BigIntValue BIGINT,BitValue BIT,DecimalValue DECIMAL(12,6),FloatValue FLOAT,DateTimeValue DATETIME,GUIDValue UNIQUEIDENTIFIER)", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteNonQuery();
            }
        }

        [Fact]
        public void Connect()
        {
            Assert.DoesNotThrow(() =>
            {
                using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
                {

                }
            });
        }

        [Fact]
        public void CommandInsert()
        {
            Guid TempGuid = Guid.NewGuid();
            Utilities.SQL.Command TempCommand = new Utilities.SQL.Command("insert into TestTable(StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue) VALUES (@0,@1,@2,@3,@4,@5,@6,@7)", CommandType.Text, "Test String", "Test String", 12345L, true, 1234.5678m, 12345.6534f, new DateTime(1999, 12, 31), TempGuid);
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper(TempCommand, "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false","Default"))
            {
                Helper.ExecuteNonQuery();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue1", ""));
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue2", ""));
                    Assert.Equal(12345, Helper.GetParameter<long>("BigIntValue", 0));
                    Assert.Equal(true, Helper.GetParameter<bool>("BitValue", false));
                    Assert.Equal(1234.5678m, Helper.GetParameter<decimal>("DecimalValue", 0));
                    Assert.Equal(12345.6534f, Helper.GetParameter<float>("FloatValue", 0));
                    Assert.Equal(TempGuid, Helper.GetParameter<Guid>("GUIDValue", Guid.Empty));
                    Assert.Equal(new DateTime(1999, 12, 31), Helper.GetParameter<DateTime>("DateTimeValue", DateTime.Now));
                }
                else
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
        }

        [Fact]
        public void CommandInsertNullString()
        {
            Guid TempGuid = Guid.NewGuid();
            Utilities.SQL.Command TempCommand = new Utilities.SQL.Command("insert into TestTable(StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue) VALUES (@0,@1,@2,@3,@4,@5,@6,@7)", CommandType.Text, "Test String", null, 12345, true, 1234.5678m, 12345.6534f, new DateTime(1999, 12, 31), TempGuid);
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper(TempCommand, "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "Default"))
            {
                Helper.ExecuteNonQuery();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue1", ""));
                    Assert.Equal("This is a null string", Helper.GetParameter<string>("StringValue2", "This is a null string"));
                    Assert.Equal(12345, Helper.GetParameter<long>("BigIntValue", 0));
                    Assert.Equal(true, Helper.GetParameter<bool>("BitValue", false));
                    Assert.Equal(1234.5678m, Helper.GetParameter<decimal>("DecimalValue", 0));
                    Assert.Equal(12345.6534f, Helper.GetParameter<float>("FloatValue", 0));
                    Assert.Equal(TempGuid, Helper.GetParameter<Guid>("GUIDValue", Guid.Empty));
                    Assert.Equal(new DateTime(1999, 12, 31), Helper.GetParameter<DateTime>("DateTimeValue", DateTime.Now));
                }
                else
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
        }

        [Fact]
        public void Insert()
        {
            Guid TempGuid = Guid.NewGuid();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("insert into TestTable(StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue) VALUES (@StringValue1,@StringValue2,@BigIntValue,@BitValue,@DecimalValue,@FloatValue,@DateTimeValue,@GUIDValue)", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.AddParameter<string>("@StringValue1", "Test String")
                    .AddParameter<string>("@StringValue2", "Test String")
                    .AddParameter<long>("@BigIntValue", 12345)
                    .AddParameter<bool>("@BitValue", true)
                    .AddParameter<decimal>("@DecimalValue", 1234.5678m)
                    .AddParameter<float>("@FloatValue", 12345.6534f)
                    .AddParameter<Guid>("@GUIDValue", TempGuid)
                    .AddParameter<DateTime>("@DateTimeValue", new DateTime(1999, 12, 31))
                    .ExecuteNonQuery();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue1", ""));
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue2", ""));
                    Assert.Equal(12345, Helper.GetParameter<long>("BigIntValue", 0));
                    Assert.Equal(true, Helper.GetParameter<bool>("BitValue", false));
                    Assert.Equal(1234.5678m, Helper.GetParameter<decimal>("DecimalValue", 0));
                    Assert.Equal(12345.6534f, Helper.GetParameter<float>("FloatValue", 0));
                    Assert.Equal(TempGuid, Helper.GetParameter<Guid>("GUIDValue", Guid.Empty));
                    Assert.Equal(new DateTime(1999, 12, 31), Helper.GetParameter<DateTime>("DateTimeValue", DateTime.Now));
                }
                else
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
        }

        [Fact]
        public void InsertEmptyString()
        {
            Guid TempGuid = Guid.NewGuid();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("insert into TestTable(StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue) VALUES (@StringValue1,@StringValue2,@BigIntValue,@BitValue,@DecimalValue,@FloatValue,@DateTimeValue,@GUIDValue)", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.AddParameter<string>("@StringValue1", "")
                    .AddParameter<string>("@StringValue2", "Test String")
                    .AddParameter<long>("@BigIntValue", 12345)
                    .AddParameter<bool>("@BitValue", true)
                    .AddParameter<decimal>("@DecimalValue", 1234.5678m)
                    .AddParameter<float>("@FloatValue", 12345.6534f)
                    .AddParameter<Guid>("@GUIDValue", TempGuid)
                    .AddParameter<DateTime>("@DateTimeValue", new DateTime(1999, 12, 31))
                    .ExecuteNonQuery();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("Test String1", Helper.GetParameter<string>("StringValue1", "Test String1"));
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue2", ""));
                    Assert.Equal(12345, Helper.GetParameter<long>("BigIntValue", 0));
                    Assert.Equal(true, Helper.GetParameter<bool>("BitValue", false));
                    Assert.Equal(1234.5678m, Helper.GetParameter<decimal>("DecimalValue", 0));
                    Assert.Equal(12345.6534f, Helper.GetParameter<float>("FloatValue", 0));
                    Assert.Equal(TempGuid, Helper.GetParameter<Guid>("GUIDValue", Guid.Empty));
                    Assert.Equal(new DateTime(1999, 12, 31), Helper.GetParameter<DateTime>("DateTimeValue", DateTime.Now));
                }
                else
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
        }

        [Fact]
        public void ClearParameters()
        {
            Guid TempGuid = Guid.NewGuid();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("insert into TestTable(StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue) VALUES (@StringValue1,@StringValue2,@BigIntValue,@BitValue,@DecimalValue,@FloatValue,@DateTimeValue,@GUIDValue)", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.AddParameter<string>("@StringValue1", "Test")
                    .AddParameter<string>("@StringValue2", "Test")
                    .AddParameter<long>("@BigIntValue", 123)
                    .AddParameter<bool>("@BitValue", false)
                    .AddParameter<decimal>("@DecimalValue", 1234)
                    .AddParameter<float>("@FloatValue", 12345)
                    .AddParameter<Guid>("@GUIDValue", Guid.NewGuid())
                    .AddParameter<DateTime>("@DateTimeValue", new DateTime(1999, 1, 1))
                    .ClearParameters();
                Helper.AddParameter<string>("@StringValue1", "Test String");
                Helper.AddParameter<string>("@StringValue2", "Test String");
                Helper.AddParameter<long>("@BigIntValue", 12345);
                Helper.AddParameter<bool>("@BitValue", true);
                Helper.AddParameter<decimal>("@DecimalValue", 1234.5678m);
                Helper.AddParameter<float>("@FloatValue", 12345.6534f);
                Helper.AddParameter<Guid>("@GUIDValue", TempGuid);
                Helper.AddParameter<DateTime>("@DateTimeValue", new DateTime(1999, 12, 31));
                Helper.ExecuteNonQuery();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue1", ""));
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue2", ""));
                    Assert.Equal(12345, Helper.GetParameter<long>("BigIntValue", 0));
                    Assert.Equal(true, Helper.GetParameter<bool>("BitValue", false));
                    Assert.Equal(1234.5678m, Helper.GetParameter<decimal>("DecimalValue", 0));
                    Assert.Equal(12345.6534f, Helper.GetParameter<float>("FloatValue", 0));
                    Assert.Equal(TempGuid, Helper.GetParameter<Guid>("GUIDValue", Guid.Empty));
                    Assert.Equal(new DateTime(1999, 12, 31), Helper.GetParameter<DateTime>("DateTimeValue", DateTime.Now));
                }
                else
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
        }

        [Fact]
        public void Update()
        {

            Guid TempGuid = Guid.NewGuid();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("insert into TestTable(StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue) VALUES (@StringValue1,@StringValue2,@BigIntValue,@BitValue,@DecimalValue,@FloatValue,@DateTimeValue,@GUIDValue)", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.AddParameter<string>("@StringValue1", "Test");
                Helper.AddParameter<string>("@StringValue2", "Test");
                Helper.AddParameter<long>("@BigIntValue", 123);
                Helper.AddParameter<bool>("@BitValue", false);
                Helper.AddParameter<decimal>("@DecimalValue", 1234);
                Helper.AddParameter<float>("@FloatValue", 12345);
                Helper.AddParameter<Guid>("@GUIDValue", Guid.NewGuid());
                Helper.AddParameter<DateTime>("@DateTimeValue", new DateTime(1999, 1, 1));
                Helper.ExecuteNonQuery();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("update TestTable set StringValue1=@StringValue1,StringValue2=@StringValue2,BigIntValue=@BigIntValue,BitValue=@BitValue,DecimalValue=@DecimalValue,FloatValue=@FloatValue,DateTimeValue=@DateTimeValue,GUIDValue=@GUIDValue", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.AddParameter<string>("@StringValue1", "Test String");
                Helper.AddParameter<string>("@StringValue2", "Test String");
                Helper.AddParameter<long>("@BigIntValue", 12345);
                Helper.AddParameter<bool>("@BitValue", true);
                Helper.AddParameter<decimal>("@DecimalValue", 1234.5678m);
                Helper.AddParameter<float>("@FloatValue", 12345.6534f);
                Helper.AddParameter<Guid>("@GUIDValue", TempGuid);
                Helper.AddParameter<DateTime>("@DateTimeValue", new DateTime(1999, 12, 31));
                Helper.ExecuteNonQuery();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue1", ""));
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue2", ""));
                    Assert.Equal(12345, Helper.GetParameter<long>("BigIntValue", 0));
                    Assert.Equal(true, Helper.GetParameter<bool>("BitValue", false));
                    Assert.Equal(1234.5678m, Helper.GetParameter<decimal>("DecimalValue", 0));
                    Assert.Equal(12345.6534f, Helper.GetParameter<float>("FloatValue", 0));
                    Assert.Equal(TempGuid, Helper.GetParameter<Guid>("GUIDValue", Guid.Empty));
                    Assert.Equal(new DateTime(1999, 12, 31), Helper.GetParameter<DateTime>("DateTimeValue", DateTime.Now));
                }
                else
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
        }

        [Fact]
        public void Delete()
        {
            Guid TempGuid = Guid.NewGuid();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("insert into TestTable(StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue) VALUES (@StringValue1,@StringValue2,@BigIntValue,@BitValue,@DecimalValue,@FloatValue,@DateTimeValue,@GUIDValue)", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.AddParameter<string>("@StringValue1", "Test String");
                Helper.AddParameter<string>("@StringValue2", "Test String");
                Helper.AddParameter<long>("@BigIntValue", 12345);
                Helper.AddParameter<bool>("@BitValue", true);
                Helper.AddParameter<decimal>("@DecimalValue", 1234.5678m);
                Helper.AddParameter<float>("@FloatValue", 12345.6534f);
                Helper.AddParameter<Guid>("@GUIDValue", TempGuid);
                Helper.AddParameter<DateTime>("@DateTimeValue", new DateTime(1999, 12, 31));
                Helper.ExecuteNonQuery();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("delete from TestTable where @ID=ID", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.AddParameter<int>("@ID", 1);
                Helper.ExecuteNonQuery();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.False(true,"Nothing was deleted");
                }
            }
        }

        [Fact]
        public void Transaction()
        {
            Guid TempGuid = Guid.NewGuid();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("insert into TestTable(StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue) VALUES (@StringValue1,@StringValue2,@BigIntValue,@BitValue,@DecimalValue,@FloatValue,@DateTimeValue,@GUIDValue)", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.BeginTransaction();
                Helper.AddParameter<string>("@StringValue1", "Test");
                Helper.AddParameter<string>("@StringValue2", "Test");
                Helper.AddParameter<long>("@BigIntValue", 123);
                Helper.AddParameter<bool>("@BitValue", false);
                Helper.AddParameter<decimal>("@DecimalValue", 1234);
                Helper.AddParameter<float>("@FloatValue", 12345);
                Helper.AddParameter<Guid>("@GUIDValue", Guid.NewGuid());
                Helper.AddParameter<DateTime>("@DateTimeValue", new DateTime(1999, 1, 1));
                Helper.ExecuteNonQuery();
                Helper.Rollback();
                Helper.BeginTransaction();
                Helper.AddParameter<string>("@StringValue1", "Test String");
                Helper.AddParameter<string>("@StringValue2", "Test String");
                Helper.AddParameter<long>("@BigIntValue", 12345);
                Helper.AddParameter<bool>("@BitValue", true);
                Helper.AddParameter<decimal>("@DecimalValue", 1234.5678m);
                Helper.AddParameter<float>("@FloatValue", 12345.6534f);
                Helper.AddParameter<Guid>("@GUIDValue", TempGuid);
                Helper.AddParameter<DateTime>("@DateTimeValue", new DateTime(1999, 12, 31));
                Helper.ExecuteNonQuery();
                Helper.Commit();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue1", ""));
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue2", ""));
                    Assert.Equal(12345, Helper.GetParameter<long>("BigIntValue", 0));
                    Assert.Equal(true, Helper.GetParameter<bool>("BitValue", false));
                    Assert.Equal(1234.5678m, Helper.GetParameter<decimal>("DecimalValue", 0));
                    Assert.Equal(12345.6534f, Helper.GetParameter<float>("FloatValue", 0));
                    Assert.Equal(TempGuid, Helper.GetParameter<Guid>("GUIDValue", Guid.Empty));
                    Assert.Equal(new DateTime(1999, 12, 31), Helper.GetParameter<DateTime>("DateTimeValue", DateTime.Now));
                }
                else
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT COUNT(*) as [ItemCount] FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal(1,Helper.GetParameter<int>("ItemCount",0));
                }
                else
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
        }

        [Fact]
        public void OutputParamter()
        {
            Guid TempGuid = Guid.NewGuid();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("insert into TestTable(StringValue1,StringValue2,BigIntValue,BitValue,DecimalValue,FloatValue,DateTimeValue,GUIDValue) VALUES (@StringValue1,@StringValue2,@BigIntValue,@BitValue,@DecimalValue,@FloatValue,@DateTimeValue,@GUIDValue)", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.AddParameter<string>("@StringValue1", "Test String");
                Helper.AddParameter<string>("@StringValue2", "Test String");
                Helper.AddParameter<long>("@BigIntValue", 12345);
                Helper.AddParameter<bool>("@BitValue", true);
                Helper.AddParameter<decimal>("@DecimalValue", 1234.5678m);
                Helper.AddParameter<float>("@FloatValue", 12345.6534f);
                Helper.AddParameter<Guid>("@GUIDValue", TempGuid);
                Helper.AddParameter<DateTime>("@DateTimeValue", new DateTime(1999, 12, 31));
                Helper.ExecuteNonQuery();
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SET @ASD=12345", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.AddParameter<long>("@ASD", Direction: ParameterDirection.Output);
                Helper.ExecuteNonQuery();
                Assert.Equal(12345, Helper.GetParameter<long>("@ASD", 0, ParameterDirection.Output));
            }
        }

        [Fact]
        public void BulkCopy()
        {
            Guid TempGuid=Guid.NewGuid();
            List<BulkCopyObject> Objects = new List<BulkCopyObject>();
            for (int x = 0; x < 100; ++x)
            {
                BulkCopyObject TempObject = new BulkCopyObject();
                TempObject.BigIntValue = 12345;
                TempObject.BitValue = true;
                TempObject.DateTimeValue = new DateTime(1999, 12, 31);
                TempObject.DecimalValue = 1234.5678m;
                TempObject.FloatValue = 12345.6534f;
                TempObject.GUIDValue = TempGuid;
                TempObject.ID = x+1;
                TempObject.StringValue1 = "Test String";
                TempObject.StringValue2 = "Test String";
                Objects.Add(TempObject);
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteBulkCopy(Objects.ToDataTable(),"TestTable");
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                bool Inserted=false;
                while (Helper.Read())
                {
                    Inserted=true;
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue1", ""));
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue2", ""));
                    Assert.Equal(12345, Helper.GetParameter<long>("BigIntValue", 0));
                    Assert.Equal(true, Helper.GetParameter<bool>("BitValue", false));
                    Assert.Equal(1234.5678m, Helper.GetParameter<decimal>("DecimalValue", 0));
                    Assert.Equal(12345.6534f, Helper.GetParameter<float>("FloatValue", 0));
                    Assert.Equal(TempGuid, Helper.GetParameter<Guid>("GUIDValue", Guid.Empty));
                    Assert.Equal(new DateTime(1999, 12, 31), Helper.GetParameter<DateTime>("DateTimeValue", DateTime.Now));
                }
                if(!Inserted)
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT COUNT(*) as [ItemCount] FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal(100, Helper.GetParameter<int>("ItemCount", 0));
                }
                else
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
        }

        [Fact]
        public void BulkCopy2()
        {
            Guid TempGuid = Guid.NewGuid();
            List<BulkCopyObject> Objects = new List<BulkCopyObject>();
            for (int x = 0; x < 100; ++x)
            {
                BulkCopyObject TempObject = new BulkCopyObject();
                TempObject.BigIntValue = 12345;
                TempObject.BitValue = true;
                TempObject.DateTimeValue = new DateTime(1999, 12, 31);
                TempObject.DecimalValue = 1234.5678m;
                TempObject.FloatValue = 12345.6534f;
                TempObject.GUIDValue = TempGuid;
                TempObject.ID = x + 1;
                TempObject.StringValue1 = "Test String";
                TempObject.StringValue2 = "Test String";
                Objects.Add(TempObject);
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteBulkCopy(Objects, "TestTable");
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                bool Inserted = false;
                while (Helper.Read())
                {
                    Inserted = true;
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue1", ""));
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue2", ""));
                    Assert.Equal(12345, Helper.GetParameter<long>("BigIntValue", 0));
                    Assert.Equal(true, Helper.GetParameter<bool>("BitValue", false));
                    Assert.Equal(1234.5678m, Helper.GetParameter<decimal>("DecimalValue", 0));
                    Assert.Equal(12345.6534f, Helper.GetParameter<float>("FloatValue", 0));
                    Assert.Equal(TempGuid, Helper.GetParameter<Guid>("GUIDValue", Guid.Empty));
                    Assert.Equal(new DateTime(1999, 12, 31), Helper.GetParameter<DateTime>("DateTimeValue", DateTime.Now));
                }
                if (!Inserted)
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT COUNT(*) as [ItemCount] FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal(100, Helper.GetParameter<int>("ItemCount", 0));
                }
                else
                {
                    Assert.False(true,"Nothing was inserted");
                }
            }
        }

        public class BulkCopyObject
        {
            public int ID{get;set;}
            public string StringValue1{get;set;}
            public string StringValue2{get;set;}
            public long BigIntValue{get;set;}
            public bool BitValue{get;set;}
            public decimal DecimalValue{get;set;}
            public float FloatValue{get;set;}
            public DateTime DateTimeValue{get;set;}
            public Guid GUIDValue { get; set; }
        }

        public void Dispose()
        {
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("ALTER DATABASE TestDatabase SET OFFLINE WITH ROLLBACK IMMEDIATE", "Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteNonQuery();
                Helper.Command = "ALTER DATABASE TestDatabase SET ONLINE";
                Helper.ExecuteNonQuery();
                Helper.Command = "DROP DATABASE TestDatabase";
                Helper.ExecuteNonQuery();
            }
        }
    }
}