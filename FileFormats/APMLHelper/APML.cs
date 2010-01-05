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
    /// Utility Class used for APML
    /// </summary>
    public class APML
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public APML()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Location">Location of the APML file</param>
        public APML(string Location)
        {
            XmlDocument Document = new XmlDocument();
            Document.Load(Location);
            foreach (XmlNode Children in Document.ChildNodes)
            {
                if (Children.Name.Equals("apml", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (XmlNode Child in Children.ChildNodes)
                    {
                        if (Child.Name.Equals("body", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Body = new Body((XmlElement)Child);
                        }
                        else if (Child.Name.Equals("head", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Head = new Head((XmlElement)Child);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Document">XmlDocument containing the APML file</param>
        public APML(XmlDocument Document)
        {
            foreach (XmlNode Children in Document.ChildNodes)
            {
                if (Children.Name.Equals("apml", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (XmlNode Child in Children.ChildNodes)
                    {
                        if (Child.Name.Equals("body", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Body=new Body((XmlElement)Child);
                        }
                        else if (Child.Name.Equals("head", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Head = new Head((XmlElement)Child);
                        }
                    }
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Body
        /// </summary>
        public Body Body
        {
            get { return _Body; }
            set { _Body = value; }
        }

        /// <summary>
        /// Head
        /// </summary>
        public Head Head
        {
            get { return _Head; }
            set { _Head = value; }
        }

        private Body _Body;
        private Head _Head;
        #endregion

        #region Overridden Functions
        public override string ToString()
        {
            StringBuilder OPMLString = new StringBuilder();
            OPMLString.Append("<?xml version=\"1.0\"?><APML version=\"0.6\" xmlns=\"http://www.apml.org/apml-0.6\">");
            OPMLString.Append(Head.ToString());
            OPMLString.Append(Body.ToString());
            OPMLString.Append("</APML>");
            return OPMLString.ToString();
        }
        #endregion
    }
}
