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
    /// Concepts class
    /// </summary>
    public class Concept
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Concept()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XmlElement containing the concept information</param>
        public Concept(XmlElement Element)
        {
            if (Element.Name.Equals("concept", StringComparison.CurrentCultureIgnoreCase))
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
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Key information
        /// </summary>
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        private string _Key = "";

        /// <summary>
        /// Value/Weight of the item
        /// </summary>
        public float Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        private float _Value = 1.0f;

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
        /// Last time updated
        /// </summary>
        public DateTime Updated
        {
            get { return _Updated; }
            set { _Updated = value; }
        }
        private DateTime _Updated = DateTime.Now;
        #endregion

        #region Overriden Functions
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            if (!string.IsNullOrEmpty(From))
            {
                Builder.Append("<Concept key=\"" + Key + "\" value=\"" + Value + "\" from=\"" + From + "\" updated=\"" + Updated.ToString("yyyy-MM-ddTHH:mm:ssZ") + "\" />");
            }
            else
            {
                Builder.Append("<Concept key=\"" + Key + "\" value=\"" + Value + "\" />");
            }
            return Builder.ToString();
        }
        #endregion
    }
}
