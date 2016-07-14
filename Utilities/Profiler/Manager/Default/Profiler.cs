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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities.DataTypes;
using Utilities.Profiler.Manager.Interfaces;

namespace Utilities.Profiler.Manager.Default
{
    /// <summary>
    /// Object class used to profile a function. Create at the beginning of a function in a using
    /// statement and it will automatically record the time. Note that this isn't exact and is based
    /// on when the object is destroyed
    /// </summary>
    public class Profiler : IProfiler, IResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Profiler()
        {
            Entries = new List<IResultEntry>();
            Function = "";
            InternalChildren = new Dictionary<string, Profiler>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FunctionName">Function/identifier</param>
        public Profiler(string FunctionName)
        {
            this.Parent = Current;
            Profiler Child = Parent != null && Parent.InternalChildren.ContainsKey(FunctionName) ? Parent.InternalChildren[FunctionName] : null;
            if (Child == null)
            {
                if (Parent != null)
                    Parent.InternalChildren.Add(FunctionName, this);
                this.Function = FunctionName;
                this.InternalChildren = new Dictionary<string, Profiler>();
                this.Entries = new List<IResultEntry>();
                this.StopWatch = new StopWatch();
                this.Level = Parent == null ? 0 : Parent.Level + 1;
                this.CalledFrom = new StackTrace().GetMethods(this.GetType().Assembly).ToString<MethodBase>(x => x.DeclaringType.Name + " > " + x.Name, "<br />");
                Running = false;
                Current = this;
                Child = this;
                if (CPUCounter == null)
                    CPUCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
                if (CounterStopWatch == null)
                {
                    CounterStopWatch = new StopWatch();
                    CounterStopWatch.Start();
                    LastCounterTime = CounterStopWatch.ElapsedTime;
                }
            }
            else
            {
                Current = Child;
            }
            Start();
        }

        /// <summary>
        /// The _ cpu value
        /// </summary>
        private static float _CPUValue = 0;

        /// <summary>
        /// The _ memory value
        /// </summary>
        private static float _MemValue = 0;

        /// <summary>
        /// Contains the current profiler
        /// </summary>
        public static Profiler Current
        {
            get
            {
                var ReturnValue = "Current_Profiler".GetFromCache<Profiler>(Cache: "Item");
                if (ReturnValue == null)
                {
                    ReturnValue = "Root_Profiler".GetFromCache<Profiler>(Cache: "Item");
                    Current = ReturnValue;
                }
                return ReturnValue;
            }
            protected set
            {
                value.Cache("Current_Profiler", "Item");
            }
        }

        /// <summary>
        /// Contains the root profiler
        /// </summary>
        public static Profiler Root
        {
            get
            {
                var ReturnValue = "Root_Profiler".GetFromCache<Profiler>(Cache: "Item");
                if (ReturnValue == null)
                {
                    ReturnValue = new Profiler("Start");
                    Root = ReturnValue;
                }
                return ReturnValue;
            }
            protected set
            {
                value.Cache("Root_Profiler", "Item");
            }
        }

        /// <summary>
        /// Where the profiler was started at
        /// </summary>
        public string CalledFrom { get; set; }

        /// <summary>
        /// Children result items
        /// </summary>
        public IDictionary<string, IResult> Children { get { return InternalChildren.ToDictionary(x => x.Key, x => (IResult)x.Value); } }

        /// <summary>
        /// Gets the entries.
        /// </summary>
        /// <value>The entries.</value>
        public ICollection<IResultEntry> Entries { get; private set; }

        /// <summary>
        /// Function name
        /// </summary>
        public string Function { get; protected set; }

        /// <summary>
        /// Children profiler items
        /// </summary>
        public IDictionary<string, Profiler> InternalChildren { get; private set; }

        /// <summary>
        /// Level of the profiler
        /// </summary>
        protected int Level { get; set; }

        /// <summary>
        /// Parent profiler item
        /// </summary>
        protected Profiler Parent { get; set; }

        /// <summary>
        /// Determines if it is running
        /// </summary>
        protected bool Running { get; set; }

        /// <summary>
        /// Stop watch
        /// </summary>
        protected StopWatch StopWatch { get; set; }

        /// <summary>
        /// Gets or sets the counter stop watch.
        /// </summary>
        /// <value>The counter stop watch.</value>
        private static StopWatch CounterStopWatch { get; set; }

        /// <summary>
        /// Gets or sets the cpu counter.
        /// </summary>
        /// <value>The cpu counter.</value>
        private static PerformanceCounter CPUCounter { get; set; }

