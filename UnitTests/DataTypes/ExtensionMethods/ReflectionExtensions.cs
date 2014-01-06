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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Utilities.DataTypes;
using Xunit;

namespace UnitTests.Reflection.ExtensionMethods
{
    public interface TestInterface
    {
        int Value { get; set; }

        int Value2 { get; set; }
    }

    public class ReflectionExtensions
    {
        [Fact]
        public void CallMethodTest()
        {
            int Value = 10;
            Assert.Equal("10", Value.Call<string>("ToString"));
        }

        [Fact]
        public void CreateInstanceTest()
        {
            Assert.NotNull(AssemblyName.GetAssemblyName(new FileInfo(@".\UnitTests.dll").FullName)
                                       .Load()
                                       .Types<TestClass>()
                                       .First()
                                       .Create());
            Assert.IsType<TestClass>(AssemblyName.GetAssemblyName(new FileInfo(@".\UnitTests.dll").FullName)
                                       .Load()
                                       .Types<TestClass>()
                                       .First()
                                       .Create<TestInterface>());
        }

        [Fact]
        public void DumpPropertiesTest()
        {
            List<int> TestObject = new List<int>();
            for (int x = 0; x < 10; ++x)
                TestObject.Add(x);
            Assert.Equal("<table><thead><tr><th>Property Name</th><th>Property Value</th></tr></thead><tbody><tr><td>Capacity</td><td>16</td></tr><tr><td>Count</td><td>10</td></tr><tr><td>Item</td><td></td></tr></tbody></table>", TestObject.ToString(true));
        }

        [Fact]
        public void GetAttribute()
        {
            TestingAttribute TestObject = typeof(TestClass).Attribute<TestingAttribute>();
            Assert.NotNull(TestObject);
        }

        [Fact]
        public void GetAttributes()
        {
            Assert.Equal(1, typeof(TestClass).Attributes<TestingAttribute>().Length);
        }

        [Fact]
        public void GetNameTest()
        {
            Assert.Equal("UnitTests.Reflection.ExtensionMethods.TestClass", typeof(TestClass).GetName());
            Assert.Equal("UnitTests.Reflection.ExtensionMethods.TestClass2", typeof(TestClass2).GetName());
            Assert.Equal("UnitTests.Reflection.ExtensionMethods.TestClass3<System.Int32>", typeof(TestClass3<int>).GetName());
        }

