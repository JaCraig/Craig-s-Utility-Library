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
using Utilities.DataTypes;
using Utilities.IO;
using Utilities.Web;

#endregion Usings

namespace Ironman.Core.ActionFilters
{
    /// <summary>
    /// Compresses the data
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class Compress : ActionFilterAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Compress()
        {
            Type = MinificationType.HTML;
            Minify = true;
        }

        /// <summary>
        /// Should this be minified
        /// </summary>
        public virtual bool Minify { get; set; }

        /// <summary>
        /// Minification type
        /// </summary>
        public virtual MinificationType Type { get; set; }

        /// <summary>
        /// On Action Executing
        /// </summary>
        /// <param name="filterContext">filter context</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
                return;
            if (filterContext.ActionDescriptor.Attributes<DoNotCompress>().Length == 0)
                filterContext.HttpContext.HTTPCompress(Minify, Type);
            base.OnActionExecuting(filterContext);
        }
    }
}