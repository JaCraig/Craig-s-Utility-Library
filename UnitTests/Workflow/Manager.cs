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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Workflow;
using Utilities.Workflow.Manager;
using Utilities.Workflow.Manager.Interfaces;
using Xunit;

namespace Utilities.Tests.Workflow
{
    public class Manager
    {
        [Fact]
        public void CreateWorkflow()
        {
            using (Utilities.Workflow.Manager.Manager TempOperation = new Utilities.Workflow.Manager.Manager(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.FileSystem.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.Serializers.Manager>()))
            {
                IWorkflow<dynamic> Workflow = TempOperation.CreateWorkflow<dynamic>("ASDF");
                Assert.NotNull(Workflow);
                Assert.Equal("ASDF", Workflow.Name);
                Assert.Equal(typeof(object), Workflow.DataType);
            }
            Assert.True(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Workflows.obj").Exists);
            new Utilities.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/").Delete();
        }

        [Fact]
        public void RemoveWorkflow()
        {
            using (Utilities.Workflow.Manager.Manager TempOperation = new Utilities.Workflow.Manager.Manager(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.FileSystem.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.Serializers.Manager>()))
            {
                IWorkflow<dynamic> Workflow = TempOperation.CreateWorkflow<dynamic>("ASDF");
                Assert.NotNull(Workflow);
                Assert.Equal("ASDF", Workflow.Name);
                Assert.Equal(typeof(object), Workflow.DataType);
                TempOperation.RemoveWorkflow(Workflow);
            }
            Assert.True(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Workflows.obj").Exists);
            using (Utilities.Workflow.Manager.Manager TempOperation = new Utilities.Workflow.Manager.Manager(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.FileSystem.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.Serializers.Manager>()))
            {
                IWorkflow<dynamic> Workflow = TempOperation.CreateWorkflow<dynamic>("ASDF");
                Assert.Equal("ASDF", Workflow.Name);
                Assert.Equal(typeof(object), Workflow.DataType);
                Assert.Equal(1, Workflow.Start(1));
            }
            Assert.True(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Workflows.obj").Exists);
            new Utilities.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/").Delete();
        }

        [Fact]
        public void Serialization()
        {
            using (Utilities.Workflow.Manager.Manager TempOperation = new Utilities.Workflow.Manager.Manager(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.FileSystem.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.Serializers.Manager>()))
            {
                IWorkflow<dynamic> Workflow = TempOperation.CreateWorkflow<dynamic>("ASDF")
                    .Do(x => x + 1)
                    .Do(x => x + 1, new GenericConstraint<dynamic>(x => x > 0));
                Assert.Equal("ASDF", Workflow.Name);
                Assert.Equal(typeof(object), Workflow.DataType);
                Assert.Equal(3, Workflow.Start(1));
            }
            Assert.True(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/Workflows.obj").Exists);
            using (Utilities.Workflow.Manager.Manager TempOperation = new Utilities.Workflow.Manager.Manager(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.FileSystem.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.Serializers.Manager>()))
            {
                IWorkflow<dynamic> Workflow = TempOperation.CreateWorkflow<dynamic>("ASDF");
                Assert.Equal("ASDF", Workflow.Name);
                Assert.Equal(typeof(object), Workflow.DataType);
                Assert.Equal(3, Workflow.Start(1));
            }
            new Utilities.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/").Delete();
        }
    }
}