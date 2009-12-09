using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Web.OAuth;
using System.Text.RegularExpressions;
using System.Web;

namespace Utilities.Web.Twitter
{
    public class Twitter:OAuth.OAuth
    {
        public Twitter()
            : base()
        {
            this.SignatureType = Signature.HMACSHA1;
        }

        public void GetRequestToken(out string Token,out string TokenSecret)
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

        public string GetAuthorizationSite()
        {
            this.Method = HTTPMethod.GET;
            this.Url = new Uri("http://twitter.com/oauth/authorize");
            this.AddParameter("oauth_callback", "oob");
            return new Uri(GenerateRequest()).ToString();
        }

        public void GetAccessToken(string PIN, out string AccessToken, out string AccessTokenSecret)
        {
            this.Url = new Uri("http://twitter.com/oauth/access_token");
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

        public string UpdateStatus(string Status)
        {
            this.Method = HTTPMethod.POST;
            this.Url = new Uri("http://twitter.com/statuses/update.xml");
            this.AddParameter("status", Status);
            REST.REST RestHelper = new Utilities.Web.REST.REST();
            RestHelper.Url = new Uri(GenerateRequest());
            return RestHelper.POST();
        }

        public string GetTimeline(string UserName)
        {
            this.Method = HTTPMethod.POST;
            this.Url = new Uri("http://twitter.com/statuses/user_timeline.xml");
            this.AddParameter("screen_name", UserName);
            REST.REST RestHelper = new Utilities.Web.REST.REST();
            RestHelper.Url = new Uri(GenerateRequest());
            return RestHelper.GET();
        }
    }
}
