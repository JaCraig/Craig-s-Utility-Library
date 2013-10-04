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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Utilities.IO.FileSystem.BaseClasses;
using Utilities.IO.FileSystem.Interfaces;
using Utilities.DataTypes;
#endregion

namespace Utilities.IO.FileSystem.Default
{
    /// <summary>
    /// Basic ftp file class
    /// </summary>
    public class FtpFile : FileBase<Uri, FtpFile>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public FtpFile()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the file</param>
        public FtpFile(string Path)
            : this(string.IsNullOrEmpty(Path) ? null : new Uri(Path))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="File">File to use</param>
        public FtpFile(Uri File)
            : base(File)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Time accessed (Just returns now)
        /// </summary>
        public override DateTime Accessed
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Time created (Just returns now)
        /// </summary>
        public override DateTime Created
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Time modified (just returns now)
        /// </summary>
        public override DateTime Modified
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Directory base path
        /// </summary>
        public override IDirectory Directory
        {
            get { return InternalFile == null ? null : new FtpDirectory((string)InternalFile.AbsolutePath.Take(InternalFile.AbsolutePath.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) - 1)); }
        }

        /// <summary>
        /// Does it exist? Always true.
        /// </summary>
        public override bool Exists
        {
            get { return true; }
        }

        /// <summary>
        /// Extension (always empty)
        /// </summary>
        public override string Extension
        {
            get { return ""; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override string FullName
        {
            get { return InternalFile == null ? "" : InternalFile.AbsolutePath; }
        }

        /// <summary>
        /// Size of the file (always 0)
        /// </summary>
        public override long Length
        {
            get { return 0; }
        }

        /// <summary>
        /// Absolute path of the file (same as FullName)
        /// </summary>
        public override string Name
        {
            get { return InternalFile == null ? "" : InternalFile.AbsolutePath; }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Delete (does nothing)
        /// </summary>
        /// <returns>Any response for deleting the resource (usually FTP, HTTP, etc)</returns>
        public override string Delete()
        {
            FtpWebRequest Request = WebRequest.Create(InternalFile) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.DeleteFile;
            SetupData(Request, null);
            SetupCredentials(Request);
            return SendRequest(Request);
        }

        /// <summary>
        /// Reads the web page
        /// </summary>
        /// <returns>The content as a string</returns>
        public override string Read()
        {
            FtpWebRequest Request = WebRequest.Create(InternalFile) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.DownloadFile;
            SetupData(Request, null);
            SetupCredentials(Request);
            return SendRequest(Request);
        }

        /// <summary>
        /// Reads the web page
        /// </summary>
        /// <returns>The content as a byte array</returns>
        public override byte[] ReadBinary()
        {
            return Read().ToByteArray();
        }

        /// <summary>
        /// Renames the file (not used)
        /// </summary>
        /// <param name="NewName">Not used</param>
        public override void Rename(string NewName)
        {
            FtpWebRequest Request = WebRequest.Create(InternalFile) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.Rename;
            Request.RenameTo = NewName;
            SetupData(Request, null);
            SetupCredentials(Request);
            SendRequest(Request);
            InternalFile = new Uri(Directory.FullName + "/" + NewName);
        }

        /// <summary>
        /// Moves the file (not used)
        /// </summary>
        /// <param name="Directory">Not used</param>
        public override void MoveTo(IDirectory Directory)
        {
            new FileInfo(Directory.FullName + "\\" + Name.Right(Name.Length - (Name.LastIndexOf("/") + 1))).Write(ReadBinary());
            Delete();
        }

        /// <summary>
        /// Copies the file to another directory
        /// </summary>
        /// <param name="Directory">Directory to copy the file to</param>
        /// <param name="Overwrite">Should the file overwrite another file if found</param>
        /// <returns>The newly created file</returns>
        public override IFile CopyTo(IDirectory Directory, bool Overwrite)
        {
            FileInfo File = new FileInfo(Directory.FullName + "\\" + Name.Right(Name.Length - (Name.LastIndexOf("/") + 1)));
            if (!File.Exists || Overwrite)
            {
                File.Write(ReadBinary());
                return File;
            }
            return this;
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Content">Not used</param>
        /// <param name="Mode">Not used</param>
        /// <param name="Encoding">Not used</param>
        /// <returns>The result of the write or original content</returns>
        public override string Write(string Content, System.IO.FileMode Mode = FileMode.Create, Encoding Encoding = null)
        {
            return Write(Content.ToByteArray(Encoding), Mode).ToString(Encoding.UTF8);
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Content">Not used</param>
        /// <param name="Mode">Not used</param>
        /// <returns>The result of the write or original content</returns>
        public override byte[] Write(byte[] Content, System.IO.FileMode Mode = FileMode.Create)
        {
            FtpWebRequest Request = WebRequest.Create(InternalFile) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.UploadFile;
            SetupData(Request, Content);
            SetupCredentials(Request);
            return SendRequest(Request).ToByteArray();
        }


        /// <summary>
        /// Sets up any data that needs to be sent
        /// </summary>
        /// <param name="Request">The web request object</param>
        /// <param name="Data">Data to send with the request</param>
        private void SetupData(FtpWebRequest Request, byte[] Data)
        {
            Request.UsePassive = true;
            Request.KeepAlive = false;
            Request.UseBinary = true;
            Request.EnableSsl = Name.ToLowerInvariant().StartsWith("ftps");
            if (Data==null)
            {
                Request.ContentLength = 0;
                return;
            }
            Request.ContentLength = Data.Length;
            using (Stream RequestStream = Request.GetRequestStream())
            {
                RequestStream.Write(Data, 0, Data.Length);
            }
        }

        /// <summary>
        /// Sets up any credentials (basic authentication,
        /// for OAuth, please use the OAuth class to create the
        /// URL)
        /// </summary>
        /// <param name="Request">The web request object</param>
        private void SetupCredentials(FtpWebRequest Request)
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                Request.Credentials = new NetworkCredential(UserName, Password);
            }
        }

        /// <summary>
        /// Sends the request to the URL specified
        /// </summary>
        /// <param name="Request">The web request object</param>
        /// <returns>The string returned by the service</returns>
        private static string SendRequest(FtpWebRequest Request)
        {
            using (FtpWebResponse Response = Request.GetResponse() as FtpWebResponse)
            {
                using (StreamReader Reader = new StreamReader(Response.GetResponseStream()))
                {
                    return Reader.ReadToEnd();
                }
            }
        }

        #endregion
    }
}