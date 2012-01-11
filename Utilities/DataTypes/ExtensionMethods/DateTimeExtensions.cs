/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Globalization;
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// DateTime extension methods
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Extension Methods

        #region DaysInMonth

        /// <summary>
        /// Returns the number of days in the month
        /// </summary>
        /// <param name="Date">Date to get the month from</param>
        /// <returns>The number of days in the month</returns>
        public static int DaysInMonth(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return Date.LastDayOfMonth().Day;
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
            Date.ThrowIfNull("Date");
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
            Date.ThrowIfNull("Date");
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
            Date.ThrowIfNull("Date");
            return 7 - ((int)Date.DayOfWeek + 1);
        }

        #endregion

        #region FirstDayOfMonth

        /// <summary>
        /// Returns the first day of a month based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the first day of the month from</param>
        /// <returns>The first day of the month</returns>
        public static DateTime FirstDayOfMonth(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return new DateTime(Date.Year, Date.Month, 1);
        }

        #endregion

        #region FirstDayOfWeek

        /// <summary>
        /// Returns the first day of a week based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the first day of the week from</param>
        /// <param name="CultureInfo">The culture to use (defaults to current culture)</param>
        /// <returns>The first day of the week</returns>
        public static DateTime FirstDayOfWeek(this DateTime Date,CultureInfo CultureInfo=null)
        {
            Date.ThrowIfNull("Date");
            return Date.AddDays(CultureInfo.NullCheck(CultureInfo.CurrentCulture).DateTimeFormat.FirstDayOfWeek - Date.DayOfWeek).Date;
        }

        #endregion

        #region FromUnixTime

        /// <summary>
        /// Returns the Unix based date as a DateTime object
        /// </summary>
        /// <param name="Date">Unix date to convert</param>
        /// <returns>The Unix Date in DateTime format</returns>
        public static DateTime FromUnixTime(this int Date)
        {
            return new DateTime((Date * TimeSpan.TicksPerSecond) + new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Returns the Unix based date as a DateTime object
        /// </summary>
        /// <param name="Date">Unix date to convert</param>
        /// <returns>The Unix Date in DateTime format</returns>
        public static DateTime FromUnixTime(this long Date)
        {
            return new DateTime((Date * TimeSpan.TicksPerSecond) + new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks, DateTimeKind.Utc);
        }

        #endregion

        #region IsInFuture

        /// <summary>
        /// Determines if the date is some time in the future
        /// </summary>
        /// <param name="Date">Date to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsInFuture(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
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
            Date.ThrowIfNull("Date");
            return DateTime.Now > Date;
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
            Date.ThrowIfNull("Date");
            return (int)Date.DayOfWeek < 6 && (int)Date.DayOfWeek > 0;
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
            Date.ThrowIfNull("Date");
            return !IsWeekDay(Date);
        }

        #endregion

        #region LastDayOfMonth

        /// <summary>
        /// Returns the last day of the month based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the last day from</param>
        /// <returns>The last day of the month</returns>
        public static DateTime LastDayOfMonth(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return Date.AddMonths(1).FirstDayOfMonth().AddDays(-1).Date;
        }

        #endregion

        #region LastDayOfWeek

        /// <summary>
        /// Returns the last day of a week based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the last day of the week from</param>
        /// <param name="CultureInfo">The culture to use (defaults to current culture)</param>
        /// <returns>The last day of the week</returns>
        public static DateTime LastDayOfWeek(this DateTime Date, CultureInfo CultureInfo = null)
        {
            Date.ThrowIfNull("Date");
            return Date.FirstDayOfWeek(CultureInfo.NullCheck(CultureInfo.CurrentCulture)).AddDays(6);
        }

        #endregion

        #region ToUnix

        /// <summary>
        /// Returns the date in Unix format
        /// </summary>
        /// <param name="Date">Date to convert</param>
        /// <returns>The date in Unix format</returns>
        public static int ToUnix(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return (int)((Date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks / TimeSpan.TicksPerSecond);
        }

        #endregion

        #endregion
    }
}
