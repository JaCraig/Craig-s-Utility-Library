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
using System.Web;
using System.Xml;
#endregion

namespace Utilities.Web.PingBack
{
    /// <summary>
    /// Handles ping backs
    /// </summary>
    public static class PingBack
    {
        #region Public Static Functions
        /// <summary>
        /// Sends a ping back
        /// </summary>
        /// <param name="Message">Message to send</param>
        public static void SendPingBack(PingBackMessage Message)
        {
            if (string.IsNullOrEmpty(Message.Source) || string.IsNullOrEmpty(Message.Target))
                return;

            try
            {
                HttpWebRequest Request = (HttpWebRequest)HttpWebRequest.Create(Message.Target);
                Request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
                string PingURL = (!string.IsNullOrEmpty(Response.Headers["x-pingback"])) ? Response.Headers["x-pingback"] : Response.Headers["pingback"];
                Uri URIUsing;
                if (!string.IsNullOrEmpty(PingURL) && Uri.TryCreate(PingURL, UriKind.Absolute, out URIUsing))
                {
                    Request = (HttpWebRequest)HttpWebRequest.Create(URIUsing);
                    Request.Method = "POST";
                    Request.Timeout = 10000;
                    Request.ContentType = "text/xml";
                    Request.ProtocolVersion = HttpVersion.Version11;
                    Request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 6.0)";
                    Stream StreamUsing = (Stream)Request.GetRequestStream();
                    using (XmlTextWriter Writer = new XmlTextWriter(StreamUsing, Encoding.ASCII))
                    {
                        Writer.WriteStartDocument(true);
                        Writer.WriteStartElement("methodCall");
                        Writer.WriteElementString("methodName", "pingback.ping");
                        Writer.WriteStartElement("params");
                        Writer.WriteStartElement("param");
                        Writer.WriteStartElement("value");
                        Writer.WriteElementString("string", Message.Source);
                        Writer.WriteEndElement();
                        Writer.WriteEndElement();
                        Writer.WriteStartElement("param");
                        Writer.WriteStartElement("value");
                        Writer.WriteElementString("string", Message.Target);
                        Writer.WriteEndElement();
                        Writer.WriteEndElement();
                        Writer.WriteEndElement();
                        Writer.WriteEndElement();
                    }
                    Request.GetResponse();
                    StreamUsing.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets a ping back
        /// </summary>
        /// <param name="Context">The HttpContext for this item</param>
        /// <returns>The ping back message</returns>
        public static PingBackMessage GetPingBack(HttpContext Context)
        {
            try
            {
                return GetPingBack(Context.Request);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets a ping back
        /// </summary>
        /// <param name="Request">The HttpRequest for this item</param>
        /// <returns>The ping back message</returns>
        public static PingBackMessage GetPingBack(HttpRequest Request)
        {
            try
            {
                PingBackMessage TempMessage = new PingBackMessage();
                TempMessage.Source = "";
                TempMessage.Target = "";
                string RequestText = GetRequest(Request);
                if (!RequestText.Contains("<methodName>pingback.ping</methodName>"))
                {
                    return TempMessage;
                }
                XmlDocument XMLDocument = new XmlDocument();
                XMLDocument.LoadXml(RequestText);
                XmlNodeList Nodes = XMLDocument.SelectNodes("methodCall/params/param/value/string");
                if (Nodes == null)
                {
                    Nodes = XMLDocument.SelectNodes("methodCall/params/param/value");
                }
                if (Nodes != null)
                {
                    TempMessage.Source = Nodes[0].InnerText.Trim();
                    TempMessage.Target = Nodes[1].InnerText.Trim();
                }
                return TempMessage;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Sends a success message.
        /// </summary>
        /// <param name="Context">HttpContext of the item</param>
        public static void SendSuccess(HttpContext Context)
        {
            SendSuccess(Context.Response);
        }
        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <param name="Context">Context of the item</param>
        /// <param name="Code">Error code</param>
        /// <param name="ErrorMessage">Error Message</param>
        public static void SendError(HttpContext Context, int Code, string ErrorMessage)
        {
            SendError(Context.Response, Code, ErrorMessage);
        }

        /// <summary>
        /// Sends a success message.
        /// </summary>
        /// <param name="Response">Response for the item</param>
        public static void SendSuccess(HttpResponse Response)
        {
            Response.Write("<methodResponse><params><param><value><string>Success</string></value></param></params></methodResponse>");
        }

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <param name="Response">Response object</param>
        /// <param name="Code">Error code</param>
        /// <param name="ErrorMessage">Error message</param>
        public static void SendError(HttpResponse Response, int Code, string ErrorMessage)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<?xml version=\"1.0\"?><methodResponse><fault><value><struct><member><name>faultCode</name>");
            Builder.Append("<value><int>" + Code + "</int></value></member><member><name>faultString</name>");
            Builder.Append("<value><string>" + ErrorMessage + "</string></value></member></struct></value></fault></methodResponse>");
            Response.Write(Builder.ToString());
        }
        #endregion

        #region Private Functions
        private static string GetRequest(HttpContext Context)
        {
            return GetRequest(Context.Request);
        }

        private static string GetRequest(HttpRequest Request)
        {
            byte[] TempBuffer = new byte[Request.InputStream.Length];
            Request.InputStream.Read(TempBuffer, 0, TempBuffer.Length);
            return System.Text.Encoding.Default.GetString(TempBuffer);
        }
        #endregion
    }

    /// <summary>
    /// Message class used for pingbacks
    /// </summary>
    public class PingBackMessage
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PingBackMessage()
        {
        }
        #endregion

        #region Private Variables
        private string _Source="";
        private string _Target="";
        #endregion

        #region Public Properties
        /// <summary>
        /// Source Location
        /// </summary>
        public string Source
        {
            get { return _Source; }
            set { _Source = value; }
        }

        /// <summary>
        /// Target location
        /// </summary>
        public string Target
        {
            get { return _Target; }
            set { _Target = value; }
        }
        #endregion
    }
}
