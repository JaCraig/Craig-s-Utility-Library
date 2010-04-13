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
    /// Phone menu class
    /// </summary>
    public class Menu
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Menu()
        {
            MenuItems = new List<MenuItem>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Title of the menu
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Prompt of the menu
        /// </summary>
        public string Prompt { get; set; }

        /// <summary>
        /// Menu items for the menu
        /// </summary>
        public List<MenuItem> MenuItems { get; set; }

        #endregion

        #region Public Overridden Functions

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<CiscoIPPhoneMenu><Title>").Append(Title).Append("</Title><Prompt>")
                .Append(Prompt).Append("</Prompt>");
            for (int x = 0; x < MenuItems.Count; ++x)
            {
                Builder.Append(MenuItems[x].ToString());
            }
            Builder.Append("</CiscoIPPhoneMenu>");
            return Builder.ToString();
        }

        #endregion
    }
}

/*<CiscoIPPhoneGraphicMenu>

  <Title>Menu title goes here</Title>

  <Prompt>Prompt text goes here</Prompt>

  <LocationX>Position information of graphic</LocationX>

  <LocationY>Position information of graphic</LocationY>

  <Width>Size information for the graphic</Width>

  <Height>Size information for the graphic</Height>

  <Depth>Number of bits per pixel</Depth>

  <Data>Packed Pixel Data</Data>

  <MenuItem>

    <Name>The name of each menu item</Name>

    <URL>The URL associated with the menu item</URL>

  </MenuItem>

</CiscoIPPhoneGraphicMenu> 


*/

/*<CiscoIPPhoneGraphicFileMenu>

  <Title>Image Title goes here</Title>

  <Prompt>Prompt text goes here</Prompt>

  <LocationX>Horizontal position of graphic</LocationX>

  <LocationY>Vertical position of graphic</LocationY>

  <URL>Points to the PNG background image</URL>

  <MenuItem>

    <Name>Same as CiscoIPPhoneGraphicMenu</Name>

    <URL>Invoked when the TouchArea is touched</URL>

    <TouchArea X1="left edge" Y1="top edge" X2="right edge" Y2="bottom 
edge"/>

  </MenuItem>

</CiscoIPPhoneGraphicFileMenu>

*/

/*<CiscoIPPhoneIconMenu>

  <Title>Title text goes here</Title>

  <Prompt>Prompt text goes here</Prompt>

  <MenuItem>

    <IconIndex>Indicates what IconItem to display</IconIndex>

    <Name>The name of each menu item</Name>

    <URL>The URL associated with the menu item</URL>

  </MenuItem>

  <SoftKeyItem>

    <Name>Name of soft key</Name>

    <URL>URL or URI of soft key</URL>

    <Position>Position information of the soft key</Position>

  </SoftKeyItem>

  <IconItem>

    <Index>A unique index from 0 to 9</Index>

    <Height>Size information for the icon</Height>

    <Width>Size information for the icon</Width>

    <Depth>Number of bits per pixel</Depth>

    <Data>Packed Pixel Data</Data>

  </IconItem>

</CiscoIPPhoneIconMenu>

*/

/*<CiscoIPPhoneIconFileMenu>

  <Title>Title text goes here</Title>

  <Prompt>Prompt text goes here</Prompt>

  <MenuItem>

    <IconIndex>Indicates what IconItem to display</IconIndex>

    <Name>The name of each menu item</Name>

    <URL>The URL associated with the menu item</URL>

  </MenuItem>

  <IconItem>

    <Index>A unique index from 0 to 9</Index>

    <URL>location of the PNG icon image</URL>

  </IconItem>

</CiscoIPPhoneIconFileMenu>

*/