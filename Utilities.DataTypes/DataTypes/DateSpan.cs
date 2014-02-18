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

#region Usings

using System;
using System.Diagnostics.Contracts;

#endregion Usings

namespace Utilities.DataTypes
{
    /// <summary>
    /// Represents a date span
    /// </summary>
    public class DateSpan
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Start">Start of the date span</param>
        /// <param name="End">End of the date span</param>
        public DateSpan(DateTime Start, DateTime End)
        {
            Contract.Requires<ArgumentException>(Start <= End, "Start is after End");
            this.Start = Start;
            this.End = End;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Days between the two dates
        /// </summary>
        public virtual int Days { get { return (End - Start).DaysRemainder(); } }

        /// <summary>
        /// End date
        /// </summary>
        public virtual DateTime End { get; protected set; }

        /// <summary>
        /// Hours between the two dates
        /// </summary>
        public virtual int Hours { get { return (End - Start).Hours; } }

        /// <summary>
        /// Milliseconds between the two dates
        /// </summary>
        public virtual int MilliSeconds { get { return (End - Start).Milliseconds; } }

        /// <summary>
        /// Minutes between the two dates
        /// </summary>
        public virtual int Minutes { get { return (End - Start).Minutes; } }

        /// <summary>
        /// Months between the two dates
        /// </summary>
        public virtual int Months { get { return (End - Start).Months(); } }

        /// <summary>
        /// Seconds between the two dates
        /// </summary>
        public virtual int Seconds { get { return (End - Start).Seconds; } }

        /// <summary>
        /// Start date
        /// </summary>
        public virtual DateTime Start { get; protected set; }

        /// <summary>
        /// Years between the two dates
        /// </summary>
        public virtual int Years { get { return (End - Start).Years(); } }

        #endregion Properties

        #region Functions

        /// <summary>
        /// Returns the intersecting time span between the two values
        /// </summary>
        /// <param name="Span">Span to use</param>
        /// <returns>The intersection of the two time spans</returns>
        public DateSpan Intersection(DateSpan Span)
        {
            if (Span == null)
                return null;
            if (!Overlap(Span))
                return null;
            DateTime Start = Span.Start > this.Start ? Span.Start : this.Start;
            DateTime End = Span.End < this.End ? Span.End : this.End;
            return new DateSpan(Start, End);
        }

        /// <summary>
        /// Determines if two DateSpans overlap
        /// </summary>
        /// <param name="Span">The span to compare to</param>
        /// <returns>True if they overlap, false otherwise</returns>
        public bool Overlap(DateSpan Span)
        {
            Contract.Requires<ArgumentNullException>(Span != null, "Span");
            return ((Start >= Span.Start && Start < Span.End) || (End <= Span.End && End > Span.Start) || (Start <= Span.Start && End >= Span.End));
        }

        #endregion Functions

        #region Operators

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <param name="Value">Value to convert</param>
        /// <returns>The value as a string</returns>
        public static implicit operator string(DateSpan Value)
        {
            Contract.Requires<ArgumentNullException>(Value != null, "Value");
            return Value.ToString();
        }

        /// <summary>
        /// Determines if two DateSpans are not equal
        /// </summary>
        /// <param name="Span1">Span 1</param>
        /// <param name="Span2">Span 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(DateSpan Span1, DateSpan Span2)
        {
            return !(Span1 == Span2);
        }

        /// <summary>
        /// Addition operator
        /// </summary>
        /// <param name="Span1">Span 1</param>
        /// <param name="Span2">Span 2</param>
        /// <returns>The combined date span</returns>
        public static DateSpan operator +(DateSpan Span1, DateSpan Span2)
        {
            if (Span1 == null && Span2 == null)
                return null;
            if (Span1 == null)
                return new DateSpan(Span2.Start, Span2.End);
            if (Span2 == null)
                return new DateSpan(Span1.Start, Span1.End);
            DateTime Start = Span1.Start < Span2.Start ? Span1.Start : Span2.Start;
            DateTime End = Span1.End > Span2.End ? Span1.End : Span2.End;
            return new DateSpan(Start, End);
        }

        /// <summary>
        /// Determines if two DateSpans are equal
        /// </summary>
        /// <param name="Span1">Span 1</param>
        /// <param name="Span2">Span 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(DateSpan Span1, DateSpan Span2)
        {
            if ((object)Span1 == null && (object)Span2 == null)
                return true;
            if ((object)Span1 == null || (object)Span2 == null)
                return false;
            return Span1.Start == Span2.Start && Span1.End == Span2.End;
        }

        #endregion Operators

        #region Overridden Functions

        /// <summary>
        /// Determines if two objects are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if they are, false otherwise</returns>
        public override bool Equals(object obj)
        {
            DateSpan Tempobj = obj as DateSpan;
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
        /// Converts the DateSpan to a string
        /// </summary>
        /// <returns>The DateSpan as a string</returns>
        public override string ToString()
        {
            return "Start: " + Start.ToString() + " End: " + End.ToString();
        }

        #endregion Overridden Functions
    }
}