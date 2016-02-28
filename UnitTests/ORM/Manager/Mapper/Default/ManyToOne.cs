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

THE SOFTWARE IS PROVManyToOneED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using System.Collections.Generic;
using Utilities.ORM.BaseClasses;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Xunit;

namespace UnitTests.ORM.Manager.Mapper.Default
{
    public class ManyToOne : DatabaseBaseClass
    {
        [Fact]
        public void CascadeDelete()
        {
            var TempObject = new TestClass();
            TempObject.A = new TestClass();
            TempObject.ID = 1;
            var TestObject = new Utilities.ORM.Manager.Mapper.Default.ManyToOne<TestClass, TestClass>(x => x.A, null);
            IBatch Result = TestObject.CascadeDelete(TempObject, new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data Source=localhost;Initial Catalog=TestDatabase8;Integrated Security=SSPI;Pooling=false"), new List<object>());
            Assert.NotNull(Result);
            Assert.Equal("DELETE FROM TestClass_ WHERE ID=0", Result.ToString());
            Assert.Equal(1, Result.CommandCount);
        }

        [Fact]
        public void CascadeJoinsDelete()
        {
            var TempObject = new TestClass();
            TempObject.A = new TestClass();
            TempObject.ID = 1;
            var TestObject = new Utilities.ORM.Manager.Mapper.Default.ManyToOne<TestClass, TestClass>(x => x.A, new TestClassMapping());
            TestObject.ForeignMapping = new TestClassMapping();
            IBatch Result = TestObject.CascadeJoinsDelete(TempObject, new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data Source=localhost;Initial Catalog=TestDatabase8;Integrated Security=SSPI;Pooling=false"), new List<object>());
            Assert.NotNull(Result);
            Assert.Equal("DELETE FROM TestClass_TestClass WHERE TestClass_ID2=1", Result.ToString());
            Assert.Equal(1, Result.CommandCount);
        }

        [Fact]
        public void CascadeJoinsSave()
        {
            var TempObject = new TestClass();
            TempObject.A = new TestClass();
            TempObject.ID = 1;
            var TestObject = new Utilities.ORM.Manager.Mapper.Default.ManyToOne<TestClass, TestClass>(x => x.A, new TestClassMapping());
            TestObject.ForeignMapping = new TestClassMapping();
            IBatch Result = TestObject.CascadeJoinsSave(TempObject, new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data Source=localhost;Initial Catalog=TestDatabase8;Integrated Security=SSPI;Pooling=false"), new List<object>());
            Assert.NotNull(Result);
            Assert.Equal("INSERT INTO TestClass_TestClass(TestClass_ID,TestClass_ID2) VALUES (0,1)", Result.ToString());
            Assert.Equal(1, Result.CommandCount);
        }

        [Fact]
        public void CascadeSave()
        {
            var TempObject = new TestClass();
            TempObject.A = new TestClass();
            TempObject.ID = 1;
            var TestObject = new Utilities.ORM.Manager.Mapper.Default.ManyToOne<TestClass, TestClass>(x => x.A, new TestClassMapping());
            IBatch Result = TestObject.CascadeSave(TempObject, new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>()).GetSource("Data Source=localhost;Initial Catalog=TestDatabase8;Integrated Security=SSPI;Pooling=false"), new List<object>());
            Assert.NotNull(Result);
            Assert.Equal("INSERT INTO TestClass_() VALUES() SELECT scope_identity() as [ID]", Result.ToString());
            Assert.Equal(1, Result.CommandCount);
        }

        [Fact]
        public void Create()
        {
            var TestObject = new Utilities.ORM.Manager.Mapper.Default.ManyToOne<TestClass, TestClass>(x => x.A, null);
            Assert.False(TestObject.AutoIncrement);
            Assert.False(TestObject.Cascade);
            Assert.NotNull(TestObject.CompiledExpression);
            Assert.NotNull(TestObject.DefaultValue);
            Assert.Null(TestObject.DefaultValue());
            Assert.Equal("_ADerived", TestObject.DerivedFieldName);
            Assert.NotNull(TestObject.Expression);
            Assert.Equal(null, TestObject.FieldName);
            Assert.Null(TestObject.ForeignMapping);
            Assert.False(TestObject.Index);
            Assert.Null(TestObject.Mapping);
            Assert.Equal(0, TestObject.MaxLength);
            Assert.Equal("A", TestObject.Name);
            Assert.False(TestObject.NotNull);
            Assert.Equal("TestClass_TestClass", TestObject.TableName);
            Assert.Equal(typeof(TestClass), TestObject.Type);
            Assert.False(TestObject.Unique);
        }

        public class TestClass
        {
            public TestClass A { get; set; }

            public int ID { get; set; }
        }

        public class TestClassDatabase : IDatabase
        {
            public bool Audit
            {
                get { return false; }
            }

            public string Name
            {
                get { return "Data Source=localhost;Initial Catalog=TestDatabase8;Integrated Security=SSPI;Pooling=false"; }
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
                get { return false; }
            }

            public bool Writable
            {
                get { return false; }
            }
        }

        public class TestClassMapping : MappingBaseClass<TestClass, TestClassDatabase>
        {
            public TestClassMapping()
            {
                ID(x => x.ID).SetFieldName("ID").SetAutoIncrement();
                ManyToOne(x => x.A).SetCascade();
            }
        }
    }
}