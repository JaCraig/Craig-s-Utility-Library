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
using System.IO;
using System.Net;
using System.Text;
#endregion

namespace Utilities.Web.REST
{
    /// <summary>
    /// Class designed to help with calling REST based applications
    /// </summary>
    public class REST
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public REST()
        {
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Does a GET to the REST service
        /// </summary>
        /// <returns>a string containing the data returned by the service</returns>
        public string GET()
        {
            try
            {
                HttpWebRequest Request = WebRequest.Create(Url) as HttpWebRequest;
                Request.Method = "GET";
                Request.ContentType = "text/xml";
                SetupData(Request);
                SetupCredentials(Request);
                return SendRequest(Request);
            }
            catch { throw; }
        }

        /// <summary>
        /// Does a POST to the REST service
        /// </summary>
        /// <returns>a string containing the data returned by the service</returns>
        public string POST()
        {
            try
            {
                HttpWebRequest Request = WebRequest.Create(Url) as HttpWebRequest;
                Request.Method = "POST";
                Request.ContentType = "application/x-www-form-urlencoded";
                SetupData(Request);
                SetupCredentials(Request);
                return SendRequest(Request);
            }
            catch { throw; }
        }

        /// <summary>
        /// Does a DELETE on the REST service
        /// </summary>
        /// <returns>a string containing the data returned by the service</returns>
        public string DELETE()
        {
            try
            {
                HttpWebRequest Request = WebRequest.Create(Url) as HttpWebRequest;
                Request.Method = "DELETE";
                Request.ContentType = "application/x-www-form-urlencoded";
                SetupData(Request);
                SetupCredentials(Request);
                return SendRequest(Request);
            }
            catch { throw; }
        }

        /// <summary>
        /// Does a PUT on the REST service
        /// </summary>
        /// <returns>a string containing the data returned by the service</returns>
        public string PUT()
        {
            try
            {
                HttpWebRequest Request = WebRequest.Create(Url) as HttpWebRequest;
                Request.Method = "PUT";
                Request.ContentType = "application/x-www-form-urlencoded";
                SetupData(Request);
                SetupCredentials(Request);
                return SendRequest(Request);
            }
            catch { throw; }
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Sets up any data that needs to be sent
        /// </summary>
        /// <param name="Request">The web request object</param>
        private void SetupData(HttpWebRequest Request)
        {
            if (string.IsNullOrEmpty(Data))
            {
                Request.ContentLength = 0;
                return;
            }
            try
            {
                byte[] ByteData = UTF8Encoding.UTF8.GetBytes(Data);
                Request.ContentLength = ByteData.Length;
                using (Stream RequestStream = Request.GetRequestStream())
                {
                    RequestStream.Write(ByteData, 0, ByteData.Length);
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Sets up any credentials (basic authentication,
        /// for OAuth, please use the OAuth class to create the
        /// URL)
        /// </summary>
        /// <param name="Request">The web request object</param>
        private void SetupCredentials(HttpWebRequest Request)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                {
                    Request.Credentials = new NetworkCredential(UserName, Password);
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Sends the request to the URL specified
        /// </summary>
        /// <param name="Request">The web request object</param>
        /// <returns>The string returned by the service</returns>
        private string SendRequest(HttpWebRequest Request)
        {
            try
            {
                using (HttpWebResponse Response = Request.GetResponse() as HttpWebResponse)
                {
                    if (Response.StatusCode != HttpStatusCode.OK)
                        throw new Exception("The request did not complete successfully and returned status code " + Response.StatusCode);
                    using (StreamReader Reader = new StreamReader(Response.GetResponseStream()))
                    {
                        return Reader.ReadToEnd();
                    }
                }
            }
            catch { throw; }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// URL to send the request to
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Any data that needs to be appended to the request
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// User name (basic authentication)
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password (basic authentication
        /// </summary>
        public string Password { get; set; }

        #endregion
    }
}
