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

namespace UnitTests.ORM.Manager
{
    //public class SessionUsingComposition : DatabaseBaseClass
    //{
    //    public SessionUsingComposition()
    //    {
    //        var BootLoader = Utilities.IoC.Manager.Bootstrapper;
    //        new Utilities.ORM.Manager.ORMManager(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.Mapper.Manager>(),
    //            Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.QueryProvider.Manager>(),
    //            Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.Schema.Manager>(),
    //            Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.SourceProvider.Manager>(),
    //            Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>());
    //    }

    // public enum TestEnum { Value1 = 0, Value2, Value3 }

    // [Fact] public void All() { var TempGuid = Guid.NewGuid(); var TestObject = new
    // Utilities.ORM.Manager.Session(); for (int x = 0; x < 100; ++x) { var TempObject = new
    // TestClass2(); TempObject.BoolReference = true; TempObject.ByteArrayReference = new byte[] { 1,
    // 2, 3, 4, 5, 6, 7, 8, 9, 10 }; TempObject.ByteReference = 12; TempObject.CharReference = 'v';
    // TempObject.DecimalReference = 1.4213m; TempObject.DoubleReference = 1.32645d;
    // TempObject.EnumReference = TestEnum.Value2; TempObject.FloatReference = 1234.5f;
    // TempObject.GuidReference = TempGuid; TempObject.IntReference = 145145;
    // TempObject.LongReference = 763421; TempObject.NullStringReference = null;
    // TempObject.ShortReference = 5423; TempObject.StringReference = "agsdpghasdg";
    // TestObject.Save<TestClass2, int>(TempObject); } var ItemList = TestObject.All<TestClass2>();
    // Assert.Equal(100, ItemList.Count()); foreach (TestClass2 Item in ItemList) {
    // Assert.Equal(true, Item.BoolReference); Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9,
    // 10 }, Item.ByteArrayReference); Assert.Equal(12, Item.ByteReference); Assert.Equal('v',
    // Item.CharReference); Assert.Equal(1.4213m, Item.DecimalReference); Assert.Equal(1.32645d,
    // Item.DoubleReference); Assert.Equal(TestEnum.Value2, Item.EnumReference);
    // Assert.Equal(1234.5f, Item.FloatReference); Assert.Equal(TempGuid, Item.GuidReference);
    // Assert.Equal(145145, Item.IntReference); Assert.Equal(763421, Item.LongReference);
    // Assert.Equal(null, Item.NullStringReference); Assert.Equal(5423, Item.ShortReference);
    // Assert.Equal("agsdpghasdg", Item.StringReference); } }

    // [Fact] public void Any() { var TempGuid = Guid.NewGuid(); var TestObject = new
    // Utilities.ORM.Manager.Session(); for (int x = 0; x < 100; ++x) { var TempObject = new
    // TestClass2(); TempObject.BoolReference = true; TempObject.ByteArrayReference = new byte[] { 1,
    // 2, 3, 4, 5, 6, 7, 8, 9, 10 }; TempObject.ByteReference = 12; TempObject.CharReference = 'v';
    // TempObject.DecimalReference = 1.4213m; TempObject.DoubleReference = 1.32645d;
    // TempObject.EnumReference = TestEnum.Value2; TempObject.FloatReference = 1234.5f;
    // TempObject.GuidReference = TempGuid; TempObject.IntReference = 145145;
    // TempObject.LongReference = 763421; TempObject.NullStringReference = null;
    // TempObject.ShortReference = 5423; TempObject.StringReference = "agsdpghasdg";
    // TestObject.Save<TestClass2, int>(TempObject); } var Item = TestObject.Any<TestClass2>();
    // Assert.Equal(true, Item.BoolReference); Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9,
    // 10 }, Item.ByteArrayReference); Assert.Equal(12, Item.ByteReference); Assert.Equal('v',
    // Item.CharReference); Assert.Equal(1.4213m, Item.DecimalReference); Assert.Equal(1.32645d,
    // Item.DoubleReference); Assert.Equal(TestEnum.Value2, Item.EnumReference);
    // Assert.Equal(1234.5f, Item.FloatReference); Assert.Equal(TempGuid, Item.GuidReference);
    // Assert.Equal(145145, Item.IntReference); Assert.Equal(763421, Item.LongReference);
    // Assert.Equal(null, Item.NullStringReference); Assert.Equal(5423, Item.ShortReference);
    // Assert.Equal("agsdpghasdg", Item.StringReference);

    // Item = TestObject.Any<TestClass2>(new Utilities.ORM.Parameters.EqualParameter<int>(1000,
    // "ID_")); Assert.Null(Item);

    // Item = TestObject.Any<TestClass2>(new Utilities.ORM.Parameters.EqualParameter<int>(10,
    // "ID_")); Assert.Equal(true, Item.BoolReference); Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6,
    // 7, 8, 9, 10 }, Item.ByteArrayReference); Assert.Equal(12, Item.ByteReference);
    // Assert.Equal('v', Item.CharReference); Assert.Equal(1.4213m, Item.DecimalReference);
    // Assert.Equal(1.32645d, Item.DoubleReference); Assert.Equal(TestEnum.Value2,
    // Item.EnumReference); Assert.Equal(1234.5f, Item.FloatReference); Assert.Equal(TempGuid,
    // Item.GuidReference); Assert.Equal(145145, Item.IntReference); Assert.Equal(763421,
    // Item.LongReference); Assert.Equal(null, Item.NullStringReference); Assert.Equal(5423,
    // Item.ShortReference); Assert.Equal("agsdpghasdg", Item.StringReference); Assert.Equal(10, Item.ID);

