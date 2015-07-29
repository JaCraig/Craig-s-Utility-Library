using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utilities.DataTypes;
using Xunit;

namespace UnitTests.IoC.Default
{
    public class DefaultBootstrapper
    {
        [Fact]
        public void CascadeConstructor()
        {
            var Temp = new Utilities.IoC.Default.DefaultBootstrapper(AppDomain.CurrentDomain.GetAssemblies(), AppDomain.CurrentDomain.GetAssemblies().Types());
            Temp.RegisterAll<ITestClass>();
            Temp.Register<TestClass3>();
            Temp.Register<TestClass4>();
            TestClass4 Object = Temp.Resolve<TestClass4>();
            Assert.NotNull(Object);
            Assert.NotNull(Object.Class);
            Assert.Equal(2, Object.Class.Classes.Count());
        }

        [Fact]
        public void Creation()
        {
            var Temp = new Utilities.IoC.Default.DefaultBootstrapper(AppDomain.CurrentDomain.GetAssemblies(), AppDomain.CurrentDomain.GetAssemblies().Types());
            Assert.Equal("Default bootstrapper", Temp.Name);
        }

        [Fact]
        public void IEnumerableConstructor()
        {
            var Temp = new Utilities.IoC.Default.DefaultBootstrapper(AppDomain.CurrentDomain.GetAssemblies(), AppDomain.CurrentDomain.GetAssemblies().Types());
            Temp.RegisterAll<ITestClass>();
            Temp.Register<TestClass3>();
            TestClass3 Object = Temp.Resolve<TestClass3>();
            Assert.NotNull(Object);
            Assert.Equal(2, Object.Classes.Count());
        }

        [Fact]
        public void Register()
        {
            var Temp = new Utilities.IoC.Default.DefaultBootstrapper(AppDomain.CurrentDomain.GetAssemblies(), AppDomain.CurrentDomain.GetAssemblies().Types());
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
            var Temp = new Utilities.IoC.Default.DefaultBootstrapper(new Assembly[] { typeof(DefaultBootstrapper).Assembly }, typeof(DefaultBootstrapper).Assembly.GetTypes());
            Temp.RegisterAll<ITestClass>();
            Assert.Null(Temp.Resolve<ITestClass>());
            Assert.Equal(2, Temp.ResolveAll<ITestClass>().Count());
            Assert.NotNull(Temp.Resolve<TestClass>());
            Assert.NotNull(Temp.Resolve<TestClass2>());
        }

        [Fact]
        public void Resolve()
        {
            var Temp = new Utilities.IoC.Default.DefaultBootstrapper(AppDomain.CurrentDomain.GetAssemblies(), AppDomain.CurrentDomain.GetAssemblies().Types());
            Temp.Register(new TestClass() { A = 12 });
            Assert.Equal(12, Temp.Resolve<TestClass>().A);
            Assert.Equal(12, Temp.Resolve<TestClass>("").A);
            Assert.Equal(12, ((TestClass)Temp.Resolve(typeof(TestClass), "")).A);
        }

        [Fact]
        public void ResolveAll()
        {
            var Temp = new Utilities.IoC.Default.DefaultBootstrapper(AppDomain.CurrentDomain.GetAssemblies(), AppDomain.CurrentDomain.GetAssemblies().Types());
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

        protected class TestClass2 : ITestClass
        {
            public int A { get; set; }
        }

        protected class TestClass3
        {
            public TestClass3()
            {
            }

            public TestClass3(IEnumerable<ITestClass> Classes)
            {
                this.Classes = Classes;
            }

            public IEnumerable<ITestClass> Classes { get; set; }
        }

        protected class TestClass4
        {
            public TestClass4()
            {
            }

            public TestClass4(TestClass3 Class)
            {
                this.Class = Class;
            }

            public TestClass3 Class { get; set; }
        }
    }
}