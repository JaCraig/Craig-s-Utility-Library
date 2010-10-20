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
using System;

#endregion

namespace Utilities.Profiler
{
    /// <summary>
    /// Object class used to profile a function.
    /// Create at the beginning of a function and it will automatically record the time.
    /// Note that this isn't exact and is based on when the object is destroyed
    /// (if stop isn't called first that is) which leaves it up to garbage collection...
    /// ie. call Stop...
    /// </summary>
    public class Profiler : IDisposable
    {
        #region Private Variables
        private int StartTime = System.Environment.TickCount;
        private int StopTime = System.Environment.TickCount;
        private bool Running = true;
        private string Function = "";
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Profiler()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FunctionName">Takes in the function name/identifier</param>
        public Profiler(string FunctionName)
        {
            Function = FunctionName;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Disposes of the object
        /// </summary>
        public virtual void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// Stops the timer and registers the information
        /// </summary>
        public virtual void Stop()
        {
            if (Running)
            {
                Running = false;
                StopTime = System.Environment.TickCount;
                ProfilerManager.Instance.AddItem(Function, StartTime, StopTime);
            }
        }

        #endregion
    }
}