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
using Utilities.ORM.Manager.Aspect.Interfaces;
using Xunit;

namespace UnitTests.ORM.Manager
{
    public class Session : DatabaseBaseClass
    {
        public Session()
            : base()
        {
            var BootLoader = Utilities.IoC.Manager.Bootstrapper;
            new Utilities.ORM.Manager.ORMManager(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.Mapper.Manager>(),
                Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.QueryProvider.Manager>(),
                Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.Schema.Manager>(),
                Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.SourceProvider.Manager>(),
                Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>());
        }

        public enum TestEnum
        {
            Value1 = 0,
            Value2,
            Value3
        }

        [Fact]
        public void All()
        {
            Guid TempGuid = Guid.NewGuid();
            var TestObject = new Utilities.ORM.Manager.Session();
            for (int x = 0; x < 100; ++x)
            {
                var TempObject = new TestClass();
                TempObject.BoolReference = true;
                TempObject.ByteArrayReference = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                TempObject.ByteReference = 12;
                TempObject.CharReference = 'v';
                TempObject.DecimalReference = 1.4213m;
                TempObject.DoubleReference = 1.32645d;
                TempObject.EnumReference = TestEnum.Value2;
                TempObject.FloatReference = 1234.5f;
                TempObject.GuidReference = TempGuid;
                TempObject.IntReference = 145145;
                TempObject.LongReference = 763421;
                TempObject.NullStringReference = null;
                TempObject.ShortReference = 5423;
                TempObject.StringReference = "agsdpghasdg";
                TestObject.Save<TestClass, int>(TempObject);
            }
            IEnumerable<TestClass> ItemList = TestObject.All<TestClass>();
            Assert.Equal(100, ItemList.Count());
            foreach (TestClass Item in ItemList)
            {
                Assert.Equal(true, Item.BoolReference);
                Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
                Assert.Equal(12, Item.ByteReference);
                Assert.Equal('v', Item.CharReference);
                Assert.Equal(1.4213m, Item.DecimalReference);
                Assert.Equal(1.32645d, Item.DoubleReference);
                Assert.Equal(TestEnum.Value2, Item.EnumReference);
                Assert.Equal(1234.5f, Item.FloatReference);
                Assert.Equal(TempGuid, Item.GuidReference);
                Assert.Equal(145145, Item.IntReference);
                Assert.Equal(763421, Item.LongReference);
                Assert.Equal(null, Item.NullStringReference);
                Assert.Equal(5423, Item.ShortReference);
                Assert.Equal("agsdpghasdg", Item.StringReference);
            }
        }

        [Fact]
        public void Any()
        {
            Guid TempGuid = Guid.NewGuid();
            var TestObject = new Utilities.ORM.Manager.Session();
            for (int x = 0; x < 100; ++x)
            {
                var TempObject = new TestClass();
                TempObject.BoolReference = true;
                TempObject.ByteArrayReference = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                TempObject.ByteReference = 12;
                TempObject.CharReference = 'v';
                TempObject.DecimalReference = 1.4213m;
                TempObject.DoubleReference = 1.32645d;
                TempObject.EnumReference = TestEnum.Value2;
                TempObject.FloatReference = 1234.5f;
                TempObject.GuidReference = TempGuid;
                TempObject.IntReference = 145145;
                TempObject.LongReference = 763421;
                TempObject.NullStringReference = null;
                TempObject.ShortReference = 5423;
                TempObject.StringReference = "agsdpghasdg";
                TestObject.Save<TestClass, int>(TempObject);
            }
            TestClass Item = TestObject.Any<TestClass>();
            Assert.Equal(true, Item.BoolReference);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
            Assert.Equal(12, Item.ByteReference);
            Assert.Equal('v', Item.CharReference);
            Assert.Equal(1.4213m, Item.DecimalReference);
            Assert.Equal(1.32645d, Item.DoubleReference);
            Assert.Equal(TestEnum.Value2, Item.EnumReference);
            Assert.Equal(1234.5f, Item.FloatReference);
            Assert.Equal(TempGuid, Item.GuidReference);
            Assert.Equal(145145, Item.IntReference);
            Assert.Equal(763421, Item.LongReference);
            Assert.Equal(null, Item.NullStringReference);
            Assert.Equal(5423, Item.ShortReference);
            Assert.Equal("agsdpghasdg", Item.StringReference);

            Item = TestObject.Any<TestClass>(new Utilities.ORM.Parameters.EqualParameter<int>(1000, "ID_"));
            Assert.Null(Item);

            Item = TestObject.Any<TestClass>(new Utilities.ORM.Parameters.EqualParameter<int>(10, "ID_"));
            Assert.Equal(true, Item.BoolReference);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
            Assert.Equal(12, Item.ByteReference);
            Assert.Equal('v', Item.CharReference);
            Assert.Equal(1.4213m, Item.DecimalReference);
            Assert.Equal(1.32645d, Item.DoubleReference);
            Assert.Equal(TestEnum.Value2, Item.EnumReference);
            Assert.Equal(1234.5f, Item.FloatReference);
            Assert.Equal(TempGuid, Item.GuidReference);
            Assert.Equal(145145, Item.IntReference);
            Assert.Equal(763421, Item.LongReference);
            Assert.Equal(null, Item.NullStringReference);
            Assert.Equal(5423, Item.ShortReference);
            Assert.Equal("agsdpghasdg", Item.StringReference);
            Assert.Equal(10, Item.ID);

            Item = TestObject.Any<TestClass>(new Utilities.ORM.Parameters.EqualParameter<int>(20, "ID_"));
            Assert.Equal(true, Item.BoolReference);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
            Assert.Equal(12, Item.ByteReference);
            Assert.Equal('v', Item.CharReference);
            Assert.Equal(1.4213m, Item.DecimalReference);
            Assert.Equal(1.32645d, Item.DoubleReference);
            Assert.Equal(TestEnum.Value2, Item.EnumReference);
            Assert.Equal(1234.5f, Item.FloatReference);
            Assert.Equal(TempGuid, Item.GuidReference);
            Assert.Equal(145145, Item.IntReference);
            Assert.Equal(763421, Item.LongReference);
            Assert.Equal(null, Item.NullStringReference);
            Assert.Equal(5423, Item.ShortReference);
            Assert.Equal("agsdpghasdg", Item.StringReference);
            Assert.Equal(20, Item.ID);
        }

