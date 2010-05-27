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
    /// RSS document class
    /// </summary>
    public class Document
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Document()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Location">Location of the RSS feed to load</param>
        public Document(string Location)
        {
            try
            {
                XmlDocument Document = new XmlDocument();
                Document.Load(Location);
                foreach (XmlNode Children in Document.ChildNodes)
                {
                    if (Children.Name.Equals("rss", StringComparison.CurrentCultureIgnoreCase))
                    {
                        foreach (XmlNode Child in Children.ChildNodes)
                        {
                            if (Child.Name.Equals("channel", StringComparison.CurrentCultureIgnoreCase))
                            {
                                Channels.Add(new Channel((XmlElement)Child));
                            }
                        }
                    }
                    else if (Children.Name.Equals("rdf:rdf", StringComparison.CurrentCultureIgnoreCase))
                    {
                        List<Item> Items = new List<Item>();
                        foreach (XmlNode Child in Children.ChildNodes)
                        {
                            if (Child.Name.Equals("channel", StringComparison.CurrentCultureIgnoreCase))
                            {
                                Channels.Add(new Channel((XmlElement)Child));
                            }
                            else if (Child.Name.Equals("item", StringComparison.CurrentCultureIgnoreCase))
                            {
                                Items.Add(new Item((XmlElement)Child));
                            }
                        }
                        if (Channels.Count > 0)
                        {
                            Channels[0].Items = Items;
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Document">XML document containing an RSS feed</param>
        public Document(XmlDocument Document)
        {
            foreach (XmlNode Children in Document.ChildNodes)
            {
                if (Children.Name.Equals("rss", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (XmlNode Child in Children.ChildNodes)
                    {
                        if (Child.Name.Equals("channel", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Channels.Add(new Channel((XmlElement)Child));
                        }
                    }
                }
                else if (Children.Name.Equals("rdf:rdf", StringComparison.CurrentCultureIgnoreCase))
                {
                    List<Item> Items = new List<Item>();
                    foreach (XmlNode Child in Children.ChildNodes)
                    {
                        if (Child.Name.Equals("channel", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Channels.Add(new Channel((XmlElement)Child));
                        }
                        else if (Child.Name.Equals("item", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Items.Add(new Item((XmlElement)Child));
                        }
                    }
                    if (Channels.Count > 0)
                    {
                        Channels[0].Items = Items;
                    }
                }
            }
        }
        #endregion

        #region Private Variables
        private List<Channel> _Channels = null;
        #endregion

        #region Properties
        /// <summary>
        /// Channels for the RSS feed
        /// </summary>
        public List<Channel> Channels
        {
            get
            {
                if (_Channels == null)
                {
                    _Channels = new List<Channel>();
                }
                return _Channels;
            }
            set { _Channels = value; }
        }
        #endregion

        #region Public Overridden Functions
        /// <summary>
        /// string representation of the RSS feed.
        /// </summary>
        /// <returns>An rss formatted string</returns>
        public override string ToString()
        {
            StringBuilder DocumentString = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<rss xmlns:itunes=\"http://www.itunes.com/dtds/podcast-1.0.dtd\" xmlns:media=\"http://search.yahoo.com/mrss/\" version=\"2.0\">\r\n");
            foreach(Channel CurrentChannel in Channels)
            {
                DocumentString.Append(CurrentChannel.ToString());
            }
            DocumentString.Append("</rss>");
            return DocumentString.ToString();
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Copies one document's channels to another
        /// </summary>
        /// <param name="CopyFrom">RSS document to copy from</param>
        public void Copy(Document CopyFrom)
        {
            foreach (Channel CurrentChannel in CopyFrom.Channels)
            {
                Channels.Add(CurrentChannel);
            }
        }
        #endregion

    }
}