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
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XmlElement containing the source information</param>
        public Source(XmlElement Element)
        {
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
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        private string _Key = "";

        /// <summary>
        /// Name of the source
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Name = "";

        /// <summary>
        /// Value associated
        /// </summary>
        public float Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        private float _Value = 1.0f;

        /// <summary>
        /// Type
        /// </summary>
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        private string _Type = "";

        /// <summary>
        /// Where it is from
        /// </summary>
        public string From
        {
            get { return _From; }
            set { _From = value; }
        }
        private string _From = "";

        /// <summary>
        /// Last time it was updated
        /// </summary>
        public DateTime Updated
        {
            get { return _Updated; }
            set { _Updated = value; }
        }
        private DateTime _Updated = DateTime.Now;

        /// <summary>
        /// List of authors
        /// </summary>
        public List<Author> Authors
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
