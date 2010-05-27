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
using System;
using System.Management;
#endregion

namespace Utilities.Environment
{
    /// <summary>
    /// Class designed to give information
    /// about the current system
    /// </summary>
    public static class Environment
    {
        #region Public Static Properties
        /// <summary>
        /// Name of the machine running the app
        /// </summary>
        public static string MachineName
        {
            get { return System.Environment.MachineName; }
        }

        /// <summary>
        /// Gets the user name that the app is running under
        /// </summary>
        public static string UserName
        {
            get { return System.Environment.UserName; }
        }

        /// <summary>
        /// Name of the domain that the app is running under
        /// </summary>
        public static string DomainName
        {
            get { return System.Environment.UserDomainName; }
        }

        /// <summary>
        /// Name of the OS running
        /// </summary>
        public static string OSName
        {
            get { return System.Environment.OSVersion.Platform.ToString(); }
        }

        /// <summary>
        /// Version information about the OS running
        /// </summary>
        public static string OSVersion
        {
            get { return System.Environment.OSVersion.Version.ToString(); }
        }

        /// <summary>
        /// The service pack running on the OS
        /// </summary>
        public static string OSServicePack
        {
            get { return System.Environment.OSVersion.ServicePack; }
        }
        
        /// <summary>
        /// Full name, includes service pack, version, etc.
        /// </summary>
        public static string OSFullName
        {
            get { return System.Environment.OSVersion.VersionString; }
        }

        /// <summary>
        /// Gets the current stack trace information
        /// </summary>
        public static string StackTrace
        {
            get { return System.Environment.StackTrace; }
        }

        /// <summary>
        /// Returns the number of processors on the machine
        /// </summary>
        public static int NumberOfProcessors
        {
            get { return System.Environment.ProcessorCount; }
        }

        /// <summary>
        /// The total amount of memory the GC believes is used
        /// by the app in bytes
        /// </summary>
        public static long TotalMemoryUsed
        {
            get { return GC.GetTotalMemory(false); }
        }

        /// <summary>
        /// The total amount of memory that is available in bytes
        /// </summary>
        public static long TotalMemory
        {
            get 
            {
                long ReturnValue = 0;
                ObjectQuery TempQuery = new ObjectQuery("SELECT * FROM Win32_LogicalMemoryConfiguration");
                using (ManagementObjectSearcher Searcher = new ManagementObjectSearcher(TempQuery))
                {
                    foreach (ManagementObject TempObject in Searcher.Get())
                    {
                        ReturnValue = long.Parse(TempObject["TotalPhysicalMemory"].ToString()) * 1024;
                    }
                }
                return ReturnValue;
            }
        }
        #endregion
    }
}
