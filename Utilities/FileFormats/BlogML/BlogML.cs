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
using System.Text;
#endregion

namespace Utilities.FileFormats.BlogML
{
    /// <summary>
    /// BlogML class
    /// </summary>
    public class BlogML
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public BlogML()
        {
            Authors = new Authors();
            Categories = new Categories();
            Posts = new Posts();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Location">Location of the XML file</param>
        public BlogML(string Location)
        {
            Location.ThrowIfNullOrEmpty("Location");
            XmlDocument Document = new XmlDocument();
            Document.Load(Location);
            foreach (XmlNode Children in Document.ChildNodes)
            {
                if (Children.Name.Equals("blog", StringComparison.CurrentCultureIgnoreCase))
                {
                    DateCreated = Children.Attributes["date-created"] != null ? DateTime.Parse(Children.Attributes["date-created"].Value) : DateTime.Now;
                    RootURL = Children.Attributes["root-url"] != null ? Children.Attributes["root-url"].Value : "";
                    foreach (XmlNode Child in Children.ChildNodes)
                    {
                        if (Child.Name.Equals("title", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Title = Child.InnerText;
                        }
                        else if (Child.Name.Equals("sub-title", StringComparison.CurrentCultureIgnoreCase))
                        {
                            SubTitle = Child.InnerText;
                        }
                        else if (Child.Name.Equals("authors", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Authors = new Authors((XmlElement)Child);
                        }
                        else if (Child.Name.Equals("categories", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Categories = new Categories((XmlElement)Child);
                        }
                        else if (Child.Name.Equals("posts", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Posts = new Posts((XmlElement)Child);
                        }
                    }
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Title of the blog
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Sub title of the blog
        /// </summary>
        public virtual string SubTitle { get; set; }

        /// <summary>
        /// Authors of the blog
        /// </summary>
        public virtual Authors Authors { get; set; }

        /// <summary>
        /// Categories of the blog (doesn't include tags)
        /// </summary>
        public virtual Categories Categories { get; set; }

        /// <summary>
        /// Posts of the blog
        /// </summary>
        public virtual Posts Posts { get; set; }

        /// <summary>
        /// Date created
        /// </summary>
        public virtual DateTime DateCreated { get; set; }

        /// <summary>
        /// Root URL
        /// </summary>
        public virtual string RootURL { get; set; }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>")
                .AppendFormat("<blog root-url=\"{0}\" date-created=\"{1}\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.blogml.com/2006/09/BlogML\">\n", RootURL, DateCreated.ToString("yyyy-MM-ddThh:mm:ss"))
                .AppendFormat("<title type=\"text\"><![CDATA[{0}]]></title>\n", Title)
                .AppendFormat("<sub-title type=\"text\"><![CDATA[{0}]]></sub-title>\n", SubTitle)
                .Append(Authors.ToString())
                .Append(Categories.ToString())
                .Append(Posts.ToString())
                .AppendLine("</blog>");
            return Builder.ToString();
        }

        #endregion
    }
}