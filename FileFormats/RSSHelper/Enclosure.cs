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
using System.Xml;
#endregion

namespace Utilities.FileFormats.RSSHelper
{
    /// <summary>
    /// Enclosure class for RSS feeds (used for pod casting)
    /// </summary>
    public class Enclosure
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Enclosure()
        {

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XML element holding info for the enclosure</param>
        public Enclosure(XmlElement Element)
        {
            try
            {
                if (Element.Name.Equals("enclosure", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (Element.Attributes["url"] != null)
                    {
                        Url = Element.Attributes["url"].Value;
                    }
                    if (Element.Attributes["length"] != null)
                    {
                        Length = Element.Attributes["length"].Value;
                    }
                    if (Element.Attributes["type"] != null)
                    {
                        Type = Element.Attributes["type"].Value;
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Private Variables
        private string _Url = string.Empty;
        private string _Length = string.Empty;
        private string _Type = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Location of the item
        /// </summary>
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }

        /// <summary>
        /// Size in bytes
        /// </summary>
        public string Length
        {
            get { return _Length; }
            set { _Length = value; }
        }

        /// <summary>
        /// File type
        /// </summary>
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        #endregion

        #region Public Overridden Functions
        /// <summary>
        /// to string item. Used for outputting the item to RSS.
        /// </summary>
        /// <returns>A string formatted for RSS output</returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_Url) && !string.IsNullOrEmpty(_Type) && !string.IsNullOrEmpty(_Length))
            {
                return "<enclosure url=\"" + _Url + "\" length=\"" + _Length + "\" type=\"" + _Type + "\" />\r\n"
                    + "<media:content url=\"" + _Url + "\" fileSize=\"" + _Length + "\" type=\"" + _Type + "\" />";

            }
            return string.Empty;
        }
        #endregion
    }
}
