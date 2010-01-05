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
    /// Defines basic MIME Types
    /// </summary>
    public class MIMEType
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MIMEType()
        {
        }
        #endregion

        #region Public Variables
        /// <summary>
        /// Defines the types in string form
        /// </summary>
        public static readonly string[] TypeTable = { "text", "image", "audio", "vedio", "application", "multipart", "message", null };

        /// <summary>
        /// Defines the sub types, file extensions, and media types
        /// </summary>
        public static readonly MediaType[] TypeCvtTable =
            new MediaType[] {
								   // media-type, sub-type, file extension
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "xml", "xml" ),
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "msword", "doc" ),
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "rtf", "rtf" ),
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "vnd.ms-excel", "xls" ),
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "vnd.ms-powerpoint", "ppt" ),
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "pdf", "pdf" ),
								   new MediaType( MediaEnum.MEDIA_APPLICATION, "zip", "zip" ),

								   new MediaType( MediaEnum.MEDIA_IMAGE, "jpeg", "jpeg" ),
								   new MediaType( MediaEnum.MEDIA_IMAGE, "jpeg", "jpg" ),
								   new MediaType( MediaEnum.MEDIA_IMAGE, "gif", "gif" ),
								   new MediaType( MediaEnum.MEDIA_IMAGE, "tiff", "tif" ),
								   new MediaType( MediaEnum.MEDIA_IMAGE, "tiff", "tiff" ),

								   new MediaType( MediaEnum.MEDIA_AUDIO, "basic", "wav" ),
								   new MediaType( MediaEnum.MEDIA_AUDIO, "basic", "mp3" ),

								   new MediaType( MediaEnum.MEDIA_VEDIO, "mpeg", "mpg" ),
								   new MediaType( MediaEnum.MEDIA_VEDIO, "mpeg", "mpeg" ),

								   new MediaType( MediaEnum.MEDIA_UNKNOWN, "", "" )		// add new subtypes before this line
							   };
        #endregion
    }
}