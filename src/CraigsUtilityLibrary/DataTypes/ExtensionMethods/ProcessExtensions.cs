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
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;
using System.Threading;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Process extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ProcessExtensions
    {
        /// <summary>
        /// Gets information about all processes and returns it in an HTML formatted string
        /// </summary>
        /// <param name="Process">Process to get information about</param>
        /// <param name="HTMLFormat">Should this be HTML formatted?</param>
        /// <returns>An HTML formatted string</returns>
        public static string GetInformation(this Process Process, bool HTMLFormat = true)
        {
            Contract.Requires<ArgumentNullException>(Process != null, "Process");
            var Builder = new StringBuilder();
            return Builder.Append(HTMLFormat ? "<strong>" : "")
                   .Append(Process.ProcessName)
                   .Append(" Information")
                   .Append(HTMLFormat ? "</strong><br />" : "\n")
                   .Append(Process.ToString(HTMLFormat))
                   .Append(HTMLFormat ? "<br />" : "\n")
                   .ToString();
        }

        /// <summary>
        /// Gets information about all processes and returns it in an HTML formatted string
        /// </summary>
        /// <param name="Processes">Processes to get information about</param>
        /// <param name="HTMLFormat">Should this be HTML formatted?</param>
        /// <returns>An HTML formatted string</returns>
        public static string GetInformation(this IEnumerable<Process> Processes, bool HTMLFormat = true)
        {
            if (Processes == null)
                return "";
            var Builder = new StringBuilder();
            Processes.ForEach(x => Builder.Append(x.GetInformation(HTMLFormat)));
            return Builder.ToString();
        }

        /// <summary>
        /// Kills a process
        /// </summary>
        /// <param name="Process">Process that should be killed</param>
        /// <param name="TimeToKill">Amount of time (in ms) until the process is killed.</param>
        public static void KillProcessAsync(this Process Process, int TimeToKill = 0)
        {
            Contract.Requires<ArgumentNullException>(Process != null, "Process");
            ThreadPool.QueueUserWorkItem(delegate { KillProcessAsyncHelper(Process, TimeToKill); });
        }

        /// <summary>
        /// Kills a list of processes
        /// </summary>
        /// <param name="Processes">Processes that should be killed</param>
        /// <param name="TimeToKill">Amount of time (in ms) until the processes are killed.</param>
        public static void KillProcessAsync(this IEnumerable<Process> Processes, int TimeToKill = 0)
        {
            Contract.Requires<ArgumentNullException>(Processes != null, "Processes");
            Processes.ForEach(x => ThreadPool.QueueUserWorkItem(delegate { KillProcessAsyncHelper(x, TimeToKill); }));
        }

        /// <summary>
        /// Kills a process asyncronously
        /// </summary>
        /// <param name="Process">Process to kill</param>
        /// <param name="TimeToKill">Amount of time until the process is killed</param>
        private static void KillProcessAsyncHelper(Process Process, int TimeToKill)
        {
            Contract.Requires<ArgumentNullException>(Process != null, "Process");
            if (TimeToKill > 0)
                Thread.Sleep(TimeToKill);
            Process.Kill();
        }
    }
}