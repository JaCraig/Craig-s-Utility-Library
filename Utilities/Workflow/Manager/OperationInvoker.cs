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
using Utilities.Workflow.Manager.Interfaces;

namespace Utilities.Workflow.Manager
{
    /// <summary>
    /// Operation invoker
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    [Serializable]
    public class OperationInvoker<T> : IOperationInvoker<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationInvoker{T}" /> class.
        /// </summary>
        /// <param name="Operation">The operation.</param>
        /// <param name="Constraints">The constraints.</param>
        public OperationInvoker(IOperation<T> Operation, IEnumerable<IConstraint<T>> Constraints)
        {
            this.Operation = Operation;
            this.Constraints = Constraints;
        }

        /// <summary>
        /// Gets the constraints.
        /// </summary>
        /// <value>The constraints.</value>
        public IEnumerable<IConstraint<T>> Constraints { get; private set; }

        /// <summary>
        /// Gets the operation.
        /// </summary>
        /// <value>The operation.</value>
        public IOperation<T> Operation { get; private set; }

        /// <summary>
        /// Executes the operation on the specified value.
        /// </summary>
        /// <param name="Value">The value.</param>
        /// <returns>The result of the operation</returns>
        public T Execute(T Value)
        {
            if (!Constraints.All(x => x.Eval((T)Value)))
                return Value;
            return Operation.Execute((T)Value);
        }
    }
}