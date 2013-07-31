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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilities.IO.Logging.Default;
using Utilities.IO.Logging.Interfaces;
using Utilities.IoC.Default;
using Utilities.IoC.Interfaces;
#endregion

namespace Utilities.IO.Logging
{
    /// <summary>
    /// Logging manager
    /// </summary>
    public class Manager : IDisposable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Manager()
        {
            List<Type> Loggers = new List<Type>();
            foreach (Assembly Assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Loggers.AddRange(Assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(ILogger))
                                                                        && x.IsClass
                                                                        && !x.IsAbstract
                                                                        && !x.ContainsGenericParameters
                                                                        && !x.Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase)));
            }
            if (Loggers.Count == 0)
            {
                Loggers.Add(typeof(DefaultLogger));
            }
            LoggerUsing = (ILogger)Activator.CreateInstance(Loggers[0]);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Logger that the system uses
        /// </summary>
        protected ILogger LoggerUsing { get; private set; }

        #endregion

        #region Functions

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
            return LoggerUsing.ToString();
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">Determines if all objects should be disposed or just managed objects</param>
        protected virtual void Dispose(bool Managed)
        {
            if (LoggerUsing != null)
            {
                LoggerUsing.Dispose();
                LoggerUsing = null;
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Manager()
        {
            Dispose(false);
        }

        #endregion
    }
}