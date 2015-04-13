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
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.DataTypes;
using Utilities.IO.FileSystem.BaseClasses;
using Utilities.IO.FileSystem.Interfaces;

namespace Utilities.IO.FileSystem.Default
{
    /// <summary>
    /// Basic Resource file class
    /// </summary>
    public class ResourceFile : FileBase<string, ResourceFile>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResourceFile()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the file</param>
        /// <param name="Domain">Domain of the user (optional)</param>
        /// <param name="Password">Password to be used to access the directory (optional)</param>
        /// <param name="UserName">User name to be used to access the directory (optional)</param>
        public ResourceFile(string Path, string UserName = "", string Password = "", string Domain = "")
            : base(Path, UserName, Password, Domain)
        {
            var Match = SplitPathRegex.Match(Path);
            this.ResourceFileName = Match.Groups["ResourceFile"].Success ? Match.Groups["ResourceFile"].Value : "";
            this.Resource = Match.Groups["FileName"].Value;
            this.AssemblyFrom = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == Match.Groups["Assembly"].Value);
        }

        /// <summary>
        /// Gets or sets the assembly this is from.
        /// </summary>
        /// <value>
        /// The assembly this is from.
        /// </value>
        private Assembly AssemblyFrom { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource file.
        /// </summary>
        /// <value>
        /// The name of the resource file.
        /// </value>
        private string ResourceFileName { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        /// <value>
        /// The resource.
        /// </value>
        private string Resource { get; set; }

        /// <summary>
        /// Gets the split path regex.
        /// </summary>
        /// <value>
        /// The split path regex.
        /// </value>
        private Regex SplitPathRegex { get { return new Regex(@"^resource://(?<Assembly>[^/]*)/((?<ResourceFile>[^/]*)/)?(?<FileName>[^/]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }

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
            get { return new ResourceDirectory("resource://" + AssemblyFrom.FullName + "/" + (string.IsNullOrEmpty(ResourceFileName) ? "" : ResourceFileName), UserName, Password, Domain); }
        }

        /// <summary>
        /// Does it exist? Always true.
        /// </summary>
        public override bool Exists
        {
            get { return AssemblyFrom.GetManifestResourceStream(Resource) == null; }
        }

        /// <summary>
        /// Extension (always empty)
        /// </summary>
        public override string Extension
        {
            get { return Resource.Right(Resource.Length - Resource.LastIndexOf('.')); }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override string FullName
        {
            get { return "resource://" + AssemblyFrom.FullName + "/" + (string.IsNullOrEmpty(ResourceFileName) ? "" : ResourceFileName + "/") + Resource; }
        }

        /// <summary>
        /// Size of the file
        /// </summary>
        public override long Length
        {
            get
            {
                using (Stream TempStream = AssemblyFrom.GetManifestResourceStream(Resource))
                {
                    return TempStream.Length;
                }
            }
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
            get { return Resource; }
        }

        /// <summary>
        /// Copies the file to another directory
        /// </summary>
        /// <param name="Directory">Directory to copy the file to</param>
        /// <param name="Overwrite">Should the file overwrite another file if found</param>
        /// <returns>The newly created file</returns>
        public override IFile CopyTo(IDirectory Directory, bool Overwrite)
        {
            return this;
        }

        /// <summary>
        /// Delete (does nothing)
        /// </summary>
        /// <returns>Any response for deleting the resource (usually FTP, HTTP, etc)</returns>
        public override string Delete()
        {
            return "";
        }

        /// <summary>
        /// Moves the file (not used)
        /// </summary>
        /// <param name="Directory">Not used</param>
        public override void MoveTo(IDirectory Directory)
        {
        }

        /// <summary>
        /// Reads the Resource page
        /// </summary>
        /// <returns>The content as a string</returns>
        public override string Read()
        {
            if (InternalFile == null)
                return "";
            using (Stream TempStream = AssemblyFrom.GetManifestResourceStream(Resource))
            {
                return TempStream.ReadAll();
            }
        }

        /// <summary>
        /// Reads the Resource page
        /// </summary>
        /// <returns>The content as a byte array</returns>
        public override byte[] ReadBinary()
        {
            if (InternalFile == null)
                return new byte[0];
            using (Stream TempStream = AssemblyFrom.GetManifestResourceStream(Resource))
            {
                return TempStream.ReadAllBinary();
            }
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
            return "";
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Content">Not used</param>
        /// <param name="Mode">Not used</param>
        /// <returns>The result of the write or original content</returns>
        public override byte[] Write(byte[] Content, System.IO.FileMode Mode = FileMode.Create)
        {
            return new byte[0];
        }
    }
}