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
using System.IO;
using System.Text;
using Utilities.IO.FileSystem.Interfaces;

namespace Utilities.IO.FileSystem.BaseClasses
{
    /// <summary>
    /// Directory base class
    /// </summary>
    /// <typeparam name="FileType">File type</typeparam>
    /// <typeparam name="InternalFileType">Internal file type</typeparam>
    public abstract class FileBase<InternalFileType, FileType> : IFile
        where FileType : FileBase<InternalFileType, FileType>, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected FileBase()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InternalFile">Internal file</param>
        protected FileBase(InternalFileType InternalFile)
            : this()
        {
            this.InternalFile = InternalFile;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InternalFile">Internal file</param>
        /// <param name="UserName">User name</param>
        /// <param name="Password">Password</param>
        /// <param name="Domain">User's domain</param>
        protected FileBase(InternalFileType InternalFile, string UserName, string Password, string Domain)
            : this(InternalFile)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.Domain = Domain;
        }

        /// <summary>
        /// Last time accessed (UTC time)
        /// </summary>
        public abstract DateTime Accessed { get; }

        /// <summary>
        /// Time created (UTC time)
        /// </summary>
        public abstract DateTime Created { get; }

        /// <summary>
        /// Directory the file is within
        /// </summary>
        public abstract IDirectory Directory { get; }

        /// <summary>
        /// Does the file exist?
        /// </summary>
        public abstract bool Exists { get; }

        /// <summary>
        /// File extension
        /// </summary>
        public abstract string Extension { get; }

        /// <summary>
        /// Full path
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// Size of the file
        /// </summary>
        public abstract long Length { get; }

        /// <summary>
        /// Time modified (UTC time)
        /// </summary>
        public abstract DateTime Modified { get; }

        /// <summary>
        /// Name of the file
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Domain of the user
        /// </summary>
        protected string Domain { get; set; }

        /// <summary>
        /// Internal directory
        /// </summary>
        protected InternalFileType InternalFile { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        protected string Password { get; set; }

        /// <summary>
        /// User name used
        /// </summary>
        protected string UserName { get; set; }

        /// <summary>
        /// Reads the file and converts it to a byte array
        /// </summary>
        /// <param name="File">File to read</param>
        /// <returns>The file as a byte array</returns>
        public static implicit operator byte[](FileBase<InternalFileType, FileType> File)
        {
            if (File == null)
                return new byte[0];
            return File.ReadBinary();
        }

        /// <summary>
        /// Reads the file and converts it to a string
        /// </summary>
        /// <param name="File">File to read</param>
        /// <returns>The file as a string</returns>
        public static implicit operator string(FileBase<InternalFileType, FileType> File)
        {
            if (File == null)
                return "";
            return File.Read();
        }

        /// <summary>
        /// Determines if two directories are not equal
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(FileBase<InternalFileType, FileType> File1, IFile File2)
        {
            return !(File1 == File2);
        }

        /// <summary>
        /// Less than
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator <(FileBase<InternalFileType, FileType> File1, IFile File2)
        {
            if (File1 == null || File2 == null)
                return false;
            return string.Compare(File1.FullName, File2.FullName, StringComparison.OrdinalIgnoreCase) < 0;
        }

        /// <summary>
        /// Less than or equal
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator <=(FileBase<InternalFileType, FileType> File1, IFile File2)
        {
            if (File1 == null || File2 == null)
                return false;
            return string.Compare(File1.FullName, File2.FullName, StringComparison.OrdinalIgnoreCase) <= 0;
        }

        /// <summary>
        /// Determines if two directories are equal
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(FileBase<InternalFileType, FileType> File1, IFile File2)
        {
            if ((object)File1 == null && (object)File2 == null)
                return true;
            if ((object)File1 == null || (object)File2 == null)
                return false;
            return File1.FullName == File2.FullName;
        }

        /// <summary>
        /// Greater than
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator >(FileBase<InternalFileType, FileType> File1, IFile File2)
        {
            if (File1 == null || File2 == null)
                return false;
            return string.Compare(File1.FullName, File2.FullName, StringComparison.OrdinalIgnoreCase) > 0;
        }

        /// <summary>
        /// Greater than or equal
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator >=(FileBase<InternalFileType, FileType> File1, IFile File2)
        {
            if (File1 == null || File2 == null)
                return false;
            return string.Compare(File1.FullName, File2.FullName, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Clones the file object
        /// </summary>
        /// <returns>The cloned object</returns>
        public object Clone()
        {
            var Temp = new FileType();
            Temp.InternalFile = InternalFile;
            Temp.Password = Password;
            Temp.UserName = UserName;
            Temp.Domain = Domain;
            return Temp;
        }

        /// <summary>
        /// Compares this to another file
        /// </summary>
        /// <param name="other">File to compare to</param>
        /// <returns></returns>
        public int CompareTo(IFile other)
        {
            if (other == null)
                return 1;
            return string.Compare(FullName, other.FullName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Compares this object to another object
        /// </summary>
        /// <param name="obj">Object to compare it to</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            var Temp = obj as FileBase<InternalFileType, FileType>;
            if (Temp == null)
                return 1;
            return CompareTo(Temp);
        }

        /// <summary>
        /// Copies the file to another directory
        /// </summary>
        /// <param name="Directory">Directory to copy the file to</param>
        /// <param name="Overwrite">Should the file overwrite another file if found</param>
        /// <returns>The newly created file</returns>
        public abstract IFile CopyTo(IDirectory Directory, bool Overwrite);

        /// <summary>
        /// Deletes the file
        /// </summary>
        /// <returns>Any response for deleting the resource (usually FTP, HTTP, etc)</returns>
        public abstract string Delete();

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var File = obj as FileBase<InternalFileType, FileType>;
            return File != null && File == this;
        }

        /// <summary>
        /// Determines if the files are equal
        /// </summary>
        /// <param name="other">Other file</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public bool Equals(IFile other)
        {
            if (other == null)
                return false;
            return other.FullName == FullName;
        }

        /// <summary>
        /// Gets the hash code for the file
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        /// <summary>
        /// Moves the file to a new directory
        /// </summary>
        /// <param name="Directory">Directory to move to</param>
        public abstract void MoveTo(IDirectory Directory);

        /// <summary>
        /// Reads the file in as a string
        /// </summary>
        /// <returns>The file contents as a string</returns>
        public abstract string Read();

        /// <summary>
        /// Reads a file as binary
        /// </summary>
        /// <returns>The file contents as a byte array</returns>
        public abstract byte[] ReadBinary();

        /// <summary>
        /// Renames the file
        /// </summary>
        /// <param name="NewName">New name for the file</param>
        public abstract void Rename(string NewName);

        /// <summary>
        /// Returns the name of the file
        /// </summary>
        /// <returns>The name of the file</returns>
        public override string ToString()
        {
            return FullName;
        }

        /// <summary>
        /// Writes content to the file
        /// </summary>
        /// <param name="Content">Content to write</param>
        /// <param name="Mode">Mode to open the file as</param>
        /// <param name="Encoding">Encoding to use for the content</param>
        /// <returns>The result of the write or original content</returns>
        public abstract string Write(string Content, System.IO.FileMode Mode = FileMode.Create, Encoding Encoding = null);

        /// <summary>
        /// Writes content to the file
        /// </summary>
        /// <param name="Content">Content to write</param>
        /// <param name="Mode">Mode to open the file as</param>
        /// <returns>The result of the write or original content</returns>
        public abstract byte[] Write(byte[] Content, System.IO.FileMode Mode = FileMode.Create);
    }
}