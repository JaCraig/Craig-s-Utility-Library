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
using System.Linq;
using System.Text;
using System.Xml;
using Utilities.DataTypes.ExtensionMethods;
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
        public Categories()
        {
            CategoryList = new List<Category>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">Categories element</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode")]
        public Categories(XmlElement Element)
        {
            Element.ThrowIfNull("Element");
            CategoryList = new List<Category>();
            foreach (XmlNode Children in Element.ChildNodes)
            {
                if (Children.Name.Equals("category", StringComparison.CurrentCultureIgnoreCase))
                    CategoryList.Add(new Category((XmlElement)Children));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Categories list
        /// </summary>
        public ICollection<Category> CategoryList { get;private set; }

        /// <summary>
        /// Gets a specified category
        /// </summary>
        /// <param name="index">The ID of the category</param>
        /// <returns>The category specified</returns>
        public Category this[string index]
        {
            get
            {
                return CategoryList.FirstOrDefault(x => x.ID.Equals(index));
            }
        }

        #endregion

        #region Overridden Functions

        /// <summary>
        /// Converts the categories to a formatted string
        /// </summary>
        /// <returns>The formatted string</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine("<categories>");
            foreach(Category Category in CategoryList)
            {
                Builder.Append(Category.ToString());
            }
            Builder.AppendLine("</categories>");
            return Builder.ToString();
        }

        #endregion
    }
}