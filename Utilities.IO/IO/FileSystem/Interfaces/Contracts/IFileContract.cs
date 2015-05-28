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
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace Utilities.IO.FileSystem.Interfaces.Contracts
{
    /// <summary>
    /// IFile Contract
    /// </summary>
    [ContractClassFor(typeof(IFile))]
    internal abstract class IFileContract : IFile
    {
        /// <summary>
        /// Last time the file was accessed
        /// </summary>
        public DateTime Accessed
        {
            get { return default(DateTime); }
        }

        /// <summary>
        /// When the file was created
        /// </summary>
        public DateTime Created
        {
            get { return default(DateTime); }
        }

        /// <summary>
        /// Directory the file is in
        /// </summary>
        public IDirectory Directory
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Does the file exist currently
        /// </summary>
        public bool Exists
        {
            get { return false; }
        }

        /// <summary>
        /// File extension
        /// </summary>
        public string Extension
        {
            get { return ""; }
        }

        /// <summary>
        /// Full path to the file
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
        /// Size of the file in bytes
        /// </summary>
        public long Length
        {
            get { return 0; }
        }

        /// <summary>
        /// When the file was last modified
        /// </summary>
        public DateTime Modified
        {
            get { return default(DateTime); }
        }

        /// <summary>
        /// File name
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
        public int CompareTo(IFile other)
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
        /// Copies the file to another directory
        /// </summary>
        /// <param name="Directory">Directory to copy the file to</param>
        /// <param name="Overwrite">Should the file overwrite another file if found</param>
        /// <returns>The newly created file</returns>
        public IFile CopyTo(IDirectory Directory, bool Overwrite)
        {
            return null;
        }

        /// <summary>
        /// Deletes the file
        /// </summary>
        /// <returns>Any response for deleting the resource (usually FTP, HTTP, etc)</returns>
        public string Delete()
        {
            return "";
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter;
        /// otherwise, false.
        /// </returns>
        public bool Equals(IFile other)
        {
            return false;
        }

        /// <summary>
        /// Moves the file to another directory
        /// </summary>
        /// <param name="Directory">Directory to move the file to</param>
        public void MoveTo(IDirectory Directory)
        {
        }

        /// <summary>
        /// Reads the file to the end as a string
        /// </summary>
        /// <returns>A string containing the contents of the file</returns>
        public string Read()
        {
            return "";
        }

        /// <summary>
        /// Reads the file to the end as a byte array
        /// </summary>
        /// <returns>A byte array containing the contents of the file</returns>
        public byte[] ReadBinary()
        {
            return new byte[0];
        }

        /// <summary>
        /// Renames the file
        /// </summary>
        /// <param name="NewName">New file name</param>
        public void Rename(string NewName)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(NewName));
        }

        /// <summary>
        /// Writes content to the file
        /// </summary>
        /// <param name="Content">Content to write</param>
        /// <param name="Mode">File mode</param>
        /// <param name="Encoding">Encoding that the content should be saved as (default is UTF8)</param>
        /// <returns>The result of the write or original content</returns>
        public string Write(string Content, System.IO.FileMode Mode = FileMode.Create, Encoding Encoding = null)
        {
            return "";
        }

        /// <summary>
        /// Writes content to the file
        /// </summary>
        /// <param name="Content">Content to write</param>
        /// <param name="Mode">File mode</param>
        /// <returns>The result of the write or original content</returns>
        public byte[] Write(byte[] Content, System.IO.FileMode Mode = FileMode.Create)
        {
            return new byte[0];
        }
    }
}