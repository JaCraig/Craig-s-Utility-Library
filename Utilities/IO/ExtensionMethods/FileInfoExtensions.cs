/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Text;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.IO.ExtensionMethods
{
    /// <summary>
    /// Extension methods for <see cref="System.IO.FileInfo"/>
    /// </summary>
    public static class FileInfoExtensions
    {
        #region Extension Methods

        #region Append

        /// <summary>
        /// Appends a string to a file
        /// </summary>
        /// <param name="File">File to append to</param>
        /// <param name="Content">Content to save to the file</param>
        /// <param name="EncodingUsing">The type of encoding the string is using (defaults to ASCII)</param>
        /// <returns>The FileInfo object</returns>
        public static FileInfo Append(this FileInfo File, string Content, Encoding EncodingUsing = null)
        {
            if (File == null)
                throw new ArgumentNullException("File");
            if (EncodingUsing == null)
                EncodingUsing = new ASCIIEncoding();
            byte[] ContentBytes = EncodingUsing.GetBytes(Content);
            return File.Append(ContentBytes);
        }

        /// <summary>
        /// Appends a byte array to a file
        /// </summary>
        /// <param name="File">File to append to</param>
        /// <param name="Content">Content to append to the file</param>
        /// <returns>The FileInfo object</returns>
        public static FileInfo Append(this FileInfo File, byte[] Content)
        {
            if (File == null)
                throw new ArgumentNullException("File");
            if (!File.Exists)
                return File.Save(Content);
            using (FileStream Writer = File.Open(FileMode.Append, FileAccess.Write))
            {
                Writer.Write(Content, 0, Content.Length);
                Writer.Close();
            }
            return File;
        }

        #endregion

        #region CompareTo

        /// <summary>
        /// Compares two files against one another
        /// </summary>
        /// <param name="File1">First file</param>
        /// <param name="File2">Second file</param>
        /// <returns>True if the content is the same, false otherwise</returns>
        public static bool CompareTo(this FileInfo File1, FileInfo File2)
        {
            if (File1 == null || !File1.Exists)
                throw new ArgumentNullException("File1");
            if (File2 == null || !File2.Exists)
                throw new ArgumentNullException("File2");
            if (File1.Length != File2.Length)
                return false;
            if (!File1.Read().Equals(File2.Read()))
                return false;
            return true;
        }

        #endregion

        #region DriveInfo

        /// <summary>
        /// Gets the drive information for a file
        /// </summary>
        /// <param name="File">The file to get the drive info of</param>
        /// <returns>The drive info connected to the file</returns>
        public static DriveInfo DriveInfo(this FileInfo File)
        {
            if (File == null)
                throw new ArgumentNullException("File");
            return File.Directory.DriveInfo();
        }

        #endregion

        #region Read

        /// <summary>
        /// Reads a file to the end as a string
        /// </summary>
        /// <param name="File">File to read</param>
        /// <returns>A string containing the contents of the file</returns>
        public static string Read(this FileInfo File)
        {
            if (File == null)
                throw new ArgumentNullException("File");
            if (!File.Exists)
                return "";
            using (StreamReader Reader = File.OpenText())
            {
                string Contents = Reader.ReadToEnd();
                Reader.Close();
                return Contents;
            }
        }

        #endregion

        #region ReadBinary

        /// <summary>
        /// Reads a file to the end and returns a binary array
        /// </summary>
        /// <param name="File">File to open</param>
        /// <returns>A binary array containing the contents of the file</returns>
        public static byte[] ReadBinary(this FileInfo File)
        {
            if (File == null)
                throw new ArgumentNullException("File");
            if (!File.Exists)
                return new byte[0];
            using (FileStream Reader = File.OpenRead())
            {
                byte[] Output = Reader.ReadAllBinary();
                Reader.Close();
                return Output;
            }
        }

        #endregion

        #region Execute

        /// <summary>
        /// Executes the file
        /// </summary>
        /// <param name="File">File to execute</param>
        /// <param name="Arguments">Arguments sent to the executable</param>
        /// <param name="Domain">Domain of the user</param>
        /// <param name="Password">Password of the user</param>
        /// <param name="User">User to run the file as</param>
        /// <param name="WindowStyle">Window style</param>
        /// <param name="WorkingDirectory">Working directory</param>
        /// <returns>The process object created when the executable is started</returns>
        public static System.Diagnostics.Process Execute(this FileInfo File, string Arguments = "", string Domain = "", string User = "", string Password = "", ProcessWindowStyle WindowStyle = ProcessWindowStyle.Normal, string WorkingDirectory = "")
        {
            if (File == null)
                throw new ArgumentNullException("File");
            if (!File.Exists)
                throw new FileNotFoundException("File note found", File.FullName);
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = Arguments;
            Info.Domain = Domain;
            Info.Password = new SecureString();
            foreach (char Char in Password)
                Info.Password.AppendChar(Char);
            Info.UserName = User;
            Info.WindowStyle = WindowStyle;
            Info.UseShellExecute = false;
            Info.WorkingDirectory = string.IsNullOrEmpty(WorkingDirectory) ? File.DirectoryName : WorkingDirectory;
            return File.Execute(Info);
        }

        /// <summary>
        /// Executes the file
        /// </summary>
        /// <param name="File">File to execute</param>
        /// <param name="Arguments">Arguments sent to the executable</param>
        /// <param name="Domain">Domain of the user</param>
        /// <param name="Password">Password of the user</param>
        /// <param name="User">User to run the file as</param>
        /// <param name="WindowStyle">Window style</param>
        /// <param name="WorkingDirectory">Working directory</param>
        /// <returns>The process object created when the executable is started</returns>
        public static System.Diagnostics.Process Execute(this string File, string Arguments = "", string Domain = "", string User = "", string Password = "", ProcessWindowStyle WindowStyle = ProcessWindowStyle.Normal, string WorkingDirectory = "")
        {
            File.ThrowIfNullOrEmpty("File");
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = Arguments;
            Info.Domain = Domain;
            Info.Password = new SecureString();
            foreach (char Char in Password)
                Info.Password.AppendChar(Char);
            Info.UserName = User;
            Info.WindowStyle = WindowStyle;
            Info.UseShellExecute = false;
            return File.Execute(Info);
        }

        /// <summary>
        /// Executes the file
        /// </summary>
        /// <param name="File">File to execute</param>
        /// <param name="Info">Info used to execute the file</param>
        /// <returns>The process object created when the executable is started</returns>
        public static System.Diagnostics.Process Execute(this FileInfo File, ProcessStartInfo Info)
        {
            if (File == null)
                throw new ArgumentNullException("File");
            if (!File.Exists)
                throw new FileNotFoundException("File note found", File.FullName);
            if (Info == null)
                throw new ArgumentNullException("Info");
            Info.FileName = File.FullName;
            return System.Diagnostics.Process.Start(Info);
        }

        /// <summary>
        /// Executes the file
        /// </summary>
        /// <param name="File">File to execute</param>
        /// <param name="Info">Info used to execute the file</param>
        /// <returns>The process object created when the executable is started</returns>
        public static System.Diagnostics.Process Execute(this string File, ProcessStartInfo Info)
        {
            File.ThrowIfNullOrEmpty("File");
            if (Info == null)
                throw new ArgumentNullException("Info");
            Info.FileName = File;
            return System.Diagnostics.Process.Start(Info);
        }

        #endregion

        #region Save

        /// <summary>
        /// Saves a string to a file
        /// </summary>
        /// <param name="File">File to save to</param>
        /// <param name="Content">Content to save to the file</param>
        /// <param name="EncodingUsing">Encoding that the content is using (defaults to ASCII)</param>
        /// <returns>The FileInfo object</returns>
        public static FileInfo Save(this FileInfo File, string Content,Encoding EncodingUsing=null)
        {
            if (File == null)
                throw new ArgumentNullException("File");
            if (EncodingUsing == null)
                EncodingUsing = new ASCIIEncoding();
            byte[] ContentBytes = EncodingUsing.GetBytes(Content);
            return File.Save(ContentBytes);
        }

        /// <summary>
        /// Saves a byte array to a file
        /// </summary>
        /// <param name="File">File to save to</param>
        /// <param name="Content">Content to save to the file</param>
        /// <returns>The FileInfo object</returns>
        public static FileInfo Save(this FileInfo File, byte[] Content)
        {
            if (File == null)
                throw new ArgumentNullException("File");
            new DirectoryInfo(File.DirectoryName).Create();
            using (FileStream Writer = File.Create())
            {
                Writer.Write(Content, 0, Content.Length);
                Writer.Close();
            }
            return File;
        }

        #endregion

        #region SaveAsync

        /// <summary>
        /// Saves a string to a file (asynchronously)
        /// </summary>
        /// <param name="File">File to save to</param>
        /// <param name="Content">Content to save to the file</param>
        /// <param name="CallBack">Call back function</param>
        /// <param name="StateObject">State object</param>
        /// <param name="EncodingUsing">Encoding that the content is using (defaults to ASCII)</param>
        /// <returns>The FileInfo object</returns>
        public static FileInfo SaveAsync(this FileInfo File, string Content, AsyncCallback CallBack, object StateObject,Encoding EncodingUsing=null)
        {
            if (File == null)
                throw new ArgumentNullException("File");
            if (EncodingUsing == null)
                EncodingUsing = new ASCIIEncoding();
            byte[] ContentBytes = EncodingUsing.GetBytes(Content);
            return File.SaveAsync(ContentBytes, CallBack, StateObject);
        }

        /// <summary>
        /// Saves a byte array to a file (asynchronously)
        /// </summary>
        /// <param name="File">File to save to</param>
        /// <param name="Content">Content to save to the file</param>
        /// <param name="CallBack">Call back function</param>
        /// <param name="StateObject">State object</param>
        /// <returns>The FileInfo object</returns>
        public static FileInfo SaveAsync(this FileInfo File, byte[] Content, AsyncCallback CallBack, object StateObject)
        {
            if (File == null)
                throw new ArgumentNullException("File");
            new DirectoryInfo(File.DirectoryName).Create();
            using (FileStream Writer = File.Create())
            {
                Writer.BeginWrite(Content, 0, Content.Length, CallBack, StateObject);
                Writer.Close();
            }
            return File;
        }

        #endregion

        #region SetAttributes

        /// <summary>
        /// Sets the attributes of a file
        /// </summary>
        /// <param name="File">File</param>
        /// <param name="Attributes">Attributes to set</param>
        /// <returns>The file info</returns>
        public static FileInfo SetAttributes(this FileInfo File, System.IO.FileAttributes Attributes)
        {
            if (File == null || !File.Exists)
                throw new ArgumentNullException("File");
            System.IO.File.SetAttributes(File.FullName, Attributes);
            return File;
        }

        #endregion

        #endregion
    }
}