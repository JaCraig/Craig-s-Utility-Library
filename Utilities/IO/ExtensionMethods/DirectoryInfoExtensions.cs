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
using System.Linq;
using System.Text;
using System.IO;
using Utilities.IO.ExtensionMethods.Enums;
#endregion

namespace Utilities.IO.ExtensionMethods
{
    /// <summary>
    /// Extension methods for <see cref="System.IO.DirectoryInfo"/>
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        #region Extension Methods

        #region CopyTo

        /// <summary>
        /// Copies a directory to another location
        /// </summary>
        /// <param name="Source">Source directory</param>
        /// <param name="Destination">Destination directory</param>
        /// <param name="Recursive">Should the copy be recursive</param>
        /// <param name="Options">Options used in copying</param>
        /// <returns>The DirectoryInfo for the destination info</returns>
        public static DirectoryInfo CopyTo(this DirectoryInfo Source, string Destination, bool Recursive = true, CopyOptions Options = CopyOptions.CopyAlways)
        {
            if (Source == null)
                throw new ArgumentNullException("Source");
            if (!Source.Exists)
                throw new DirectoryNotFoundException("Source directory " + Source.FullName + " not found.");
            if (string.IsNullOrEmpty(Destination))
                throw new ArgumentNullException("Destination");
            DirectoryInfo DestinationInfo = new DirectoryInfo(Destination);
            DestinationInfo.Create();
            foreach (FileInfo TempFile in Source.EnumerateFiles())
            {
                if (Options == CopyOptions.CopyAlways)
                {
                    TempFile.CopyTo(Path.Combine(DestinationInfo.FullName, TempFile.Name), true);
                }
                else if (Options == CopyOptions.CopyIfNewer)
                {
                    if (File.Exists(Path.Combine(DestinationInfo.FullName, TempFile.Name)))
                    {
                        FileInfo FileInfo = new FileInfo(Path.Combine(DestinationInfo.FullName, TempFile.Name));
                        if (FileInfo.LastWriteTime.CompareTo(TempFile.LastWriteTime) < 0)
                            TempFile.CopyTo(Path.Combine(DestinationInfo.FullName, TempFile.Name), true);
                    }
                    else
                    {
                        TempFile.CopyTo(Path.Combine(DestinationInfo.FullName, TempFile.Name), true);
                    }
                }
                else if (Options == CopyOptions.DoNotOverwrite)
                {
                    TempFile.CopyTo(Path.Combine(DestinationInfo.FullName, TempFile.Name), false);
                }
            }
            if (Recursive)
            {
                foreach (DirectoryInfo SubDirectory in Source.EnumerateDirectories())
                    SubDirectory.CopyTo(Path.Combine(DestinationInfo.FullName, SubDirectory.Name), Recursive, Options);
            }
            return new DirectoryInfo(Destination);
        }

        #endregion

        #region DeleteAll

        /// <summary>
        /// Deletes directory and all content found within it
        /// </summary>
        /// <param name="Info">Directory info object</param>
        public static void DeleteAll(this DirectoryInfo Info)
        {
            if (!Info.Exists)
                return;
            System.IO.Directory.Delete(Info.FullName, true);
        }

        #endregion

        #region DeleteFilesNewerThan

        /// <summary>
        /// Deletes files newer than the specified date
        /// </summary>
        /// <param name="Directory">Directory to look within</param>
        /// <param name="CompareDate">The date to compare to</param>
        /// <param name="Recursive">Is this a recursive call</param>
        /// <returns>Returns the directory object</returns>
        public static DirectoryInfo DeleteFilesNewerThan(this DirectoryInfo Directory, DateTime CompareDate, bool Recursive = false)
        {
            if (!Directory.Exists)
                throw new DirectoryNotFoundException("Directory");
            if (CompareDate == null)
                throw new ArgumentNullException("CompareDate");
            foreach (FileInfo File in Directory.EnumerateFiles("*", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Where(x => x.LastWriteTime > CompareDate))
                File.Delete();
            return Directory;
        }

        #endregion

        #region DeleteFilesOlderThan

        /// <summary>
        /// Deletes files older than the specified date
        /// </summary>
        /// <param name="Directory">Directory to look within</param>
        /// <param name="CompareDate">The date to compare to</param>
        /// <param name="Recursive">Is this a recursive call</param>
        /// <returns>Returns the directory object</returns>
        public static DirectoryInfo DeleteFilesOlderThan(this DirectoryInfo Directory, DateTime CompareDate, bool Recursive = false)
        {
            if (!Directory.Exists)
                throw new DirectoryNotFoundException("Directory");
            if (CompareDate == null)
                throw new ArgumentNullException("CompareDate");
            foreach (FileInfo File in Directory.EnumerateFiles("*", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Where(x => x.LastWriteTime < CompareDate))
                File.Delete();
            return Directory;
        }

        #endregion

        #region Size

        /// <summary>
        /// Gets the size of all files within a directory
        /// </summary>
        /// <param name="Directory">Directory</param>
        /// <param name="Recursive">determines if this is a recursive call or not</param>
        /// <returns>The directory size</returns>
        public static long Size(this DirectoryInfo Directory, bool Recursive = false)
        {
            if (Directory == null)
                throw new ArgumentNullException("Directory");
            return Directory.EnumerateFiles("*", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Sum(x => x.Length);
        }

        #endregion

        #region SetAttributes

        /// <summary>
        /// Sets a directory's attributes
        /// </summary>
        /// <param name="Directory">Directory</param>
        /// <param name="Attributes">Attributes to set</param>
        /// <param name="Recursive">Determines if this is a recursive call</param>
        /// <returns>The directory object</returns>
        public static DirectoryInfo SetAttributes(this DirectoryInfo Directory, System.IO.FileAttributes Attributes, bool Recursive = false)
        {
            foreach (FileInfo File in Directory.EnumerateFiles())
                File.SetAttributes(Attributes);
            if (Recursive)
            {
                foreach (DirectoryInfo TempDirectory in Directory.EnumerateDirectories())
                    TempDirectory.SetAttributes(Attributes, true);
            }
            return Directory;
        }

        #endregion

        #endregion
    }
}