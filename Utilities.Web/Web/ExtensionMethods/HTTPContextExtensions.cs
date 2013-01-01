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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Reflection.ExtensionMethods;
#endregion

namespace Utilities.Web.ExtensionMethods
{
    /// <summary>
    /// HttpContext extensions
    /// </summary>
    public static class HTTPContextExtensions
    {
        #region Functions

        #region DumpAllInformation

        /// <summary>
        /// Dumps a lot of information about the request to a string (Request, Response, Session, Cookies, Cache, and Application state)
        /// </summary>
        /// <param name="Context">HttpContext</param>
        /// <param name="HTMLOutput">Determines if this should be HTML output or not</param>
        /// <returns>The exported data</returns>
        public static string DumpAllInformation(this HttpContext Context, bool HTMLOutput = false)
        {
            string HTMLTemplate = "<strong>Request Variables</strong><br />{Request}<br /><br /><strong>Response Variables</strong><br />{Response}<br /><br /><strong>Server Variables</strong><br />{Server}<br /><br /><strong>Session Variables</strong><br />{Session}<br /><br /><strong>Cookie Variables</strong><br />{Cookie}<br /><br /><strong>Cache Variables</strong><br />{Cache}<br /><br /><strong>Application Variables</strong><br />{Application}";
            string NormalTemplate = "Request Variables\r\n{Request}\r\n\r\nResponse Variables\r\n{Response}\r\n\r\nServer Variables\r\n{Server}\r\n\r\nSession Variables\r\n{Session}\r\n\r\nCookie Variables\r\n{Cookie}\r\n\r\nCache Variables\r\n{Cache}\r\n\r\nApplication Variables\r\n{Application}";
            KeyValuePair<string, string>[] Values = new KeyValuePair<string, string>[]{new KeyValuePair<string,string>("{Request}",Context.Request.DumpRequestVariable(HTMLOutput)),
                    new KeyValuePair<string,string>("{Response}",Context.Response.DumpResponseVariable(HTMLOutput)),
                    new KeyValuePair<string,string>("{Server}",Context.Request.DumpServerVars(HTMLOutput)),
                    new KeyValuePair<string,string>("{Session}",Context.Session.DumpSession(HTMLOutput)),
                    new KeyValuePair<string,string>("{Cookies}",Context.Request.Cookies.DumpCookies(HTMLOutput)),
                    new KeyValuePair<string,string>("{Cache}",Context.Cache.DumpCache(HTMLOutput)),
                    new KeyValuePair<string,string>("{Application}",Context.Application.DumpApplicationState(HTMLOutput))};
            return HTMLOutput ? HTMLTemplate.FormatString(Values) : NormalTemplate.FormatString(Values);
        }

        #endregion

        #region DumpApplicationState

        /// <summary>
        /// Dumps the values found in the Application State
        /// </summary>
        /// <param name="Page">Page in which to dump</param>
        /// <param name="HTMLOutput">Should html output be used?</param>
        /// <returns>A string containing the application state information</returns>
        public static string DumpApplicationState(this System.Web.UI.Page Page, bool HTMLOutput = false)
        {
            return Page.Application.DumpApplicationState(HTMLOutput);
        }

        /// <summary>
        /// Dumps the values found in the application state
        /// </summary>
        /// <param name="Input">Application state variable</param>
        /// <param name="HTMLOutput">Should html output be used?</param>
        /// <returns>A string containing the application state information</returns>
        public static string DumpApplicationState(this HttpApplicationState Input, bool HTMLOutput = false)
        {
            StringBuilder String = new StringBuilder();
            foreach (string Key in Input.Keys)
            {
                String.Append(Key).Append(": ")
                    .Append(Input[Key].ToString())
                    .Append(HTMLOutput ? "<br />Properties<br />" : "\r\nProperties\r\n")
                    .Append(Input[Key].DumpProperties(HTMLOutput))
                    .Append(HTMLOutput ? "<br />" : "\r\n");
            }
            return String.ToString();
        }

        #endregion

        #region DumpCache

