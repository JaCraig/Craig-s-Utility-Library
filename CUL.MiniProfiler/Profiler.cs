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

using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Utilities.Profiler.Manager.Interfaces;

#endregion Usings

namespace Batman.Core.Profiling.MiniProfiler
{
    /// <summary>
    /// MiniProfiler based profiler
    /// </summary>
    public class Profiler : IProfiler, IResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Profiler()
        {
            this.CalledFrom = CalledFrom;
            Times = new List<long>();
            Children = new Dictionary<string, IResult>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ProfilerUsing">Profiler using</param>
        /// <param name="StepDisposable">Object to use</param>
        public Profiler(StackExchange.Profiling.MiniProfiler ProfilerUsing, IDisposable StepDisposable)
        {
            this.Current = (ProfilerUsing == null) ? StackExchange.Profiling.MiniProfiler.Current : ProfilerUsing;
            this.StepDisposable = StepDisposable;
        }

        /// <summary>
        /// Called from
        /// </summary>
        public string CalledFrom { get; set; }

        /// <summary>
        /// Results
        /// </summary>
        public IDictionary<string, IResult> Children { get; set; }

        /// <summary>
        /// Times
        /// </summary>
        public ICollection<long> Times { get; set; }

        /// <summary>
        /// Current profiler
        /// </summary>
        private StackExchange.Profiling.MiniProfiler Current { get; set; }

        /// <summary>
        /// Step that is disposable
        /// </summary>
        private IDisposable StepDisposable { get; set; }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        public void Dispose()
        {
            if (StepDisposable != null)
            {
                StepDisposable.Dispose();
            }
        }

        /// <summary>
        /// Profiles a new sub item
        /// </summary>
        /// <param name="Name">Name of the item</param>
        /// <returns>new profiler</returns>
        public IDisposable Profile(string Name)
        {
            return new Profiler(Current, Current.Step(Name));
        }

        /// <summary>
        /// Start profiling
        /// </summary>
        /// <returns>This</returns>
        public IDisposable StartProfiling()
        {
            StackExchange.Profiling.MiniProfiler.Start();
            return this;
        }

        /// <summary>
        /// Stops profiling
        /// </summary>
        /// <param name="DiscardResults">Determines if the results should be discarded or not</param>
        /// <returns>Result</returns>
        public IResult StopProfiling(bool DiscardResults)
        {
            StackExchange.Profiling.MiniProfiler.Stop(DiscardResults);
            Times.Add((long)StackExchange.Profiling.MiniProfiler.Current.DurationMilliseconds);
            return this;
        }
    }
}