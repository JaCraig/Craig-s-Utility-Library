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
using System.Text;
#endregion

namespace Utilities.Web.OpenSearch
{
    /// <summary>
    /// Open search helper (not recommended for use)
    /// </summary>
    public class OpenSearch
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenSearch()
        {
            RestHelper = new Utilities.Web.REST.REST();
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Searches a service
        /// </summary>
        /// <param name="QueryString">Query string</param>
        /// <param name="AdditionalInfo">Additional info</param>
        /// <returns>The returned data</returns>
        public string Search(string QueryString, string AdditionalInfo)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendFormat(APILocation, QueryString, AdditionalInfo);
            RestHelper.Url = new Uri(Builder.ToString());
            return RestHelper.GET();
        }

        #endregion

        #region Protected Properties

        protected REST.REST RestHelper { get; set; }
        protected string APILocation { get; set; }

        #endregion
    }
}