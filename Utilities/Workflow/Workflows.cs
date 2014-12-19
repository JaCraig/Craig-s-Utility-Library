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

using Utilities.Workflow.Manager.Interfaces;

namespace Utilities.Workflow
{
    /// <summary>
    /// Workflow system
    /// </summary>
    public static class Workflows
    {
        /// <summary>
        /// Creates or loads the workflow specified
        /// </summary>
        /// <typeparam name="T">Object type that the workflow accepts</typeparam>
        /// <param name="Name">The name of the workflow.</param>
        /// <returns>The workflow specified</returns>
        public static IWorkflow<T> CreateOrLoad<T>(string Name)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager.Manager>().CreateWorkflow<T>(Name);
        }

        /// <summary>
        /// Gets the specified workflow based on the specified name.
        /// </summary>
        /// <typeparam name="T">Object type that the workflow accepts</typeparam>
        /// <param name="Name">The name of the workflow.</param>
        /// <returns>The workflow if it exists, null otherwise</returns>
        public static IWorkflow<T> Get<T>(string Name)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager.Manager>()[Name] as IWorkflow<T>;
        }

        /// <summary>
        /// Removes the workflow specified from the system
        /// </summary>
        /// <param name="Workflow">Workflow to remove</param>
        /// <returns>True if the workflow is able to be removed, false otherwise.</returns>
        public static bool RemoveWorkflow(IWorkflow Workflow)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager.Manager>().RemoveWorkflow(Workflow);
        }
    }
}