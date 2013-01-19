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
using System.Globalization;
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

        #region AddWeeks

        /// <summary>
        /// Adds the number of weeks to the date
        /// </summary>
        /// <param name="Date">Date input</param>
        /// <param name="NumberOfWeeks">Number of weeks to add</param>
        /// <returns>The date after the number of weeks are added</returns>
        public static DateTime AddWeeks(this DateTime Date,int NumberOfWeeks)
        {
            return Date.AddDays(NumberOfWeeks * 7);
        }

        #endregion

        #region Age

        /// <summary>
        /// Calculates age based on date supplied
        /// </summary>
        /// <param name="Date">Birth date</param>
        /// <param name="CalculateFrom">Date to calculate from</param>
        /// <returns>The total age in years</returns>
        public static int Age(this DateTime Date, DateTime CalculateFrom = default(DateTime))
        {
            if (CalculateFrom.IsDefault())
                CalculateFrom = DateTime.Now;
            return (CalculateFrom-Date).Years();
        }

        #endregion

        #region ConvertToTimeZone

        /// <summary>
        /// Converts a DateTime to a specific time zone
        /// </summary>
        /// <param name="Date">DateTime to convert</param>
        /// <param name="TimeZone">Time zone to convert to</param>
        /// <returns>The converted DateTime</returns>
        public static DateTime ConvertToTimeZone(this DateTime Date, TimeZoneInfo TimeZone)
        {
            return TimeZoneInfo.ConvertTime(Date, TimeZone);
        }

        #endregion

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

        #region EndOfDay

        /// <summary>
        /// Returns the end of the day
        /// </summary>
        /// <param name="Input">Input date</param>
        /// <returns>The end of the day</returns>
        public static DateTime EndOfDay(this DateTime Input)
        {
            return new DateTime(Input.Year, Input.Month, Input.Day, 23, 59, 59);
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

        #region FirstDayOfQuarter

        /// <summary>
        /// Returns the first day of a quarter based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the first day of the quarter from</param>
        /// <param name="Quarter1Start">Beginning of the first quarter (defaults to the beginning of the year)</param>
        /// <returns>The first day of the quarter</returns>
        public static DateTime FirstDayOfQuarter(this DateTime Date, DateTime Quarter1Start = default(DateTime))
        {
            Date.ThrowIfNull("Date");
            if (Quarter1Start.IsDefault())
                Quarter1Start = Date.FirstDayOfYear();
            if (Date.Between(Quarter1Start, Quarter1Start.AddMonths(3).AddDays(-1).EndOfDay()))
                return Quarter1Start.Date;
            else if (Date.Between(Quarter1Start.AddMonths(3), Quarter1Start.AddMonths(6).AddDays(-1).EndOfDay()))
                return Quarter1Start.AddMonths(3).Date;
            else if (Date.Between(Quarter1Start.AddMonths(6), Quarter1Start.AddMonths(9).AddDays(-1).EndOfDay()))
                return Quarter1Start.AddMonths(6).Date;
            return Quarter1Start.AddMonths(9).Date;
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

        #region FirstDayOfYear

        /// <summary>
        /// Returns the first day of a year based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the first day of the year from</param>
        /// <returns>The first day of the year</returns>
        public static DateTime FirstDayOfYear(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return new DateTime(Date.Year, 1, 1);
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

        #region IsToday

        /// <summary>
        /// Is this today?
        /// </summary>
        /// <param name="Date">Date to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsToday(this DateTime Date)
        {
            return (Date.Date == DateTime.Today);
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

        #region LastDayOfQuarter

        /// <summary>
        /// Returns the last day of a quarter based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the last day of the quarter from</param>
        /// <param name="Quarter1Start">Beginning of the first quarter (defaults to the beginning of the year)</param>
        /// <returns>The last day of the quarter</returns>
        public static DateTime LastDayOfQuarter(this DateTime Date, DateTime Quarter1Start = default(DateTime))
        {
            Date.ThrowIfNull("Date");
            if (Quarter1Start.IsDefault())
                Quarter1Start = Date.FirstDayOfYear();
            if (Date.Between(Quarter1Start, Quarter1Start.AddMonths(3).AddDays(-1).EndOfDay()))
                return Quarter1Start.AddMonths(3).AddDays(-1).Date;
            else if (Date.Between(Quarter1Start.AddMonths(3), Quarter1Start.AddMonths(6).AddDays(-1).EndOfDay()))
                return Quarter1Start.AddMonths(6).AddDays(-1).Date;
            else if (Date.Between(Quarter1Start.AddMonths(6), Quarter1Start.AddMonths(9).AddDays(-1).EndOfDay()))
                return Quarter1Start.AddMonths(9).AddDays(-1).Date;
            return Quarter1Start.AddYears(1).AddDays(-1).Date;
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
            return Date.FirstDayOfWeek(CultureInfo.NullCheck( CultureInfo.CurrentCulture)).AddDays(6);
        }

        #endregion

        #region LastDayOfYear

        /// <summary>
        /// Returns the last day of the year based on the date sent in
        /// </summary>
        /// <param name="Date">Date to get the last day from</param>
        /// <returns>The last day of the year</returns>
        public static DateTime LastDayOfYear(this DateTime Date)
        {
            Date.ThrowIfNull("Date");
            return new DateTime(Date.Year, 12, 31);
        }

        #endregion

        #region LocalTimeZone

        /// <summary>
        /// Gets the local time zone
        /// </summary>
        /// <param name="Date">Date object</param>
        /// <returns>The local time zone</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "Date")]
        public static TimeZoneInfo LocalTimeZone(this DateTime Date)
        {
            return TimeZoneInfo.Local;
        }

        #endregion

        #region RelativeTime

        /// <summary>
        /// Converts the DateTime object to string describing, relatively how long ago or how far in the future
        /// the input is based off of another DateTime object specified.
        /// ex: 
        /// Input=March 21, 2013
        /// Epoch=March 22, 2013
        /// returns "1 day ago"
        /// Input=March 22, 2013
        /// Epoch=March 21, 2013
        /// returns "1 day from now"
        /// </summary>
        /// <param name="Input">Input</param>
        /// <param name="Epoch">DateTime object that the input is comparred to</param>
        /// <returns>The difference between the input and epoch expressed as a string</returns>
        public static string RelativeTime(this DateTime Input, DateTime Epoch)
        {
            if (Epoch == Input)
                return "now";
            return Epoch > Input ? (Epoch - Input).ToStringFull() + " ago" : (Input - Epoch).ToStringFull() + " from now";
        }

        #endregion

        #region SetTime

        /// <summary>
        /// Sets the time portion of a specific date
        /// </summary>
        /// <param name="Date">Date input</param>
        /// <param name="Hour">Hour to set</param>
        /// <param name="Minutes">Minutes to set</param>
        /// <param name="Seconds">Seconds to set</param>
        /// <returns>Sets the time portion of the specified date</returns>
        public static DateTime SetTime(this DateTime Date, int Hour, int Minutes, int Seconds)
        {
            return Date.SetTime(new TimeSpan(Hour, Minutes, Seconds));
        }

        /// <summary>
        /// Sets the time portion of a specific date
        /// </summary>
        /// <param name="Date">Date input</param>
        /// <param name="Time">Time to set</param>
        /// <returns>Sets the time portion of the specified date</returns>
        public static DateTime SetTime(this DateTime Date, TimeSpan Time)
        {
            return Date.Date.Add(Time);
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

        #region UTCOffset

        /// <summary>
        /// Gets the UTC offset
        /// </summary>
        /// <param name="Date">Date to get the offset of</param>
        /// <returns>UTC offset</returns>
        public static double UTCOffset(this DateTime Date)
        {
            return (Date - Date.ToUniversalTime()).TotalHours;
        }

        #endregion

        #endregion
    }
}