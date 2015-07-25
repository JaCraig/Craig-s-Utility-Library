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
using Utilities.DataTypes.Patterns;

namespace Utilities.Workflow.Manager.Interfaces
{
    /// <summary>
    /// Workflow interface
    /// </summary>
    public interface IWorkflow : IFluentInterface
    {
        /// <summary>
        /// Gets the type of the data expected
        /// </summary>
        /// <value>
        /// The type of the data expected
        /// </value>
        Type DataType { get; }

        /// <summary>
        /// Gets the name of the workflow
        /// </summary>
        /// <value>
        /// The name of the workflow
        /// </value>
        string Name { get; }
    }

    /// <summary>
    /// Workflow interface
    /// </summary>
    /// <typeparam name="T">Data type expected</typeparam>
    public interface IWorkflow<T> : IWorkflow
    {
        /// <summary>
        /// Causes multiple commands to be executed concurrently
        /// </summary>
        /// <typeparam name="OperationType">The type of the operation.</typeparam>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> And<OperationType>(params IConstraint<T>[] Constraints)
            where OperationType : IOperation<T>, new();

        /// <summary>
        /// Causes multiple commands to be executed concurrently
        /// </summary>
        /// <param name="Operation">The operation to add.</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> And(IOperation<T> Operation, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Causes multiple commands to be executed concurrently
        /// </summary>
        /// <param name="Operation">The operation to add.</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> And(Func<T, T> Operation, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Causes multiple commands to be executed concurrently
        /// </summary>
        /// <param name="Workflow">The workflow to append</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The resulting workflow object</returns>
        IWorkflow<T> And(IWorkflow<T> Workflow, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Adds an instance of the specified operation type to the workflow
        /// </summary>
        /// <typeparam name="OperationType">The type of the operation.</typeparam>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> Do<OperationType>(params IConstraint<T>[] Constraints)
            where OperationType : IOperation<T>, new();

        /// <summary>
        /// Adds the specified operation to the workflow
        /// </summary>
        /// <param name="Operation">The operation to add.</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> Do(IOperation<T> Operation, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Adds the specified operation to the workflow
        /// </summary>
        /// <param name="Operation">The operation to add.</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> Do(Func<T, T> Operation, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Appends the workflow specified to this workflow as an operation
        /// </summary>
        /// <param name="Workflow">The workflow to append</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The resulting workflow object</returns>
        IWorkflow<T> Do(IWorkflow<T> Workflow, params IConstraint<T>[] Constraints);

        /// <summary>
        /// Called when an exception of the specified type is thrown in the workflow
        /// </summary>
        /// <typeparam name="ExceptionType">The exception type.</typeparam>
        /// <param name="Operation">The operation to run.</param>
        /// <returns>The resulting workflow object</returns>
        IWorkflow<T> On<ExceptionType>(Action<T> Operation)
            where ExceptionType : Exception;

        /// <summary>
        /// Repeats the last operation the specified number of times.
        /// </summary>
        /// <param name="Times">The number of times to repeat</param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> Repeat(int Times = 1);

        /// <summary>
        /// Retries the last operation the specified number of times if it fails.
        /// </summary>
        /// <param name="Times">The number of times to retry.</param>
        /// <returns>The workflow object</returns>
        IWorkflow<T> Retry(int Times = 1);

        /// <summary>
        /// Starts the workflow with the specified data
        /// </summary>
        /// <param name="Data">The data to pass in to the workflow</param>
        /// <returns>The result from the workflow</returns>
        T Start(T Data);
    }
}