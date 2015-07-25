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
using System.Web;
using System.Web.Mvc;
using Utilities.DataTypes;
using Utilities.IO;

#endregion Usings

namespace Ironman.Core.ActionFilters
{
    /// <summary>
    /// Used to fill a parameter with content
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal class DeserializationFilter : ActionFilterAttribute
    {
        /// <summary>
        /// type of the object
        /// </summary>
        public Type ObjectType { get; set; }

        /// <summary>
        /// Parameter to fill
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// Run when the action is executing
        /// </summary>
        /// <param name="filterContext">Filter context</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
                return;
            HttpRequestBase Request = filterContext.HttpContext.Request;
            if (string.IsNullOrEmpty(Request.ContentType))
                return;
            if (!Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.Serializers.Manager>().CanSerialize(Request.ContentType))
                return;
            string Body = Request.InputStream.ReadAll();
            Request.InputStream.Seek(0, System.IO.SeekOrigin.Begin);

            if (!string.IsNullOrEmpty(Body))
                filterContext.ActionParameters[Parameter] = Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.IO.Serializers.Manager>().Deserialize(Body, ObjectType, Request.ContentType);
        }
    }
}