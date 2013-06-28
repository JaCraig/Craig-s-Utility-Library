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
using System.Diagnostics.Contracts;
using System.Xml.Linq;
#endregion

namespace Utilities.FileFormats.RSD
{
    /// <summary>
    /// API class
    /// </summary>
    public class API
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public API()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">Element containing the API info</param>
        public API(XElement Element)
        {
            Contract.Requires<ArgumentNullException>(Element!=null,"Element");
            if (Element.Attribute("name") != null)
            {
                Name = Element.Attribute("name").Value;
            }
            if (Element.Attribute("preferred") != null)
            {
                Preferred = bool.Parse(Element.Attribute("preferred").Value);
            }
            if (Element.Attribute("apiLink") != null)
            {
                APILink = Element.Attribute("apiLink").Value;
            }
            if (Element.Attribute("blogID") != null)
            {
                BlogID = Element.Attribute("blogID").Value;
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Name of the API
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Is this API preferred?
        /// </summary>
        public virtual bool Preferred { get; set; }

        /// <summary>
        /// Link to the API
        /// </summary>
        public virtual string APILink { get; set; }

        /// <summary>
        /// Link to the blog
        /// </summary>
        public virtual string BlogID { get; set; }

        #endregion

        #region Public Overridden Function

        /// <summary>
        /// To string function
        /// </summary>
        /// <returns>The API info</returns>
        public override string ToString()
        {
            return "<api name=\"" + Name + "\" preferred=\"" + Preferred.ToString() + "\" apiLink=\"" + APILink + "\" blogID=\"" + BlogID + "\"/>";
        }

        #endregion
    }
}