        [Fact]
        public void AnyByID()
        {
            Guid TempGuid = Guid.NewGuid();
            var TestObject = new Utilities.ORM.Manager.Session();
            for (int x = 0; x < 100; ++x)
            {
                var TempObject = new TestClass();
                TempObject.BoolReference = true;
                TempObject.ByteArrayReference = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                TempObject.ByteReference = 12;
                TempObject.CharReference = 'v';
                TempObject.DecimalReference = 1.4213m;
                TempObject.DoubleReference = 1.32645d;
                TempObject.EnumReference = TestEnum.Value2;
                TempObject.FloatReference = 1234.5f;
                TempObject.GuidReference = TempGuid;
                TempObject.IntReference = 145145;
                TempObject.LongReference = 763421;
                TempObject.NullStringReference = null;
                TempObject.ShortReference = 5423;
                TempObject.StringReference = "agsdpghasdg";
                TestObject.Save<TestClass, int>(TempObject);
            }
            TestClass Item = TestObject.Any<TestClass, int>(10);
            Assert.Equal(10, Item.ID);
            Assert.Equal(true, Item.BoolReference);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
            Assert.Equal(12, Item.ByteReference);
            Assert.Equal('v', Item.CharReference);
            Assert.Equal(1.4213m, Item.DecimalReference);
            Assert.Equal(1.32645d, Item.DoubleReference);
            Assert.Equal(TestEnum.Value2, Item.EnumReference);
            Assert.Equal(1234.5f, Item.FloatReference);
            Assert.Equal(TempGuid, Item.GuidReference);
            Assert.Equal(145145, Item.IntReference);
            Assert.Equal(763421, Item.LongReference);
            Assert.Equal(null, Item.NullStringReference);
            Assert.Equal(5423, Item.ShortReference);
            Assert.Equal("agsdpghasdg", Item.StringReference);

            Item = TestObject.Any<TestClass, int>(20);
            Assert.Equal(20, Item.ID);
            Assert.Equal(true, Item.BoolReference);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
            Assert.Equal(12, Item.ByteReference);
            Assert.Equal('v', Item.CharReference);
            Assert.Equal(1.4213m, Item.DecimalReference);
            Assert.Equal(1.32645d, Item.DoubleReference);
            Assert.Equal(TestEnum.Value2, Item.EnumReference);
            Assert.Equal(1234.5f, Item.FloatReference);
            Assert.Equal(TempGuid, Item.GuidReference);
            Assert.Equal(145145, Item.IntReference);
            Assert.Equal(763421, Item.LongReference);
            Assert.Equal(null, Item.NullStringReference);
            Assert.Equal(5423, Item.ShortReference);
            Assert.Equal("agsdpghasdg", Item.StringReference);
        }

        [Fact]
        public void Create()
        {
            new Utilities.ORM.Manager.Session();
        }

