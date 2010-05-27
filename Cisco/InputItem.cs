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
    /// Input item
    /// </summary>
    public class InputItem
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public InputItem()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Query string parameter
        /// </summary>
        public string QueryStringParam { get; set; }

        /// <summary>
        /// Default value
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// input flags
        /// </summary>
        public InputFlag InputFlags { get; set; }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            try
            {
                StringBuilder Builder = new StringBuilder();
                Builder.Append("<InputItem><DisplayName>").Append(DisplayName).Append("</DisplayName><QueryStringParam>")
                    .Append(QueryStringParam).Append("</QueryStringParam><DefaultValue>").Append(DefaultValue)
                    .Append("</DefaultValue><InputFlags>");
                if (InputFlags == InputFlag.ASCII)
                {
                    Builder.Append("A");
                }
                else if (InputFlags == InputFlag.TelephoneNumber)
                {
                    Builder.Append("T");
                }
                else if (InputFlags == InputFlag.Numeric)
                {
                    Builder.Append("N");
                }
                else if (InputFlags == InputFlag.Equation)
                {
                    Builder.Append("E");
                }
                else if (InputFlags == InputFlag.Uppercase)
                {
                    Builder.Append("U");
                }
                else if (InputFlags == InputFlag.Lowercase)
                {
                    Builder.Append("L");
                }
                else if (InputFlags == InputFlag.Password)
                {
                    Builder.Append("P");
                }
                Builder.Append("</InputFlags></InputItem>");
                return Builder.ToString();
            }
            catch { throw; }
        }

        #endregion
    }
}