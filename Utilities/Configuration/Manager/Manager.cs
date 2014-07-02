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
using Utilities.Configuration.Manager.Interfaces;
using Utilities.DataTypes;
using Utilities.DataTypes.Patterns.BaseClasses;

namespace Utilities.Configuration.Manager
{
    /// <summary>
    /// Config manager
    /// </summary>
    public class Manager : SafeDisposableBaseClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Manager" /> class.
        /// </summary>
        /// <param name="ConfigSystems">The configuration systems.</param>
        public Manager(IEnumerable<IConfigSystem> ConfigSystems)
        {
            Contract.Requires<ArgumentNullException>(ConfigSystems != null, "ConfigSystems");
            this.ConfigSystems = ConfigSystems.ToDictionary(x => x.Name);
        }

        /// <summary>
        /// Config systems that the library can use
        /// </summary>
        protected IDictionary<string, IConfigSystem> ConfigSystems { get; private set; }

        /// <summary>
        /// Gets the config system by name
        /// </summary>
        /// <param name="Name">Name of the config system</param>
        /// <returns>The config system specified</returns>
        public IConfigSystem this[string Name] { get { return Get(Name); } }

        /// <summary>
        /// Gets the config system specified
        /// </summary>
        /// <param name="Name">Name of the config system</param>
        /// <returns>The config system specified</returns>
        public IConfigSystem Get(string Name)
        {
            Contract.Requires<ArgumentException>(ConfigSystems.ContainsKey(Name), "The config system was not found.");
            return ConfigSystems[Name];
        }

        /// <summary>
        /// Outputs the config system information in string format
        /// </summary>
        /// <returns>The list of config systems that are available</returns>
        public override string ToString()
        {
            return "Configuration systems: " + ConfigSystems.ToString(x => x.Value.Name) + "\r\n";
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">
        /// Determines if all objects should be disposed or just managed objects
        /// </param>
        protected override void Dispose(bool Managed)
        {
            if (ConfigSystems != null)
            {
                foreach (IDisposable ConfigSystem in ConfigSystems.OfType<IDisposable>())
                {
                    ConfigSystem.Dispose();
                }
                ConfigSystems = null;
            }
        }
    }
}