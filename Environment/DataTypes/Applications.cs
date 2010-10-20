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
using System.Collections;
using System.Collections.Generic;
using System.Management;
#endregion

namespace Utilities.Environment.DataTypes
{
    /// <summary>
    /// Application list
    /// </summary>
    public class Applications : IEnumerable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Computer Name</param>
        /// <param name="Password">Password</param>
        /// <param name="UserName">User Name</param>
        public Applications(string Name = "", string UserName = "", string Password = "")
        {
            LoadApplications(Name, UserName, Password);
        }

        #endregion

        #region Properties

        public List<Application> ApplicationList { get; set; }

        #endregion

        #region Private Functions

        /// <summary>
        /// Loads applications
        /// </summary>
        /// <param name="Name">Computer name</param>
        /// <param name="UserName">User name</param>
        /// <param name="Password">Password</param>
        private void LoadApplications(string Name, string UserName, string Password)
        {
            ApplicationList = new List<Application>();
            ManagementScope Scope = null;
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                ConnectionOptions Options = new ConnectionOptions();
                Options.Username = UserName;
                Options.Password = Password;
                Scope = new ManagementScope("\\\\" + Name + "\\root\\default", Options);
            }
            else
            {
                Scope = new ManagementScope("\\\\" + Name + "\\root\\default");
            }
            ManagementPath Path = new ManagementPath("StdRegProv");
            using (ManagementClass Registry = new ManagementClass(Scope, Path, null))
            {
                const uint HKEY_LOCAL_MACHINE = unchecked((uint)0x80000002);
                object[] Args = new object[] { HKEY_LOCAL_MACHINE, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", null };
                uint MethodValue = (uint)Registry.InvokeMethod("EnumKey", Args);
                string[] Keys = Args[2] as String[];
                using (ManagementBaseObject MethodParams = Registry.GetMethodParameters("GetStringValue"))
                {
                    MethodParams["hDefKey"] = HKEY_LOCAL_MACHINE;
                    foreach (string SubKey in Keys)
                    {
                        MethodParams["sSubKeyName"] = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + SubKey;
                        MethodParams["sValueName"] = "DisplayName";
                        using (ManagementBaseObject Results = Registry.InvokeMethod("GetStringValue", MethodParams, null))
                        {
                            if (Results != null && (uint)Results["ReturnValue"] == 0)
                            {
                                Application Temp = new Application();
                                Temp.Name = Results["sValue"].ToString();
                                ApplicationList.Add(Temp);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region IEnumerable

        public IEnumerator GetEnumerator()
        {
            return ApplicationList.GetEnumerator();
        }

        #endregion
    }
}