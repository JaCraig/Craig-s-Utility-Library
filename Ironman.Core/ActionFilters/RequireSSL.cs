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
using System.Web;
using System.Web.Mvc;
#endregion

namespace Batman.MVC.ActionFilters
{
    /// <summary>
    /// Require SSL, forcing everything to https unless it's a local call
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RequireSSL : ActionFilterAttribute
    {
        #region Functions

        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="filterContext">filter context</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase Request = filterContext.HttpContext.Request;
            HttpResponseBase Response = filterContext.HttpContext.Response;
            if (!Request.IsSecureConnection && !Request.IsLocal)
            {
                UriBuilder Builder = new UriBuilder(Request.Url);
                Builder.Scheme = Uri.UriSchemeHttps;
                Response.Redirect(Builder.Uri.ToString());
            }
            base.OnActionExecuting(filterContext);
        }

        #endregion
    }
}
