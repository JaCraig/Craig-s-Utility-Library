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
        public int TimeZoneAdjustment{get;set;}
        
        /// <summary>
        /// The start time
        /// </summary>
        public DateTime StartTime{get;set;}

        /// <summary>
        /// The end time
        /// </summary>
        public DateTime EndTime{get;set;}

        /// <summary>
        /// The location of the event
        /// </summary>
        public string Location{get;set;}

        /// <summary>
        /// The subject of the item to send
        /// </summary>
        public string Subject{get;set;}

        /// <summary>
        /// The description of the event
        /// </summary>
        public string Description{get;set;}

        #endregion

        #region Public Functions

        /// <summary>
        /// Returns the VCalendar item
        /// </summary>
        /// <returns>a string output of the VCalendar item</returns>
        public string GetVCalendar()
        {
            try
            {
                StringBuilder FileOutput = new StringBuilder();
                FileOutput.AppendLine("BEGIN:VCALENDAR\n");
                FileOutput.AppendLine("VERSION:1.0\n");
                FileOutput.AppendLine("BEGIN:VEVENT\n");
                StartTime = StartTime.AddHours(-TimeZoneAdjustment);
                EndTime = EndTime.AddHours(-TimeZoneAdjustment);

                string Start = StartTime.ToString("yyyyMMdd") + "T" + StartTime.ToString("HHmmss");
                string End = EndTime.ToString("yyyyMMdd") + "T" + EndTime.ToString("HHmmss");
                FileOutput.AppendLine("DTStart:" + Start + "\n");
                FileOutput.AppendLine("DTEnd:" + End + "\n");
                FileOutput.AppendLine("Location;ENCODING=QUOTED-PRINTABLE:" + Location + "\n");
                FileOutput.AppendLine("SUMMARY;ENCODING=QUOTED-PRINTABLE:" + Subject + "\n");
                FileOutput.AppendLine("DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + Description + "\n");
                FileOutput.AppendLine("UID:" + Start + End + Subject + "\n");
                FileOutput.AppendLine("PRIORITY:3\n");
                FileOutput.AppendLine("End:VEVENT\n");
                FileOutput.AppendLine("End:VCALENDAR\n");
                return FileOutput.ToString();
            }
            catch { throw; }
        }

        /// <summary>
        /// Returns the HCalendar item
        /// </summary>
        /// <returns>A string output of the HCalendar item</returns>
        public string GetHCalendar()
        {
            try
            {
                StringBuilder Output = new StringBuilder();
                Output.Append("<div class=\"vevent\">");
                Output.Append("<div class=\"summary\">" + Subject + "</div>");
                Output.Append("<div>Date: <abbr class=\"dtstart\" title=\"" + StartTime.ToString("MM-dd-yyyy hh:mm tt") + "\">" + StartTime.ToString("MMMM dd, yyyy hh:mm tt") + "</abbr> to ");
                Output.Append("<abbr class=\"dtend\" title=\"" + EndTime.ToString("MM-dd-yyyy hh:mm tt") + "\">");
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
                Output.Append("<div>Location: <span class=\"location\">" + Location + "</span></div>");
                Output.Append("<div class=\"description\">" + Description + "</div>");
                Output.Append("</div>");
                return Output.ToString();
            }
            catch { throw; }
        }

        #endregion
    }
}