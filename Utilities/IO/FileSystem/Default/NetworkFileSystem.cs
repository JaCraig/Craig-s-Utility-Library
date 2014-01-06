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

#region Usings

using Utilities.IO.FileSystem.BaseClasses;

#endregion Usings

namespace Utilities.IO.FileSystem.Default
{
    /// <summary>
    /// Network file system
    /// </summary>
    public class NetworkFileSystem : LocalFileSystemBase
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkFileSystem()
            : base()
        {
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Name of the file system
        /// </summary>
        public override string Name { get { return "Network"; } }

        /// <summary>
        /// Relative starter
        /// </summary>
        protected override string HandleRegexString { get { return @"^\\"; } }

        #endregion Properties

        #region Functions

        /// <summary>
        /// Gets the absolute path of the variable passed in
        /// </summary>
        /// <param name="Path">Path to convert to absolute</param>
        /// <returns>The absolute path of the path passed in</returns>
        protected override string AbsolutePath(string Path)
        {
            return Path;
        }

        /// <summary>
        /// Function to override in order to dispose objects
        /// </summary>
        /// <param name="Managed">
        /// If true, managed and unmanaged objects should be disposed. Otherwise unmanaged objects only.
        /// </param>
        protected override void Dispose(bool Managed)
        {
            base.Dispose(Managed);
        }

        #endregion Functions
    }
}