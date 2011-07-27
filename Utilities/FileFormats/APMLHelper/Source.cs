/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
    /// Source class
    /// </summary>
    public class Source
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Source()
        {
            Value = 1.0f;
            Updated = DateTime.Now;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XmlElement containing the source information</param>
        public Source(XmlElement Element)
        {
            if (Element == null)
                throw new ArgumentNullException("Element");
            Value = 1.0f;
            Updated = DateTime.Now;
            if (Element.Name.Equals("source", StringComparison.CurrentCultureIgnoreCase))
            {
                if (Element.Attributes["key"] != null)
                {
                    Key = Element.Attributes["key"].Value;
                }
                if (Element.Attributes["value"] != null)
                {
                    Value = float.Parse(Element.Attributes["value"].Value);
                }
                if (Element.Attributes["from"] != null)
                {
                    From = Element.Attributes["from"].Value;
                }
                if (Element.Attributes["updated"] != null)
                {
                    Updated = DateTime.Parse(Element.Attributes["updated"].Value);
                }
                if (Element.Attributes["type"] != null)
                {
                    Type = Element.Attributes["type"].Value;
                }
                if (Element.Attributes["name"] != null)
                {
                    Name = Element.Attributes["name"].Value;
                }
                foreach (XmlNode Child in Element.ChildNodes)
                {
                    try
                    {
                        if (Child.Name.Equals("author", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Authors.Add(new Author((XmlElement)Child));
                        }
                    }
                    catch { }
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Key for this source
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// Name of the source
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Value associated
        /// </summary>
        public virtual float Value { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// Where it is from
        /// </summary>
        public virtual string From { get; set; }

        /// <summary>
        /// Last time it was updated
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// List of authors
        /// </summary>
        public virtual List<Author> Authors
        {
            get { return _Authors; }
            set { _Authors = value; }
        }
        private List<Author> _Authors = new List<Author>();
        #endregion

        #region Overridden Functions
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            if (!string.IsNullOrEmpty(From))
            {
                Builder.Append("<Source key=\"" + Key + "\" name=\"" + Name + "\" value=\"" + Value + "\" type=\"" + Type + "\" from=\"" + From + "\" updated=\"" + Updated.ToString("yyyy-MM-ddTHH:mm:ssZ") + "\">");
            }
            else
            {
                Builder.Append("<Source key=\"" + Key + "\" name=\"" + Name + "\" value=\"" + Value + "\" type=\"" + Type + "\">");
            }
            foreach (Author Author in Authors)
            {
                Builder.Append(Author.ToString());
            }
            Builder.Append("</Source>");
            return Builder.ToString();
        }
        #endregion
    }
}