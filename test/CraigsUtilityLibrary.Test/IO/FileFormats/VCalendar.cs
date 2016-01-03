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
using System.Net.Mail;
using UnitTests.Fixtures;
using Xunit;

namespace UnitTests.IO.FileFormats
{
    public class VCalendar : TestingDirectoryFixture
    {
        [Fact]
        public void BasicTest()
        {
            var Calendar = new Utilities.IO.FileFormats.VCalendar();
            Calendar.AttendeeList.Add(new MailAddress("test@test.com", "Test Test"));
            Calendar.AttendeeList.Add(new MailAddress("test2@test.com", "Test2 Test2"));
            Calendar.Description = "Test vcal item";
            Calendar.EndTime = DateTime.Today.AddHours(7).AddDays(10);
            Calendar.Location = "That spot";
            Calendar.Organizer = new MailAddress("test3@test.com", "Test3 Test3");
            Calendar.StartTime = DateTime.Today.AddHours(5).AddDays(10);
            Calendar.Status = "BUSY";
            Calendar.Subject = "This is a test";
            Assert.Equal("BEGIN:VCALENDAR\r\nMETHOD:REQUEST\r\nPRODID:-//Craigs Utility Library//EN\r\nVERSION:2.0\r\nBEGIN:VEVENT\r\nCLASS:PUBLIC\r\nDTSTAMP:" + DateTime.Now.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "\r\nCREATED:" + DateTime.Now.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "\r\nTest vcal item\r\nDTStart:" + DateTime.Today.AddHours(5).AddDays(10).ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "\r\nDTEnd:" + DateTime.Today.AddHours(7).AddDays(10).ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "\r\nLOCATION:That spot\r\nSUMMARY;LANGUAGE=en-us:This is a test\r\nUID:" + DateTime.Today.AddHours(5).AddDays(10).ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + DateTime.Today.AddHours(7).AddDays(10).ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "This is a test\r\nATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE;CN=\"\"Test Test\" <test@test.com>, \"Test2 Test2\" <test2@test.com>\":MAILTO:\"Test Test\" <test@test.com>, \"Test2 Test2\" <test2@test.com>\r\nACTION;RSVP=TRUE;CN=\"test3@test.com\":MAILTO:test3@test.com\r\nORGANIZER;CN=\"Test3 Test3\":mailto:test3@test.com\r\nDESCRIPTION:Test vcal item\r\nSEQUENCE:1\r\nPRIORITY:5\r\nCLASS:\r\nLAST-MODIFIED:" + DateTime.Now.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "\r\nSTATUS:CONFIRMED\r\nTRANSP:OPAQUE\r\nX-MICROSOFT-CDO-BUSYSTATUS:BUSY\r\nX-MICROSOFT-CDO-INSTTYPE:0\r\nX-MICROSOFT-CDO-INTENDEDSTATUS:BUSY\r\nX-MICROSOFT-CDO-ALLDAYEVENT:FALSE\r\nX-MICROSOFT-CDO-IMPORTANCE:1\r\nX-MICROSOFT-CDO-OWNERAPPTID:-1\r\nX-MICROSOFT-CDO-ATTENDEE-CRITICAL-CHANGE:" + DateTime.Now.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "\r\nX-MICROSOFT-CDO-OWNER-CRITICAL-CHANGE:" + DateTime.Now.ToUniversalTime().ToString("yyyyMMddTHHmmssZ") + "\r\nBEGIN:VALARM\r\nTRIGGER;RELATED=START:-PT00H15M00S\r\nACTION:DISPLAY\r\nDESCRIPTION:Reminder\r\nEND:VALARM\r\nEND:VEVENT\r\nEND:VCALENDAR\r\n", Calendar.GetICalendar());
            Assert.Equal("<div class=\"vevent\"><div class=\"summary\">This is a test</div><div>Date: <abbr class=\"dtstart\" title=\"" + DateTime.Today.AddHours(5).AddDays(10).ToString("MM-dd-yyyy hh:mm tt") + "\">" + DateTime.Today.AddHours(5).AddDays(10).ToString("MMMM dd, yyyy hh:mm tt") + "</abbr> to <abbr class=\"dtend\" title=\"" + DateTime.Today.AddHours(7).AddDays(10).ToString("MM-dd-yyyy hh:mm tt") + "\">" + DateTime.Today.AddHours(7).AddDays(10).ToString("hh:mm tt") + "</abbr></div><div>Location: <span class=\"location\">That spot</span></div><div class=\"description\">Test vcal item</div></div>", Calendar.GetHCalendar());
            Assert.Equal("BEGIN:VCALENDAR\r\nVERSION:1.0\r\nBEGIN:VEVENT\r\nDTStart:" + DateTime.Today.AddHours(5).AddDays(10).ToUniversalTime().ToString("yyyyMMddTHHmmss") + "\r\nDTEnd:" + DateTime.Today.AddHours(7).AddDays(10).ToUniversalTime().ToString("yyyyMMddTHHmmss") + "\r\nLocation;ENCODING=QUOTED-PRINTABLE:That spot\r\nSUMMARY;ENCODING=QUOTED-PRINTABLE:This is a test\r\nDESCRIPTION;ENCODING=QUOTED-PRINTABLE:Test vcal item\r\nUID:" + DateTime.Today.AddHours(5).AddDays(10).ToUniversalTime().ToString("yyyyMMddTHHmmss") + DateTime.Today.AddHours(7).AddDays(10).ToUniversalTime().ToString("yyyyMMddTHHmmss") + "This is a test\r\nPRIORITY:3\r\nEnd:VEVENT\r\nEnd:VCALENDAR\r\n", Calendar.GetVCalendar());
            Assert.Equal("Type:Single Meeting\r\nOrganizer:Test3 Test3\r\nStart Time:" + DateTime.Today.AddHours(5).AddDays(10).ToLongDateString() + " " + DateTime.Today.AddHours(5).AddDays(10).ToLongTimeString() + "\r\nEnd Time:" + DateTime.Today.AddHours(7).AddDays(10).ToLongDateString() + " " + DateTime.Today.AddHours(7).AddDays(10).ToLongTimeString() + "\r\nTime Zone:Eastern Standard Time\r\nLocation: That spot\r\n\r\n*~*~*~*~*~*~*~*~*~*\r\n\r\nTest vcal item", Calendar.GetText());
            Calendar.Save("./Testing/Item.ics");
            Assert.Equal("Type:Single Meeting\r\nOrganizer:\r\nStart Time:" + DateTime.Today.AddHours(5).AddDays(10).ToLongDateString() + " " + DateTime.Today.AddHours(5).AddDays(10).ToLongTimeString() + "\r\nEnd Time:" + DateTime.Today.AddHours(7).AddDays(10).ToLongDateString() + " " + DateTime.Today.AddHours(7).AddDays(10).ToLongTimeString() + "\r\nTime Zone:Eastern Standard Time\r\nLocation: That spot\r\n\r\n*~*~*~*~*~*~*~*~*~*\r\n\r\nTest vcal item", Utilities.IO.FileFormats.VCalendar.Load("./Testing/Item.ics").GetText());
        }
    }
}