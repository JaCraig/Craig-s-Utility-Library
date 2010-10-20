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
    /// Status class
    /// </summary>
    public class Status : IDisplay
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Status()
        {
            SoftKeys = new List<SoftKeyItem>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Timer value in seconds
        /// </summary>
        public int Timer { get; set; }

        /// <summary>
        /// X location
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y location
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Location of the image
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Soft key list
        /// </summary>
        public List<SoftKeyItem> SoftKeys { get; set; }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<CiscoIPPhoneStatusFile><Text>").Append(Text).Append("</Text><Timer>").Append(Timer).Append("</Timer<LocationX>")
                .Append(X).Append("</LocationX><LocationY>").Append(Y).Append("</LocationY><URL>").Append(URL)
                .Append("</URL>");
            foreach (SoftKeyItem Item in SoftKeys)
            {
                Builder.Append(Item.ToString());
            }
            Builder.Append("</CiscoIPPhoneStatusFile>");
            return Builder.ToString();
        }

        #endregion
    }
}