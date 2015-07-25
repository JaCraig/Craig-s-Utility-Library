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
using Utilities.DataTypes;
using Utilities.Workflow.Manager.Interfaces;

namespace Utilities.Workflow.Manager
{
    /// <summary>
    /// Workflow node
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    [Serializable]
    public class WorkflowNode<T> : IWorkflowNode<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowNode{T}" /> class.
        /// </summary>
        public WorkflowNode()
        {
            Operations = new List<IInvoker<T>>();
            RepeatCount = 0;
            RetryCount = 0;
        }

        /// <summary>
        /// Gets or sets the operations.
        /// </summary>
        /// <value>The operations.</value>
        public ICollection<IInvoker<T>> Operations { get; private set; }

        /// <summary>
        /// Gets the repeat count
        /// </summary>
        /// <value>The repeat count</value>
        public int RepeatCount { get; private set; }

        /// <summary>
        /// Gets the retry count.
        /// </summary>
        /// <value>The retry count.</value>
        public int RetryCount { get; private set; }

        /// <summary>
        /// Adds an operation to be run with the node
        /// </summary>
        /// <param name="Operation">The operation.</param>
        /// <param name="Constraints">The constraints.</param>
        public void AddOperation(IOperation<T> Operation, params IConstraint<T>[] Constraints)
        {
            Operations.Add(new OperationInvoker<T>(Operation, Constraints));
        }

        /// <summary>
        /// Adds an operation to be run with the node
        /// </summary>
        /// <param name="Operation">The operation.</param>
        /// <param name="Constraints">The constraints.</param>
        public void AddOperation(IWorkflow<T> Operation, params IConstraint<T>[] Constraints)
        {
            Operations.Add(new WorkflowInvoker<T>(Operation, Constraints));
        }

        /// <summary>
        /// Repeats the last operation the specified number of times.
        /// </summary>
        /// <param name="Times">The number of times to repeat</param>
        /// <returns>The workflow object</returns>
        public void Repeat(int Times = 1)
        {
            RepeatCount = Times;
        }

        /// <summary>
        /// Retries the last operation the specified number of times if it fails.
        /// </summary>
        /// <param name="Times">The number of times to retry.</param>
        /// <returns>The workflow object</returns>
        public void Retry(int Times = 1)
        {
            RetryCount = Times;
        }

        /// <summary>
        /// Starts the node using the data specified
        /// </summary>
        /// <param name="Data">The data.</param>
        /// <returns>The result from the workflow node</returns>
        public T Start(T Data)
        {
            int CurrentRetryCount = 0;
            T ReturnValue = default(T);
            for (int x = 0; x <= RepeatCount; ++x)
            {
                try
                {
                    ReturnValue = Operations.ForEachParallel(y => y.Execute(Data)).First();
                }
                catch
                {
                    if (CurrentRetryCount >= RetryCount)
                        throw;
                    ++CurrentRetryCount;
                }
            }
            return ReturnValue;
        }
    }
}