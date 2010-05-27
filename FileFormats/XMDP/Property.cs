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
using System.Collections.Generic;
using System.Text;
#endregion

namespace Utilities.FileFormats.XMDP
{
    /// <summary>
    /// Class for holding the individual properties and their
    /// descriptions.
    /// </summary>
    public class Property
    {
        #region Constructors

        /// <summary>
        /// Constructors
        /// </summary>
        public Property()
        {
            Properties = new List<Property>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the property
        /// </summary>
        public string Name{get;set;}

        /// <summary>
        /// Description of the property
        /// </summary>
        public string Description{get;set;}

        /// <summary>
        /// List of sub properties (only used for rel type when you need to
        /// define new link types)
        /// </summary>
        public List<Property>Properties{get;set;}

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            try
            {
                StringBuilder Builder = new StringBuilder();
                Builder.Append("<dt id=\"" + Name + "\">" + Name + "</dt>");
                if (Properties.Count > 0)
                {
                    Builder.Append("<dd><dl>");
                    foreach (Property Property in Properties)
                    {
                        Builder.Append(Property.ToString());
                    }
                    Builder.Append("</dl></dd>");
                }
                else
                {
                    Builder.Append("<dd>" + Description + "</dd>");
                }
                return Builder.ToString();
            }
            catch { throw; }
        }

        #endregion
    }
}
