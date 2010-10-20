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
using System.Net.NetworkInformation;
using System.Text;
#endregion

namespace Utilities.Web.Ping
{
    /// <summary>
    /// Class used to ping another computer
    /// </summary>
    public static class Ping
    {
        #region Public Static Functions

        /// <summary>
        /// Does a ping against the host specified
        /// </summary>
        /// <param name="Address">Address of the host</param>
        /// <param name="TimeOut">Timeout in ms (optional)</param>
        /// <returns>True if a response is received, false otherwise</returns>
        public static bool PingHost(string Address, int TimeOut = 1000)
        {
            using (System.Net.NetworkInformation.Ping PingSender = new System.Net.NetworkInformation.Ping())
            {
                PingOptions Options = new PingOptions();
                Options.DontFragment = true;
                string Data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] DataBuffer = Encoding.ASCII.GetBytes(Data);
                PingReply Reply = PingSender.Send(Address, TimeOut, DataBuffer, Options);
                if (Reply.Status == IPStatus.Success)
                    return true;
                return false;
            }
        }

        #endregion
    }
}