/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using Utilities.IO.Logging.BaseClasses;
using Utilities.IO.Logging.Interfaces;
#endregion

namespace Utilities.IO.Logging
{
    /// <summary>
    /// Logging manager
    /// </summary>
    public static class LoggingManager
    {
        #region Fields

        /// <summary>
        /// Logs
        /// </summary>
        private static Dictionary<string, ILog> Logs = new Dictionary<string, ILog>();

        #endregion

        #region Functions

        /// <summary>
        /// Gets a specified log
        /// </summary>
        /// <param name="Name">The name of the log file</param>
        /// <typeparam name="LogType">Log type that the log object should be</typeparam>
        /// <returns>The log file specified</returns>
        public static LogType GetLog<LogType>(string Name = "Default") where LogType : ILog
        {
            if (!Logs.ContainsKey(Name))
                throw new ArgumentException(Name + " was not found");
            if (!(Logs[Name] is LogType))
                throw new ArgumentException(Name + " is not the type specified");
            return (LogType)Logs[Name];
        }

        /// <summary>
        /// Gets a specified log
        /// </summary>
        /// <param name="Name">The name of the log file</param>
        /// <returns>The log file specified</returns>
        public static ILog GetLog(string Name = "Default")
        {
            if (!Logs.ContainsKey(Name))
                throw new ArgumentException(Name + " was not found");
            return Logs[Name];
        }

        /// <summary>
        /// Adds a log object or replaces one already in use
        /// </summary>
        /// <param name="Name">The name of the log file</param>
        /// <typeparam name="LogType">Log type to add</typeparam>
        public static void AddLog<LogType>(string Name = "Default") where LogType : LogBase<LogType>, new()
        {
            AddLog(new LogType(), Name);
        }

        /// <summary>
        /// Adds a log object or replaces one already in use
        /// </summary>
        /// <param name="Log">The log object to add</param>
        /// <param name="Name">The name of the log file</param>
        public static void AddLog(ILog Log, string Name = "Default")
        {
            if (Log == null)
                throw new ArgumentNullException("Log");
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name");
            if (Logs.ContainsKey(Name))
                Logs[Name] = Log;
            else
                Logs.Add(Name, Log);
        }

        /// <summary>
        /// Destroys the logging manager
        /// </summary>
        public static void Destroy()
        {
            foreach (string Key in Logs.Keys)
            {
                Logs[Key].Dispose();
            }
        }

        #endregion
    }
}