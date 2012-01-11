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
using System.Xml;
using Utilities.DataTypes.ExtensionMethods;
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
        /// <param name="Element">Element containing the post info</param>
        public Comment(XmlElement Element)
        {
            Element.ThrowIfNull("Element");
            DateCreated = Element.Attributes["date-created"] != null ? DateTime.Parse(Element.Attributes["date-created"].Value) : DateTime.MinValue;
            Approved = Element.Attributes["approved"] != null ? bool.Parse(Element.Attributes["approved"].Value) : false;
            UserName = Element.Attributes["user-name"] != null ? Element.Attributes["user-name"].Value : "";
            UserEmail = Element.Attributes["user-email"] != null ? Element.Attributes["user-email"].Value : "";
            UserIP = Element.Attributes["user-ip"] != null ? Element.Attributes["user-ip"].Value : "";
            UserURL = Element.Attributes["user-url"] != null ? Element.Attributes["user-url"].Value : "";

            foreach (XmlNode Children in Element.ChildNodes)
            {
                if (Children.Name.Equals("title", StringComparison.CurrentCultureIgnoreCase))
                    Title = Children.InnerText;
                else if (Children.Name.Equals("content", StringComparison.CurrentCultureIgnoreCase))
                    Content = Children.InnerText;
            }
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

        #endregion
    }
}