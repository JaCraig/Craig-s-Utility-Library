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

#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Ironman.Core.Tasks.Enums;
using Ironman.Core.Tasks.Interfaces;
using Utilities.DataTypes;
using Utilities.IO;
using Utilities.IO.Logging.Enums;

#endregion Usings

namespace Ironman.Core.Tasks
{
    /// <summary>
    /// Task manager
    /// </summary>
    public class TaskManager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TaskManager(IEnumerable<ITask> Tasks)
        {
            Contract.Requires<ArgumentNullException>(Tasks != null, "Tasks");
            this.Tasks = new ListMapping<RunTime, ITask>();
            Tasks.ForEach(x => this.Tasks.Add(x.TimeToRun, x));
        }

        /// <summary>
        /// Tasks to run
        /// </summary>
        public ListMapping<RunTime, ITask> Tasks { get; private set; }

        /// <summary>
        /// Runs the tasks associated with the run time specified
        /// </summary>
        /// <param name="TimeToRun">Time to run</param>
        public void Run(RunTime TimeToRun)
        {
            Contract.Requires<ArgumentNullException>(Tasks != null, "Tasks");
            if (Tasks.ContainsKey(TimeToRun))
            {
                Tasks[TimeToRun].ForEach(x =>
                {
                    Log.Get().LogMessage("Running {0}", MessageType.Info, x.Name);
                    x.Run();
                });
            }
        }

        /// <summary>
        /// Outputs the task manager as a string
        /// </summary>
        /// <returns>string version of the task manager</returns>
        public override string ToString()
        {
            return Tasks.ToString(x => x.Key.ToString() + " Tasks: " + x.Value.ToString(y => y.Name), "\r\n") + "\r\n";
        }
    }
}