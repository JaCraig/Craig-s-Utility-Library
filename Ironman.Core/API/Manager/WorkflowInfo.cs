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

namespace Ironman.Core.API.Manager
{
    /// <summary>
    /// Workflow info
    /// </summary>
    public class WorkflowInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowInfo" /> class.
        /// </summary>
        /// <param name="Mapping">The mapping.</param>
        /// <param name="Function">The function.</param>
        /// <param name="Version">The version.</param>
        /// <param name="ReturnValues">The return values.</param>
        public WorkflowInfo(string Mapping, WorkflowType Function, int Version, dynamic ReturnValues)
        {
            this.Mapping = Mapping;
            this.Function = Function;
            this.Version = Version;
            this.ReturnValues = ReturnValues;
            this.Continue = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WorkflowInfo"/> should continue.
        /// </summary>
        /// <value>
        ///   <c>true</c> if continue; otherwise, <c>false</c>.
        /// </value>
        public bool Continue { get; set; }

        /// <summary>
        /// Gets or sets the function.
        /// </summary>
        /// <value>
        /// The function.
        /// </value>
        public WorkflowType Function { get; set; }

        /// <summary>
        /// Gets or sets the mapping name.
        /// </summary>
        /// <value>
        /// The mapping.
        /// </value>
        public string Mapping { get; set; }

        /// <summary>
        /// Gets or sets the return values.
        /// </summary>
        /// <value>
        /// The return values.
        /// </value>
        public dynamic ReturnValues { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }
    }
}