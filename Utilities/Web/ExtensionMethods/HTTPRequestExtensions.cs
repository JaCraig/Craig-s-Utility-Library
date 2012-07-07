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
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Collections.Generic;
using System.Web;
using Utilities.DataTypes.ExtensionMethods;
using System;
#endregion

namespace Utilities.Web.ExtensionMethods
{
    /// <summary>
    /// Extensions for HttpRequest classes
    /// </summary>
    public static class HTTPRequestExtensions
    {
        #region Functions

        #region UserIPAddress

        /// <summary>
        /// Gets the user's IP address if it exists, null is returned otherwise
        /// </summary>
        /// <param name="Request">Request</param>
        /// <returns>The IPAddress object if it exists, null otherwise</returns>
        public static IPAddress UserIPAddress(this HttpRequestBase Request)
        {
            Request.ThrowIfNull("Request");
            IPAddress Address = null;
            if (!IPAddress.TryParse(Request.UserHostAddress, out Address))
                Address = null;
            return Address;
        }

        /// <summary>
        /// Gets the user's IP address if it exists, null is returned otherwise
        /// </summary>
        /// <param name="Request">Request</param>
        /// <returns>The IPAddress object if it exists, null otherwise</returns>
        public static IPAddress UserIPAddress(this HttpRequest Request)
        {
            Request.ThrowIfNull("Request");
            IPAddress Address = null;
            if (!IPAddress.TryParse(Request.UserHostAddress, out Address))
                Address = null;
            return Address;
        }

        #endregion

        #region IfModifiedSince

        /// <summary>
        /// Converts the If-Modified-Since header value to a DateTime object
        /// </summary>
        /// <param name="Request">Request</param>
        /// <returns>The If-Modified-Since header value expressed as a DateTime object</returns>
        public static DateTime IfModifiedSince(this HttpRequestBase Request)
        {
            DateTime Result = DateTime.MinValue;
            DateTime.TryParse(Request.Headers["If-Modified-Since"], out Result);
            return Result;
        }

        /// <summary>
        /// Converts the If-Modified-Since header value to a DateTime object
        /// </summary>
        /// <param name="Request">Request</param>
        /// <returns>The If-Modified-Since header value expressed as a DateTime object</returns>
        public static DateTime IfModifiedSince(this HttpRequest Request)
        {
            DateTime Result = DateTime.MinValue;
            DateTime.TryParse(Request.Headers["If-Modified-Since"], out Result);
            return Result;
        }

        #endregion

        #endregion
    }
}