        [Fact]
        public void Delete()
        {
            Guid TempGuid = Guid.NewGuid();
            var TestObject = new Utilities.ORM.Manager.Session();
            for (int x = 0; x < 100; ++x)
            {
                var TempObject = new TestClass();
                TempObject.BoolReference = true;
                TempObject.ByteArrayReference = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                TempObject.ByteReference = 12;
                TempObject.CharReference = 'v';
                TempObject.DecimalReference = 1.4213m;
                TempObject.DoubleReference = 1.32645d;
                TempObject.EnumReference = TestEnum.Value2;
                TempObject.FloatReference = 1234.5f;
                TempObject.GuidReference = TempGuid;
                TempObject.IntReference = 145145;
                TempObject.LongReference = 763421;
                TempObject.NullStringReference = null;
                TempObject.ShortReference = 5423;
                TempObject.StringReference = "agsdpghasdg";
                TestObject.Save<TestClass, int>(TempObject);
            }
            TestObject.Delete(TestObject.Any<TestClass>());
            IEnumerable<TestClass> ItemList = TestObject.All<TestClass>();
            Assert.Equal(99, ItemList.Count());
        }

        public override void Dispose()
        {
            base.Dispose();
            var Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(MasterDatabaseSource);
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
        public void PageCount()
        {
            Guid TempGuid = Guid.NewGuid();
            var TestObject = new Utilities.ORM.Manager.Session();
            for (int x = 0; x < 100; ++x)
            {
                var TempObject = new TestClass();
                TempObject.BoolReference = true;
                TempObject.ByteArrayReference = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                TempObject.ByteReference = 12;
                TempObject.CharReference = 'v';
                TempObject.DecimalReference = 1.4213m;
                TempObject.DoubleReference = 1.32645d;
                TempObject.EnumReference = TestEnum.Value2;
                TempObject.FloatReference = 1234.5f;
                TempObject.GuidReference = TempGuid;
                TempObject.IntReference = 145145;
                TempObject.LongReference = 763421;
                TempObject.NullStringReference = null;
                TempObject.ShortReference = 5423;
                TempObject.StringReference = "agsdpghasdg";
                TestObject.Save<TestClass, int>(TempObject);
            }
            Assert.Equal(4, TestObject.PageCount<TestClass>());
        }

        [Fact]
        public void Paged()
        {
            Guid TempGuid = Guid.NewGuid();
            var TestObject = new Utilities.ORM.Manager.Session();
            for (int x = 0; x < 100; ++x)
            {
                var TempObject = new TestClass();
                TempObject.BoolReference = true;
                TempObject.ByteArrayReference = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                TempObject.ByteReference = 12;
                TempObject.CharReference = 'v';
                TempObject.DecimalReference = 1.4213m;
                TempObject.DoubleReference = 1.32645d;
                TempObject.EnumReference = TestEnum.Value2;
                TempObject.FloatReference = 1234.5f;
                TempObject.GuidReference = TempGuid;
                TempObject.IntReference = 145145;
                TempObject.LongReference = 763421;
                TempObject.NullStringReference = null;
                TempObject.ShortReference = 5423;
                TempObject.StringReference = "agsdpghasdg";
                TestObject.Save<TestClass, int>(TempObject);
            }
            IEnumerable<TestClass> ItemList = TestObject.Paged<TestClass>();
            Assert.Equal(25, ItemList.Count());
            foreach (TestClass Item in ItemList)
            {
                Assert.Equal(true, Item.BoolReference);
                Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
                Assert.Equal(12, Item.ByteReference);
                Assert.Equal('v', Item.CharReference);
                Assert.Equal(1.4213m, Item.DecimalReference);
                Assert.Equal(1.32645d, Item.DoubleReference);
                Assert.Equal(TestEnum.Value2, Item.EnumReference);
                Assert.Equal(1234.5f, Item.FloatReference);
                Assert.Equal(TempGuid, Item.GuidReference);
                Assert.Equal(145145, Item.IntReference);
                Assert.Equal(763421, Item.LongReference);
                Assert.Equal(null, Item.NullStringReference);
                Assert.Equal(5423, Item.ShortReference);
                Assert.Equal("agsdpghasdg", Item.StringReference);
            }
            ItemList = TestObject.Paged<TestClass>(CurrentPage: 1);
            Assert.Equal(25, ItemList.Count());
            foreach (TestClass Item in ItemList)
            {
                Assert.Equal(true, Item.BoolReference);
                Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
                Assert.Equal(12, Item.ByteReference);
                Assert.Equal('v', Item.CharReference);
                Assert.Equal(1.4213m, Item.DecimalReference);
                Assert.Equal(1.32645d, Item.DoubleReference);
                Assert.Equal(TestEnum.Value2, Item.EnumReference);
                Assert.Equal(1234.5f, Item.FloatReference);
                Assert.Equal(TempGuid, Item.GuidReference);
                Assert.Equal(145145, Item.IntReference);
                Assert.Equal(763421, Item.LongReference);
                Assert.Equal(null, Item.NullStringReference);
                Assert.Equal(5423, Item.ShortReference);
                Assert.Equal("agsdpghasdg", Item.StringReference);
            }
            ItemList = TestObject.Paged<TestClass>(CurrentPage: 2);
            Assert.Equal(25, ItemList.Count());
            foreach (TestClass Item in ItemList)
            {
                Assert.Equal(true, Item.BoolReference);
                Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
                Assert.Equal(12, Item.ByteReference);
                Assert.Equal('v', Item.CharReference);
                Assert.Equal(1.4213m, Item.DecimalReference);
                Assert.Equal(1.32645d, Item.DoubleReference);
                Assert.Equal(TestEnum.Value2, Item.EnumReference);
                Assert.Equal(1234.5f, Item.FloatReference);
                Assert.Equal(TempGuid, Item.GuidReference);
                Assert.Equal(145145, Item.IntReference);
                Assert.Equal(763421, Item.LongReference);
                Assert.Equal(null, Item.NullStringReference);
                Assert.Equal(5423, Item.ShortReference);
                Assert.Equal("agsdpghasdg", Item.StringReference);
            }
            ItemList = TestObject.Paged<TestClass>(CurrentPage: 3);
            Assert.Equal(25, ItemList.Count());
            foreach (TestClass Item in ItemList)
            {
                Assert.Equal(true, Item.BoolReference);
                Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
                Assert.Equal(12, Item.ByteReference);
                Assert.Equal('v', Item.CharReference);
                Assert.Equal(1.4213m, Item.DecimalReference);
                Assert.Equal(1.32645d, Item.DoubleReference);
                Assert.Equal(TestEnum.Value2, Item.EnumReference);
                Assert.Equal(1234.5f, Item.FloatReference);
                Assert.Equal(TempGuid, Item.GuidReference);
                Assert.Equal(145145, Item.IntReference);
                Assert.Equal(763421, Item.LongReference);
                Assert.Equal(null, Item.NullStringReference);
                Assert.Equal(5423, Item.ShortReference);
                Assert.Equal("agsdpghasdg", Item.StringReference);
            }
            ItemList = TestObject.Paged<TestClass>(CurrentPage: 4);
            Assert.Equal(0, ItemList.Count());
        }

        [Fact]
        public void Save()
        {
            Guid TempGuid = Guid.NewGuid();
            var TestObject = new Utilities.ORM.Manager.Session();
            var TempObject = new TestClass();
            TempObject.BoolReference = true;
            TempObject.ByteArrayReference = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            TempObject.ByteReference = 12;
            TempObject.CharReference = 'v';
            TempObject.DecimalReference = 1.4213m;
            TempObject.DoubleReference = 1.32645d;
            TempObject.EnumReference = TestEnum.Value2;
            TempObject.FloatReference = 1234.5f;
            TempObject.GuidReference = TempGuid;
            TempObject.IntReference = 145145;
            TempObject.LongReference = 763421;
            TempObject.ManyToManyIEnumerable = new TestClass[] { new TestClass(), new TestClass() };
            TempObject.ManyToManyList = new TestClass[] { new TestClass(), new TestClass(), new TestClass() }.ToList();
            TempObject.ManyToOneIEnumerable = new TestClass[] { new TestClass(), new TestClass(), new TestClass() };
            TempObject.ManyToOneItem = new TestClass();
            TempObject.ManyToOneList = new TestClass[] { new TestClass(), new TestClass(), new TestClass() }.ToList();
            TempObject.Map = new TestClass();
            TempObject.NullStringReference = null;
            TempObject.ShortReference = 5423;
            TempObject.StringReference = "agsdpghasdg";
            TestObject.Save<TestClass, int>(TempObject);

            Assert.Equal(true, TempObject.BoolReference);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, TempObject.ByteArrayReference);
            Assert.Equal(12, TempObject.ByteReference);
            Assert.Equal('v', TempObject.CharReference);
            Assert.Equal(1.4213m, TempObject.DecimalReference);
            Assert.Equal(1.32645d, TempObject.DoubleReference);
            Assert.Equal(TestEnum.Value2, TempObject.EnumReference);
            Assert.Equal(1234.5f, TempObject.FloatReference);
            Assert.Equal(TempGuid, TempObject.GuidReference);
            Assert.Equal(145145, TempObject.IntReference);
            Assert.Equal(763421, TempObject.LongReference);
            Assert.Equal(2, TempObject.ManyToManyIEnumerable.Count());
            Assert.Equal(3, TempObject.ManyToManyList.Count);
            Assert.Equal(3, TempObject.ManyToOneIEnumerable.Count());
            Assert.NotNull(TempObject.ManyToOneItem);
            Assert.Equal(3, TempObject.ManyToOneList.Count);
            Assert.NotNull(TempObject.Map);
            Assert.Equal(null, TempObject.NullStringReference);
            Assert.Equal(5423, TempObject.ShortReference);
            Assert.Equal("agsdpghasdg", TempObject.StringReference);

            var Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data Source=localhost;Initial Catalog=SessionTestDatabase;Integrated Security=SSPI;Pooling=false"));

            IList<dynamic> Items = Temp.AddCommand(null, null, CommandType.Text, "SELECT * FROM TestClass_").Execute().First();
            TestClass Item = Items.FirstOrDefault(x => x.BoolReference_);
            ((IORMObject)Item).Session0 = new Utilities.ORM.Manager.Session();
            Assert.Equal(true, Item.BoolReference);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
            Assert.Equal(12, Item.ByteReference);
            Assert.Equal('v', Item.CharReference);
            Assert.Equal(1.4213m, Item.DecimalReference);
            Assert.Equal(1.32645d, Item.DoubleReference);
            Assert.Equal(TestEnum.Value2, Item.EnumReference);
            Assert.Equal(1234.5f, Item.FloatReference);
            Assert.Equal(TempGuid, Item.GuidReference);
            Assert.Equal(145145, Item.IntReference);
            Assert.Equal(763421, Item.LongReference);
            Assert.Equal(2, Item.ManyToManyIEnumerable.Count());
            Assert.Equal(3, Item.ManyToManyList.Count);
            Assert.Equal(3, Item.ManyToOneIEnumerable.Count());
            Assert.NotNull(Item.ManyToOneItem);
            Assert.Equal(3, Item.ManyToOneList.Count);
            Assert.NotNull(Item.Map);
            Assert.Equal(null, Item.NullStringReference);
            Assert.Equal(5423, Item.ShortReference);
            Assert.Equal("agsdpghasdg", Item.StringReference);
        }

