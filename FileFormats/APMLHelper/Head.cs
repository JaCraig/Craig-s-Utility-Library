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

namespace Utilities.FileFormats.APMLHelper
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
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XmlElement containing the head information</param>
        public Head(XmlElement Element)
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
                        else if (Child.Name.Equals("userEmail", StringComparison.CurrentCultureIgnoreCase))
                        {
                            UserEmail = Child.InnerText;
                        }
                        else if (Child.Name.Equals("generator", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Generator = Child.InnerText;
                        }
                        else if (Child.Name.Equals("dateCreated", StringComparison.CurrentCultureIgnoreCase))
                        {
                            DateCreated = DateTime.Parse(Child.InnerText);
                        }
                    }
                    catch { }
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Title of the APML document
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        private string _Title="";

        /// <summary>
        /// Generator of the APML document
        /// </summary>
        public string Generator
        {
            get { return _Generator; }
            set { _Generator = value; }
        }
        private string _Generator="";

        /// <summary>
        /// User's email address
        /// </summary>
        public string UserEmail
        {
            get { return _UserEmail; }
            set { _UserEmail = value; }
        }
        private string _UserEmail="";

        /// <summary>
        /// Date the document was created
        /// </summary>
        public DateTime DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }
        private DateTime _DateCreated=DateTime.Now;
        #endregion

        #region Overridden Functions
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<Head>");
            Builder.Append("<Title>" + Title + "</Title>");
            Builder.Append("<Generator>" + Generator + "</Generator>");
            Builder.Append("<UserEmail>" + UserEmail + "</UserEmail>");
            Builder.Append("<DateCreated>" + DateCreated.ToString("yyyy-MM-ddTHH:mm:ssZ") + "</DateCreated>");
            Builder.Append("</Head>");
            return Builder.ToString();
        }
        #endregion
    }
}
