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
    /// Thumbnail info holder
    /// </summary>
    public class Thumbnail
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Thumbnail()
        {

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XML element holding info for the enclosure</param>
        public Thumbnail(XmlElement Element)
        {
            try
            {
                if (Element.Name.Equals("media:thumbnail", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (Element.Attributes["url"] != null)
                    {
                        Url = Element.Attributes["url"].Value;
                    }
                    if (Element.Attributes["width"] != null)
                    {
                        Width = int.Parse(Element.Attributes["width"].Value);
                    }
                    if (Element.Attributes["height"] != null)
                    {
                        Height = int.Parse(Element.Attributes["height"].Value);
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Private Variables

        private string _Url = string.Empty;
        private int _Width = 1;
        private int _Height = 1;

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
        /// Image width
        /// </summary>
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        /// <summary>
        /// Image height
        /// </summary>
        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        #endregion

        #region Public Overridden Functions

        /// <summary>
        /// to string item. Used for outputting the item to RSS.
        /// </summary>
        /// <returns>A string formatted for RSS output</returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_Url))
            {
                return "<media:thumbnail url=\"" + _Url + "\" width=\"" + _Width.ToString() + "\" height=\"" + _Height + "\" />\r\n";
            }
            return string.Empty;
        }

        #endregion
    }
}
