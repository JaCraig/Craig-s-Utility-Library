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

#endregion

namespace Utilities.Web.Email.MIME
{
    /// <summary>
    /// Class containing constant used by the MIME parser
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// MIME version
        /// </summary>
		public static string MimeVersion { get{return "MIME-Version";} }
        /// <summary>
        /// Content type
        /// </summary>
		public static string ContentType { get{ return "Content-Type";} }
        /// <summary>
        /// Transfer encoding
        /// </summary>
		public static string TransferEncoding { get{return "Content-Transfer-Encoding";} }
        /// <summary>
        /// Content ID
        /// </summary>
		public static string ContentID { get{return "Content-ID";} }
        /// <summary>
        /// Content Description
        /// </summary>
		public static string ContentDescription { get{return "Content-Description";} }
        /// <summary>
        /// Content disposition
        /// </summary>
		public static string ContentDisposition { get{return "Content-Disposition";} }
        /// <summary>
        /// Charset
        /// </summary>
		public static string Charset { get{return "charset";} }
        /// <summary>
        /// Subject
        /// </summary>
        public static string Subject { get { return "Subject"; } }
        /// <summary>
        /// To
        /// </summary>
        public static string To { get { return "To"; } }
        /// <summary>
        /// From
        /// </summary>
        public static string From { get { return "From"; } }
        /// <summary>
        /// Name
        /// </summary>
		public static string Name { get{return "name";} }
        /// <summary>
        /// Filename
        /// </summary>
		public static string Filename { get{return "filename";} }
        /// <summary>
        /// Boundary
        /// </summary>
		public static string Boundary { get{return "boundary";} }
        /// <summary>
        /// Encoding 7bit
        /// </summary>
		public static string Encoding7Bit { get{return "7bit";} }
        /// <summary>
        /// Encoding 8bit
        /// </summary>
		public static string Encoding8Bit { get{return "8bit";} }
        /// <summary>
        /// Encoding binary
        /// </summary>
		public static string EncodingBinary { get{return "binary";} }
        /// <summary>
        /// Encoding QP
        /// </summary>
		public static string EncodingQP { get{return "quoted-printable";} }
        /// <summary>
        /// Encoding base64
        /// </summary>
		public static string EncodingBase64 { get{return "base64";} }
        /// <summary>
        /// Media text
        /// </summary>
		public static string MediaText { get{return "text";} }
        /// <summary>
        /// Media image
        /// </summary>
		public static string MediaImage { get{return "image";} }
        /// <summary>
        /// Media audio
        /// </summary>
		public static string MediaAudio { get{return "audio";} }
        /// <summary>
        /// Media video
        /// </summary>
		public static string MediaVideo { get{return "vedio";} }
        /// <summary>
        /// Media application
        /// </summary>
		public static string MediaApplication { get{return "application";} }
        /// <summary>
        /// Media multi part
        /// </summary>
		public static string MediaMultiPart { get{return "multipart";} }
        /// <summary>
        /// Media message
        /// </summary>
		public static string MediaMessage { get{return "message";} }
    }
}