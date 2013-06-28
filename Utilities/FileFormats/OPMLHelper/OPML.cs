/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Diagnostics.Contracts;
using System.Text;
using System.Xml.Linq;
using Utilities.FileFormats.BaseClasses;
#endregion

namespace Utilities.FileFormats.OPMLHelper
{
    /// <summary>
    /// OPML class
    /// </summary>
    public class OPML:StringFormatBase<OPML>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public OPML()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Location">Location of the OPML file</param>
        public OPML(string Location)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(Location),"Location");
            InternalLoad(Location);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Document">XmlDocument containing the OPML file</param>
        public OPML(XDocument Document)
        {
            Contract.Requires<ArgumentNullException>(Document!=null,"Document");
            LoadDoc(Document);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Body of the file
        /// </summary>
        public virtual Body Body { get; set; }

        /// <summary>
        /// Header information
        /// </summary>
        public virtual Head Head { get; set; }

        #endregion

        #region Overridden Functions

        /// <summary>
        /// Loads the object from the data specified
        /// </summary>
        /// <param name="Data">Data to load into the object</param>
        protected override void LoadFromData(string Data)
        {
            XDocument Document = XDocument.Parse(Data);
            LoadDoc(Document);
        }

        private void LoadDoc(XDocument Document)
        {
            foreach (XElement Children in Document.Elements("opml"))
            {
                if (Children.Element("head") != null)
                    Head = new Head(Children.Element("head"));
                if (Children.Element("body") != null)
                    Body = new Body(Children.Element("body"));
            }
        }

        /// <summary>
        /// Converts the file to a string
        /// </summary>
        /// <returns>The file as a string</returns>
        public override string ToString()
        {
            StringBuilder OPMLString = new StringBuilder();
            OPMLString.Append("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?><opml version=\"2.0\">");
            OPMLString.Append(Head.ToString());
            OPMLString.Append(Body.ToString());
            OPMLString.Append("</opml>");
            return OPMLString.ToString();
        }
        #endregion
    }
}