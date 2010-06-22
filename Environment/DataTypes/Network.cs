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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Management;
#endregion

namespace Utilities.Environment.DataTypes
{
    /// <summary>
    /// Represents network info
    /// </summary>
    public class Network
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Computer name</param>
        /// <param name="Password">Password</param>
        /// <param name="UserName">Username</param>
        public Network(string Name="",string UserName="",string Password="")
        {
            try
            {
                GetNetworkInfo(Name, UserName, Password);
            }
            catch { throw; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Network addresses (IP Addresses, etc.)
        /// </summary>
        public List<NetworkAddress> NetworkAddresses { get; set; }

        /// <summary>
        /// MAC Address 
        /// </summary>
        public string MACAddress { get; set; }

        #endregion

        #region Private Functions

        /// <summary>
        /// Gets the network info
        /// </summary>
        /// <param name="Name">Computer name</param>
        /// <param name="Password">Password</param>
        /// <param name="UserName">Username</param>
        private void GetNetworkInfo(string Name, string UserName, string Password)
        {
            try
            {
                NetworkAddresses = new List<NetworkAddress>();
                IPHostEntry HostEntry = Dns.GetHostEntry(Name);
                foreach (IPAddress Address in HostEntry.AddressList)
                {
                    NetworkAddress TempAddress = new NetworkAddress();
                    TempAddress.Type = Address.AddressFamily.ToString();
                    TempAddress.Address = Address.ToString();
                    NetworkAddresses.Add(TempAddress);
                }
            }
            catch { throw; }
        }

        private void GetNetworkAdapterInfo(string Name, string UserName, string Password)
        {
            try
            {
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
                ObjectQuery Query = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled=True");
                using (ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, Query))
                {
                    using (ManagementObjectCollection Collection = Searcher.Get())
                    {
                        foreach (ManagementObject TempNetworkAdapter in Collection)
                        {
                            MACAddress = TempNetworkAdapter.Properties["MacAddress"].Value.ToString();
                        }
                    }
                }
            }
            catch { throw; }
        }

        #endregion
    }
}
