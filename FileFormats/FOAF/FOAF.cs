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

namespace Utilities.FileFormats.FOAF
{
    /// <summary>
    /// FOAF Parser/Generator
    /// </summary>
    public class FOAF
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public FOAF()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Location">Location of the FOAF file</param>
        public FOAF(string Location)
        {
            XmlDocument Document = new XmlDocument();
            Document.Load(Location);
            foreach (XmlNode Children in Document.ChildNodes)
            {
                if (Children.Name.Equals("rdf:RDF", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (XmlNode Child in Children.ChildNodes)
                    {
                        if (Child.Name.Equals("foaf:Person", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Person = new Person((XmlElement)Child);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Document">XmlDocument containing the FOAF file</param>
        public FOAF(XmlDocument Document)
        {
            foreach (XmlNode Children in Document.ChildNodes)
            {
                if (Children.Name.Equals("rdf:RDF", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (XmlNode Child in Children.ChildNodes)
                    {
                        if (Child.Name.Equals("foaf:Person", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Person = new Person((XmlElement)Child);
                        }
                    }
                }
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Person object
        /// </summary>
        public Person Person
        {
            get { return _Person; }
            set { _Person = value; }
        }
        private Person _Person = null;
        #endregion

        #region Public Overridden Functions

        /// <summary>
        /// Outputs the FOAF object to a string
        /// </summary>
        /// <returns>Outputs an xml/rdf formatted output of the object</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" xmlns:rdfs=\"http://www.w3.org/2000/01/rdf-schema#\" xmlns:foaf=\"http://xmlns.com/foaf/0.1/\" xmlns:admin=\"http://webns.net/mvcb/\"><foaf:PersonalProfileDocument rdf:about=\"\"><foaf:maker rdf:resource=\"#me\"/><foaf:primaryTopic rdf:resource=\"#me\"/><admin:generatorAgent rdf:resource=\"http://www.gutgames.com\"/><admin:errorReportsTo rdf:resource=\"\"/></foaf:PersonalProfileDocument><foaf:Person rdf:ID=\"me\">");
            Builder.Append(Person.ToString());
            Builder.Append("</foaf:Person></rdf:RDF>");
            return Builder.ToString();
        }
        #endregion
    }
}