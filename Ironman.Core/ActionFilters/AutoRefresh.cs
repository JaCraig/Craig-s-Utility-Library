/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Web.Mvc;

#endregion Usings

namespace Ironman.Core.ActionFilters
{
    /// <summary>
    /// Auto refreshes the page after a given number of seconds
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AutoRefresh : ActionFilterAttribute
    {
        /// <summary>
        /// Duration in seconds
        /// </summary>
        public virtual int Duration { get; set; }

        /// <summary>
        /// URL to refresh
        /// </summary>
        public virtual string URL { get; set; }

        /// <summary>
        /// OnActionExecuted
        /// </summary>
        /// <param name="filterContext">Filter context</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext == null)
                return;
            filterContext.HttpContext.Response.AppendHeader("Refresh", Duration + ";Url=" + filterContext.HttpContext.Server.UrlEncode(URL));
            base.OnActionExecuted(filterContext);
        }
    }
}