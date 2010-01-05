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
    /// Categories
    /// </summary>
    public class Categories
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">Categories element</param>
        public Categories(XmlElement Element)
        {
            foreach (XmlNode Children in Element.ChildNodes)
            {
                if (Children.Name.Equals("category", StringComparison.CurrentCultureIgnoreCase))
                {
                    _Categories.Add(new Category((XmlElement)Children));
                }
            }
        }
        #endregion

        #region Private Variables
        private List<Category> _Categories = new List<Category>();
        #endregion

        #region Public Properties
        /// <summary>
        /// Categories list
        /// </summary>
        public List<Category> CategoryList
        {
            get { return _Categories; }
            set { _Categories = value; }
        }

        /// <summary>
        /// Gets a specified category
        /// </summary>
        /// <param name="index">The ID of the category</param>
        /// <returns>The category specified</returns>
        public Category this[string index]
        {
            get
            {
                foreach (Category Category in CategoryList)
                {
                    if (Category.ID.Equals(index))
                    {
                        return Category;
                    }
                }
                return null;
            }
        }
        #endregion
    }
}
