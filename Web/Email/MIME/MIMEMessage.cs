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

#endregion

namespace Utilities.Web.Email.MIME
{
    /// <summary>
    /// Base message class for MIME messages
    /// </summary>
    public class MIMEMessage
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public MIMEMessage()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Input">string containing the MIME message</param>
        public MIMEMessage(string Input)
        {
            LoadMessage(Input);
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Loads the message
        /// </summary>
        /// <param name="Input">string containing the message</param>
        public void LoadMessage(string Input)
        {
            _Content = Input;
            int HeaderEnd = Input.IndexOf("\r\n\r\n");
            Header = new MIMEHeader(Input.Substring(0, HeaderEnd + 2));
            Input = Input.Substring(HeaderEnd + 2);
            Body = new MIMEBody(Input, Header);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Header of the message
        /// </summary>
        public MIMEHeader Header
        {
            get { return _Header; }
            set { _Header = value; }
        }

        private MIMEHeader _Header = null;

        /// <summary>
        /// Body of the message (may contain sub messages/boundries)
        /// </summary>
        public MIMEBody Body
        {
            get { return _Body; }
            set { _Body = value; }
        }

        private MIMEBody _Body = null;

        /// <summary>
        /// Subject of the message
        /// </summary>
        public string Subject
        {
            get { try { return Header[Constants.Subject].Attributes[0].Value; } catch { return ""; } }
        }

        /// <summary>
        /// Whom the message is to
        /// </summary>
        public string To
        {
            get { try { return Header[Constants.To].Attributes[0].Value; } catch { return ""; } }
        }

        /// <summary>
        /// Whom the message is from
        /// </summary>
        public string From
        {
            get { try { return Header[Constants.From].Attributes[0].Value; } catch { return ""; } }
        }

        /// <summary>
        /// The text of the message
        /// </summary>
        public string BodyText
        {
            get
            {
                try
                {
                    if (GetMediaType(Header)==MediaEnum.MEDIA_TEXT)
                    {
                        return Body.Content;
                    }
                    foreach (MIMEMessage TempMessage in Body.Boundries)
                    {
                        if (!string.IsNullOrEmpty(TempMessage.BodyText))
                        {
                            return TempMessage.BodyText;
                        }
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets the HTML version of the text
        /// </summary>
        public string HTMLBodyText
        {
            get
            {
                try
                {
                    if (GetMediaType(Header) == MediaEnum.MEDIA_TEXT && GetContentSubType(Header).Equals("html"))
                    {
                        return Body.Content;
                    }
                    foreach (MIMEMessage TempMessage in Body.Boundries)
                    {
                        if (!string.IsNullOrEmpty(TempMessage.BodyText))
                        {
                            return TempMessage.BodyText;
                        }
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets the plain version of the text
        /// </summary>
        public string PlainBodyText
        {
            get
            {
                try
                {
                    if (GetMediaType(Header) == MediaEnum.MEDIA_TEXT&&GetContentSubType(Header).Equals("plain"))
                    {
                        return Body.Content;
                    }
                    foreach (MIMEMessage TempMessage in Body.Boundries)
                    {
                        if (!string.IsNullOrEmpty(TempMessage.BodyText))
                        {
                            return TempMessage.BodyText;
                        }
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Gets the content type
        /// </summary>
        /// <param name="Header">Header of the message</param>
        /// <returns>A string stating the content type</returns>
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
        /// Gets the media type for the body
        /// </summary>
        /// <param name="Header">The header of the message</param>
        /// <returns>An enum value indicating the media type of the boundary</returns>
        private MediaEnum GetMediaType(MIMEHeader Header)
        {
            string ContentType = GetContentType(Header);
            int x = 0;
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
        /// Gets the sub type (used to determine if the item is HTML or plain text)
        /// </summary>
        /// <param name="Header">Header for this boundary/message</param>
        /// <returns>a string indicating the sub type of the boundary/message</returns>
        private string GetContentSubType(MIMEHeader Header)
        {
            if (Header != null && Header[Constants.ContentType] != null && Header[Constants.ContentType].Attributes.Count > 0)
            {
                string ContentType = Header[Constants.ContentType].Attributes[0].Value;
                if (null != ContentType)
                {
                    int Index = ContentType.IndexOf('/', 0);
                    if (Index != -1)
                    {
                        return ContentType.Substring(Index + 1, ContentType.Length - (Index + 1));
                    }
                    else
                    {
                        return ContentType;
                    }
                }
            }
            return "text";
        }
        #endregion

        #region Public Overriden Functions
        private string _Content="";

        /// <summary>
        /// To string function returns the content of the message (including header)
        /// </summary>
        /// <returns>A string containing the initial message</returns>
        public override string ToString()
        {
            return _Content;
        }
        #endregion
    }
}