/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

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

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Mail;
using System.Net.Mime;
using Utilities.DataTypes;
using Utilities.IO.Messaging.BaseClasses;

namespace Utilities.IO.Messaging.Default
{
    /// <summary>
    /// SMTP emailer
    /// </summary>
    public class SMTPSystem : MessagingSystemBase
    {
        /// <summary>
        /// Message type accepts
        /// </summary>
        public override Type MessageType { get { return typeof(EmailMessage); } }

        /// <summary>
        /// Name of the system
        /// </summary>
        public override string Name { get { return "SMTP"; } }

        /// <summary>
        /// Internal send message
        /// </summary>
        /// <param name="Message2">Message</param>
        protected override void InternalSend(Interfaces.IMessage Message2)
        {
            EmailMessage Message = Message2 as EmailMessage;
            if (Message == null)
                return;
            if (string.IsNullOrEmpty(Message.Body))
                Message.Body = " ";
            using (System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage())
            {
                char[] Splitter = { ',', ';' };
                string[] AddressCollection = Message.To.Split(Splitter);
                for (int x = 0; x < AddressCollection.Length; ++x)
                {
                    if (!string.IsNullOrEmpty(AddressCollection[x].Trim()))
                        message.To.Add(AddressCollection[x]);
                }
                if (!string.IsNullOrEmpty(Message.CC))
                {
                    AddressCollection = Message.CC.Split(Splitter);
                    for (int x = 0; x < AddressCollection.Length; ++x)
                    {
                        if (!string.IsNullOrEmpty(AddressCollection[x].Trim()))
                            message.CC.Add(AddressCollection[x]);
                    }
                }
                if (!string.IsNullOrEmpty(Message.Bcc))
                {
                    AddressCollection = Message.Bcc.Split(Splitter);
                    for (int x = 0; x < AddressCollection.Length; ++x)
                    {
                        if (!string.IsNullOrEmpty(AddressCollection[x].Trim()))
                            message.Bcc.Add(AddressCollection[x]);
                    }
                }
                message.Subject = Message.Subject;
                if (!string.IsNullOrEmpty(Message.From))
                    message.From = new System.Net.Mail.MailAddress(Message.From);
                using (AlternateView BodyView = AlternateView.CreateAlternateViewFromString(Message.Body, null, MediaTypeNames.Text.Html))
                {
                    foreach (LinkedResource Resource in Message.EmbeddedResources.Check(new List<LinkedResource>()))
                    {
                        BodyView.LinkedResources.Add(Resource);
                    }
                    message.AlternateViews.Add(BodyView);
                    message.Priority = Message.Priority;
                    message.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                    message.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                    message.IsBodyHtml = true;
                    foreach (Attachment TempAttachment in Message.Attachments.Check(new List<Attachment>()))
                    {
                        message.Attachments.Add(TempAttachment);
                    }
                    if (!string.IsNullOrEmpty(Message.Server))
                    {
                        SendMessage(new System.Net.Mail.SmtpClient(Message.Server, Message.Port), Message, message);
                    }
                    else
                    {
                        SendMessage(new SmtpClient(), Message, message);
                    }
                }
            }
        }

        /// <summary>
        /// Sends the message
        /// </summary>
        /// <param name="smtpClient">SMTP client object</param>
        /// <param name="Message">Email message object</param>
        /// <param name="message">Mail message object</param>
        private static void SendMessage(SmtpClient smtpClient, EmailMessage Message, MailMessage message)
        {
            Contract.Requires<ArgumentNullException>(Message != null, "Message");
            Contract.Requires<ArgumentNullException>(smtpClient != null, "smtpClient");
            Contract.Requires<ArgumentNullException>(message != null, "message");
            using (System.Net.Mail.SmtpClient smtp = smtpClient)
            {
                if (!string.IsNullOrEmpty(Message.UserName) && !string.IsNullOrEmpty(Message.Password))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(Message.UserName, Message.Password);
                }
                if (Message.UseSSL)
                    smtp.EnableSsl = true;
                else
                    smtp.EnableSsl = false;
                smtp.Send(message);
            }
        }
    }
}