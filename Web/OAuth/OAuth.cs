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
using System.Security.Cryptography;
using System.Text;
using Utilities.DataTypes;
#endregion

namespace Utilities.Web.OAuth
{
    /// <summary>
    /// OAuth base class
    /// </summary>
    public class OAuth
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public OAuth()
        {
            Parameters = new System.Collections.Generic.List<Pair<string, string>>();
            AddParameter("oauth_consumer_key", "");
            AddParameter("oauth_nonce", "");
            AddParameter("oauth_signature_method", "");
            AddParameter("oauth_timestamp", "");
            AddParameter("oauth_version", "1.0");
            RandomGenerator = new Random.Random();
        }

        #endregion

        #region Protected Functions

        /// <summary>
        /// Generates a request
        /// </summary>
        /// <returns>The string containing the request</returns>
        protected string GenerateRequest()
        {
            string Url = "";
            string Parameters = "";
            string Signature = GenerateSignature(out Url, out Parameters);
            string ReturnUrl = Url.ToString() + "?" + Parameters + "&oauth_signature=" + UrlEncode(Signature);
            return ReturnUrl;
        }

        /// <summary>
        /// Generates the signature
        /// </summary>
        /// <param name="Url">Url</param>
        /// <param name="Parameters">Parameters</param>
        /// <returns>The signature</returns>
        protected string GenerateSignature(out string Url, out string Parameters)
        {
            Parameters = "";
            Url = "";

            if (this.SignatureType == Signature.HMACSHA1)
            {
                string Base = GenerateBase(out Url, out Parameters);
                HMACSHA1 SHA1 = new HMACSHA1();
                SHA1.Key = Encoding.ASCII.GetBytes(UrlEncode(ConsumerKeySecret) + "&" + (string.IsNullOrEmpty(TokenSecret) ? "" : UrlEncode(TokenSecret)));
                return Convert.ToBase64String(SHA1.ComputeHash(System.Text.Encoding.ASCII.GetBytes(Base)));
            }
            else if (this.SignatureType == Signature.RSASHA1)
            {
                throw new NotImplementedException();
            }
            else if (this.SignatureType == Signature.PLAINTEXT)
            {
                return UrlEncode(ConsumerKeySecret + "&" + TokenSecret);
            }
            return "";
        }

        /// <summary>
        /// Does url encoding using uppercase since that is needed for .Net
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>Url encoded string</returns>
        protected string UrlEncode(string Input)
        {
            StringBuilder Result = new StringBuilder();
            for (int x = 0; x < Input.Length; ++x)
            {
                if (UnreservedChars.IndexOf(Input[x]) != -1)
                    Result.Append(Input[x]);
                else
                    Result.Append("%").Append(String.Format("{0:X2}", (int)Input[x]));
            }
            return Result.ToString();
        }

        /// <summary>
        /// Adds a parameter
        /// </summary>
        /// <param name="Key">Key text</param>
        /// <param name="Value">Value text</param>
        protected void AddParameter(string Key, string Value)
        {
            bool Found = false;
            foreach (Pair<string, string> Pair in Parameters)
            {
                if (Pair.Left == Key)
                {
                    Pair.Right = Value;
                    Found = true;
                    break;
                }
            }
            if (!Found)
            {
                Parameters.Add(new Pair<string, string>(Key, Value));
            }
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Generates the info used in the signature
        /// </summary>
        /// <param name="UrlString">Url string</param>
        /// <param name="ParameterString">Parameter string</param>
        /// <returns>The base information for the signature</returns>
        private string GenerateBase(out string UrlString, out string ParameterString)
        {
            StringBuilder UrlBuilder = new StringBuilder();
            StringBuilder Builder = new StringBuilder();
            StringBuilder ParameterBuilder = new StringBuilder();

            string SignatureMethod = "";
            if (this.SignatureType == Signature.HMACSHA1)
                SignatureMethod = "HMAC-SHA1";
            else if (this.SignatureType == Signature.RSASHA1)
                SignatureMethod = "RSA-SHA1";
            else if (this.SignatureType == Signature.PLAINTEXT)
                SignatureMethod = "PLAINTEXT";

            AddParameter("oauth_consumer_key", this.ConsumerKey);
            AddParameter("oauth_nonce", RandomGenerator.Next(123400, 9999999).ToString());
            AddParameter("oauth_signature_method", SignatureMethod);
            AddParameter("oauth_timestamp", Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString());
            AddParameter("oauth_version", "1.0");

            if (!string.IsNullOrEmpty(this.Token))
                AddParameter("oauth_token", this.Token);
            if (!string.IsNullOrEmpty(this.TokenSecret))
                AddParameter("oauth_token_secret", this.TokenSecret);

            Parameters.Sort(new PairComparer());

            string Splitter = "";
            foreach (Pair<string, string> Key in Parameters)
            {
                ParameterBuilder.Append(Splitter)
                    .Append(Key.Left)
                    .Append("=")
                    .Append(UrlEncode(Key.Right));
                Splitter = "&";
            }

            UrlBuilder.Append(Url.Scheme).Append("://").Append(Url.Host);
            if ((Url.Scheme == "http" && Url.Port != 80) || (Url.Scheme == "https" && Url.Port != 443))
                UrlBuilder.Append(":").Append(Url.Port);
            UrlBuilder.Append(Url.AbsolutePath);

            UrlString = UrlBuilder.ToString();
            ParameterString = ParameterBuilder.ToString();

            Builder.Append(this.Method.ToString().ToUpper())
                .Append("&")
                .Append(UrlEncode(UrlBuilder.ToString()))
                .Append("&")
                .Append(UrlEncode(ParameterBuilder.ToString()));

            return Builder.ToString();
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Url that is being used
        /// </summary>
        protected Uri Url { get; set; }

        /// <summary>
        /// Consumer key
        /// </summary>
        public virtual string ConsumerKey { get; set; }

        /// <summary>
        /// Consumer key secret
        /// </summary>
        public virtual string ConsumerKeySecret { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public virtual string Token { get; set; }

        /// <summary>
        /// Token secret
        /// </summary>
        public virtual string TokenSecret { get; set; }

        /// <summary>
        /// HTTP Method
        /// </summary>
        protected HTTPMethod Method { get; set; }

        /// <summary>
        /// The hash type that is suppose to be used
        /// </summary>
        protected Signature SignatureType { get; set; }

        #endregion

        #region Private Variables

        private string UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
        private System.Collections.Generic.List<Pair<string, string>> Parameters { get; set; }
        private Random.Random RandomGenerator { get; set; }

        #endregion

        #region Private Classes

        /// <summary>
        /// Comparer class for the pair type
        /// </summary>
        private class PairComparer : IComparer<Pair<string, string>>
        {
            public int Compare(Pair<string, string> x, Pair<string, string> y)
            {
                if (x.Left == y.Left)
                {
                    return string.Compare(x.Right, y.Right);
                }
                else
                {
                    return string.Compare(x.Left, y.Left);
                }
            }
        }

        #endregion
    }

    #region Enums

    /// <summary>
    /// HTTP Method
    /// </summary>
    public enum HTTPMethod
    {
        GET,
        POST,
        DELETE,
        PUT
    }

    /// <summary>
    /// Hash type
    /// </summary>
    public enum Signature
    {
        PLAINTEXT,
        RSASHA1,
        HMACSHA1
    }

    #endregion
}