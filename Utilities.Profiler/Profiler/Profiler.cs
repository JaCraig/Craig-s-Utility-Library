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
using Utilities.DataTypes.Patterns.BaseClasses;
using Utilities.Profiler.Manager.Interfaces;

namespace Utilities.Profiler
{
    /// <summary>
    /// Profiler object
    /// </summary>
    public class Profiler : SafeDisposableBaseClass
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FunctionName">Function name</param>
        public Profiler(string FunctionName)
        {
            this.ProfilerObject = Manager.Manager.Profile(FunctionName);
        }

        /// <summary>
        /// Profiler Object
        /// </summary>
        private IDisposable ProfilerObject { get; set; }

        /// <summary>
        /// Starts profiling
        /// </summary>
        /// <returns>The profiler object</returns>
        public static IDisposable StartProfiling()
        {
            return Manager.Manager.StartProfiling();
        }

        /// <summary>
        /// Stops profiling and returns the result
        /// </summary>
        /// <param name="DiscardResults">Determines if the results should be discarded or not</param>
        /// <returns>Result of the profiling</returns>
        public static IResult StopProfiling(bool DiscardResults)
        {
            return Manager.Manager.StopProfiling(DiscardResults);
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>String equivalent of the info held by the profiler</returns>
        public override string ToString()
        {
            return ProfilerObject != null ? ProfilerObject.ToString() : "";
        }

        /// <summary>
        /// Dispose function
        /// </summary>
        /// <param name="Managed">Is it managed or not</param>
        protected override void Dispose(bool Managed)
        {
            if (ProfilerObject != null)
            {
                ProfilerObject.Dispose();
                ProfilerObject = null;
            }
        }
    }
}