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
using System.Net.Mail;
using System.Threading;
#endregion

namespace Utilities.Web.Email.SMTP
{
    /// <summary>
    /// Utility for sending an email
    /// </summary>
    public class EmailSender : Message
    {
        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public EmailSender()
        {
            Attachments = new List<Attachment>();
            Priority = MailPriority.Normal;
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
                Body = Message;
                SendMail();
            }
            catch { throw; }
        }

        /// <summary>
        /// Sends a piece of mail asynchronous
        /// </summary>
        /// <param name="Message">Message to be sent</param>
        public void SendMailAsync(string Message)
        {
            try
            {
                Body = Message;
                ThreadPool.QueueUserWorkItem(delegate { SendMail(); });
            }
            catch { throw; }
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        public void SendMail()
        {
            try
            {
                using (System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage())
                {
                    char[] Splitter = { ',' };
                    string[] AddressCollection = To.Split(Splitter);
                    for (int x = 0; x < AddressCollection.Length; ++x)
                    {
                        message.To.Add(AddressCollection[x]);
                    }
                    if (!string.IsNullOrEmpty(CC))
                    {
                        AddressCollection = CC.Split(Splitter);
                        for (int x = 0; x < AddressCollection.Length; ++x)
                        {
                            message.CC.Add(AddressCollection[x]);
                        }
                    }
                    if (!string.IsNullOrEmpty(Bcc))
                    {
                        AddressCollection = Bcc.Split(Splitter);
                        for (int x = 0; x < AddressCollection.Length; ++x)
                        {
                            message.Bcc.Add(AddressCollection[x]);
                        }
                    }
                    message.Subject = Subject;
                    message.From = new System.Net.Mail.MailAddress((From));
                    message.Body = Body;
                    message.Priority = Priority;
                    message.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                    message.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                    message.IsBodyHtml = true;
                    foreach (Attachment TempAttachment in Attachments)
                    {
                        message.Attachments.Add(TempAttachment);
                    }
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Server, Port);
                    if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
                    }
                    if (UseSSL)
                        smtp.EnableSsl = true;
                    else
                        smtp.EnableSsl = false;
                    smtp.Send(message);
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Sends a piece of mail asynchronous
        /// </summary>
        public void SendMailAsync()
        {
            try
            {
                ThreadPool.QueueUserWorkItem(delegate { SendMail(); });
            }
            catch { throw; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Any attachments that are included with this
        /// message.
        /// </summary>
        public List<Attachment> Attachments { get; set; }

        /// <summary>
        /// The priority of this message
        /// </summary>
        public MailPriority Priority { get; set; }

        /// <summary>
        /// Server Location
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// User Name for the server
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password for the server
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Port to send the information on
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Decides whether we are using STARTTLS (SSL) or not
        /// </summary>
        public bool UseSSL { get; set; }

        /// <summary>
        /// Carbon copy send (seperate email addresses with a comma)
        /// </summary>
        public string CC { get; set; }

        /// <summary>
        /// Blind carbon copy send (seperate email addresses with a comma)
        /// </summary>
        public string Bcc { get; set; }

        #endregion
    }
}