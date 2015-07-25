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

namespace Utilities.IO.FileSystem.Interfaces.Contracts
{
    /// <summary>
    /// IDirectory contract class
    /// </summary>
    [ContractClassFor(typeof(IDirectory))]
    public abstract class IDirectoryContract : IDirectory
    {
        /// <summary>
        /// Last time it was accessed
        /// </summary>
        public DateTime Accessed
        {
            get { return default(DateTime); }
        }

        /// <summary>
        /// When it was created
        /// </summary>
        public DateTime Created
        {
            get { return default(DateTime); }
        }

        /// <summary>
        /// Does the directory exist
        /// </summary>
        public bool Exists
        {
            get { return false; }
        }

        /// <summary>
        /// Full path to the directory
        /// </summary>
        public string FullName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        /// <summary>
        /// When it was last modified
        /// </summary>
        public DateTime Modified
        {
            get { return default(DateTime); }
        }

        /// <summary>
        /// Name of the directory
        /// </summary>
        public string Name
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return null;
            }
        }

        /// <summary>
        /// Parent directory
        /// </summary>
        public IDirectory Parent
        {
            get { return null; }
        }

        /// <summary>
        /// Root directory
        /// </summary>
        public IDirectory Root
        {
            get { return null; }
        }

        /// <summary>
        /// Size of the contents of the directory in bytes
        /// </summary>
        public long Size
        {
            get { return 0; }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return null;
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return
        /// value has the following meanings: Value Meaning Less than zero This object is less than
        /// the <paramref name="other"/> parameter.Zero This object is equal to <paramref
        /// name="other"/>. Greater than zero This object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(IDirectory other)
        {
            return 0;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an
        /// integer that indicates whether the current instance precedes, follows, or occurs in the
        /// same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return
        /// value has these meanings: Value Meaning Less than zero This instance precedes <paramref
        /// name="obj"/> in the sort order. Zero This instance occurs in the same position in the
        /// sort order as <paramref name="obj"/>. Greater than zero This instance follows <paramref
        /// name="obj"/> in the sort order.
        /// </returns>
        public int CompareTo(object obj)
        {
            return 0;
        }

        /// <summary>
        /// Copies the directory to the specified parent directory
        /// </summary>
        /// <param name="Directory">Directory to copy to</param>
        /// <param name="Options">Copy options</param>
        /// <returns></returns>
        public IDirectory CopyTo(IDirectory Directory, Enums.CopyOptions Options = CopyOptions.CopyAlways)
        {
            Contract.Ensures(Contract.Result<IDirectory>() != null);
            return null;
        }

        /// <summary>
        /// Creates the directory if it does not currently exist
        /// </summary>
        public void Create()
        {
        }

        /// <summary>
        /// Deletes the directory
        /// </summary>
        public void Delete()
        {
        }

        /// <summary>
        /// Enumerates sub directories (defaults to top level sub directories)
        /// </summary>
        /// <param name="SearchPattern">Search pattern to use</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of directories</returns>
        public IEnumerable<IDirectory> EnumerateDirectories(string SearchPattern = "*", System.IO.SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            Contract.Ensures(Contract.Result<IEnumerable<IDirectory>>() != null);
            return null;
        }

        /// <summary>
        /// Enumerates sub directories (defaults to top level sub directories)
        /// </summary>
        /// <param name="Predicate">Predicate used to filter directories</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of directories</returns>
        public IEnumerable<IDirectory> EnumerateDirectories(Predicate<IDirectory> Predicate, System.IO.SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            Contract.Requires<ArgumentNullException>(Predicate != null);
            Contract.Ensures(Contract.Result<IEnumerable<IDirectory>>() != null);
            return null;
        }

        /// <summary>
        /// Enumerates files within the directory (defaults to top level directory and not the sub directories)
        /// </summary>
        /// <param name="SearchPattern">Search pattern to use</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of files</returns>
        public IEnumerable<IFile> EnumerateFiles(string SearchPattern = "*", System.IO.SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            Contract.Ensures(Contract.Result<IEnumerable<IFile>>() != null);
            return null;
        }

        /// <summary>
        /// Enumerates files within the directory (defaults to top level directory and not the sub directories)
        /// </summary>
        /// <param name="Predicate">Predicate used to filter files</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of files</returns>
        public IEnumerable<IFile> EnumerateFiles(Predicate<IFile> Predicate, System.IO.SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            Contract.Requires<ArgumentNullException>(Predicate != null);
            Contract.Ensures(Contract.Result<IEnumerable<IFile>>() != null);
            return null;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter;
        /// otherwise, false.
        /// </returns>
        public bool Equals(IDirectory other)
        {
            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate
        /// through the collection.
        /// </returns>
        public IEnumerator<IFile> GetEnumerator()
        {
            return null;
        }

        /// <summary>
        /// Moves the directory to the specified parent directory
        /// </summary>
        /// <param name="Directory">Directory to move to</param>
        /// <returns></returns>
        public IDirectory MoveTo(IDirectory Directory)
        {
            Contract.Ensures(Contract.Result<IDirectory>() != null);
            return null;
        }

        /// <summary>
        /// Renames the directory
        /// </summary>
        /// <param name="Name">The new name of the directory</param>
        public void Rename(string Name)
        {
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate
        /// through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return null;
        }
    }
}