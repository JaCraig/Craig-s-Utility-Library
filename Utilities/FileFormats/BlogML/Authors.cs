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
using System.Diagnostics.Contracts;
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
        public Authors()
        {
            AuthorList = new List<Author>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">Element containing the authors information</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode")]
        public Authors(XmlElement Element)
        {
            Contract.Requires<ArgumentNullException>(Element != null, "Element");
            AuthorList = new List<Author>();
            foreach (XmlNode Children in Element.ChildNodes)
            {
                if (Children.Name.Equals("author", StringComparison.CurrentCultureIgnoreCase))
                    AuthorList.Add(new Author((XmlElement)Children));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// List of authors
        /// </summary>
        public ICollection<Author> AuthorList { get;private set; }

        /// <summary>
        /// Gets a specific author
        /// </summary>
        /// <param name="index">Index within the list</param>
        /// <returns>The author specified</returns>
        public Author this[int index]
        {
            get { return AuthorList.ElementAt(index); }
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
                return AuthorList.FirstOrDefault(x => x.ID.Equals(index));
            }
        }

        #endregion

        #region Overridden Functions

        /// <summary>
        /// Converts the authors to a string
        /// </summary>
        /// <returns>The authors as a string</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLine("<authors>");
            foreach (Author Author in AuthorList)
            {
                Builder.Append(Author.ToString());
            }
            Builder.AppendLine("</authors>");
            return Builder.ToString();
        }

        #endregion
    }
}