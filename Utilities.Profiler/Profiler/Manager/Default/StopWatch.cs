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

namespace Utilities.Profiler.Manager.Default
{
    /// <summary>
    /// Acts as a stop watch (records start and stop times)
    /// </summary>
    public class StopWatch
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StopWatch()
        {
            Watch = new System.Diagnostics.Stopwatch();
            Reset();
        }

        /// <summary>
        /// Returns the elapsed time
        /// </summary>
        public virtual long ElapsedTime { get { return Watch.ElapsedMilliseconds; } }

        /// <summary>
        /// Internal stop watch
        /// </summary>
        protected System.Diagnostics.Stopwatch Watch { get; set; }

        /// <summary>
        /// Resets the watch
        /// </summary>
        public virtual void Reset()
        {
            Watch.Reset();
        }

        /// <summary>
        /// Starts the stop watch
        /// </summary>
        public virtual void Start()
        {
            Reset();
            Watch.Start();
        }

        /// <summary>
        /// Stops the stop watch
        /// </summary>
        public virtual void Stop()
        {
            Watch.Stop();
        }
    }
}