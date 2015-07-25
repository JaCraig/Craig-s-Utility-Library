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
        /// <param name="List">List of TimeSpans</param>
        /// <returns>The average value</returns>
        public static TimeSpan Average(this IEnumerable<TimeSpan> List)
        {
            List = List.Check(new List<TimeSpan>());
            return List.Any() ? new TimeSpan((long)List.Average(x => x.Ticks)) : new TimeSpan(0);
        }

        /// <summary>
        /// Days in the TimeSpan minus the months and years
        /// </summary>
        /// <param name="Span">TimeSpan to get the days from</param>
        /// <returns>The number of days minus the months and years that the TimeSpan has</returns>
        public static int DaysRemainder(this TimeSpan Span)
        {
            return (DateTime.MinValue + Span).Day - 1;
        }

        /// <summary>
        /// Months in the TimeSpan
        /// </summary>
        /// <param name="Span">TimeSpan to get the months from</param>
        /// <returns>The number of months that the TimeSpan has</returns>
        public static int Months(this TimeSpan Span)
        {
            return (DateTime.MinValue + Span).Month - 1;
        }

        /// <summary>
        /// Converts the input to a string in this format: (Years) years, (Months) months,
        /// (DaysRemainder) days, (Hours) hours, (Minutes) minutes, (Seconds) seconds
        /// </summary>
        /// <param name="Input">Input TimeSpan</param>
        /// <returns>The TimeSpan as a string</returns>
        public static string ToStringFull(this TimeSpan Input)
        {
            string Result = "";
            string Splitter = "";
            if (Input.Years() > 0) { Result += Input.Years() + " year" + (Input.Years() > 1 ? "s" : ""); Splitter = ", "; }
            if (Input.Months() > 0) { Result += Splitter + Input.Months() + " month" + (Input.Months() > 1 ? "s" : ""); Splitter = ", "; }
            if (Input.DaysRemainder() > 0) { Result += Splitter + Input.DaysRemainder() + " day" + (Input.DaysRemainder() > 1 ? "s" : ""); Splitter = ", "; }
            if (Input.Hours > 0) { Result += Splitter + Input.Hours + " hour" + (Input.Hours > 1 ? "s" : ""); Splitter = ", "; }
            if (Input.Minutes > 0) { Result += Splitter + Input.Minutes + " minute" + (Input.Minutes > 1 ? "s" : ""); Splitter = ", "; }
            if (Input.Seconds > 0) { Result += Splitter + Input.Seconds + " second" + (Input.Seconds > 1 ? "s" : ""); Splitter = ", "; }
            return Result;
        }

        /// <summary>
        /// Years in the TimeSpan
        /// </summary>
        /// <param name="Span">TimeSpan to get the years from</param>
        /// <returns>The number of years that the TimeSpan has</returns>
        public static int Years(this TimeSpan Span)
        {
            return (DateTime.MinValue + Span).Year - 1;
        }
    }
}