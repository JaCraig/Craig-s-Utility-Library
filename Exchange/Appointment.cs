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
using System.Net;
using System.Net.Mail;
using System.Text;
#endregion


namespace Utilities.Exchange
{
    /// <summary>
    /// Utility class for creating and sending an appointment
    /// </summary>
    public class Appointment
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Appointment()
        {
            AttendeeList = new MailAddressCollection();
            Attachments = new List<Attachment>();
        }
        #endregion

        #region Public Functions

        /// <summary>
        /// Adds an appointment to a user's calendar
        /// </summary>
        public virtual void AddAppointment()
        {
            string XMLNSInfo = "xmlns:g=\"DAV:\" "
                + "xmlns:e=\"http://schemas.microsoft.com/exchange/\" "
                + "xmlns:mapi=\"http://schemas.microsoft.com/mapi/\" "
                + "xmlns:mapit=\"http://schemas.microsoft.com/mapi/proptag/\" "
                + "xmlns:x=\"xml:\" xmlns:cal=\"urn:schemas:calendar:\" "
                + "xmlns:dt=\"urn:uuid:c2f41010-65b3-11d1-a29f-00aa00c14882/\" "
                + "xmlns:header=\"urn:schemas:mailheader:\" "
                + "xmlns:mail=\"urn:schemas:httpmail:\"";

            string CalendarInfo = "<cal:location>" + Location + "</cal:location>"// + Location + "</cal:location>"
                + "<cal:dtstart dt:dt=\"dateTime.tz\">" + StartDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.000Z") + "</cal:dtstart>"// + StartDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "</cal:dtstart>"
                + "<cal:dtend dt:dt=\"dateTime.tz\">" + EndDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.000Z") + "</cal:dtend>"// + EndDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "</cal:dtend>"
                + "<cal:instancetype dt:dt=\"int\">0</cal:instancetype>"
                + "<cal:busystatus>BUSY</cal:busystatus>"
                + "<cal:meetingstatus>CONFIRMED</cal:meetingstatus>"
                + "<cal:alldayevent dt:dt=\"boolean\">0</cal:alldayevent>"
                + "<cal:responserequested dt:dt=\"boolean\">1</cal:responserequested>"
                + "<cal:reminderoffset dt:dt=\"int\">900</cal:reminderoffset>"
                + "<cal:uid>" + MeetingGUID.ToString("B") + "</cal:uid>";

            string HeaderInfo = "<header:to>" + AttendeeList.ToString() + "</header:to>";

            string MailInfo = "<mail:subject>" + Subject + "</mail:subject>"
                + "<mail:htmldescription>" + Summary + "</mail:htmldescription>";

            string AppointmentRequest = "<?xml version=\"1.0\"?>"
                + "<g:propertyupdate " + XMLNSInfo + ">"
                + "<g:set><g:prop>"
                + "<g:contentclass>urn:content-classes:appointment</g:contentclass>"
                + "<e:outlookmessageclass>IPM.Appointment</e:outlookmessageclass>"
                + MailInfo
                + CalendarInfo
                + HeaderInfo
                + "<mapi:finvited dt:dt=\"boolean\">1</mapi:finvited>"
                + "</g:prop></g:set>"
                + "</g:propertyupdate>";

            System.Net.HttpWebRequest PROPPATCHRequest = (System.Net.HttpWebRequest)HttpWebRequest.Create(ServerName + "/exchange/" + Directory + "/Calendar/" + MeetingGUID.ToString() + ".eml");

            System.Net.CredentialCache MyCredentialCache = new System.Net.CredentialCache();
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                MyCredentialCache.Add(new System.Uri(ServerName + "/exchange/" + Directory + "/Calendar/" + MeetingGUID.ToString() + ".eml"),
                                       "NTLM",
                                       new System.Net.NetworkCredential(UserName, Password));
            }
            else
            {
                MyCredentialCache.Add(new System.Uri(ServerName + "/exchange/" + Directory + "/Calendar/" + MeetingGUID.ToString() + ".eml"),
                                       "Negotiate",
                                       (System.Net.NetworkCredential)CredentialCache.DefaultCredentials);
            }

