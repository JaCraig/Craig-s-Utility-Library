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
using System.Threading;

#endregion

namespace Utilities.DataTypes
{
    /// <summary>
    /// Date related functions
    /// </summary>
    public static class DateHelper
    {
        #region Static Public Functions

        /// <summary>
        /// Determines if two date periods overlap
        /// </summary>
        /// <param name="Start1">Start of period 1</param>
        /// <param name="End1">End of period 1</param>
        /// <param name="Start2">Start of period 2</param>
        /// <param name="End2">End of period 2</param>
        /// <returns>True if they overlap, false otherwise</returns>
        public static bool DatePeriodsOverlap(DateTime Start1, DateTime End1, DateTime Start2, DateTime End2)
        {
            return ((Start1 >= Start2 && Start1 < End2) || (End1 <= End2 && End1 > Start2) || (Start1 <= Start2 && End1 >= End2));
        }

        /// <summary>
        /// Determines if the date is some time in the future
        /// </summary>
        /// <param name="Date">Date to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsInFuture(DateTime Date)
        {
            if (Date == null)
                throw new ArgumentNullException();
            return DateTime.Now < Date;
        }

        /// <summary>
        /// Determines if the date is some time in the past
        /// </summary>
        /// <param name="Date">Date to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsInPast(DateTime Date)
        {
            if (Date == null)
                throw new ArgumentNullException();
            return DateTime.Now > Date;
        }

        /// <summary>
        /// Gets the number of days left in the month based on the date passed in
        /// </summary>
        /// <param name="Date">The date to check against</param>
        /// <returns>The number of days left in a month</returns>
        public static int DaysLeftInMonth(DateTime Date)
        {
            return Thread.CurrentThread.CurrentCulture.Calendar.GetDaysInMonth(Date.Year, Date.Month) - Date.Day;
        }

        /// <summary>
        /// Gets the number of days left in a year based on the date passed in
        /// </summary>
        /// <param name="Date">The date to check against</param>
        /// <returns>The number of days left in a year</returns>
        public static int DaysLeftInYear(DateTime Date)
        {
            return Thread.CurrentThread.CurrentCulture.Calendar.GetDaysInYear(Date.Year) - Date.DayOfYear;
        }

        /// <summary>
        /// Gets the number of days left in a week
        /// </summary>
        /// <param name="Date">The date to check against</param>
        /// <returns>The number of days left in a week</returns>
        public static int DaysLeftInWeek(DateTime Date)
        {
            return 7 - DataTypeConversion.DayToInt(Date.DayOfWeek.ToString());
        }

        /// <summary>
        /// Determines if this is a week day
        /// </summary>
        /// <param name="Date">Date to check against</param>
        /// <returns>Whether this is a week day or not</returns>
        public static bool IsWeekDay(DateTime Date)
        {
            if (DataTypeConversion.DayToInt(Date.DayOfWeek.ToString()) < 6)
                return true;
            return false;
        }

        /// <summary>
        /// Determines if this is a week end
        /// </summary>
        /// <param name="Date">Date to check against</param>
        /// <returns>Whether this is a week end or not</returns>
        public static bool IsWeekEnd(DateTime Date)
        {
            return !IsWeekDay(Date);
        }

        #endregion
    }
}