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
            if (!Element.Name.Equals("item", StringComparison.CurrentCultureIgnoreCase))
                throw new ArgumentException("Element is not an item");
            XmlNamespaceManager NamespaceManager = new XmlNamespaceManager(Element.OwnerDocument.NameTable);
            NamespaceManager.AddNamespace("media", "http://search.yahoo.com/mrss/");
            XmlNode Node = Element.SelectSingleNode("./title", NamespaceManager);
            if (Node != null)
            {
                Title = Node.InnerText;
            }
            Node = Element.SelectSingleNode("./link", NamespaceManager);
            if (Node != null)
            {
                Link = Node.InnerText;
            }
            Node = Element.SelectSingleNode("./description", NamespaceManager);
            if (Node != null)
            {
                Description = Node.InnerText;
            }
            Node = Element.SelectSingleNode("./author", NamespaceManager);
            if (Node != null)
            {
                Author = Node.InnerText;
            }
            XmlNodeList Nodes = Element.SelectNodes("./category", NamespaceManager);
            foreach (XmlNode TempNode in Nodes)
            {
                Categories.Add(RSS.StripIllegalCharacters(TempNode.InnerText));
            }
            Node = Element.SelectSingleNode("./enclosure", NamespaceManager);
            if (Node != null)
            {
                Enclosure = new Enclosure((XmlElement)Node);
            }
            Node = Element.SelectSingleNode("./pubdate", NamespaceManager);
            if (Node != null)
            {
                PubDate = DateTime.Parse(Node.InnerText);
            }
            Node = Element.SelectSingleNode("./media:thumbnail", NamespaceManager);
            if (Node != null)
            {
                Thumbnail = new Thumbnail((XmlElement)Node);
            }
            Node = Element.SelectSingleNode("./guid", NamespaceManager);
            if (Node != null)
            {
                GUID = new GUID((XmlElement)Node);
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
            ItemString.Append("<item><title>").Append(Title).Append("</title>\r\n<link>")
                .Append(Link).Append("</link>\r\n<author>").Append(Author)
                .Append("</author>\r\n");
            foreach (string Category in Categories)
            {
                ItemString.Append("<category>").Append(Category).Append("</category>\r\n");
            }
            ItemString.Append("<pubDate>").Append(PubDate.ToString("r")).Append("</pubDate>\r\n");
            ItemString.Append(Enclosure.ToString());
            ItemString.Append(Thumbnail.ToString());
            ItemString.Append("<description><![CDATA[").Append(Description).Append("]]></description>\r\n");
            ItemString.Append(GUID.ToString());
            ItemString.Append("<itunes:subtitle>").Append(Title).Append("</itunes:subtitle>");
            ItemString.Append("<itunes:summary><![CDATA[").Append(Description).Append("]]></itunes:summary>");
            ItemString.Append("</item>\r\n");
            return ItemString.ToString();
        }

        #endregion
    }
}