    // Item = TestObject.Any<TestClass2>(new Utilities.ORM.Parameters.EqualParameter<int>(20,
    // "ID_")); Assert.Equal(true, Item.BoolReference); Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6,
    // 7, 8, 9, 10 }, Item.ByteArrayReference); Assert.Equal(12, Item.ByteReference);
    // Assert.Equal('v', Item.CharReference); Assert.Equal(1.4213m, Item.DecimalReference);
    // Assert.Equal(1.32645d, Item.DoubleReference); Assert.Equal(TestEnum.Value2,
    // Item.EnumReference); Assert.Equal(1234.5f, Item.FloatReference); Assert.Equal(TempGuid,
    // Item.GuidReference); Assert.Equal(145145, Item.IntReference); Assert.Equal(763421,
    // Item.LongReference); Assert.Equal(null, Item.NullStringReference); Assert.Equal(5423,
    // Item.ShortReference); Assert.Equal("agsdpghasdg", Item.StringReference); Assert.Equal(20,
    // Item.ID); }

    // [Fact] public void AnyByID() { var TempGuid = Guid.NewGuid(); var TestObject = new
    // Utilities.ORM.Manager.Session(); for (int x = 0; x < 100; ++x) { var TempObject = new
    // TestClass2(); TempObject.BoolReference = true; TempObject.ByteArrayReference = new byte[] { 1,
    // 2, 3, 4, 5, 6, 7, 8, 9, 10 }; TempObject.ByteReference = 12; TempObject.CharReference = 'v';
    // TempObject.DecimalReference = 1.4213m; TempObject.DoubleReference = 1.32645d;
    // TempObject.EnumReference = TestEnum.Value2; TempObject.FloatReference = 1234.5f;
    // TempObject.GuidReference = TempGuid; TempObject.IntReference = 145145;
    // TempObject.LongReference = 763421; TempObject.NullStringReference = null;
    // TempObject.ShortReference = 5423; TempObject.StringReference = "agsdpghasdg";
    // TestObject.Save<TestClass2, int>(TempObject); } var Item = TestObject.Any<TestClass2,
    // int>(10); Assert.Equal(10, Item.ID); Assert.Equal(true, Item.BoolReference); Assert.Equal(new
    // byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference); Assert.Equal(12,
    // Item.ByteReference); Assert.Equal('v', Item.CharReference); Assert.Equal(1.4213m,
    // Item.DecimalReference); Assert.Equal(1.32645d, Item.DoubleReference);
    // Assert.Equal(TestEnum.Value2, Item.EnumReference); Assert.Equal(1234.5f, Item.FloatReference);
    // Assert.Equal(TempGuid, Item.GuidReference); Assert.Equal(145145, Item.IntReference);
    // Assert.Equal(763421, Item.LongReference); Assert.Equal(null, Item.NullStringReference);
    // Assert.Equal(5423, Item.ShortReference); Assert.Equal("agsdpghasdg", Item.StringReference);

    // Item = TestObject.Any<TestClass2, int>(20); Assert.Equal(20, Item.ID); Assert.Equal(true,
    // Item.BoolReference); Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
    // Item.ByteArrayReference); Assert.Equal(12, Item.ByteReference); Assert.Equal('v',
    // Item.CharReference); Assert.Equal(1.4213m, Item.DecimalReference); Assert.Equal(1.32645d,
    // Item.DoubleReference); Assert.Equal(TestEnum.Value2, Item.EnumReference);
    // Assert.Equal(1234.5f, Item.FloatReference); Assert.Equal(TempGuid, Item.GuidReference);
    // Assert.Equal(145145, Item.IntReference); Assert.Equal(763421, Item.LongReference);
    // Assert.Equal(null, Item.NullStringReference); Assert.Equal(5423, Item.ShortReference);
    // Assert.Equal("agsdpghasdg", Item.StringReference); }

    // [Fact] public void Create() { new Utilities.ORM.Manager.Session(); }

    // [Fact] public void Delete() { var TempGuid = Guid.NewGuid(); var TestObject = new
    // Utilities.ORM.Manager.Session(); for (int x = 0; x < 100; ++x) { var TempObject = new
    // TestClass2(); TempObject.BoolReference = true; TempObject.ByteArrayReference = new byte[] { 1,
    // 2, 3, 4, 5, 6, 7, 8, 9, 10 }; TempObject.ByteReference = 12; TempObject.CharReference = 'v';
    // TempObject.DecimalReference = 1.4213m; TempObject.DoubleReference = 1.32645d;
    // TempObject.EnumReference = TestEnum.Value2; TempObject.FloatReference = 1234.5f;
    // TempObject.GuidReference = TempGuid; TempObject.IntReference = 145145;
    // TempObject.LongReference = 763421; TempObject.NullStringReference = null;
    // TempObject.ShortReference = 5423; TempObject.StringReference = "agsdpghasdg";
    // TestObject.Save<TestClass2, int>(TempObject); }
    // TestObject.Delete(TestObject.Any<TestClass2>()); var ItemList = TestObject.All<TestClass2>();
    // Assert.Equal(99, ItemList.Count()); }

