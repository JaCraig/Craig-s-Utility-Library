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
using System.IO;
using System.Linq;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.IO.ExtensionMethods.Enums;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(Source!=null, "Source");
            Contract.Requires<DirectoryNotFoundException>(Source.Exists, "Source directory not found.");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(Destination), "Destination");
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
            Info.ThrowIfNull("Info");
            if (!Info.Exists)
                return;
            Info.DeleteFiles();
            Info.EnumerateDirectories().ForEach(x => x.DeleteAll());
            Info.Delete(true);
        }

        #endregion

        #region DeleteDirectoriesNewerThan

        /// <summary>
        /// Deletes directories newer than the specified date
        /// </summary>
        /// <param name="Directory">Directory to look within</param>
        /// <param name="CompareDate">The date to compare to</param>
        /// <param name="Recursive">Is this a recursive call</param>
        /// <returns>Returns the directory object</returns>
        public static DirectoryInfo DeleteDirectoriesNewerThan(this DirectoryInfo Directory, DateTime CompareDate, bool Recursive = false)
        {
            Directory.ThrowIfNull("Directory");
            Directory.ThrowIfNot(x => x.Exists, new DirectoryNotFoundException("Directory"));
            Directory.EnumerateDirectories("*", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                     .Where(x => x.LastWriteTime > CompareDate)
                     .ForEach(x => x.DeleteAll());
            return Directory;
        }

        #endregion

        #region DeleteDirectoriesOlderThan

        /// <summary>
        /// Deletes directories newer than the specified date
        /// </summary>
        /// <param name="Directory">Directory to look within</param>
        /// <param name="CompareDate">The date to compare to</param>
        /// <param name="Recursive">Is this a recursive call</param>
        /// <returns>Returns the directory object</returns>
        public static DirectoryInfo DeleteDirectoriesOlderThan(this DirectoryInfo Directory, DateTime CompareDate, bool Recursive = false)
        {
            Directory.ThrowIfNull("Directory");
            Directory.ThrowIfNot(x => x.Exists, new DirectoryNotFoundException("Directory"));
            Directory.EnumerateDirectories("*", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                     .Where(x => x.LastWriteTime < CompareDate)
                     .ForEach(x => x.DeleteAll());
            return Directory;
        }

        #endregion

        #region DeleteFiles

        /// <summary>
        /// Deletes files from a directory
        /// </summary>
        /// <param name="Directory">Directory to delete the files from</param>
        /// <param name="Recursive">Should this be recursive?</param>
        /// <returns>The directory that is sent in</returns>
        public static DirectoryInfo DeleteFiles(this DirectoryInfo Directory, bool Recursive = false)
        {
            Directory.ThrowIfNull("Directory");
            Directory.ThrowIfNot(x => x.Exists, new DirectoryNotFoundException("Directory"));
            Directory.EnumerateFiles("*", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                     .ForEach(x => x.Delete());
            return Directory;
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
            Directory.ThrowIfNull("Directory");
            Directory.ThrowIfNot(x => x.Exists, new DirectoryNotFoundException("Directory"));
            Directory.EnumerateFiles("*", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                     .Where(x => x.LastWriteTime > CompareDate)
                     .ForEach(x => x.Delete());
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
            Directory.ThrowIfNull("Directory");
            Directory.ThrowIfNot(x => x.Exists, new DirectoryNotFoundException("Directory"));
            Directory.EnumerateFiles("*", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                     .Where(x => x.LastWriteTime < CompareDate)
                     .ForEach(x => x.Delete());
            return Directory;
        }

        #endregion

        #region DriveInfo

        /// <summary>
        /// Gets the drive information for a directory
        /// </summary>
        /// <param name="Directory">The directory to get the drive info of</param>
        /// <returns>The drive info connected to the directory</returns>
        public static DriveInfo DriveInfo(this DirectoryInfo Directory)
        {
            Directory.ThrowIfNull("Directory");
            return new DriveInfo(Directory.Root.FullName);
        }

        #endregion

        #region EnumerateFiles

        /// <summary>
        /// Enumerates the files within a directory
        /// </summary>
        /// <param name="Directory">Directory to search in</param>
        /// <param name="SearchPatterns">Patterns to search for</param>
        /// <param name="Options">Search options</param>
        /// <returns>The enumerated files from the directory</returns>
        public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo Directory, IEnumerable<string> SearchPatterns, SearchOption Options)
        {
            List<FileInfo> Files = new List<FileInfo>();
            foreach (string SearchPattern in SearchPatterns)
                Files.Add(Directory.EnumerateFiles(SearchPattern, Options));
            return Files;
        }

        #endregion

        #region Size

        /// <summary>
        /// Gets the size of all files within a directory
        /// </summary>
        /// <param name="Directory">Directory</param>
        /// <param name="SearchPattern">Search pattern used to tell what files to include (defaults to all)</param>
        /// <param name="Recursive">determines if this is a recursive call or not</param>
        /// <returns>The directory size</returns>
        public static long Size(this DirectoryInfo Directory,string SearchPattern="*", bool Recursive = false)
        {
            Directory.ThrowIfNull("Directory");
            return Directory.EnumerateFiles(SearchPattern, Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                            .Sum(x => x.Length);
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
            Directory.ThrowIfNull("Directory");
            Directory.EnumerateFiles()
                     .ForEach(x => x.SetAttributes(Attributes));
            if (Recursive)
                Directory.EnumerateDirectories().ForEach(x => x.SetAttributes(Attributes, true));
            return Directory;
        }

        #endregion
        
        #endregion
    }
}