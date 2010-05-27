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
using System.Text;
using System.Xml;
#endregion

namespace Utilities.FileFormats.OPMLHelper
{
    /// <summary>
    /// Head class
    /// </summary>
    public class Head
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Head()
        {
            Docs="http://www.opml.org/spec2";
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XmlElement containing the header information</param>
        public Head(XmlElement Element)
        {
            try
            {
                if (Element.Name.Equals("head", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (XmlNode Child in Element.ChildNodes)
                    {
                        try
                        {
                            if (Child.Name.Equals("title", StringComparison.CurrentCultureIgnoreCase))
                            {
                                Title = Child.InnerText;
                            }
                            else if (Child.Name.Equals("ownerName", StringComparison.CurrentCultureIgnoreCase))
                            {
                                OwnerName = Child.InnerText;
                            }
                            else if (Child.Name.Equals("ownerEmail", StringComparison.CurrentCultureIgnoreCase))
                            {
                                OwnerEmail = Child.InnerText;
                            }
                            else if (Child.Name.Equals("dateCreated", StringComparison.CurrentCultureIgnoreCase))
                            {
                                DateCreated = DateTime.Parse(Child.InnerText);
                            }
                            else if (Child.Name.Equals("dateModified", StringComparison.CurrentCultureIgnoreCase))
                            {
                                DateModified = DateTime.Parse(Child.InnerText);
                            }
                            else if (Child.Name.Equals("docs", StringComparison.CurrentCultureIgnoreCase))
                            {
                                Docs = Child.InnerText;
                            }
                        }
                        catch { }
                    }
                }
            }
            catch { throw; }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Title of the OPML document
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Date it was created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Date it was last modified
        /// </summary>
        public DateTime DateModified { get; set; }

        /// <summary>
        /// Owner of the file
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Owner's email address
        /// </summary>
        public string OwnerEmail { get; set; }

        /// <summary>
        /// Location of the OPML spec
        /// </summary>
        public string Docs { get; set; }
        #endregion

        #region Overridden Functions
        public override string ToString()
        {
            try
            {
                StringBuilder HeadString = new StringBuilder();
                HeadString.Append("<head>");
                HeadString.Append("<title>" + Title + "</title>\r\n");
                HeadString.Append("<dateCreated>" + DateCreated.ToString("R") + "</dateCreated>\r\n");
                HeadString.Append("<dateModified>" + DateModified.ToString("R") + "</dateModified>\r\n");
                HeadString.Append("<ownerName>" + OwnerName + "</ownerName>\r\n");
                HeadString.Append("<ownerEmail>" + OwnerEmail + "</ownerEmail>\r\n");
                HeadString.Append("<docs>" + Docs + "</docs>\r\n");
                HeadString.Append("</head>\r\n");
                return HeadString.ToString();
            }
            catch { throw; }
        }
        #endregion
    }
}