    // public override void Dispose() { base.Dispose(); var Temp = new
    // Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(MasterDatabaseSource); try {
    // Temp.AddCommand(null, null, CommandType.Text, "ALTER DATABASE SessionUsingComposition SET
    // OFFLINE WITH ROLLBACK IMMEDIATE") .AddCommand(null, null, CommandType.Text, "ALTER DATABASE
    // SessionUsingComposition SET ONLINE") .AddCommand(null, null, CommandType.Text, "DROP DATABASE
    // SessionUsingComposition") .Execute(); } catch { } }

    // [Fact] public void PageCount() { var TempGuid = Guid.NewGuid(); var TestObject = new
    // Utilities.ORM.Manager.Session(); for (int x = 0; x < 100; ++x) { var TempObject = new
    // TestClass2(); TempObject.BoolReference = true; TempObject.ByteArrayReference = new byte[] { 1,
    // 2, 3, 4, 5, 6, 7, 8, 9, 10 }; TempObject.ByteReference = 12; TempObject.CharReference = 'v';
    // TempObject.DecimalReference = 1.4213m; TempObject.DoubleReference = 1.32645d;
    // TempObject.EnumReference = TestEnum.Value2; TempObject.FloatReference = 1234.5f;
    // TempObject.GuidReference = TempGuid; TempObject.IntReference = 145145;
    // TempObject.LongReference = 763421; TempObject.NullStringReference = null;
    // TempObject.ShortReference = 5423; TempObject.StringReference = "agsdpghasdg";
    // TestObject.Save<TestClass2, int>(TempObject); } Assert.Equal(4,
    // TestObject.PageCount<TestClass2>()); }

    // [Fact] public void Paged() { var TempGuid = Guid.NewGuid(); var TestObject = new
    // Utilities.ORM.Manager.Session(); for (int x = 0; x < 100; ++x) { var TempObject = new
    // TestClass2(); TempObject.BoolReference = true; TempObject.ByteArrayReference = new byte[] { 1,
    // 2, 3, 4, 5, 6, 7, 8, 9, 10 }; TempObject.ByteReference = 12; TempObject.CharReference = 'v';
    // TempObject.DecimalReference = 1.4213m; TempObject.DoubleReference = 1.32645d;
    // TempObject.EnumReference = TestEnum.Value2; TempObject.FloatReference = 1234.5f;
    // TempObject.GuidReference = TempGuid; TempObject.IntReference = 145145;
    // TempObject.LongReference = 763421; TempObject.NullStringReference = null;
    // TempObject.ShortReference = 5423; TempObject.StringReference = "agsdpghasdg";
    // TestObject.Save<TestClass2, int>(TempObject); } var ItemList = TestObject.Paged<TestClass2>();
    // Assert.Equal(25, ItemList.Count()); foreach (TestClass2 Item in ItemList) { Assert.Equal(true,
    // Item.BoolReference); Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
    // Item.ByteArrayReference); Assert.Equal(12, Item.ByteReference); Assert.Equal('v',
    // Item.CharReference); Assert.Equal(1.4213m, Item.DecimalReference); Assert.Equal(1.32645d,
    // Item.DoubleReference); Assert.Equal(TestEnum.Value2, Item.EnumReference);
    // Assert.Equal(1234.5f, Item.FloatReference); Assert.Equal(TempGuid, Item.GuidReference);
    // Assert.Equal(145145, Item.IntReference); Assert.Equal(763421, Item.LongReference);
    // Assert.Equal(null, Item.NullStringReference); Assert.Equal(5423, Item.ShortReference);
    // Assert.Equal("agsdpghasdg", Item.StringReference); } ItemList =
    // TestObject.Paged<TestClass2>(CurrentPage: 1); Assert.Equal(25, ItemList.Count()); foreach
    // (TestClass2 Item in ItemList) { Assert.Equal(true, Item.BoolReference); Assert.Equal(new
    // byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference); Assert.Equal(12,
    // Item.ByteReference); Assert.Equal('v', Item.CharReference); Assert.Equal(1.4213m,
    // Item.DecimalReference); Assert.Equal(1.32645d, Item.DoubleReference);
    // Assert.Equal(TestEnum.Value2, Item.EnumReference); Assert.Equal(1234.5f, Item.FloatReference);
    // Assert.Equal(TempGuid, Item.GuidReference); Assert.Equal(145145, Item.IntReference);
    // Assert.Equal(763421, Item.LongReference); Assert.Equal(null, Item.NullStringReference);
    // Assert.Equal(5423, Item.ShortReference); Assert.Equal("agsdpghasdg", Item.StringReference); }
    // ItemList = TestObject.Paged<TestClass2>(CurrentPage: 2); Assert.Equal(25, ItemList.Count());
    // foreach (TestClass2 Item in ItemList) { Assert.Equal(true, Item.BoolReference);
    // Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
    // Assert.Equal(12, Item.ByteReference); Assert.Equal('v', Item.CharReference);
    // Assert.Equal(1.4213m, Item.DecimalReference); Assert.Equal(1.32645d, Item.DoubleReference);
    // Assert.Equal(TestEnum.Value2, Item.EnumReference); Assert.Equal(1234.5f, Item.FloatReference);
    // Assert.Equal(TempGuid, Item.GuidReference); Assert.Equal(145145, Item.IntReference);
    // Assert.Equal(763421, Item.LongReference); Assert.Equal(null, Item.NullStringReference);
    // Assert.Equal(5423, Item.ShortReference); Assert.Equal("agsdpghasdg", Item.StringReference); }
    // ItemList = TestObject.Paged<TestClass2>(CurrentPage: 3); Assert.Equal(25, ItemList.Count());
    // foreach (TestClass2 Item in ItemList) { Assert.Equal(true, Item.BoolReference);
    // Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Item.ByteArrayReference);
    // Assert.Equal(12, Item.ByteReference); Assert.Equal('v', Item.CharReference);
    // Assert.Equal(1.4213m, Item.DecimalReference); Assert.Equal(1.32645d, Item.DoubleReference);
    // Assert.Equal(TestEnum.Value2, Item.EnumReference); Assert.Equal(1234.5f, Item.FloatReference);
    // Assert.Equal(TempGuid, Item.GuidReference); Assert.Equal(145145, Item.IntReference);
    // Assert.Equal(763421, Item.LongReference); Assert.Equal(null, Item.NullStringReference);
    // Assert.Equal(5423, Item.ShortReference); Assert.Equal("agsdpghasdg", Item.StringReference); }
    // ItemList = TestObject.Paged<TestClass2>(CurrentPage: 4); Assert.Equal(0, ItemList.Count()); }

