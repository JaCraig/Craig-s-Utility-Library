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
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Utilities.DataTypes;
using Utilities.Web.OpenID.Extensions;
using Utilities.Web.OpenID.Extensions.Interfaces;
#endregion

namespace Utilities.Web.OpenID
{
    /// <summary>
    /// OpenID helper
    /// </summary>
    public class OpenID
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenID()
        {
            Extensions = new System.Collections.Generic.List<IExtension>();
            Extensions.Add(new AttributeExchange());
            if (NonceList == null)
                NonceList = new System.Collections.Generic.List<string>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Extensions list
        /// </summary>
        public System.Collections.Generic.List<IExtension> Extensions { get; set; }

        /// <summary>
        /// Redirect URL
        /// </summary>
        public string RedirectURL { get; set; }

        /// <summary>
        /// Server URL
        /// </summary>
        public string ServerURL { get; set; }

        /// <summary>
        /// Endpoint URL
        /// </summary>
        protected string EndpointURL { get; set; }

        #endregion

        #region Public Functions

        /// <summary>
        /// Generates login URL
        /// </summary>
        /// <returns>The login URL based on request</returns>
        public string GenerateLoginURL()
        {
            EndpointURL = GetServerURL();
            return CreateLoginRedirectUrl();
        }

        /// <summary>
        /// Gets attributes returned by the provider
        /// </summary>
        /// <param name="URL">URL</param>
        /// <returns></returns>
        public System.Collections.Generic.List<Pair<string, string>> GetAttributes(string URL)
        {
            System.Collections.Generic.List<Pair<string, string>> Pairs = new System.Collections.Generic.List<Pair<string, string>>();
            MatchCollection Matches = Keys.Matches(URL);
            foreach (Match Match in Matches)
            {
                Pairs.Add(new Pair<string, string>(Match.Groups["Left"].Value, Match.Groups["Right"].Value));
            }
            if (Verify(URL, Pairs))
                return Pairs;
            return null;
        }

        #endregion

        #region Protected Functions

        /// <summary>
        /// Creates a redirect URL for login requests
        /// </summary>
        /// <returns>A redirect URL</returns>
        protected string CreateLoginRedirectUrl()
        {
            System.Collections.Generic.List<Pair<string, string>> Pairs = new System.Collections.Generic.List<Pair<string, string>>();
            Pairs.Add(new Pair<string, string>("openid.ns", HttpUtility.UrlEncode("http://specs.openid.net/auth/2.0")));
            Pairs.Add(new Pair<string, string>("openid.mode", "checkid_setup"));
            Pairs.Add(new Pair<string, string>("openid.identity", HttpUtility.UrlEncode("http://specs.openid.net/auth/2.0/identifier_select")));
            Pairs.Add(new Pair<string, string>("openid.claimed_id", HttpUtility.UrlEncode("http://specs.openid.net/auth/2.0/identifier_select")));
            Pairs.Add(new Pair<string, string>("openid.return_to", HttpUtility.UrlEncode(RedirectURL)));
            foreach (IExtension Extension in Extensions)
            {
                Pairs.AddRange(Extension.GenerateURLAttributes());
            }
            StringBuilder Builder = new StringBuilder();
            Builder.Append(EndpointURL);
            string Splitter = "?";
            foreach (Pair<string, string> Pair in Pairs)
            {
                Builder.Append(Splitter).Append(Pair.Left).Append("=").Append(Pair.Right);
                Splitter = "&";
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Get server endpoint URL
        /// </summary>
        /// <returns>Endpoint URL</returns>
        protected string GetServerURL()
        {
            if (!string.IsNullOrEmpty(EndpointURL))
                return EndpointURL;
            using (WebClient Client = new WebClient())
            {
                string Html = Client.DownloadString(ServerURL);

                foreach (Match Match in Links.Matches(Html))
                {
                    string Temp = GetLink(Match);
                    if (!string.IsNullOrEmpty(Temp))
                        return Temp;
                }
                Match TempURI = Uri.Match(Html);
                if (TempURI != null)
                    return TempURI.Groups["URI"].Value;
                return "";
            }
        }

        /// <summary>
        /// Gets the link to the end point
        /// </summary>
        /// <param name="Match">Match found</param>
        /// <returns>The end point or empty string</returns>
        protected static string GetLink(Match Match)
        {
            if (Match.Value.IndexOf("openid.server") > 0)
            {
                Match = Href.Match(Match.Value);
                if (Match.Success)
                {
                    return Match.Groups[1].Value;
                }
            }
            return "";
        }

        /// <summary>
        /// Verifies the request
        /// </summary>
        /// <param name="URL">URL returned</param>
        /// <param name="Pairs">Individual attribute pairs</param>
        /// <returns>true if it is valid, false otherwise</returns>
        protected bool Verify(string URL, System.Collections.Generic.List<Pair<string, string>> Pairs)
        {
            Pair<string, string> Mode = Pairs.Find(x => x.Left.Equals("openid.mode", StringComparison.CurrentCultureIgnoreCase));
            if (Mode.Right.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
                return false;
            Pair<string, string> ReturnTo = Pairs.Find(x => x.Left.Equals("openid.return_to", StringComparison.CurrentCultureIgnoreCase));
            if (ReturnTo == null)
                return false;
            Match PartialUrl = Regex.Match(URL, "^" + ReturnTo.Right, RegexOptions.IgnoreCase);
            if (!PartialUrl.Success)
                return false;
            if (Pairs.Find(x => x.Left.Equals("openid.identity", StringComparison.CurrentCultureIgnoreCase)) == null
                || Pairs.Find(x => x.Left.Equals("openid.claimed_id", StringComparison.CurrentCultureIgnoreCase)) == null)
                return false;
            foreach (IExtension Extension in Extensions)
            {
                if (!Extension.Verify(URL, Pairs))
                    return false;
            }
            Pair<string, string> Nonce = Pairs.Find(x => x.Left.Equals("openid.response_nonce", StringComparison.CurrentCultureIgnoreCase));
            if (Nonce == null)
                return false;
            string CurrentNonce = Nonce.Right;
            foreach (string TempNonce in NonceList)
            {
                if (CurrentNonce == TempNonce)
                    return false;
            }
            NonceList.Add(CurrentNonce);
            string VerificationURL = GenerateVerificationUrl(Pairs);
            using (WebClient Client = new WebClient())
            {
                string Html = Client.DownloadString(VerificationURL);
                if (!Html.Contains("is_valid:true"))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Generates a verification URL
        /// </summary>
        /// <param name="Pairs">The individual attribute pairs</param>
        /// <returns>The appropriate verification URL</returns>
        protected string GenerateVerificationUrl(System.Collections.Generic.List<Pair<string, string>> Pairs)
        {
            Pairs.Find(x => x.Left.Equals("openid.mode", StringComparison.CurrentCultureIgnoreCase)).Right = "check_authentication";
            StringBuilder Builder = new StringBuilder();
            Builder.Append(Pairs.Find(x => x.Left.Equals("openid.op_endpoint", StringComparison.CurrentCultureIgnoreCase)).Right);
            string Splitter = "?";
            foreach (Pair<string, string> Pair in Pairs)
            {
                Builder.Append(Splitter).Append(Pair.Left).Append("=").Append(Pair.Right);
                Splitter = "&";
            }
            return Builder.ToString();
        }

        #endregion

        #region Static Properties

        private static readonly Regex Links = new Regex(@"<link[^>]*/?>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex Href = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex Uri = new Regex("<URI>(?<URI>(.*))</URI>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex Keys = new Regex("[?&](?<Left>[^=]*)=(?<Right>[^&]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex GetUrl = new Regex(@"^(?<URL>.*)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// List of previously received Nonce
        /// </summary>
        public static System.Collections.Generic.List<string> NonceList { get; set; }

        #endregion
    }
}