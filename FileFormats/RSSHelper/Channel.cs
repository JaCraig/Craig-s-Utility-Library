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
            if (Element.Name.Equals("channel", StringComparison.CurrentCultureIgnoreCase))
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
                        else if (Child.Name.Equals("copyright", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Copyright = Child.InnerText;
                        }
                        else if (Child.Name.Equals("language", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Language = Child.InnerText;
                        }
                        else if (Child.Name.Equals("webmaster", StringComparison.CurrentCultureIgnoreCase))
                        {
                            WebMaster = Child.InnerText;
                        }
                        else if (Child.Name.Equals("pubdate", StringComparison.CurrentCultureIgnoreCase))
                        {
                            PubDate = DateTime.Parse(Child.InnerText);
                        }
                        else if (Child.Name.Equals("category", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Categories.Add(RSS.StripIllegalCharacters(Child.InnerText));
                        }
                        else if (Child.Name.Equals("docs", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Docs = Child.InnerText;
                        }
                        else if (Child.Name.Equals("ttl", StringComparison.CurrentCultureIgnoreCase))
                        {
                            TTL = int.Parse(Child.InnerText);
                        }
                        else if (Child.Name.Equals("image", StringComparison.CurrentCultureIgnoreCase))
                        {
                            foreach (XmlNode SubChild in Child.ChildNodes)
                            {
                                if (SubChild.Name.Equals("url", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    ImageUrl = SubChild.InnerText;
                                }
                            }
                        }
                        else if (Child.Name.Equals("item", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Items.Add(new Item((XmlElement)Child));
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

        public override string ToString()
        {
            StringBuilder ChannelString = new StringBuilder();
            ChannelString.Append("<channel>");
            ChannelString.Append("<title>" + Title + "</title>\r\n");
            ChannelString.Append("<link>" + Link + "</link>\r\n");
            ChannelString.Append("<atom:link xmlns:atom=\"http://www.w3.org/2005/Atom\" rel=\"self\" href=\"" + Link + "\" type=\"application/rss+xml\" />");

            ChannelString.Append("<description><![CDATA[" + Description + "]]></description>\r\n");
            ChannelString.Append("<language>" + Language + "</language>\r\n");
            ChannelString.Append("<copyright>" + Copyright + "</copyright>\r\n");
            ChannelString.Append("<webMaster>" + WebMaster + "</webMaster>\r\n");
            ChannelString.Append("<pubDate>" + PubDate.ToString("Ddd, dd MMM yyyy HH':'mm':'ss") + "</pubDate>\r\n");
            ChannelString.Append("<itunes:explicit>" + (Explicit ? "yes" : "no") + "</itunes:explicit>");
            ChannelString.Append("<itunes:subtitle>" + Title + "</itunes:subtitle>");
            ChannelString.Append("<itunes:summary><![CDATA[" + Description + "]]></itunes:summary>");
            
            foreach (string Category in Categories)
            {
                ChannelString.Append("<category>" + Category + "</category>\r\n");
                ChannelString.Append("<itunes:category text=\"" + Category + "\" />\r\n");
            }
            ChannelString.Append("<docs>" + Docs + "</docs>\r\n");
            ChannelString.Append("<ttl>" + TTL.ToString() + "</ttl>\r\n");
            if (!string.IsNullOrEmpty(ImageUrl))
            {
                ChannelString.Append("<image><url>" + ImageUrl + "</url>\r\n<title>" + Title + "</title>\r\n<link>" + Link + "</link>\r\n</image>\r\n");
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
