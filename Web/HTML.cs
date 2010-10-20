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
using System.Collections;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;


#endregion

namespace Utilities.Web
{
    /// <summary>
    /// Utility class holding HTML related functions
    /// </summary>
    public static class HTML
    {
        #region Variables

        private const string GZIP = "gzip";
        private const string DEFLATE = "deflate";

        /// <summary>
        /// Regex used to strip html from a string
        /// </summary>
        private static readonly Regex STRIP_HTML_REGEX = new Regex("<[^>]*>", RegexOptions.Compiled);

        #endregion

        #region Static Public Functions

        /// <summary>
        /// Minifies HTML, removing unnecessary spaces, javascript comments, etc.
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>The final, minified string</returns>
        public static string Minify(string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            Input = Regex.Replace(Input, "/// <.+>", "");
            if (string.IsNullOrEmpty(Input))
                return "";
            Input = Regex.Replace(Input, @">[\s\S]*?<", new MatchEvaluator(Evaluate));
            return Input;
        }

        /// <summary>
        /// Removes illegal characters (used in uri's, etc.)
        /// </summary>
        /// <param name="Input">string to be converted</param>
        /// <returns>A stripped string</returns>
        public static string RemoveIllegalCharacters(string Input)
        {
            if (string.IsNullOrEmpty(Input))
            {
                return Input;
            }

            Input = Input.Replace(":", string.Empty);
            Input = Input.Replace("/", string.Empty);
            Input = Input.Replace("?", string.Empty);
            Input = Input.Replace("#", string.Empty);
            Input = Input.Replace("[", string.Empty);
            Input = Input.Replace("]", string.Empty);
            Input = Input.Replace("@", string.Empty);
            Input = Input.Replace(".", string.Empty);
            Input = Input.Replace("\"", string.Empty);
            Input = Input.Replace("&", string.Empty);
            Input = Input.Replace("'", string.Empty);
            Input = Input.Replace(" ", "-");
            RemoveExtraHyphen(Input);
            RemoveDiacritics(Input);
            return HttpUtility.UrlEncode(Input).Replace("%", string.Empty);
        }

        /// <summary>
        /// removes HTML elements from a string
        /// </summary>
        /// <param name="HTML">HTML laiden string</param>
        /// <returns>HTML-less string</returns>
        public static string StripHTML(string HTML)
        {
            if (string.IsNullOrEmpty(HTML))
                return string.Empty;

            HTML = STRIP_HTML_REGEX.Replace(HTML, string.Empty);
            HTML = HTML.Replace("&nbsp;", " ");
            return HTML.Replace("&#160;", string.Empty);
        }

        /// <summary>
        /// Decides if the string contains HTML
        /// </summary>
        /// <param name="Input">Input string to check</param>
        /// <returns>false if it does not contain HTML, true otherwise</returns>
        public static bool ContainsHTML(string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return false;

            return STRIP_HTML_REGEX.IsMatch(Input);
        }


        /// <summary>
        /// Adds a script file to the header of the current page
        /// </summary>
        /// <param name="File">Script file</param>
        /// <param name="Directory">Script directory</param>
        public static void AddScriptFile(string File, string Directory)
        {
            System.Web.UI.Page CurrentPage = (System.Web.UI.Page)HttpContext.Current.CurrentHandler;
            if (!CurrentPage.ClientScript.IsClientScriptIncludeRegistered(typeof(System.Web.UI.Page), Directory + File))
            {
                CurrentPage.ClientScript.RegisterClientScriptInclude(typeof(System.Web.UI.Page), Directory + File, HttpContext.Current.Server.MapPath(Directory + File));
            }
        }

        /// <summary>
        /// Checks the request headers to see if the specified
        /// encoding is accepted by the client.
        /// </summary>
        public static bool IsEncodingAccepted(string encoding)
        {
            return HttpContext.Current.Request.Headers["Accept-encoding"] != null && HttpContext.Current.Request.Headers["Accept-encoding"].Contains(encoding);
        }

        /// <summary>
        /// Adds the specified encoding to the response headers.
        /// </summary>
        /// <param name="encoding"></param>
        public static void SetEncoding(string encoding)
        {
            HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
        }

        /// <summary>
        /// Adds HTTP compression to the current context
        /// </summary>
        /// <param name="context">Current context</param>
        public static void HTTPCompress(HttpContext context)
        {
            if (context.Request.UserAgent != null && context.Request.UserAgent.Contains("MSIE 6"))
                return;

            if (HTML.IsEncodingAccepted(GZIP))
            {
                context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
                HTML.SetEncoding(GZIP);
            }
            else if (HTML.IsEncodingAccepted(DEFLATE))
            {
                context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
                HTML.SetEncoding(DEFLATE);
            }
        }

