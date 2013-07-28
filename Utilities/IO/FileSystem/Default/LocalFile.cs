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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilities.IO.FileSystem.BaseClasses;
using Utilities.IO.FileSystem.Interfaces;
using Utilities.IoC.Default;
using Utilities.IoC.Interfaces;
#endregion

namespace Utilities.IO.FileSystem.Default
{
    /// <summary>
    /// Basic local file class
    /// </summary>
    public class LocalFile : FileBase<System.IO.FileInfo, LocalFile>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public LocalFile()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the file</param>
        public LocalFile(string Path)
            : base(new System.IO.FileInfo(Path))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="File">File to use</param>
        public LocalFile(System.IO.FileInfo File)
            : base(File)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Last time accessed (UTC time)
        /// </summary>
        public override DateTime Accessed
        {
            get { return InternalFile.LastAccessTimeUtc; }
        }

        /// <summary>
        /// Time created (UTC time)
        /// </summary>
        public override DateTime Created
        {
            get { return InternalFile.CreationTimeUtc; }
        }

        /// <summary>
        /// Time modified (UTC time)
        /// </summary>
        public override DateTime Modified
        {
            get { return InternalFile.LastWriteTimeUtc; }
        }

        /// <summary>
        /// Directory the file is within
        /// </summary>
        public override IDirectory Directory
        {
            get { return new LocalDirectory(InternalFile.Directory); }
        }

        /// <summary>
        /// Does the file exist?
        /// </summary>
        public override bool Exists
        {
            get { return InternalFile.Exists; }
        }

        /// <summary>
        /// File extension
        /// </summary>
        public override string Extension
        {
            get { return InternalFile.Extension; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override string FullName
        {
            get { return InternalFile.FullName; }
        }

        /// <summary>
        /// Size of the file
        /// </summary>
        public override long Length
        {
            get { return InternalFile.Exists ? InternalFile.Length : 0; }
        }

        /// <summary>
        /// Name of the file
        /// </summary>
        public override string Name
        {
            get { return InternalFile.Name; }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Deletes the file
        /// </summary>
        public override async Task Delete()
        {
            if (!Exists)
                return;
            await Task.Run(() =>
            {
                InternalFile.Delete();
                InternalFile.Refresh();
            });
        }

        /// <summary>
        /// Reads the file in as a string
        /// </summary>
        /// <returns>The file contents as a string</returns>
        public override string Read()
        {
            if (!InternalFile.Exists)
                return "";
            using (StreamReader Reader = InternalFile.OpenText())
            {
                return Reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Reads a file as binary
        /// </summary>
        /// <returns>The file contents as a byte array</returns>
        public override byte[] ReadBinary()
        {
            if (!InternalFile.Exists)
                return new byte[0];
            using (FileStream Reader = InternalFile.OpenRead())
            {
                byte[] Buffer = new byte[1024];
                using (MemoryStream Temp = new MemoryStream())
                {
                    while (true)
                    {
                        int Count = Reader.Read(Buffer, 0, Buffer.Length);
                        if (Count <= 0)
                        {
                            return Temp.ToArray();
                        }
                        Temp.Write(Buffer, 0, Count);
                    }
                }
            }
        }

        /// <summary>
        /// Renames the file
        /// </summary>
        /// <param name="NewName">New name for the file</param>
        public override async Task Rename(string NewName)
        {
            await Task.Run(() =>
            {
                InternalFile.MoveTo(InternalFile.DirectoryName + "\\" + NewName);
                InternalFile = new System.IO.FileInfo(InternalFile.DirectoryName + "\\" + NewName);
            });
        }

        /// <summary>
        /// Moves the file to a new directory
        /// </summary>
        /// <param name="Directory">Directory to move to</param>
        public override async Task MoveTo(IDirectory Directory)
        {
            await Task.Run(() =>
            {
                InternalFile.MoveTo(Directory.FullName + "\\" + Name);
                InternalFile = new System.IO.FileInfo(Directory.FullName + "\\" + Name);
            });
        }

        /// <summary>
        /// Writes content to the file
        /// </summary>
        /// <param name="Content">Content to write</param>
        /// <param name="Mode">Mode to open the file as</param>
        /// <param name="Encoding">Encoding to use for the content</param>
        public override Task Write(string Content, System.IO.FileMode Mode = FileMode.Create, Encoding Encoding = null)
        {
            if (Encoding == null)
                Encoding = new ASCIIEncoding();
            return Write(Encoding.GetBytes(Content), Mode);
        }

        /// <summary>
        /// Writes content to the file
        /// </summary>
        /// <param name="Content">Content to write</param>
        /// <param name="Mode">Mode to open the file as</param>
        public override async Task Write(byte[] Content, System.IO.FileMode Mode = FileMode.Create)
        {
            await Directory.Create();
            using (FileStream Writer = InternalFile.Open(Mode, FileAccess.Write))
            {
                await Writer.WriteAsync(Content, 0, Content.Length);
            }
            InternalFile.Refresh();
        }

        #endregion
    }
}