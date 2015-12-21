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

namespace Utilities.DataTypes
{
    /// <summary>
    /// Represents a date span
    /// </summary>
    public class DateSpan
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">Start of the date span</param>
        /// <param name="end">End of the date span</param>
        public DateSpan(DateTime start, DateTime end)
        {
            if (start > end)
            {
                var Temp = start;
                start = end;
                end = Temp;
            }
            Start = start;
            End = end;
        }

        /// <summary>
        /// Days between the two dates
        /// </summary>
        public int Days => (End - Start).DaysRemainder();

        /// <summary>
        /// End date
        /// </summary>
        public DateTime End { get; protected set; }

        /// <summary>
        /// Hours between the two dates
        /// </summary>
        public int Hours => (End - Start).Hours;

        /// <summary>
        /// Milliseconds between the two dates
        /// </summary>
        public int MilliSeconds => (End - Start).Milliseconds;

        /// <summary>
        /// Minutes between the two dates
        /// </summary>
        public int Minutes => (End - Start).Minutes;

        /// <summary>
        /// Months between the two dates
        /// </summary>
        public int Months => (End - Start).Months();

        /// <summary>
        /// Seconds between the two dates
        /// </summary>
        public int Seconds => (End - Start).Seconds;

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime Start { get; protected set; }

        /// <summary>
        /// Years between the two dates
        /// </summary>
        public int Years => (End - Start).Years();

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>The value as a string</returns>
        public static implicit operator string(DateSpan value)
        {
            if (value == null)
                return "";
            return value.ToString();
        }

        /// <summary>
        /// Determines if two DateSpans are not equal
        /// </summary>
        /// <param name="span1">Span 1</param>
        /// <param name="span2">Span 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(DateSpan span1, DateSpan span2)
        {
            return !(span1 == span2);
        }

        /// <summary>
        /// Addition operator
        /// </summary>
        /// <param name="span1">Span 1</param>
        /// <param name="span2">Span 2</param>
        /// <returns>The combined date span</returns>
        public static DateSpan operator +(DateSpan span1, DateSpan span2)
        {
            if (span1 == null && span2 == null)
                return null;
            if (span1 == null)
                return new DateSpan(span2.Start, span2.End);
            if (span2 == null)
                return new DateSpan(span1.Start, span1.End);
            DateTime Start = span1.Start < span2.Start ? span1.Start : span2.Start;
            DateTime End = span1.End > span2.End ? span1.End : span2.End;
            return new DateSpan(Start, End);
        }

        /// <summary>
        /// Determines if two DateSpans are equal
        /// </summary>
        /// <param name="span1">Span 1</param>
        /// <param name="span2">Span 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(DateSpan span1, DateSpan span2)
        {
            if ((object)span1 == null && (object)span2 == null)
                return true;
            if ((object)span1 == null || (object)span2 == null)
                return false;
            return span1.Start == span2.Start && span1.End == span2.End;
        }

        /// <summary>
        /// Determines if two objects are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if they are, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var Tempobj = obj as DateSpan;
            return Tempobj != null && Tempobj == this;
        }

        /// <summary>
        /// Gets the hash code for the date span
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return End.GetHashCode() & Start.GetHashCode();
        }

        /// <summary>
        /// Returns the intersecting time span between the two values
        /// </summary>
        /// <param name="span">Span to use</param>
        /// <returns>The intersection of the two time spans</returns>
        public DateSpan Intersection(DateSpan span)
        {
            if (span == null)
                return null;
            if (!Overlap(span))
                return null;
            DateTime Start = span.Start > this.Start ? span.Start : this.Start;
            DateTime End = span.End < this.End ? span.End : this.End;
            return new DateSpan(Start, End);
        }

        /// <summary>
        /// Determines if two DateSpans overlap
        /// </summary>
        /// <param name="span">The span to compare to</param>
        /// <returns>True if they overlap, false otherwise</returns>
        public bool Overlap(DateSpan span)
        {
            if (span == null)
                return false;
            return ((Start >= span.Start && Start < span.End) || (End <= span.End && End > span.Start) || (Start <= span.Start && End >= span.End));
        }

        /// <summary>
        /// Converts the DateSpan to a string
        /// </summary>
        /// <returns>The DateSpan as a string</returns>
        public override string ToString()
        {
            return "Start: " + Start.ToString() + " End: " + End.ToString();
        }
    }
}