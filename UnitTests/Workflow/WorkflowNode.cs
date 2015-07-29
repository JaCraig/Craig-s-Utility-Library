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
using Xunit;

namespace Utilities.Tests.Workflow
{
    public class WorkflowNode
    {
        [Fact]
        public void Execute()
        {
            var TempOperation = new Utilities.Workflow.Manager.WorkflowNode<dynamic>();
            TempOperation.AddOperation(new GenericOperation<dynamic>(x => x));
            Assert.Equal(1, TempOperation.Start(1));
            Assert.Equal("A", TempOperation.Start("A"));
        }

        [Fact]
        public void ExecuteFailedConstraint()
        {
            var TempOperation = new Utilities.Workflow.Manager.WorkflowNode<dynamic>();
            TempOperation.AddOperation(new GenericOperation<dynamic>(x => x), new GenericConstraint<dynamic>(x => x > 1));
            Assert.Equal(1, TempOperation.Start(1));
        }

        [Fact]
        public void ExecuteRepeat()
        {
            var TempOperation = new Utilities.Workflow.Manager.WorkflowNode<dynamic>();
            TempOperation.AddOperation(new GenericOperation<dynamic>(x => x + 1));
            TempOperation.Repeat(1);
            Assert.Equal(2, TempOperation.Start(1));
        }

        [Fact]
        public void ExecuteRetry()
        {
            var Rand = new System.Random(1234);
            var TempOperation = new Utilities.Workflow.Manager.WorkflowNode<dynamic>();
            TempOperation.AddOperation(new GenericOperation<dynamic>(x =>
            {
                if (Rand.Next(1, 3) > 1)
                    throw new Exception("ASDF");
                return x;
            }));
            TempOperation.Retry(10000);
            Assert.Equal(1, TempOperation.Start(1));
        }

        [Fact]
        public void ExecuteTrueConstraint()
        {
            var TempOperation = new Utilities.Workflow.Manager.WorkflowNode<dynamic>();
            TempOperation.AddOperation(new GenericOperation<dynamic>(x => x + 1), new GenericConstraint<dynamic>(x => x > 1));
            Assert.Equal(3, TempOperation.Start(2));
        }

        [Fact]
        public void Setup()
        {
            var TempOperation = new Utilities.Workflow.Manager.WorkflowNode<dynamic>();
            TempOperation.AddOperation(new GenericOperation<dynamic>(x => x), new GenericConstraint<dynamic>(x => x > 1));
            Assert.Equal(1, TempOperation.Operations.Count());
            Assert.Equal(0, TempOperation.RepeatCount);
            Assert.Equal(0, TempOperation.RetryCount);
        }
    }
}