/*
Copyright (c) 2013 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Linq;
using System.Text;
using Batman.Core.Bootstrapper.Interfaces;

using Utilities.DataTypes.ExtensionMethods;
using Batman.Core.Logging.BaseClasses;
using Utilities.IO.Logging.Enums;
using Batman.Core.Logging;
using System.IO;
using Utilities.DataTypes;
using Batman.Core.Tasks.Enums;
using Batman.Core.Tasks.Interfaces;
using System.Reflection;
#endregion

namespace Batman.Core.Tasks
{
    /// <summary>
    /// Task manager
    /// </summary>
    public class TaskManager
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TaskManager()
        {
            Tasks = new ListMapping<RunTime, ITask>();
            foreach (ITask Task in AppDomain.CurrentDomain.GetAssemblies().Objects<ITask>())
            {
                Tasks.Add(Task.TimeToRun, Task);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Tasks to run
        /// </summary>
        public ListMapping<RunTime, ITask> Tasks { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Runs the tasks associated with the run time specified
        /// </summary>
        /// <param name="TimeToRun">Time to run</param>
        public void Run(RunTime TimeToRun)
        {
            if (Tasks.ContainsKey(TimeToRun))
            {
                Tasks[TimeToRun].ForEach(x =>
                {
                    if (BatComputer.Bootstrapper != null)
                        BatComputer.Bootstrapper.Resolve<LogBase>(new NullLogger()).LogMessage("Running {0}", MessageType.Info, x.Name);
                    x.Run();
                });
            }
        }

        public override string ToString()
        {
            return Tasks.ToString(x => x.Value.ToString(y => y.Name, "\n"));
        }

        #endregion
    }
}