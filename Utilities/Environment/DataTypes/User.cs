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
using System.Collections.Generic;
using System.Management;
using System.Text.RegularExpressions;
#endregion

namespace Utilities.Environment.DataTypes
{
    /// <summary>
    /// Holds user data
    /// </summary>
    public class User
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Computer name</param>
        /// <param name="Password">Password</param>
        /// <param name="UserName">Username</param>
        public User(string Name = "", string UserName = "", string Password = "")
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name");
            GetCurrentUser(Name, UserName, Password);
        }

        #endregion

        #region Properties

        /// <summary>
        /// User names
        /// </summary>
        public virtual List<string> UserNames { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <param name="Name">Computer name</param>
        /// <param name="UserName">User name</param>
        /// <param name="Password">Password</param>
        protected virtual void GetCurrentUser(string Name, string UserName, string Password)
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
            ObjectQuery Query = new ObjectQuery("SELECT * FROM Win32_LoggedOnUser");
            List<string> TempUserNames = new List<string>();
            using (ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, Query))
            {
                using (ManagementObjectCollection Collection = Searcher.Get())
                {
                    foreach (ManagementObject TempObject in Collection)
                    {
                        TempUserNames.Add(TempObject["Antecedent"].ToString());
                    }
                }
            }
            foreach (string TempName in UserNames)
            {
                if (Regex.IsMatch(TempName, "Domain=\"(?<Domain>.*)\",Name=\"(?<Name>.*)\""))
                {
                    Match Value = Regex.Match(TempName, "Domain=\"(?<Domain>.*)\",Name=\"(?<Name>.*)\"");
                    UserNames.Add(Value.Groups["Domain"].Value + "\\" + Value.Groups["Name"].Value);
                }
            }
        }

        #endregion
    }
}