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

using System.ComponentModel;
using System.IO;

namespace Utilities.IO
{
    /// <summary>
    /// Extension methods for strings
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StringExtensions
    {
        /// <summary>
        /// Removes illegal characters from a directory
        /// </summary>
        /// <param name="DirectoryName">Directory name</param>
        /// <param name="ReplacementChar">Replacement character</param>
        /// <returns>DirectoryName with all illegal characters replaced with ReplacementChar</returns>
        public static string RemoveIllegalDirectoryNameCharacters(this string DirectoryName, char ReplacementChar = '_')
        {
            if (string.IsNullOrEmpty(DirectoryName))
                return DirectoryName;
            foreach (char Char in Path.GetInvalidPathChars())
                DirectoryName = DirectoryName.Replace(Char, ReplacementChar);
            return DirectoryName;
        }

        /// <summary>
        /// Removes illegal characters from a file
        /// </summary>
        /// <param name="FileName">File name</param>
        /// <param name="ReplacementChar">Replacement character</param>
        /// <returns>FileName with all illegal characters replaced with ReplacementChar</returns>
        public static string RemoveIllegalFileNameCharacters(this string FileName, char ReplacementChar = '_')
        {
            if (string.IsNullOrEmpty(FileName))
                return FileName;
            foreach (char Char in Path.GetInvalidFileNameChars())
                FileName = FileName.Replace(Char, ReplacementChar);
            return FileName;
        }
    }
}