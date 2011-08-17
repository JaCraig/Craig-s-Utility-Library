/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Data;
using System.Data.SqlClient;
using System.Threading;
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// DateTime extension methods
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Extension Methods

        #region IsInFuture

        /// <summary>
        /// Determines if the date is some time in the future
        /// </summary>
        /// <param name="Date">Date to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsInFuture(this DateTime Date)
        {
            if (Date == null)
                throw new ArgumentNullException("Date");
            return DateTime.Now < Date;
        }

        #endregion

        #region IsInPast

        /// <summary>
        /// Determines if the date is some time in the past
        /// </summary>
        /// <param name="Date">Date to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsInPast(this DateTime Date)
        {
            if (Date == null)
                throw new ArgumentNullException("Date");
            return DateTime.Now > Date;
        }

        #endregion

        #region DaysLeftInMonth

        /// <summary>
        /// Gets the number of days left in the month based on the date passed in
        /// </summary>
        /// <param name="Date">The date to check against</param>
        /// <returns>The number of days left in a month</returns>
        public static int DaysLeftInMonth(this DateTime Date)
        {
            if (Date == null)
                throw new ArgumentNullException("Date");
            return Thread.CurrentThread.CurrentCulture.Calendar.GetDaysInMonth(Date.Year, Date.Month) - Date.Day;
        }

        #endregion

        #region DaysLeftInYear

        /// <summary>
        /// Gets the number of days left in a year based on the date passed in
        /// </summary>
        /// <param name="Date">The date to check against</param>
        /// <returns>The number of days left in a year</returns>
        public static int DaysLeftInYear(this DateTime Date)
        {
            if (Date == null)
                throw new ArgumentNullException("Date");
            return Thread.CurrentThread.CurrentCulture.Calendar.GetDaysInYear(Date.Year) - Date.DayOfYear;
        }

        #endregion

        #region DaysLeftInWeek

        /// <summary>
        /// Gets the number of days left in a week
        /// </summary>
        /// <param name="Date">The date to check against</param>
        /// <returns>The number of days left in a week</returns>
        public static int DaysLeftInWeek(this DateTime Date)
        {
            if (Date == null)
                throw new ArgumentNullException("Date");
            return 7 - ((int)Date.DayOfWeek + 1);
        }

        #endregion

        #region IsWeekDay

        /// <summary>
        /// Determines if this is a week day
        /// </summary>
        /// <param name="Date">Date to check against</param>
        /// <returns>Whether this is a week day or not</returns>
        public static bool IsWeekDay(this DateTime Date)
        {
            if (Date == null)
                throw new ArgumentNullException("Date");
            if ((int)Date.DayOfWeek < 6 && (int)Date.DayOfWeek > 0)
                return true;
            return false;
        }

        #endregion

        #region IsWeekEnd

        /// <summary>
        /// Determines if this is a week end
        /// </summary>
        /// <param name="Date">Date to check against</param>
        /// <returns>Whether this is a week end or not</returns>
        public static bool IsWeekEnd(this DateTime Date)
        {
            if (Date == null)
                throw new ArgumentNullException("Date");
            return !IsWeekDay(Date);
        }

        #endregion

        #endregion
    }
}
