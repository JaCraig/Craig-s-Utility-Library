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

namespace Utilities.Workflow.Manager.Interfaces
{
    /// <summary>
    /// Workflow node interface
    /// </summary>
    /// <typeparam name="T">Data type expected</typeparam>
    public interface IWorkflowNode<T>
    {
        /// <summary>
        /// Adds an operation to be run with the node
        /// </summary>
        /// <param name="Operation">The operation.</param>
        /// <param name="Constraints">The constraints.</param>
        void AddOperation(IOperation<T> Operation, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Adds an operation to be run with the node
        /// </summary>
        /// <param name="Operation">The operation.</param>
        /// <param name="Constraints">The constraints.</param>
        void AddOperation(IWorkflow<T> Operation, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Repeats the last operation the specified number of times.
        /// </summary>
        /// <param name="Times">The number of times to repeat</param>
        /// <returns>The workflow object</returns>
        void Repeat(int Times = 1);

        /// <summary>
        /// Retries the last operation the specified number of times if it fails.
        /// </summary>
        /// <param name="Times">The number of times to retry.</param>
        /// <returns>The workflow object</returns>
        void Retry(int Times = 1);

        /// <summary>
        /// Starts the node using the data specified
        /// </summary>
        /// <param name="Data">The data.</param>
        /// <returns>The result from the workflow node</returns>
        T Start(T Data);
    }
}