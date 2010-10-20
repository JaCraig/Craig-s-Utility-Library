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
using System.Text.RegularExpressions;
using System.Web;

#endregion

namespace Utilities.Web.PingBack
{
    /// <summary>
    /// Handles track backs
    /// </summary>
    public static class TrackBack
    {
        #region Public Static Functions

        /// <summary>
        /// Sends a track back message
        /// </summary>
        /// <param name="Message">Message to send</param>
        public static void SendTrackBack(TrackBackMessage Message)
        {
            if (Message == null)
                throw new ArgumentException("Message can not be null.");

            GetTrackBackURL(Message);
            HttpWebRequest Request = (HttpWebRequest)System.Net.HttpWebRequest.Create(Message.NotificationURL);
            Request.Credentials = CredentialCache.DefaultNetworkCredentials;
            Request.Timeout = 10000;
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.ContentLength = Message.ToString().Length;
            Request.KeepAlive = false;
            using (StreamWriter Writer = new StreamWriter(Request.GetRequestStream()))
            {
                Writer.Write(Message.ToString());
            }
            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
            string ResponseText = "";
            using (System.IO.StreamReader Reader = new System.IO.StreamReader(Response.GetResponseStream()))
            {
                ResponseText = Reader.ReadToEnd();
            }
            if (Response.StatusCode == HttpStatusCode.OK)
            {
                if (!ResponseText.Contains("<error>0</error>"))
                {
                    throw new Exception(ResponseText);
                }
            }
            else
            {
                throw new Exception("HTTP Error occurred: " + Response.StatusCode.ToString());
            }
        }

        /// <summary>
        /// Gets a trackback message
        /// </summary>
        /// <param name="Context">Context object</param>
        /// <returns>A trackback message</returns>
        public static TrackBackMessage GetTrackBack(HttpContext Context)
        {
            return GetTrackBack(Context.Request);
        }

        /// <summary>
        /// Gets a trackback message
        /// </summary>
        /// <param name="Request">Request object</param>
        /// <returns>A trackback message</returns>
        public static TrackBackMessage GetTrackBack(HttpRequest Request)
        {
            TrackBackMessage Message = new TrackBackMessage();
            Message.Title = Request.Params["title"];
            Message.ID = Request.Params["id"];
            Message.Excerpt = Request.Params["excerpt"];
            Message.BlogName = Request.Params["blog_name"];
            if (Request.Params["url"] != null)
                Message.PostUrl = new Uri(Request.Params["url"].Split(',')[0]);
            return Message;
        }

        /// <summary>
        /// Send a success message
        /// </summary>
        /// <param name="Context">Context object</param>
        public static void SendSuccess(HttpContext Context)
        {
            SendSuccess(Context.Response);
        }

        /// <summary>
        /// Send a success message
        /// </summary>
        /// <param name="Response">Response object</param>
        public static void SendSuccess(HttpResponse Response)
        {
            Response.Write("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><response><error>0</error></response>");
        }

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <param name="Context">Context Object</param>
        /// <param name="ErrorMessage">Error message to send</param>
        public static void SendError(HttpContext Context, string ErrorMessage)
        {
            SendError(Context.Response, ErrorMessage);
        }

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <param name="Response">Response Object</param>
        /// <param name="ErrorMessage">Error message to send</param>
        public static void SendError(HttpResponse Response, string ErrorMessage)
        {
            Response.Write("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><response><error>" + ErrorMessage + "</error></response>");
        }

        #endregion

        #region Private Static Functions

        private static readonly Regex TrackBackLink = new Regex("trackback:ping=\"([^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex TrackBackID = new Regex("id=([^\"]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static void GetTrackBackURL(TrackBackMessage Message)
        {
            using (WebClient Client = new WebClient())
            {
                Client.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)");
                string TempContent = Client.DownloadString(Message.NotificationURL);
                string TempURL = TrackBackLink.Match(TempContent).Groups[1].ToString().Trim();
                Uri TempURI;
                Uri.TryCreate(TempURL, UriKind.Absolute, out TempURI);
                Message.NotificationURL = TempURI;

                TempURL = TrackBackID.Match(TempURL).Groups[1].ToString().Trim();
                Message.ID = TempURL;
            }
        }

        #endregion
    }

    /// <summary>
    /// Track back message class
    /// </summary>
    public class TrackBackMessage
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public TrackBackMessage()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// ID of the item (most likely this is not set)
        /// </summary>
        public object ID { get; set; }

        /// <summary>
        /// Title of the post linking to you
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// URL of the post linking to you
        /// </summary>
        public Uri PostUrl { get; set; }

        /// <summary>
        /// Excerpt from the post linking to you
        /// </summary>
        public string Excerpt { get; set; }

        /// <summary>
        /// Name of the blog linking to you
        /// </summary>
        public string BlogName { get; set; }

        /// <summary>
        /// URL to send the message notification to.
        /// Only used for sending the message and filled in
        /// automatically.
        /// </summary>
        public Uri NotificationURL { get; set; }

        #endregion

        #region Public Overridden Functions

        /// <summary>
        /// Writes out the message (used for sending)
        /// </summary>
        /// <returns>A string with the message information</returns>
        public override string ToString()
        {
            string First = "&";
            if (string.IsNullOrEmpty(NotificationURL.Query))
                First = "?";
            return First + "title=" + Title + "&url=" + PostUrl + "&excerpt=" + Excerpt + "&blog_name=" + BlogName;
        }

        #endregion
    }
}