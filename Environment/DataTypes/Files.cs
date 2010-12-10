/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Management;
using System.Collections.Generic;
#endregion

namespace Utilities.Environment.DataTypes
{
    /// <summary>
    /// Helper class for searching for files (will be modified later)
    /// </summary>
    public static class Files
    {
        #region Public Static Functions

        /// <summary>
        /// Gets a list of files on the machine with a specific extension
        /// </summary>
        /// <param name="Computer">Computer to search</param>
        /// <param name="UserName">User name (if not local)</param>
        /// <param name="Password">Password (if not local)</param>
        /// <param name="Extension">File extension to look for</param>
        /// <returns>List of files that are found to have the specified extension</returns>
        public static List<string> GetFilesWithExtension(string Computer, string UserName, string Password, string Extension)
        {
            List<string> Files = new List<string>();
            ManagementScope Scope = null;
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                ConnectionOptions Options = new ConnectionOptions();
                Options.Username = UserName;
                Options.Password = Password;
                Scope = new ManagementScope("\\\\" + Computer + "\\root\\cimv2", Options);
            }
            else
            {
                Scope = new ManagementScope("\\\\" + Computer + "\\root\\cimv2");
            }
            Scope.Connect();
            ObjectQuery Query = new ObjectQuery("SELECT * FROM CIM_DataFile where Extension='" + Extension + "'");
            using (ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, Query))
            {
                using (ManagementObjectCollection Collection = Searcher.Get())
                {
                    foreach (ManagementObject TempBIOS in Collection)
                    {
                        Files.Add(TempBIOS.Properties["Drive"].Value.ToString()+TempBIOS.Properties["Path"].Value.ToString()+TempBIOS.Properties["FileName"].Value.ToString());
                    }
                }
            }
            return Files;
        }

        #endregion
    }
}
