/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Net;
using System.Text;

#endregion

namespace Utilities.IO
{
    /// <summary>
    /// Utility class for managing files
    /// </summary>
    public static class FileManager
    {
        #region Public Static Functions

        #region CompareFiles

        /// <summary>
        /// Compares 2 files and determines if they are the same or not
        /// </summary>
        /// <param name="FileName1">name of the first file</param>
        /// <param name="FileName2">name of the second file</param>
        /// <returns>False if they are different, otherwise it returns true.</returns>
        public static bool CompareFiles(string FileName1, string FileName2)
        {
            if (string.IsNullOrEmpty(FileName1))
                throw new ArgumentNullException("FileName1");
            if (string.IsNullOrEmpty(FileName2))
                throw new ArgumentNullException("FileName2");
            if (!FileExists(FileName1))
                throw new ArgumentException("FileName1 does not exist");
            if (!FileExists(FileName2))
                throw new ArgumentException("FileName2 does not exist");
            FileInfo File1 = new FileInfo(FileName1);
            FileInfo File2 = new FileInfo(FileName2);
            if (File1.Length != File2.Length)
            {
                return false;
            }
            string File1Contents = FileManager.GetFileContents(FileName1);
            string File2Contents = FileManager.GetFileContents(FileName2);
            if (!File1Contents.Equals(File2Contents))
                return false;
            return true;
        }

        #endregion

        #region CopyDirectory

        /// <summary>
        /// Copies a directory to a new location
        /// </summary>
        /// <param name="Source">Source directory</param>
        /// <param name="Destination">Destination to move the directory to</param>
        /// <param name="Recursive">If true it will go through all sub directories, otherwise it wont</param>
        /// <param name="Options">Copy options, can be set to copy if newer, always copy, or do not overwrite</param>
        public static void CopyDirectory(string Source, string Destination, bool Recursive = true, CopyOptions Options = CopyOptions.CopyAlways)
        {
            if (string.IsNullOrEmpty(Source))
                throw new ArgumentNullException("Source");
            if (string.IsNullOrEmpty(Destination))
                throw new ArgumentNullException("Destination");
            if (!DirectoryExists(Source))
                throw new ArgumentException("Source directory does not exist");
            DirectoryInfo SourceInfo = new DirectoryInfo(Source);
            DirectoryInfo DestinationInfo = new DirectoryInfo(Destination);
            CreateDirectory(Destination);
            List<FileInfo> Files = FileList(Source);
            foreach (FileInfo File in Files)
            {
                if (Options == CopyOptions.CopyAlways)
                {
                    File.CopyTo(Path.Combine(DestinationInfo.FullName, File.Name), true);
                }
                else if (Options == CopyOptions.CopyIfNewer)
                {
                    if (FileExists(Path.Combine(DestinationInfo.FullName, File.Name)))
                    {
                        FileInfo FileInfo = new FileInfo(Path.Combine(DestinationInfo.FullName, File.Name));
                        if (FileInfo.LastWriteTime.CompareTo(File.LastWriteTime) < 0)
                        {
                            File.CopyTo(Path.Combine(DestinationInfo.FullName, File.Name), true);
                        }
                    }
                    else
                    {
                        File.CopyTo(Path.Combine(DestinationInfo.FullName, File.Name), true);
                    }
                }
                else if (Options == CopyOptions.DoNotOverwrite)
                {
                    File.CopyTo(Path.Combine(DestinationInfo.FullName, File.Name), false);
                }
            }
            if (Recursive)
            {
                List<DirectoryInfo> Directories = DirectoryList(SourceInfo.FullName);
                foreach (DirectoryInfo Directory in Directories)
                {
                    CopyDirectory(Directory.FullName, Path.Combine(DestinationInfo.FullName, Directory.Name), Recursive, Options);
                }
            }
        }

        #endregion

        #region CreateDirectory

