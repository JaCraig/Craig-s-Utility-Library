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
using System.Text;
using System.Web;
using Utilities.Web;

#endregion

namespace Utilities.Error
{
    /// <summary>
    /// Class that handles gathering of error information
    /// for reporting purposes
    /// </summary>
    public static class ErrorManager
    {
        #region Public Static Functions

        /// <summary>
        /// returns information specific to ASP.Net/IIS (Request, Response, Cache, etc.)
        /// </summary>
        /// <returns>An HTML formatted string containing the ASP.Net information</returns>
        public static string GetAllASPNetInformation()
        {
            StringBuilder Builder = new StringBuilder();
            HttpContext Current = HttpContext.Current;
            Builder.Append("<strong>Request Variables</strong><br />");
            Builder.Append(HTML.DumpRequestVariable(Current.Request));
            Builder.Append("<br /><br /><strong>Response Variables</strong><br />");
            Builder.Append(HTML.DumpResponseVariable(Current.Response));
            Builder.Append("<br /><br /><strong>Server Variables</strong><br />");
            Builder.Append(HTML.DumpServerVars(Current.Request));
            Builder.Append("<br /><br /><strong>Session Variables</strong><br />");
            Builder.Append(HTML.DumpSession(Current.Session));
            Builder.Append("<br /><br /><strong>Cookie Variables</strong><br />");
            Builder.Append(HTML.DumpCookies(Current.Request.Cookies));
            Builder.Append("<br /><br /><strong>Cache Variables</strong><br />");
            Builder.Append(HTML.DumpCache(Current.Cache));
            Builder.Append("<br /><br /><strong>Application State Variables</strong><br />");
            Builder.Append(HTML.DumpApplicationState(Current.Application));
            return Builder.ToString();
        }

        /// <summary>
        /// Gets assembly information for all currently loaded assemblies
        /// </summary>
        /// <returns>An HTML formatted string containing the assembly information</returns>
        public static string GetAssemblyInformation()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<strong>Assembly Information</strong><br />");
            Builder.Append(Reflection.Reflection.DumpAllAssembliesAndProperties());
            return Builder.ToString();
        }

        /// <summary>
        /// Gets information about the system.
        /// </summary>
        /// <returns>An HTML formatted string containing the state of the system.</returns>
        public static string GetSystemInformation()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<strong>System Information</strong><br />");
            Builder.Append(Reflection.Reflection.DumpProperties(System.Type.GetType("Utilities.Environment.Environment")));
            return Builder.ToString();
        }

        /// <summary>
        /// Gets all process information and outputs it to an HTML formatted string
        /// </summary>
        /// <returns>An HTML formatted string containing the process information</returns>
        public static string GetProcessInformation()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<strong>Process Information</strong><br />");
            Builder.Append(Environment.ProcessManager.GetProcessInformation());
            return Builder.ToString();
        }

        #endregion
    }
}