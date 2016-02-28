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

using Ironman.Core.API.Manager.BaseClasses;
using Ironman.Core.API.Manager.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Utilities.DataTypes;
using Xunit;

namespace IronmanTests.API
{
    public class Manager
    {
        [Fact]
        public void All()
        {
            var TestObject = new Ironman.Core.API.Manager.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IAPIMapping>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IService>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IWorkflowModule>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.Workflow.Manager.Manager>());
            List<Dynamo> Objects = TestObject.All(1, "TestClass").Select(x => (Dynamo)x).ToList();
            Assert.Equal("ASDFG", Objects[0]["A"]);
            Assert.Equal(10, Objects[0]["B"]);
            Assert.Equal(2.13f, Objects[0]["C"]);
            Assert.Equal("/10/D", Objects[0]["D"]);
            Assert.Equal("/10/E", Objects[0]["E"]);
            Assert.Equal("ZXCV", Objects[1]["A"]);
            Assert.Equal(212, Objects[1]["B"]);
            Assert.Equal(213f, Objects[1]["C"]);
            Assert.Equal("/212/D", Objects[1]["D"]);
            Assert.Equal("/212/E", Objects[1]["E"]);
        }

        [Fact]
        public void Any()
        {
            var TestObject = new Ironman.Core.API.Manager.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IAPIMapping>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IService>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IWorkflowModule>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.Workflow.Manager.Manager>());
            Dynamo Object = TestObject.Any(1, "TestClass", "A");
            Assert.Equal("ASDFG", Object["A"]);
            Assert.Equal(10, Object["B"]);
            Assert.Equal(2.13f, Object["C"]);
            Assert.Equal("/D", Object["D"]);
            Assert.Equal("/E", Object["E"]);
        }

        [Fact]
        public void AnyEmbedded()
        {
            var TestObject = new Ironman.Core.API.Manager.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IAPIMapping>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IService>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IWorkflowModule>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.Workflow.Manager.Manager>());
            Dynamo Object = TestObject.Any(1, "TestClass", "A", "E");
            Assert.Equal("ASDFG", Object["A"]);
            Assert.Equal(10, Object["B"]);
            Assert.Equal(2.13f, Object["C"]);
            Assert.Equal("/D", Object["D"]);
            Assert.NotNull(Object["E"]);
        }

        [Fact]
        public void Create()
        {
            new Ironman.Core.API.Manager.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IAPIMapping>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IService>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IWorkflowModule>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.Workflow.Manager.Manager>());
        }

        [Fact]
        public void Delete()
        {
            var TestObject = new Ironman.Core.API.Manager.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IAPIMapping>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IService>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IWorkflowModule>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.Workflow.Manager.Manager>());
            Dynamo Object = TestObject.Delete(1, "TestClass", "A");
            Assert.Equal("Object deleted successfully", Object["Message"]);
            Assert.Equal("Success", Object["Status"]);
        }

        [Fact]
        public void Save()
        {
            var TestObject = new Ironman.Core.API.Manager.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IAPIMapping>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IService>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IWorkflowModule>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.Workflow.Manager.Manager>());
            Dynamo Object = TestObject.Save(1, "TestClass", new Dynamo[] { new Dynamo() });
            Assert.Equal("Object saved successfully", Object["Message"]);
            Assert.Equal("Success", Object["Status"]);
        }

        [Fact]
        public void Service()
        {
            var TestObject = new Ironman.Core.API.Manager.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IAPIMapping>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IService>(), Utilities.IoC.Manager.Bootstrapper.ResolveAll<IWorkflowModule>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.Workflow.Manager.Manager>());
            Dynamo Value = TestObject.CallService(1, "TestService", new Dynamo(new { A = 1 }));
            Assert.NotNull(Value);
            Assert.Equal(1, Value["A"]);
        }

        public class TestClass
        {
            public string A { get; set; }

            public int B { get; set; }

            public float C { get; set; }

            public TestClass D { get; set; }

            public List<TestClass> E { get; set; }
        }

        public class TestClassMapping : APIMappingBaseClass<TestClass, int>
        {
            public TestClassMapping()
            {
                ID(x => x.B);
                Reference(x => x.A);
                Reference(x => x.C);
                Map(x => x.D);
                Map(x => x.E);
                this.SetAll(() => new TestClass[] { new TestClass { A = "ASDFG", B = 10, C = 2.13f }, new TestClass { A = "ZXCV", B = 212, C = 213f } }.ToList());
                this.SetAny(x => new TestClass { A = "ASDFG", B = 10, C = 2.13f });
                this.SetCanGet(x => true);
                this.SetCanDelete(x => true);
                this.SetCanSave(x => true);
                this.SetDelete(x => true);
                this.SetSave(x => true);
            }
        }

        public class TestService : IService
        {
            public string Name
            {
                get { return "TestService"; }
            }

            public IEnumerable<int> Versions
            {
                get { return new int[] { 1 }; }
            }

            public Dynamo Process(Dynamo Value)
            {
                return Value;
            }
        }
    }
}