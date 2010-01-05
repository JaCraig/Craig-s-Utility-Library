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
using System.Text;
using System.Xml;
#endregion

namespace Utilities.FileFormats.APMLHelper
{
    /// <summary>
    /// Concepts class
    /// </summary>
    public class Concepts
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Concepts()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XmlElement containing the Concepts information</param>
        public Concepts(XmlElement Element)
        {
            if (Element.Name.Equals("concepts", StringComparison.CurrentCultureIgnoreCase))
            {
                foreach (XmlNode Child in Element.ChildNodes)
                {
                    try
                    {
                        if (Child.Name.Equals("concept", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Concept.Add(new Concept((XmlElement)Child));
                        }
                    }
                    catch { }
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of concepts
        /// </summary>
        public List<Concept> Concept
        {
            get { return _Concept; }
            set { _Concept = value; }
        }
        private List<Concept> _Concept = new List<Concept>();
        #endregion

        #region Overridden Functions
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<Concepts>");
            foreach (Concept TempConcept in Concept)
            {
                Builder.Append(TempConcept.ToString());
            }
            Builder.Append("</Concepts>");
            return Builder.ToString();
        }
        #endregion
    }
}
