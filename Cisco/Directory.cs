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
using Utilities.Cisco.Interfaces;
#endregion

namespace Utilities.Cisco
{
    /// <summary>
    /// Directory class
    /// </summary>
    public class Directory:IDisplay
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Directory()
        {
            DirectoryEntries = new List<DirectoryEntry>();
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
        /// Directory entries
        /// </summary>
        public List<DirectoryEntry> DirectoryEntries { get; set; }

        /// <summary>
        /// Softkey entries
        /// </summary>
        public List<SoftKeyItem> SoftKeys { get; set; }

        #endregion

        #region Overridden Functions

        public override string ToString()
        {
            try
            {
                StringBuilder Builder = new StringBuilder();
                Builder.Append("<CiscoIPPhoneDirectory><Title>").Append(Title).Append("</Title><Prompt>")
                    .Append(Prompt).Append("</Prompt>");
                foreach (DirectoryEntry Entry in DirectoryEntries)
                {
                    Builder.Append(Entry.ToString());
                }
                foreach (SoftKeyItem Item in SoftKeys)
                {
                    Builder.Append(Item.ToString());
                }
                Builder.Append("</CiscoIPPhoneDirectory>");
                return Builder.ToString();
            }
            catch { throw; }
        }

        #endregion
    }
}