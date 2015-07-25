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

using System;
using System.Collections.Generic;
using System.IO;

#endregion Usings

namespace Ironman.Core.Assets.Utils
{
    /// <summary>
    /// Compares two file info objects
    /// </summary>
    public class FileInfoComparer : IEqualityComparer<FileInfo>
    {
        /// <summary>
        /// Determines if two FileInfo objects are equal
        /// </summary>
        /// <param name="x">File 1</param>
        /// <param name="y">File 2</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public bool Equals(FileInfo x, FileInfo y)
        {
            if (x == y)
                return true;

            if (x == null || y == null)
                return false;

            return string.Equals(x.FullName, y.FullName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the hash code of the file
        /// </summary>
        /// <param name="obj">File</param>
        /// <returns>The hash code</returns>
        public int GetHashCode(FileInfo obj)
        {
            if (obj == null)
                return 0;
            return obj.FullName.GetHashCode();
        }
    }
}