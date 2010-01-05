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
    /// BlogML class
    /// </summary>
    public class BlogML
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Location">Location of the XML file</param>
        public BlogML(string Location)
        {
            try
            {
                XmlDocument Document = new XmlDocument();
                Document.Load(Location);
                foreach (XmlNode Children in Document.ChildNodes)
                {
                    if (Children.Name.Equals("blog", StringComparison.CurrentCultureIgnoreCase))
                    {
                        foreach (XmlNode Child in Children.ChildNodes)
                        {
                            if (Child.Name.Equals("title", StringComparison.CurrentCultureIgnoreCase))
                            {
                                _Title = Child.InnerText;
                            }
                            else if (Child.Name.Equals("sub-title", StringComparison.CurrentCultureIgnoreCase))
                            {
                                _SubTitle = Child.InnerText;
                            }
                            else if (Child.Name.Equals("authors", StringComparison.CurrentCultureIgnoreCase))
                            {
                                _Authors = new Authors((XmlElement)Child);
                            }
                            else if (Child.Name.Equals("categories", StringComparison.CurrentCultureIgnoreCase))
                            {
                                _Categories = new Categories((XmlElement)Child);
                            }
                            else if (Child.Name.Equals("posts", StringComparison.CurrentCultureIgnoreCase))
                            {
                                _Posts = new Posts((XmlElement)Child);
                            }
                        }
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Private Variables
        private string _Title = "";
        private string _SubTitle = "";
        private Authors _Authors=null;
        private Categories _Categories = null;
        private Posts _Posts = null;
        #endregion

        #region Public Properties
        /// <summary>
        /// Title of the blog
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        /// <summary>
        /// Sub title of the blog
        /// </summary>
        public string SubTitle
        {
            get { return _SubTitle; }
            set { _SubTitle = value; }
        }
        
        /// <summary>
        /// Authors of the blog
        /// </summary>
        public Authors Authors
        {
            get { return _Authors; }
            set { _Authors = value; }
        }

        /// <summary>
        /// Categories of the blog (doesn't include tags)
        /// </summary>
        public Categories Categories
        {
            get { return _Categories; }
            set { _Categories = value; }
        }

        /// <summary>
        /// Posts of the blog
        /// </summary>
        public Posts Posts
        {
            get { return _Posts; }
            set { _Posts = value; }
        }
        #endregion
    }
}
