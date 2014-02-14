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
using Utilities.ORM.BaseClasses;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.Schema.Default.Database;
using Utilities.ORM.Manager.Schema.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;
using Xunit;

namespace UnitTests.ORM.Manager
{
    public class Session : DatabaseBaseClass
    {
        public Session()
            : base()
        {
            var BootLoader = Utilities.IoC.Manager.Bootstrapper;
        }

        [Fact]
        public void Create()
        {
            Assert.DoesNotThrow(() => new Utilities.ORM.Manager.Session());
        }

        public override void Dispose()
        {
            base.Dispose();
            Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(MasterDatabaseSource);
            try
            {
                Temp.AddCommand(null, null, CommandType.Text, "ALTER DATABASE SessionTestDatabase SET OFFLINE WITH ROLLBACK IMMEDIATE")
                        .AddCommand(null, null, CommandType.Text, "ALTER DATABASE SessionTestDatabase SET ONLINE")
                        .AddCommand(null, null, CommandType.Text, "DROP DATABASE SessionTestDatabase")
                        .Execute();
            }
            catch { }
        }

        [Fact]
        public void Save()
        {
            Utilities.ORM.Manager.Session TestObject = new Utilities.ORM.Manager.Session();
            //Utilities.SQL.SQLHelper.Database("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false");
            //Utilities.SQL.SQLHelper.Map<ObjectClass1>("TestTable", "ID_", Database: "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false")
            //        .Map(x => x.ID, "ID_")
            //        .Map(x => x.StringValue, "StringValue_")
            //        .Map(x => x.FloatValue, "FloatValue_")
            //        .Map(x => x.BoolValue, "BoolValue_")
            //        .Map(x => x.LongValue, "LongValue_");
            //ObjectClass1 TempObject = new ObjectClass1();
            //using (Utilities.SQL.SQLHelper ORM = new Utilities.SQL.SQLHelper("Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false"))
            //{
            //    TempObject.StringValue = "Test";
            //    TempObject.BoolValue = false;
            //    TempObject.FloatValue = 1.5f;
            //    TempObject.LongValue = 12;
            //    ORM.Save<ObjectClass1, int>(TempObject);
            //    TempObject.StringValue = "Test String";
            //    TempObject.BoolValue = true;
            //    TempObject.FloatValue = 1234.5f;
            //    TempObject.LongValue = 12345;
            //    ORM.Save<ObjectClass1, int>(TempObject);
            //}
            //using (Utilities.SQL.SQLHelper Helper = new Utilities.SQL.SQLHelper("SELECT * FROM TestTable", CommandType.Text, "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false"))
            //{
            //    Helper.ExecuteReader();
            //    if (Helper.Read())
            //    {
            //        Assert.Equal("Test String", Helper.GetParameter<string>("StringValue_", ""));
            //        Assert.Equal(1234.5f, Helper.GetParameter<float>("FloatValue_", 0));
            //        Assert.Equal(true, Helper.GetParameter<bool>("BoolValue_", false));
            //        Assert.Equal(12345, Helper.GetParameter<long>("LongValue_", 0));
            //        Assert.Equal(TempObject.ID, Helper.GetParameter<int>("ID_", 0));
            //    }
            //    else
            //    {
            //        Assert.False(true, "Nothing was inserted");
            //    }
            //}
        }

        public enum TestEnum
        {
            Value1 = 0,
            Value2,
            Value3
        }

        public class TestClass
        {
            public bool BoolReference { get; set; }

            public byte[] ByteArrayReference { get; set; }

            public byte ByteReference { get; set; }

            public char CharReference { get; set; }

            public decimal DecimalReference { get; set; }

            public double DoubleReference { get; set; }

            public TestEnum EnumReference { get; set; }

            public float FloatReference { get; set; }

            public Guid GuidReference { get; set; }

            public int ID { get; set; }

            public int IntReference { get; set; }

            public long LongReference { get; set; }

            public IEnumerable<TestClass> ManyToManyIEnumerable { get; set; }

            public List<TestClass> ManyToManyList { get; set; }

            public IEnumerable<TestClass> ManyToOneIEnumerable { get; set; }

            public TestClass ManyToOneItem { get; set; }

            public List<TestClass> ManyToOneList { get; set; }

            public TestClass Map { get; set; }

            public string NullStringReference { get; set; }

            public short ShortReference { get; set; }

            public string StringReference { get; set; }
        }

        public class TestClassDatabase : IDatabase
        {
            public bool Audit
            {
                get { return false; }
            }

            public string Name
            {
                get { return "Data Source=localhost;Initial Catalog=SessionTestDatabase;Integrated Security=SSPI;Pooling=false"; }
            }

            public int Order
            {
                get { return 0; }
            }

            public bool Readable
            {
                get { return true; }
            }

            public bool Update
            {
                get { return true; }
            }

            public bool Writable
            {
                get { return true; }
            }
        }

        public class TestClassMapping : MappingBaseClass<TestClass, TestClassDatabase>
        {
            public TestClassMapping()
                : base()
            {
                ID(x => x.ID).SetAutoIncrement();
                ManyToMany(x => x.ManyToManyIEnumerable).SetTableName("ManyToManyIEnumerable").SetCascade();
                ManyToMany(x => x.ManyToManyList).SetTableName("ManyToManyList").SetCascade();
                ManyToOne(x => x.ManyToOneIEnumerable).SetTableName("ManyToOneIEnumerable").SetCascade();
                ManyToOne(x => x.ManyToOneList).SetTableName("ManyToOneList").SetCascade();
                ManyToOne(x => x.ManyToOneItem).SetTableName("ManyToOneList");
                Map(x => x.Map).SetCascade();
                Reference(x => x.BoolReference);
                Reference(x => x.ByteArrayReference);
                Reference(x => x.ByteReference);
                Reference(x => x.CharReference);
                Reference(x => x.DecimalReference);
                Reference(x => x.DoubleReference);
                Reference(x => x.EnumReference);
                Reference(x => x.FloatReference);
                Reference(x => x.GuidReference);
                Reference(x => x.IntReference);
                Reference(x => x.LongReference);
                Reference(x => x.NullStringReference).SetMaxLength(100);
                Reference(x => x.ShortReference);
                Reference(x => x.StringReference).SetMaxLength(100);
            }
        }
    }
}