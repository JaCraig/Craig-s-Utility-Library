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
using System.Xml;
#endregion

namespace Utilities.Web.Netflix
{
    /// <summary>
    /// Person class
    /// </summary>
    public class Person
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">person node</param>
        public Person(XmlElement Element)
        {
            foreach (XmlNode Children in Element.ChildNodes)
            {
                if (Children.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase))
                {
                    ID = Children.InnerText;
                }
                else if (Children.Name.Equals("name", StringComparison.CurrentCultureIgnoreCase))
                {
                    Name = Children.InnerText;
                }
                else if (Children.Name.Equals("bio", StringComparison.CurrentCultureIgnoreCase))
                {
                    Bio = Children.InnerText;
                }
                else if (Children.Name.Equals("link", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (Children.Attributes["rel"] != null
                        && Children.Attributes["rel"].Value.Equals("http://schemas.netflix.com/catlog/person/filmography",
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        Filmography = Children.Attributes["href"].Value;
                    }
                    else if (Children.Attributes["rel"] != null
                        && Children.Attributes["rel"].Value.Equals("alternate",
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        Webpage = Children.Attributes["href"].Value;
                    }
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// ID for the person
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Name of the person
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Bio information for the person
        /// </summary>
        public string Bio { get; set; }

        /// <summary>
        /// Filmography link
        /// </summary>
        public string Filmography { get; set; }

        /// <summary>
        /// Webpage link
        /// </summary>
        public string Webpage { get; set; }

        #endregion
    }
}