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
using Utilities.DataTypes;
using Utilities.IO.Enums;
using Utilities.IO.FileSystem.Interfaces;

namespace Utilities.IO.FileSystem.BaseClasses
{
    /// <summary>
    /// Directory base class
    /// </summary>
    /// <typeparam name="InternalDirectoryType">
    /// Data type internally to hold true directory info
    /// </typeparam>
    /// <typeparam name="DirectoryType">Directory type</typeparam>
    public abstract class DirectoryBase<InternalDirectoryType, DirectoryType> : IDirectory
        where DirectoryType : DirectoryBase<InternalDirectoryType, DirectoryType>, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected DirectoryBase()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InternalDirectory">Internal directory object</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <param name="Password">Password to be used to access the file (optional)</param>
        /// <param name="UserName">User name to be used to access the file (optional)</param>
        protected DirectoryBase(InternalDirectoryType InternalDirectory, string UserName = "", string Password = "", string Domain = "")
        {
            this.InternalDirectory = InternalDirectory;
            this.UserName = UserName;
            this.Password = Password;
            this.Domain = Domain;
        }

        /// <summary>
        /// Last time accessed (UTC time)
        /// </summary>
        public abstract DateTime Accessed { get; }

        /// <summary>
        /// Date created (UTC time)
        /// </summary>
        public abstract DateTime Created { get; }

        /// <summary>
        /// Does it exist?
        /// </summary>
        public abstract bool Exists { get; }

        /// <summary>
        /// Full path
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// Date modified (UTC time)
        /// </summary>
        public abstract DateTime Modified { get; }

        /// <summary>
        /// Name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Parent directory
        /// </summary>
        public abstract IDirectory Parent { get; }

        /// <summary>
        /// Root directory
        /// </summary>
        public abstract IDirectory Root { get; }

        /// <summary>
        /// Size of the directory
        /// </summary>
        public abstract long Size { get; }

        /// <summary>
        /// Domain
        /// </summary>
        protected string Domain { get; set; }

