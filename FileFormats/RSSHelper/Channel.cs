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
    /// Channel item for RSS feeds
    /// </summary>
    public class Channel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Channel()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XML representation of the channel</param>
        public Channel(XmlElement Element)
        {
            if (!Element.Name.Equals("channel", StringComparison.CurrentCultureIgnoreCase))
                throw new ArgumentException("Element is not a channel");
            XmlNamespaceManager NamespaceManager = new XmlNamespaceManager(Element.OwnerDocument.NameTable);
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
            Node = Element.SelectSingleNode("./copyright", NamespaceManager);
            if (Node != null)
            {
                Copyright = Node.InnerText;
            }
            Node = Element.SelectSingleNode("./language", NamespaceManager);
            if (Node != null)
            {
                Language = Node.InnerText;
            }
            Node = Element.SelectSingleNode("./webmaster", NamespaceManager);
            if (Node != null)
            {
                WebMaster = Node.InnerText;
            }
            Node = Element.SelectSingleNode("./pubdate", NamespaceManager);
            if (Node != null)
            {
                PubDate = DateTime.Parse(Node.InnerText);
            }
            XmlNodeList Nodes = Element.SelectNodes("./category", NamespaceManager);
            foreach (XmlNode TempNode in Nodes)
            {
                Categories.Add(RSS.StripIllegalCharacters(TempNode.InnerText));
            }
            Node = Element.SelectSingleNode("./docs", NamespaceManager);
            if (Node != null)
            {
                Docs = Node.InnerText;
            }
            Node = Element.SelectSingleNode("./ttl", NamespaceManager);
            if (Node != null)
            {
                TTL = int.Parse(Node.InnerText);
            }
            Node = Element.SelectSingleNode("./image/url", NamespaceManager);
            if (Node != null)
            {
                ImageUrl = Node.InnerText;
            }
            Nodes = Element.SelectNodes("./item", NamespaceManager);
            foreach (XmlNode TempNode in Nodes)
            {
                Items.Add(new Item((XmlElement)TempNode));
            }
        }

        #endregion

        #region Private Variables
        private string _Title = string.Empty;
        private string _Link = string.Empty;
        private string _Description = string.Empty;
        private string _Copyright = "Copyright " + DateTime.Now.ToString("yyyy") + ". All rights reserved.";
        private string _Language = "en-us";
        private string _webMaster = string.Empty;
        private DateTime _pubDate = DateTime.Now;
        private List<string> _Categories = null;
        private string _Docs = "http://blogs.law.harvard.edu/tech/rss";
        private string _Cloud = string.Empty;
        private int _TTL = 5;
        private string _ImageUrl = string.Empty;
        private List<Item> _Items = null;
        private bool _Explicit = false;
        #endregion

        #region Properties
        /// <summary>
        /// Determines if this is explicit or not
        /// </summary>
        public bool Explicit
        {
            get
            {
                return _Explicit;
            }
            set { _Explicit = value; }
        }
        /// <summary>
        /// Items for this channel
        /// </summary>
        public List<Item> Items
        {
            get
            {
                if (_Items == null)
                {
                    _Items = new List<Item>();
                }
                return _Items;
            }
            set { _Items = value; }
        }
        /// <summary>
        /// Title of the channel
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = RSS.StripIllegalCharacters(value); }
        }

        /// <summary>
        /// Link to the website
        /// </summary>
        public string Link
        {
            get { return _Link; }
            set { _Link = value; }
        }

        /// <summary>
        /// Description of the channel
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = RSS.StripIllegalCharacters(value); }
        }

        /// <summary>
        /// Copyright info
        /// </summary>
        public string Copyright
        {
            get { return _Copyright; }
            set { _Copyright = value; }
        }

        /// <summary>
        /// Language it is in
        /// </summary>
        public string Language
        {
            get { return _Language; }
            set { _Language = value; }
        }

        /// <summary>
        /// Web Master info
        /// </summary>
        public string WebMaster
        {
            get { return _webMaster; }
            set { _webMaster = value; }
        }

        /// <summary>
        /// Date the channel was published
        /// </summary>
        public DateTime PubDate
        {
            get { return _pubDate; }
            set { _pubDate = value; }
        }

        /// <summary>
        /// Categories associated with this channel
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
        /// Document describing the file format
        /// </summary>
        public string Docs
        {
            get { return _Docs; }
            set { _Docs = value; }
        }

        /// <summary>
        /// Cloud information
        /// </summary>
        public string Cloud
        {
            get { return _Cloud; }
            set { _Cloud = value; }
        }

        /// <summary>
        /// Time to live... Amount of time between updates.
        /// </summary>
        public int TTL
        {
            get { return _TTL; }
            set { _TTL = value; }
        }

        /// <summary>
        /// Url pointing to the image/logo associated with the channel
        /// </summary>
        public string ImageUrl
        {
            get { return _ImageUrl; }
            set { _ImageUrl = value; }
        }
        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            StringBuilder ChannelString = new StringBuilder();
            ChannelString.Append("<channel>");
            ChannelString.Append("<title>").Append(Title).Append("</title>\r\n");
            ChannelString.Append("<link>").Append(Link).Append("</link>\r\n");
            ChannelString.Append("<atom:link xmlns:atom=\"http://www.w3.org/2005/Atom\" rel=\"self\" href=\"").Append(Link).Append("\" type=\"application/rss).Append(xml\" />");

            ChannelString.Append("<description><![CDATA[").Append(Description).Append("]]></description>\r\n");
            ChannelString.Append("<language>").Append(Language).Append("</language>\r\n");
            ChannelString.Append("<copyright>").Append(Copyright).Append("</copyright>\r\n");
            ChannelString.Append("<webMaster>").Append(WebMaster).Append("</webMaster>\r\n");
            ChannelString.Append("<pubDate>").Append(PubDate.ToString("Ddd, dd MMM yyyy HH':'mm':'ss")).Append("</pubDate>\r\n");
            ChannelString.Append("<itunes:explicit>").Append((Explicit ? "yes" : "no")).Append("</itunes:explicit>");
            ChannelString.Append("<itunes:subtitle>").Append(Title).Append("</itunes:subtitle>");
            ChannelString.Append("<itunes:summary><![CDATA[").Append(Description).Append("]]></itunes:summary>");

            foreach (string Category in Categories)
            {
                ChannelString.Append("<category>").Append(Category).Append("</category>\r\n");
                ChannelString.Append("<itunes:category text=\"").Append(Category).Append("\" />\r\n");
            }
            ChannelString.Append("<docs>").Append(Docs).Append("</docs>\r\n");
            ChannelString.Append("<ttl>").Append(TTL.ToString()).Append("</ttl>\r\n");
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

        #endregion
    }
}