    // [Fact] public void Save() { var TempGuid = Guid.NewGuid(); var TestObject = new
    // Utilities.ORM.Manager.Session(); var TempObject = new TestClass2(); TempObject.BoolReference =
    // true; TempObject.ByteArrayReference = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    // TempObject.ByteReference = 12; TempObject.CharReference = 'v'; TempObject.DecimalReference =
    // 1.4213m; TempObject.DoubleReference = 1.32645d; TempObject.EnumReference = TestEnum.Value2;
    // TempObject.FloatReference = 1234.5f; TempObject.GuidReference = TempGuid;
    // TempObject.IntReference = 145145; TempObject.LongReference = 763421;
    // TempObject.ManyToManyIEnumerable = new TestClass2[] { new TestClass2(), new TestClass2() };
    // TempObject.ManyToManyList = new ITestInterface[] { new TestClass2(), new TestClass2(), new
    // TestClass2() }.ToList(); TempObject.ManyToOneIEnumerable = new TestClass2[] { new
    // TestClass2(), new TestClass2(), new TestClass2() }; TempObject.ManyToOneItem = new
    // TestClass2(); TempObject.ManyToOneList = new ITestInterface[] { new TestClass2(), new
    // TestClass2(), new TestClass2() }.ToList(); TempObject.Map = new TestClass2();
    // TempObject.NullStringReference = null; TempObject.ShortReference = 5423;
    // TempObject.StringReference = "agsdpghasdg"; TestObject.Save<TestClass2, int>(TempObject);

    // Assert.Equal(true, TempObject.BoolReference); Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7,
    // 8, 9, 10 }, TempObject.ByteArrayReference); Assert.Equal(12, TempObject.ByteReference);
    // Assert.Equal('v', TempObject.CharReference); Assert.Equal(1.4213m,
    // TempObject.DecimalReference); Assert.Equal(1.32645d, TempObject.DoubleReference);
    // Assert.Equal(TestEnum.Value2, TempObject.EnumReference); Assert.Equal(1234.5f,
    // TempObject.FloatReference); Assert.Equal(TempGuid, TempObject.GuidReference);
    // Assert.Equal(145145, TempObject.IntReference); Assert.Equal(763421, TempObject.LongReference);
    // Assert.Equal(2, TempObject.ManyToManyIEnumerable.Count()); Assert.Equal(3,
    // TempObject.ManyToManyList.Count); Assert.Equal(3, TempObject.ManyToOneIEnumerable.Count());
    // Assert.NotNull(TempObject.ManyToOneItem); Assert.Equal(3, TempObject.ManyToOneList.Count);
    // Assert.NotNull(TempObject.Map); Assert.Equal(null, TempObject.NullStringReference);
    // Assert.Equal(5423, TempObject.ShortReference); Assert.Equal("agsdpghasdg", TempObject.StringReference);

    // var Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(new
    // Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data
    // Source=localhost;Initial Catalog=SessionUsingComposition;Integrated Security=SSPI;Pooling=false"));

    // var Items = Temp.AddCommand(null, null, CommandType.Text, "SELECT * FROM
    // TestClass2_").Execute().First(); TestClass2 Item = Items.FirstOrDefault(x =>
    // x.BoolReference_); ((IORMObject)Item).Session0 = new Utilities.ORM.Manager.Session();
    // Assert.Equal(true, Item.BoolReference); Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9,
    // 10 }, Item.ByteArrayReference); Assert.Equal(12, Item.ByteReference); Assert.Equal('v',
    // Item.CharReference); Assert.Equal(1.4213m, Item.DecimalReference); Assert.Equal(1.32645d,
    // Item.DoubleReference); Assert.Equal(TestEnum.Value2, Item.EnumReference);
    // Assert.Equal(1234.5f, Item.FloatReference); Assert.Equal(TempGuid, Item.GuidReference);
    // Assert.Equal(145145, Item.IntReference); Assert.Equal(763421, Item.LongReference);
    // Assert.Equal(2, Item.ManyToManyIEnumerable.Count()); Assert.Equal(3,
    // Item.ManyToManyList.Count); Assert.Equal(3, Item.ManyToOneIEnumerable.Count());
    // Assert.NotNull(Item.ManyToOneItem); Assert.Equal(3, Item.ManyToOneList.Count);
    // Assert.NotNull(Item.Map); Assert.Equal(null, Item.NullStringReference); Assert.Equal(5423,
    // Item.ShortReference); Assert.Equal("agsdpghasdg", Item.StringReference); }

