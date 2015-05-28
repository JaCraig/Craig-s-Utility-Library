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
using Utilities.IO.Enums;
using Utilities.IO.FileSystem.Interfaces.Contracts;

namespace Utilities.IO.FileSystem.Interfaces
{
    /// <summary>
    /// Represents a directory
    /// </summary>
    [ContractClass(typeof(IDirectoryContract))]
    public interface IDirectory : IComparable<IDirectory>, IEnumerable<IFile>, IComparable, IEquatable<IDirectory>, ICloneable
    {
        /// <summary>
        /// Last time it was accessed
        /// </summary>
        DateTime Accessed { get; }

        /// <summary>
        /// When it was created
        /// </summary>
        DateTime Created { get; }

        /// <summary>
        /// Does the directory exist
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Full path to the directory
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// When it was last modified
        /// </summary>
        DateTime Modified { get; }

        /// <summary>
        /// Name of the directory
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Parent directory
        /// </summary>
        IDirectory Parent { get; }

        /// <summary>
        /// Root directory
        /// </summary>
        IDirectory Root { get; }

        /// <summary>
        /// Size of the contents of the directory in bytes
        /// </summary>
        long Size { get; }

        /// <summary>
        /// Copies the directory to the specified parent directory
        /// </summary>
        /// <param name="Directory">Directory to copy to</param>
        /// <param name="Options">Copy options</param>
        IDirectory CopyTo(IDirectory Directory, CopyOptions Options = CopyOptions.CopyAlways);

        /// <summary>
        /// Creates the directory if it does not currently exist
        /// </summary>
        void Create();

        /// <summary>
        /// Deletes the directory
        /// </summary>
        void Delete();

        /// <summary>
        /// Enumerates sub directories (defaults to top level sub directories)
        /// </summary>
        /// <param name="SearchPattern">Search pattern to use</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of directories</returns>
        IEnumerable<IDirectory> EnumerateDirectories(string SearchPattern = "*", SearchOption Options = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Enumerates sub directories (defaults to top level sub directories)
        /// </summary>
        /// <param name="Predicate">Predicate used to filter directories</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of directories</returns>
        IEnumerable<IDirectory> EnumerateDirectories(Predicate<IDirectory> Predicate, SearchOption Options = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Enumerates files within the directory (defaults to top level directory and not the sub directories)
        /// </summary>
        /// <param name="SearchPattern">Search pattern to use</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of files</returns>
        IEnumerable<IFile> EnumerateFiles(string SearchPattern = "*", SearchOption Options = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Enumerates files within the directory (defaults to top level directory and not the sub directories)
        /// </summary>
        /// <param name="Predicate">Predicate used to filter files</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of files</returns>
        IEnumerable<IFile> EnumerateFiles(Predicate<IFile> Predicate, SearchOption Options = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Moves the directory to the specified parent directory
        /// </summary>
        /// <param name="Directory">Directory to move to</param>
        IDirectory MoveTo(IDirectory Directory);

        /// <summary>
        /// Renames the directory
        /// </summary>
        /// <param name="Name">The new name of the directory</param>
        void Rename(string Name);
    }
}