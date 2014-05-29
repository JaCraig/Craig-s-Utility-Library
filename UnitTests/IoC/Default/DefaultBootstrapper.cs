using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace UnitTests.IoC.Default
{
    public class DefaultBootstrapper
    {
        [Fact]
        public void Creation()
        {
            Utilities.IoC.Default.DefaultBootstrapper Temp = new Utilities.IoC.Default.DefaultBootstrapper(AppDomain.CurrentDomain.GetAssemblies());
            Assert.Equal("Default bootstrapper", Temp.Name);
        }

        [Fact]
        public void Register()
        {
            Utilities.IoC.Default.DefaultBootstrapper Temp = new Utilities.IoC.Default.DefaultBootstrapper(AppDomain.CurrentDomain.GetAssemblies());
            Temp.Register(new TestClass() { A = 12 });
            Assert.Equal(12, Temp.Resolve<TestClass>().A);
            Temp.Register<TestClass>();
            Assert.Equal(0, Temp.Resolve<TestClass>().A);
            Temp.Register(() => new TestClass() { A = 12 });
            Assert.Equal(12, Temp.Resolve<TestClass>().A);
            Temp.Register<ITestClass, TestClass>();
            Assert.Equal(0, Temp.Resolve<ITestClass>().A);
            Temp.Register(new TestClass() { A = 21 }, "Test");
            Assert.Equal(21, Temp.Resolve<TestClass>("Test").A);
            Assert.Equal(0, Temp.Resolve<ITestClass>().A);
            Assert.Equal(12, Temp.Resolve<TestClass>().A);
        }

        [Fact]
        public void RegisterAll()
        {
            Utilities.IoC.Default.DefaultBootstrapper Temp = new Utilities.IoC.Default.DefaultBootstrapper(new Assembly[] { typeof(DefaultBootstrapper).Assembly });
            Temp.RegisterAll<ITestClass>();
            Assert.Null(Temp.Resolve<ITestClass>());
            Assert.Equal(1, Temp.ResolveAll<ITestClass>().Count());
        }

        [Fact]
        public void Resolve()
        {
            Utilities.IoC.Default.DefaultBootstrapper Temp = new Utilities.IoC.Default.DefaultBootstrapper(AppDomain.CurrentDomain.GetAssemblies());
            Temp.Register(new TestClass() { A = 12 });
            Assert.Equal(12, Temp.Resolve<TestClass>().A);
            Assert.Equal(12, Temp.Resolve<TestClass>("").A);
            Assert.Equal(12, ((TestClass)Temp.Resolve(typeof(TestClass), "")).A);
        }

        [Fact]
        public void ResolveAll()
        {
            Utilities.IoC.Default.DefaultBootstrapper Temp = new Utilities.IoC.Default.DefaultBootstrapper(AppDomain.CurrentDomain.GetAssemblies());
            Temp.Register(new TestClass() { A = 12 });
            Temp.Register(new TestClass() { A = 13 }, "A");
            Temp.Register(new TestClass() { A = 14 }, "B");
            IEnumerable<TestClass> Objects = Temp.ResolveAll<TestClass>();
            Assert.Equal(3, Objects.Count());
            foreach (TestClass Object in Objects)
            {
                Assert.Contains(Object.A, new int[] { 12, 13, 14 });
            }
        }

        protected interface ITestClass
        {
            int A { get; set; }
        }

        protected class TestClass : ITestClass
        {
            public int A { get; set; }
        }
    }
}