        /// <summary>
        /// Dumps the values found in the cache
        /// </summary>
        /// <param name="Page">Page in which to dump</param>
        /// <param name="HTMLOutput">Should HTML output be used</param>
        /// <returns>A string containing the cache information</returns>
        public static string DumpCache(this System.Web.UI.Page Page, bool HTMLOutput = false)
        {
            return Page.Cache.DumpCache(HTMLOutput);
        }

        /// <summary>
        /// Dumps the values found in the cache
        /// </summary>
        /// <param name="Input">Cache variable</param>
        /// <param name="HTMLOutput">Should HTML output be used</param>
        /// <returns>A string containing the cache information</returns>
        public static string DumpCache(this System.Web.Caching.Cache Input, bool HTMLOutput = false)
        {
            StringBuilder String = new StringBuilder();
            foreach (DictionaryEntry Entry in Input)
            {
                String.Append(Entry.Key).Append(": ")
                    .Append(Entry.Value.ToString())
                    .Append(HTMLOutput ? "<br />Properties<br />" : "\r\nProperties\r\n")
                    .Append(Entry.Value.DumpProperties(HTMLOutput))
                    .Append(HTMLOutput ? "<br />" : "\r\n");
            }
            return String.ToString();
        }

        #endregion

        #region DumpCookies

        /// <summary>
        /// Dumps the values found in the cookies sent by the user
        /// </summary>
        /// <param name="Page">Page in which to dump</param>
        /// <param name="HTMLOutput">Should html output be used</param>
        /// <returns>A string containing the cookie information</returns>
        public static string DumpCookies(this System.Web.UI.Page Page, bool HTMLOutput = false)
        {
            return Page.Request.Cookies.DumpCookies();
        }

        /// <summary>
        /// Dumps the values found in the cookies sent by the user
        /// </summary>
        /// <param name="Input">Cookies</param>
        /// <param name="HTMLOutput">Should html output be used</param>
        /// <returns>A string containing the cookie information</returns>
        public static string DumpCookies(this HttpCookieCollection Input, bool HTMLOutput = false)
        {
            StringBuilder String = new StringBuilder();
            String.Append(HTMLOutput ? "<table><thead><tr><th>Name</th><th>Sub Name</th><th>Value</th></tr></thead><tbody>" : "Name\t\tSub Name\t\tValue\r\n");
            foreach (string Key in Input.Keys)
            {
                if (Input[Key].Values.Count > 1)
                {
                    foreach (string SubKey in Input[Key].Values.Keys)
                    {
                        String.Append(HTMLOutput ? "<tr><td>" : "").Append(Key)
                            .Append(HTMLOutput ? "</td><td>" : "\t\t")
                            .Append(SubKey)
                            .Append(HTMLOutput ? "</td><td>" : "\t\t")
                            .Append(Input[Key].Values[SubKey])
                            .Append(HTMLOutput ? "</td></tr>" : "\r\n");
                    }
                }
                else
                {
                    String.Append(HTMLOutput ? "<tr><td>" : "").Append(Key)
                            .Append(HTMLOutput ? "</td><td>" : "\t\t")
                            .Append(HTMLOutput ? "</td><td>" : "\t\t")
                            .Append(Input[Key].Value)
                            .Append(HTMLOutput ? "</td></tr>" : "\r\n");
                }
            }
            String.Append(HTMLOutput ? "</tbody></table>" : "\r\n");
            return String.ToString();
        }

        #endregion

        #region DumpRequestVariable

        /// <summary>
        /// Dumps information about the request variable
        /// </summary>
        /// <param name="Request">Request to dump the information about</param>
        /// <param name="HTMLOutput">Should HTML output be used</param>
        /// <returns>a string containing the information</returns>
        public static string DumpRequestVariable(this HttpRequest Request, bool HTMLOutput = false)
        {
            return Request.DumpProperties(HTMLOutput);
        }

