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
using Utilities.Cisco.Interfaces;
#endregion

namespace Utilities.Cisco
{
    /// <summary>
    /// Text class
    /// </summary>
    public class Text:IDisplay
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Text()
        {
            SoftKeys = new List<SoftKeyItem>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Prompt
        /// </summary>
        public string Prompt { get; set; }

        /// <summary>
        /// Text
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Softkey list
        /// </summary>
        public List<SoftKeyItem> SoftKeys { get; set; }

        #endregion

        #region Public Overridden Functions

        public override string ToString()
        {
                StringBuilder Builder = new StringBuilder();
                Builder.Append("<CiscoIPPhoneText><Title>").Append(Title).Append("</Title><Prompt>")
                    .Append(Prompt).Append("</Prompt><Text>").Append(Content).Append("</Text>");
                foreach (SoftKeyItem Item in SoftKeys)
                {
                    Builder.Append(Item.ToString());
                }
                Builder.Append("</CiscoIPPhoneText>");
                return Builder.ToString();
        }

        #endregion
    }
}