        /// <summary>
        /// Internal directory
        /// </summary>
        protected InternalDirectoryType InternalDirectory { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        protected string Password { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        protected string UserName { get; set; }

        /// <summary>
        /// Determines if two directories are not equal
        /// </summary>
        /// <param name="Directory1">Directory 1</param>
        /// <param name="Directory2">Directory 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(DirectoryBase<InternalDirectoryType, DirectoryType> Directory1, IDirectory Directory2)
        {
            return !(Directory1 == Directory2);
        }

        /// <summary>
        /// Less than
        /// </summary>
        /// <param name="Directory1">Directory 1</param>
        /// <param name="Directory2">Directory 2</param>
        /// <returns>The result</returns>
        public static bool operator <(DirectoryBase<InternalDirectoryType, DirectoryType> Directory1, IDirectory Directory2)
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
        public static bool operator <=(DirectoryBase<InternalDirectoryType, DirectoryType> Directory1, IDirectory Directory2)
        {
            if (Directory1 == null || Directory2 == null)
                return false;
            return string.Compare(Directory1.FullName, Directory2.FullName, StringComparison.OrdinalIgnoreCase) <= 0;
        }

        /// <summary>
        /// Determines if two directories are equal
        /// </summary>
        /// <param name="Directory1">Directory 1</param>
        /// <param name="Directory2">Directory 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(DirectoryBase<InternalDirectoryType, DirectoryType> Directory1, IDirectory Directory2)
        {
            if ((object)Directory1 == null && (object)Directory2 == null)
                return true;
            if ((object)Directory1 == null || (object)Directory2 == null)
                return false;
            return Directory1.FullName == Directory2.FullName;
        }

        /// <summary>
        /// Greater than
        /// </summary>
        /// <param name="Directory1">Directory 1</param>
        /// <param name="Directory2">Directory 2</param>
        /// <returns>The result</returns>
        public static bool operator >(DirectoryBase<InternalDirectoryType, DirectoryType> Directory1, IDirectory Directory2)
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
        public static bool operator >=(DirectoryBase<InternalDirectoryType, DirectoryType> Directory1, IDirectory Directory2)
        {
            if (Directory1 == null || Directory2 == null)
                return false;
            return string.Compare(Directory1.FullName, Directory2.FullName, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Clones the directory object
        /// </summary>
        /// <returns>The cloned object</returns>
        public object Clone()
        {
            DirectoryBase<InternalDirectoryType, DirectoryType> Temp = new DirectoryType();
            Temp.InternalDirectory = InternalDirectory;
            Temp.UserName = UserName;
            Temp.Password = Password;
            Temp.Domain = Domain;
            return Temp;
        }

        /// <summary>
        /// Compares this to another directory
        /// </summary>
        /// <param name="other">Directory to compare to</param>
        /// <returns></returns>
        public int CompareTo(IDirectory other)
        {
            if (other == null)
                return 1;
            if (InternalDirectory == null)
                return -1;
            return string.Compare(FullName, other.FullName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Compares this object to another object
        /// </summary>
        /// <param name="obj">Object to compare it to</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            IDirectory Temp = obj as IDirectory;
            if (Temp == null)
                return 1;
            return CompareTo(Temp);
        }

        /// <summary>
        /// Copies the directory to the specified parent directory
        /// </summary>
        /// <param name="Directory">Directory to copy to</param>
        /// <param name="Options">Copy options</param>
        /// <returns>Returns the new directory</returns>
        public virtual IDirectory CopyTo(IDirectory Directory, CopyOptions Options = CopyOptions.CopyAlways)
        {
            if (InternalDirectory == null || Directory == null)
                return this;
            Directory.Create();
            foreach (IFile TempFile in EnumerateFiles())
            {
                if (Options == CopyOptions.CopyAlways)
                {
                    TempFile.CopyTo(Directory, true);
                }
                else if (Options == CopyOptions.CopyIfNewer)
                {
                    if (File.Exists(Path.Combine(Directory.FullName, TempFile.Name)))
                    {
                        FileInfo FileInfo = new FileInfo(Path.Combine(Directory.FullName, TempFile.Name));
                        if (FileInfo.Modified.CompareTo(TempFile.Modified) < 0)
                            TempFile.CopyTo(Directory, true);
                    }
                    else
                    {
                        TempFile.CopyTo(Directory, true);
                    }
                }
                else if (Options == CopyOptions.DoNotOverwrite)
                {
                    TempFile.CopyTo(Directory, false);
                }
            }
            foreach (IDirectory SubDirectory in EnumerateDirectories())
                SubDirectory.CopyTo(new DirectoryInfo(Path.Combine(Directory.FullName, SubDirectory.Name)), Options);
            return Directory;
        }

        /// <summary>
        /// Creates the directory
        /// </summary>
        public abstract void Create();

        /// <summary>
        /// Deletes the directory
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// Enumerates directories under this directory
        /// </summary>
        /// <param name="SearchPattern">Search pattern</param>
        /// <param name="Options">Search options</param>
        /// <returns>List of directories under this directory</returns>
        public abstract IEnumerable<IDirectory> EnumerateDirectories(string SearchPattern = "*", SearchOption Options = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Enumerates sub directories (defaults to top level sub directories)
        /// </summary>
        /// <param name="Predicate">Predicate used to filter directories</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of directories</returns>
        public IEnumerable<IDirectory> EnumerateDirectories(Predicate<IDirectory> Predicate, SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            return EnumerateDirectories("*", Options).Where(x => Predicate(x));
        }

        /// <summary>
        /// Enumerates files under this directory
        /// </summary>
        /// <param name="SearchPattern">Search pattern</param>
        /// <param name="Options">Search options</param>
        /// <returns>List of files under this directory</returns>
        public abstract IEnumerable<IFile> EnumerateFiles(string SearchPattern = "*", SearchOption Options = SearchOption.TopDirectoryOnly);

        /// <summary>
        /// Enumerates files within the directory (defaults to top level directory and not the sub directories)
        /// </summary>
        /// <param name="Predicate">Predicate used to filter files</param>
        /// <param name="Options">Search options to use</param>
        /// <returns>The list of files</returns>
        public IEnumerable<IFile> EnumerateFiles(Predicate<IFile> Predicate, SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            return EnumerateFiles("*", Options).Where(x => Predicate(x));
        }

        /// <summary>
        /// Determines if the two directories are the same
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they're the same, false otherwise</returns>
        public override bool Equals(object obj)
        {
            DirectoryBase<InternalDirectoryType, DirectoryType> Other = obj as DirectoryBase<InternalDirectoryType, DirectoryType>;
            return Other != null && Other == this;
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
        /// Enumerates the files in the directory
        /// </summary>
        /// <returns>The files in the directory</returns>
        public IEnumerator<IFile> GetEnumerator()
        {
            foreach (IFile File in EnumerateFiles())
                yield return File;
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
        /// Moves this directory under another directory
        /// </summary>
        /// <param name="Directory">Directory to move to</param>
        public virtual void MoveTo(IDirectory Directory)
        {
            CopyTo(Directory);
            Delete();
        }

        /// <summary>
        /// Renames the directory
        /// </summary>
        /// <param name="Name">Name of the new directory</param>
        public abstract void Rename(string Name);

        /// <summary>
        /// Enumerates the files and directories in the directory
        /// </summary>
        /// <returns>The files and directories</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (IFile File in EnumerateFiles())
                yield return File;
        }

        /// <summary>
        /// Gets info for the directory
        /// </summary>
        /// <returns>The full path to the directory</returns>
        public override string ToString()
        {
            return FullName;
        }
    }
}