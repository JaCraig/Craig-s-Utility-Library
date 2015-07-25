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
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Utilities.DataTypes;

namespace Ironman.Models.Plugins
{
    /// <summary>
    /// Files included with a plugin
    /// </summary>
    [Serializable]
    public class PluginFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginFile" /> class.
        /// </summary>
        public PluginFile()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a directory.
        /// </summary>
        /// <value><c>true</c> if this instance is a directory; otherwise, <c>false</c>.</value>
        public bool IsDirectory { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        /// <summary>
        /// Removes this instance.
        /// </summary>
        public void Remove()
        {
            try
            {
                if (IsDirectory)
                    Delete(new DirectoryInfo(HttpContext.Current != null ? HttpContext.Current.Server.MapPath(Path) : Path.Replace("~", AppDomain.CurrentDomain.BaseDirectory)));
                else
                    new FileInfo(HttpContext.Current != null ? HttpContext.Current.Server.MapPath(Path) : Path.Replace("~", AppDomain.CurrentDomain.BaseDirectory)).Delete();
            }
            catch { }
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="Directory">The directory.</param>
        private void Delete(DirectoryInfo Directory)
        {
            Contract.Requires<ArgumentNullException>(Directory != null, "Directory");
            if (!Directory.Exists)
                return;
            foreach (FileInfo File in Directory.EnumerateFiles())
            {
                try
                {
                    File.Delete();
                }
                catch { }
            }
            foreach (DirectoryInfo SubDirectory in Directory.EnumerateDirectories())
            {
                Delete(SubDirectory);
            }
            try
            {
                Directory.Delete();
            }
            catch { }
        }
    }
}