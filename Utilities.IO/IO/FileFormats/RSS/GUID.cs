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

using System;
using System.Diagnostics.Contracts;
using System.Xml.XPath;

namespace Utilities.IO.FileFormats.RSS
{
    /// <summary>
    /// GUID info holder
    /// </summary>
    public class GUID
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GUID()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XML element holding info for the enclosure</param>
        public GUID(IXPathNavigable Element)
        {
            Contract.Requires<ArgumentNullException>(Element != null, "Element");
            XPathNavigator Navigator = Element.CreateNavigator();
            if (Navigator.GetAttribute("isPermaLink", "") != null)
            {
                PermaLink = Navigator.GetAttribute("isPermaLink", "");
            }
            GUIDText = Navigator.Value;
        }

        /// <summary>
        /// GUID Text
        /// </summary>
        public virtual string GUIDText { get; set; }

        /// <summary>
        /// Is this a perma link?
        /// </summary>
        public virtual string PermaLink { get; set; }

        /// <summary>
        /// to string item. Used for outputting the item to RSS.
        /// </summary>
        /// <returns>A string formatted for RSS output</returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(GUIDText))
            {
                return "<guid" + (string.IsNullOrEmpty(PermaLink) ? " IsPermaLink=" + PermaLink : " IsPermaLink='False'") + ">" + GUIDText + "</guid>\r\n";
            }
            return string.Empty;
        }
    }
}