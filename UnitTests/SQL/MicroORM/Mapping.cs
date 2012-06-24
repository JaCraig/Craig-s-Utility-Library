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
using MoonUnit.Attributes;
using MoonUnit;
using Utilities.SQL;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;
using System.Data;
using Utilities.SQL.MicroORM;
using Utilities.SQL.ParameterTypes;
using Utilities.Random.ExtensionMethods;
using Utilities.Random.StringGenerators;

namespace UnitTests.SQL.MicroORM
{
    public class Mapping:IDisposable
    {
        public Mapping()
        {
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("Create Database TestDatabase", "Data Source=localhost;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteNonQuery();

            }
            using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("Create Table TestTable(ID_ INT PRIMARY KEY IDENTITY,StringValue_ NVARCHAR(100),LongValue_ BIGINT,BoolValue_ BIT,FloatValue_ FLOAT,StringMaxValue_ NVARCHAR(MAX))", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
            {
                Helper.ExecuteNonQuery();
            }
        }

        [Test]
        public void Creation()
        {
            Assert.DoesNotThrow<Exception>(() =>
            {
                using (Mapping<ObjectClass1> TestObject = new Mapping<ObjectClass1>("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "TestTable", "ID_"))
                {
                    using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
                    {
                        using (Mapping<ObjectClass1> TestObject2 = new Mapping<ObjectClass1>(TestObject, Helper))
                        {

                        }
                    }
                }
                using (Mapping<ObjectClass1> TestObject = new Mapping<ObjectClass1>("TestTable", "ID_"))
                {

                }
            });
        }

        [Test]
        public void Map()
        {
            using (Mapping<ObjectClass1> TestObject = new Mapping<ObjectClass1>("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "TestTable", "ID_"))
            {
                TestObject.Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_")
                    .Map(x => x.StringMaxValue, "StringMaxValue_", -1);
                Assert.Equal(6, TestObject.ParameterNames.Count());
                Assert.NotNull(TestObject.Mappings);
                Assert.NotNull(TestObject.Helper);
            }
        }

        [Test]
        public void Insert()
        {
            using (Mapping<ObjectClass1> TestObject = new Mapping<ObjectClass1>("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "TestTable", "ID_"))
            {
                TestObject.Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_")
                    .Map(x => x.StringMaxValue, "StringMaxValue_", -1);
                Utilities.Random.Random Rand = new Utilities.Random.Random(12345);
                ObjectClass1 TempObject=new ObjectClass1();
                TempObject.StringValue="Test String";
                TempObject.BoolValue=true;
                TempObject.FloatValue=1234.5f;
                TempObject.LongValue=12345;
                TempObject.StringMaxValue = Rand.Next<string>(new RegexStringGenerator(6000));
                TempObject.ID = TestObject.Insert<int>(TempObject);
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
                        Assert.Equal(TempObject.StringMaxValue, Helper.GetParameter<string>("StringMaxValue_", ""));
                    }
                    else
                    {
                        Assert.Fail("Nothing was inserted");
                    }
                }
            }
        }

        [Test]
        public void Update()
        {
            using (Mapping<ObjectClass1> TestObject = new Mapping<ObjectClass1>("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "TestTable", "ID_"))
            {
                TestObject.Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_")
                    .Map(x => x.StringMaxValue, "StringMaxValue_", -1);
                Utilities.Random.Random Rand = new Utilities.Random.Random(12346);
                ObjectClass1 TempObject = new ObjectClass1();
                TempObject.StringValue = "Test";
                TempObject.BoolValue = false;
                TempObject.FloatValue = 1.5f;
                TempObject.LongValue = 12;
                TempObject.StringMaxValue = Rand.Next<string>(new RegexStringGenerator(6000));
                TempObject.ID = TestObject.Insert<int>(TempObject);
                Rand = new Utilities.Random.Random(12345);
                TempObject.StringValue = "Test String";
                TempObject.BoolValue = true;
                TempObject.FloatValue = 1234.5f;
                TempObject.LongValue = 12345;
                TempObject.StringMaxValue = Rand.Next<string>(new RegexStringGenerator(6000));
                TestObject.Update(TempObject);
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
                        Assert.Equal(TempObject.StringMaxValue, Helper.GetParameter<string>("StringMaxValue_", ""));
                    }
                    else
                    {
                        Assert.Fail("Nothing was inserted");
                    }
                }
            }
        }

        [Test]
        public void Save()
        {
            using (Mapping<ObjectClass1> TestObject = new Mapping<ObjectClass1>("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "TestTable", "ID_"))
            {
                TestObject.Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_")
                    .Map(x => x.StringMaxValue, "StringMaxValue_", -1);
                Utilities.Random.Random Rand = new Utilities.Random.Random(12346);
                ObjectClass1 TempObject = new ObjectClass1();
                TempObject.StringValue = "Test";
                TempObject.BoolValue = false;
                TempObject.FloatValue = 1.5f;
                TempObject.LongValue = 12;
                TempObject.StringMaxValue = Rand.Next<string>(new RegexStringGenerator(6000));
                TestObject.Save<int>(TempObject);
                Rand = new Utilities.Random.Random(12345);
                TempObject.StringValue = "Test String";
                TempObject.BoolValue = true;
                TempObject.FloatValue = 1234.5f;
                TempObject.LongValue = 12345;
                TempObject.StringMaxValue = Rand.Next<string>(new RegexStringGenerator(6000));
                TestObject.Save<int>(TempObject);
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
                        Assert.Equal(TempObject.StringMaxValue, Helper.GetParameter<string>("StringMaxValue_", ""));
                    }
                    else
                    {
                        Assert.Fail("Nothing was inserted");
                    }
                }
            }
        }

        [Test]
        public void Any()
        {
            using (Mapping<ObjectClass1> TestObject = new Mapping<ObjectClass1>("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "TestTable", "ID_"))
            {
                TestObject.Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_")
                    .Map(x => x.StringMaxValue, "StringMaxValue_", -1);
                Utilities.Random.Random Rand = new Utilities.Random.Random(12345);
                ObjectClass1 TempObject = new ObjectClass1();
                TempObject.StringValue = "Test String";
                TempObject.BoolValue = true;
                TempObject.FloatValue = 1234.5f;
                TempObject.LongValue = 12345;
                TempObject.StringMaxValue = Rand.Next<string>(new RegexStringGenerator(6000));
                string StringMaxValue = TempObject.StringMaxValue;
                TestObject.Save<int>(TempObject);
                TempObject = TestObject.Any();
                Assert.Equal("Test String", TempObject.StringValue);
                Assert.Equal(1234.5f, TempObject.FloatValue);
                Assert.Equal(true, TempObject.BoolValue);
                Assert.Equal(12345, TempObject.LongValue);
                Assert.Equal(1, TempObject.ID);
                Assert.Equal(StringMaxValue, TempObject.StringMaxValue);
            }
        }

        [Test]
        public void All()
        {
            using (Mapping<ObjectClass1> TestObject = new Mapping<ObjectClass1>("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "TestTable", "ID_"))
            {
                TestObject.Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_")
                    .Map(x => x.StringMaxValue, "StringMaxValue_", -1);
                Utilities.Random.Random Rand = new Utilities.Random.Random(12345);
                ObjectClass1 TempObject = new ObjectClass1();
                TempObject.StringValue = "Test String";
                TempObject.BoolValue = true;
                TempObject.FloatValue = 1234.5f;
                TempObject.LongValue = 12345;
                TempObject.StringMaxValue = Rand.Next<string>(new RegexStringGenerator(6000));
                TestObject.Save<int>(TempObject);
                IEnumerable<ObjectClass1> Objects = TestObject.All();
                Assert.Equal(1, Objects.Count());
                foreach (ObjectClass1 Item in Objects)
                {
                    Assert.Equal("Test String", Item.StringValue);
                    Assert.Equal(1234.5f, Item.FloatValue);
                    Assert.Equal(true, Item.BoolValue);
                    Assert.Equal(12345, Item.LongValue);
                    Assert.Equal(1, Item.ID);
                    Assert.Equal(TempObject.StringMaxValue, Item.StringMaxValue);
                }
                List<ObjectClass1> Objects2 = new List<ObjectClass1>();
                Rand = new Utilities.Random.Random();
                for (int x = 0; x < 10; ++x)
                {
                    TempObject = new ObjectClass1();
                    TempObject.StringValue = Rand.Next<string>(new RegexStringGenerator(10));
                    TempObject.BoolValue = Rand.Next<bool>();
                    TempObject.FloatValue = (float)Rand.NextDouble();
                    TempObject.LongValue = Rand.Next(0, 100);
                    TempObject.StringMaxValue = Rand.Next<string>(new RegexStringGenerator(6000));
                    Objects2.Add(TempObject);
                }
                TestObject.Save<int>(Objects2);
                Objects = TestObject.All();
                Assert.Equal(11, Objects.Count());
            }
        }

        [Test]
        public void Paged()
        {
            using (Mapping<ObjectClass1> TestObject = new Mapping<ObjectClass1>("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "TestTable", "ID_"))
            {
                TestObject.Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_")
                    .Map(x => x.StringMaxValue, "StringMaxValue_", -1);
                List<ObjectClass1> Objects2 = new List<ObjectClass1>();
                Utilities.Random.Random Rand = new Utilities.Random.Random();
                for (int x = 0; x < 115; ++x)
                {
                    ObjectClass1 TempObject = new ObjectClass1();
                    TempObject.StringValue = Rand.Next<string>(new RegexStringGenerator(10));
                    TempObject.BoolValue = Rand.Next<bool>();
                    TempObject.FloatValue = (float)Rand.NextDouble();
                    TempObject.LongValue = Rand.Next(0, 100);
                    TempObject.StringMaxValue = Rand.Next<string>(new RegexStringGenerator(6000));
                    Objects2.Add(TempObject);
                }
                TestObject.Save<int>(Objects2);
                IEnumerable<ObjectClass1> Objects = TestObject.Paged();
                Assert.Equal(25, Objects.Count());
                Objects = TestObject.Paged(CurrentPage:1);
                Assert.Equal(25, Objects.Count());
                Objects = TestObject.Paged(CurrentPage: 2);
                Assert.Equal(25, Objects.Count());
                Objects = TestObject.Paged(CurrentPage: 3);
                Assert.Equal(25, Objects.Count());
                Objects = TestObject.Paged(CurrentPage: 4);
                Assert.Equal(15, Objects.Count());
                Assert.Equal(5, TestObject.PageCount());
            }
        }

        [Test]
        public void Paged2()
        {
            using (Mapping<ObjectClass1> TestObject = new Mapping<ObjectClass1>("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "TestTable", "ID_"))
            {
                TestObject.Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_")
                    .Map(x => x.StringMaxValue, "StringMaxValue_", -1);
                List<ObjectClass1> Objects2 = new List<ObjectClass1>();
                Utilities.Random.Random Rand = new Utilities.Random.Random();
                for (int x = 0; x < 115; ++x)
                {
                    ObjectClass1 TempObject = new ObjectClass1();
                    TempObject.StringValue = Rand.Next<string>(new RegexStringGenerator(10));
                    TempObject.BoolValue = Rand.Next<bool>();
                    TempObject.FloatValue = (float)Rand.NextDouble();
                    TempObject.LongValue = Rand.Next(0, 100);
                    TempObject.StringMaxValue = Rand.Next<string>(new RegexStringGenerator(6000));
                    Objects2.Add(TempObject);
                }
                TestObject.Save<int>(Objects2);
                IEnumerable<ObjectClass1> Objects = TestObject.PagedCommand("SELECT * FROM TestTable");
                Assert.Equal(25, Objects.Count());
                Objects = TestObject.PagedCommand("SELECT * FROM TestTable", CurrentPage: 1);
                Assert.Equal(25, Objects.Count());
                Objects = TestObject.PagedCommand("SELECT * FROM TestTable", CurrentPage: 2);
                Assert.Equal(25, Objects.Count());
                Objects = TestObject.PagedCommand("SELECT * FROM TestTable", CurrentPage: 3);
                Assert.Equal(25, Objects.Count());
                Objects = TestObject.PagedCommand("SELECT * FROM TestTable", CurrentPage: 4);
                Assert.Equal(15, Objects.Count());
                Assert.Equal(5, TestObject.PageCount("SELECT * FROM TestTable"));

                Objects = TestObject.PagedCommand("SELECT * FROM TestTable WHERE ID_>@ID", "", 25, 0, null, null, new EqualParameter<int>(50, "ID"));
                Assert.Equal(25, Objects.Count());
                Objects = TestObject.PagedCommand("SELECT * FROM TestTable WHERE ID_>@ID", "", 25, 1, null, null, new EqualParameter<int>(50, "ID"));
                Assert.Equal(25, Objects.Count());
                Objects = TestObject.PagedCommand("SELECT * FROM TestTable WHERE ID_>@ID", "", 25, 2, null, null, new EqualParameter<int>(50, "ID"));
                Assert.Equal(15, Objects.Count());
                Assert.Equal(3, TestObject.PageCount("SELECT * FROM TestTable WHERE ID_>@ID", 25, new EqualParameter<int>(50, "ID")));
            }
        }

        [Test]
        public void Delete()
        {
            using (Mapping<ObjectClass1> TestObject = new Mapping<ObjectClass1>("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", "TestTable", "ID_"))
            {
                TestObject.Map(x => x.ID, "ID_")
                    .Map(x => x.StringValue, "StringValue_", 100)
                    .Map(x => x.FloatValue, "FloatValue_")
                    .Map(x => x.BoolValue, "BoolValue_")
                    .Map(x => x.LongValue, "LongValue_")
                    .Map(x => x.StringMaxValue, "StringMaxValue_", -1);
                Utilities.Random.Random Rand = new Utilities.Random.Random();
                ObjectClass1 TempObject = new ObjectClass1();
                TempObject.StringValue = "Test";
                TempObject.BoolValue = false;
                TempObject.FloatValue = 1.5f;
                TempObject.LongValue = 12;
                TempObject.StringMaxValue = Rand.Next<string>(new RegexStringGenerator(6000));
                TestObject.Save<int>(TempObject);
                Assert.Equal(1, TestObject.Delete(TempObject));
                
                using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT COUNT(*) AS ItemCount FROM TestTable", "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false", CommandType.Text))
                {
                    Helper.ExecuteReader();
                    if (Helper.Read())
                    {
                        Assert.Equal(0, Helper.GetParameter<int>("ItemCount", -1));
                    }
                    else
                    {
                        Assert.Fail("Nothing was inserted");
                    }
                }
            }
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

    public class ObjectClass1
    {
        public virtual int ID { get; set; }
        public virtual string StringValue { get; set; }
        public virtual string StringMaxValue { get; set; }
        public virtual float FloatValue { get; set; }
        public virtual bool BoolValue { get; set; }
        public virtual long LongValue { get; set; }
    }
}
