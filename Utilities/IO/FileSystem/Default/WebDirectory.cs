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
using Utilities.IO.FileSystem.Interfaces;
#endregion

namespace Utilities.IO.FileSystem.Default
{
    /// <summary>
    /// Directory class
    /// </summary>
    public class WebDirectory : DirectoryBase<Uri, WebDirectory>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public WebDirectory()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the directory</param>
        public WebDirectory(string Path)
            : base(new Uri(Path))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Directory">Internal directory</param>
        public WebDirectory(Uri Directory)
            : base(Directory)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// returns now
        /// </summary>
        public override DateTime Accessed
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// returns now
        /// </summary>
        public override DateTime Created
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// returns now
        /// </summary>
        public override DateTime Modified
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// returns true
        /// </summary>
        public override bool Exists
        {
            get { return true; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override string FullName
        {
            get { return InternalDirectory.AbsolutePath; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override string Name
        {
            get { return InternalDirectory.AbsolutePath; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override IDirectory Parent
        {
            get { return new WebDirectory((string)InternalDirectory.AbsolutePath.Take(InternalDirectory.AbsolutePath.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) - 1)); }
        }

        /// <summary>
        /// Root
        /// </summary>
        public override IDirectory Root
        {
            get { return new WebDirectory(InternalDirectory.Scheme + "://" + InternalDirectory.Host); }
        }

        /// <summary>
        /// Size (returns 0)
        /// </summary>
        public override long Size
        {
            get { return 0; }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Not used
        /// </summary>
        public override void Create()
        {

        }

        /// <summary>
        /// Not used
        /// </summary>
        public override void Delete()
        {

        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="SearchPattern"></param>
        /// <param name="Options"></param>
        /// <returns></returns>
        public override IEnumerable<IDirectory> EnumerateDirectories(string SearchPattern, SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            return new List<WebDirectory>();
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="SearchPattern"></param>
        /// <param name="Options"></param>
        /// <returns></returns>
        public override IEnumerable<IFile> EnumerateFiles(string SearchPattern="*", SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            return new List<WebFile>();
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Directory"></param>
        public override void MoveTo(IDirectory Directory)
        {
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Name"></param>
        public override void Rename(string Name)
        {
        }

        #endregion
    }
}