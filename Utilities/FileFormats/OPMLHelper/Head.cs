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
using System.Globalization;
using System.Text;
using System.Xml.Linq;
#endregion

namespace Utilities.FileFormats.OPMLHelper
{
    /// <summary>
    /// Head class
    /// </summary>
    public class Head
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Head()
        {
            Docs = "http://www.opml.org/spec2";
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XmlElement containing the header information</param>
        public Head(XElement Element)
        {
            Contract.Requires<ArgumentNullException>(Element != null, "Element");
            if (Element.Element("title") != null)
                Title = Element.Element("title").Value;
            if (Element.Element("ownerName") != null)
                OwnerName = Element.Element("ownerName").Value;
            if (Element.Element("ownerEmail") != null)
                OwnerEmail = Element.Element("ownerEmail").Value;
            if (Element.Element("dateCreated") != null)
                DateCreated = DateTime.Parse(Element.Element("dateCreated").Value, CultureInfo.InvariantCulture);
            if (Element.Element("dateModified") != null)
                DateModified = DateTime.Parse(Element.Element("dateModified").Value, CultureInfo.InvariantCulture);
            if (Element.Element("docs") != null)
                Docs = Element.Element("docs").Value;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Title of the OPML document
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Date it was created
        /// </summary>
        public virtual DateTime DateCreated { get; set; }

        /// <summary>
        /// Date it was last modified
        /// </summary>
        public virtual DateTime DateModified { get; set; }

        /// <summary>
        /// Owner of the file
        /// </summary>
        public virtual string OwnerName { get; set; }

        /// <summary>
        /// Owner's email address
        /// </summary>
        public virtual string OwnerEmail { get; set; }

        /// <summary>
        /// Location of the OPML spec
        /// </summary>
        public virtual string Docs { get; set; }
        #endregion

        #region Overridden Functions

        /// <summary>
        /// Converts the head to a string
        /// </summary>
        /// <returns>The head as a string</returns>
        public override string ToString()
        {
            StringBuilder HeadString = new StringBuilder();
            HeadString.Append("<head>");
            HeadString.Append("<title>" + Title + "</title>\r\n");
            HeadString.Append("<dateCreated>" + DateCreated.ToString("R", CultureInfo.InvariantCulture) + "</dateCreated>\r\n");
            HeadString.Append("<dateModified>" + DateModified.ToString("R", CultureInfo.InvariantCulture) + "</dateModified>\r\n");
            HeadString.Append("<ownerName>" + OwnerName + "</ownerName>\r\n");
            HeadString.Append("<ownerEmail>" + OwnerEmail + "</ownerEmail>\r\n");
            HeadString.Append("<docs>" + Docs + "</docs>\r\n");
            HeadString.Append("</head>\r\n");
            return HeadString.ToString();
        }

        #endregion
    }
}