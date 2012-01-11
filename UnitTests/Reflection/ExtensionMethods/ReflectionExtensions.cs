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
using Utilities.Reflection.ExtensionMethods;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;

namespace UnitTests.Reflection.ExtensionMethods
{
    public class ReflectionExtensions
    {
        [Test]
        public void CallMethodTest()
        {
            int Value = 10;
            Assert.Equal("10", Value.CallMethod<string>("ToString"));
        }

        [Test]
        public void DumpPropertiesTest()
        {
            List<int> TestObject = new List<int>();
            for (int x = 0; x < 10; ++x)
                TestObject.Add(x);
            Assert.Equal("<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody><tr><td>Capacity</td><td>16</td></tr><tr><td>Count</td><td>10</td></tr><tr><td>Item</td><td></td></tr></tbody></table>", TestObject.DumpProperties());
        }

        [Test]
        public void IsIEnumerableTest()
        {
            List<int> TestObject = new List<int>();
            Assert.True(TestObject.GetType().IsIEnumerable());
        }

        [Test]
        public void IsOfTypeTest()
        {
            List<int> TestObject = new List<int>();
            Assert.True(TestObject.IsOfType(typeof(List<int>)));
        }

        [Test]
        public void LoadAssembliesTest()
        {
            Assert.Equal(11, new DirectoryInfo(@".\").LoadAssemblies().Count());
        }

        [Test]
        public void LoadTest()
        {
            Assert.NotNull(AssemblyName.GetAssemblyName(new FileInfo(@".\Utilities.dll").FullName).Load());
        }

        [Test]
        public void GetTypesTest()
        {
            Assert.Equal(3, AssemblyName.GetAssemblyName(new FileInfo(@".\UnitTests.dll").FullName)
                                        .Load()
                                        .GetTypes<TestInterface>()
                                        .Count());
            Assert.Equal(3, new DirectoryInfo(@".\").LoadAssemblies()
                                                    .GetTypes<TestInterface>()
                                                    .Count());
        }

        [Test]
        public void CreateInstanceTest()
        {
            Assert.NotNull(AssemblyName.GetAssemblyName(new FileInfo(@".\UnitTests.dll").FullName)
                                       .Load()
                                       .GetTypes<TestClass>()
                                       .First()
                                       .CreateInstance());
            Assert.OfType<TestClass>(AssemblyName.GetAssemblyName(new FileInfo(@".\UnitTests.dll").FullName)
                                       .Load()
                                       .GetTypes<TestClass>()
                                       .First()
                                       .CreateInstance<TestInterface>());
        }

        [Test]
        public void GetObjectsTest()
        {
            Assert.Equal(2, new DirectoryInfo(@".\").GetObjects<TestInterface>()
                                                    .Count());
        }

        [Test]
        public void MakeShallowCopyTest()
        {
            TestClass TestObject1=new TestClass();
            TestObject1.Value=3;
            TestObject1.Value3 = "This is a test";
            TestClass TestObject2=TestObject1.MakeShallowCopy<TestClass>();
            Assert.Equal(TestObject1.Value, TestObject2.Value);
            Assert.Equal(TestObject1.Value2, TestObject2.Value2);
            Assert.Equal(TestObject1.Value3, TestObject2.Value3);
        }

        [Test]
        public void MakeShallowCopyTest2()
        {
            TestClass TestObject1 = new TestClass();
            TestObject1.Value = 3;
            TestInterface TestObject2 = TestObject1.MakeShallowCopy<TestInterface>();
            Assert.Equal(TestObject1.Value, TestObject2.Value);
            Assert.Equal(TestObject1.Value2, TestObject2.Value2);
        }

        [Test]
        public void SetPropertyTest()
        {
            TestClass TestObject1 = new TestClass();
            TestObject1.SetProperty("Value", 3);
            Assert.Equal(3, TestObject1.Value);
        }

        [Test]
        public void GetNameTest()
        {
            Assert.Equal("TestClass", typeof(TestClass).GetName());
            Assert.Equal("TestClass2", typeof(TestClass2).GetName());
            Assert.Equal("TestClass3<Int32>", typeof(TestClass3<int>).GetName());
        }

        [Test]
        public void GetPropertyTypeTest()
        {
            Assert.Equal(typeof(int), new TestClass().GetPropertyType("Value"));
            Assert.Equal(typeof(int), new TestClass().GetPropertyType("Value2"));
        }

        [Test]
        public void GetPropertyTest()
        {
            Assert.Equal(1, new TestClass().GetProperty("Value"));
            Assert.Equal(2, new TestClass().GetProperty("Value2"));
        }

        [Test]
        public void GetPropertyNameTest()
        {
            Expression<Func<TestClass, int>> TestObject = x => x.Value;
            Assert.Equal("Value", TestObject.GetPropertyName());
            Expression<Func<TestClass, int>> TestObject2 = x => x.Value2;
            Assert.Equal("Value2", TestObject2.GetPropertyName());
        }

        [Test]
        public void GetPropertySetterTest()
        {
            Expression<Func<TestClass, int>> TestObject = x => x.Value;
            Expression<Action<TestClass, int>> TestObject2 = TestObject.GetPropertySetter();
            TestClass TestObject3=new TestClass();
            TestObject2.Compile()(TestObject3, 10);
            Assert.Equal(10, TestObject3.Value);
        }

        [Test]
        public void GetPropertySetterTest2()
        {
            Expression<Func<TestClass4, int>> TestObject = x => x.Temp.Value;
            Expression<Action<TestClass4, int>> TestObject2 = TestObject.GetPropertySetter();
            TestClass4 TestObject3 = new TestClass4();
            TestObject2.Compile()(TestObject3, 10);
            Assert.Equal(10, TestObject3.Temp.Value);
        }

        [Test]
        public void GetPropertyGetterTest()
        {
            Expression<Func<TestClass, int>> TestObject = typeof(TestClass).GetProperty("Value").GetPropertyGetter<TestClass, int>();
            TestClass TestObject2 = new TestClass();
            TestObject2.Value = 10;
            Assert.Equal(10, TestObject.Compile()(TestObject2));
        }

        [Test]
        public void ToLongVersionString()
        {
            Assert.Equal("1.0.0.0", typeof(TestClass).Assembly.ToLongVersionString());
        }

        [Test]
        public void ToShortVersionString()
        {
            Assert.Equal("1.0", typeof(TestClass).Assembly.ToShortVersionString());
        }

        [Test]
        public void GetAttributes()
        {
            Assert.Equal(1, typeof(TestClass).GetAttributes<TestingAttribute>().Length);
        }

        [Test]
        public void GetAttribute()
        {
            TestingAttribute TestObject = typeof(TestClass).GetAttribute<TestingAttribute>();
            Assert.NotNull(TestObject);
        }

        [Test]
        public void MarkedWith()
        {
            Assert.Equal(1, AssemblyName.GetAssemblyName(new FileInfo(@".\UnitTests.dll").FullName)
                                        .Load()
                                        .GetTypes<TestInterface>()
                                        .MarkedWith<TestingAttribute>()
                                        .Count());
        }
    }

    public class TestingAttribute : Attribute
    {
    }

    [Testing]
    public class TestClass : TestInterface
    {
        public TestClass() { Value = 1; Value2 = 2; }
        public int Value { get; set; }
        public int Value2 { get; set; }
        public string Value3 = "ASDF";
    }

    public class TestClass2 : TestInterface
    {
        public int Value { get; set; }
        public int Value2 { get; set; }
    }

    public class TestClass3<T> : TestInterface
    {
        public int Value { get; set; }
        public int Value2 { get; set; }
    }

    public class TestClass4
    {
        public TestClass4() { Temp = new TestClass(); }
        public TestClass Temp { get; set; }
    }

    public interface TestInterface
    {
        int Value { get; set; }
        int Value2 { get; set; }
    }
}
