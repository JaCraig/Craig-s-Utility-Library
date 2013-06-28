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
using System.Globalization;
using System.Web;
#endregion

namespace Utilities.Web.Akismet
{
    /// <summary>
    /// Akismet helper class
    /// </summary>
    public class Akismet:REST.REST
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Key">API Key</param>
        /// <param name="Site">Site using Akismet</param>
        public Akismet(string Key, string Site)
            : base()
        {
            this.Key = Key;
            this.Website = Site;
            this.VerifyKeyData = string.Format(CultureInfo.InvariantCulture,"key={0}&blog={1}", Key, HttpUtility.UrlEncode(Website));
            this.CommentCheckUrl = new Uri(string.Format(CultureInfo.InvariantCulture,"http://{0}.rest.akismet.com/1.1/comment-check", Key));
            this.CommentCheckData = "blog=" + HttpUtility.UrlEncode(Website) + "&user_ip={0}&user_agent={1}&referrer={2}" +
                "&permalink={3}&comment_type={4}&comment_author={5}&comment_author_email={6}&" +
                "comment_author_url={7}&comment_content={8}";
            this.SubmitSpamUrl = new Uri(string.Format(CultureInfo.InvariantCulture,"http://{0}.rest.akismet.com/1.1/submit-spam", Key));
            this.SubmitHamUrl = new Uri(string.Format(CultureInfo.InvariantCulture,"http://{0}.rest.akismet.com/1.1/submit-ham", Key));
        }

        #endregion

        #region Properties

        /// <summary>
        /// API Key
        /// </summary>
        protected virtual string Key { get; set; }

        /// <summary>
        /// Website
        /// </summary>
        protected virtual string Website { get; set; }

        /// <summary>
        /// Data sent to verify the API key
        /// </summary>
        protected virtual string VerifyKeyData { get; set; }

        /// <summary>
        /// Comment check url
        /// </summary>
        protected virtual Uri CommentCheckUrl { get; set; }

        /// <summary>
        /// Submit spam url
        /// </summary>
        protected virtual Uri SubmitSpamUrl { get; set; }

        /// <summary>
        /// Submit ham url
        /// </summary>
        protected virtual Uri SubmitHamUrl { get; set; }

        /// <summary>
        /// Comment check data string
        /// </summary>
        protected virtual string CommentCheckData { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Verifies the Key
        /// </summary>
        /// <returns>True if the key is valid, false otherwise</returns>
        public virtual bool VerifyKey()
        {
            this.Url = new Uri("http://rest.akismet.com/1.1/verify-key");
            this.Data = VerifyKeyData;
            return POST() == "valid";
        }

        /// <summary>
        /// Checks if a comment is spam or ham
        /// </summary>
        /// <param name="Comment">Comment to check</param>
        /// <returns>True if it is spam, false otherwise</returns>
        public virtual bool IsSpam(Comment Comment)
        {
            this.Url = CommentCheckUrl;
            this.Data = SetupData(Comment);
            return bool.Parse(POST());
        }

        /// <summary>
        /// Submits a spam message
        /// </summary>
        /// <param name="Comment">Comment to submit</param>
        public virtual void SubmitSpam(Comment Comment)
        {
            this.Url = SubmitSpamUrl;
            this.Data = SetupData(Comment);
            POST();
        }

        /// <summary>
        /// Submits a ham message
        /// </summary>
        /// <param name="Comment">Comment to submit</param>
        public virtual void SubmitHam(Comment Comment)
        {
            this.Url = SubmitHamUrl;
            this.Data = SetupData(Comment);
            POST();
        }

        private string SetupData(Comment Comment)
        {
            return string.Format(CultureInfo.InvariantCulture,this.CommentCheckData,
                 HttpUtility.UrlEncode(Comment.UserIP),
                 HttpUtility.UrlEncode(Comment.UserAgent),
                 HttpUtility.UrlEncode(Comment.Referrer),
                 HttpUtility.UrlEncode(Comment.Permalink),
                 HttpUtility.UrlEncode(Comment.CommentType),
                 HttpUtility.UrlEncode(Comment.UserName),
                 HttpUtility.UrlEncode(Comment.UserEmail),
                 HttpUtility.UrlEncode(Comment.UserUrl),
                 HttpUtility.UrlEncode(Comment.Content));
        }

        #endregion
    }
}