        [Fact]
        public void Update()
        {
            Guid TempGuid = Guid.NewGuid();
            var TestObject = new Utilities.ORM.Manager.Session();
            var TempObject = new TestClass();
            TempObject.BoolReference = true;
            TempObject.ByteArrayReference = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            TempObject.ByteReference = 12;
            TempObject.CharReference = 'v';
            TempObject.DecimalReference = 1.4213m;
            TempObject.DoubleReference = 1.32645d;
            TempObject.EnumReference = TestEnum.Value2;
            TempObject.FloatReference = 1234.5f;
            TempObject.GuidReference = TempGuid;
            TempObject.IntReference = 145145;
            TempObject.LongReference = 763421;
            TempObject.ManyToManyIEnumerable = new TestClass[] { new TestClass(), new TestClass() };
            TempObject.ManyToManyList = new TestClass[] { new TestClass(), new TestClass(), new TestClass() }.ToList();
            TempObject.ManyToOneIEnumerable = new TestClass[] { new TestClass(), new TestClass(), new TestClass() };
            TempObject.ManyToOneItem = new TestClass();
            TempObject.ManyToOneList = new TestClass[] { new TestClass(), new TestClass(), new TestClass() }.ToList();
            TempObject.Map = new TestClass();
            TempObject.NullStringReference = null;
            TempObject.ShortReference = 5423;
            TempObject.StringReference = "agsdpghasdg";
            TestObject.Save<TestClass, int>(TempObject);

            Assert.Equal(true, TempObject.BoolReference);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, TempObject.ByteArrayReference);
            Assert.Equal(12, TempObject.ByteReference);
            Assert.Equal('v', TempObject.CharReference);
            Assert.Equal(1.4213m, TempObject.DecimalReference);
            Assert.Equal(1.32645d, TempObject.DoubleReference);
            Assert.Equal(TestEnum.Value2, TempObject.EnumReference);
            Assert.Equal(1234.5f, TempObject.FloatReference);
            Assert.Equal(TempGuid, TempObject.GuidReference);
            Assert.Equal(145145, TempObject.IntReference);
            Assert.Equal(763421, TempObject.LongReference);
            Assert.Equal(2, TempObject.ManyToManyIEnumerable.Count());
            Assert.Equal(3, TempObject.ManyToManyList.Count);
            Assert.Equal(3, TempObject.ManyToOneIEnumerable.Count());
            Assert.NotNull(TempObject.ManyToOneItem);
            Assert.Equal(3, TempObject.ManyToOneList.Count);
            Assert.NotNull(TempObject.Map);
            Assert.Equal(null, TempObject.NullStringReference);
            Assert.Equal(5423, TempObject.ShortReference);
            Assert.Equal("agsdpghasdg", TempObject.StringReference);

