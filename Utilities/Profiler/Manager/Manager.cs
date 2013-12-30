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
using System.Linq;
using Utilities.DataTypes;
using Utilities.DataTypes.Patterns.BaseClasses;
using Utilities.Profiler.Manager.Interfaces;

#endregion

namespace Utilities.Profiler.Manager
{
    /// <summary>
    /// Profiler manager
    /// </summary>
    public class Manager : SafeDisposableBaseClass
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Manager()
        {
            Type TempType = AppDomain.CurrentDomain.GetAssemblies()
                                                   .Types<IProfiler>()
                                                   .Where(x => !x.Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase))
                                                   .FirstOrDefault();
            if (TempType == null)
            {
                TempType = AppDomain.CurrentDomain.GetAssemblies()
                                                  .Types<IProfiler>()
                                                  .Where(x => x.Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase))
                                                  .FirstOrDefault();
            }
            Profiler = TempType.Create<IProfiler>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Root profiler object
        /// </summary>
        protected IProfiler Profiler { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Starts the profiler and uses the name specified
        /// </summary>
        /// <param name="Name">Name of the entry</param>
        /// <returns>An IDisposable object that will stop the profiler when disposed of</returns>
        public static IDisposable Profile(string Name)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Profiler.Profile(Name);
        }

        /// <summary>
        /// Outputs the profiler information as a string
        /// </summary>
        /// <returns>The profiler information as a string</returns>
        public override string ToString()
        {
            return Profiler.ToString();
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">Determines if all objects should be disposed or just managed objects</param>
        protected override void Dispose(bool Managed)
        {
            if (Profiler != null)
            {
                Profiler.Dispose();
                Profiler = null;
            }
        }

        #endregion
    }
}