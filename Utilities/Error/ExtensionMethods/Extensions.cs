/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Text;
using System.Web;
using Utilities.Reflection.ExtensionMethods;
using System.Reflection;
using System;
using System.Linq;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Environment.ExtensionMethods;
using System.Diagnostics;
using System.Collections;
#endregion

namespace Utilities.Error.ExtensionMethods
{
    /// <summary>
    /// Error related extensions for various items
    /// </summary>
    public static class Extensions
    {
        #region DumpApplicationState

        /// <summary>
        /// Dumps the values found in the Application State
        /// </summary>
        /// <param name="Page">Page in which to dump</param>
        /// <returns>A string containing the application state information</returns>
        public static string DumpApplicationState(this System.Web.UI.Page Page)
        {
            return Page.Application.DumpApplicationState();
        }

        /// <summary>
        /// Dumps the values found in the application state
        /// </summary>
        /// <param name="Input">Application state variable</param>
        /// <returns>A string containing the application state information</returns>
        public static string DumpApplicationState(this HttpApplicationState Input)
        {
            StringBuilder String = new StringBuilder();
            foreach (string Key in Input.Keys)
            {
                String.Append(Key).Append(": ")
                    .Append(Input[Key].ToString())
                    .Append("<br />Properties<br />")
                    .Append(Input[Key].DumpProperties())
                    .Append("<br />");
            }
            return String.ToString();
        }

        #endregion

        #region DumpCache

        /// <summary>
        /// Dumps the values found in the cache
        /// </summary>
        /// <param name="Page">Page in which to dump</param>
        /// <returns>A string containing the cache information</returns>
        public static string DumpCache(this System.Web.UI.Page Page)
        {
            return Page.Cache.DumpCache();
        }

        /// <summary>
        /// Dumps the values found in the cache
        /// </summary>
        /// <param name="Input">Cache variable</param>
        /// <returns>A string containing the cache information</returns>
        public static string DumpCache(this System.Web.Caching.Cache Input)
        {
            StringBuilder String = new StringBuilder();
            foreach (DictionaryEntry Entry in Input)
            {
                String.Append(Entry.Key).Append(": ")
                    .Append(Entry.Value.ToString())
                    .Append("<br />Properties<br />")
                    .Append(Entry.Value.DumpProperties())
                    .Append("<br />");
            }
            return String.ToString();
        }

        #endregion

        #region DumpCookies

        /// <summary>
        /// Dumps the values found in the cookies sent by the user
        /// </summary>
        /// <param name="Page">Page in which to dump</param>
        /// <returns>A string containing the cookie information</returns>
        public static string DumpCookies(this System.Web.UI.Page Page)
        {
            return Page.Request.Cookies.DumpCookies();
        }

        /// <summary>
        /// Dumps the values found in the cookies sent by the user
        /// </summary>
        /// <param name="Input">Cookies</param>
        /// <returns>A string containing the cookie information</returns>
        public static string DumpCookies(this HttpCookieCollection Input)
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

        #region DumpRequestVariable

        /// <summary>
        /// Dumps information about the request variable
        /// </summary>
        /// <param name="Request">Request to dump the information about</param>
        /// <returns>a string containing the information</returns>
        public static string DumpRequestVariable(this HttpRequest Request)
        {
            return Request.DumpProperties();
        }

        /// <summary>
        /// Dumps information about the request variable
        /// </summary>
        /// <param name="Page">Page to dump the information about</param>
        /// <returns>a string containing the information</returns>
        public static string DumpRequestVariable(this System.Web.UI.Page Page)
        {
            return Page.Request.DumpRequestVariable();
        }

        #endregion

        #region DumpResponseVariable

        /// <summary>
        /// Dumps information about the response variable
        /// </summary>
        /// <param name="Response">Response to dump the information about</param>
        /// <returns>a string containing the information</returns>
        public static string DumpResponseVariable(this HttpResponse Response)
        {
            return Response.DumpProperties();
        }

        /// <summary>
        /// Dumps information about the response variable
        /// </summary>
        /// <param name="Page">Page to dump the information about</param>
        /// <returns>a string containing the information</returns>
        public static string DumpResponseVariable(this System.Web.UI.Page Page)
        {
            return Page.Response.DumpResponseVariable();
        }

        #endregion

        #region DumpServerVars

        /// <summary>
        /// Gets the server variables and dumps them out
        /// </summary>
        /// <param name="Request">request to get server variables from</param>
        /// <returns>a string containing an HTML formatted list of the server variables</returns>
        public static string DumpServerVars(this HttpRequest Request)
        {
            StringBuilder String = new StringBuilder();
            String.Append("<table><thead><tr><th>Property Name</th><th>Value</th></tr></thead><tbody>");
            foreach (string Key in Request.ServerVariables.Keys)
                String.Append("<tr><td>").Append(Key).Append("</td><td>").Append(Request.ServerVariables[Key]).Append("</td></tr>");
            String.Append("</tbody></table>");
            return String.ToString();
        }

        /// <summary>
        /// Gets the server variables and dumps them out
        /// </summary>
        /// <param name="Page">page to get server variables from</param>
        /// <returns>A string containing an HTML formatted list of the server variables</returns>
        public static string DumpServerVars(this System.Web.UI.Page Page)
        {
            return Page.Request.DumpServerVars();
        }

        #endregion

        #region DumpSession

        /// <summary>
        /// Dumps the values found in the session
        /// </summary>
        /// <param name="Page">Page in which to dump</param>
        /// <returns>A string containing the session information</returns>
        public static string DumpSession(this System.Web.UI.Page Page)
        {
            return Page.Session.DumpSession();
        }

        /// <summary>
        /// Dumps the values found in the session
        /// </summary>
        /// <param name="Input">Session variable</param>
        /// <returns>A string containing the session information</returns>
        public static string DumpSession(this System.Web.SessionState.HttpSessionState Input)
        {
            StringBuilder String = new StringBuilder();
            foreach (string Key in Input.Keys)
            {
                String.Append(Key).Append(": ")
                    .Append(Input[Key].ToString())
                    .Append("<br />Properties<br />")
                    .Append(Input[Key].DumpProperties())
                    .Append("<br />");
            }
            return String.ToString();
        }

        #endregion
    }
}