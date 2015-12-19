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

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="process">Process to get information about</param>
        /// <param name="htmlFormat">Should this be HTML formatted?</param>
        /// <returns>An HTML formatted string</returns>
        public static string GetInformation(this Process process, bool htmlFormat = true)
        {
            if (process == null)
                return "";
            var Builder = new StringBuilder();
            return Builder.Append(htmlFormat ? "<strong>" : "")
                   .Append(process.ProcessName)
                   .Append(" Information")
                   .Append(htmlFormat ? "</strong><br />" : "\n")
                   .Append(process.ToString(htmlFormat))
                   .Append(htmlFormat ? "<br />" : "\n")
                   .ToString();
        }

        /// <summary>
        /// Gets information about all processes and returns it in an HTML formatted string
        /// </summary>
        /// <param name="processes">Processes to get information about</param>
        /// <param name="htmlFormat">Should this be HTML formatted?</param>
        /// <returns>An HTML formatted string</returns>
        public static string GetInformation(this IEnumerable<Process> processes, bool htmlFormat = true)
        {
            if (processes == null)
                return "";
            var Builder = new StringBuilder();
            processes.ForEach(x => Builder.Append(x.GetInformation(htmlFormat)));
            return Builder.ToString();
        }

        /// <summary>
        /// Kills a process
        /// </summary>
        /// <param name="process">Process that should be killed</param>
        /// <param name="timeToKill">Amount of time (in ms) until the process is killed.</param>
        /// <returns>The input process</returns>
        public static async Task<Process> KillProcessAsync(this Process process, int timeToKill = 0)
        {
            if (process == null)
                return null;
            await Task.Run(() => KillProcessAsyncHelper(process, timeToKill));
            return process;
        }

        /// <summary>
        /// Kills a list of processes
        /// </summary>
        /// <param name="processes">Processes that should be killed</param>
        /// <param name="timeToKill">Amount of time (in ms) until the processes are killed.</param>
        /// <returns>The list of processes</returns>
        public static async Task<IEnumerable<Process>> KillProcessAsync(this IEnumerable<Process> processes, int timeToKill = 0)
        {
            if (processes == null || processes.Count() == 0)
                return new List<Process>();
            await Task.Run(() => processes.ForEach(x => KillProcessAsyncHelper(x, timeToKill)));
            return processes;
        }

        /// <summary>
        /// Kills a process asyncronously
        /// </summary>
        /// <param name="process">Process to kill</param>
        /// <param name="timeToKill">Amount of time until the process is killed</param>
        private static void KillProcessAsyncHelper(Process process, int timeToKill)
        {
            if (process == null)
                return;
            if (timeToKill > 0)
                Thread.Sleep(timeToKill);
            process.Kill();
        }
    }
}