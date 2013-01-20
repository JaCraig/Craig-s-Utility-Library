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
using System.Text;
using System.Xml;
using Utilities.DataTypes.ExtensionMethods;
using System.Diagnostics.Contracts;
using System;
#endregion

namespace Utilities.FileFormats.BlogML
{
    /// <summary>
    /// Tag class
    /// </summary>
    public class Tag
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Tag()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">Element containing the tag info</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode")]
        public Tag(XmlElement Element)
        {
            Contract.Requires<ArgumentNullException>(Element != null, "Element");
            if (Element.Attributes["ref"] != null)
                REF = Element.Attributes["ref"].Value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the REF info for the tag (usually just the tag's name)
        /// </summary>
        public virtual string REF { get; set; }

        #endregion

        #region Overridden Functions

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <returns>The object as a string</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendFormat("<tag ref=\"{0}\" />", REF);
            return Builder.ToString();
        }

        #endregion
    }
}