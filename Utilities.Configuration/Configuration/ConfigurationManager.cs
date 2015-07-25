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
using System.Diagnostics.Contracts;
using Utilities.Configuration.Manager.Interfaces;

namespace Utilities.Configuration
{
    /// <summary>
    /// Configuration manager
    /// </summary>
    public static class ConfigurationManager
    {
        /// <summary>
        /// Gets or sets the configuration manager.
        /// </summary>
        /// <value>The configuration manager.</value>
        private static Utilities.Configuration.Manager.Manager ConfigManager { get; set; }

        /// <summary>
        /// Gets the config file specified
        /// </summary>
        /// <typeparam name="T">The config type</typeparam>
        /// <param name="System">Configuration system to pull from</param>
        /// <param name="Name">
        /// Name of the config file (Defaults to "Default" which is the web.config/app.config file
        /// if using the default config system)
        /// </param>
        /// <returns>The config file specified</returns>
        public static T Get<T>(ConfigurationSystem System, string Name = "Default")
            where T : IConfig, new()
        {
            Contract.Requires<ArgumentNullException>(System != null, "The config system can not be null.");
            if (ConfigManager == null)
                ConfigManager = IoC.Manager.Bootstrapper.Resolve<Utilities.Configuration.Manager.Manager>();
            return ConfigManager.Get(System).Config<T>(Name);
        }

        /// <summary>
        /// Gets the config file specified
        /// </summary>
        /// <typeparam name="T">The config type</typeparam>
        /// <param name="Name">
        /// Name of the config file (Defaults to "Default" which is the web.config/app.config file
        /// if using the default config system)
        /// </param>
        /// <returns>The config file specified</returns>
        public static T Get<T>(string Name = "Default")
            where T : IConfig, new()
        {
            return Get<T>(ConfigurationSystem.Default, Name);
        }
    }

    /// <summary>
    /// Configuration system enum
    /// </summary>
    public class ConfigurationSystem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Name</param>
        protected ConfigurationSystem(string Name)
        {
            this.Name = Name;
        }

        /// <summary>
        /// Default
        /// </summary>
        /// <value>The default.</value>
        public static ConfigurationSystem Default { get { return new ConfigurationSystem("Default"); } }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        private string Name { get; set; }

        /// <summary>
        /// Converts the object to a string implicitly
        /// </summary>
        /// <param name="Object">Object to convert</param>
        /// <returns>The string version of the configuration system type</returns>
        public static implicit operator string(ConfigurationSystem Object)
        {
            Contract.Requires<ArgumentNullException>(Object != null, "Object");
            return Object.ToString();
        }

        /// <summary>
        /// Returns the name of the serialization type
        /// </summary>
        /// <returns>Name</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}