/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Diagnostics.Contracts;
using System.Text;
using System.Xml.Linq;
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
            Outlines = new List<Outline>();
        }

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="Element">Element containing outline information</param>
        public Outline(XElement Element)
        {
            Contract.Requires<ArgumentNullException>(Element != null, "Element");
            if (Element.Attribute("text") != null)
            {
                Text = Element.Attribute("text").Value;
            }
            if (Element.Attribute("description") != null)
            {
                Description = Element.Attribute("description").Value;
            }
            if (Element.Attribute("htmlUrl") != null)
            {
                HTMLUrl = Element.Attribute("htmlUrl").Value;
            }
            if (Element.Attribute("type") != null)
            {
                Type = Element.Attribute("type").Value;
            }
            if (Element.Attribute("language") != null)
            {
                Language = Element.Attribute("language").Value;
            }
            if (Element.Attribute("title") != null)
            {
                Title = Element.Attribute("title").Value;
            }
            if (Element.Attribute("version") != null)
            {
                Version = Element.Attribute("version").Value;
            }
            if (Element.Attribute("xmlUrl") != null)
            {
                XMLUrl = Element.Attribute("xmlUrl").Value;
            }

            foreach (XElement Child in Element.Elements("outline"))
            {
                Outlines.Add(new Outline(Child));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Outline list
        /// </summary>
        public ICollection<Outline> Outlines { get; private set; }

        /// <summary>
        /// Url of the XML file
        /// </summary>
        public string XMLUrl { get; set; }

        /// <summary>
        /// Version number
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Title of the item
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Language used
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// HTML Url
        /// </summary>
        public string HTMLUrl { get; set; }

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        #endregion

        #region Overridden Functions

        /// <summary>
        /// Converts the outline to a string
        /// </summary>
        /// <returns>The outline as a string</returns>
        public override string ToString()
        {
            StringBuilder OutlineString = new StringBuilder();
            OutlineString.Append("<outline text=\"" + Text + "\"");
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