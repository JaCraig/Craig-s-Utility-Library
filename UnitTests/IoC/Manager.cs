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
using Xunit;

using Utilities.IoC.Mappings.BaseClasses;
using Utilities.IoC.Providers.Interfaces;
using Utilities.IoC.Providers.Scope;

namespace UnitTests.IoC
{
    public class Manager
    {
        [Fact]
        public void SetupTest()
        {
            Utilities.IoC.Manager TestObject = new Utilities.IoC.Manager();
            Assert.DoesNotThrow(() => TestObject.Setup(typeof(Manager).Assembly));
        }

        [Fact]
        public void GetTest1()
        {
            Utilities.IoC.Manager TestObject = new Utilities.IoC.Manager();
            TestObject.Setup(typeof(Manager).Assembly);
            TestClass1 Object = TestObject.Get<TestClass1>();
            Assert.NotNull(Object);
        }

        [Fact]
        public void GetTest2()
        {
            Utilities.IoC.Manager TestObject = new Utilities.IoC.Manager();
            TestObject.Setup(typeof(Manager).Assembly);
            TestClass2 Object = TestObject.Get<TestClass2>();
            Assert.NotNull(Object);
            Assert.NotNull(Object.Value1);
        }
    }

    public class Module : BaseModule
    {
        public override void Setup()
        {
            Map<ITestInterface>().To<TestClass1>().SetScope(new StandardScope());
            Map<TestClass1>().To<TestClass1>(() => new TestClass1());
            Map<ITestInterface, MyAttribute>().To(new TestImplementation());
            Map<TestClass2>().To<TestClass2>();
        }
    }

    public class MyAttribute : Attribute
    {
    }

    public interface ITestInterface
    {
    }

    public class TestClass2 : ITestInterface
    {
        public TestClass2(TestClass1 Value1) { this.Value1 = Value1; }
        public virtual TestClass1 Value1 { get; set; }
    }

    public class TestClass1 : ITestInterface
    {
        public TestClass1() { }
    }

    public class TestImplementation : IImplementation
    {
        public Type ReturnType
        {
            get { return typeof(TestClass1); }
        }

        public object Create()
        {
            return new TestClass1();
        }
    }
}
