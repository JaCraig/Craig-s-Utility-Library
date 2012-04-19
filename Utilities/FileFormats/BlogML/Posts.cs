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
using System.Xml;
using Utilities.DataTypes.ExtensionMethods;
using System.Text;
#endregion

namespace Utilities.FileFormats.BlogML
{
    /// <summary>
    /// Posts class
    /// </summary>
    public class Posts
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Posts()
        {
            PostList = new List<Post>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">Element containing post info</param>
        public Posts(XmlElement Element)
        {
            Element.ThrowIfNull("Element");
            PostList = new List<Post>();
            foreach (XmlNode Children in Element.ChildNodes)
            {
                if (Children.Name.Equals("post", StringComparison.CurrentCultureIgnoreCase))
                    PostList.Add(new Post((XmlElement)Children));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// List of posts
        /// </summary>
        public virtual List<Post> PostList { get; set; }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine("<posts>");
            foreach (Post Post in PostList)
            {
                Builder.AppendLine(Post.ToString());
            }
            Builder.AppendLine("</posts>");
            return Builder.ToString();
        }

        #endregion
    }
}