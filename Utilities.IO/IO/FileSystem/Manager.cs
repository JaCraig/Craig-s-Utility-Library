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
using System.Linq;
using Utilities.DataTypes;
using Utilities.DataTypes.Patterns.BaseClasses;
using Utilities.IO.FileSystem.Interfaces;

namespace Utilities.IO.FileSystem
{
    /// <summary>
    /// File system manager
    /// </summary>
    public class Manager : SafeDisposableBaseClass
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FileSystems">The file systems.</param>
        public Manager(IEnumerable<IFileSystem> FileSystems)
        {
            this.FileSystems = FileSystems;
        }

        /// <summary>
        /// File systems that the library can use
        /// </summary>
        protected IEnumerable<IFileSystem> FileSystems { get; private set; }

        /// <summary>
        /// Gets the file system by name
        /// </summary>
        /// <param name="Name">Name of the file system</param>
        /// <returns>The file system specified</returns>
        public IFileSystem this[string Name] { get { return FileSystems.FirstOrDefault(x => x.Name == Name); } }

        /// <summary>
        /// Gets the directory representation for the directory
        /// </summary>
        /// <param name="Path">Path to the directory</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <param name="Password">Password to be used to access the directory (optional)</param>
        /// <param name="UserName">User name to be used to access the directory (optional)</param>
        /// <returns>The directory object</returns>
        public IDirectory Directory(string Path, string UserName = "", string Password = "", string Domain = "")
        {
            IFileSystem FileSystem = FindSystem(Path);
            return FileSystem == null ? null : FileSystem.Directory(Path, UserName, Password, Domain);
        }

        /// <summary>
        /// Gets the class representation for the file
        /// </summary>
        /// <param name="Path">Path to the file</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <param name="Password">Password to be used to access the file (optional)</param>
        /// <param name="UserName">User name to be used to access the file (optional)</param>
        /// <returns>The file object</returns>
        public IFile File(string Path, string UserName = "", string Password = "", string Domain = "")
        {
            IFileSystem FileSystem = FindSystem(Path);
            return FileSystem == null ? null : FileSystem.File(Path, UserName, Password, Domain);
        }

        /// <summary>
        /// Outputs the file system information in string format
        /// </summary>
        /// <returns>The list of file systems that are available</returns>
        public override string ToString()
        {
            return "File systems: " + FileSystems.ToString(x => x.Name) + "\r\n";
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        /// <param name="Managed">
        /// Determines if all objects should be disposed or just managed objects
        /// </param>
        protected override void Dispose(bool Managed)
        {
            if (FileSystems != null)
            {
                foreach (IDisposable FileSystem in FileSystems)
                {
                    FileSystem.Dispose();
                }
                FileSystems = null;
            }
        }

        /// <summary>
        /// Finds a file system compatible with the path
        /// </summary>
        /// <param name="Path">Path to search for</param>
        /// <returns>The file system associated with the path</returns>
        protected IFileSystem FindSystem(string Path)
        {
            if (FileSystems == null)
                return null;
            return FileSystems.FirstOrDefault(x => x.CanHandle(Path));
        }
    }
}