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
using System.IO;
using System.Linq;
using Utilities.IO.FileSystem.BaseClasses;
using Utilities.IO.FileSystem.Interfaces;

namespace Utilities.IO.FileSystem.Default
{
    /// <summary>
    /// Local directory class
    /// </summary>
    public class LocalDirectory : DirectoryBase<System.IO.DirectoryInfo, LocalDirectory>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LocalDirectory()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the directory</param>
        public LocalDirectory(string Path)
            : base(string.IsNullOrEmpty(Path) ? null : new System.IO.DirectoryInfo(Path))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Directory">Internal directory</param>
        public LocalDirectory(System.IO.DirectoryInfo Directory)
            : base(Directory)
        {
        }

        /// <summary>
        /// Time accessed (UTC time)
        /// </summary>
        public override DateTime Accessed
        {
            get { return InternalDirectory == null ? DateTime.Now : InternalDirectory.LastAccessTimeUtc; }
        }

        /// <summary>
        /// Time created (UTC time)
        /// </summary>
        public override DateTime Created
        {
            get { return InternalDirectory == null ? DateTime.Now : InternalDirectory.CreationTimeUtc; }
        }

        /// <summary>
        /// Does the directory exist?
        /// </summary>
        public override bool Exists
        {
            get { return InternalDirectory != null && InternalDirectory.Exists; }
        }

        /// <summary>
        /// Full path of the directory
        /// </summary>
        public override string FullName
        {
            get { return InternalDirectory == null ? "" : InternalDirectory.FullName; }
        }

        /// <summary>
        /// Time modified (UTC time)
        /// </summary>
        public override DateTime Modified
        {
            get { return InternalDirectory == null ? DateTime.Now : InternalDirectory.LastWriteTimeUtc; }
        }

        /// <summary>
        /// Name of the directory
        /// </summary>
        public override string Name
        {
            get { return InternalDirectory == null ? "" : InternalDirectory.Name; }
        }

        /// <summary>
        /// Parent directory
        /// </summary>
        public override IDirectory Parent
        {
            get { return InternalDirectory == null ? null : new LocalDirectory(InternalDirectory.Parent); }
        }

        /// <summary>
        /// Root directory
        /// </summary>
        public override IDirectory Root
        {
            get { return InternalDirectory == null ? null : new LocalDirectory(InternalDirectory.Root); }
        }

        /// <summary>
        /// Size of the directory
        /// </summary>
        public override long Size
        {
            get { return Exists ? InternalDirectory.EnumerateFiles("*", SearchOption.AllDirectories).Sum(x => x.Length) : 0; }
        }

        /// <summary>
        /// Creates the directory
        /// </summary>
        public override void Create()
        {
            if (InternalDirectory == null)
                return;
            InternalDirectory.Create();
            InternalDirectory.Refresh();
        }

        /// <summary>
        /// Deletes the directory
        /// </summary>
        public override void Delete()
        {
            if (!Exists)
                return;
            foreach (IFile File in EnumerateFiles())
            {
                File.Delete();
            }
            foreach (IDirectory Directory in EnumerateDirectories())
            {
                Directory.Delete();
            }
            InternalDirectory.Delete(true);
            InternalDirectory.Refresh();
        }

        /// <summary>
        /// Enumerates directories under this directory
        /// </summary>
        /// <param name="SearchPattern">Search pattern</param>
        /// <param name="Options">Search options</param>
        /// <returns>List of directories under this directory</returns>
        public override IEnumerable<IDirectory> EnumerateDirectories(string SearchPattern = "*", SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            if (InternalDirectory != null)
            {
                foreach (System.IO.DirectoryInfo SubDirectory in InternalDirectory.EnumerateDirectories(SearchPattern, Options))
                {
                    yield return new LocalDirectory(SubDirectory);
                }
            }
        }

        /// <summary>
        /// Enumerates files under this directory
        /// </summary>
        /// <param name="SearchPattern">Search pattern</param>
        /// <param name="Options">Search options</param>
        /// <returns>List of files under this directory</returns>
        public override IEnumerable<IFile> EnumerateFiles(string SearchPattern = "*", SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            if (InternalDirectory != null)
            {
                foreach (System.IO.FileInfo File in InternalDirectory.EnumerateFiles(SearchPattern, Options))
                {
                    yield return new LocalFile(File);
                }
            }
        }

        /// <summary>
        /// Renames the directory
        /// </summary>
        /// <param name="Name">Name of the new directory</param>
        public override void Rename(string Name)
        {
            if (InternalDirectory == null || string.IsNullOrEmpty(Name))
                return;
            InternalDirectory.MoveTo(Parent.FullName + "\\" + Name);
            InternalDirectory = new System.IO.DirectoryInfo(Parent.FullName + "\\" + Name);
        }
    }
}