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
    /// Class containing constant used by the MIME parser
    /// </summary>
    public static class Constants
    {
		public static string MimeVersion { get{return "MIME-Version";} }
		public static string ContentType { get{ return "Content-Type";} }
		public static string TransferEncoding { get{return "Content-Transfer-Encoding";} }
		public static string ContentID { get{return "Content-ID";} }
		public static string ContentDescription { get{return "Content-Description";} }
		public static string ContentDisposition { get{return "Content-Disposition";} }
		public static string Charset { get{return "charset";} }
        public static string Subject { get { return "Subject"; } }
        public static string To { get { return "To"; } }
        public static string From { get { return "From"; } }
		public static string Name { get{return "name";} }
		public static string Filename { get{return "filename";} }
		public static string Boundary { get{return "boundary";} }
		public static string Encoding7Bit { get{return "7bit";} }
		public static string Encoding8Bit { get{return "8bit";} }
		public static string EncodingBinary { get{return "binary";} }
		public static string EncodingQP { get{return "quoted-printable";} }
		public static string EncodingBase64 { get{return "base64";} }
		public static string MediaText { get{return "text";} }
		public static string MediaImage { get{return "image";} }
		public static string MediaAudio { get{return "audio";} }
		public static string MediaVideo { get{return "vedio";} }
		public static string MediaApplication { get{return "application";} }
		public static string MediaMultiPart { get{return "multipart";} }
		public static string MediaMessage { get{return "message";} }
    }
}