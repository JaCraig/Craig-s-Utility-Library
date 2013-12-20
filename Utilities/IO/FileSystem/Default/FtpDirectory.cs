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
using System.Net;
using Utilities.IO.Enums;
using Utilities.IO.FileSystem.BaseClasses;
using Utilities.IO.FileSystem.Interfaces;
using Utilities.DataTypes;
using System.Diagnostics.Contracts;
#endregion

namespace Utilities.IO.FileSystem.Default
{
    /// <summary>
    /// Directory class
    /// </summary>
    public class FtpDirectory : DirectoryBase<Uri, FtpDirectory>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public FtpDirectory()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the directory</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <param name="Password">Password to be used to access the directory (optional)</param>
        /// <param name="UserName">User name to be used to access the directory (optional)</param>
        public FtpDirectory(string Path, string UserName = "", string Password = "", string Domain = "")
            : this(string.IsNullOrEmpty(Path) ? null : new Uri(Path), UserName, Password, Domain)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Directory">Internal directory</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <param name="Password">Password to be used to access the directory (optional)</param>
        /// <param name="UserName">User name to be used to access the directory (optional)</param>
        public FtpDirectory(Uri Directory, string UserName = "", string Password = "", string Domain = "")
            : base(Directory, UserName, Password, Domain)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// returns now
        /// </summary>
        public override DateTime Accessed
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// returns now
        /// </summary>
        public override DateTime Created
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// returns now
        /// </summary>
        public override DateTime Modified
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// returns true
        /// </summary>
        public override bool Exists
        {
            get { return true; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override string FullName
        {
            get { return InternalDirectory == null ? "" : InternalDirectory.AbsolutePath; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override string Name
        {
            get { return InternalDirectory == null ? "" : InternalDirectory.AbsolutePath; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override IDirectory Parent
        {
            get { return InternalDirectory == null ? null : new FtpDirectory((string)InternalDirectory.AbsolutePath.Take(InternalDirectory.AbsolutePath.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) - 1),UserName,Password,Domain); }
        }

        /// <summary>
        /// Root
        /// </summary>
        public override IDirectory Root
        {
            get { return InternalDirectory == null ? null : new FtpDirectory(InternalDirectory.Scheme + "://" + InternalDirectory.Host, UserName, Password, Domain); }
        }

        /// <summary>
        /// Size (returns 0)
        /// </summary>
        public override long Size
        {
            get { return 0; }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Not used
        /// </summary>
        public override void Create()
        {
            FtpWebRequest Request = WebRequest.Create(InternalDirectory) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.MakeDirectory;
            SetupData(Request, null);
            SetupCredentials(Request);
            SendRequest(Request);
        }

        /// <summary>
        /// Not used
        /// </summary>
        public override void Delete()
        {
            FtpWebRequest Request = WebRequest.Create(InternalDirectory) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.RemoveDirectory;
            SetupData(Request, null);
            SetupCredentials(Request);
            SendRequest(Request);
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="SearchPattern"></param>
        /// <param name="Options"></param>
        /// <returns></returns>
        public override IEnumerable<IDirectory> EnumerateDirectories(string SearchPattern, SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            FtpWebRequest Request = WebRequest.Create(InternalDirectory) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.ListDirectory;
            SetupData(Request, null);
            SetupCredentials(Request);
            string Data = SendRequest(Request);
            string[] Folders = Data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            SetupData(Request, null);
            SetupCredentials(Request);
            Data = SendRequest(Request);
            string[] DetailedFolders = Data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            List<IDirectory> Directories = new List<IDirectory>();
            foreach (string Folder in Folders)
            {
                string DetailedFolder = DetailedFolders.FirstOrDefault(x => x.EndsWith(Folder, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(DetailedFolder))
                {
                    if (DetailedFolder.StartsWith("d", StringComparison.OrdinalIgnoreCase) && !DetailedFolder.EndsWith(".", StringComparison.OrdinalIgnoreCase))
                    {
                        Directories.Add(new DirectoryInfo(FullName + "/" + Folder, UserName, Password, Domain));
                    }
                }
            }
            return Directories;
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="SearchPattern"></param>
        /// <param name="Options"></param>
        /// <returns></returns>
        public override IEnumerable<IFile> EnumerateFiles(string SearchPattern = "*", SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            FtpWebRequest Request = WebRequest.Create(InternalDirectory) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.ListDirectory;
            SetupData(Request, null);
            SetupCredentials(Request);
            string Data = SendRequest(Request);
            string[] Folders = Data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            SetupData(Request, null);
            SetupCredentials(Request);
            Data = SendRequest(Request);
            string[] DetailedFolders = Data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            List<IFile> Directories = new List<IFile>();
            foreach (string Folder in Folders)
            {
                string DetailedFolder = DetailedFolders.FirstOrDefault(x => x.EndsWith(Folder, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(DetailedFolder))
                {
                    if (!DetailedFolder.StartsWith("d", StringComparison.OrdinalIgnoreCase))
                    {
                        Directories.Add(new FileInfo(FullName + "/" + Folder, UserName, Password, Domain));
                    }
                }
            }
            return Directories;
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Directory"></param>
        public override void MoveTo(IDirectory Directory)
        {
            DirectoryInfo NewDirectory = new DirectoryInfo(Directory.FullName + "\\" + Name.Right(Name.Length - (Name.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1)), UserName, Password, Domain);
            NewDirectory.Create();
            foreach (DirectoryInfo Temp in EnumerateDirectories("*"))
            {
                Temp.MoveTo(NewDirectory);
            }
            foreach (FileInfo Temp in EnumerateFiles("*"))
            {
                Temp.MoveTo(NewDirectory);
            }
            Delete();
        }

        /// <summary>
        /// Copies the directory to the specified parent directory
        /// </summary>
        /// <param name="Directory">Directory to copy to</param>
        /// <param name="Options">Options</param>
        /// <returns>Newly created directory</returns>
        public override IDirectory CopyTo(IDirectory Directory, CopyOptions Options = CopyOptions.CopyAlways)
        {
            DirectoryInfo NewDirectory = new DirectoryInfo(Directory.FullName + "\\" + Name.Right(Name.Length - (Name.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1)), UserName, Password, Domain);
            NewDirectory.Create();
            foreach (DirectoryInfo Temp in EnumerateDirectories("*"))
            {
                Temp.CopyTo(NewDirectory);
            }
            foreach (FileInfo Temp in EnumerateFiles("*"))
            {
                Temp.CopyTo(NewDirectory, true);
            }
            return NewDirectory;
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Name"></param>
        public override void Rename(string Name)
        {
            FtpWebRequest Request = WebRequest.Create(InternalDirectory) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.Rename;
            Request.RenameTo = Name;
            SetupData(Request, null);
            SetupCredentials(Request);
            SendRequest(Request);
            InternalDirectory = new Uri(FullName + "/" + Name);
        }

        /// <summary>
        /// Sets up any data that needs to be sent
        /// </summary>
        /// <param name="Request">The web request object</param>
        /// <param name="Data">Data to send with the request</param>
        private void SetupData(FtpWebRequest Request, byte[] Data)
        {
            Contract.Requires<ArgumentNullException>(Request != null, "Request");
            Request.UsePassive = true;
            Request.KeepAlive = false;
            Request.UseBinary = true;
            Request.EnableSsl = Name.ToUpperInvariant().StartsWith("FTPS", StringComparison.OrdinalIgnoreCase);
            if (Data == null)
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
            Contract.Requires<ArgumentNullException>(Request != null, "Request");
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