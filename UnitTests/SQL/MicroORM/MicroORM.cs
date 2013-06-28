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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Utilities.Random.ExtensionMethods;
using Utilities.Random.StringGenerators;
using Utilities.SQL.ParameterTypes;
using Xunit;

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

        [Fact]
        public void Creation()
        {
            Assert.DoesNotThrow(() => { Utilities.SQL.SQLHelper ORM = new Utilities.SQL.SQLHelper("", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text); });
        }

        [Fact]
        public void Database()
        {
            Assert.DoesNotThrow(() => { Utilities.SQL.SQLHelper.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "DatabaseTestName"); });
        }

        [Fact]
        public void Map()
        {
            Assert.DoesNotThrow(() => { Utilities.SQL.SQLHelper.Map<ObjectClass1>("TestTable", "ID_"); });
        }

        [Fact]
        public void Save()
        {
            Utilities.SQL.SQLHelper.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.SQLHelper.Map<ObjectClass1>("TestTable","ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_")
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = new ObjectClass1();
            using (Utilities.SQL.SQLHelper ORM = new Utilities.SQL.SQLHelper())
            {
                TempObject.StringValue = "Test";
                TempObject.BoolValue = false;
                TempObject.FloatValue = 1.5f;
                TempObject.LongValue = 12;
                ORM.Save<ObjectClass1, int>(TempObject);
                TempObject.StringValue = "Test String";
                TempObject.BoolValue = true;
                TempObject.FloatValue = 1234.5f;
                TempObject.LongValue = 12345;
                ORM.Save<ObjectClass1,int>(TempObject);
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
                    Assert.False(true,"Nothing was inserted");
                }
            }
        }

        [Fact]
        public void Insert()
        {
            Utilities.SQL.SQLHelper.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.SQLHelper.Map<ObjectClass1>("TestTable", "ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_")
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = new ObjectClass1();
            using (Utilities.SQL.SQLHelper ORM = new Utilities.SQL.SQLHelper())
            {
                TempObject.StringValue = "Test String";
                TempObject.BoolValue = true;
                TempObject.FloatValue = 1234.5f;
                TempObject.LongValue = 12345;
                TempObject.ID = ORM.Insert<ObjectClass1, int>(TempObject);
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
                    Assert.False(true,"Nothing was inserted");
                }
            }
        }

        [Fact]
        public void AnyDifferentParameterTypes()
        {
            Utilities.SQL.SQLHelper.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.SQLHelper.Map<ObjectClass1>("TestTable", "ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_")
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            for (int x = 0; x < 30; ++x)
            {
                using (Utilities.SQL.SQLHelper ORM = new Utilities.SQL.SQLHelper())
                {
                    ObjectClass1 TempObject = new ObjectClass1();
                    TempObject.StringValue = "Test String";
                    TempObject.BoolValue = true;
                    TempObject.FloatValue = 1234.5f;
                    TempObject.LongValue = x;
                    TempObject.ID = ORM.Insert<ObjectClass1, int>(TempObject);
                }
            }
            using (Utilities.SQL.SQLHelper ORM = new Utilities.SQL.SQLHelper())
            {
                ObjectClass1 TempObject = ORM.Any<ObjectClass1>("*", null, null, false, new EqualParameter<long>(20, "LongValue_"));
                Assert.Equal(21, TempObject.ID);
                Assert.Equal(20, TempObject.LongValue);
                IEnumerable<ObjectClass1> TempObjects = ORM.All<ObjectClass1>("*", 0, "", null, null, false, new NotEqualParameter<long>(20, "LongValue_"));
                Assert.Equal(29, TempObjects.Count());
                TempObjects = ORM.All<ObjectClass1>("*", 0, "", null, null, false, new BetweenParameter<long>(20, 25, "LongValue_"));
                Assert.Equal(6, TempObjects.Count());
                TempObjects = ORM.All<ObjectClass1>("*", 0, "", null, null, false, new AndParameter(new BetweenParameter<long>(20, 25, "LongValue_"), new NotEqualParameter<long>(20, "LongValue_")));
                Assert.Equal(5, TempObjects.Count());
                TempObjects = ORM.All<ObjectClass1>("*", 0, "", null, null, false, new OrParameter(new BetweenParameter<long>(20, 25, "LongValue_"), new EqualParameter<long>(29, "LongValue_")));
                Assert.Equal(7, TempObjects.Count());
                TempObjects = ORM.All<ObjectClass1>("*", 0, "", null, null, false, new LikeParameter("Test%", "StringValue_", 100));
                Assert.Equal(30, TempObjects.Count());
                TempObjects = ORM.All<ObjectClass1>("*", 0, "", null, null, false, new LikeParameter("Test2%", "StringValue_", 100));
                Assert.Equal(0, TempObjects.Count());
                TempObjects = ORM.All<ObjectClass1>("*", 0, "", null, null, false, new StringEqualParameter("Test String", "StringValue_", 100));
                Assert.Equal(30, TempObjects.Count());
                TempObjects = ORM.All<ObjectClass1>("*", 0, "", null, null,false, new StringNotEqualParameter("Test String", "StringValue_", 100));
                Assert.Equal(0, TempObjects.Count());
            }
        }

        [Fact]
        public void Update()
        {
            Utilities.SQL.SQLHelper.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.SQLHelper.Map<ObjectClass1>("TestTable", "ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_")
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = new ObjectClass1();
            using (Utilities.SQL.SQLHelper ORM = new Utilities.SQL.SQLHelper())
            {
                TempObject.StringValue = "Test";
                TempObject.BoolValue = false;
                TempObject.FloatValue = 1.5f;
                TempObject.LongValue = 12;
                TempObject.ID = ORM.Insert<ObjectClass1, int>(TempObject);
                TempObject.StringValue = "Test String";
                TempObject.BoolValue = true;
                TempObject.FloatValue = 1234.5f;
                TempObject.LongValue = 12345;
                ORM.Update<ObjectClass1>(TempObject);
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
                    Assert.False(true,"Nothing was inserted");
                }
            }
        }

        [Fact]
        public void Any()
        {
            Utilities.SQL.SQLHelper.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.SQLHelper.Map<ObjectClass1>("TestTable", "ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_")
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = new ObjectClass1();
            using (Utilities.SQL.SQLHelper ORM = new Utilities.SQL.SQLHelper())
            {
                TempObject.StringValue = "Test String";
                TempObject.BoolValue = true;
                TempObject.FloatValue = 1234.5f;
                TempObject.LongValue = 12345;
                ORM.Save<ObjectClass1,int>(TempObject);
                TempObject = null;
                TempObject = ORM.Any<ObjectClass1>();
                Assert.Equal("Test String", TempObject.StringValue);
                Assert.Equal(1234.5f, TempObject.FloatValue);
                Assert.Equal(true, TempObject.BoolValue);
                Assert.Equal(12345, TempObject.LongValue);
                Assert.Equal(1, TempObject.ID);
            }
        }

        [Fact]
        public void All()
        {
            Utilities.SQL.SQLHelper.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.SQLHelper.Map<ObjectClass1>("TestTable", "ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_")
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = null;
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            using (Utilities.SQL.SQLHelper ORM = new Utilities.SQL.SQLHelper())
            {
                for (int x = 0; x < 100; ++x)
                {
                    TempObject = new ObjectClass1();
                    TempObject.StringValue = Rand.Next<string>(new RegexStringGenerator(10));
                    TempObject.BoolValue = Rand.Next<bool>();
                    TempObject.FloatValue = (float)Rand.NextDouble();
                    TempObject.LongValue =Rand.Next();
                    ORM.Save<ObjectClass1, int>(TempObject);
                }
                TempObject = null;
                IEnumerable<ObjectClass1> Objects = ORM.All<ObjectClass1>();
                Assert.Equal(100, Objects.Count());
            }
        }

        [Fact]
        public void Paged()
        {
            Utilities.SQL.SQLHelper.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.SQLHelper.Map<ObjectClass1>("TestTable", "ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_")
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = null;
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            using (Utilities.SQL.SQLHelper ORM = new Utilities.SQL.SQLHelper())
            {
                for (int x = 0; x < 115; ++x)
                {
                    TempObject = new ObjectClass1();
                    TempObject.StringValue = Rand.Next<string>(new RegexStringGenerator(10));
                    TempObject.BoolValue = Rand.Next<bool>();
                    TempObject.FloatValue = (float)Rand.NextDouble();
                    TempObject.LongValue = Rand.Next();
                    ORM.Save<ObjectClass1, int>(TempObject);
                }
                TempObject = null;
                IEnumerable<ObjectClass1> Objects = ORM.Paged<ObjectClass1>();
                Assert.Equal(25, Objects.Count());
                Objects = ORM.Paged<ObjectClass1>(CurrentPage: 1);
                Assert.Equal(25, Objects.Count());
                Objects = ORM.Paged<ObjectClass1>(CurrentPage: 2);
                Assert.Equal(25, Objects.Count());
                Objects = ORM.Paged<ObjectClass1>(CurrentPage: 3);
                Assert.Equal(25, Objects.Count());
                Objects = ORM.Paged<ObjectClass1>(CurrentPage: 4);
                Assert.Equal(15, Objects.Count());
                Assert.Equal(5, ORM.PageCount<ObjectClass1>());
            }
        }

        [Fact]
        public void Delete()
        {
            Utilities.SQL.SQLHelper.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            Utilities.SQL.SQLHelper.Map<ObjectClass1>("TestTable", "ID_")
                    .Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_")
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_");
            ObjectClass1 TempObject = null;
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            using (Utilities.SQL.SQLHelper ORM = new Utilities.SQL.SQLHelper())
            {
                for (int x = 0; x < 100; ++x)
                {
                    TempObject = new ObjectClass1();
                    TempObject.StringValue = Rand.Next<string>(new RegexStringGenerator(10));
                    TempObject.BoolValue = Rand.Next<bool>();
                    TempObject.FloatValue = (float)Rand.NextDouble();
                    TempObject.LongValue = Rand.Next();
                    ORM.Save<ObjectClass1, int>(TempObject);
                }
                TempObject = null;
                IEnumerable<ObjectClass1> Objects = ORM.All<ObjectClass1>();
                Assert.Equal(100, Objects.Count());
                foreach (ObjectClass1 Object in Objects)
                {
                    ORM.Delete<ObjectClass1>(Object);
                }
                Objects = ORM.All<ObjectClass1>();
                Assert.Equal(0, Objects.Count());
            }
        }

        public void Dispose()
        {
            Utilities.SQL.SQLHelper.ClearAllMappings();
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("", "Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.Batch().AddCommand("ALTER DATABASE TestDatabase SET OFFLINE WITH ROLLBACK IMMEDIATE", CommandType.Text)
                    .AddCommand("ALTER DATABASE TestDatabase SET ONLINE", CommandType.Text)
                    .AddCommand("DROP DATABASE TestDatabase", CommandType.Text);
                Helper.ExecuteNonQuery();
            }
        }
    }
}