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

#endregion

namespace Utilities.Web.Email.MIME
{
    /// <summary>
    /// Media type struct
    /// </summary>
    public struct MediaType
    {
        /// <summary>
        /// Enum associated with the sub type and file extension
        /// </summary>
        public MediaEnum MediaEnum;
        /// <summary>
        /// Sub type
        /// </summary>
        public string SubType;
        /// <summary>
        /// File extension
        /// </summary>
        public string FileExtension;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MediaEnum">The media type</param>
        /// <param name="SubType">The sub type</param>
        /// <param name="FileExtension">File extension associated with the media type and sub type</param>
        public MediaType(MediaEnum MediaEnum, string SubType, string FileExtension)
        {
            this.MediaEnum = MediaEnum;
            this.SubType = SubType;
            this.FileExtension = FileExtension;
        }
    }
}