            var Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data Source=localhost;Initial Catalog=SessionTestDatabase;Integrated Security=SSPI;Pooling=false"));

            IList<dynamic> Items = Temp.AddCommand(null, null, CommandType.Text, "SELECT * FROM TestClass_").Execute().First();
            TestClass Item = Items.FirstOrDefault(x => x.BoolReference_);
            ((IORMObject)Item).Session0 = new Utilities.ORM.Manager.Session();
            Assert.Equal(true, Item.BoolReference);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
            Assert.Equal(12, Item.ByteReference);
            Assert.Equal('v', Item.CharReference);
            Assert.Equal(1.4213m, Item.DecimalReference);
            Assert.Equal(1.32645d, Item.DoubleReference);
            Assert.Equal(TestEnum.Value2, Item.EnumReference);
            Assert.Equal(1234.5f, Item.FloatReference);
            Assert.Equal(TempGuid, Item.GuidReference);
            Assert.Equal(145145, Item.IntReference);
            Assert.Equal(763421, Item.LongReference);
            Assert.Equal(2, Item.ManyToManyIEnumerable.Count());
            Assert.Equal(3, Item.ManyToManyList.Count);
            Assert.Equal(3, Item.ManyToOneIEnumerable.Count());
            Assert.NotNull(Item.ManyToOneItem);
            Assert.Equal(3, Item.ManyToOneList.Count);
            Assert.NotNull(Item.Map);
            Assert.Equal(null, Item.NullStringReference);
            Assert.Equal(5423, Item.ShortReference);
            Assert.Equal("agsdpghasdg", Item.StringReference);

