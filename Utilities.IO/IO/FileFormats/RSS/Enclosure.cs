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
    /// Enclosure class for RSS feeds (used for pod casting)
    /// </summary>
    public class Enclosure
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Enclosure()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Doc">XML element holding info for the enclosure</param>
        public Enclosure(IXPathNavigable Doc)
        {
            Contract.Requires<ArgumentNullException>(Doc != null, "Doc");
            XPathNavigator Element = Doc.CreateNavigator();
            if (string.IsNullOrEmpty(Element.GetAttribute("url", "")))
            {
                Url = Element.GetAttribute("url", "");
            }
            if (string.IsNullOrEmpty(Element.GetAttribute("length", "")))
            {
                Length = Element.GetAttribute("length", "");
            }
            if (string.IsNullOrEmpty(Element.GetAttribute("type", "")))
            {
                Type = Element.GetAttribute("type", "");
            }
        }

        /// <summary>
        /// Size in bytes
        /// </summary>
        public virtual string Length { get; set; }

        /// <summary>
        /// File type
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// Location of the item
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// to string item. Used for outputting the item to RSS.
        /// </summary>
        /// <returns>A string formatted for RSS output</returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Url) && !string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Length))
            {
                return "<enclosure url=\"" + Url + "\" length=\"" + Length + "\" type=\"" + Type + "\" />\r\n"
                    + "<media:content url=\"" + Url + "\" fileSize=\"" + Length + "\" type=\"" + Type + "\" />";
            }
            return string.Empty;
        }
    }
}