        [Fact]
        public void GetObjectsTest()
        {
            Assert.Equal(2, new DirectoryInfo(@".\").Objects<TestInterface>()
                                                    .Count());
        }

        [Fact]
        public void GetPropertyGetterTest()
        {
            Expression<Func<TestClass, int>> TestObject = typeof(TestClass).GetProperty("Value").PropertyGetter<TestClass, int>();
            TestClass TestObject2 = new TestClass();
            TestObject2.Value = 10;
            Assert.Equal(10, TestObject.Compile()(TestObject2));
        }

        [Fact]
        public void GetPropertyNameTest()
        {
            Expression<Func<TestClass, int>> TestObject = x => x.Value;
            Assert.Equal("Value", TestObject.PropertyName());
            Expression<Func<TestClass, int>> TestObject2 = x => x.Value2;
            Assert.Equal("Value2", TestObject2.PropertyName());
        }

        [Fact]
        public void GetPropertySetterTest()
        {
            Expression<Func<TestClass, int>> TestObject = x => x.Value;
            Expression<Action<TestClass, int>> TestObject2 = TestObject.PropertySetter<TestClass, int>();
            TestClass TestObject3 = new TestClass();
            TestObject2.Compile()(TestObject3, 10);
            Assert.Equal(10, TestObject3.Value);
        }

        [Fact]
        public void GetPropertySetterTest2()
        {
            Expression<Func<TestClass4, int>> TestObject = x => x.Temp.Value;
            Expression<Action<TestClass4, int>> TestObject2 = TestObject.PropertySetter<TestClass4, int>();
            TestClass4 TestObject3 = new TestClass4();
            TestObject2.Compile()(TestObject3, 10);
            Assert.Equal(10, TestObject3.Temp.Value);
        }

        [Fact]
        public void GetPropertyTest()
        {
            Assert.Equal(1, new TestClass().Property("Value"));
            Assert.Equal(2, new TestClass().Property("Value2"));
        }

        [Fact]
        public void GetPropertyTypeTest()
        {
            Assert.Equal(typeof(int), new TestClass().PropertyType("Value"));
            Assert.Equal(typeof(int), new TestClass().PropertyType("Value2"));
        }

        [Fact]
        public void GetTypesTest()
        {
            Assert.Equal(3, AssemblyName.GetAssemblyName(new FileInfo(@".\UnitTests.dll").FullName)
                                        .Load()
                                        .Types<TestInterface>()
                                        .Count());
            Assert.Equal(3, new DirectoryInfo(@".\").LoadAssemblies()
                                                    .Types<TestInterface>()
                                                    .Count());
        }

        [Fact]
        public void HasDefaultConstructor()
        {
            Assert.True(typeof(TestClass).HasDefaultConstructor());
        }

        [Fact]
        public void IsIEnumerableTest()
        {
            List<int> TestObject = new List<int>();
            Assert.True(TestObject.GetType().Is(typeof(IEnumerable)));
        }

        [Fact]
        public void IsOfTypeTest()
        {
            List<int> TestObject = new List<int>();
            Assert.True(TestObject.Is(typeof(List<int>)));
        }

        [Fact]
        public void LoadAssembliesTest()
        {
            Assert.Equal(12, new DirectoryInfo(@".\").LoadAssemblies().Count());
        }

        [Fact]
        public void LoadTest()
        {
            Assert.NotNull(AssemblyName.GetAssemblyName(new FileInfo(@".\Utilities.dll").FullName).Load());
        }

        [Fact]
        public void MakeShallowCopyTest()
        {
            TestClass TestObject1 = new TestClass();
            TestObject1.Value = 3;
            TestObject1.Value3 = "This is a test";
            TestClass TestObject2 = TestObject1.MakeShallowCopy<TestClass>();
            Assert.Equal(TestObject1.Value, TestObject2.Value);
            Assert.Equal(TestObject1.Value2, TestObject2.Value2);
            Assert.Equal(TestObject1.Value3, TestObject2.Value3);
        }

        [Fact]
        public void MakeShallowCopyTest2()
        {
            TestClass TestObject1 = new TestClass();
            TestObject1.Value = 3;
            TestInterface TestObject2 = TestObject1.MakeShallowCopy<TestInterface>();
            Assert.Equal(TestObject1.Value, TestObject2.Value);
            Assert.Equal(TestObject1.Value2, TestObject2.Value2);
        }

        [Fact]
        public void MarkedWith()
        {
            Assert.Equal(1, AssemblyName.GetAssemblyName(new FileInfo(@".\UnitTests.dll").FullName)
                                        .Load()
                                        .Types<TestInterface>()
                                        .MarkedWith<TestingAttribute>()
                                        .Count());
        }

        [Fact]
        public void SetPropertyTest()
        {
            TestClass TestObject1 = new TestClass();
            TestObject1.Property("Value", 3);
            Assert.Equal(3, TestObject1.Value);
        }

        [Fact]
        public void ToLongVersionString()
        {
            Assert.Equal("1.0.0.0", typeof(TestClass).Assembly.ToString(VersionInfo.LongVersion));
        }

        [Fact]
        public void ToShortVersionString()
        {
            Assert.Equal("1.0", typeof(TestClass).Assembly.ToString(VersionInfo.ShortVersion));
        }

        [Fact]
        public void VersionInfo2()
        {
            Assert.Equal("Microsoft.Web.Infrastructure: 1.0\r\nRoslyn.Compilers: 1.2\r\nRoslyn.Compilers.CSharp: 1.2\r\nSystem.Web.Helpers: 3.0\r\nSystem.Web.Mvc: 5.0\r\nSystem.Web.Razor: 3.0\r\nSystem.Web.WebPages: 3.0\r\nSystem.Web.WebPages.Deployment: 3.0\r\nSystem.Web.WebPages.Razor: 3.0\r\nUnitTests: 1.0\r\nUtilities: 4.0\r\nxunit: 1.9\r\n", new DirectoryInfo(@".\").LoadAssemblies().ToString(VersionInfo.ShortVersion));
        }
    }

    [Testing]
    public class TestClass : TestInterface
    {
        public TestClass()
        {
            Value = 1; Value2 = 2;
        }

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
        public TestClass4()
        {
            Temp = new TestClass();
        }

        public TestClass Temp { get; set; }
    }

    public class TestingAttribute : Attribute
    {
    }
}