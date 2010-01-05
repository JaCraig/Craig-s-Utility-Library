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

namespace Utilities.Web.Netflix
{
    /// <summary>
    /// List of titles
    /// </summary>
    public class Titles
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="XMLContent">XML Content</param>
        public Titles(string XMLContent)
        {
            XmlDocument Document = new XmlDocument();
            Document.LoadXml(XMLContent);
            TitleList = new List<Title>();

            foreach (XmlNode Children in Document.ChildNodes)
            {
                if (Children.Name.Equals("catalog_titles", StringComparison.CurrentCultureIgnoreCase)
                    || Children.Name.Equals("similars", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (XmlNode Child in Children.ChildNodes)
                    {
                        if (Child.Name.Equals("catalog_title", StringComparison.CurrentCultureIgnoreCase)
                            || Child.Name.Equals("similars_item", StringComparison.CurrentCultureIgnoreCase))
                        {
                            TitleList.Add(new Title((XmlElement)Child));
                        }
                        else if (Child.Name.Equals("number_of_results", StringComparison.CurrentCultureIgnoreCase))
                        {
                            NumberOfResults = int.Parse(Child.InnerText);
                        }
                        else if (Child.Name.Equals("start_index", StringComparison.CurrentCultureIgnoreCase))
                        {
                            StartIndex = int.Parse(Child.InnerText);
                        }
                        else if (Child.Name.Equals("results_per_page", StringComparison.CurrentCultureIgnoreCase))
                        {
                            ResultsPerPage = int.Parse(Child.InnerText);
                        }
                    }
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Number of results
        /// </summary>
        public int NumberOfResults { get; set; }

        /// <summary>
        /// Start index
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// Results per page
        /// </summary>
        public int ResultsPerPage { get; set; }

        /// <summary>
        /// List of titles
        /// </summary>
        public List<Title> TitleList { get; set; }

        #endregion
    }
}