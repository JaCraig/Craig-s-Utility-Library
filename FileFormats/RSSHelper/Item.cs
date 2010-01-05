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
using System.Collections.Generic;
using System.Text;
using System.Xml;
#endregion

namespace Utilities.FileFormats.RSSHelper
{
    /// <summary>
    /// Item class for RSS feeds
    /// </summary>
    public class Item
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Item()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XML element containing the item content</param>
        public Item(XmlElement Element)
        {
            if (Element.Name.Equals("item", StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (XmlNode Child in Element.ChildNodes)
                {
                    try
                    {
                        if (Child.Name.Equals("title", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Title = Child.InnerText;
                        }
                        else if (Child.Name.Equals("link", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Link = Child.InnerText;
                        }
                        else if (Child.Name.Equals("description", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Description = Child.InnerText;
                        }
                        else if (Child.Name.Equals("author", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Author = Child.InnerText;
                        }
                        else if (Child.Name.Equals("category", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Categories.Add(RSS.StripIllegalCharacters(Child.InnerText));
                        }
                        else if (Child.Name.Equals("enclosure", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _Enclosure = new Enclosure((XmlElement)Child);
                        }
                        else if (Child.Name.Equals("pubDate", StringComparison.CurrentCultureIgnoreCase))
                        {
                            PubDate = DateTime.Parse(Child.InnerText);
                        }
                        else if (Child.Name.Equals("media:thumbnail", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Thumbnail = new Thumbnail((XmlElement)Child);
                        }
                        else if (Child.Name.Equals("guid", StringComparison.CurrentCultureIgnoreCase))
                        {
                            GUID = new GUID((XmlElement)Child);
                        }
                    }
                    catch { }
                }
            }
        }

        #endregion

        #region Private Variables
        private string _Title = string.Empty;
        private string _Link = string.Empty;
        private string _Description = string.Empty;
        private string _Author = string.Empty;
        private Thumbnail _Thumbnail = null;
        private List<string> _Categories = null;
        private DateTime _pubDate = DateTime.Now;
        private Enclosure _Enclosure = null;
        #endregion

        #region Properties

        /// <summary>
        /// GUID for the item
        /// </summary>
        public GUID GUID { get; set; }

        /// <summary>
        /// Thumbnail information
        /// </summary>
        public Thumbnail Thumbnail
        {
            get 
            {
                if (_Thumbnail == null)
                {
                    _Thumbnail = new Thumbnail();
                }
                return _Thumbnail; 
            }
            set { _Thumbnail = value; }
        }

        /// <summary>
        /// Title of the item
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = RSS.StripIllegalCharacters(value); }
        }

        /// <summary>
        /// Link to its location
        /// </summary>
        public string Link
        {
            get { return _Link; }
            set { _Link = value; }
        }

        /// <summary>
        /// Description of the item
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = RSS.StripIllegalCharacters(value); }
        }

        /// <summary>
        /// Author of the item
        /// </summary>
        public string Author
        {
            get { return _Author; }
            set { _Author = RSS.StripIllegalCharacters(value); }
        }

        /// <summary>
        /// Categories associated with the item
        /// </summary>
        public List<string> Categories
        {
            get
            {
                if (_Categories == null)
                {
                    _Categories = new List<string>();
                }
                return _Categories;
            }
            set { _Categories = value; }
        }

        /// <summary>
        /// Date it was published
        /// </summary>
        public DateTime PubDate
        {
            get { return _pubDate; }
            set { _pubDate = value; }
        }

        /// <summary>
        /// Enclosure (used for podcasting)
        /// </summary>
        public Enclosure Enclosure
        {
            get
            {
                if (_Enclosure == null)
                {
                    _Enclosure = new Enclosure();
                }
                return _Enclosure;
            }
            set { _Enclosure = value; }
        }
        #endregion

        #region Public Overridden Function
        /// <summary>
        /// Outputs a string ready for RSS
        /// </summary>
        /// <returns>A string formatted for RSS</returns>
        public override string ToString()
        {
            StringBuilder ItemString = new StringBuilder();
            ItemString.Append("<item><title>" + Title + "</title>\r\n<link>" + Link + "</link>\r\n<author>" + Author + "</author>\r\n");
            foreach (string Category in Categories)
            {
                ItemString.Append("<category>" + Category + "</category>\r\n");
            }
            ItemString.Append("<pubDate>" + PubDate.ToString("r") + "</pubDate>\r\n");
            ItemString.Append(Enclosure.ToString());
            ItemString.Append(Thumbnail.ToString());
            ItemString.Append("<description><![CDATA[" + Description + "]]></description>\r\n");
            ItemString.Append(GUID.ToString());
            ItemString.Append("<itunes:subtitle>" + Title + "</itunes:subtitle>");
            ItemString.Append("<itunes:summary><![CDATA[" + Description + "]]></itunes:summary>");
            ItemString.Append("</item>\r\n");
            return ItemString.ToString();
        }
        #endregion
    }
}
