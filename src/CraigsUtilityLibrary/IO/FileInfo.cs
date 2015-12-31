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
using System.Diagnostics;
using System.IO;
using System.Text;
using Utilities.IO.FileFormats;
using Utilities.IO.FileFormats.BaseClasses;
using Utilities.IO.FileSystem;
using Utilities.IO.FileSystem.Interfaces;

namespace Utilities.IO
{
    /// <summary>
    /// File info class
    /// </summary>
    public class FileInfo : IFile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <param name="domain">Domain of the user (optional)</param>
        /// <param name="password">Password to be used to access the file (optional)</param>
        /// <param name="userName">User name to be used to access the file (optional)</param>
        public FileInfo(string path, string userName = "", string password = "", string domain = "")
            : this(IoC.Manager.Bootstrapper.Resolve<Manager>().File(path, userName, password, domain))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="internalFile">Internal file</param>
        public FileInfo(IFile internalFile)
        {
            InternalFile = internalFile;
        }

        /// <summary>
        /// Last time accessed (UTC time)
        /// </summary>
        public DateTime Accessed => InternalFile == null ? DateTime.Now : InternalFile.Accessed;

        /// <summary>
        /// Time created (UTC time)
        /// </summary>
        public DateTime Created => InternalFile == null ? DateTime.Now : InternalFile.Created;

        /// <summary>
        /// Directory the file is within
        /// </summary>
        public IDirectory Directory => InternalFile == null ? null : InternalFile.Directory;

        /// <summary>
        /// Does the file exist?
        /// </summary>
        public bool Exists => InternalFile != null && InternalFile.Exists;

        /// <summary>
        /// File extension
        /// </summary>
        public string Extension => InternalFile == null ? "" : InternalFile.Extension;

        /// <summary>
        /// Full path
        /// </summary>
        public string FullName => InternalFile == null ? "" : InternalFile.FullName;

        /// <summary>
        /// Size of the file
        /// </summary>
        public long Length => InternalFile == null ? 0 : InternalFile.Length;

        /// <summary>
        /// Time modified (UTC time)
        /// </summary>
        public DateTime Modified => InternalFile == null ? DateTime.Now : InternalFile.Modified;

        /// <summary>
        /// Name of the file
        /// </summary>
        public string Name => InternalFile == null ? "" : InternalFile.Name;

        /// <summary>
        /// Internal directory
        /// </summary>
        protected IFile InternalFile { get; private set; }

        /// <summary>
        /// Reads the file and converts it to a byte array
        /// </summary>
        /// <param name="file">File to read</param>
        /// <returns>The file as a byte array</returns>
        public static implicit operator byte[] (FileInfo file)
        {
            if (file == null)
                return new byte[0];
            return file.ReadBinary();
        }

        /// <summary>
        /// Reads the file and converts it to a string
        /// </summary>
        /// <param name="file">File to read</param>
        /// <returns>The file as a string</returns>
        public static implicit operator string(FileInfo file)
        {
            if (file == null)
                return "";
            return file.Read();
        }

        /// <summary>
        /// Determines if two directories are not equal
        /// </summary>
        /// <param name="file1">File 1</param>
        /// <param name="file2">File 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(FileInfo file1, FileInfo file2)
        {
            return !(file1 == file2);
        }

        /// <summary>
        /// Less than
        /// </summary>
        /// <param name="file1">File 1</param>
        /// <param name="file2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator <(FileInfo file1, FileInfo file2)
        {
            if (file1 == null || file2 == null)
                return false;
            return string.Compare(file1.FullName, file2.FullName, StringComparison.OrdinalIgnoreCase) < 0;
        }

        /// <summary>
        /// Less than or equal
        /// </summary>
        /// <param name="file1">File 1</param>
        /// <param name="file2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator <=(FileInfo file1, FileInfo file2)
        {
            if (file1 == null || file2 == null)
                return false;
            return string.Compare(file1.FullName, file2.FullName, StringComparison.OrdinalIgnoreCase) <= 0;
        }

        /// <summary>
        /// Determines if two directories are equal
        /// </summary>
        /// <param name="file1">File 1</param>
        /// <param name="file2">File 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(FileInfo file1, FileInfo file2)
        {
            if ((object)file1 == null && (object)file2 == null)
                return true;
            if ((object)file1 == null || (object)file2 == null)
                return false;
            return file1.FullName == file2.FullName;
        }

        /// <summary>
        /// Greater than
        /// </summary>
        /// <param name="file1">File 1</param>
        /// <param name="file2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator >(FileInfo file1, FileInfo file2)
        {
            if (file1 == null || file2 == null)
                return false;
            return string.Compare(file1.FullName, file2.FullName, StringComparison.OrdinalIgnoreCase) > 0;
        }

