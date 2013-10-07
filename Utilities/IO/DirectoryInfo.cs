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
using System.IO;
using System.Linq;
using Utilities.IO.Enums;
using Utilities.IO.FileSystem;
using Utilities.IO.FileSystem.Interfaces;

#endregion

namespace Utilities.IO
{
    /// <summary>
    /// Directory info class
    /// </summary>
    public class DirectoryInfo : IDirectory
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the directory</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <param name="Password">Password to be used to access the directory (optional)</param>
        /// <param name="UserName">User name to be used to access the directory (optional)</param>
        public DirectoryInfo(string Path, string UserName = "", string Password = "", string Domain = "")
            : this(IoC.Manager.Bootstrapper.Resolve<Manager>().Directory(Path, UserName, Password, Domain))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Directory">Directory object</param>
        public DirectoryInfo(IDirectory Directory)
        {
            InternalDirectory = Directory;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Internal directory object
        /// </summary>
        protected IDirectory InternalDirectory { get; private set; }

        /// <summary>
        /// Last time it was accessed
        /// </summary>
        public DateTime Accessed { get { return InternalDirectory == null ? DateTime.Now : InternalDirectory.Accessed; } }

        /// <summary>
        /// When it was created
        /// </summary>
        public DateTime Created { get { return InternalDirectory == null ? DateTime.Now : InternalDirectory.Created; } }

        /// <summary>
        /// When it was last modified
        /// </summary>
        public DateTime Modified { get { return InternalDirectory == null ? DateTime.Now : InternalDirectory.Modified; } }

        /// <summary>
        /// Does the directory exist
        /// </summary>
        public bool Exists { get { return InternalDirectory == null ? false : InternalDirectory.Exists; } }

        /// <summary>
        /// Full path to the directory
        /// </summary>
        public string FullName { get { return InternalDirectory == null ? "" : InternalDirectory.FullName; } }

        /// <summary>
        /// Name of the directory
        /// </summary>
        public string Name { get { return InternalDirectory == null ? "" : InternalDirectory.Name; } }

        /// <summary>
        /// Parent directory
        /// </summary>
        public IDirectory Parent { get { return InternalDirectory == null ? null : new DirectoryInfo(InternalDirectory.Parent); } }

        /// <summary>
        /// Root directory
        /// </summary>
        public IDirectory Root { get { return InternalDirectory == null ? null : new DirectoryInfo(InternalDirectory.Root); } }

        /// <summary>
        /// Size of the contents of the directory in bytes
        /// </summary>
        public long Size { get { return InternalDirectory == null ? 0 : InternalDirectory.Size; } }

        #endregion

        #region Functions

        /// <summary>
        /// Creates the directory if it does not currently exist
        /// </summary>
        public void Create()
        {
            if (InternalDirectory == null)
                return;
            InternalDirectory.Create();
        }

        /// <summary>
        /// Deletes the directory
        /// </summary>
        public void Delete()
        {
            if (InternalDirectory == null)
                return;
            InternalDirectory.Delete();
        }

        /// <summary>
        /// Enumerates sub directories (defaults to top level sub directories)
        /// </summary>
        /// <param name="SearchPattern">Search pattern to use</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of directories</returns>
        public IEnumerable<IDirectory> EnumerateDirectories(string SearchPattern = "*", SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            if (InternalDirectory != null)
            {
                foreach (IDirectory Directory in InternalDirectory.EnumerateDirectories(SearchPattern, Options))
                {
                    yield return new DirectoryInfo(Directory);
                }
            }
        }

        /// <summary>
        /// Enumerates files within the directory (defaults to top level directory and not the sub directories)
        /// </summary>
        /// <param name="SearchPattern">Search pattern to use</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of files</returns>
        public IEnumerable<IFile> EnumerateFiles(string SearchPattern = "*", SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            if (InternalDirectory != null)
            {
                foreach (IFile File in InternalDirectory.EnumerateFiles(SearchPattern, Options))
                {
                    yield return new Utilities.IO.FileInfo(File);
                }
            }
        }

        /// <summary>
        /// Enumerates sub directories (defaults to top level sub directories)
        /// </summary>
        /// <param name="Predicate">Predicate used to filter directories</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of directories</returns>
        public IEnumerable<IDirectory> EnumerateDirectories(Predicate<IDirectory> Predicate, SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            if (InternalDirectory != null)
            {
                return InternalDirectory.EnumerateDirectories("*", Options).Where(x => Predicate(x)).Select(x => new DirectoryInfo(x));
            }
            return new List<IDirectory>();
        }

        /// <summary>
        /// Enumerates files within the directory (defaults to top level directory and not the sub directories)
        /// </summary>
        /// <param name="Predicate">Predicate used to filter files</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of files</returns>
        public IEnumerable<IFile> EnumerateFiles(Predicate<IFile> Predicate, SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            if (InternalDirectory != null)
            {
                return InternalDirectory.EnumerateFiles("*", Options).Where(x => Predicate(x)).Select(x => new FileInfo(x));
            }
            return new List<IFile>();
        }

        /// <summary>
        /// Moves the directory to the specified parent directory
        /// </summary>
        /// <param name="Directory">Directory to move to</param>
        public void MoveTo(IDirectory Directory)
        {
            if (InternalDirectory == null || Directory == null)
                return;
            InternalDirectory.MoveTo(Directory);
        }

        /// <summary>
        /// Copies the directory to the specified parent directory
        /// </summary>
        /// <param name="Directory">Directory to copy to</param>
        /// <param name="Options">Copy options</param>
        /// <returns>Returns the new directory</returns>
        public IDirectory CopyTo(IDirectory Directory, Enums.CopyOptions Options = CopyOptions.CopyAlways)
        {
            if (InternalDirectory == null || Directory == null)
                return null;
            return InternalDirectory.CopyTo(Directory, Options);
        }

        /// <summary>
        /// Renames the directory
        /// </summary>
        /// <param name="Name">The new name of the directory</param>
        public void Rename(string Name)
        {
            if (InternalDirectory == null || string.IsNullOrEmpty(Name))
                return;
            InternalDirectory.Rename(Name);
        }

        /// <summary>
        /// Determines if the two directories are the same
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they're the same, false otherwise</returns>
        public override bool Equals(object obj)
        {
            DirectoryInfo Other = obj as DirectoryInfo;
            return Other != null && Other == this;
        }

        /// <summary>
        /// Returns the hash code for the directory
        /// </summary>
        /// <returns>The hash code for the directory</returns>
        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        /// <summary>
        /// Gets info for the directory
        /// </summary>
        /// <returns>The full path to the directory</returns>
        public override string ToString()
        {
            return FullName;
        }

        /// <summary>
        /// Compares this to another directory
        /// </summary>
        /// <param name="other">Directory to compare to</param>
        /// <returns>-1 if this is smaller, 0 if they are the same, 1 if it is larger</returns>
        public int CompareTo(IDirectory other)
        {
            if (other == null)
                return 1;
            if (InternalDirectory == null)
                return -1;
            return string.Compare(FullName, other.FullName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Enumerates the files in the directory
        /// </summary>
        /// <returns>The files in the directory</returns>
        public IEnumerator<IFile> GetEnumerator()
        {
            foreach (FileInfo File in EnumerateFiles())
                yield return File;
        }

        /// <summary>
        /// Enumerates the files and directories in the directory
        /// </summary>
        /// <returns>The files and directories</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (FileInfo File in EnumerateFiles())
                yield return File;
        }

        /// <summary>
        /// Compares this object to another object
        /// </summary>
        /// <param name="obj">Object to compare it to</param>
        /// <returns>-1 if this is smaller, 0 if they are the same, 1 if it is larger</returns>
        public int CompareTo(object obj)
        {
            DirectoryInfo Temp = obj as DirectoryInfo;
            if (Temp == null)
                return 1;
            return CompareTo(Temp);
        }

        /// <summary>
        /// Determines if the directories are equal
        /// </summary>
        /// <param name="other">Other directory</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public bool Equals(IDirectory other)
        {
            if (other == null)
                return false;
            return FullName == other.FullName;
        }

        /// <summary>
        /// Clones the directory object
        /// </summary>
        /// <returns>The cloned object</returns>
        public object Clone()
        {
            DirectoryInfo Temp = new DirectoryInfo(InternalDirectory);
            return Temp;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines if two directories are equal
        /// </summary>
        /// <param name="Directory1">Directory 1</param>
        /// <param name="Directory2">Directory 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(DirectoryInfo Directory1, DirectoryInfo Directory2)
        {
            if ((object)Directory1 == null && (object)Directory2 == null)
                return true;
            if ((object)Directory1 == null || (object)Directory2 == null)
                return false;
            return Directory1.FullName == Directory2.FullName;
        }

        /// <summary>
        /// Determines if two directories are not equal
        /// </summary>
        /// <param name="Directory1">Directory 1</param>
        /// <param name="Directory2">Directory 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(DirectoryInfo Directory1, DirectoryInfo Directory2)
        {
            return !(Directory1 == Directory2);
        }

        /// <summary>
        /// Less than
        /// </summary>
        /// <param name="Directory1">Directory 1</param>
        /// <param name="Directory2">Directory 2</param>
        /// <returns>The result</returns>
        public static bool operator <(DirectoryInfo Directory1, DirectoryInfo Directory2)
        {
            if (Directory1 == null || Directory2 == null)
                return false;
            return string.Compare(Directory1.FullName, Directory2.FullName, StringComparison.OrdinalIgnoreCase) < 0;
        }

        /// <summary>
        /// Less than or equal
        /// </summary>
        /// <param name="Directory1">Directory 1</param>
        /// <param name="Directory2">Directory 2</param>
        /// <returns>The result</returns>
        public static bool operator <=(DirectoryInfo Directory1, DirectoryInfo Directory2)
        {
            if (Directory1 == null || Directory2 == null)
                return false;
            return string.Compare(Directory1.FullName, Directory2.FullName, StringComparison.OrdinalIgnoreCase) <= 0;
        }

        /// <summary>
        /// Greater than
        /// </summary>
        /// <param name="Directory1">Directory 1</param>
        /// <param name="Directory2">Directory 2</param>
        /// <returns>The result</returns>
        public static bool operator >(DirectoryInfo Directory1, DirectoryInfo Directory2)
        {
            if (Directory1 == null || Directory2 == null)
                return false;
            return string.Compare(Directory1.FullName, Directory2.FullName, StringComparison.OrdinalIgnoreCase) > 0;
        }

        /// <summary>
        /// Greater than or equal
        /// </summary>
        /// <param name="Directory1">Directory 1</param>
        /// <param name="Directory2">Directory 2</param>
        /// <returns>The result</returns>
        public static bool operator >=(DirectoryInfo Directory1, DirectoryInfo Directory2)
        {
            if (Directory1 == null || Directory2 == null)
                return false;
            return string.Compare(Directory1.FullName, Directory2.FullName, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        #endregion
    }
}