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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using Utilities.DataTypes;
using Utilities.IO.Enums;
using Utilities.IO.FileSystem.BaseClasses;
using Utilities.IO.FileSystem.Interfaces;

namespace Utilities.IO.FileSystem.Default
{
    /// <summary>
    /// Directory class
    /// </summary>
    public class ResourceDirectory : DirectoryBase<string, ResourceDirectory>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResourceDirectory()
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
        public ResourceDirectory(string Path, string UserName = "", string Password = "", string Domain = "")
            : base(Path, UserName, Password, Domain)
        {
            var Match = SplitPathRegex.Match(Path);
            this.ResourceFileName = Match.Groups["ResourceFile"].Success ? Match.Groups["ResourceFile"].Value : "";
            this.AssemblyFrom = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName == Match.Groups["Assembly"].Value);
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
        /// Gets the split path regex.
        /// </summary>
        /// <value>
        /// The split path regex.
        /// </value>
        private Regex SplitPathRegex { get { return new Regex(@"^resource://(?<Assembly>[^/]*)/((?<ResourceFile>[^/]*)/?)?", RegexOptions.Compiled | RegexOptions.IgnoreCase); } }

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
            get { return InternalDirectory; }
        }

        /// <summary>
        /// returns now
        /// </summary>
        public override DateTime Modified
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override string Name
        {
            get { return InternalDirectory; }
        }

        /// <summary>
        /// Full path
        /// </summary>
        public override IDirectory Parent
        {
            get { return string.IsNullOrEmpty(InternalDirectory) ? null : new ResourceDirectory((string)InternalDirectory.Take(InternalDirectory.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) - 1), UserName, Password, Domain); }
        }

        /// <summary>
        /// Root
        /// </summary>
        public override IDirectory Root
        {
            get { return string.IsNullOrEmpty(InternalDirectory) ? null : new ResourceDirectory("resource://" + AssemblyFrom.FullName, UserName, Password, Domain); }
        }

        /// <summary>
        /// Size (returns 0)
        /// </summary>
        public override long Size
        {
            get { return 0; }
        }

        /// <summary>
        /// Copies the directory to the specified parent directory
        /// </summary>
        /// <param name="Directory">Directory to copy to</param>
        /// <param name="Options">Options</param>
        /// <returns>Newly created directory</returns>
        public override IDirectory CopyTo(IDirectory Directory, CopyOptions Options = CopyOptions.CopyAlways)
        {
            return this;
        }

        /// <summary>
        /// Not used
        /// </summary>
        public override void Create()
        {
        }

        /// <summary>
        /// Not used
        /// </summary>
        public override void Delete()
        {
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="SearchPattern"></param>
        /// <param name="Options"></param>
        /// <returns></returns>
        public override IEnumerable<IDirectory> EnumerateDirectories(string SearchPattern, SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            return new List<ResourceDirectory>();
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="SearchPattern"></param>
        /// <param name="Options"></param>
        /// <returns></returns>
        public override IEnumerable<IFile> EnumerateFiles(string SearchPattern = "*", SearchOption Options = SearchOption.TopDirectoryOnly)
        {
            return AssemblyFrom.GetManifestResourceNames().Select(x => new ResourceFile(x, UserName, Password, Domain));
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Directory"></param>
        public override void MoveTo(IDirectory Directory)
        {
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="Name"></param>
        public override void Rename(string Name)
        {
        }
    }
}