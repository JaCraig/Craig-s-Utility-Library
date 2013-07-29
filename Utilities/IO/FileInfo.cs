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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilities.IO.FileSystem;
using Utilities.IO.FileSystem.Interfaces;
using Utilities.IoC.Default;
using Utilities.IoC.Interfaces;
#endregion

namespace Utilities.IO
{
    /// <summary>
    /// File info class
    /// </summary>
    public class FileInfo : IFile
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the file</param>
        public FileInfo(string Path)
        {
            this.InternalFile = IoC.Manager.Bootstrapper.Resolve<Manager>().File(Path);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InternalFile">Internal file</param>
        public FileInfo(IFile InternalFile)
        {
            this.InternalFile = InternalFile;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Internal directory
        /// </summary>
        protected IFile InternalFile { get; private set; }

        /// <summary>
        /// Last time accessed (UTC time)
        /// </summary>
        public DateTime Accessed { get { return InternalFile == null ? DateTime.Now : InternalFile.Accessed; } }

        /// <summary>
        /// Time created (UTC time)
        /// </summary>
        public DateTime Created { get { return InternalFile == null ? DateTime.Now : InternalFile.Created; } }

        /// <summary>
        /// Time modified (UTC time)
        /// </summary>
        public DateTime Modified { get { return InternalFile == null ? DateTime.Now : InternalFile.Modified; } }

        /// <summary>
        /// Directory the file is within
        /// </summary>
        public IDirectory Directory { get { return InternalFile == null ? null : InternalFile.Directory; } }

        /// <summary>
        /// Does the file exist?
        /// </summary>
        public bool Exists { get { return InternalFile == null ? false : InternalFile.Exists; } }

        /// <summary>
        /// File extension
        /// </summary>
        public string Extension { get { return InternalFile == null ? "" : InternalFile.Extension; } }

        /// <summary>
        /// Full path
        /// </summary>
        public string FullName { get { return InternalFile == null ? "" : InternalFile.FullName; } }

        /// <summary>
        /// Size of the file
        /// </summary>
        public long Length { get { return InternalFile == null ? 0 : InternalFile.Length; } }

        /// <summary>
        /// Name of the file
        /// </summary>
        public string Name { get { return InternalFile == null ? "" : InternalFile.Name; } }

        #endregion

        #region Functions

        /// <summary>
        /// Deletes the file
        /// </summary>
        public void Delete()
        {
            if (InternalFile == null)
                return;
            InternalFile.Delete();
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
        /// <param name="NewName">New name for the file</param>
        public void Rename(string NewName)
        {
            if (InternalFile == null || string.IsNullOrEmpty(NewName))
                return;
            InternalFile.Rename(NewName);
        }

        /// <summary>
        /// Moves the file to a new directory
        /// </summary>
        /// <param name="Directory">Directory to move to</param>
        public void MoveTo(IDirectory Directory)
        {
            if (InternalFile == null || Directory == null)
                return;
            InternalFile.MoveTo(Directory);
        }

        /// <summary>
        /// Writes content to the file
        /// </summary>
        /// <param name="Content">Content to write</param>
        /// <param name="Mode">Mode to open the file as</param>
        /// <param name="Encoding">Encoding to use for the content</param>
        /// <returns>Task associated with the write process</returns>
        public void Write(string Content, System.IO.FileMode Mode = FileMode.Create, Encoding Encoding = null)
        {
            if (InternalFile == null)
                return;
            InternalFile.Write(Content, Mode, Encoding);
        }

        /// <summary>
        /// Writes content to the file
        /// </summary>
        /// <param name="Content">Content to write</param>
        /// <param name="Mode">Mode to open the file as</param>
        /// <returns>Task associated with the write process</returns>
        public void Write(byte[] Content, System.IO.FileMode Mode = FileMode.Create)
        {
            if (InternalFile == null)
                return;
            InternalFile.Write(Content, Mode);
        }

        /// <summary>
        /// Executes the file
        /// </summary>
        /// <param name="Info">Info used to execute the file</param>
        /// <returns>The process object created when the executable is started</returns>
        public Process Execute(ProcessStartInfo Info = null)
        {
            if (InternalFile == null)
                return null;
            Info = Info == null ? new ProcessStartInfo() : Info;
            Info.FileName = FullName;
            return Process.Start(Info);
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
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            FileInfo File = obj as FileInfo;
            return File != null && File == this;
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
        /// Compares this to another file
        /// </summary>
        /// <param name="other">File to compare to</param>
        /// <returns>-1 if this is smaller, 0 if they are the same, 1 if it is larger</returns>
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
        /// <returns>-1 if this is smaller, 0 if they are the same, 1 if it is larger</returns>
        public int CompareTo(object obj)
        {
            FileInfo Temp = obj as FileInfo;
            if (Temp == null)
                return 1;
            return CompareTo(Temp);
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
        /// Clones the file object
        /// </summary>
        /// <returns>The cloned object</returns>
        public object Clone()
        {
            FileInfo Temp = new FileInfo(InternalFile);
            return Temp;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines if two directories are equal
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(FileInfo File1, FileInfo File2)
        {
            if ((object)File1 == null && (object)File2 == null)
                return true;
            if ((object)File1 == null || (object)File2 == null)
                return false;
            return File1.FullName == File2.FullName;
        }

        /// <summary>
        /// Determines if two directories are not equal
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(FileInfo File1, FileInfo File2)
        {
            return !(File1 == File2);
        }

        /// <summary>
        /// Less than
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator <(FileInfo File1, FileInfo File2)
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
        public static bool operator <=(FileInfo File1, FileInfo File2)
        {
            if (File1 == null || File2 == null)
                return false;
            return string.Compare(File1.FullName, File2.FullName, StringComparison.OrdinalIgnoreCase) <= 0;
        }

        /// <summary>
        /// Greater than
        /// </summary>
        /// <param name="File1">File 1</param>
        /// <param name="File2">File 2</param>
        /// <returns>The result</returns>
        public static bool operator >(FileInfo File1, FileInfo File2)
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
        public static bool operator >=(FileInfo File1, FileInfo File2)
        {
            if (File1 == null || File2 == null)
                return false;
            return string.Compare(File1.FullName, File2.FullName, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Reads the file and converts it to a string
        /// </summary>
        /// <param name="File">File to read</param>
        /// <returns>The file as a string</returns>
        public static implicit operator string(FileInfo File)
        {
            if (File == null)
                return "";
            return File.Read();
        }

        /// <summary>
        /// Reads the file and converts it to a byte array
        /// </summary>
        /// <param name="File">File to read</param>
        /// <returns>The file as a byte array</returns>
        public static implicit operator byte[](FileInfo File)
        {
            if (File == null)
                return new byte[0];
            return File.ReadBinary();
        }

        #endregion
    }
}
