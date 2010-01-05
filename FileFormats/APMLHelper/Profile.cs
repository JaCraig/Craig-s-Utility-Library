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
    /// Profile class
    /// </summary>
    public class Profile
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Profile()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XmlElement containing the profile information</param>
        public Profile(XmlElement Element)
        {
            if (Element.Name.Equals("profile", StringComparison.CurrentCultureIgnoreCase))
            {
                if (Element.Attributes["name"] != null)
                {
                    Name = Element.Attributes["name"].Value;
                }
                foreach (XmlNode Child in Element.ChildNodes)
                {
                    try
                    {
                        if (Child.Name.Equals("implicitdata", StringComparison.CurrentCultureIgnoreCase))
                        {
                            ImplicitData=new Data((XmlElement)Child);
                        }
                        else if (Child.Name.Equals("explicitdata", StringComparison.CurrentCultureIgnoreCase))
                        {
                            ExplicitData=new Data((XmlElement)Child);
                        }
                    }
                    catch { }
                }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Name of the profile
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Name = "";

        /// <summary>
        /// Implicit data (data defined by an outside source)
        /// </summary>
        public Data ImplicitData
        {
            get { return _ImplicitData; }
            set { _ImplicitData = value; }
        }
        private Data _ImplicitData = new Data();

        /// <summary>
        /// Explicit data (data defined by the user)
        /// </summary>
        public Data ExplicitData
        {
            get { return _ExplicitData; }
            set { _ExplicitData = value; }
        }
        private Data _ExplicitData = new Data();
        #endregion

        #region Overridden Functions
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<Profile name=\"" + Name + "\">");
            Builder.Append("<ImplicitData>"+ImplicitData.ToString()+"</ImplicitData>");
            Builder.Append("<ExplicitData>"+ExplicitData.ToString()+"</ExplicitData>");
            Builder.Append("</Profile>");
            return Builder.ToString();
        }
        #endregion
    }
}
