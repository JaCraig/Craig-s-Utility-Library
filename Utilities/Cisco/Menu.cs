/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
    /// Phone menu class
    /// </summary>
    public class Menu : IDisplay
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Menu()
        {
            MenuItems = new List<IMenuItem>();
            SoftKeys = new List<SoftKeyItem>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Title of the menu
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Prompt of the menu
        /// </summary>
        public virtual string Prompt { get; set; }

        /// <summary>
        /// Menu items for the menu
        /// </summary>
        public virtual List<IMenuItem> MenuItems { get; set; }

        /// <summary>
        /// X location of backgroun image (if present)
        /// </summary>
        public virtual int X { get; set; }

        /// <summary>
        /// Y location of backgroun image (if present)
        /// </summary>
        public virtual int Y { get; set; }

        /// <summary>
        /// URL for background image (if needed)
        /// </summary>
        public virtual string ImageURL { get; set; }

        public virtual List<SoftKeyItem> SoftKeys { get; set; }

        #endregion

        #region Public Overridden Functions

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            if (string.IsNullOrEmpty(ImageURL))
            {
                Builder.Append("<CiscoIPPhoneMenu>");
            }
            else
            {
                Builder.Append("<CiscoIPPhoneGraphicFileMenu>");
            }
            Builder.Append("<Title>").Append(Title).Append("</Title><Prompt>")
                .Append(Prompt).Append("</Prompt>");
            if (!string.IsNullOrEmpty(ImageURL))
            {
                Builder.Append("<LocationX>").Append(X).Append("</LocationX><LocationY>").Append(Y)
                    .Append("</LocationY><URL>").Append(ImageURL).Append("</URL>");
            }
            if (MenuItems != null)
            {
                for (int x = 0; x < MenuItems.Count; ++x)
                {
                    if(MenuItems[x]!=null)
                        Builder.Append(MenuItems[x].ToString());
                }
            }
            if (SoftKeys != null)
            {
                for (int x = 0; x < SoftKeys.Count; ++x)
                {
                    if(SoftKeys[x]!=null)
                        Builder.Append(SoftKeys[x].ToString());
                }
            }
            if (string.IsNullOrEmpty(ImageURL))
            {
                Builder.Append("</CiscoIPPhoneMenu>");
            }
            else
            {
                Builder.Append("</CiscoIPPhoneGraphicFileMenu>");
            }
            return Builder.ToString();
        }

        #endregion
    }
}