            Item.ByteArrayReference = new byte[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 110 };
            Item.ByteReference = 121;
            Item.CharReference = 'V';
            Item.DecimalReference = 11.4213m;
            Item.DoubleReference = 11.32645d;
            Item.EnumReference = TestEnum.Value3;
            Item.FloatReference = 14.5f;
            Item.IntReference = 1451445;
            Item.LongReference = 7634121;
            Item.ShortReference = 43;
            Item.StringReference = "Something";
            TestObject.Save<TestClass, int>(Item);

            Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data Source=localhost;Initial Catalog=SessionTestDatabase;Integrated Security=SSPI;Pooling=false"));

            Items = Temp.AddCommand(null, null, CommandType.Text, "SELECT * FROM TestClass_").Execute().First();
            Item = Items.FirstOrDefault(x => x.BoolReference_);
            ((IORMObject)Item).Session0 = new Utilities.ORM.Manager.Session();
            Assert.Equal(true, Item.BoolReference);
            Assert.Equal(new byte[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 110 }, Item.ByteArrayReference);
            Assert.Equal(121, Item.ByteReference);
            Assert.Equal('V', Item.CharReference);
            Assert.Equal(11.4213m, Item.DecimalReference);
            Assert.Equal(11.32645d, Item.DoubleReference);
            Assert.Equal(TestEnum.Value3, Item.EnumReference);
            Assert.Equal(14.5f, Item.FloatReference);
            Assert.Equal(TempGuid, Item.GuidReference);
            Assert.Equal(1451445, Item.IntReference);
            Assert.Equal(7634121, Item.LongReference);
            Assert.Equal(2, Item.ManyToManyIEnumerable.Count());
            Assert.Equal(3, Item.ManyToManyList.Count);
            Assert.Equal(3, Item.ManyToOneIEnumerable.Count());
            Assert.NotNull(Item.ManyToOneItem);
            Assert.Equal(3, Item.ManyToOneList.Count);
            Assert.NotNull(Item.Map);
            Assert.Equal(null, Item.NullStringReference);
            Assert.Equal(43, Item.ShortReference);
            Assert.Equal("Something", Item.StringReference);
        }

