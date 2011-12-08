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
using MoonUnit.Attributes;
using MoonUnit;
using Utilities.SQL;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;
using System.Data;
using Utilities.SQL.MicroORM;

namespace UnitTests.SQL.MicroORM
{
    public class MicroORM:IDisposable
    {
        public MicroORM()
        {
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("Create Database TestDatabase", "Data Source=localhost;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteNonQuery();

            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("Create Table TestTable(ID_ INT PRIMARY KEY IDENTITY,StringValue_ NVARCHAR(100),LongValue_ BIGINT,BoolValue_ BIT,FloatValue_ FLOAT)", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteNonQuery();
            }
        }

        [Test]
        public void Creation()
        {
            Assert.DoesNotThrow<Exception>(() => { Utilities.SQL.MicroORM.MicroORM ORM = new Utilities.SQL.MicroORM.MicroORM("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false"); });
        }

        [Test]
        public void Database()
        {
            Assert.DoesNotThrow<Exception>(() => { Utilities.SQL.MicroORM.MicroORM.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "DatabaseTestName"); });
        }

        [Test]
        public void Map()
        {
            Assert.DoesNotThrow<Exception>(() => { Utilities.SQL.MicroORM.MicroORM.Map<ObjectClass1>("TestTable", "ID_"); });
        }

        [Test]
        public void Save()
        {
            Utilities.SQL.MicroORM.MicroORM.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.MicroORM.MicroORM.Map<ObjectClass1>("TestTable","ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = new ObjectClass1();
            using (Utilities.SQL.MicroORM.MicroORM ORM = new Utilities.SQL.MicroORM.MicroORM())
            {
                TempObject.StringValue = "Test";
                TempObject.BoolValue = false;
                TempObject.FloatValue = 1.5f;
                TempObject.LongValue = 12;
                ORM.Map<ObjectClass1>().Save<int>(TempObject);
                TempObject.StringValue = "Test String";
                TempObject.BoolValue = true;
                TempObject.FloatValue = 1234.5f;
                TempObject.LongValue = 12345;
                ORM.Map<ObjectClass1>().Save<int>(TempObject);
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue_", ""));
                    Assert.Equal(1234.5f, Helper.GetParameter<float>("FloatValue_", 0));
                    Assert.Equal(true, Helper.GetParameter<bool>("BoolValue_", false));
                    Assert.Equal(12345, Helper.GetParameter<long>("LongValue_", 0));
                    Assert.Equal(TempObject.ID, Helper.GetParameter<int>("ID_", 0));
                }
                else
                {
                    Assert.Fail("Nothing was inserted");
                }
            }
        }

        [Test]
        public void Insert()
        {
            Utilities.SQL.MicroORM.MicroORM.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.MicroORM.MicroORM.Map<ObjectClass1>("TestTable", "ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = new ObjectClass1();
            using (Utilities.SQL.MicroORM.MicroORM ORM = new Utilities.SQL.MicroORM.MicroORM())
            {
                TempObject.StringValue = "Test String";
                TempObject.BoolValue = true;
                TempObject.FloatValue = 1234.5f;
                TempObject.LongValue = 12345;
                TempObject.ID = ORM.Map<ObjectClass1>().Insert<int>(TempObject);
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue_", ""));
                    Assert.Equal(1234.5f, Helper.GetParameter<float>("FloatValue_", 0));
                    Assert.Equal(true, Helper.GetParameter<bool>("BoolValue_", false));
                    Assert.Equal(12345, Helper.GetParameter<long>("LongValue_", 0));
                    Assert.Equal(TempObject.ID, Helper.GetParameter<int>("ID_", 0));
                }
                else
                {
                    Assert.Fail("Nothing was inserted");
                }
            }
        }