            PROPPATCHRequest.Credentials = MyCredentialCache;
            PROPPATCHRequest.Method = "PROPPATCH";
            byte[] bytes = Encoding.UTF8.GetBytes((string)AppointmentRequest);
            PROPPATCHRequest.ContentLength = bytes.Length;
            using (System.IO.Stream PROPPATCHRequestStream = PROPPATCHRequest.GetRequestStream())
            {
                PROPPATCHRequestStream.Write(bytes, 0, bytes.Length);
                PROPPATCHRequestStream.Close();
                PROPPATCHRequest.ContentType = "text/xml";
                System.Net.WebResponse PROPPATCHResponse = (System.Net.HttpWebResponse)PROPPATCHRequest.GetResponse();
                PROPPATCHResponse.Close();
            }
        }

        /// <summary>
        /// Emails an appointment to the specified users
        /// </summary>
        public virtual void EmailAppointment()
        {
            using (MailMessage Mail = new MailMessage())
            {
                System.Net.Mime.ContentType TextType = new System.Net.Mime.ContentType("text/plain");
                using (AlternateView TextView = AlternateView.CreateAlternateViewFromString(GetText(), TextType))
                {
                    System.Net.Mime.ContentType HTMLType = new System.Net.Mime.ContentType("text/html");
                    using (AlternateView HTMLView = AlternateView.CreateAlternateViewFromString(GetHTML(false), HTMLType))
                    {
                        System.Net.Mime.ContentType CalendarType = new System.Net.Mime.ContentType("text/calendar");
                        CalendarType.Parameters.Add("method", "REQUEST");
                        CalendarType.Parameters.Add("name", "meeting.ics");
                        using (AlternateView CalendarView = AlternateView.CreateAlternateViewFromString(GetCalendar(false), CalendarType))
                        {
                            CalendarView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                            Mail.AlternateViews.Add(TextView);
                            Mail.AlternateViews.Add(HTMLView);
                            Mail.AlternateViews.Add(CalendarView);

                            Mail.From = new MailAddress(OrganizerEmail);

                            foreach (MailAddress attendee in AttendeeList)
                            {
                                Mail.To.Add(attendee);
                            }


                            Mail.Subject = Subject;

                            foreach (Attachment Attachment in Attachments)
                            {
                                Mail.Attachments.Add(Attachment);
                            }


                            SmtpClient Server = new SmtpClient(ServerName, Port);
                            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                            {
                                Server.Credentials = new System.Net.NetworkCredential(UserName, Password);
                            }
                            if (AttendeeList.Count > 0)
                            {
                                Server.Send(Mail);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sends a cancellation to the people specified
        /// </summary>
        public virtual void SendCancelEmails()
        {
            using (MailMessage Mail = new MailMessage())
            {
                System.Net.Mime.ContentType TextType = new System.Net.Mime.ContentType("text/plain");
                using (AlternateView TextView = AlternateView.CreateAlternateViewFromString(GetText(), TextType))
                {
                    System.Net.Mime.ContentType HTMLType = new System.Net.Mime.ContentType("text/html");
                    using (AlternateView HTMLView = AlternateView.CreateAlternateViewFromString(GetHTML(true), HTMLType))
                    {
                        System.Net.Mime.ContentType CalendarType = new System.Net.Mime.ContentType("text/calendar");
                        CalendarType.Parameters.Add("method", "CANCEL");
                        CalendarType.Parameters.Add("name", "meeting.ics");
                        using (AlternateView CalendarView = AlternateView.CreateAlternateViewFromString(GetCalendar(true), CalendarType))
                        {
                            CalendarView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                            Mail.AlternateViews.Add(TextView);
                            Mail.AlternateViews.Add(HTMLView);
                            Mail.AlternateViews.Add(CalendarView);

                            Mail.From = new MailAddress(OrganizerEmail);
                            foreach (MailAddress attendee in AttendeeList)
                            {
                                Mail.To.Add(attendee);
                            }
                            Mail.Subject = Subject + " - Cancelled";
                            foreach (Attachment Attachment in Attachments)
                            {
                                Mail.Attachments.Add(Attachment);
                            }
                            SmtpClient Server = new SmtpClient(ServerName, Port);
                            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                            {
                                Server.Credentials = new System.Net.NetworkCredential(UserName, Password);
                            }
                            if (AttendeeList.Count > 0)
                            {
                                Server.Send(Mail);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cancels an appointment on someone's calendar
        /// </summary>
        public virtual void CancelAppointment()
        {
            System.Net.HttpWebRequest PROPPATCHRequest = (System.Net.HttpWebRequest)HttpWebRequest.Create(ServerName + "/exchange/" + Directory + "/Calendar/" + MeetingGUID.ToString() + ".eml");

            System.Net.CredentialCache MyCredentialCache = new System.Net.CredentialCache();
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                MyCredentialCache.Add(new System.Uri(ServerName + "/exchange/" + Directory + "/Calendar/" + MeetingGUID.ToString() + ".eml"),
                                   "NTLM",
                                   new System.Net.NetworkCredential(UserName, Password));
            }
            else
            {
                MyCredentialCache.Add(new System.Uri(ServerName + "/exchange/" + Directory + "/Calendar/" + MeetingGUID.ToString() + ".eml"),
                                       "Negotiate",
                                       (System.Net.NetworkCredential)CredentialCache.DefaultCredentials);
            }

            PROPPATCHRequest.Credentials = MyCredentialCache;
            PROPPATCHRequest.Method = "DELETE";
            System.Net.WebResponse PROPPATCHResponse = (System.Net.HttpWebResponse)PROPPATCHRequest.GetResponse();
            PROPPATCHResponse.Close();
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Returns the text version of the appointment
        /// </summary>
        /// <returns>A text version of the appointment</returns>
        private string GetText()
        {
            string Body = "Type:Single Meeting\n" +
                "Organizer:" + OrganizerName + "\n" +
                "Start Time:" + StartDate.ToLongDateString() + " " + StartDate.ToLongTimeString() + "\n" +
                "End Time:" + EndDate.ToLongDateString() + " " + EndDate.ToLongTimeString() + "\n" +
                "Time Zone:" + System.TimeZone.CurrentTimeZone.StandardName + "\n" +
                "Location: " + Location + "\n\n" +
                "*~*~*~*~*~*~*~*~*~*\n\n" +
                Summary;
            return Body;
        }

        /// <summary>
        /// Gets an HTML version of the appointment
        /// </summary>
        /// <param name="Canceled">If true it returns a cancellation, false it returns a request</param>
        /// <returns>An HTML version of the appointment</returns>
        private string GetHTML(bool Canceled)
        {
            string bodyHTML = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 3.2//EN\">\r\n<HTML>\r\n<HEAD>\r\n<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=utf-8\">\r\n<META NAME=\"Generator\" CONTENT=\"MS Exchange Server version 6.5.7652.24\">\r\n<TITLE>{0}</TITLE>\r\n</HEAD>\r\n<BODY>\r\n<!-- Converted from text/plain format -->\r\n<P><FONT SIZE=2>Type:Single Meeting<BR>\r\nOrganizer:{1}<BR>\r\nStart Time:{2}<BR>\r\nEnd Time:{3}<BR>\r\nTime Zone:{4}<BR>\r\nLocation:{5}<BR>\r\n<BR>\r\n*~*~*~*~*~*~*~*~*~*<BR>\r\n<BR>\r\n{6}<BR>\r\n</FONT>\r\n</P>\r\n\r\n</BODY>\r\n</HTML>";
            string TempSummary = Summary;
            if (Canceled)
            {
                TempSummary += " - Canceled";
            }
            return string.Format(bodyHTML,
                TempSummary,
                OrganizerName,
                StartDate.ToLongDateString() + " " + StartDate.ToLongTimeString(),
                EndDate.ToLongDateString() + " " + EndDate.ToLongTimeString(),
                System.TimeZone.CurrentTimeZone.StandardName,
                Location,
                TempSummary);
        }

        /// <summary>
        /// Gets an iCalendar version of the appointment
        /// </summary>
        /// <param name="Canceled">If true, it returns a cancellation version.
        /// If false, it returns a request version.</param>
        /// <returns>An iCalendar version of the appointment</returns>
        private string GetCalendar(bool Canceled)
        {
            string DateFormatUsing = "yyyyMMddTHHmmssZ";
            string Method;
            if (!Canceled)
                Method = "REQUEST";
            else
                Method = "CANCEL";
            string bodyCalendar = "BEGIN:VCALENDAR\r\nMETHOD:{10}\r\nPRODID:Microsoft CDO for Microsoft Exchange\r\nVERSION:2.0\r\nBEGIN:VTIMEZONE\r\nTZID:(GMT-06.00) Central Time (US & Canada)\r\nX-MICROSOFT-CDO-TZID:11\r\nBEGIN:STANDARD\r\nDTSTART:16010101T020000\r\nTZOFFSETFROM:-0500\r\nTZOFFSETTO:-0600\r\nRRULE:FREQ=YEARLY;WKST=MO;INTERVAL=1;BYMONTH=11;BYDAY=1SU\r\nEND:STANDARD\r\nBEGIN:DAYLIGHT\r\nDTSTART:16010101T020000\r\nTZOFFSETFROM:-0600\r\nTZOFFSETTO:-0500\r\nRRULE:FREQ=YEARLY;WKST=MO;INTERVAL=1;BYMONTH=3;BYDAY=2SU\r\nEND:DAYLIGHT\r\nEND:VTIMEZONE\r\nBEGIN:VEVENT\r\nDTSTAMP:{8}\r\nDTSTART:{0}\r\nSUMMARY:{7}\r\nUID:{5}\r\nATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE;CN=\"{9}\":MAILTO:{9}\r\nACTION;RSVP=TRUE;CN=\"{4}\":MAILTO:{4}\r\nORGANIZER;CN=\"{3}\":mailto:{4}\r\nLOCATION:{2}\r\nDTEND:{1}\r\nDESCRIPTION:{7}\\N\r\nSEQUENCE:1\r\nPRIORITY:5\r\nCLASS:\r\nCREATED:{8}\r\nLAST-MODIFIED:{8}\r\nSTATUS:CONFIRMED\r\nTRANSP:OPAQUE\r\nX-MICROSOFT-CDO-BUSYSTATUS:BUSY\r\nX-MICROSOFT-CDO-INSTTYPE:0\r\nX-MICROSOFT-CDO-INTENDEDSTATUS:BUSY\r\nX-MICROSOFT-CDO-ALLDAYEVENT:FALSE\r\nX-MICROSOFT-CDO-IMPORTANCE:1\r\nX-MICROSOFT-CDO-OWNERAPPTID:-1\r\nX-MICROSOFT-CDO-ATTENDEE-CRITICAL-CHANGE:{8}\r\nX-MICROSOFT-CDO-OWNER-CRITICAL-CHANGE:{8}\r\nBEGIN:VALARM\r\nACTION:DISPLAY\r\nDESCRIPTION:REMINDER\r\nTRIGGER;RELATED=START:-PT00H15M00S\r\nEND:VALARM\r\nEND:VEVENT\r\nEND:VCALENDAR\r\n";
            bodyCalendar = string.Format(bodyCalendar,
                StartDate.ToUniversalTime().ToString(DateFormatUsing),
                EndDate.ToUniversalTime().ToString(DateFormatUsing),
                Location,
                OrganizerName,
                OrganizerEmail,
                MeetingGUID.ToString("B"),
                Summary,
                Subject,
                DateTime.Now.ToUniversalTime().ToString(DateFormatUsing),
                AttendeeList.ToString(),
                Method);
            return bodyCalendar;
        }

        #endregion

        #region Variables
        public DateTime StartDate;
        public DateTime EndDate;
        public string Subject;
        public string Summary;
        public string Location;
        public MailAddressCollection AttendeeList;
        public string OrganizerName;
        public string OrganizerEmail;

        public List<Attachment> Attachments;

        public Guid MeetingGUID;

        public string Directory;
        public string ServerName;
        public string UserName;
        public string Password;
        public int Port;

        #endregion
    }
}