    // [Fact] public void Update() { var TempGuid = Guid.NewGuid(); var TestObject = new
    // Utilities.ORM.Manager.Session(); var TempObject = new TestClass2(); TempObject.BoolReference =
    // true; TempObject.ByteArrayReference = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    // TempObject.ByteReference = 12; TempObject.CharReference = 'v'; TempObject.DecimalReference =
    // 1.4213m; TempObject.DoubleReference = 1.32645d; TempObject.EnumReference = TestEnum.Value2;
    // TempObject.FloatReference = 1234.5f; TempObject.GuidReference = TempGuid;
    // TempObject.IntReference = 145145; TempObject.LongReference = 763421;
    // TempObject.ManyToManyIEnumerable = new TestClass2[] { new TestClass2(), new TestClass2() };
    // TempObject.ManyToManyList = new ITestInterface[] { new TestClass2(), new TestClass2(), new
    // TestClass2() }.ToList(); TempObject.ManyToOneIEnumerable = new TestClass2[] { new
    // TestClass2(), new TestClass2(), new TestClass2() }; TempObject.ManyToOneItem = new
    // TestClass2(); TempObject.ManyToOneList = new ITestInterface[] { new TestClass2(), new
    // TestClass2(), new TestClass2() }.ToList(); TempObject.Map = new TestClass2();
    // TempObject.NullStringReference = null; TempObject.ShortReference = 5423;
    // TempObject.StringReference = "agsdpghasdg"; TestObject.Save<TestClass2, int>(TempObject);

    // Assert.Equal(true, TempObject.BoolReference); Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7,
    // 8, 9, 10 }, TempObject.ByteArrayReference); Assert.Equal(12, TempObject.ByteReference);
    // Assert.Equal('v', TempObject.CharReference); Assert.Equal(1.4213m,
    // TempObject.DecimalReference); Assert.Equal(1.32645d, TempObject.DoubleReference);
    // Assert.Equal(TestEnum.Value2, TempObject.EnumReference); Assert.Equal(1234.5f,
    // TempObject.FloatReference); Assert.Equal(TempGuid, TempObject.GuidReference);
    // Assert.Equal(145145, TempObject.IntReference); Assert.Equal(763421, TempObject.LongReference);
    // Assert.Equal(2, TempObject.ManyToManyIEnumerable.Count()); Assert.Equal(3,
    // TempObject.ManyToManyList.Count); Assert.Equal(3, TempObject.ManyToOneIEnumerable.Count());
    // Assert.NotNull(TempObject.ManyToOneItem); Assert.Equal(3, TempObject.ManyToOneList.Count);
    // Assert.NotNull(TempObject.Map); Assert.Equal(null, TempObject.NullStringReference);
    // Assert.Equal(5423, TempObject.ShortReference); Assert.Equal("agsdpghasdg", TempObject.StringReference);

    // var Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(new
    // Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data
    // Source=localhost;Initial Catalog=SessionUsingComposition;Integrated Security=SSPI;Pooling=false"));

    // var Items = Temp.AddCommand(null, null, CommandType.Text, "SELECT * FROM
    // TestClass2_").Execute().First(); TestClass2 Item = Items.FirstOrDefault(x =>
    // x.BoolReference_); ((IORMObject)Item).Session0 = new Utilities.ORM.Manager.Session();
    // Assert.Equal(true, Item.BoolReference); Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9,
    // 10 }, Item.ByteArrayReference); Assert.Equal(12, Item.ByteReference); Assert.Equal('v',
    // Item.CharReference); Assert.Equal(1.4213m, Item.DecimalReference); Assert.Equal(1.32645d,
    // Item.DoubleReference); Assert.Equal(TestEnum.Value2, Item.EnumReference);
    // Assert.Equal(1234.5f, Item.FloatReference); Assert.Equal(TempGuid, Item.GuidReference);
    // Assert.Equal(145145, Item.IntReference); Assert.Equal(763421, Item.LongReference);
    // Assert.Equal(2, Item.ManyToManyIEnumerable.Count()); Assert.Equal(3,
    // Item.ManyToManyList.Count); Assert.Equal(3, Item.ManyToOneIEnumerable.Count());
    // Assert.NotNull(Item.ManyToOneItem); Assert.Equal(3, Item.ManyToOneList.Count);
    // Assert.NotNull(Item.Map); Assert.Equal(null, Item.NullStringReference); Assert.Equal(5423,
    // Item.ShortReference); Assert.Equal("agsdpghasdg", Item.StringReference);