        /// <summary>
        /// Gets the relative root of the web site
        /// </summary>
        public static string RelativeRoot
        {
            get
            {
                return VirtualPathUtility.ToAbsolute("~/");
            }
        }

        /// <summary>
        /// Returns the absolute root
        /// </summary>
        public static Uri AbsoluteRoot
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context == null)
                    throw new System.Net.WebException("The current HttpContext is null");

                if (context.Items["absoluteurl"] == null)
                    context.Items["absoluteurl"] = new Uri(context.Request.Url.GetLeftPart(UriPartial.Authority) + RelativeRoot);

                return context.Items["absoluteurl"] as Uri;
            }
        }

        /// <summary>
        /// Gets the server variables and dumps them out
        /// </summary>
        /// <param name="Request">request to get server variables from</param>
        /// <returns>a string containing an HTML formatted list of the server variables</returns>
        public static string DumpServerVars(HttpRequest Request)
        {
            StringBuilder String = new StringBuilder();
            String.Append("<table><thead><tr><th>Property Name</th><th>Value</th></tr></thead><tbody>");
            foreach (string Key in Request.ServerVariables.Keys)
            {
                String.Append("<tr><td>").Append(Key).Append("</td><td>").Append(Request.ServerVariables[Key]).Append("</td></tr>");
            }
            String.Append("</tbody></table>");
            return String.ToString();
        }

        /// <summary>
        /// Gets the server variables and dumps them out
        /// </summary>
        /// <param name="page">page to get server variables from</param>
        /// <returns>a string containing an HTML formatted list of the server variables</returns>
        public static string DumpServerVars(System.Web.UI.Page page)
        {
            return DumpServerVars(page.Request);
        }

        /// <summary>
        /// Dumps information about the request variable
        /// </summary>
        /// <param name="Request">Request to dump the information about</param>
        /// <returns>a string containing the information</returns>
        public static string DumpRequestVariable(HttpRequest Request)
        {
            StringBuilder String = new StringBuilder();
            String.Append(Reflection.Reflection.DumpProperties(Request));
            return String.ToString();
        }

        /// <summary>
        /// Dumps information about the request variable
        /// </summary>
        /// <param name="Page">Page to dump the information about</param>
        /// <returns>a string containing the information</returns>
        public static string DumpRequestVariable(System.Web.UI.Page Page)
        {
            return DumpRequestVariable(Page.Request);
        }

        /// <summary>
        /// Dumps information about the response variable
        /// </summary>
        /// <param name="Response">Response to dump the information about</param>
        /// <returns>a string containing the information</returns>
        public static string DumpResponseVariable(HttpResponse Response)
        {
            StringBuilder String = new StringBuilder();
            String.Append(Reflection.Reflection.DumpProperties(Response));
            return String.ToString();
        }

        /// <summary>
        /// Dumps information about the response variable
        /// </summary>
        /// <param name="Page">Page to dump the information about</param>
        /// <returns>a string containing the information</returns>
        public static string DumpResponseVariable(System.Web.UI.Page Page)
        {
            return DumpResponseVariable(Page.Response);
        }

        /// <summary>
        /// Dumps the values found in the session
        /// </summary>
        /// <param name="Page">Page in which to dump</param>
        /// <returns>A string containing the session information</returns>
        public static string DumpSession(System.Web.UI.Page Page)
        {
            return DumpSession(Page.Session);
        }

        /// <summary>
        /// Dumps the values found in the session
        /// </summary>
        /// <param name="Input">Session variable</param>
        /// <returns>A string containing the session information</returns>
        public static string DumpSession(System.Web.SessionState.HttpSessionState Input)
        {
            StringBuilder String = new StringBuilder();
            foreach (string Key in Input.Keys)
            {
                String.Append(Key).Append(": ")
                    .Append(Input[Key].ToString())
                    .Append("<br />Properties<br />")
                    .Append(Reflection.Reflection.DumpProperties(Input[Key]))
                    .Append("<br />");
            }
            return String.ToString();
        }

        /// <summary>
        /// Dumps the values found in the cache
        /// </summary>
        /// <param name="Page">Page in which to dump</param>
        /// <returns>A string containing the cache information</returns>
        public static string DumpCache(System.Web.UI.Page Page)
        {
            return DumpCache(Page.Cache);
        }

        /// <summary>
        /// Dumps the values found in the cache
        /// </summary>
        /// <param name="Input">Cache variable</param>
        /// <returns>A string containing the cache information</returns>
        public static string DumpCache(System.Web.Caching.Cache Input)
        {
            StringBuilder String = new StringBuilder();
            foreach (DictionaryEntry Entry in Input)
            {
                String.Append(Entry.Key).Append(": ")
                    .Append(Entry.Value.ToString())
                    .Append("<br />Properties<br />")
                    .Append(Reflection.Reflection.DumpProperties(Entry.Value))
                    .Append("<br />");
            }
            return String.ToString();
        }

        /// <summary>
        /// Dumps the values found in the Application State
        /// </summary>
        /// <param name="Page">Page in which to dump</param>
        /// <returns>A string containing the application state information</returns>
        public static string DumpApplicationState(System.Web.UI.Page Page)
        {
            return DumpApplicationState(Page.Application);
        }

        /// <summary>
        /// Dumps the values found in the application state
        /// </summary>
        /// <param name="Input">Application state variable</param>
        /// <returns>A string containing the application state information</returns>
        public static string DumpApplicationState(HttpApplicationState Input)
        {
            StringBuilder String = new StringBuilder();
            foreach (string Key in Input.Keys)
            {
                String.Append(Key).Append(": ")
                    .Append(Input[Key].ToString())
                    .Append("<br />Properties<br />")
                    .Append(Reflection.Reflection.DumpProperties(Input[Key]))
                    .Append("<br />");
            }
            return String.ToString();
        }

        /// <summary>
        /// Dumps the values found in the cookies sent by the user
        /// </summary>
        /// <param name="Page">Page in which to dump</param>
        /// <returns>A string containing the cookie information</returns>
        public static string DumpCookies(System.Web.UI.Page Page)
        {
            return DumpCookies(Page.Request.Cookies);
        }

        /// <summary>
        /// Dumps the values found in the cookies sent by the user
        /// </summary>
        /// <param name="Input">Cookies</param>
        /// <returns>A string containing the cookie information</returns>
        public static string DumpCookies(HttpCookieCollection Input)
        {
            StringBuilder String = new StringBuilder();
            String.Append("<table><thead><tr><th>Name</th><th>Sub Name</th><th>Value</th></tr></thead><tbody>");
            foreach (string Key in Input.Keys)
            {
                if (Input[Key].Values.Count > 1)
                {
                    foreach (string SubKey in Input[Key].Values.Keys)
                    {
                        String.Append("<tr><td>").Append(Key).Append("</td><td>").Append(SubKey).Append("</td><td>");
                        String.Append(Input[Key].Values[SubKey]).Append("</td></tr>");
                    }
                }
                else
                {
                    String.Append("<tr><td>").Append(Key).Append("</td><td></td><td>");
                    String.Append(Input[Key].Value).Append("</td></tr>");
                }
            }
            String.Append("</tbody></table>");
            return String.ToString();
        }
        #endregion

        #region Static Private Functions

        /// <summary>
        /// Removes extra hyphens from a string
        /// </summary>
        /// <param name="Input">string to be stripped</param>
        /// <returns>Stripped string</returns>
        private static string RemoveExtraHyphen(string Input)
        {
            while (Input.Contains("--"))
            {
                Input = Input.Replace("--", "-");
            }

            return Input;
        }

        /// <summary>
        /// Removes special characters (Diacritics) from the string
        /// </summary>
        /// <param name="Input">String to strip</param>
        /// <returns>Stripped string</returns>
        private static string RemoveDiacritics(string Input)
        {
            string Normalized = Input.Normalize(NormalizationForm.FormD);
            StringBuilder Builder = new StringBuilder();

            for (int i = 0; i < Normalized.Length; i++)
            {
                Char TempChar = Normalized[i];
                if (CharUnicodeInfo.GetUnicodeCategory(TempChar) != UnicodeCategory.NonSpacingMark)
                    Builder.Append(TempChar);
            }

            return Builder.ToString();
        }

        /// <summary>
        /// Evaluates whether the text has spaces, page breaks, etc. and removes them.
        /// </summary>
        /// <param name="Matcher">The matched text</param>
        /// <returns>Stripped text</returns>
        private static string Evaluate(Match Matcher)
        {
            string MyString = Matcher.ToString();
            if (string.IsNullOrEmpty(MyString))
                return "";
            MyString = Regex.Replace(MyString, @"\r\n\s*", "");
            return MyString;
        }

        #endregion
    }
}