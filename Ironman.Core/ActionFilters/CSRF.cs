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
#endregion

namespace Batman.MVC.ActionFilters
{
    /// <summary>
    /// Automatically sets up a cross site request forgery token
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CSRF : FilterAttribute, IAuthorizationFilter
    {
        #region Functions

        /// <summary>
        /// OnAuthorization
        /// </summary>
        /// <param name="filterContext">filter context</param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (ShouldValidate(filterContext))
                new ValidateAntiForgeryTokenAttribute().OnAuthorization(filterContext);
        }

        /// <summary>
        /// Should the cross site request forgery token be set up?
        /// </summary>
        /// <param name="Context">Context item</param>
        /// <returns>True if it should be set up, false otherwise</returns>
        private bool ShouldValidate(AuthorizationContext Context)
        {
            return (string.Compare(Context.HttpContext.Request.HttpMethod, System.Net.WebRequestMethods.Http.Post, true) == 0
                && Context.ActionDescriptor.Attributes<ValidateAntiForgeryTokenAttribute>().Length == 0
                && Context.ActionDescriptor.Attributes<ByPassCRSF>().Length == 0);
        }

        #endregion
    }
}