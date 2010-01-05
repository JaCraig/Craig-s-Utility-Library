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

#endregion

namespace Utilities.Web.Email.MIME
{
    /// <summary>
    /// Body of the MIME message
    /// </summary>
    public class MIMEBody
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MIMEBody()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Input">Body text</param>
        /// <param name="Header">Header of the message</param>
        public MIMEBody(string Input,MIMEHeader Header)
        {
            if (string.IsNullOrEmpty(Input))
            {
                throw new ArgumentNullException("Input can not be null");
            }
            MediaEnum ContentType;
            ContentType=GetMediaType(Header);
            if(MediaEnum.MEDIA_MULTIPART==ContentType)
            {
                string CurrentBoundry=GetBoundryMarker(Header);
                if (string.IsNullOrEmpty(CurrentBoundry))
                    return;
                CurrentBoundry = CurrentBoundry.Replace("\"", "");
                
                string BoundryStart = "--"+CurrentBoundry;
                string BoundryEnd = BoundryStart+"--";

                int StartIndex = Input.IndexOf(BoundryStart, 0);
                if (StartIndex == -1) return;
                int EndIndex = Input.IndexOf(BoundryEnd, 0);
                if (EndIndex == -1) EndIndex = Input.Length;

                Content = Input.Substring(0, StartIndex);
                while (StartIndex < EndIndex)
                {
                    StartIndex += BoundryStart.Length + 2;
                    if (StartIndex >= EndIndex)
                        break;
                    int TempIndex = Input.IndexOf(BoundryStart, StartIndex);
                    if (TempIndex != -1)
                    {
                        Boundries.Add(new MIMEMessage(Input.Substring(StartIndex, TempIndex - StartIndex)));
                    }
                    else
                        break;
                    StartIndex = TempIndex;
                }
            }
            else
            {
                Content = Input;
            }
            string Encoding = "";
            try
            {
                Encoding = Header[Constants.TransferEncoding].Attributes[0].Value;
            }
            catch
            {
                Encoding = Constants.Encoding7Bit;
            }
            Code CodeUsing = CodeManager.Instance[Encoding];
            CodeUsing.CharacterSet = Header[Constants.ContentType][Constants.Charset];
            CodeUsing.Decode(Content, out _Content);
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Gets the media type of the message
        /// </summary>
        /// <param name="Header">Header of the message</param>
        /// <returns>The media type</returns>
        private MediaEnum GetMediaType(MIMEHeader Header)
        {
            string ContentType = GetContentType(Header);
            int x=0;
            foreach (string TempType in MIMEType.TypeTable)
            {
                if (TempType.Equals(ContentType, StringComparison.InvariantCultureIgnoreCase))
                {
                    return (MediaEnum)x;
                }
                ++x;
            }
            return (MediaEnum)MIMEType.TypeTable.Length - 1;
        }

        /// <summary>
        /// Gets the content type
        /// </summary>
        /// <param name="Header">Header of the message</param>
        /// <returns>A string containing the content type</returns>
        private string GetContentType(MIMEHeader Header)
        {
            if (Header != null && Header[Constants.ContentType] != null && Header[Constants.ContentType].Attributes.Count > 0)
            {
                string ContentType = Header[Constants.ContentType].Attributes[0].Value;
                if (null != ContentType)
                {
                    int Index = ContentType.IndexOf('/', 0);
                    if (Index != -1)
                    {
                        return ContentType.Substring(0, Index);
                    }
                    else
                    {
                        return ContentType;
                    }
                }
            }
            return "text";
        }

        /// <summary>
        /// Gets the boundary marker
        /// </summary>
        /// <param name="Header">Header of the message</param>
        /// <returns>A string containing the boundary marker</returns>
        private string GetBoundryMarker(MIMEHeader Header)
        {
            return Header[Constants.ContentType][Constants.Boundary];
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Boundaries found within this item (files/messages)
        /// </summary>
        public List<MIMEMessage> Boundries
        {
            get { return _Boundries; }
            set { _Boundries = value; }
        }
        private List<MIMEMessage> _Boundries = new List<MIMEMessage>();

        /// <summary>
        /// Content of this boundary/message
        /// </summary>
        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }
        private string _Content = "";
        #endregion
    }
}