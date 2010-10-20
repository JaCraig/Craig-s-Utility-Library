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
using System.Text;
using System.Text.RegularExpressions;
#endregion

namespace Utilities.FileFormats
{
    /// <summary>
    /// Creates a VCalendar item
    /// </summary>
    public class VCalendar
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public VCalendar()
        {
        }

        #endregion

        #region Properties
        /// <summary>
        /// The time zone adjustment for the calendar event
        /// </summary>
        public int TimeZoneAdjustment { get; set; }

        /// <summary>
        /// The start time
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The end time
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// The location of the event
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The subject of the item to send
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The description of the event
        /// </summary>
        public string Description { get; set; }

        private static readonly Regex STRIP_HTML_REGEX = new Regex("<[^>]*>", RegexOptions.Compiled);

        #endregion

        #region Private Functions

        private string StripHTML(string HTML)
        {
            if (string.IsNullOrEmpty(HTML))
                return string.Empty;

            HTML = STRIP_HTML_REGEX.Replace(HTML, string.Empty);
            HTML = HTML.Replace("&nbsp;", " ");
            return HTML.Replace("&#160;", string.Empty);
        }

        private bool ContainsHTML(string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return false;

            return STRIP_HTML_REGEX.IsMatch(Input);
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Returns the VCalendar item
        /// </summary>
        /// <returns>a string output of the VCalendar item</returns>
        public string GetVCalendar()
        {
            StringBuilder FileOutput = new StringBuilder();
            FileOutput.AppendLine("BEGIN:VCALENDAR");
            FileOutput.AppendLine("VERSION:1.0");
            FileOutput.AppendLine("BEGIN:VEVENT");
            StartTime = StartTime.AddHours(-TimeZoneAdjustment);
            EndTime = EndTime.AddHours(-TimeZoneAdjustment);

            string Start = StartTime.ToString("yyyyMMdd") + "T" + StartTime.ToString("HHmmss");
            string End = EndTime.ToString("yyyyMMdd") + "T" + EndTime.ToString("HHmmss");
            FileOutput.Append("DTStart:").AppendLine(Start);
            FileOutput.Append("DTEnd:").AppendLine(End);
            FileOutput.Append("Location;ENCODING=QUOTED-PRINTABLE:").AppendLine(Location);
            FileOutput.Append("SUMMARY;ENCODING=QUOTED-PRINTABLE:").AppendLine(Subject);
            FileOutput.Append("DESCRIPTION;ENCODING=QUOTED-PRINTABLE:").AppendLine(Description);
            FileOutput.Append("UID:").Append(Start).Append(End).AppendLine(Subject);
            FileOutput.AppendLine("PRIORITY:3");
            FileOutput.AppendLine("End:VEVENT");
            FileOutput.AppendLine("End:VCALENDAR");
            return FileOutput.ToString();
        }

        /// <summary>
        /// Returns the ICalendar item
        /// </summary>
        /// <returns>a string output of the ICalendar item</returns>
        public string GetICalendar()
        {
            StringBuilder FileOutput = new StringBuilder();
            FileOutput.AppendLine("BEGIN:VCALENDAR");
            FileOutput.AppendLine("PRODID:-//Craigs Utility Library//EN");
            FileOutput.AppendLine("VERSION:2.0");
            FileOutput.AppendLine("METHOD:PUBLISH");
            StartTime = StartTime.AddHours(-TimeZoneAdjustment);
            EndTime = EndTime.AddHours(-TimeZoneAdjustment);
            string Start = StartTime.ToString("yyyyMMdd") + "T" + StartTime.ToString("HHmmss");
            string End = EndTime.ToString("yyyyMMdd") + "T" + EndTime.ToString("HHmmss");
            FileOutput.AppendLine("BEGIN:VEVENT");
            FileOutput.AppendLine("CLASS:PUBLIC");
            FileOutput.Append("CREATED:").AppendLine(DateTime.Now.ToString("yyyyMMddTHHmmssZ"));
            FileOutput.AppendLine(StripHTML(Description.Replace("<br />", "\\n")));
            FileOutput.Append("DTEND:").AppendLine(Start);
            FileOutput.Append("DTSTART:").AppendLine(End);
            FileOutput.Append("LOCATION:").AppendLine(Location);
            FileOutput.Append("SUMMARY;LANGUAGE=en-us:").AppendLine(Subject);
            FileOutput.Append("UID:").Append(Start).Append(End).AppendLine(Subject);
            if (ContainsHTML(Description))
            {
                FileOutput.Append("X-ALT-DESC;FMTTYPE=text/html:").AppendLine(Description.Replace("\n", ""));
            }
            else
            {
                FileOutput.Append("DESCRIPTION:").AppendLine(Description);
            }
            FileOutput.AppendLine("BEGIN:VALARM");
            FileOutput.AppendLine("TRIGGER:-PT15M");
            FileOutput.AppendLine("ACTION:DISPLAY");
            FileOutput.AppendLine("DESCRIPTION:Reminder");
            FileOutput.AppendLine("END:VALARM");
            FileOutput.AppendLine("END:VEVENT");
            FileOutput.AppendLine("END:VCALENDAR");
            return FileOutput.ToString();
        }

        /// <summary>
        /// Returns the HCalendar item
        /// </summary>
        /// <returns>A string output of the HCalendar item</returns>
        public string GetHCalendar()
        {
            StringBuilder Output = new StringBuilder();
            Output.Append("<div class=\"vevent\">")
                .Append("<div class=\"summary\">").Append(Subject).Append("</div>")
                .Append("<div>Date: <abbr class=\"dtstart\" title=\"")
                .Append(StartTime.ToString("MM-dd-yyyy hh:mm tt")).Append("\">")
                .Append(StartTime.ToString("MMMM dd, yyyy hh:mm tt")).Append("</abbr> to ")
                .Append("<abbr class=\"dtend\" title=\"").Append(EndTime.ToString("MM-dd-yyyy hh:mm tt"))
                .Append("\">");
            if (EndTime.Year != StartTime.Year)
            {
                Output.Append(EndTime.ToString("MMMM dd, yyyy hh:mm tt"));
            }
            else if (EndTime.Month != StartTime.Month)
            {
                Output.Append(EndTime.ToString("MMMM dd hh:mm tt"));
            }
            else if (EndTime.Day != StartTime.Day)
            {
                Output.Append(EndTime.ToString("dd hh:mm tt"));
            }
            else
            {
                Output.Append(EndTime.ToString("hh:mm tt"));
            }
            Output.Append("</abbr></div>");
            Output.Append("<div>Location: <span class=\"location\">").Append(Location).Append("</span></div>");
            Output.Append("<div class=\"description\">").Append(Description).Append("</div>");
            Output.Append("</div>");
            return Output.ToString();
        }

        #endregion
    }
}