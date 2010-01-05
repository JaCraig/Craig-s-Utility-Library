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
using System.Collections.Generic;
using System.Text;
#endregion

namespace Utilities.Profiler
{
    /// <summary>
    /// Actual location that profiler information is stored
    /// </summary>
    public class ProfilerManager
    {
        #region Private Variables
        private static ProfilerManager _Manager=null;
        private List<ProfilerInfo>Profilers=null;
        #endregion

        #region Public Static Members
        /// <summary>
        /// Uses singleton method to create the item
        /// </summary>
        public static ProfilerManager Manager
        {
            get
            {
                if (_Manager == null)
                {
                    _Manager = new ProfilerManager();
                }
                return _Manager;
            }
        }
        #endregion

        #region Constuctors
        private ProfilerManager()
        {
            Profilers=new List<ProfilerInfo>();
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Adds an item to the manager (used by Profiler class)
        /// </summary>
        /// <param name="FunctionName">Function name/identifier</param>
        /// <param name="StartTime">Start time (in ms)</param>
        /// <param name="StopTime">Stop time (in ms)</param>
        public void AddItem(string FunctionName, int StartTime, int StopTime)
        {
            for(int x=0;x<Profilers.Count;++x)
            {
                if (Profilers[x].FunctionName.Equals(FunctionName))
                {
                    int Time=(StopTime - StartTime);
                    Profilers[x].TotalTime += Time;
                    if (Profilers[x].MaxTime < Time)
                        Profilers[x].MaxTime = Time;
                    else if (Profilers[x].MinTime > Time)
                        Profilers[x].MinTime = Time;
                    ++Profilers[x].TimesCalled;
                    return;
                }
            }
            ProfilerInfo Info = new ProfilerInfo();
            Info.FunctionName = FunctionName;
            Info.TotalTime = Info.MaxTime = Info.MinTime = StopTime - StartTime;
            Info.TimesCalled = 1;
            Profilers.Add(Info);
        }

        /// <summary>
        /// Outputs the information to a table
        /// </summary>
        /// <returns>an html string containing the information</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<table><tr><th>Function Name</th><th>Total Time</th><th>Max Time</th><th>Min Time</th><th>Average Time</th><th>Times Called</th></tr>");
            foreach (ProfilerInfo Info in Profilers)
            {
                Builder.Append("<tr><td>" + Info.FunctionName + "</td><td>" + Info.TotalTime.ToString() + "</td><td>" + Info.MaxTime + "</td><td>" + Info.MinTime + "</td><td>" + ((double)Info.TotalTime / (double)Info.TimesCalled) + "</td><td>" + Info.TimesCalled + "</td></tr>");
            }
            Builder.Append("</table>");
            return Builder.ToString();
        }
        #endregion

        #region Private Classes
        /// <summary>
        /// Holds the profiler information
        /// </summary>
        private class ProfilerInfo
        {
            public string FunctionName;
            public int TotalTime;
            public int TimesCalled;
            public int MaxTime;
            public int MinTime;
        }
        #endregion
    }
}
