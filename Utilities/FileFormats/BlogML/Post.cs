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
using System.Collections.Generic;
using Utilities.DataTypes.ExtensionMethods;
using System.Text;
#endregion

namespace Utilities.FileFormats.BlogML
{
    /// <summary>
    /// Post class
    /// </summary>
    public class Post
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Post()
        {
            Authors = new Authors();
            Categories = new Categories();
            Tags = new Tags();
            Comments = new Comments();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">Element containing the post info</param>
        public Post(XmlElement Element)
        {
            Element.ThrowIfNull("Element");
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
            ID = Element.Attributes["id"] != null ? Element.Attributes["id"].Value : "";
            PostURL = Element.Attributes["post-url"] != null ? Element.Attributes["post-url"].Value : "";
            DateCreated = Element.Attributes["date-created"] != null ? DateTime.Parse(Element.Attributes["date-created"].Value) : DateTime.MinValue;
            DateModified = Element.Attributes["date-modified"] != null ? DateTime.Parse(Element.Attributes["date-modified"].Value) : DateCreated;

            foreach (XmlNode Children in Element.ChildNodes)
            {
                if (Children.Name.Equals("title", StringComparison.CurrentCultureIgnoreCase))
                {
                    Title = Children.InnerText;
                }
                else if (Children.Name.Equals("content", StringComparison.CurrentCultureIgnoreCase))
                {
                    Content = Children.InnerText;
                }
                else if (Children.Name.Equals("post-name", StringComparison.CurrentCultureIgnoreCase))
                {
                    PostName = Children.InnerText;
                }
                else if (Children.Name.Equals("excerpt", StringComparison.CurrentCultureIgnoreCase))
                {
                    Excerpt = Children.InnerText;
                }
                else if (Children.Name.Equals("authors", StringComparison.CurrentCultureIgnoreCase))
                {
                    Authors = new Authors((XmlElement)Children);
                }
                else if (Children.Name.Equals("categories", StringComparison.CurrentCultureIgnoreCase))
                {
                    Categories = new Categories((XmlElement)Children);
                }
                else if (Children.Name.Equals("tags", StringComparison.CurrentCultureIgnoreCase))
                {
                    Tags = new Tags((XmlElement)Children);
                }
                else if (Children.Name.Equals("comments", StringComparison.CurrentCultureIgnoreCase))
                {
                    Comments = new Comments((XmlElement)Children);
                }
            }
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// ID of the post
        /// </summary>
        public virtual string ID { get; set; }

        /// <summary>
        /// URL of the post
        /// </summary>
        public virtual string PostURL { get; set; }

        /// <summary>
        /// Title of the post
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Content of the post
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// Excerpt of the post
        /// </summary>
        public virtual string Excerpt { get; set; }

        /// <summary>
        /// Name of the post
        /// </summary>
        public virtual string PostName { get; set; }

        /// <summary>
        /// Date the post was created
        /// </summary>
        public virtual DateTime DateCreated { get; set; }

        /// <summary>
        /// Date the post was modified
        /// </summary>
        public virtual DateTime DateModified { get; set; }

        /// <summary>
        /// Authors of the post
        /// </summary>
        public virtual Authors Authors { get; set; }

        /// <summary>
        /// Categories associated with the post
        /// </summary>
        public virtual Categories Categories { get; set; }

        /// <summary>
        /// Tags associated with the post
        /// </summary>
        public virtual Tags Tags { get; set; }

        /// <summary>
        /// Comments associated with the post
        /// </summary>
        public virtual Comments Comments { get; set; }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendFormat("<post id=\"{0}\" date-created=\"{1}\" date-modified=\"{2}\" approved=\"true\" post-url=\"{3}\" type=\"normal\" hasexcerpt=\"true\" views=\"0\" is-published=\"True\">\n", ID, DateCreated.ToString("yyyy-MM-ddThh:mm:ss"), DateModified.ToString("yyyy-MM-ddThh:mm:ss"), PostURL);
            Builder.AppendFormat("<title type=\"text\"><![CDATA[{0}]]></title>\n", Title);
            Builder.AppendFormat("<content type=\"text\"><![CDATA[{0}]]></content>\n", Content);
            Builder.AppendFormat("<post-name type=\"text\"><![CDATA[{0}]]></post-name>\n", Title);
            Builder.AppendFormat("<excerpt type=\"text\"><![CDATA[{0}]]></excerpt>\n", Excerpt);
            Builder.AppendLine(Authors.ToString());
            Builder.AppendLine(Categories.ToString());
            Builder.AppendLine(Tags.ToString());
            Builder.AppendLine(Comments.ToString());
            Builder.AppendLine("<trackbacks />");
            Builder.AppendLine("</post>");
            return Builder.ToString();
        }

        #endregion
    }
}