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
using System;
using System.Collections.Generic;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Web.Google.Enums;
using Utilities.Web.ExtensionMethods;
using Utilities.Web.Google.Interfaces;
#endregion

namespace Utilities.Web.Google.HelperClasses
{
    /// <summary>
    /// Holds data for displaying a set of markers on a map
    /// </summary>
    public class Markers
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Markers() 
        { 
            MarkerList = new List<ILocation>(); 
            Size=MarkerSize.Small;
            Color="0xFF0000";
            Label="";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Marker list
        /// </summary>
        public ICollection<ILocation> MarkerList { get;private set; }

        /// <summary>
        /// Marker size
        /// </summary>
        public MarkerSize Size { get; set; }

        /// <summary>
        /// Marker color (24 bit hex color values)
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Single uppercase alphanumeric character
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Custom icon (may be PNG, JPEG, or GIF but PNG is recommended)
        /// </summary>
        public Uri CustomIcon { get; set; }

        /// <summary>
        /// Should a shadow be generated from the custom icon?
        /// </summary>
        public bool CustomIconShadow { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Exports the location as an url encoded string
        /// </summary>
        /// <returns>Url encoded string of the location</returns>
        public override string ToString()
        {
            string ReturnValue = "size:" + Size.ToString().ToLower()
                    + (Color.IsNullOrEmpty() ? "" : "|color:" + Color)
                    + (Label.IsNullOrEmpty() ? "" : "|label:" + Label.ToUpper())
                    + (CustomIcon.IsNull() ? "" : ("|icon:" + CustomIcon + "|shadow:" + CustomIconShadow.ToString().ToLower()));
            foreach (ILocation Marker in MarkerList)
                ReturnValue += "|" + Marker.ToString();
            return "markers=" + ReturnValue.URLEncode();
        }

        #endregion
    }
}
