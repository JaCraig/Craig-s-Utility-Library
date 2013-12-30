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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities.DataTypes;
using Utilities.Profiler.Manager.Interfaces;
#endregion

namespace Utilities.Profiler.Manager.Default
{
    /// <summary>
    /// Object class used to profile a function.
    /// Create at the beginning of a function in a using statement and it will automatically record the time.
    /// Note that this isn't exact and is based on when the object is destroyed
    /// </summary>
    public class Profiler : IProfiler,IResult
    {
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
                this.Times = new List<long>();
                this.StopWatch = new StopWatch();
                this.Level = Parent == null ? 0 : Parent.Level + 1;
                this.CalledFrom = new StackTrace().GetMethods(this.GetType().Assembly).ToString<MethodBase>(x => x.DeclaringType.Name + " > " + x.Name, "<br />");
                Running = false;
                Current = this;
                Child = this;
            }
            else
            {
                Current = Child;
            }
            Child.Start();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Total time that the profiler has taken (in milliseconds)
        /// </summary>
        public ICollection<long> Times { get; private set; }

        /// <summary>
        /// Children profiler items
        /// </summary>
        public IDictionary<string, Profiler> InternalChildren { get; private set; }

        /// <summary>
        /// Children result items
        /// </summary>
        public IDictionary<string, IResult> Children { get { return InternalChildren.ToDictionary(x => x.Key, x => (IResult)x.Value); } }

        /// <summary>
        /// Parent profiler item
        /// </summary>
        protected Profiler Parent { get; set; }

        /// <summary>
        /// Function name
        /// </summary>
        public string Function { get; protected set; }

        /// <summary>
        /// Determines if it is running
        /// </summary>
        protected bool Running { get; set; }

        /// <summary>
        /// Level of the profiler
        /// </summary>
        protected int Level { get; set; }

        /// <summary>
        /// Where the profiler was started at
        /// </summary>
        public string CalledFrom { get; set; }

        /// <summary>
        /// Stop watch
        /// </summary>
        protected StopWatch StopWatch { get; set; }

        /// <summary>
        /// Contains the root profiler
        /// </summary>
        public static Profiler Root
        {
            get
            {
                Profiler ReturnValue = "Root_Profiler".GetFromCache<Profiler>(Cache: "Item");
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
        /// Contains the current profiler
        /// </summary>
        public static Profiler Current
        {
            get
            {
                Profiler ReturnValue = "Current_Profiler".GetFromCache<Profiler>(Cache: "Item");
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

        #endregion

        #region Functions

        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the objects
        /// </summary>
        /// <param name="Disposing">True to dispose of all resources, false only disposes of native resources</param>
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

        /// <summary>
        /// Stops the timer and registers the information
        /// </summary>
        public void Stop()
        {
            if (Running)
            {
                Running = false;
                StopWatch.Stop();
                Times.Add(StopWatch.ElapsedTime);
                Current = Parent;
            }
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            if (Running)
            {
                Running = false;
                StopWatch.Stop();
                Times.Add(StopWatch.ElapsedTime);
            }
            Running = true;
            StopWatch.Start();
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
        /// Stops profiling
        /// </summary>
        /// <returns>The root profiler</returns>
        public IResult StopProfiling()
        {
            Root.Stop();
            return Root;
        }

        /// <summary>
        /// Outputs the information to a table
        /// </summary>
        /// <returns>an html string containing the information</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Level.Times(x => { Builder.Append("\t"); });
            Builder.AppendLineFormat("{0} ({1} ms)", Function, Times.Sum());
            foreach (string Key in Children.Keys)
            {
                Builder.AppendLineFormat(Children[Key].ToString());
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            Profiler Temp = obj as Profiler;
            if (Temp == null)
                return false;
            return Temp == this;
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
        /// Gets the hash code for the profiler
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return Function.GetHashCode();
        }

        #endregion
    }
}