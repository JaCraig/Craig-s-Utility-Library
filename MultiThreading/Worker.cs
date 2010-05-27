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
using System.Threading;
using Utilities.Events;
using Utilities.Events.EventArgs;
#endregion

namespace Utilities.MultiThreading
{
    /// <summary>
    /// Worker class used in multi threading
    /// </summary>
    /// <typeparam name="ResultType">Result type</typeparam>
    /// <typeparam name="InputParams">The input parameter type</typeparam>
    public abstract class Worker<ResultType,InputParams>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Params">Parameters used in the function</param>
        protected Worker(InputParams Params)
        {
            try
            {
                this.Params = Params;
                this.WorkerThread = new Thread(DoWork);
                this.WorkerThread.IsBackground = true;
            }
            catch { throw; }
        }

        #endregion

        #region Protected Abstract Functions

        /// <summary>
        /// Function that actually does the work
        /// </summary>
        /// <param name="Params">Parameter used by the function</param>
        /// <returns>The result of the function</returns>
        protected abstract ResultType Work(InputParams Params);

        #endregion

        #region Protected Functions

        /// <summary>
        /// Causes the worker thread to sleep for a given number of milliseconds
        /// </summary>
        /// <param name="TimeInMs">Time to sleep in milliseconds</param>
        protected void Sleep(int TimeInMs)
        {
            try
            {
                Thread.Sleep(TimeInMs);
            }
            catch (Exception e)
            {
                OnErrorEventArgs Error = new OnErrorEventArgs();
                Error.Content = e;
                EventHelper.Raise<OnErrorEventArgs>(Exception, this, Error);
                throw;
            }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Starts the thread
        /// </summary>
        public void Start()
        {
            try
            {
                Stopping = false;
                if (this.WorkerThread == null)
                {
                    this.WorkerThread = new Thread(DoWork);
                    this.WorkerThread.IsBackground = true;
                }
                this.WorkerThread.Start();
            }
            catch (Exception e)
            {
                OnErrorEventArgs Error = new OnErrorEventArgs();
                Error.Content = e;
                EventHelper.Raise<OnErrorEventArgs>(Exception, this, Error);
                throw;
            }
        }

        /// <summary>
        /// Stops the thread and waits for it to finish
        /// </summary>
        public void Stop()
        {
            try
            {
                Stopping = true;
                if (WorkerThread != null && WorkerThread.IsAlive)
                {
                    this.WorkerThread.Join();
                    this.WorkerThread = null;
                }
                OnEndEventArgs EndEvents = new OnEndEventArgs();
                EndEvents.Content = Result;
                EventHelper.Raise<OnEndEventArgs>(Finished, this, EndEvents);
            }
            catch (Exception e)
            {
                OnErrorEventArgs Error = new OnErrorEventArgs();
                Error.Content = e;
                EventHelper.Raise<OnErrorEventArgs>(Exception, this, Error);
                throw;
            }
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Wrapper for the function that actually does the work.
        /// Calls the start and finished events as well as stores
        /// the result for use within the class.
        /// </summary>
        private void DoWork()
        {
            try
            {
                EventHelper.Raise<OnStartEventArgs>(Started, this, new OnStartEventArgs());

                Result = Work(this.Params);

                OnEndEventArgs EndEvents = new OnEndEventArgs();
                EndEvents.Content = Result;
                EventHelper.Raise<OnEndEventArgs>(Finished, this, EndEvents);
            }
            catch (Exception e)
            {
                OnErrorEventArgs Error = new OnErrorEventArgs();
                Error.Content = e;
                EventHelper.Raise<OnErrorEventArgs>(Exception, this, Error);
                throw;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Called when the thread is finished
        /// </summary>
        public EventHandler<OnEndEventArgs> Finished { get; set; }

        /// <summary>
        /// Called when the thread is started
        /// </summary>
        public EventHandler<OnStartEventArgs> Started { get; set; }

        /// <summary>
        /// Can be used by the worker function to indicate progress
        /// </summary>
        public EventHandler<ChangedEventArgs> Updated { get; set; }

        /// <summary>
        /// Can be used by the worker function to indicate an exception has occurred
        /// </summary>
        public EventHandler<OnErrorEventArgs> Exception { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether or not the thread is stopped/started
        /// </summary>
        public bool Stopped 
        { 
            get 
            { 
                if (WorkerThread != null && WorkerThread.IsAlive)
                    return false;
                return true; 
            }
        }

        /// <summary>
        /// The result (can be used by the class that inherits from this base class
        /// </summary>
        protected ResultType Result { get; set; }

        /// <summary>
        /// Parameters used in the function
        /// </summary>
        private InputParams Params = default(InputParams);

        /// <summary>
        /// The thread used
        /// </summary>
        private Thread WorkerThread=null;

        /// <summary>
        /// Can be used to determine if the thread needs to stop
        /// </summary>
        protected volatile bool Stopping = false;

        #endregion
    }
}