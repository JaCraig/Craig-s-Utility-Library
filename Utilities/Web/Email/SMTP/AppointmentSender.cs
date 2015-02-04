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

using System.IO;
using System.Net.Mail;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.FileFormats;

namespace Utilities.Web.Email.SMTP
{
    /// <summary>
    /// Sends appointments through SMTP
    /// </summary>
    public class AppointmentSender : EmailSender
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AppointmentSender()
            : base()
        {
            AppointmentInfo = new VCalendar();
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="MessageBody">The body of the message</param>
        public override void SendMail(string MessageBody = "")
        {
            using (MailMessage Mail = new MailMessage())
            {
                using (AlternateView TextView = AlternateView.CreateAlternateViewFromString(AppointmentInfo.ToString(), new System.Net.Mime.ContentType("text/plain")))
                {
                    using (AlternateView HTMLView = AlternateView.CreateAlternateViewFromString(AppointmentInfo.GetHCalendar(), new System.Net.Mime.ContentType("text/html")))
                    {
                        System.Net.Mime.ContentType CalendarType = new System.Net.Mime.ContentType("text/calendar");
                        CalendarType.Parameters.Add("method", AppointmentInfo.Cancel ? "CANCEL" : "REQUEST");
                        CalendarType.Parameters.Add("name", "meeting.ics");
                        using (AlternateView CalendarView = AlternateView.CreateAlternateViewFromString(AppointmentInfo.GetICalendar(), CalendarType))
                        {
                            CalendarView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;
                            Mail.AlternateViews.Add(TextView);
                            Mail.AlternateViews.Add(HTMLView);
                            Mail.AlternateViews.Add(CalendarView);
                            char[] Splitter = { ',', ';' };
                            string[] AddressCollection = To.Split(Splitter);
                            for (int x = 0; x < AddressCollection.Length; ++x)
                            {
                                if (!string.IsNullOrEmpty(AddressCollection[x].Trim()))
                                    Mail.To.Add(AddressCollection[x]);
                            }
                            if (!string.IsNullOrEmpty(CC))
                            {
                                AddressCollection = CC.Split(Splitter);
                                for (int x = 0; x < AddressCollection.Length; ++x)
                                {
                                    if (!string.IsNullOrEmpty(AddressCollection[x].Trim()))
                                        Mail.CC.Add(AddressCollection[x]);
                                }
                            }
                            if (!string.IsNullOrEmpty(Bcc))
                            {
                                AddressCollection = Bcc.Split(Splitter);
                                for (int x = 0; x < AddressCollection.Length; ++x)
                                {
                                    if (!string.IsNullOrEmpty(AddressCollection[x].Trim()))
                                        Mail.Bcc.Add(AddressCollection[x]);
                                }
                            }
                            Mail.From = new MailAddress(From);
                            Mail.Subject = Subject;
                            foreach (Attachment Attachment in Attachments)
                            {
                                Mail.Attachments.Add(Attachment);
                            }
                            if (Attachments.Count > 0)
                                Mail.Attachments.Add(new Attachment(new MemoryStream(AppointmentInfo.GetICalendar().ToByteArray()), "meeting.ics"));
                            Mail.Priority = Priority;
                            Mail.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                            Mail.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                            using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Server, Port))
                            {
                                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                                {
                                    smtp.Credentials = new System.Net.NetworkCredential(UserName, Password);
                                }
                                if (UseSSL)
                                    smtp.EnableSsl = true;
                                else
                                    smtp.EnableSsl = false;
                                smtp.Send(Mail);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Contains all of the appointment info
        /// </summary>
        public VCalendar AppointmentInfo { get; set; }
    }
}