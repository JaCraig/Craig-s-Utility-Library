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
using System.Linq;
using System.Net;
using System.Text;
using Utilities.DataTypes;
using Utilities.IO.FileSystem.BaseClasses;
using Utilities.IO.FileSystem.Interfaces;

namespace Utilities.IO.FileSystem.Default
{
    /// <summary>
    /// Basic web file class
    /// </summary>
    public class WebFile : FileBase<Uri, WebFile>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WebFile()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the file</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <param name="Password">Password to be used to access the directory (optional)</param>
        /// <param name="UserName">User name to be used to access the directory (optional)</param>
        public WebFile(string Path, string UserName = "", string Password = "", string Domain = "")
            : this(string.IsNullOrEmpty(Path) ? null : new Uri(Path), UserName, Password, Domain)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="File">File to use</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <param name="Password">Password to be used to access the file (optional)</param>
        /// <param name="UserName">User name to be used to access the file (optional)</param>
        public WebFile(Uri File, string UserName = "", string Password = "", string Domain = "")
            : base(File, UserName, Password, Domain)
        {
        }

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
        /// Directory base path
        /// </summary>
        public override IDirectory Directory
        {
            get { return InternalFile == null ? null : new WebDirectory((string)InternalFile.AbsolutePath.Take(InternalFile.AbsolutePath.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) - 1), UserName, Password, Domain); }
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
        /// Time modified (just returns now)
        /// </summary>
        public override DateTime Modified
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Absolute path of the file (same as FullName)
        /// </summary>
        public override string Name
        {
            get { return InternalFile == null ? "" : InternalFile.AbsolutePath; }
        }

        /// <summary>
        /// Copies the file to another directory
        /// </summary>
        /// <param name="Directory">Directory to copy the file to</param>
        /// <param name="Overwrite">Should the file overwrite another file if found</param>
        /// <returns>The newly created file</returns>
        public override IFile CopyTo(IDirectory Directory, bool Overwrite)
        {
            if (Directory == null)
                return this;
            var File = new FileInfo(Directory.FullName + "\\" + Name.Right(Name.Length - (Name.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1)), UserName, Password, Domain);
            if (!File.Exists || Overwrite)
            {
                File.Write(ReadBinary());
                return File;
            }
            return this;
        }

        /// <summary>
        /// Delete (does nothing)
        /// </summary>
        /// <returns>Any response for deleting the resource (usually FTP, HTTP, etc)</returns>
        public override string Delete()
        {
            if (InternalFile == null)
                return "";
            var Request = WebRequest.Create(InternalFile) as HttpWebRequest;
            Request.Method = "DELETE";
            Request.ContentType = "text/xml";
            SetupData(Request, "");
            SetupCredentials(Request);
            return SendRequest(Request);
        }

        /// <summary>
        /// Moves the file (not used)
        /// </summary>
        /// <param name="Directory">Not used</param>
        public override void MoveTo(IDirectory Directory)
        {
            if (Directory == null || !Exists)
                return;
            new FileInfo(Directory.FullName + "\\" + Name.Right(Name.Length - (Name.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1)), UserName, Password, Domain).Write(ReadBinary());
            Delete();
        }

        /// <summary>
        /// Reads the web page
        /// </summary>
        /// <returns>The content as a string</returns>
        public override string Read()
        {
            if (InternalFile == null)
                return "";
            var Request = WebRequest.Create(InternalFile) as HttpWebRequest;
            Request.Method = "GET";
            Request.ContentType = "text/xml";
            SetupData(Request, "");
            SetupCredentials(Request);
            return SendRequest(Request);
        }

        /// <summary>
        /// Reads the web page
        /// </summary>
        /// <returns>The content as a byte array</returns>
        public override byte[] ReadBinary()
        {
            if (InternalFile == null)
                return new byte[0];
            return Read().ToByteArray();
        }

        /// <summary>
        /// Renames the file (not used)
        /// </summary>
        /// <param name="NewName">Not used</param>
        public override void Rename(string NewName)
        {
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
            var Request = WebRequest.Create(InternalFile) as HttpWebRequest;
            if (Request == null)
                return "";
            if (Mode.HasFlag(FileMode.Append) || Mode.HasFlag(FileMode.Open))
                Request.Method = "PUT";
            else if (Mode.HasFlag(FileMode.Create) || Mode.HasFlag(FileMode.CreateNew))
                Request.Method = "POST";
            Request.ContentType = "text/xml";
            SetupData(Request, Content);
            SetupCredentials(Request);
            return SendRequest(Request);
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Content">Not used</param>
        /// <param name="Mode">Not used</param>
        /// <returns>The result of the write or original content</returns>
        public override byte[] Write(byte[] Content, System.IO.FileMode Mode = FileMode.Create)
        {
            return Write(Content.ToString(Encoding.UTF8), Mode).ToByteArray();
        }

        /// <summary>
        /// Sends the request to the URL specified
        /// </summary>
        /// <param name="Request">The web request object</param>
        /// <returns>The string returned by the service</returns>
        private static string SendRequest(HttpWebRequest Request)
        {
            Contract.Requires<ArgumentNullException>(Request != null, "Request");
            using (HttpWebResponse Response = Request.GetResponse() as HttpWebResponse)
            {
                if (Response.StatusCode != HttpStatusCode.OK)
                    return "";
                using (StreamReader Reader = new StreamReader(Response.GetResponseStream()))
                {
                    return Reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Sets up any data that needs to be sent
        /// </summary>
        /// <param name="Request">The web request object</param>
        /// <param name="Data">Data to send with the request</param>
        private static void SetupData(HttpWebRequest Request, string Data)
        {
            Contract.Requires<ArgumentNullException>(Request != null, "Request");
            if (string.IsNullOrEmpty(Data))
            {
                Request.ContentLength = 0;
                return;
            }
            var ByteData = Data.ToByteArray();
            Request.ContentLength = ByteData.Length;
            using (Stream RequestStream = Request.GetRequestStream())
            {
                RequestStream.Write(ByteData, 0, ByteData.Length);
            }
        }

        /// <summary>
        /// Sets up any credentials (basic authentication, for OAuth, please use the OAuth class to
        /// create the URL)
        /// </summary>
        /// <param name="Request">The web request object</param>
        private void SetupCredentials(HttpWebRequest Request)
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                Request.Credentials = new NetworkCredential(UserName, Password);
            }
        }
    }
}