        [Fact]
        public void UpdateCascade()
        {
            Guid TempGuid = Guid.NewGuid();
            var TestObject = new Utilities.ORM.Manager.Session();
            var TempObject = new TestClass();
            TempObject.BoolReference = true;
            TempObject.ByteArrayReference = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            TempObject.ByteReference = 12;
            TempObject.CharReference = 'v';
            TempObject.DecimalReference = 1.4213m;
            TempObject.DoubleReference = 1.32645d;
            TempObject.EnumReference = TestEnum.Value2;
            TempObject.FloatReference = 1234.5f;
            TempObject.GuidReference = TempGuid;
            TempObject.IntReference = 145145;
            TempObject.LongReference = 763421;
            TempObject.ManyToManyIEnumerable = new TestClass[] { new TestClass(), new TestClass() };
            TempObject.ManyToManyList = new TestClass[] { new TestClass(), new TestClass(), new TestClass() }.ToList();
            TempObject.ManyToOneIEnumerable = new TestClass[] { new TestClass(), new TestClass(), new TestClass() };
            TempObject.ManyToOneItem = new TestClass();
            TempObject.ManyToOneList = new TestClass[] { new TestClass(), new TestClass(), new TestClass() }.ToList();
            TempObject.ManyToOneIList = new TestClass[] { new TestClass(), new TestClass(), new TestClass() }.ToList();
            TempObject.ManyToManyIList = new TestClass[] { new TestClass(), new TestClass() }.ToList();
            TempObject.ManyToOneICollection = new TestClass[] { new TestClass(), new TestClass(), new TestClass(), new TestClass() }.ToList();
            TempObject.ManyToManyICollection = new TestClass[] { new TestClass() }.ToList();
            TempObject.Map = new TestClass();
            TempObject.NullStringReference = null;
            TempObject.ShortReference = 5423;
            TempObject.StringReference = "agsdpghasdg";
            TestObject.Save<TestClass, int>(TempObject);

            Assert.Equal(true, TempObject.BoolReference);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, TempObject.ByteArrayReference);
            Assert.Equal(12, TempObject.ByteReference);
            Assert.Equal('v', TempObject.CharReference);
            Assert.Equal(1.4213m, TempObject.DecimalReference);
            Assert.Equal(1.32645d, TempObject.DoubleReference);
            Assert.Equal(TestEnum.Value2, TempObject.EnumReference);
            Assert.Equal(1234.5f, TempObject.FloatReference);
            Assert.Equal(TempGuid, TempObject.GuidReference);
            Assert.Equal(145145, TempObject.IntReference);
            Assert.Equal(763421, TempObject.LongReference);
            Assert.Equal(2, TempObject.ManyToManyIEnumerable.Count());
            Assert.Equal(3, TempObject.ManyToManyList.Count);
            Assert.Equal(3, TempObject.ManyToOneIEnumerable.Count());
            Assert.NotNull(TempObject.ManyToOneItem);
            Assert.Equal(3, TempObject.ManyToOneList.Count);
            Assert.Equal(3, TempObject.ManyToOneIList.Count);
            Assert.Equal(2, TempObject.ManyToManyIList.Count);
            Assert.Equal(4, TempObject.ManyToOneICollection.Count);
            Assert.Equal(1, TempObject.ManyToManyICollection.Count);
            Assert.NotNull(TempObject.Map);
            Assert.Equal(null, TempObject.NullStringReference);
            Assert.Equal(5423, TempObject.ShortReference);
            Assert.Equal("agsdpghasdg", TempObject.StringReference);

            var Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data Source=localhost;Initial Catalog=SessionTestDatabase;Integrated Security=SSPI;Pooling=false"));

            IList<dynamic> Items = Temp.AddCommand(null, null, CommandType.Text, "SELECT * FROM TestClass_").Execute().First();
            TestClass Item = Items.FirstOrDefault(x => x.BoolReference_);
            ((IORMObject)Item).Session0 = new Utilities.ORM.Manager.Session();
            Assert.Equal(true, Item.BoolReference);
            Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
            Assert.Equal(12, Item.ByteReference);
            Assert.Equal('v', Item.CharReference);
            Assert.Equal(1.4213m, Item.DecimalReference);
            Assert.Equal(1.32645d, Item.DoubleReference);
            Assert.Equal(TestEnum.Value2, Item.EnumReference);
            Assert.Equal(1234.5f, Item.FloatReference);
            Assert.Equal(TempGuid, Item.GuidReference);
            Assert.Equal(145145, Item.IntReference);
            Assert.Equal(763421, Item.LongReference);
            Assert.Equal(2, Item.ManyToManyIEnumerable.Count());
            Assert.Equal(3, Item.ManyToManyList.Count);
            Assert.Equal(3, Item.ManyToOneIEnumerable.Count());
            Assert.NotNull(Item.ManyToOneItem);
            Assert.Equal(3, Item.ManyToOneList.Count);
            Assert.NotNull(Item.Map);
            Assert.Equal(null, Item.NullStringReference);
            Assert.Equal(5423, Item.ShortReference);
            Assert.Equal("agsdpghasdg", Item.StringReference);

