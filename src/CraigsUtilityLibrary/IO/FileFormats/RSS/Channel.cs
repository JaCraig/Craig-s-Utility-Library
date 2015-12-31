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
    /// Channel item for RSS feeds
    /// </summary>
    public class Channel : IList<Item>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Channel()
        {
            Copyright = "Copyright " + DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture) + ". All rights reserved.";
            Language = "en-us";
            PubDate = DateTime.Now;
            Categories = new List<string>();
            Docs = "http://blogs.law.harvard.edu/tech/rss";
            TTL = 5;
            Items = new List<Item>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Doc">XML representation of the channel</param>
        public Channel(IXPathNavigable Doc)
            : this()
        {
            Contract.Requires<ArgumentNullException>(Doc != null, "Element");
            XPathNavigator Element = Doc.CreateNavigator();
            if (!Element.Name.Equals("channel", StringComparison.CurrentCultureIgnoreCase))
                throw new ArgumentException("Element is not a channel");
            var NamespaceManager = new XmlNamespaceManager(Element.NameTable);
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
            Node = Element.SelectSingleNode("./copyright", NamespaceManager);
            if (Node != null)
            {
                Copyright = Node.Value;
            }
            Node = Element.SelectSingleNode("./language", NamespaceManager);
            if (Node != null)
            {
                Language = Node.Value;
            }
            Node = Element.SelectSingleNode("./webmaster", NamespaceManager);
            if (Node != null)
            {
                WebMaster = Node.Value;
            }
            Node = Element.SelectSingleNode("./pubdate", NamespaceManager);
            if (Node != null)
            {
                PubDate = DateTime.Parse(Node.Value, CultureInfo.InvariantCulture);
            }
            XPathNodeIterator Nodes = Element.Select("./category", NamespaceManager);
            foreach (XmlNode TempNode in Nodes)
            {
                Categories.Add(Utils.StripIllegalCharacters(TempNode.Value));
            }
            Node = Element.SelectSingleNode("./docs", NamespaceManager);
            if (Node != null)
            {
                Docs = Node.Value;
            }
            Node = Element.SelectSingleNode("./ttl", NamespaceManager);
            if (Node != null)
            {
                TTL = int.Parse(Node.Value, CultureInfo.InvariantCulture);
            }
            Node = Element.SelectSingleNode("./image/url", NamespaceManager);
            if (Node != null)
            {
                ImageUrl = Node.Value;
            }
            Nodes = Element.Select("./item", NamespaceManager);
            foreach (XmlNode TempNode in Nodes)
            {
                Items.Add(new Item((XmlElement)TempNode));
            }
        }

        /// <summary>
        /// Categories
        /// </summary>
        public ICollection<string> Categories { get; private set; }

        /// <summary>
        /// Cloud
        /// </summary>
        public string Cloud { get; set; }

        /// <summary>
        /// Copyright
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// Count
        /// </summary>
        public int Count
        {
            get { return Items.Count; }
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Docs
        /// </summary>
        public string Docs { get; set; }

        /// <summary>
        /// Explicit
        /// </summary>
        public bool Explicit { get; set; }

        /// <summary>
        /// Image URL
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Is read only?
        /// </summary>
        public bool IsReadOnly
        {
            get { return Items.IsReadOnly; }
        }

        /// <summary>
        /// Language
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Publish date
        /// </summary>
        public DateTime PubDate { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// TTL
        /// </summary>
        public int TTL { get; set; }

        /// <summary>
        /// Web master
        /// </summary>
        public string WebMaster { get; set; }

        /// <summary>
        /// Items
        /// </summary>
        protected IList<Item> Items { get; private set; }

        /// <summary>
        /// Gets/sets an item at a specific index
        /// </summary>
        /// <param name="index">index to get/set</param>
        /// <returns>Item specified</returns>
        public Item this[int index]
        {
            get
            {
                return Items[index];
            }
            set
            {
                Items[index] = value;
            }
        }

        /// <summary>
        /// Adds an item
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(Item item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Clears the items from the channel
        /// </summary>
        public void Clear()
        {
            Items.Clear();
        }

        /// <summary>
        /// Does the channel contain a specific item?
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>True if it is there, false otherwise</returns>
        public bool Contains(Item item)
        {
            return Items.Contains(item);
        }

        /// <summary>
        /// Copies the items to an array
        /// </summary>
        /// <param name="array">array to copy to</param>
        /// <param name="arrayIndex">Array index</param>
        public void CopyTo(Item[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator for the channel</returns>
        public IEnumerator<Item> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Index of the item specified
        /// </summary>
        /// <param name="item">Item specified</param>
        /// <returns>The index of the item</returns>
        public int IndexOf(Item item)
        {
            return Items.IndexOf(item);
        }

        /// <summary>
        /// Inserts the item at a specific index
        /// </summary>
        /// <param name="index">Index to insert at</param>
        /// <param name="item">Item to insert</param>
        public void Insert(int index, Item item)
        {
            Items.Insert(index, item);
        }

        /// <summary>
        /// Removes an item from the channel
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(Item item)
        {
            return Items.Remove(item);
        }

        /// <summary>
        /// Removes an item at a specific index
        /// </summary>
        /// <param name="index">Index to remove at</param>
        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator for the channel</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Converts the channel to a string
        /// </summary>
        /// <returns>The channel as a string</returns>
        public override string ToString()
        {
            var ChannelString = new StringBuilder();
            ChannelString.Append("<channel>");
            ChannelString.Append("<title>").Append(Utils.StripIllegalCharacters(Title)).Append("</title>\r\n");
            ChannelString.Append("<link>").Append(Link).Append("</link>\r\n");
            ChannelString.Append("<atom:link xmlns:atom=\"http://www.w3.org/2005/Atom\" rel=\"self\" href=\"").Append(Link).Append("\" type=\"application/rss+xml\" />");

            ChannelString.Append("<description><![CDATA[").Append(Utils.StripIllegalCharacters(Description)).Append("]]></description>\r\n");
            ChannelString.Append("<language>").Append(Language).Append("</language>\r\n");
            ChannelString.Append("<copyright>").Append(Copyright).Append("</copyright>\r\n");
            ChannelString.Append("<webMaster>").Append(WebMaster).Append("</webMaster>\r\n");
            ChannelString.Append("<pubDate>").Append(PubDate.ToString("Ddd, dd MMM yyyy HH':'mm':'ss", CultureInfo.InvariantCulture)).Append("</pubDate>\r\n");
            ChannelString.Append("<itunes:explicit>").Append((Explicit ? "yes" : "no")).Append("</itunes:explicit>");
            ChannelString.Append("<itunes:subtitle>").Append(Utils.StripIllegalCharacters(Title)).Append("</itunes:subtitle>");
            ChannelString.Append("<itunes:summary><![CDATA[").Append(Utils.StripIllegalCharacters(Description)).Append("]]></itunes:summary>");

            foreach (string Category in Categories)
            {
                ChannelString.Append("<category>").Append(Category).Append("</category>\r\n");
                ChannelString.Append("<itunes:category text=\"").Append(Category).Append("\" />\r\n");
            }
            ChannelString.Append("<docs>").Append(Docs).Append("</docs>\r\n");
            ChannelString.Append("<ttl>").Append(TTL.ToString(CultureInfo.InvariantCulture)).Append("</ttl>\r\n");
            if (!string.IsNullOrEmpty(ImageUrl))
            {
                ChannelString.Append("<image><url>").Append(ImageUrl).Append("</url>\r\n<title>").Append(Title).Append("</title>\r\n<link>").Append(Link).Append("</link>\r\n</image>\r\n");
            }
            foreach (Item CurrentItem in Items)
            {
                ChannelString.Append(CurrentItem.ToString());
            }
            ChannelString.Append("</channel>\r\n");
            return ChannelString.ToString();
        }
    }
}