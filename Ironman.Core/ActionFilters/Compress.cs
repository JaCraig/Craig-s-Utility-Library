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
using System.Web.Mvc;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Web.ExtensionMethods;
#endregion

namespace Batman.MVC.ActionFilters
{
    /// <summary>
    /// Compresses the data
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class Compress : ActionFilterAttribute
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Compress()
            : base()
        {
            Type = MinificationType.HTML;
            Minify = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Should this be minified
        /// </summary>
        public virtual bool Minify { get; set; }

        /// <summary>
        /// Minification type
        /// </summary>
        public virtual MinificationType Type { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// On Action Executing
        /// </summary>
        /// <param name="filterContext">filter context</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.Attributes<DoNotCompress>().Length == 0)
                filterContext.HttpContext.HTTPCompress(Minify, Type);
            base.OnActionExecuting(filterContext);
        }

        #endregion
    }
}