        /// <summary>
        /// Dumps information about the request variable
        /// </summary>
        /// <param name="Page">Page to dump the information about</param>
        /// <param name="HTMLOutput">Should HTML output be used</param>
        /// <returns>a string containing the information</returns>
        public static string DumpRequestVariable(this System.Web.UI.Page Page, bool HTMLOutput = false)
        {
            return Page.Request.DumpRequestVariable(HTMLOutput);
        }

        #endregion

        #region DumpResponseVariable

        /// <summary>
        /// Dumps information about the response variable
        /// </summary>
        /// <param name="Response">Response to dump the information about</param>
        /// <param name="HTMLOutput">Should HTML output be used</param>
        /// <returns>a string containing the information</returns>
        public static string DumpResponseVariable(this HttpResponse Response, bool HTMLOutput = false)
        {
            return Response.DumpProperties(HTMLOutput);
        }

        /// <summary>
        /// Dumps information about the response variable
        /// </summary>
        /// <param name="Page">Page to dump the information about</param>
        /// <param name="HTMLOutput">Should HTML output be used</param>
        /// <returns>a string containing the information</returns>
        public static string DumpResponseVariable(this System.Web.UI.Page Page, bool HTMLOutput = false)
        {
            return Page.Response.DumpResponseVariable(HTMLOutput);
        }

        #endregion

        #region DumpServerVars

        /// <summary>
        /// Gets the server variables and dumps them out
        /// </summary>
        /// <param name="Request">request to get server variables from</param>
        /// <param name="HTMLOutput">Should HTML output be used</param>
        /// <returns>a string containing an HTML formatted list of the server variables</returns>
        public static string DumpServerVars(this HttpRequest Request, bool HTMLOutput = false)
        {
            StringBuilder String = new StringBuilder();
            String.Append(HTMLOutput ? "<table><thead><tr><th>Property Name</th><th>Value</th></tr></thead><tbody>" : "Property Name\t\tValue\r\n");
            foreach (string Key in Request.ServerVariables.Keys)
                String.Append(HTMLOutput ? "<tr><td>" : "")
                        .Append(Key)
                        .Append(HTMLOutput ? "</td><td>" : "\t\t")
                        .Append(Request.ServerVariables[Key])
                        .Append(HTMLOutput ? "</td></tr>" : "\r\n");
            String.Append(HTMLOutput ? "</tbody></table>" : "\r\n");
            return String.ToString();
        }

        /// <summary>
        /// Gets the server variables and dumps them out
        /// </summary>
        /// <param name="Page">page to get server variables from</param>
        /// <param name="HTMLOutput">Should HTML output be used</param>
        /// <returns>A string containing an HTML formatted list of the server variables</returns>
        public static string DumpServerVars(this System.Web.UI.Page Page, bool HTMLOutput = false)
        {
            return Page.Request.DumpServerVars(HTMLOutput);
        }

        #endregion

        #region DumpSession

        /// <summary>
        /// Dumps the values found in the session
        /// </summary>
        /// <param name="Page">Page in which to dump</param>
        /// <param name="HTMLOutput">Should HTML output be used</param>
        /// <returns>A string containing the session information</returns>
        public static string DumpSession(this System.Web.UI.Page Page, bool HTMLOutput = false)
        {
            return Page.Session.DumpSession(HTMLOutput);
        }

        /// <summary>
        /// Dumps the values found in the session
        /// </summary>
        /// <param name="Input">Session variable</param>
        /// <param name="HTMLOutput">Should HTML output be used</param>
        /// <returns>A string containing the session information</returns>
        public static string DumpSession(this System.Web.SessionState.HttpSessionState Input, bool HTMLOutput = false)
        {
            StringBuilder String = new StringBuilder();
            foreach (string Key in Input.Keys)
            {
                String.Append(Key).Append(": ")
                    .Append(Input[Key].ToString())
                    .Append(HTMLOutput ? "<br />Properties<br />" : "\r\nProperties\r\n")
                    .Append(Input[Key].DumpProperties(HTMLOutput))
                    .Append(HTMLOutput ? "<br />" : "\r\n");
            }
            return String.ToString();
        }

        #endregion

        #endregion
    }
}