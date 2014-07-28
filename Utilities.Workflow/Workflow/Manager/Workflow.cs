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
using Utilities.DataTypes;
using Utilities.IoC.Interfaces;
using Utilities.Workflow.Manager.Interfaces;

namespace Utilities.Workflow.Manager
{
    /// <summary>
    /// Workflow holder
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    [Serializable]
    public class Workflow<T> : IWorkflow<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Workflow{T}"/> class.
        /// </summary>
        /// <param name="Name">The name.</param>
        public Workflow(string Name)
        {
            this.Name = Name;
            this.Nodes = new List<IWorkflowNode<T>>();
            ExceptionActions = new ListMapping<Type, Action<T>>();
        }

        /// <summary>
        /// Gets the type of the data expected
        /// </summary>
        /// <value>
        /// The type of the data expected
        /// </value>
        public Type DataType { get { return typeof(T); } }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <value>The nodes.</value>
        public ICollection<IWorkflowNode<T>> Nodes { get; private set; }

        /// <summary>
        /// Gets the bootstrapper.
        /// </summary>
        /// <value>The bootstrapper.</value>
        private IBootstrapper Bootstrapper { get; set; }

        /// <summary>
        /// Gets or sets the exception actions.
        /// </summary>
        /// <value>The exception actions.</value>
        private ListMapping<Type, Action<T>> ExceptionActions { get; set; }

        /// <summary>
        /// Causes multiple commands to be executed concurrently
        /// </summary>
        /// <typeparam name="OperationType">The type of the operation.</typeparam>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        public IWorkflow<T> And<OperationType>(params IConstraint<T>[] Constraints)
            where OperationType : IOperation<T>, new()
        {
            return And(new OperationType(), Constraints);
        }

        /// <summary>
        /// Causes multiple commands to be executed concurrently
        /// </summary>
        /// <param name="Operation">The operation to add.</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        public IWorkflow<T> And(IOperation<T> Operation, params IConstraint<T>[] Constraints)
        {
            IWorkflowNode<T> Node = Nodes.LastOrDefault();
            if (Node == null)
                Node = Nodes.AddAndReturn(new WorkflowNode<T>());
            Node.AddOperation(Operation, Constraints);
            return this;
        }

        /// <summary>
        /// Causes multiple commands to be executed concurrently
        /// </summary>
        /// <param name="Operation">The operation to add.</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        public IWorkflow<T> And(Func<T, T> Operation, params IConstraint<T>[] Constraints)
        {
            return And(new GenericOperation<T>(Operation), Constraints);
        }

        /// <summary>
        /// Causes multiple commands to be executed concurrently
        /// </summary>
        /// <param name="Workflow">The workflow to append</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The resulting workflow object</returns>
        public IWorkflow<T> And(IWorkflow<T> Workflow, params IConstraint<T>[] Constraints)
        {
            IWorkflowNode<T> Node = Nodes.LastOrDefault();
            if (Node == null)
                Node = Nodes.AddAndReturn(new WorkflowNode<T>());
            Node.AddOperation(Workflow, Constraints);
            return this;
        }

        /// <summary>
        /// Adds an instance of the specified operation type to the workflow
        /// </summary>
        /// <typeparam name="OperationType">The type of the operation.</typeparam>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        public IWorkflow<T> Do<OperationType>(params IConstraint<T>[] Constraints)
            where OperationType : IOperation<T>, new()
        {
            return Do(new OperationType(), Constraints);
        }

        /// <summary>
        /// Adds the specified operation to the workflow
        /// </summary>
        /// <param name="Operation">The operation to add.</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        public IWorkflow<T> Do(IOperation<T> Operation, params IConstraint<T>[] Constraints)
        {
            IWorkflowNode<T> Node = Nodes.AddAndReturn(new WorkflowNode<T>());
            Node.AddOperation(Operation, Constraints);
            return this;
        }

        /// <summary>
        /// Adds the specified operation to the workflow
        /// </summary>
        /// <param name="Operation">The operation to add.</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The workflow object</returns>
        public IWorkflow<T> Do(Func<T, T> Operation, params IConstraint<T>[] Constraints)
        {
            return Do(new GenericOperation<T>(Operation), Constraints);
        }

        /// <summary>
        /// Appends the workflow specified to this workflow as an operation
        /// </summary>
        /// <param name="Workflow">The workflow to append</param>
        /// <param name="Constraints">
        /// Determines if the operation should be run or if it should be skipped
        /// </param>
        /// <returns>The resulting workflow object</returns>
        public IWorkflow<T> Do(IWorkflow<T> Workflow, params IConstraint<T>[] Constraints)
        {
            IWorkflowNode<T> Node = Nodes.AddAndReturn(new WorkflowNode<T>());
            Node.AddOperation(Workflow, Constraints);
            return this;
        }

        /// <summary>
        /// Called when an exception of the specified type is thrown in the workflow
        /// </summary>
        /// <typeparam name="ExceptionType">The exception type.</typeparam>
        /// <param name="Operation">The operation to run.</param>
        /// <returns>The resulting workflow object</returns>
        public IWorkflow<T> On<ExceptionType>(Action<T> Operation)
            where ExceptionType : Exception
        {
            ExceptionActions.Add(typeof(ExceptionType), Operation);
            return this;
        }

        /// <summary>
        /// Repeats the last operation the specified number of times.
        /// </summary>
        /// <param name="Times">The number of times to repeat</param>
        /// <returns>The workflow object</returns>
        public IWorkflow<T> Repeat(int Times = 1)
        {
            Nodes.LastOrDefault().Chain(x => x.Repeat(Times));
            return this;
        }

        /// <summary>
        /// Retries the last operation the specified number of times if it fails.
        /// </summary>
        /// <param name="Times">The number of times to retry.</param>
        /// <returns>The workflow object</returns>
        public IWorkflow<T> Retry(int Times = 1)
        {
            Nodes.LastOrDefault().Chain(x => x.Retry(Times));
            return this;
        }

        /// <summary>
        /// Starts the workflow with the specified data
        /// </summary>
        /// <param name="Data">The data to pass in to the workflow</param>
        /// <returns>The result from the workflow</returns>
        public T Start(T Data)
        {
            try
            {
                foreach (IWorkflowNode<T> Node in Nodes)
                {
                    Data = Node.Start(Data);
                }
            }
            catch (Exception e)
            {
                Type ExceptionType = e.GetType();
                while (ExceptionType != null && !ExceptionActions.Keys.Contains(ExceptionType))
                {
                    ExceptionType = ExceptionType.BaseType;
                }
                if (ExceptionType != null)
                    ExceptionActions[ExceptionType].ForEach(x => x(Data));
                throw;
            }
            return Data;
        }
    }
}