    // Item.ByteArrayReference = new byte[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 110 };
    // Item.ByteReference = 121; Item.CharReference = 'V'; Item.DecimalReference = 11.4213m;
    // Item.DoubleReference = 11.32645d; Item.EnumReference = TestEnum.Value3; Item.FloatReference =
    // 14.5f; Item.IntReference = 1451445; Item.LongReference = 7634121; Item.ShortReference = 43;
    // Item.StringReference = "Something"; TestObject.Save<TestClass2, int>(Item);

    // Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(new
    // Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data
    // Source=localhost;Initial Catalog=SessionUsingComposition;Integrated Security=SSPI;Pooling=false"));

    // Items = Temp.AddCommand(null, null, CommandType.Text, "SELECT * FROM
    // TestClass2_").Execute().First(); Item = Items.FirstOrDefault(x => x.BoolReference_);
    // ((IORMObject)Item).Session0 = new Utilities.ORM.Manager.Session(); Assert.Equal(true,
    // Item.BoolReference); Assert.Equal(new byte[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 110 },
    // Item.ByteArrayReference); Assert.Equal(121, Item.ByteReference); Assert.Equal('V',
    // Item.CharReference); Assert.Equal(11.4213m, Item.DecimalReference); Assert.Equal(11.32645d,
    // Item.DoubleReference); Assert.Equal(TestEnum.Value3, Item.EnumReference); Assert.Equal(14.5f,
    // Item.FloatReference); Assert.Equal(TempGuid, Item.GuidReference); Assert.Equal(1451445,
    // Item.IntReference); Assert.Equal(7634121, Item.LongReference); Assert.Equal(2,
    // Item.ManyToManyIEnumerable.Count()); Assert.Equal(3, Item.ManyToManyList.Count);
    // Assert.Equal(3, Item.ManyToOneIEnumerable.Count()); Assert.NotNull(Item.ManyToOneItem);
    // Assert.Equal(3, Item.ManyToOneList.Count); Assert.NotNull(Item.Map); Assert.Equal(null,
    // Item.NullStringReference); Assert.Equal(43, Item.ShortReference); Assert.Equal("Something",
    // Item.StringReference); }

    // [Fact] public void UpdateCascade() { var TempGuid = Guid.NewGuid(); var TestObject = new
    // Utilities.ORM.Manager.Session(); var TempObject = new TestClass2(); TempObject.BoolReference =
    // true; TempObject.ByteArrayReference = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    // TempObject.ByteReference = 12; TempObject.CharReference = 'v'; TempObject.DecimalReference =
    // 1.4213m; TempObject.DoubleReference = 1.32645d; TempObject.EnumReference = TestEnum.Value2;
    // TempObject.FloatReference = 1234.5f; TempObject.GuidReference = TempGuid;
    // TempObject.IntReference = 145145; TempObject.LongReference = 763421;
    // TempObject.ManyToManyIEnumerable = new TestClass2[] { new TestClass2(), new TestClass2() };
    // TempObject.ManyToManyList = new ITestInterface[] { new TestClass2(), new TestClass2(), new
    // TestClass2() }.ToList(); TempObject.ManyToOneIEnumerable = new TestClass2[] { new
    // TestClass2(), new TestClass2(), new TestClass2() }; TempObject.ManyToOneItem = new
    // TestClass2(); TempObject.ManyToOneList = new ITestInterface[] { new TestClass2(), new
    // TestClass2(), new TestClass2() }.ToList(); TempObject.ManyToOneIList = new ITestInterface[] {
    // new TestClass2(), new TestClass2(), new TestClass2() }.ToList(); TempObject.ManyToManyIList =
    // new ITestInterface[] { new TestClass2(), new TestClass2() }.ToList();
    // TempObject.ManyToOneICollection = new ITestInterface[] { new TestClass2(), new TestClass2(),
    // new TestClass2(), new TestClass2() }.ToList(); TempObject.ManyToManyICollection = new
    // ITestInterface[] { new TestClass2() }.ToList(); TempObject.Map = new TestClass2();
    // TempObject.NullStringReference = null; TempObject.ShortReference = 5423;
    // TempObject.StringReference = "agsdpghasdg"; TestObject.Save<TestClass2, int>(TempObject);

    // Assert.Equal(true, TempObject.BoolReference); Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7,
    // 8, 9, 10 }, TempObject.ByteArrayReference); Assert.Equal(12, TempObject.ByteReference);
    // Assert.Equal('v', TempObject.CharReference); Assert.Equal(1.4213m,
    // TempObject.DecimalReference); Assert.Equal(1.32645d, TempObject.DoubleReference);
    // Assert.Equal(TestEnum.Value2, TempObject.EnumReference); Assert.Equal(1234.5f,
    // TempObject.FloatReference); Assert.Equal(TempGuid, TempObject.GuidReference);
    // Assert.Equal(145145, TempObject.IntReference); Assert.Equal(763421, TempObject.LongReference);
    // Assert.Equal(2, TempObject.ManyToManyIEnumerable.Count()); Assert.Equal(3,
    // TempObject.ManyToManyList.Count); Assert.Equal(3, TempObject.ManyToOneIEnumerable.Count());
    // Assert.NotNull(TempObject.ManyToOneItem); Assert.Equal(3, TempObject.ManyToOneList.Count);
    // Assert.Equal(3, TempObject.ManyToOneIList.Count); Assert.Equal(2,
    // TempObject.ManyToManyIList.Count); Assert.Equal(4, TempObject.ManyToOneICollection.Count);
    // Assert.Equal(1, TempObject.ManyToManyICollection.Count); Assert.NotNull(TempObject.Map);
    // Assert.Equal(null, TempObject.NullStringReference); Assert.Equal(5423,
    // TempObject.ShortReference); Assert.Equal("agsdpghasdg", TempObject.StringReference);

