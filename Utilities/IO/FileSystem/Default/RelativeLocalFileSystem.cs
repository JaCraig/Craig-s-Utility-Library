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
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using Utilities.IO.FileSystem.BaseClasses;
#endregion

namespace Utilities.IO.FileSystem.Default
{
    /// <summary>
    /// Relative local file system
    /// </summary>
    public class RelativeLocalFileSystem : LocalFileSystemBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public RelativeLocalFileSystem() : base() { }

        #endregion

        #region Properties

        /// <summary>
        /// Relative starter
        /// </summary>
        protected override string HandleRegexString { get { return @"^[~|\.]"; } }

        /// <summary>
        /// Name of the file system
        /// </summary>
        public override string Name { get { return "Relative Local"; } }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the absolute path of the variable passed in
        /// </summary>
        /// <param name="Path">Path to convert to absolute</param>
        /// <returns>The absolute path of the path passed in</returns>
        protected override string AbsolutePath(string Path)
        {
            Path = Path.Replace("/", "\\");
            string BaseDirectory = "";
            string ParentDirectory = "";
            if (HttpContext.Current == null)
            {
                BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                ParentDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
            }
            else
            {
                BaseDirectory = HttpContext.Current.Server.MapPath("~/");
                ParentDirectory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/")).Parent.FullName;
            }
            if (Path.StartsWith("..\\",StringComparison.OrdinalIgnoreCase))
            {
                Path = ParentDirectory + Path.Remove(0, 2);
            }
            else if (Path.StartsWith(".\\", StringComparison.OrdinalIgnoreCase))
            {
                Path = BaseDirectory + Path.Remove(0, 1);
            }
            else if (Path.StartsWith("~\\", StringComparison.OrdinalIgnoreCase))
            {
                Path = BaseDirectory + Path.Remove(0, 1);
            }
            return Path;
        }

        #endregion
    }
}