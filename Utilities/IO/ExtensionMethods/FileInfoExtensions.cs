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
using System.Diagnostics.Contracts;
using System.IO;
using System.Security;
using System.Text;
using System.Web;
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
            return File1.Read().Equals(File2.Read());
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
            Contract.Requires<ArgumentNullException>(File != null, "File");
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
            Contract.Requires<ArgumentNullException>(File != null, "File");
            if (!File.Exists)
                return "";
            using (StreamReader Reader = File.OpenText())
            {
                string Contents = Reader.ReadToEnd();
                return Contents;
            }
        }

        /// <summary>
        /// Reads a file to the end as a string
        /// </summary>
        /// <param name="Location">File to read</param>
        /// <returns>A string containing the contents of the file</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", MessageId = "System.String.StartsWith(System.String,System.StringComparison)")]
        public static string Read(this string Location)
        {
            if (Location.StartsWith("~", StringComparison.InvariantCulture))
            {
                if (HttpContext.Current == null)
                    Location = Location.Replace("~", AppDomain.CurrentDomain.BaseDirectory);
                else
                    Location = HttpContext.Current.Server.MapPath(Location);
            }
            return new FileInfo(Location).Read();
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
            Contract.Requires<ArgumentNullException>(File != null, "File");
            if (!File.Exists)
                return new byte[0];
            using (FileStream Reader = File.OpenRead())
            {
                byte[] Output = Reader.ReadAllBinary();
                return Output;
            }
        }

        /// <summary>
        /// Reads a file to the end and returns a binary array
        /// </summary>
        /// <param name="Location">File to open</param>
        /// <returns>A binary array containing the contents of the file</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", MessageId = "System.String.StartsWith(System.String,System.StringComparison)")]
        public static byte[] ReadBinary(this string Location)
        {
            if (Location.StartsWith("~", StringComparison.InvariantCulture))
            {
                if (HttpContext.Current == null)
                    Location = Location.Replace("~", AppDomain.CurrentDomain.BaseDirectory);
                else
                    Location = HttpContext.Current.Server.MapPath(Location);
            }
            return new FileInfo(Location).ReadBinary();
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
        public static System.Diagnostics.Process Execute(this FileInfo File, string Arguments = "",
            string Domain = "", string User = "", string Password = "",
            ProcessWindowStyle WindowStyle = ProcessWindowStyle.Normal, string WorkingDirectory = "")
        {
            Contract.Requires<ArgumentNullException>(File != null, "File");
            Contract.Requires<FileNotFoundException>(File.Exists, "file does not exist");
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
        public static System.Diagnostics.Process Execute(this string File, string Arguments = "",
            string Domain = "", string User = "", string Password = "",
            ProcessWindowStyle WindowStyle = ProcessWindowStyle.Normal, string WorkingDirectory = "")
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(File), "File");
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = Arguments;
            Info.Domain = Domain;
            Info.Password = new SecureString();
            foreach (char Char in Password)
                Info.Password.AppendChar(Char);
            Info.UserName = User;
            Info.WindowStyle = WindowStyle;
            Info.UseShellExecute = false;
            Info.WorkingDirectory = WorkingDirectory;
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
            Contract.Requires<ArgumentNullException>(File != null, "File");
            Contract.Requires<FileNotFoundException>(File.Exists, "File not found");
            Contract.Requires<ArgumentNullException>(Info != null, "Info");
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
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(File), "File");
            Contract.Requires<ArgumentNullException>(Info!=null, "Info");
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
        /// <param name="Mode">Mode for saving the file (defaults to Create)</param>
        /// <returns>The FileInfo object</returns>
        public static FileInfo Save(this FileInfo File, string Content, FileMode Mode = FileMode.Create, Encoding EncodingUsing = null)
        {
            Contract.Requires<ArgumentNullException>(File != null, "File");
            return File.Save(EncodingUsing.NullCheck(new ASCIIEncoding()).GetBytes(Content), Mode);
        }

        /// <summary>
        /// Saves a byte array to a file
        /// </summary>
        /// <param name="File">File to save to</param>
        /// <param name="Content">Content to save to the file</param>
        /// <param name="Mode">Mode for saving the file (defaults to Create)</param>
        /// <returns>The FileInfo object</returns>
        public static FileInfo Save(this FileInfo File, byte[] Content, FileMode Mode = FileMode.Create)
        {
            Contract.Requires<ArgumentNullException>(File != null, "File");
            new DirectoryInfo(File.DirectoryName).Create();
            using (FileStream Writer = File.Open(Mode, FileAccess.Write))
            {
                Writer.Write(Content, 0, Content.Length);
            }
            return File;
        }

        /// <summary>
        /// Saves the string to the specified file
        /// </summary>
        /// <param name="Location">Location to save the content to</param>
        /// <param name="Content">Content to save the the file</param>
        /// <param name="EncodingUsing">Encoding that the content is using (defaults to ASCII)</param>
        /// <param name="Mode">Mode for saving the file (defaults to Create)</param>
        /// <returns>The FileInfo object associated with the location</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", MessageId = "System.String.StartsWith(System.String,System.StringComparison)")]
        public static FileInfo Save(this string Location, string Content, FileMode Mode = FileMode.Create, Encoding EncodingUsing = null)
        {
            if (Location.StartsWith("~", StringComparison.InvariantCulture))
            {
                if (HttpContext.Current == null)
                    Location = Location.Replace("~", AppDomain.CurrentDomain.BaseDirectory);
                else
                    Location = HttpContext.Current.Server.MapPath(Location);
            }
            return new FileInfo(Location).Save(Content, Mode, EncodingUsing);
        }

        /// <summary>
        /// Saves a byte array to a file
        /// </summary>
        /// <param name="Location">File to save to</param>
        /// <param name="Content">Content to save to the file</param>
        /// <param name="Mode">Mode for saving the file (defaults to Create)</param>
        /// <returns>The FileInfo object associated with the location</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", MessageId = "System.String.StartsWith(System.String,System.StringComparison)")]
        public static FileInfo Save(this string Location, byte[] Content, FileMode Mode = FileMode.Create)
        {
            if (Location.StartsWith("~", StringComparison.InvariantCulture))
            {
                if (HttpContext.Current == null)
                    Location = Location.Replace("~", AppDomain.CurrentDomain.BaseDirectory);
                else
                    Location = HttpContext.Current.Server.MapPath(Location);
            }
            return new FileInfo(Location).Save(Content, Mode);
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
        /// <param name="Mode">Mode for saving the file (defaults to Create)</param>
        /// <returns>The FileInfo object</returns>
        public static FileInfo SaveAsync(this FileInfo File, string Content, AsyncCallback CallBack,
            object StateObject, FileMode Mode = FileMode.Create, Encoding EncodingUsing = null)
        {
            Contract.Requires<ArgumentNullException>(File != null, "File");
            return File.SaveAsync(EncodingUsing.NullCheck(new ASCIIEncoding()).GetBytes(Content), CallBack, StateObject, Mode);
        }

        /// <summary>
        /// Saves a byte array to a file (asynchronously)
        /// </summary>
        /// <param name="File">File to save to</param>
        /// <param name="Content">Content to save to the file</param>
        /// <param name="CallBack">Call back function</param>
        /// <param name="StateObject">State object</param>
        /// <param name="Mode">Mode for saving the file (defaults to Create)</param>
        /// <returns>The FileInfo object</returns>
        public static FileInfo SaveAsync(this FileInfo File, byte[] Content, AsyncCallback CallBack,
            object StateObject, FileMode Mode = FileMode.Create)
        {
            Contract.Requires<ArgumentNullException>(File != null, "File");
            new DirectoryInfo(File.DirectoryName).Create();
            using (FileStream Writer = File.Open(Mode, FileAccess.Write))
            {
                Writer.BeginWrite(Content, 0, Content.Length, CallBack, StateObject);
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
            Contract.Requires<ArgumentNullException>(File != null, "File");
            Contract.Requires<FileNotFoundException>(File.Exists, "File");
            System.IO.File.SetAttributes(File.FullName, Attributes);
            return File;
        }

        #endregion

        #endregion
    }
}