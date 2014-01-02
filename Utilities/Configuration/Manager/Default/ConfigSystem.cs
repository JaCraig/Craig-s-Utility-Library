/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation Configs (the "Software"), to deal
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
using System.Linq;
using Utilities.Configuration.Manager.Interfaces;
using Utilities.DataTypes;
#endregion

namespace Utilities.Configuration.Manager.Default
{
    /// <summary>
    /// Default config system
    /// </summary>
    public class ConfigSystem : IConfigSystem
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigSystem()
        {
            ConfigFiles = AppDomain.CurrentDomain.GetAssemblies().Objects<IConfig>().ToDictionary(x => x.Name, x => (IConfig)x);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the Config system
        /// </summary>
        public string Name { get { return "Default"; } }

        /// <summary>
        /// Config files
        /// </summary>
        protected Dictionary<string, IConfig> ConfigFiles { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the config object specified
        /// </summary>
        /// <param name="Name">Name of the config object</param>
        /// <returns>The config object</returns>
        public T Config<T>(string Name = "Default")
            where T : IConfig, new()

        {
            if (!ContainsConfigFile<T>(Name))
                throw new ArgumentException("The config object was not found or was not of the type specified.");
            return (T)ConfigFiles[Name];
        }

        /// <summary>
        /// Determines if a specified config file is registered
        /// </summary>
        /// <typeparam name="T">Type of the config object</typeparam>
        /// <param name="Name">Name of the config object</param>
        /// <returns>The config object specified</returns>
        public bool ContainsConfigFile<T>(string Name)
        {
            return ConfigFiles.ContainsKey(Name) && ConfigFiles[Name] is T;
        }

        #endregion
    }
}