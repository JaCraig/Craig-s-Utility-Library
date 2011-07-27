/*
Copyright (c) 2008 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Net.Mail;
using System.Threading;
#endregion

namespace Utilities.Email
{
    /// <summary>
    /// Utility for sending an email
    /// </summary>
    public class EmailSender
    {
        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EmailSender()
        {
            Attachment_ = null;
            Priority_ = MailPriority.Normal;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="Message">The body of the message</param>
        public void SendMail(string Message)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                char[] Splitter = { ',' };
                string[] AddressCollection = to.Split(Splitter);
                for (int x = 0; x < AddressCollection.Length; ++x)
                {
                    message.To.Add(AddressCollection[x]);
                }
                message.Subject = subject;
                message.From = new System.Net.Mail.MailAddress((from));
                message.Body = Message;
                message.Priority = Priority_;
                message.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                message.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                message.IsBodyHtml = true;
                if (Attachment_ != null)
                {
                    message.Attachments.Add(Attachment_);
                }
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Server,Port);
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(UserName,Password);
                }
                smtp.Send(message);
                message.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        /// <summary>
        /// Sends a piece of mail asynchronous
        /// </summary>
        /// <param name="Message">Message to be sent</param>
        public void SendMailAsync(string Message)
        {
            ThreadPool.QueueUserWorkItem(delegate { SendMail(Message); });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Whom the message is to
        /// </summary>
        public string To
        {
            get
            {
                return to;
            }
            set
            {
                to = value;
            }
        }

        /// <summary>
        /// The subject of the email
        /// </summary>
        public string Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
            }
        }

        /// <summary>
        /// Whom the message is from
        /// </summary>
        public string From
        {
            get
            {
                return from;
            }
            set
            {
                from = value;
            }
        }

        /// <summary>
        /// Any attachments that are included with this
        /// message.
        /// </summary>
        public Attachment Attachment
        {
            get
            {
                return Attachment_;
            }
            set
            {
                Attachment_ = value;
            }
        }

        /// <summary>
        /// The priority of this message
        /// </summary>
        public MailPriority Priority
        {
            get
            {
                return Priority_;
            }
            set
            {
                Priority_ = value;
            }
        }

        /// <summary>
        /// Server Location
        /// </summary>
        public string Server
        {
            get { return Server_; }
            set { Server_ = value; }
        }

        /// <summary>
        /// User Name for the server
        /// </summary>
        public string UserName
        {
            get { return UserName_; }
            set { UserName_ = value; }
        }

        /// <summary>
        /// Password for the server
        /// </summary>
        public string Password
        {
            get { return Password_; }
            set { Password_ = value; }
        }

        /// <summary>
        /// Port to send the information on
        /// </summary>
        public int Port
        {
            get { return Port_; }
            set { Port_ = value; }
        }

        #endregion

        #region Variables

        private MailPriority Priority_;
        private Attachment Attachment_;
        private string from;
        private string subject;
        private string to;
        private string Server_;
        private string UserName_;
        private string Password_;
        private int Port_;

        #endregion
    }
}