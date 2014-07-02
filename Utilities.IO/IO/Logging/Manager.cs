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
using System.Diagnostics.Contracts;
using System.Linq;
using Utilities.DataTypes.Patterns.BaseClasses;
using Utilities.IO.Logging.Default;
using Utilities.IO.Logging.Interfaces;

namespace Utilities.IO.Logging
{
    /// <summary>
    /// Logging manager
    /// </summary>
    public class Manager : SafeDisposableBaseClass
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Loggers">The loggers.</param>
        public Manager(IEnumerable<ILogger> Loggers)
        {
            Contract.Requires<ArgumentNullException>(Loggers != null, "Loggers");
            LoggerUsing = Loggers.FirstOrDefault(x => !x.GetType().Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase));
            if (LoggerUsing == null)
                LoggerUsing = new DefaultLogger();
        }

        /// <summary>
        /// Logger that the system uses
        /// </summary>
        protected ILogger LoggerUsing { get; private set; }

        /// <summary>
        /// Gets a specified log
        /// </summary>
        /// <param name="Name">The name of the log file</param>
        /// <returns>The log file specified</returns>
        public ILog GetLog(string Name = "Default")
        {
            return LoggerUsing.GetLog(Name);
        }

        /// <summary>
        /// Outputs the logging information
        /// </summary>
        /// <returns>The logger information</returns>
        public override string ToString()
        {
            return "Logger: " + LoggerUsing.ToString() + "\r\n";
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">
        /// Determines if all objects should be disposed or just managed objects
        /// </param>
        protected override void Dispose(bool Managed)
        {
            if (LoggerUsing != null)
            {
                LoggerUsing.Dispose();
                LoggerUsing = null;
            }
        }
    }
}