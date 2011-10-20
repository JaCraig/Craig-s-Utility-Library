/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Reflection;
using System.Text;
using Utilities.Configuration.Interfaces;
using Utilities.Reflection.ExtensionMethods;
#endregion

namespace Utilities.Configuration
{
    /// <summary>
    /// Config manager
    /// </summary>
    public class ConfigurationManager
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ConfigurationManager()
        {
            
        }

        #endregion

        #region Public Functions

        #region RegisterConfigFile

        /// <summary>
        /// Registers a config file
        /// </summary>
        /// <typeparam name="ConfigType">The config object type to register</typeparam>
        public static void RegisterConfigFile<ConfigType>() where ConfigType : Config<ConfigType>, new()
        {
            RegisterConfigFile(new ConfigType());
        }

        /// <summary>
        /// Registers a config file
        /// </summary>
        /// <param name="ConfigObject">Config object to register</param>
        public static void RegisterConfigFile(IConfig ConfigObject)
        {
            if (ConfigObject == null) throw new ArgumentNullException("ConfigObject");
            if (ConfigFiles.ContainsKey(ConfigObject.Name)) return;
            ConfigObject.Load();
            ConfigFiles.Add(ConfigObject.Name, ConfigObject);
        }

        /// <summary>
        /// Registers a set of config file
        /// </summary>
        /// <param name="ConfigObjects">Config objects to register</param>
        public static void RegisterConfigFile(IEnumerable<IConfig> ConfigObjects)
        {
            if (ConfigObjects == null) throw new ArgumentNullException("ConfigObjects");
            foreach (IConfig ConfigObject in ConfigObjects)
                RegisterConfigFile(ConfigObject);
        }

        /// <summary>
        /// Registers all config files in an assembly
        /// </summary>
        /// <param name="AssemblyContainingConfig">Assembly to search</param>
        public static void RegisterConfigFile(Assembly AssemblyContainingConfig)
        {
            if (AssemblyContainingConfig == null) throw new ArgumentNullException("AssemblyContainingConfig");
            RegisterConfigFile(AssemblyContainingConfig.GetObjects<IConfig>());
        }

        #endregion

        #region GetConfigFile

        /// <summary>
        /// Gets a specified config file
        /// </summary>
        /// <typeparam name="T">Type of the config object</typeparam>
        /// <param name="Name">Name of the config object</param>
        /// <returns>The config object specified</returns>
        public static T GetConfigFile<T>(string Name)
        {
            if (!ConfigFiles.ContainsKey(Name))
                throw new ArgumentException("The config object " + Name + " was not found.");
            if (!(ConfigFiles[Name] is T))
                throw new ArgumentException("The config object " + Name + " is not the specified type.");
            return (T)ConfigFiles[Name];
        }

        #endregion

        #region ContainsConfigFile

        /// <summary>
        /// Determines if a specified config file is registered
        /// </summary>
        /// <typeparam name="T">Type of the config object</typeparam>
        /// <param name="Name">Name of the config object</param>
        /// <returns>The config object specified</returns>
        public static bool ContainsConfigFile<T>(string Name)
        {
            return ConfigFiles.ContainsKey(Name) && ConfigFiles[Name] is T;
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<ul>").Append("<li>").Append(ConfigFiles.Count).Append("</li>");
            foreach (string Name in ConfigFiles.Keys)
                Builder.Append("<li>").Append(Name).Append(":").Append(ConfigFiles[Name].GetType().FullName).Append("</li>");
            Builder.Append("</ul>");
            return Builder.ToString();
        }

        #endregion

        #endregion

        #region Private fields

        private static Dictionary<string, IConfig> ConfigFiles = new Dictionary<string, IConfig>();

        #endregion
    }
}