        /// <summary>
        /// Creates a directory
        /// </summary>
        /// <param name="DirectoryPath">Directory to create</param>
        public static void CreateDirectory(string DirectoryPath)
        {
            if (string.IsNullOrEmpty(DirectoryPath))
                throw new ArgumentNullException("DirectoryPath");
            if (!DirectoryExists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="FileName">Path of the file</param>
        public static void Delete(string FileName)
        {
            if (string.IsNullOrEmpty(FileName))
                throw new ArgumentNullException("FileName");
            File.Delete(FileName);
        }

        #endregion

        #region DeleteDirectory

        /// <summary>
        /// Deletes a directory and all files found within it.
        /// </summary>
        /// <param name="DirectoryPath">Path to remove</param>
        public static void DeleteDirectory(string DirectoryPath)
        {
            if (string.IsNullOrEmpty(DirectoryPath))
                throw new ArgumentNullException("DirectoryPath");
            Directory.Delete(DirectoryPath, true);
        }

        #endregion

        #region DeleteFilesNewerThan

        /// <summary>
        /// Deletes files newer than the specified date
        /// </summary>
        /// <param name="Directory">Directory to look within</param>
        /// <param name="CompareDate">The date to compare to</param>
        /// <param name="Recursive">Is this a recursive call</param>
        public static void DeleteFilesNewerThan(string Directory, DateTime CompareDate, bool Recursive = false)
        {
            if (string.IsNullOrEmpty(Directory))
                throw new ArgumentNullException("Directory");
            List<FileInfo> Files = FileManager.FileList(Directory, Recursive);
            foreach (FileInfo File in Files)
            {
                if (File.LastWriteTime > CompareDate)
                {
                    FileManager.Delete(File.FullName);
                }
            }
        }

        #endregion

        #region DeleteFilesOlderThan

        /// <summary>
        /// Deletes files older than the specified date
        /// </summary>
        /// <param name="Directory">Directory to look within</param>
        /// <param name="CompareDate">The date to compare to</param>
        /// <param name="Recursive">Is this a recursive call</param>
        public static void DeleteFilesOlderThan(string Directory, DateTime CompareDate, bool Recursive = false)
        {
            if (string.IsNullOrEmpty(Directory))
                throw new ArgumentNullException("Directory");
            List<FileInfo> Files = FileManager.FileList(Directory, Recursive);
            foreach (FileInfo File in Files)
            {
                if (File.LastWriteTime < CompareDate)
                {
                    FileManager.Delete(File.FullName);
                }
            }
        }

        #endregion

        #region DirectoryExists

        /// <summary>
        /// Determines if a directory exists
        /// </summary>
        /// <param name="DirectoryPath">Path of the directory</param>
        /// <returns>true if it exists, false otherwise</returns>
        public static bool DirectoryExists(string DirectoryPath)
        {
            if (string.IsNullOrEmpty(DirectoryPath))
                throw new ArgumentNullException("DirectoryPath");
            return Directory.Exists(DirectoryPath);
        }

        #endregion

        #region DirectoryList

        /// <summary>
        /// Directory listing
        /// </summary>
        /// <param name="DirectoryPath">Path to get the directories in</param>
        /// <returns>List of directories</returns>
        public static List<DirectoryInfo> DirectoryList(string DirectoryPath)
        {
            if (string.IsNullOrEmpty(DirectoryPath))
                throw new ArgumentNullException("DirectoryPath");
            List<DirectoryInfo> Directories = new List<DirectoryInfo>();
            if (DirectoryExists(DirectoryPath))
            {
                DirectoryInfo Directory = new DirectoryInfo(DirectoryPath);
                DirectoryInfo[] SubDirectories = Directory.GetDirectories();
                foreach (DirectoryInfo SubDirectory in SubDirectories)
                {
                    Directories.Add(SubDirectory);
                }
            }
            return Directories;
        }

        #endregion

        #region FileExists

        /// <summary>
        /// Determines if a file exists
        /// </summary>
        /// <param name="FileName">File name</param>
        /// <returns>true if it exists, false otherwise</returns>
        public static bool FileExists(string FileName)
        {
            if (string.IsNullOrEmpty(FileName))
                throw new ArgumentNullException("FileName");
            return File.Exists(FileName);
        }

        #endregion

        #region FileList

        /// <summary>
        /// Gets a list of files
        /// </summary>
        /// <param name="DirectoryPath">Directory to check for files</param>
        /// <param name="Recursive">Determines if this is a recursive look at all directories under this one</param>
        /// <returns>a list of files</returns>
        public static List<FileInfo> FileList(string DirectoryPath, bool Recursive = false)
        {
            if (string.IsNullOrEmpty(DirectoryPath))
                throw new ArgumentNullException("DirectoryPath");
            List<FileInfo> Files = new List<FileInfo>();
            if (DirectoryExists(DirectoryPath))
            {
                DirectoryInfo Directory = new DirectoryInfo(DirectoryPath);
                Files.AddRange(Directory.GetFiles());
                if (Recursive)
                {
                    DirectoryInfo[] SubDirectories = Directory.GetDirectories();
                    foreach (DirectoryInfo SubDirectory in SubDirectories)
                    {
                        Files.AddRange(FileList(SubDirectory.FullName, true));
                    }
                }
            }
            return Files;
        }

        #endregion

        #region GetDirectorySize

        /// <summary>
        /// Gets the size of all files within a directory
        /// </summary>
        /// <param name="Directory">Directory path</param>
        /// <param name="Recursive">determines if this is a recursive call or not</param>
        /// <returns>The directory size</returns>
        public static long GetDirectorySize(string Directory, bool Recursive = false)
        {
            if (string.IsNullOrEmpty(Directory))
                throw new ArgumentNullException("Directory");
            long Size = 0;
            List<FileInfo> Files = FileManager.FileList(Directory, Recursive);
            foreach (FileInfo File in Files)
            {
                Size += File.Length;
            }
            return Size;
        }

        #endregion

        #region GetFileContents

        /// <summary>
        /// Gets a files' contents
        /// </summary>
        /// <param name="FileName">File name</param>
        /// <param name="TimeOut">Amount of time in ms to wait for the file</param>
        /// <returns>a string containing the file's contents</returns>
        public static string GetFileContents(string FileName, int TimeOut = 5000)
        {
            if (string.IsNullOrEmpty(FileName))
                throw new ArgumentNullException("FileName");
            if (!FileExists(FileName))
                return "";
            StreamReader Reader = null;
            int StartTime = System.Environment.TickCount;
            try
            {
                bool Opened = false;
                while (!Opened)
                {
                    try
                    {
                        if (System.Environment.TickCount - StartTime >= TimeOut)
                            throw new System.IO.IOException("File opening timed out");
                        Reader = File.OpenText(FileName);
                        Opened = true;
                    }
                    catch (System.IO.IOException) { throw; }
                }
                string Contents = Reader.ReadToEnd();
                Reader.Close();
                return Contents;
            }
            catch { throw; }
            finally
            {
                if (Reader != null)
                {
                    Reader.Close();
                    Reader.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets a files' contents
        /// </summary>
        /// <param name="FileName">File name</param>
        /// <param name="TimeOut">Amount of time in ms to wait for the file</param>
        /// <param name="Output">Output of the file in bytes</param>
        public static void GetFileContents(string FileName, out byte[] Output, int TimeOut = 5000)
        {
            if (string.IsNullOrEmpty(FileName))
                throw new ArgumentNullException("FileName");
            if (!FileExists(FileName))
            {
                Output = null;
                return;
            }
            FileStream Reader = null;
            int StartTime = System.Environment.TickCount;
            try
            {
                bool Opened = false;
                while (!Opened)
                {
                    try
                    {
                        if (System.Environment.TickCount - StartTime >= TimeOut)
                            throw new System.IO.IOException("File opening timed out");
                        Reader = File.OpenRead(FileName);
                        Opened = true;
                    }
                    catch (System.IO.IOException) { throw; }
                }
                byte[] Buffer = new byte[1024];
                using (MemoryStream TempReader = new MemoryStream())
                {
                    while (true)
                    {
                        int Count = Reader.Read(Buffer, 0, 1024);
                        TempReader.Write(Buffer, 0, Count);
                        if (Count < 1024)
                            break;
                    }
                    Reader.Close();
                    Output = TempReader.ToArray();
                    TempReader.Close();
                }
            }
            catch
            {
                Output = null;
                throw;
            }
            finally
            {
                if (Reader != null)
                {
                    Reader.Close();
                    Reader.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets a file's contents (Used primarily for text documents on an FTP)
        /// </summary>
        /// <param name="FileName">Name of the file (should include the server address)</param>
        /// <param name="UserName">User name to log in</param>
        /// <param name="Password">Password to log in</param>
        /// <returns>A string containing the file's contents</returns>
        public static string GetFileContents(Uri FileName, string UserName = "", string Password = "")
        {
            if (FileName == null)
                throw new ArgumentNullException("FileName");
            using (WebClient Client = new WebClient())
            {
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                    Client.Credentials = new NetworkCredential(UserName, Password);
                using (StreamReader Reader = new StreamReader(Client.OpenRead(FileName)))
                {
                    string Contents = Reader.ReadToEnd();
                    Reader.Close();
                    return Contents;
                }
            }
        }

        /// <summary>
        /// Gets a files' contents
        /// </summary>
        /// <param name="FileName">File name</param>
        /// <param name="UserName">User name to log in</param>
        /// <param name="Password">Password to log in</param>
        /// <param name="OutputStream">The output stream of the file</param>
        /// <param name="Client">WebClient that is opened by the system</param>
        /// <returns>a string containing the file's contents</returns>
        public static void GetFileContents(Uri FileName, out Stream OutputStream, out WebClient Client, string UserName = "", string Password = "")
        {
            if (FileName == null)
                throw new ArgumentNullException("FileName");
            Client = new WebClient();
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                Client.Credentials = new NetworkCredential(UserName, Password);
            OutputStream = Client.OpenRead(FileName);
        }

        #endregion

        #region RenameFile

        /// <summary>
        /// Renames a file
        /// </summary>
        /// <param name="FileName">Original file</param>
        /// <param name="NewFileName">New file name</param>
        public static void RenameFile(string FileName, string NewFileName)
        {
            if (string.IsNullOrEmpty(FileName))
                throw new ArgumentNullException("FileName");
            if (string.IsNullOrEmpty(NewFileName))
                throw new ArgumentNullException("NewFileName");
            File.Move(FileName, NewFileName);
        }

        #endregion

        #region SaveFile

        /// <summary>
        /// Saves a file
        /// </summary>
        /// <param name="Content">Content of the file</param>
        /// <param name="FileName">Path of the file</param>
        /// <param name="Append">Tells the system if you wish to append data or create a new document</param>
        public static void SaveFile(string Content, string FileName, bool Append = false)
        {
            if (string.IsNullOrEmpty(FileName))
                throw new ArgumentNullException("FileName");
            int Index = FileName.LastIndexOf('/');
            if (Index <= 0)
                Index = FileName.LastIndexOf('\\');
            if (Index <= 0)
                throw new Exception("Directory must be specified for the file");
            string Directory = FileName.Remove(Index) + "/";
            CreateDirectory(Directory);
            FileStream Writer = null;
            try
            {
                byte[] ContentBytes = Encoding.UTF8.GetBytes(Content);
                bool Opened = false;
                while (!Opened)
                {
                    try
                    {
                        if (Append)
                        {
                            Writer = File.Open(FileName, FileMode.Append, FileAccess.Write, FileShare.None);
                        }
                        else
                        {
                            Writer = File.Open(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                        }
                        Opened = true;
                    }
                    catch (System.IO.IOException) { throw; }
                }
                Writer.Write(ContentBytes, 0, ContentBytes.Length);
                Writer.Close();
            }
            catch { throw; }
            finally
            {
                if (Writer != null)
                {
                    Writer.Close();
                    Writer.Dispose();
                }
            }
        }

        /// <summary>
        /// Saves a file to an FTP server
        /// </summary>
        /// <param name="Content">File content</param>
        /// <param name="FileName">File name to save this as (should include directories if applicable)</param>
        /// <param name="FTPServer">Location of the ftp server</param>
        /// <param name="UserName">User name to log in</param>
        /// <param name="Password">Password to log in</param>
        public static void SaveFile(string Content, string FileName, Uri FTPServer, string UserName, string Password)
        {
            if (string.IsNullOrEmpty(FileName))
                throw new ArgumentNullException("FileName");
            if (FTPServer == null)
                throw new ArgumentNullException("FTPServer");
            Uri TempURI = new Uri(Path.Combine(FTPServer.ToString(), FileName));
            FtpWebRequest FTPRequest = (FtpWebRequest)FtpWebRequest.Create(TempURI);
            if (!string.IsNullOrEmpty("UserName") && !string.IsNullOrEmpty("Password"))
                FTPRequest.Credentials = new NetworkCredential(UserName, Password);
            FTPRequest.KeepAlive = false;
            FTPRequest.Method = WebRequestMethods.Ftp.UploadFile;
            FTPRequest.UseBinary = true;
            FTPRequest.ContentLength = Content.Length;
            FTPRequest.Proxy = null;
            using (Stream TempStream = FTPRequest.GetRequestStream())
            {
                System.Text.ASCIIEncoding TempEncoding = new System.Text.ASCIIEncoding();
                byte[] TempBytes = TempEncoding.GetBytes(Content);
                TempStream.Write(TempBytes, 0, TempBytes.Length);
            }
            FTPRequest.GetResponse();
        }

        /// <summary>
        /// Saves a file
        /// </summary>
        /// <param name="Content">File content</param>
        /// <param name="FileName">File name to save this as (should include directories if applicable)</param>
        /// <param name="Append">Tells the system if you wish to append data or create a new document</param>
        public static void SaveFile(byte[] Content, string FileName, bool Append = false)
        {
            int Index = FileName.LastIndexOf('/');
            if (Index <= 0)
                Index = FileName.LastIndexOf('\\');
            if (Index <= 0)
                throw new Exception("Directory must be specified for the file");
            string Directory = FileName.Remove(Index) + "/";
            CreateDirectory(Directory);
            FileStream Writer = null;
            try
            {
                bool Opened = false;
                while (!Opened)
                {
                    try
                    {
                        if (Append)
                        {
                            Writer = File.Open(FileName, FileMode.Append, FileAccess.Write, FileShare.None);
                        }
                        else
                        {
                            Writer = File.Open(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                        }
                        Opened = true;
                    }
                    catch (System.IO.IOException) { throw; }
                }
                Writer.Write(Content, 0, Content.Length);
                Writer.Close();
            }
            catch { throw; }
            finally
            {
                if (Writer != null)
                {
                    Writer.Close();
                    Writer.Dispose();
                }
            }
        }

        #endregion

        #endregion
    }

    #region Enums
    /// <summary>
    /// Options used in directory copying
    /// </summary>
    public enum CopyOptions
    {
        CopyIfNewer,
        CopyAlways,
        DoNotOverwrite
    }
    #endregion
}