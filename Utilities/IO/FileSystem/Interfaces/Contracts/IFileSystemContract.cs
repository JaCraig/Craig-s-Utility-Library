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

using System.Diagnostics.Contracts;

namespace Utilities.IO.FileSystem.Interfaces.Contracts
{
    /// <summary>
    /// IFileSystem contract
    /// </summary>
    [ContractClassFor(typeof(IFileSystem))]
    public abstract class IFileSystemContract : IFileSystem
    {
        /// <summary>
        /// Name of the file system
        /// </summary>
        public string Name
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
                return null;
            }
        }

        /// <summary>
        /// Returns true if it can handle the path, false otherwise
        /// </summary>
        /// <param name="Path">The path to check against</param>
        /// <returns>True if it can handle the path, false otherwise</returns>
        public bool CanHandle(string Path)
        {
            return false;
        }

        /// <summary>
        /// Gets the directory representation for the directory
        /// </summary>
        /// <param name="Path">Path to the directory</param>
        /// <param name="UserName">User name to be used to access the directory (optional)</param>
        /// <param name="Password">Password to be used to access the directory (optional)</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <returns>The directory object</returns>
        public IDirectory Directory(string Path, string UserName = "", string Password = "", string Domain = "")
        {
            Contract.Ensures(Contract.Result<IDirectory>() != null);
            return null;
        }

        /// <summary>
        /// Gets the class representation for the file
        /// </summary>
        /// <param name="Path">Path to the file</param>
        /// <param name="UserName">User name to be used to access the file (optional)</param>
        /// <param name="Password">Password to be used to access the file (optional)</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <returns>The file object</returns>
        public IFile File(string Path, string UserName = "", string Password = "", string Domain = "")
        {
            Contract.Ensures(Contract.Result<IFile>() != null);
            return null;
        }
    }
}