            Item.Map = new TestClass() { FloatReference = 10f };
            Item.ManyToManyIEnumerable.First().FloatReference = 11f;
            Item.ManyToManyList.Add(new TestClass() { FloatReference = 12f });
            Item.ManyToOneIEnumerable.First().FloatReference = 13f;
            Item.ManyToOneItem.FloatReference = 14f;
            Item.ManyToOneList = new TestClass[] { new TestClass(), new TestClass() }.ToList();
            Item.ManyToManyIList.Add(new TestClass() { FloatReference = 15f });
            Item.ManyToOneIList.Add(new TestClass() { FloatReference = 16f });
            Item.ManyToManyICollection.Add(new TestClass() { FloatReference = 17f });
            Item.ManyToOneICollection.Add(new TestClass() { FloatReference = 18f });
            TestObject.Save<TestClass, int>(Item);

            Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data Source=localhost;Initial Catalog=SessionTestDatabase;Integrated Security=SSPI;Pooling=false"));

            Items = Temp.AddCommand(null, null, CommandType.Text, "SELECT * FROM TestClass_").Execute().First();
            Item = Items.FirstOrDefault(x => x.BoolReference_);
            ((IORMObject)Item).Session0 = new Utilities.ORM.Manager.Session();
            Assert.Equal(10, Item.Map.FloatReference);
            Assert.Equal(11f, Item.ManyToManyIEnumerable.First().FloatReference);
            Assert.Equal(12f, Item.ManyToManyList.Last().FloatReference);
            Assert.Equal(13f, Item.ManyToOneIEnumerable.First().FloatReference);
            Assert.Equal(14f, Item.ManyToOneItem.FloatReference);
            Assert.Equal(15f, Item.ManyToManyIList.Last().FloatReference);
            Assert.Equal(16f, Item.ManyToOneIList.Last().FloatReference);
            Assert.Equal(17f, Item.ManyToManyICollection.Last().FloatReference);
            Assert.Equal(18f, Item.ManyToOneICollection.Last().FloatReference);
            Assert.Equal(2, Item.ManyToOneList.Count);
        }

        public class TestClass
        {
            public virtual bool BoolReference { get; set; }

            public virtual byte[] ByteArrayReference { get; set; }

            public virtual byte ByteReference { get; set; }

            public virtual char CharReference { get; set; }

            public virtual decimal DecimalReference { get; set; }

            public virtual double DoubleReference { get; set; }

            public virtual TestEnum EnumReference { get; set; }

            public virtual float FloatReference { get; set; }

            public virtual Guid GuidReference { get; set; }

            public virtual int ID { get; set; }

            public virtual int IntReference { get; set; }

            public virtual long LongReference { get; set; }

            public virtual ICollection<TestClass> ManyToManyICollection { get; set; }
            public virtual IEnumerable<TestClass> ManyToManyIEnumerable { get; set; }

            public virtual IList<TestClass> ManyToManyIList { get; set; }

            public virtual List<TestClass> ManyToManyList { get; set; }

            public virtual ICollection<TestClass> ManyToOneICollection { get; set; }
            public virtual IEnumerable<TestClass> ManyToOneIEnumerable { get; set; }

            public virtual IList<TestClass> ManyToOneIList { get; set; }
            public virtual TestClass ManyToOneItem { get; set; }

            public virtual List<TestClass> ManyToOneList { get; set; }

            public virtual TestClass Map { get; set; }

            public virtual string NullStringReference { get; set; }

            public virtual short ShortReference { get; set; }

            public virtual string StringReference { get; set; }
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
                ManyToOne(x => x.ManyToOneItem).SetTableName("ManyToOneList").SetCascade();
                ManyToOne(x => x.ManyToOneIList).SetTableName("ManyToOneIList").SetCascade();
                ManyToMany(x => x.ManyToManyIList).SetTableName("ManyToManyIList").SetCascade();
                ManyToOne(x => x.ManyToOneICollection).SetTableName("ManyToOneICollection").SetCascade();
                ManyToMany(x => x.ManyToManyICollection).SetTableName("ManyToManyICollection").SetCascade();
                Map(x => x.Map).SetCascade();
                Reference(x => x.BoolReference);
                Reference(x => x.ByteArrayReference).SetMaxLength(100);
                Reference(x => x.ByteReference);
                Reference(x => x.CharReference);
                Reference(x => x.DecimalReference).SetMaxLength(8);
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