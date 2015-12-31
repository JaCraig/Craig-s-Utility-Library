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
using System.ComponentModel;
using System.Linq;

namespace Utilities.DataTypes
{
    /// <summary>
    /// TimeSpan extension methods
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Averages a list of TimeSpans
        /// </summary>
        /// <param name="list">List of TimeSpans</param>
        /// <returns>The average value</returns>
        public static TimeSpan Average(this IEnumerable<TimeSpan> list)
        {
            list = list ?? new List<TimeSpan>();
            return list.Any() ? new TimeSpan((long)list.Average(x => x.Ticks)) : new TimeSpan(0);
        }

        /// <summary>
        /// Days in the TimeSpan minus the months and years
        /// </summary>
        /// <param name="span">TimeSpan to get the days from</param>
        /// <returns>The number of days minus the months and years that the TimeSpan has</returns>
        public static int DaysRemainder(this TimeSpan span)
        {
            return (DateTime.MinValue + span).Day - 1;
        }

        /// <summary>
        /// Months in the TimeSpan
        /// </summary>
        /// <param name="span">TimeSpan to get the months from</param>
        /// <returns>The number of months that the TimeSpan has</returns>
        public static int Months(this TimeSpan span)
        {
            return (DateTime.MinValue + span).Month - 1;
        }

        /// <summary>
        /// Converts the input to a string in this format: (Years) years, (Months) months,
        /// (DaysRemainder) days, (Hours) hours, (Minutes) minutes, (Seconds) seconds
        /// </summary>
        /// <param name="input">input TimeSpan</param>
        /// <returns>The TimeSpan as a string</returns>
        public static string ToStringFull(this TimeSpan input)
        {
            string Result = "";
            string Splitter = "";
            if (input.Years() > 0) { Result += input.Years() + " year" + (input.Years() > 1 ? "s" : ""); Splitter = ", "; }
            if (input.Months() > 0) { Result += Splitter + input.Months() + " month" + (input.Months() > 1 ? "s" : ""); Splitter = ", "; }
            if (input.DaysRemainder() > 0) { Result += Splitter + input.DaysRemainder() + " day" + (input.DaysRemainder() > 1 ? "s" : ""); Splitter = ", "; }
            if (input.Hours > 0) { Result += Splitter + input.Hours + " hour" + (input.Hours > 1 ? "s" : ""); Splitter = ", "; }
            if (input.Minutes > 0) { Result += Splitter + input.Minutes + " minute" + (input.Minutes > 1 ? "s" : ""); Splitter = ", "; }
            if (input.Seconds > 0) { Result += Splitter + input.Seconds + " second" + (input.Seconds > 1 ? "s" : ""); Splitter = ", "; }
            return Result;
        }

        /// <summary>
        /// Years in the TimeSpan
        /// </summary>
        /// <param name="span">TimeSpan to get the years from</param>
        /// <returns>The number of years that the TimeSpan has</returns>
        public static int Years(this TimeSpan span)
        {
            return (DateTime.MinValue + span).Year - 1;
        }
    }
}