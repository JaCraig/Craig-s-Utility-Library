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
using System.Management;
#endregion

namespace Utilities.Environment.DataTypes
{
    /// <summary>
    /// Holds operating system info
    /// </summary>
    public class OperatingSystem
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Computer name</param>
        /// <param name="Password">Password</param>
        /// <param name="UserName">Username</param>
        public OperatingSystem(string Name = "", string UserName = "", string Password = "")
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name");
            GetOperatingSystemInfo(Name, UserName, Password);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Last bootup time
        /// </summary>
        public virtual DateTime LastBootUpTime { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Gets operating system info
        /// </summary>
        /// <param name="Name">Computer name</param>
        /// <param name="UserName">User name</param>
        /// <param name="Password">Password</param>
        protected virtual void GetOperatingSystemInfo(string Name, string UserName, string Password)
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name");
            ManagementScope Scope = null;
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                ConnectionOptions Options = new ConnectionOptions();
                Options.Username = UserName;
                Options.Password = Password;
                Scope = new ManagementScope("\\\\" + Name + "\\root\\cimv2", Options);
            }
            else
            {
                Scope = new ManagementScope("\\\\" + Name + "\\root\\cimv2");
            }
            Scope.Connect();
            ObjectQuery Query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            using (ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, Query))
            {
                using (ManagementObjectCollection Collection = Searcher.Get())
                {
                    foreach (ManagementObject TempNetworkAdapter in Collection)
                    {
                        if (TempNetworkAdapter.Properties["LastBootUpTime"].Value != null)
                        {
                            LastBootUpTime = ManagementDateTimeConverter.ToDateTime(TempNetworkAdapter.Properties["LastBootUpTime"].Value.ToString());
                        }
                    }
                }
            }
        }

        #endregion
    }
}