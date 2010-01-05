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
using System.Xml;
#endregion

namespace Utilities.Web.Netflix
{
    /// <summary>
    /// Box art information
    /// </summary>
    public class BoxArt
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Element">Element information</param>
        public BoxArt(XmlElement Element)
        {
            if (Element.Attributes["small"] != null)
            {
                SmallPicture = Element.Attributes["small"].Value;
            }
            if (Element.Attributes["medium"] != null)
            {
                MediumPicture = Element.Attributes["medium"].Value;
            }
            if (Element.Attributes["large"] != null)
            {
                LargePicture = Element.Attributes["large"].Value;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Small picture
        /// </summary>
        public string SmallPicture { get; set; }

        /// <summary>
        /// Medium picture
        /// </summary>
        public string MediumPicture { get; set; }

        /// <summary>
        /// Large picture
        /// </summary>
        public string LargePicture { get; set; }

        #endregion
    }
}