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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Utilities.IO.FileFormats.RSS
{
    /// <summary>
    /// Item class for RSS feeds
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Item()
        {
            this.Categories = new List<string>();
            PubDate = DateTime.Now;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Doc">XML element containing the item content</param>
        public Item(IXPathNavigable Doc)
            : this()
        {
            Contract.Requires<ArgumentNullException>(Doc != null, "Doc");
            XPathNavigator Element = Doc.CreateNavigator();
            var NamespaceManager = new XmlNamespaceManager(Element.NameTable);
            NamespaceManager.AddNamespace("media", "http://search.yahoo.com/mrss/");
            XPathNavigator Node = Element.SelectSingleNode("./title", NamespaceManager);
            if (Node != null)
            {
                Title = Node.Value;
            }
            Node = Element.SelectSingleNode("./link", NamespaceManager);
            if (Node != null)
            {
                Link = Node.Value;
            }
            Node = Element.SelectSingleNode("./description", NamespaceManager);
            if (Node != null)
            {
                Description = Node.Value;
            }
            Node = Element.SelectSingleNode("./author", NamespaceManager);
            if (Node != null)
            {
                Author = Node.Value;
            }
            XPathNodeIterator Nodes = Element.Select("./category", NamespaceManager);
            foreach (XmlNode TempNode in Nodes)
            {
                Categories.Add(Utils.StripIllegalCharacters(TempNode.Value));
            }
            Node = Element.SelectSingleNode("./enclosure", NamespaceManager);
            if (Node != null)
            {
                Enclosure = new Enclosure(Node);
            }
            Node = Element.SelectSingleNode("./pubdate", NamespaceManager);
            if (Node != null)
            {
                PubDate = DateTime.Parse(Node.Value, CultureInfo.InvariantCulture);
            }
            Node = Element.SelectSingleNode("./media:thumbnail", NamespaceManager);
            if (Node != null)
            {
                Thumbnail = new Thumbnail(Node);
            }
            Node = Element.SelectSingleNode("./guid", NamespaceManager);
            if (Node != null)
            {
                GUID = new GUID(Node);
            }
        }

        /// <summary>
        /// Author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Categories
        /// </summary>
        public ICollection<string> Categories { get; private set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Enclosure
        /// </summary>
        public Enclosure Enclosure { get; set; }

        /// <summary>
        /// GUID for the item
        /// </summary>
        public virtual GUID GUID { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Publication date
        /// </summary>
        public DateTime PubDate { get; set; }

        /// <summary>
        /// Thumbnail
        /// </summary>
        public Thumbnail Thumbnail { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Outputs a string ready for RSS
        /// </summary>
        /// <returns>A string formatted for RSS</returns>
        public override string ToString()
        {
            var ItemString = new StringBuilder();
            ItemString.Append("<item><title>").Append(Title).Append("</title>\r\n<link>")
                .Append(Link).Append("</link>\r\n<author>").Append(Author)
                .Append("</author>\r\n");
            foreach (string Category in Categories)
            {
                ItemString.Append("<category>").Append(Category).Append("</category>\r\n");
            }
            ItemString.Append("<pubDate>").Append(PubDate.ToString("r", CultureInfo.InvariantCulture)).Append("</pubDate>\r\n");
            if (Enclosure != null)
                ItemString.Append(Enclosure.ToString());
            if (Thumbnail != null)
                ItemString.Append(Thumbnail.ToString());
            ItemString.Append("<description><![CDATA[").Append(Description).Append("]]></description>\r\n");
            if (GUID != null)
                ItemString.Append(GUID.ToString());
            ItemString.Append("<itunes:subtitle>").Append(Title).Append("</itunes:subtitle>");
            ItemString.Append("<itunes:summary><![CDATA[").Append(Description).Append("]]></itunes:summary>");
            ItemString.Append("</item>\r\n");
            return ItemString.ToString();
        }
    }
}