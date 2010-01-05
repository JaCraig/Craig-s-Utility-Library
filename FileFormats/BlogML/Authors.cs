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
using System.Collections.Generic;
using System.Xml;
#endregion

namespace Utilities.FileFormats.BlogML
{
    /// <summary>
    /// Authors list
    /// </summary>
    public class Authors
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">Element containing the authors information</param>
        public Authors(XmlElement Element)
        {
            foreach (XmlNode Children in Element.ChildNodes)
            {
                if (Children.Name.Equals("author", StringComparison.CurrentCultureIgnoreCase))
                {
                    _Authors.Add(new Author((XmlElement)Children));
                }
            }
        }
        #endregion

        #region Private Variables
        private List<Author> _Authors = new List<Author>();
        #endregion

        #region Public Properties
        /// <summary>
        /// List of authors
        /// </summary>
        public List<Author> AuthorList
        {
            get { return _Authors; }
            set { _Authors = value; }
        }

        /// <summary>
        /// Gets a specific author
        /// </summary>
        /// <param name="index">Index within the list</param>
        /// <returns>The author specified</returns>
        public Author this[int index]
        {
            get { return AuthorList[index]; }
        }

        /// <summary>
        /// Gets a specific author
        /// </summary>
        /// <param name="index">Author's ID that you want</param>
        /// <returns>The author specified</returns>
        public Author this[string index]
        {
            get
            {
                foreach (Author Author in AuthorList)
                {
                    if (Author.ID.Equals(index))
                    {
                        return Author;
                    }
                }
                return null;
            }
        }
        #endregion
    }
}
