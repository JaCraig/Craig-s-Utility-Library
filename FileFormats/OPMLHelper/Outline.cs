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

namespace Utilities.FileFormats.OPMLHelper
{
    /// <summary>
    /// Outline class
    /// </summary>
    public class Outline
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Outline()
        {
        }

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="Element">Element containing outline information</param>
        public Outline(XmlElement Element)
        {
            if (Element.Name.Equals("outline", StringComparison.CurrentCultureIgnoreCase))
            {
                if (Element.Attributes["text"] != null)
                {
                    Text = Element.Attributes["text"].Value;
                }
                else if (Element.Attributes["description"] != null)
                {
                    Description = Element.Attributes["description"].Value;
                }
                else if (Element.Attributes["htmlUrl"] != null)
                {
                    HTMLUrl = Element.Attributes["htmlUrl"].Value;
                }
                else if (Element.Attributes["type"] != null)
                {
                    Type = Element.Attributes["type"].Value;
                }
                else if (Element.Attributes["language"] != null)
                {
                    Language = Element.Attributes["language"].Value;
                }
                else if (Element.Attributes["title"] != null)
                {
                    Title = Element.Attributes["title"].Value;
                }
                else if (Element.Attributes["version"] != null)
                {
                    Version = Element.Attributes["version"].Value;
                }
                else if (Element.Attributes["xmlUrl"] != null)
                {
                    XMLUrl = Element.Attributes["xmlUrl"].Value;
                }
                foreach (XmlNode Child in Element.ChildNodes)
                {
                    try
                    {
                        if (Child.Name.Equals("outline", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Outlines.Add(new Outline((XmlElement)Child));
                        }
                    }
                    catch { }
                }
            }
        }
        #endregion

        #region Properties
        private string _XMLUrl;
        private string _Version;
        private string _Title;
        private string _Language;
        private string _Text;
        private string _Description;
        private string _HTMLUrl;
        private string _Type;
        private List<Outline> _Outlines = null;

        /// <summary>
        /// Outline list
        /// </summary>
        public List<Outline> Outlines
        {
            get
            {
                if (_Outlines == null)
                {
                    _Outlines = new List<Outline>();
                }
                return _Outlines;
            }
            set { _Outlines = value; }
        }
        
        /// <summary>
        /// Url of the XML file
        /// </summary>
        public string XMLUrl
        {
            get { return _XMLUrl; }
            set { _XMLUrl = value; }
        }

        /// <summary>
        /// Version number
        /// </summary>
        public string Version
        {
            get { return _Version; }
            set { _Version = value; }
        }

        /// <summary>
        /// Title of the item
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        /// <summary>
        /// Language used
        /// </summary>
        public string Language
        {
            get { return _Language; }
            set { _Language = value; }
        }

        /// <summary>
        /// Type
        /// </summary>
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        /// <summary>
        /// HTML Url
        /// </summary>
        public string HTMLUrl
        {
            get { return _HTMLUrl; }
            set { _HTMLUrl = value; }
        }

        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        /// <summary>
        /// Description
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        #endregion

        #region Overridden Functions
        public override string ToString()
        {
            StringBuilder OutlineString = new StringBuilder();
            OutlineString.Append("<outline text=\""+Text+"\"");
            if (!string.IsNullOrEmpty(XMLUrl))
            {
                OutlineString.Append(" xmlUrl=\"" + XMLUrl + "\"");
            }
            if (!string.IsNullOrEmpty(Version))
            {
                OutlineString.Append(" version=\"" + Version + "\"");
            }
            if (!string.IsNullOrEmpty(Title))
            {
                OutlineString.Append(" title=\"" + Title + "\"");
            }
            if (!string.IsNullOrEmpty(Language))
            {
                OutlineString.Append(" language=\"" + Language + "\"");
            }
            if (!string.IsNullOrEmpty(Type))
            {
                OutlineString.Append(" type=\"" + Type + "\"");
            }
            if (!string.IsNullOrEmpty(HTMLUrl))
            {
                OutlineString.Append(" htmlUrl=\"" + HTMLUrl + "\"");
            }
            if (!string.IsNullOrEmpty(Text))
            {
                OutlineString.Append(" text=\"" + Text + "\"");
            }
            if (!string.IsNullOrEmpty(Description))
            {
                OutlineString.Append(" description=\"" + Description + "\"");
            }
            if (Outlines.Count > 0)
            {
                OutlineString.Append(">\r\n");
                foreach (Outline Outline in Outlines)
                {
                    OutlineString.Append(Outline.ToString());
                }
                OutlineString.Append("</outline>\r\n");
            }
            else
            {
                OutlineString.Append(" />\r\n");
            }
            return OutlineString.ToString();
        }
        #endregion
    }
}
