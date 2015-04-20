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
using System.Diagnostics.Contracts;
using Utilities.DataTypes;
using Utilities.DataTypes.Patterns.BaseClasses;
using Utilities.IO;
using Utilities.IO.FileSystem.Interfaces;
using Utilities.Workflow.Manager.Interfaces;

namespace Utilities.Workflow.Manager
{
    /// <summary>
    /// Workflow manager
    /// </summary>
    public class Manager : SafeDisposableBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Manager"/> class.
        /// </summary>
        /// <param name="FileManager">The file manager.</param>
        /// <param name="SerializationManager">The serialization manager.</param>
        public Manager(IO.FileSystem.Manager FileManager, IO.Serializers.Manager SerializationManager)
        {
            this.FileManager = FileManager;
            this.SerializationManager = SerializationManager;
            IFile Workflows = FileManager.File("~/App_Data/Workflows.obj");
            this.Workflows = Workflows.Exists ?
                (Dictionary<string, IWorkflow>)SerializationManager.Deserialize(Workflows.ReadBinary(), typeof(Dictionary<string, IWorkflow>), SerializationType.Binary.ToString()) :
                new Dictionary<string, IWorkflow>();
            this.Workflows = this.Workflows.Check(new Dictionary<string, IWorkflow>());
            this.LastModified = Workflows.Exists ? Workflows.Modified : new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// Gets the last modified date for the workflows
        /// </summary>
        /// <value>The last modified date for the workflows</value>
        public DateTime LastModified { get; private set; }

        /// <summary>
        /// Gets or sets the file manager.
        /// </summary>
        /// <value>The file manager.</value>
        private IO.FileSystem.Manager FileManager { get; set; }

        /// <summary>
        /// Gets or sets the serialization manager.
        /// </summary>
        /// <value>The serialization manager.</value>
        private IO.Serializers.Manager SerializationManager { get; set; }

        /// <summary>
        /// Gets or sets the workflows.
        /// </summary>
        /// <value>The workflows.</value>
        private Dictionary<string, IWorkflow> Workflows { get; set; }

        /// <summary>
        /// Gets the <see cref="IWorkflow"/> with the specified name.
        /// </summary>
        /// <value>The <see cref="IWorkflow"/>.</value>
        /// <param name="Name">The name.</param>
        /// <returns>The workflow if it exists, null otherwise</returns>
        public IWorkflow this[string Name]
        {
            get
            {
                return Workflows.GetValue(Name);
            }
        }

        /// <summary>
        /// Creates the workflow.
        /// </summary>
        /// <param name="Name">The name.</param>
        /// <returns>The workflow that is created</returns>
        public IWorkflow<T> CreateWorkflow<T>(string Name)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(Name), "Name");
            if (Exists(Name))
                return (IWorkflow<T>)Workflows[Name];
            IWorkflow<T> ReturnValue = new Workflow<T>(Name);
            Workflows.Add(new KeyValuePair<string, IWorkflow>(Name, ReturnValue));
            return ReturnValue;
        }

        /// <summary>
        /// Determines if a workflow exists
        /// </summary>
        /// <param name="Name">The name of a workflow</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool Exists(string Name)
        {
            return Workflows.ContainsKey(Name);
        }

        ///<summary>
        /// Removes the workflow.
        /// </summary>
        /// <param name="Workflow">The workflow.</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool RemoveWorkflow(IWorkflow Workflow)
        {
            return Workflows.Remove(Workflow.Name);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return "Workflows: " + Workflows.ToString(x => x.Key) + "\r\n";
        }

        /// <summary>
        /// Function to override in order to dispose objects
        /// </summary>
        /// <param name="Managed">
        /// If true, managed and unmanaged objects should be disposed. Otherwise unmanaged objects only.
        /// </param>
        protected override void Dispose(bool Managed)
        {
            new System.IO.DirectoryInfo(Environment.CurrentDirectory + "/App_Data/").Create();
            if (FileManager != null && SerializationManager != null)
            {
                IFile File = FileManager.File("~/App_Data/Workflows.obj");
                if (File != null)
                    File.Write(SerializationManager.Serialize<byte[]>(this.Workflows, typeof(Dictionary<string, IWorkflow>), SerializationType.Binary.ToString()));
                FileManager = null;
                SerializationManager = null;
            }
        }
    }
}