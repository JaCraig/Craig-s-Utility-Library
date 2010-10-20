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
using System.Text.RegularExpressions;
using Utilities.Web.OAuth;

#endregion

namespace Utilities.Web.Twitter
{
    /// <summary>
    /// Helper class to deal with Twitter
    /// </summary>
    public class Twitter : OAuth.OAuth
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Twitter()
            : base()
        {
            this.SignatureType = Signature.HMACSHA1;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Gets a request token/token secret
        /// </summary>
        /// <param name="Token">The request token</param>
        /// <param name="TokenSecret">The request token secret</param>
        public void GetRequestToken(out string Token, out string TokenSecret)
        {
            this.Token = "";
            this.TokenSecret = "";
            this.Method = HTTPMethod.GET;
            this.Url = new Uri("http://twitter.com/oauth/request_token");
            REST.REST RestHelper = new Utilities.Web.REST.REST();
            RestHelper.Url = new Uri(GenerateRequest());
            string Value = RestHelper.GET();
            Regex TokenRegex = new Regex("oauth_token=(?<Value>[^&]*)");
            Match TempToken = TokenRegex.Match(Value);
            Token = TempToken.Groups["Value"].Value;
            Regex TokenSecretRegex = new Regex("oauth_token_secret=(?<Value>[^&]*)");
            Match TempTokenSecret = TokenSecretRegex.Match(Value);
            TokenSecret = TempTokenSecret.Groups["Value"].Value;
        }

        /// <summary>
        /// Gets the location of the authorization site (requires
        /// request token/token secret)
        /// </summary>
        /// <returns>The location that the user must go in order to
        /// authorize the app and get the authorization PIN</returns>
        public string GetAuthorizationSite()
        {
            this.Method = HTTPMethod.GET;
            this.Url = new Uri("https://twitter.com/oauth/authorize");
            this.AddParameter("oauth_callback", "oob");
            return new Uri(GenerateRequest()).ToString();
        }

        /// <summary>
        /// Gets the access token/token secret which are used in actual calls
        /// (requires the PIN from the authorization site and the request token
        /// and request token secret)
        /// </summary>
        /// <param name="PIN">PIN received from the authorization site</param>
        /// <param name="AccessToken">The access token</param>
        /// <param name="AccessTokenSecret">The access token secret</param>
        public void GetAccessToken(string PIN, out string AccessToken, out string AccessTokenSecret)
        {
            this.Url = new Uri("https://twitter.com/oauth/access_token");
            this.AddParameter("oauth_verifier", PIN);
            this.Method = HTTPMethod.POST;
            REST.REST RestHelper = new Utilities.Web.REST.REST();
            RestHelper.Url = new Uri(GenerateRequest());
            string Value = RestHelper.POST();
            Regex TokenRegex = new Regex("oauth_token=(?<Value>[^&]*)");
            Match TempToken = TokenRegex.Match(Value);
            AccessToken = TempToken.Groups["Value"].Value;
            Regex TokenSecretRegex = new Regex("oauth_token_secret=(?<Value>[^&]*)");
            Match TempTokenSecret = TokenSecretRegex.Match(Value);
            AccessTokenSecret = TempTokenSecret.Groups["Value"].Value;
        }

        /// <summary>
        /// Updates the status of the user
        /// </summary>
        /// <param name="Status">Status of the user (needs to be within 140 characters)</param>
        /// <returns>The XML doc returned from the Twitter service</returns>
        public string UpdateStatus(string Status)
        {
            this.Method = HTTPMethod.POST;
            this.Url = new Uri("http://twitter.com/statuses/update.xml");
            this.AddParameter("status", Status);
            REST.REST RestHelper = new Utilities.Web.REST.REST();
            RestHelper.Url = new Uri(GenerateRequest());
            return RestHelper.POST();
        }

        /// <summary>
        /// Gets a user's timeline from the Twitter service
        /// </summary>
        /// <param name="UserName">The screen name of the user</param>
        /// <returns>The XML doc returned from the Twitter service
        /// contatining the timeline</returns>
        public string GetTimeline(string UserName)
        {
            this.Method = HTTPMethod.POST;
            this.Url = new Uri("http://twitter.com/statuses/user_timeline.xml");
            this.AddParameter("screen_name", UserName);
            REST.REST RestHelper = new Utilities.Web.REST.REST();
            RestHelper.Url = new Uri(GenerateRequest());
            return RestHelper.GET();
        }

        #endregion
    }
}