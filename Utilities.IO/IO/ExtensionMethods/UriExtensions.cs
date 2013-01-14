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
using System.Net;

#endregion

namespace Utilities.IO.ExtensionMethods
{
    /// <summary>
    /// Uri Extension methods
    /// </summary>
    public static class UriExtensions
    {
        #region Execute

        /// <summary>
        /// opens the URL in a browser
        /// </summary>
        /// <param name="URL">URL to execute</param>
        /// <returns>The process object created when opening the URL</returns>
        public static System.Diagnostics.Process Execute(this Uri URL)
        {
            if (URL == null)
                throw new ArgumentNullException("URL");
            return System.Diagnostics.Process.Start(URL.ToString());
        }

        #endregion

        #region Read

        /// <summary>
        /// Reads the text content of a URL
        /// </summary>
        /// <param name="URL">Uri to read the content of</param>
        /// <param name="UserName">User name used in network credentials</param>
        /// <param name="Password">Password used in network credentials</param>
        /// <param name="Domain">Domain to use in network credentials</param>
        /// <param name="AuthenticationType">Authentication type to use in network credentials</param>
        /// <returns>String representation of the content of the URL</returns>
        public static string Read(this Uri URL, string UserName = "", string Password = "", string Domain = "", string AuthenticationType = "")
        {
            if (URL == null)
                throw new ArgumentNullException("URL");
            using (WebClient Client = new WebClient())
            {
                using (StreamReader Reader = new StreamReader(URL.Read(Client, UserName, Password,Domain,AuthenticationType)))
                {
                    string Contents = Reader.ReadToEnd();
                    return Contents;
                }
            }
        }

        /// <summary>
        /// Reads the text content of a URL
        /// </summary>
        /// <param name="URL">The Uri to read the content of</param>
        /// <param name="Client">WebClient used to load the data</param>
        /// <param name="UserName">User name used in network credentials</param>
        /// <param name="Password">Password used in network credentials</param>
        /// <param name="Domain">Domain to use in network credentials</param>
        /// <param name="AuthenticationType">Authentication type to use in network credentials</param>
        /// <returns>Stream containing the content of the URL</returns>
        public static Stream Read(this Uri URL, WebClient Client, string UserName = "", string Password = "", string Domain = "",string AuthenticationType="")
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {

                NetworkCredential Credentials=!string.IsNullOrEmpty(Domain)?new NetworkCredential(UserName, Password, Domain):new NetworkCredential(UserName, Password);
                CredentialCache Cache = new CredentialCache();
                if (!string.IsNullOrEmpty(AuthenticationType))
                {
                    Cache.Add(URL, AuthenticationType, Credentials);
                    Client.Credentials = Cache;
                }
                else
                {
                    Client.Credentials = Credentials;
                }
            }
            return Client.OpenRead(URL);
        }

        #endregion

        #region ReadBinary

        /// <summary>
        /// Reads the content of a URL
        /// </summary>
        /// <param name="URL">Uri to read the content of</param>
        /// <param name="UserName">User name used in network credentials</param>
        /// <param name="Password">Password used in network credentials</param>
        /// <param name="Domain">Domain to use in network credentials</param>
        /// <param name="AuthenticationType">Authentication type to use in network credentials</param>
        /// <returns>Byte array representation of the content of the URL</returns>
        public static byte[] ReadBinary(this Uri URL, string UserName = "", string Password = "", string Domain = "",string AuthenticationType="")
        {
            if (URL == null)
                throw new ArgumentNullException("URL");
            using (WebClient Client = new WebClient())
            {
                using (Stream Reader = URL.Read(Client, UserName, Password, Domain, AuthenticationType))
                {
                    using (MemoryStream FinalStream = new MemoryStream())
                    {
                        while (true)
                        {
                            byte[] Buffer = new byte[1024];
                            int Count = Reader.Read(Buffer, 0, Buffer.Length);
                            if (Count == 0)
                                break;
                            FinalStream.Write(Buffer, 0, Count);
                        }
                        byte[] ReturnValue = FinalStream.ToArray();
                        return ReturnValue;
                    }
                }
            }
        }

        #endregion
    }
}