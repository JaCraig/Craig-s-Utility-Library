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
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.FileFormats.BlogML
{
    /// <summary>
    /// Comments class
    /// </summary>
    public class Comments
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Comments()
        {
            CommentList = new List<Comment>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">Element containing post info</param>
        public Comments(XmlElement Element)
        {
            Element.ThrowIfNull("Element");
            CommentList = new List<Comment>();
            foreach (XmlNode Children in Element.ChildNodes)
            {
                if (Children.Name.Equals("comment", StringComparison.CurrentCultureIgnoreCase))
                    CommentList.Add(new Comment((XmlElement)Children));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// List of comments
        /// </summary>
        public virtual List<Comment> CommentList { get; set; }

        #endregion

        #region Overridden Functions

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <returns>The object as a string</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine("<comments>");
            foreach (Comment Comment in CommentList)
            {
                Builder.AppendLine(Comment.ToString());
            }
            Builder.AppendLine("</comments>");
            return Builder.ToString();
        }

        #endregion
    }
}