        /// <summary>
        /// Greater than or equal
        /// </summary>
        /// <param name="file1">File 1</param>
        /// <param name="file2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator >=(FileInfo file1, FileInfo file2)
        {
            if (file1 == null || file2 == null)
                return false;
            return string.Compare(file1.FullName, file2.FullName, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Clones the file object
        /// </summary>
        /// <returns>The cloned object</returns>
        public object Clone()
        {
            var Temp = new FileInfo(InternalFile);
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
            if (InternalFile == null)
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
            var Temp = obj as FileInfo;
            if (Temp == null)
                return 1;
            return CompareTo(Temp);
        }

        /// <summary>
        /// Copies the file to another directory
        /// </summary>
        /// <param name="directory">Directory to copy the file to</param>
        /// <param name="overwrite">Should the file overwrite another file if found</param>
        /// <returns>The newly created file</returns>
        public IFile CopyTo(IDirectory directory, bool overwrite)
        {
            if (directory == null || !Exists)
                return null;
            return InternalFile.CopyTo(directory, overwrite);
        }

        /// <summary>
        /// Deletes the file
        /// </summary>
        /// <returns>Any response for deleting the resource (usually FTP, HTTP, etc)</returns>
        public string Delete()
        {
            if (InternalFile == null)
                return "";
            return InternalFile.Delete();
        }

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var File = obj as FileInfo;
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
        /// Executes the file
        /// </summary>
        /// <param name="info">Info used to execute the file</param>
        /// <returns>The process object created when the executable is started</returns>
        public Process Execute(ProcessStartInfo info = null)
        {
            if (InternalFile == null)
                return null;
            info = info ?? new ProcessStartInfo();
            info.FileName = FullName;
            return Process.Start(info);
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
        /// <param name="directory">Directory to move to</param>
        public void MoveTo(IDirectory directory)
        {
            if (InternalFile == null || directory == null)
                return;
            InternalFile.MoveTo(directory);
        }

        /// <summary>
        /// Reads the file in as a string
        /// </summary>
        /// <returns>The file contents as a string</returns>
        public string Read()
        {
            if (InternalFile == null)
                return "";
            return InternalFile.Read();
        }

        /// <summary>
        /// Reads a file as binary
        /// </summary>
        /// <returns>The file contents as a byte array</returns>
        public byte[] ReadBinary()
        {
            if (InternalFile == null)
                return new byte[0];
            return InternalFile.ReadBinary();
        }

        /// <summary>
        /// Renames the file
        /// </summary>
        /// <param name="newName">New name for the file</param>
        public void Rename(string newName)
        {
            if (InternalFile == null || string.IsNullOrEmpty(newName))
                return;
            InternalFile.Rename(newName);
        }

        /// <summary>
        /// Converts the file to the specified file format
        /// </summary>
        /// <typeparam name="T">File format</typeparam>
        /// <returns>The file as the file format object</returns>
        public T To<T>()
            where T : FormatBase<T, string>, new()
        {
            return FormatBase<T, string>.Load(FullName);
        }

        /// <summary>
        /// Converts the file to the specified file format
        /// </summary>
        /// <typeparam name="T">File format</typeparam>
        /// <typeparam name="R">Record type</typeparam>
        /// <returns>The file as the file format object</returns>
        public T To<T, R>()
            where T : StringListFormatBase<T, R>, new()
        {
            return FormatBase<T, string>.Load(FullName);
        }

        /// <summary>
        /// Converts the file to the specified file format
        /// </summary>
        /// <returns>The file as the file format object</returns>
        public Excel To()
        {
            return Excel.Load(FullName);
        }

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
        /// <param name="content">Content to write</param>
        /// <param name="mode">Mode to open the file as</param>
        /// <param name="encoding">Encoding to use for the content</param>
        /// <returns>The result of the write or original content</returns>
        public string Write(string content, FileMode mode = FileMode.Create, Encoding encoding = null)
        {
            if (InternalFile == null)
                return content;
            return InternalFile.Write(content, mode, encoding);
        }

        /// <summary>
        /// Writes content to the file
        /// </summary>
        /// <param name="content">Content to write</param>
        /// <param name="mode">Mode to open the file as</param>
        /// <returns>The result of the write or original content</returns>
        public byte[] Write(byte[] content, FileMode mode = FileMode.Create)
        {
            if (InternalFile == null)
                return content;
            return InternalFile.Write(content, mode);
        }
    }
}