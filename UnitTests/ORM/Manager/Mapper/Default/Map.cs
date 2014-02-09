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

THE SOFTWARE IS PROVMapED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using System.Collections.Generic;
using System.Data;
using Utilities.ORM.BaseClasses;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.Schema.Default.Database;
using Xunit;

namespace UnitTests.ORM.Manager.Mapper.Default
{
    public class Map
    {
        [Fact]
        public void Create()
        {
            Utilities.ORM.Manager.Mapper.Default.Map<TestClass, TestClass> TestObject = new Utilities.ORM.Manager.Mapper.Default.Map<TestClass, TestClass>(x => x.A, new TestClassMapping());
            Assert.False(TestObject.AutoIncrement);
            Assert.False(TestObject.Cascade);
            Assert.NotNull(TestObject.CompiledExpression);
            Assert.NotNull(TestObject.DefaultValue);
            Assert.Null(TestObject.DefaultValue());
            Assert.Equal("_ADerived", TestObject.DerivedFieldName);
            Assert.NotNull(TestObject.Expression);
            Assert.Equal("TestClass_A_ID", TestObject.FieldName);
            Assert.Null(TestObject.ForeignMapping);
            Assert.False(TestObject.Index);
            Assert.NotNull(TestObject.Mapping);
            Assert.Equal(0, TestObject.MaxLength);
            Assert.Equal("A", TestObject.Name);
            Assert.False(TestObject.NotNull);
            Assert.Equal("TestClass_", TestObject.TableName);
            Assert.Equal(typeof(TestClass), TestObject.Type);
            Assert.False(TestObject.Unique);
        }

        private class Database : IDatabase
        {
            public bool Audit
            {
                get { return false; }
            }

            public string Name
            {
                get { return "MapTest"; }
            }

            public int Order
            {
                get { return 0; }
            }

            public bool Readable
            {
                get { return false; }
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

        private class TestClass
        {
            public TestClass A { get; set; }
        }

        private class TestClassMapping : MappingBaseClass<TestClass, Database>
        {
            public TestClassMapping()
            {
            }
        }
    }
}