    // var Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(new
    // Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data
    // Source=localhost;Initial Catalog=SessionUsingComposition;Integrated Security=SSPI;Pooling=false"));

    // var Items = Temp.AddCommand(null, null, CommandType.Text, "SELECT * FROM
    // TestClass2_").Execute().First(); TestClass2 Item = Items.FirstOrDefault(x =>
    // x.BoolReference_); ((IORMObject)Item).Session0 = new Utilities.ORM.Manager.Session();
    // Assert.Equal(true, Item.BoolReference); Assert.Equal(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9,
    // 10 }, Item.ByteArrayReference); Assert.Equal(12, Item.ByteReference); Assert.Equal('v',
    // Item.CharReference); Assert.Equal(1.4213m, Item.DecimalReference); Assert.Equal(1.32645d,
    // Item.DoubleReference); Assert.Equal(TestEnum.Value2, Item.EnumReference);
    // Assert.Equal(1234.5f, Item.FloatReference); Assert.Equal(TempGuid, Item.GuidReference);
    // Assert.Equal(145145, Item.IntReference); Assert.Equal(763421, Item.LongReference);
    // Assert.Equal(2, Item.ManyToManyIEnumerable.Count()); Assert.Equal(3,
    // Item.ManyToManyList.Count); Assert.Equal(3, Item.ManyToOneIEnumerable.Count());
    // Assert.NotNull(Item.ManyToOneItem); Assert.Equal(3, Item.ManyToOneList.Count);
    // Assert.NotNull(Item.Map); Assert.Equal(null, Item.NullStringReference); Assert.Equal(5423,
    // Item.ShortReference); Assert.Equal("agsdpghasdg", Item.StringReference);

    // Item.Map = new TestClass2 { FloatReference = 10f };
    // Item.ManyToManyIEnumerable.First().FloatReference = 11f; Item.ManyToManyList.Add(new
    // TestClass2 { FloatReference = 12f }); Item.ManyToOneIEnumerable.First().FloatReference = 13f;
    // Item.ManyToOneItem.FloatReference = 14f; Item.ManyToOneList = new ITestInterface[] { new
    // TestClass2(), new TestClass2() }.ToList(); Item.ManyToManyIList.Add(new TestClass2 {
    // FloatReference = 15f }); Item.ManyToOneIList.Add(new TestClass2 { FloatReference = 16f });
    // Item.ManyToManyICollection.Add(new TestClass2 { FloatReference = 17f });
    // Item.ManyToOneICollection.Add(new TestClass2 { FloatReference = 18f });
    // TestObject.Save<TestClass2, int>(Item);

    // Temp = new Utilities.ORM.Manager.QueryProvider.Default.DatabaseBatch(new
    // Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data
    // Source=localhost;Initial Catalog=SessionUsingComposition;Integrated Security=SSPI;Pooling=false"));

    // Items = Temp.AddCommand(null, null, CommandType.Text, "SELECT * FROM
    // TestClass2_").Execute().First(); Item = Items.FirstOrDefault(x => x.BoolReference_);
    // ((IORMObject)Item).Session0 = new Utilities.ORM.Manager.Session(); Assert.Equal(10,
    // Item.Map.FloatReference); Assert.Equal(11f,
    // Item.ManyToManyIEnumerable.First().FloatReference); Assert.Equal(12f,
    // Item.ManyToManyList.Last().FloatReference); Assert.Equal(13f,
    // Item.ManyToOneIEnumerable.First().FloatReference); Assert.Equal(14f,
    // Item.ManyToOneItem.FloatReference); Assert.Equal(15f,
    // Item.ManyToManyIList.Last().FloatReference); Assert.Equal(16f,
    // Item.ManyToOneIList.Last().FloatReference); Assert.Equal(17f,
    // Item.ManyToManyICollection.Last().FloatReference); Assert.Equal(18f,
    // Item.ManyToOneICollection.Last().FloatReference); Assert.Equal(2, Item.ManyToOneList.Count); }

    // public interface ITestInterface { bool BoolReference { get; set; } byte[] ByteArrayReference {
    // get; set; } byte ByteReference { get; set; } char CharReference { get; set; } decimal
    // DecimalReference { get; set; } double DoubleReference { get; set; } TestEnum EnumReference {
    // get; set; } float FloatReference { get; set; } Guid GuidReference { get; set; } int ID { get;
    // set; } int IntReference { get; set; } long LongReference { get; set; } }

    // public abstract class TestAbstractClass : ITestInterface { protected TestAbstractClass() { }