        [Test]
        public void Update()
        {
            Utilities.SQL.MicroORM.MicroORM.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.MicroORM.MicroORM.Map<ObjectClass1>("TestTable", "ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = new ObjectClass1();
            using (Utilities.SQL.MicroORM.MicroORM ORM = new Utilities.SQL.MicroORM.MicroORM())
            {
                TempObject.StringValue = "Test";
                TempObject.BoolValue = false;
                TempObject.FloatValue = 1.5f;
                TempObject.LongValue = 12;
                TempObject.ID = ORM.Map<ObjectClass1>().Insert<int>(TempObject);
                TempObject.StringValue = "Test String";
                TempObject.BoolValue = true;
                TempObject.FloatValue = 1234.5f;
                TempObject.LongValue = 12345;
                ORM.Map<ObjectClass1>().Update(TempObject);
            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteReader();
                if (Helper.Read())
                {
                    Assert.Equal("Test String", Helper.GetParameter<string>("StringValue_", ""));
                    Assert.Equal(1234.5f, Helper.GetParameter<float>("FloatValue_", 0));
                    Assert.Equal(true, Helper.GetParameter<bool>("BoolValue_", false));
                    Assert.Equal(12345, Helper.GetParameter<long>("LongValue_", 0));
                    Assert.Equal(TempObject.ID, Helper.GetParameter<int>("ID_", 0));
                }
                else
                {
                    Assert.Fail("Nothing was inserted");
                }
            }
        }

        [Test]
        public void Any()
        {
            Utilities.SQL.MicroORM.MicroORM.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.MicroORM.MicroORM.Map<ObjectClass1>("TestTable", "ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = new ObjectClass1();
            using (Utilities.SQL.MicroORM.MicroORM ORM = new Utilities.SQL.MicroORM.MicroORM())
            {
                TempObject.StringValue = "Test String";
                TempObject.BoolValue = true;
                TempObject.FloatValue = 1234.5f;
                TempObject.LongValue = 12345;
                ORM.Map<ObjectClass1>().Save<int>(TempObject);
                TempObject = null;
                TempObject = ORM.Map<ObjectClass1>().Any();
                Assert.Equal("Test String", TempObject.StringValue);
                Assert.Equal(1234.5f, TempObject.FloatValue);
                Assert.Equal(true, TempObject.BoolValue);
                Assert.Equal(12345, TempObject.LongValue);
                Assert.Equal(1, TempObject.ID);
            }
        }

        [Test]
        public void All()
        {
            Utilities.SQL.MicroORM.MicroORM.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.MicroORM.MicroORM.Map<ObjectClass1>("TestTable", "ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = null;
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            using (Utilities.SQL.MicroORM.MicroORM ORM = new Utilities.SQL.MicroORM.MicroORM())
            {
                for (int x = 0; x < 100; ++x)
                {
                    TempObject = new ObjectClass1();
                    TempObject.StringValue = Rand.NextString(10);
                    TempObject.BoolValue = Rand.NextBool();
                    TempObject.FloatValue = (float)Rand.NextDouble();
                    TempObject.LongValue =Rand.Next();
                    ORM.Map<ObjectClass1>().Save<int>(TempObject);
                }
                TempObject = null;
                IEnumerable<ObjectClass1> Objects = ORM.Map<ObjectClass1>().All();
                Assert.Equal(100, Objects.Count());
            }
        }

        [Test]
        public void Paged()
        {
            Utilities.SQL.MicroORM.MicroORM.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.MicroORM.MicroORM.Map<ObjectClass1>("TestTable", "ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = null;
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            using (Utilities.SQL.MicroORM.MicroORM ORM = new Utilities.SQL.MicroORM.MicroORM())
            {
                for (int x = 0; x < 115; ++x)
                {
                    TempObject = new ObjectClass1();
                    TempObject.StringValue = Rand.NextString(10);
                    TempObject.BoolValue = Rand.NextBool();
                    TempObject.FloatValue = (float)Rand.NextDouble();
                    TempObject.LongValue = Rand.Next();
                    ORM.Map<ObjectClass1>().Save<int>(TempObject);
                }
                TempObject = null;
                IEnumerable<ObjectClass1> Objects = ORM.Map<ObjectClass1>().Paged();
                Assert.Equal(25, Objects.Count());
                Objects = ORM.Map<ObjectClass1>().Paged(CurrentPage: 1);
                Assert.Equal(25, Objects.Count());
                Objects = ORM.Map<ObjectClass1>().Paged(CurrentPage: 2);
                Assert.Equal(25, Objects.Count());
                Objects = ORM.Map<ObjectClass1>().Paged(CurrentPage: 3);
                Assert.Equal(25, Objects.Count());
                Objects = ORM.Map<ObjectClass1>().Paged(CurrentPage: 4);
                Assert.Equal(15, Objects.Count());
                Assert.Equal(5, ORM.Map<ObjectClass1>().PageCount());
            }
        }

        [Test]
        public void Delete()
        {
            Utilities.SQL.MicroORM.MicroORM.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.MicroORM.MicroORM.Map<ObjectClass1>("TestTable", "ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = null;
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            using (Utilities.SQL.MicroORM.MicroORM ORM = new Utilities.SQL.MicroORM.MicroORM())
            {
                for (int x = 0; x < 100; ++x)
                {
                    TempObject = new ObjectClass1();
                    TempObject.StringValue = Rand.NextString(10);
                    TempObject.BoolValue = Rand.NextBool();
                    TempObject.FloatValue = (float)Rand.NextDouble();
                    TempObject.LongValue = Rand.Next();
                    ORM.Map<ObjectClass1>().Save<int>(TempObject);
                }
                TempObject = null;
                IEnumerable<ObjectClass1> Objects = ORM.Map<ObjectClass1>().All();
                Assert.Equal(100, Objects.Count());
                foreach (ObjectClass1 Object in Objects)
                {
                    ORM.Map<ObjectClass1>().Delete(Object);
                }
                Objects = ORM.Map<ObjectClass1>().All();
                Assert.Equal(0, Objects.Count());
            }
        }

        public void Dispose()
        {
            Utilities.SQL.MicroORM.MicroORM.ClearMappings();
            Utilities.SQL.MicroORM.MicroORM.ClearMappings("DatabaseTestName");
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