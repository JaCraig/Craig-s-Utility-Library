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

namespace Utilities.Web.Email.Pop3
{
    /// <summary>
    /// Class for containing the messages
    /// returned by the Pop3Client class.
    /// </summary>
    public class Message:Utilities.Web.Email.Message
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Message()
            : base()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The number associated with the message
        /// </summary>
        public long MessageNumber
        {
            get { return _MessageNumber; }
            set { _MessageNumber = value; }
        }
        private long _MessageNumber = 0;

        /// <summary>
        /// Size of the message in bytes
        /// </summary>
        public long MessageSize
        {
            get { return _MessageSize; }
            set { _MessageSize = value; }
        }
        private long _MessageSize = 0;

        /// <summary>
        /// If true, we've retrieved this message from the server
        /// </summary>
        public bool Retrieved
        {
            get { return _Retrieved; }
            set { _Retrieved = value; }
        }
        private bool _Retrieved = false;

        /// <summary>
        /// MIME version of the email
        /// </summary>
        public MIME.MIMEMessage MessageBody
        {
            get { return _Message; }
            set { _Message = value; }
        }
        private MIME.MIMEMessage _Message = null;
        #endregion

        #region Public Overridden Properties
        public override string Body
        {
            get
            {
                string ReturnValue=_Message.HTMLBodyText;
                if (string.IsNullOrEmpty(ReturnValue))
                    ReturnValue = _Message.PlainBodyText;
                if (string.IsNullOrEmpty(ReturnValue))
                    ReturnValue = _Message.BodyText;

                ReturnValue = ReturnValue.Replace("\r\n", "<br />");
                return ReturnValue;
            }
        }

        public override string From
        {
            get
            {
                return _Message.From;
            }
        }

        public override string To
        {
            get
            {
                return _Message.To;
            }
        }

        public override string Subject
        {
            get
            {
                return _Message.Subject;
            }
        }
        #endregion
    }
}