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
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Utilities.DataTypes;
using Utilities.IO.FileFormats.BaseClasses;

namespace Utilities.IO.FileFormats.RSS
{
    /// <summary>
    /// RSS document class
    /// </summary>
    public class Document : StringFormatBase<Document>, IList<Channel>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Document()
        {
            Channels = new List<Channel>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Location">Location of the RSS feed to load</param>
        public Document(string Location)
            : this()
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(Location), "Location");
            InternalLoad(Location);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Document">XML document containing an RSS feed</param>
        public Document(IXPathNavigable Document)
            : this()
        {
            Contract.Requires<ArgumentNullException>(Document != null, "Document");
            Load(Document);
        }

        /// <summary>
        /// Channel count
        /// </summary>
        public int Count
        {
            get { return Channels.Count; }
        }

        /// <summary>
        /// Determines if it is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return Channels.IsReadOnly; }
        }

        /// <summary>
        /// Channels for the RSS feed
        /// </summary>
        protected IList<Channel> Channels { get; private set; }

        /// <summary>
        /// Gets/sets a channel
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>The channel specified</returns>
        public Channel this[int index]
        {
            get
            {
                return Channels[index];
            }
            set
            {
                Channels[index] = value;
            }
        }

        /// <summary>
        /// Adds a channel to the document
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(Channel item)
        {
            Channels.Add(item);
        }

        /// <summary>
        /// Clears the document
        /// </summary>
        public void Clear()
        {
            Channels.Clear();
        }

        /// <summary>
        /// Does this contain the channel specified
        /// </summary>
        /// <param name="item">Channel to check</param>
        /// <returns>True if it does, false otherwise</returns>
        public bool Contains(Channel item)
        {
            return Channels.Contains(item);
        }

        /// <summary>
        /// Copies one document's channels to another
        /// </summary>
        /// <param name="CopyFrom">RSS document to copy from</param>
        public virtual void Copy(Document CopyFrom)
        {
            Contract.Requires<ArgumentNullException>(CopyFrom != null, "CopyFrom");
            foreach (Channel CurrentChannel in CopyFrom)
            {
                Channels.Add(CurrentChannel);
            }
        }

        /// <summary>
        /// Copies the channels to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Array starting index</param>
        public void CopyTo(Channel[] array, int arrayIndex)
        {
            Channels.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>Enumerator of the document</returns>
        public IEnumerator<Channel> GetEnumerator()
        {
            return Channels.GetEnumerator();
        }

        /// <summary>
        /// Index of the item specified
        /// </summary>
        /// <param name="item">Channel</param>
        /// <returns>Index of the channel</returns>
        public int IndexOf(Channel item)
        {
            return Channels.IndexOf(item);
        }

        /// <summary>
        /// Inserts a channel
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="item">Channel item</param>
        public void Insert(int index, Channel item)
        {
            Channels.Insert(index, item);
        }

        /// <summary>
        /// Removes an item from the document
        /// </summary>
        /// <param name="item">Channel to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(Channel item)
        {
            return Channels.Remove(item);
        }

        /// <summary>
        /// Remove channel at a specific index
        /// </summary>
        /// <param name="index">Index to remove</param>
        public void RemoveAt(int index)
        {
            Channels.RemoveAt(index);
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>Enumerator of the document</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Channels.GetEnumerator();
        }

        /// <summary>
        /// string representation of the RSS feed.
        /// </summary>
        /// <returns>An rss formatted string</returns>
        public override string ToString()
        {
            return new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<rss xmlns:itunes=\"http://www.itunes.com/dtds/podcast-1.0.dtd\" xmlns:media=\"http://search.yahoo.com/mrss/\" version=\"2.0\">\r\n")
                .Append(Channels.ToString(x => x.ToString(), ""))
                .Append("</rss>")
                .ToString();
        }

        /// <summary>
        /// Loads the object from the data specified
        /// </summary>
        /// <param name="Data">Data to load into the object</param>
        protected override void LoadFromData(string Data)
        {
            var Document = new XmlDocument();
            Document.LoadXml(Data);
            Load(Document);
        }

        private void Load(IXPathNavigable Document)
        {
            Contract.Requires<ArgumentNullException>(Document != null, "Document");
            Channels = Channels.Check(new List<Channel>());
            XPathNavigator Navigator = Document.CreateNavigator();
            var NamespaceManager = new XmlNamespaceManager(Navigator.NameTable);
            XPathNodeIterator Nodes = Navigator.Select("./channel", NamespaceManager);
            foreach (XmlNode Element in Nodes)
            {
                Channels.Add(new Channel((XmlElement)Element));
            }
            if (Channels.Count == 0)
            {
                Nodes = Navigator.Select(".//channel", NamespaceManager);
                foreach (XmlNode Element in Nodes)
                {
                    Channels.Add(new Channel((XmlElement)Element));
                }
                var Items = new List<Item>();
                Nodes = Navigator.Select(".//item", NamespaceManager);
                foreach (XmlNode Element in Nodes)
                {
                    Items.Add(new Item((XmlElement)Element));
                }
                if (Channels.Count > 0)
                {
                    Channels.FirstOrDefault().Add(Items);
                }
            }
        }
    }
}