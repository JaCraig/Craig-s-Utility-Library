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
    /// Application class
    /// </summary>
    public class Application
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Application()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">XmlElement containing the application information</param>
        public Application(XmlElement Element)
        {
            if (Element.Name.Equals("application", StringComparison.CurrentCultureIgnoreCase))
            {
                if (Element.Attributes["name"] != null)
                {
                    Name = Element.Attributes["name"].Value;
                }
                Data = Element.InnerXml;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Name of the application
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Name = "";

        /// <summary>
        /// Extra application specific data (string containing XML info)
        /// </summary>
        public string Data
        {
            get { return _Data; }
            set { _Data = value; }
        }
        private string _Data = null;
        #endregion

        #region Overridden Functions
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<Application name=\"" + Name + "\">");
            Builder.Append(Data);
            Builder.Append("</Application>");
            return Builder.ToString();
        }
        #endregion
    }
}
