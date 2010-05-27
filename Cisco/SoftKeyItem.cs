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
using System.Linq;
using System.Text;
#endregion

namespace Utilities.Cisco
{
    /// <summary>
    /// Softkey class
    /// </summary>
    public class SoftKeyItem
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SoftKeyItem()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the softkey
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URL action for release event
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// URL action for press event
        /// </summary>
        public string URLDown { get; set; }

        /// <summary>
        /// position of softkey
        /// </summary>
        public int Position { get; set; }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            try
            {
                StringBuilder Builder = new StringBuilder();
                Builder.Append("<SoftKeyItem><Name>").Append(Name).Append("</Name><URL>").Append(URL).Append("</URL><URLDown>")
                    .Append(URLDown).Append("</URLDown><Position>").Append(Position).Append("</Position></SoftKeyItem>");
                return Builder.ToString();
            }
            catch { throw; }
        }

        #endregion
    }
}