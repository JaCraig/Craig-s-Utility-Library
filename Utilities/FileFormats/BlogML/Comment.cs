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
using System.Text;
using System.Xml;
using Utilities.DataTypes.ExtensionMethods;
using System.Globalization;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
#endregion

namespace Utilities.FileFormats.BlogML
{
    /// <summary>
    /// Comment class
    /// </summary>
    public class Comment
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Comment()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">Element containing the post info</param>
        public Comment(XElement Element)
        {
            Contract.Requires<ArgumentNullException>(Element != null, "Element");
            DateCreated = Element.Attribute("date-created") != null ? DateTime.Parse(Element.Attribute("date-created").Value, CultureInfo.InvariantCulture) : DateTime.MinValue;
            Approved = Element.Attribute("approved") != null ? bool.Parse(Element.Attribute("approved").Value) : false;
            UserName = Element.Attribute("user-name") != null ? Element.Attribute("user-name").Value : "";
            UserEmail = Element.Attribute("user-email") != null ? Element.Attribute("user-email").Value : "";
            UserIP = Element.Attribute("user-ip") != null ? Element.Attribute("user-ip").Value : "";
            UserURL = Element.Attribute("user-url") != null ? Element.Attribute("user-url").Value : "";
            ID = Element.Attribute("id") != null ? Element.Attribute("id").Value : "";
            if (Element.Element("title") != null)
                Title = Element.Element("title").Value;
            if (Element.Element("content") != null)
                Content = Element.Element("content").Value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Title of the comment
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Actual content of the comment
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// Date created
        /// </summary>
        public virtual DateTime DateCreated { get; set; }

        /// <summary>
        /// Determines if the comment is approved
        /// </summary>
        public virtual bool Approved { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public virtual string UserEmail { get; set; }

        /// <summary>
        /// User IP
        /// </summary>
        public virtual string UserIP { get; set; }

        /// <summary>
        /// User URL
        /// </summary>
        public virtual string UserURL { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; set; }

        #endregion

        #region Overridden Functions

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <returns>The object as a string</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendFormat(CultureInfo.InvariantCulture, "<comment id=\"{0}\" parentid=\"00000000-0000-0000-0000-000000000000\" date-created=\"{1}\" date-modified=\"{1}\" approved=\"true\" user-name=\"{2}\" user-email=\"{3}\" user-ip=\"{4}\" user-url=\"{5}\">\n", ID, DateCreated.ToString("yyyy-MM-ddThh:mm:ss", CultureInfo.InvariantCulture), UserName, UserEmail, UserIP, UserURL);
            Builder.AppendFormat(CultureInfo.InvariantCulture, "<title type=\"text\"><![CDATA[{0}]]></title>\n<content type=\"text\"><![CDATA[{1}]]></content>\n</comment>\n", Title, Content);
            return Builder.ToString();
        }

        #endregion
    }
}