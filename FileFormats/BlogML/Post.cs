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
using System.Xml;
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
        /// <param name="Element">Element containing the post info</param>
        public Post(XmlElement Element)
        {
            if (Element.Attributes["id"] != null)
            {
                _ID = Element.Attributes["id"].Value;
            }
            if (Element.Attributes["post-url"] != null)
            {
                _PostURL = Element.Attributes["post-url"].Value;
            }
            if (Element.Attributes["date-created"] != null)
            {
                _DateCreated = DateTime.Parse(Element.Attributes["date-created"].Value);
            }
            if (Element.Attributes["date-modified"] != null)
            {
                _DateModified = DateTime.Parse(Element.Attributes["date-modified"].Value);
            }
            foreach (XmlNode Children in Element.ChildNodes)
            {
                if (Children.Name.Equals("title", StringComparison.CurrentCultureIgnoreCase))
                {
                    _Title = Children.InnerText;
                }
                else if (Children.Name.Equals("content", StringComparison.CurrentCultureIgnoreCase))
                {
                    _Content = Children.InnerText;
                }
                else if (Children.Name.Equals("post-name", StringComparison.CurrentCultureIgnoreCase))
                {
                    _PostName = Children.InnerText;
                }
                else if (Children.Name.Equals("excerpt", StringComparison.CurrentCultureIgnoreCase))
                {
                    _Excerpt = Children.InnerText;
                }
                else if (Children.Name.Equals("authors", StringComparison.CurrentCultureIgnoreCase))
                {
                    _Authors = new Authors((XmlElement)Children);
                }
                else if (Children.Name.Equals("categories", StringComparison.CurrentCultureIgnoreCase))
                {
                    _Categories = new Categories((XmlElement)Children);
                }
                else if (Children.Name.Equals("tags", StringComparison.CurrentCultureIgnoreCase))
                {
                    _Tags = new Tags((XmlElement)Children);
                }
            }
        }
        #endregion

        #region Private Variables
        private string _ID="";
        private string _PostURL = "";
        private string _Title = "";
        private string _Content = "";
        private string _Excerpt = "";
        private string _PostName = "";
        private DateTime _DateCreated = DateTime.Now;
        private DateTime _DateModified = DateTime.Now;
        private Authors _Authors = null;
        private Categories _Categories = null;
        private Tags _Tags = null;
        #endregion

        #region Public Properties
        /// <summary>
        /// ID of the post
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        /// <summary>
        /// URL of the post
        /// </summary>
        public string PostURL
        {
            get { return _PostURL; }
            set { _PostURL = value; }
        }

        /// <summary>
        /// Title of the post
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        /// <summary>
        /// Content of the post
        /// </summary>
        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }

        /// <summary>
        /// Excerpt of the post
        /// </summary>
        public string Excerpt
        {
            get { return _Excerpt; }
            set { _Excerpt = value; }
        }

        /// <summary>
        /// Name of the post
        /// </summary>
        public string PostName
        {
            get { return _PostName; }
            set { _PostName = value; }
        }

        /// <summary>
        /// Date the post was created
        /// </summary>
        public DateTime DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }

        /// <summary>
        /// Date the post was modified
        /// </summary>
        public DateTime DateModified
        {
            get { return _DateModified; }
            set { _DateModified = value; }
        }

        /// <summary>
        /// Authors of the post
        /// </summary>
        public Authors Authors
        {
            get { return _Authors; }
            set { _Authors = value; }
        }

        /// <summary>
        /// Categories associated with the post
        /// </summary>
        public Categories Categories
        {
            get { return _Categories; }
            set { _Categories = value; }
        }

        /// <summary>
        /// Tags associated with the post
        /// </summary>
        public Tags Tags
        {
            get { return _Tags; }
            set { _Tags = value; }
        }
        #endregion
    }
}