        /// <summary>
        /// Gets the cpu value.
        /// </summary>
        /// <value>The cpu value.</value>
        private static float CPUValue
        {
            get
            {
                if (CounterStopWatch.ElapsedTime >= LastCounterTime + 500)
                {
                    LastCounterTime = CounterStopWatch.ElapsedTime;
                    _CPUValue = CPUCounter.NextValue();
                    _MemValue = GC.GetTotalMemory(false) / 1048576;
                }
                return _CPUValue;
            }
        }

        /// <summary>
        /// Gets or sets the last counter time.
        /// </summary>
        /// <value>The last counter time.</value>
        private static double LastCounterTime { get; set; }

        /// <summary>
        /// Gets the memory value.
        /// </summary>
        /// <value>The memory value.</value>
        private static float MemValue
        {
            get
            {
                if (CounterStopWatch.ElapsedTime >= LastCounterTime + 500)
                {
                    LastCounterTime = CounterStopWatch.ElapsedTime;
                    _CPUValue = CPUCounter.NextValue();
                    _MemValue = GC.GetTotalMemory(false) / 1048576;
                }
                return _MemValue;
            }
        }

        /// <summary>
        /// Compares the profilers and determines if they are not equal
        /// </summary>
        /// <param name="First">First</param>
        /// <param name="Second">Second</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public static bool operator !=(Profiler First, Profiler Second)
        {
            return !(First == Second);
        }

        /// <summary>
        /// Compares the profilers and determines if they are equal
        /// </summary>
        /// <param name="First">First</param>
        /// <param name="Second">Second</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public static bool operator ==(Profiler First, Profiler Second)
        {
            if ((object)First == null && (object)Second == null)
                return true;
            if ((object)First == null)
                return false;
            if ((object)Second == null)
                return false;
            return First.Function == Second.Function;
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public static void Start()
        {
            if (Current == null)
                return;
            if (Current.Running)
            {
                Current.Running = false;
                Current.StopWatch.Stop();
                Current.Entries.Add(new Entry(Current.StopWatch.ElapsedTime, MemValue, CPUValue));
            }
            Current.Running = true;
            Current.StopWatch.Start();
        }

        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var Temp = obj as Profiler;
            if (Temp == null)
                return false;
            return Temp == this;
        }

        /// <summary>
        /// Gets the hash code for the profiler
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return Function.GetHashCode();
        }

        /// <summary>
        /// Creates a profiler object and starts profiling
        /// </summary>
        /// <param name="Name">Function name</param>
        /// <returns>An IDisposable that is used to stop profiling</returns>
        public IDisposable Profile(string Name)
        {
            return new Profiler(Name);
        }

        /// <summary>
        /// Starts profiling
        /// </summary>
        /// <returns>The root profiler</returns>
        public IDisposable StartProfiling()
        {
            return Root;
        }

        /// <summary>
        /// Stops the timer and registers the information
        /// </summary>
        public void Stop()
        {
            if (Current == null)
            {
                if (CPUCounter != null)
                {
                    CPUCounter.Dispose();
                    CPUCounter = null;
                }
                return;
            }
            if (Current.Running)
            {
                Current.Running = false;
                Current.StopWatch.Stop();
                Current.Entries.Add(new Entry(Current.StopWatch.ElapsedTime, MemValue, CPUValue));
                Current = Parent;
            }
        }

        /// <summary>
        /// Stops profiling
        /// </summary>
        /// <param name="DiscardResults">Discard results</param>
        /// <returns>The root profiler</returns>
        public IResult StopProfiling(bool DiscardResults)
        {
            if (Root == null)
                return null;
            Root.Stop();
            if (DiscardResults)
                Root.Entries.Clear();
            return Root;
        }

        /// <summary>
        /// Outputs the information to a table
        /// </summary>
        /// <returns>an html string containing the information</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            Level.Times(x => { Builder.Append("\t"); });
            Builder.AppendLineFormat("{0} ({1} ms)", Function, Entries.Sum(x => x.Time));
            foreach (string Key in Children.Keys)
            {
                Builder.AppendLine(Children[Key].ToString());
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Disposes of the objects
        /// </summary>
        /// <param name="Disposing">
        /// True to dispose of all resources, false only disposes of native resources
        /// </param>
        protected virtual void Dispose(bool Disposing)
        {
            if (Disposing)
                Stop();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Profiler()
        {
            Dispose(false);
        }
    }
}