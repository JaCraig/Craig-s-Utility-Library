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

#region Usings

using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

#endregion Usings

namespace Glimpse.CUL
{
    /// <summary>
    /// Plugin that displays info from CUL
    /// </summary>
    public class Plugin : AspNetTab, IDocumentation
    {
        /// <summary>
        /// Documentation URL
        /// </summary>
        public string DocumentationUri { get { return "http://www.gutgames.com"; } }

        /// <summary>
        /// Name of the plugin
        /// </summary>
        public override string Name { get { return "Craig's Utility Library"; } }

        /// <summary>
        /// Gets the configuration manager.
        /// </summary>
        /// <value>The configuration manager.</value>
        private static Utilities.Configuration.Manager.Manager ConfigurationManager { get { Contract.Requires<ArgumentNullException>(Container != null, "Container"); return Container.Resolve<Utilities.Configuration.Manager.Manager>(); } }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        private static Utilities.IoC.Interfaces.IBootstrapper Container { get { return Utilities.IoC.Manager.Bootstrapper; } }

        /// <summary>
        /// Gets the file manager.
        /// </summary>
        /// <value>The file manager.</value>
        private static Utilities.IO.FileSystem.Manager FileManager { get { Contract.Requires<ArgumentNullException>(Container != null, "Container"); return Container.Resolve<Utilities.IO.FileSystem.Manager>(); } }

        /// <summary>
        /// Gets the logging manager.
        /// </summary>
        /// <value>The logging manager.</value>
        private static Utilities.IO.Logging.Manager LoggingManager { get { Contract.Requires<ArgumentNullException>(Container != null, "Container"); return Container.Resolve<Utilities.IO.Logging.Manager>(); } }

        /// <summary>
        /// Gets the serialization manager.
        /// </summary>
        /// <value>The serialization manager.</value>
        private static Utilities.IO.Serializers.Manager SerializationManager { get { Contract.Requires<ArgumentNullException>(Container != null, "Container"); return Container.Resolve<Utilities.IO.Serializers.Manager>(); } }

        /// <summary>
        /// Gets data for glimpse
        /// </summary>
        /// <param name="context">Tab context</param>
        /// <returns>A dictionary of information to put in the tab</returns>
        public override object GetData(ITabContext context)
        {
            Dictionary<string, string[]> Return = new Dictionary<string, string[]>();
            if (Container == null)
                return Return;
            Return.Add("IoC Container", new string[] { Container.Name });
            if (FileManager != null)
                Return.Add("File systems", FileManager.ToString().Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            if (ConfigurationManager != null)
                Return.Add("Configuration systems", ConfigurationManager.ToString().Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            if (LoggingManager != null)
                Return.Add("Logging systems", LoggingManager.ToString().Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            if (SerializationManager != null)
                Return.Add("Serializers", SerializationManager.ToString().Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            return Return;
        }
    }
}