    // public abstract bool BoolReference { get; set; } public abstract byte[] ByteArrayReference {
    // get; set; } public abstract byte ByteReference { get; set; } public abstract char
    // CharReference { get; set; } public abstract decimal DecimalReference { get; set; } public
    // abstract double DoubleReference { get; set; } public abstract TestEnum EnumReference { get;
    // set; } public abstract float FloatReference { get; set; } public abstract Guid GuidReference {
    // get; set; } public abstract int ID { get; set; } public abstract int IntReference { get; set;
    // } public abstract long LongReference { get; set; } public abstract ICollection<ITestInterface>
    // ManyToManyICollection { get; set; } public abstract IEnumerable<ITestInterface>
    // ManyToManyIEnumerable { get; set; } public abstract IList<ITestInterface> ManyToManyIList {
    // get; set; } public abstract List<ITestInterface> ManyToManyList { get; set; } public abstract
    // ICollection<ITestInterface> ManyToOneICollection { get; set; } public abstract
    // IEnumerable<ITestInterface> ManyToOneIEnumerable { get; set; } public abstract
    // IList<ITestInterface> ManyToOneIList { get; set; } public abstract ITestInterface
    // ManyToOneItem { get; set; } public abstract List<ITestInterface> ManyToOneList { get; set; }
    // public abstract ITestInterface Map { get; set; } }

    // public class TestAbstractClassMapping : MappingBaseClass<TestAbstractClass,
    // TestCompositionDatabase> { public TestAbstractClassMapping() { ID(x => x.ID); ManyToMany(x =>
    // x.ManyToManyIEnumerable).SetTableName("ManyToManyIEnumerable").SetCascade(); ManyToMany(x =>
    // x.ManyToManyList).SetTableName("ManyToManyList").SetCascade(); ManyToOne(x =>
    // x.ManyToOneIEnumerable).SetTableName("ManyToOneIEnumerable").SetCascade(); ManyToOne(x =>
    // x.ManyToOneList).SetTableName("ManyToOneList").SetCascade(); ManyToOne(x =>
    // x.ManyToOneItem).SetTableName("ManyToOneList").SetCascade(); ManyToOne(x =>
    // x.ManyToOneIList).SetTableName("ManyToOneIList").SetCascade(); ManyToMany(x =>
    // x.ManyToManyIList).SetTableName("ManyToManyIList").SetCascade(); ManyToOne(x =>
    // x.ManyToOneICollection).SetTableName("ManyToOneICollection").SetCascade(); ManyToMany(x =>
    // x.ManyToManyICollection).SetTableName("ManyToManyICollection").SetCascade(); Map(x =>
    // x.Map).SetCascade(); } }

    // public class TestClass2 : TestAbstractClass { public override bool BoolReference { get; set; }

    // public override byte[] ByteArrayReference { get; set; }

    // public override byte ByteReference { get; set; }

    // public override char CharReference { get; set; }

    // public override decimal DecimalReference { get; set; }

    // public override double DoubleReference { get; set; }

    // public override TestEnum EnumReference { get; set; }

    // public override float FloatReference { get; set; }

    // public override Guid GuidReference { get; set; }

    // public override int ID { get; set; }

    // public override int IntReference { get; set; }

    // public override long LongReference { get; set; }

    // public override ICollection<ITestInterface> ManyToManyICollection { get; set; } public
    // override IEnumerable<ITestInterface> ManyToManyIEnumerable { get; set; }

    // public override IList<ITestInterface> ManyToManyIList { get; set; }

    // public override List<ITestInterface> ManyToManyList { get; set; }

    // public override ICollection<ITestInterface> ManyToOneICollection { get; set; } public override
    // IEnumerable<ITestInterface> ManyToOneIEnumerable { get; set; }

    // public override IList<ITestInterface> ManyToOneIList { get; set; } public override
    // ITestInterface ManyToOneItem { get; set; }

    // public override List<ITestInterface> ManyToOneList { get; set; }

    // public override ITestInterface Map { get; set; }

    // public string NullStringReference { get; set; }

    // public short ShortReference { get; set; }

    // public string StringReference { get; set; } }

    // public class TestClass2Mapping : MappingBaseClass<TestClass2, TestCompositionDatabase> {
    // public TestClass2Mapping() { ID(x => x.ID); Reference(x =>
    // x.NullStringReference).SetMaxLength(100); Reference(x => x.ShortReference); Reference(x =>
    // x.StringReference).SetMaxLength(100); } }

    // public class TestCompositionDatabase : IDatabase { public bool Audit { get { return false; } }

    // public string Name { get { return "Data Source=localhost;Initial
    // Catalog=SessionUsingComposition;Integrated Security=SSPI;Pooling=false"; } }

    // public int Order { get { return 0; } }

    // public bool Readable { get { return true; } }

    // public bool Update { get { return true; } }

    // public bool Writable { get { return true; } } }

    //    public class TestInterfaceMapping : MappingBaseClass<ITestInterface, TestCompositionDatabase>
    //    {
    //        public TestInterfaceMapping()
    //        {
    //            ID(x => x.ID).SetAutoIncrement();
    //            Reference(x => x.BoolReference);
    //            Reference(x => x.ByteArrayReference).SetMaxLength(100);
    //            Reference(x => x.ByteReference);
    //            Reference(x => x.CharReference);
    //            Reference(x => x.DecimalReference).SetMaxLength(8);
    //            Reference(x => x.DoubleReference);
    //            Reference(x => x.EnumReference);
    //            Reference(x => x.FloatReference);
    //            Reference(x => x.GuidReference);
    //            Reference(x => x.IntReference);
    //            Reference(x => x.LongReference);
    //        }
    //    }
    //}
}