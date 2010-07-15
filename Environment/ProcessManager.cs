/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Diagnostics;
using System.Text;
using System.Threading;
#endregion

namespace Utilities.Environment
{
    /// <summary>
    /// Class that helps with managing processes
    /// </summary>
    public static class ProcessManager
    {
        #region Public Static Functions
        /// <summary>
        /// Kills a process
        /// </summary>
        /// <param name="ProcessName">Name of the process without the ending (ie iexplore instead of iexplore.exe)</param>
        public static void KillProcess(string ProcessName)
        {
            try
            {
                Process[] Processes = Process.GetProcessesByName(ProcessName);
                foreach (Process CurrentProcess in Processes)
                {
                    CurrentProcess.Kill();
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Kills a process after a specified amount of time
        /// </summary>
        /// <param name="ProcessName">Name of the process</param>
        /// <param name="TimeToKill">Amount of time (in ms) until the process is killed.</param>
        public static void KillProcess(string ProcessName, int TimeToKill)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(delegate { KillProcessAsync(ProcessName, TimeToKill); });
            }
            catch { throw; }
        }

        /// <summary>
        /// Gets information about all processes and returns it in an HTML formatted string
        /// </summary>
        /// <returns>An HTML formatted string</returns>
        public static string GetProcessInformation()
        {
            try
            {
                StringBuilder Builder = new StringBuilder();
                Process[] Processes = Process.GetProcesses();
                foreach (Process TempProcess in Processes)
                {
                    Builder.Append("<strong>" + TempProcess.ProcessName + " Information</strong><br />");
                    Builder.Append(Reflection.Reflection.DumpProperties(TempProcess));
                    Builder.Append("<br />");
                }
                return Builder.ToString();
            }
            catch { throw; }
        }
        #endregion

        #region Private Static Functions
        /// <summary>
        /// Kills a process asyncronously
        /// </summary>
        /// <param name="ProcessName">Name of the process to kill</param>
        /// <param name="TimeToKill">Amount of time until the process is killed</param>
        private static void KillProcessAsync(string ProcessName, int TimeToKill)
        {
            try
            {
                if (TimeToKill > 0)
                    Thread.Sleep(TimeToKill);
                Process[] Processes = Process.GetProcessesByName(ProcessName);
                foreach (Process CurrentProcess in Processes)
                {
                    CurrentProcess.Kill();
                }
            }
            catch { throw; }
        }
        #endregion
    }
}
