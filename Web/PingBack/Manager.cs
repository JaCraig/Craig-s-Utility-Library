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
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
#endregion

namespace Utilities.Web.PingBack
{
    /// <summary>
    /// Handles ping backs, track backs, and pinging services
    /// </summary>
    public static class Manager
    {
        #region Public Static Functions
        /// <summary>
        /// Pings services such as technorati, etc.
        /// </summary>
        /// <param name="Services">List of services</param>
        /// <param name="Blog">URI of your blog</param>
        /// <param name="BlogName">Name of the blog</param>
        /// <param name="Threaded">If true this is done in a seperate thread,
        /// if false it will wait for it to end</param>
        public static void PingServices(List<Uri> Services,Uri Blog,string BlogName, bool Threaded)
        {
            try
            {
                if (Threaded)
                {
                    ThreadPool.QueueUserWorkItem(delegate { PingServices(Services, Blog, BlogName); });
                }
                else
                {
                    PingServices(Services, Blog, BlogName);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void PingServices(List<Uri> Services, Uri Blog, string BlogName)
        {
            foreach (Uri Service in Services)
            {
                try
                {
                    HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Service);
                    Request.Credentials = CredentialCache.DefaultNetworkCredentials;
                    Request.ContentType = "text/xml";
                    Request.Method = "POST";
                    Request.Timeout = 10000;
                    Stream Stream = (Stream)Request.GetRequestStream();
                    using (XmlTextWriter XMLWriter = new XmlTextWriter(Stream, Encoding.ASCII))
                    {
                        XMLWriter.WriteStartDocument();
                        XMLWriter.WriteStartElement("methodCall");
                        XMLWriter.WriteElementString("methodName", "weblogUpdates.ping");
                        XMLWriter.WriteStartElement("params");
                        XMLWriter.WriteStartElement("param");
                        XMLWriter.WriteElementString("value", BlogName);
                        XMLWriter.WriteEndElement();
                        XMLWriter.WriteStartElement("param");
                        XMLWriter.WriteElementString("value", Blog.ToString());
                        XMLWriter.WriteEndElement();
                        XMLWriter.WriteEndElement();
                        XMLWriter.WriteEndElement();
                    }
                    Request.GetResponse();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        #endregion
    }
}
