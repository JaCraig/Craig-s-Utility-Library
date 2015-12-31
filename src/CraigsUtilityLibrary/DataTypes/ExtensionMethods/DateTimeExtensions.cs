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
using System.ComponentModel;
using System.Globalization;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Date comparison type
    /// </summary>
    [Flags]
    public enum DateCompare
    {
        /// <summary>
        /// In the future
        /// </summary>
        InFuture = 1,

        /// <summary>
        /// In the past
        /// </summary>
        InPast = 2,

        /// <summary>
        /// Today
        /// </summary>
        Today = 4,

        /// <summary>
        /// Weekday
        /// </summary>
        WeekDay = 8,

        /// <summary>
        /// Weekend
        /// </summary>
        WeekEnd = 16
    }

    /// <summary>
    /// Time frame
    /// </summary>
    public enum TimeFrame
    {
        /// <summary>
        /// Day
        /// </summary>
        Day,

        /// <summary>
        /// Week
        /// </summary>
        Week,

        /// <summary>
        /// Month
        /// </summary>
        Month,

        /// <summary>
        /// Quarter
        /// </summary>
        Quarter,

        /// <summary>
        /// Year
        /// </summary>
        Year
    }

    /// <summary>
    /// DateTime extension methods
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Adds the number of weeks to the date
        /// </summary>
        /// <param name="date">Date input</param>
        /// <param name="numberOfWeeks">Number of weeks to add</param>
        /// <returns>The date after the number of weeks are added</returns>
        public static DateTime AddWeeks(this DateTime date, int numberOfWeeks)
        {
            return date.AddDays(numberOfWeeks * 7);
        }

        /// <summary>
        /// Calculates age based on date supplied
        /// </summary>
        /// <param name="date">Birth date</param>
        /// <param name="calculateFrom">Date to calculate from</param>
        /// <returns>The total age in years</returns>
        public static int Age(this DateTime date, DateTime calculateFrom = default(DateTime))
        {
            if (calculateFrom == default(DateTime))
                calculateFrom = DateTime.Now;
            return (calculateFrom - date).Years();
        }

        /// <summary>
        /// Beginning of a specific time frame
        /// </summary>
        /// <param name="date">Date to base off of</param>
        /// <param name="timeFrame">Time frame to use</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <returns>The beginning of a specific time frame</returns>
        public static DateTime BeginningOf(this DateTime date, TimeFrame timeFrame, CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.CurrentCulture;
            if (timeFrame == TimeFrame.Day)
                return date.Date;
            if (timeFrame == TimeFrame.Week)
                return date.AddDays(culture.DateTimeFormat.FirstDayOfWeek - date.DayOfWeek).Date;
            if (timeFrame == TimeFrame.Month)
                return new DateTime(date.Year, date.Month, 1);
            if (timeFrame == TimeFrame.Quarter)
                return date.BeginningOf(TimeFrame.Quarter, date.BeginningOf(TimeFrame.Year, culture), culture);
            return new DateTime(date.Year, 1, 1);
        }

        /// <summary>
        /// Beginning of a specific time frame
        /// </summary>
        /// <param name="date">Date to base off of</param>
        /// <param name="timeFrame">Time frame to use</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <param name="startOfQuarter1">Start of the first quarter</param>
        /// <returns>The beginning of a specific time frame</returns>
        public static DateTime BeginningOf(this DateTime date, TimeFrame timeFrame, DateTime startOfQuarter1, CultureInfo culture = null)
        {
            if (timeFrame != TimeFrame.Quarter)
                return date.BeginningOf(timeFrame, culture);
            culture = culture ?? CultureInfo.CurrentCulture;
            if (date.Between(startOfQuarter1, startOfQuarter1.AddMonths(3).AddDays(-1).EndOf(TimeFrame.Day, CultureInfo.CurrentCulture)))
                return startOfQuarter1.Date;
            else if (date.Between(startOfQuarter1.AddMonths(3), startOfQuarter1.AddMonths(6).AddDays(-1).EndOf(TimeFrame.Day, CultureInfo.CurrentCulture)))
                return startOfQuarter1.AddMonths(3).Date;
            else if (date.Between(startOfQuarter1.AddMonths(6), startOfQuarter1.AddMonths(9).AddDays(-1).EndOf(TimeFrame.Day, CultureInfo.CurrentCulture)))
                return startOfQuarter1.AddMonths(6).Date;
            return startOfQuarter1.AddMonths(9).Date;
        }

        /// <summary>
        /// Gets the number of days in the time frame specified based on the date
        /// </summary>
        /// <param name="date">Date</param>
        /// <param name="timeFrame">Time frame to calculate the number of days from</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <returns>The number of days in the time frame</returns>
        public static int DaysIn(this DateTime date, TimeFrame timeFrame, CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.CurrentCulture;
            if (timeFrame == TimeFrame.Day)
                return 1;
            if (timeFrame == TimeFrame.Week)
                return 7;
            if (timeFrame == TimeFrame.Month)
                return culture.Calendar.GetDaysInMonth(date.Year, date.Month);
            if (timeFrame == TimeFrame.Quarter)
                return date.EndOf(TimeFrame.Quarter, culture).DayOfYear - date.BeginningOf(TimeFrame.Quarter, culture).DayOfYear;
            return culture.Calendar.GetDaysInYear(date.Year);
        }

        /// <summary>
        /// Gets the number of days in the time frame specified based on the date
        /// </summary>
        /// <param name="date">Date</param>
        /// <param name="timeFrame">Time frame to calculate the number of days from</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <param name="startOfQuarter1">Start of the first quarter</param>
        /// <returns>The number of days in the time frame</returns>
        public static int DaysIn(this DateTime date, TimeFrame timeFrame, DateTime startOfQuarter1, CultureInfo culture = null)
        {
            if (timeFrame != TimeFrame.Quarter)
                date.DaysIn(timeFrame, culture);
            culture = culture ?? CultureInfo.CurrentCulture;
            return date.EndOf(TimeFrame.Quarter, culture).DayOfYear - startOfQuarter1.DayOfYear;
        }

        /// <summary>
        /// Gets the number of days left in the time frame specified based on the date
        /// </summary>
        /// <param name="date">Date</param>
        /// <param name="timeFrame">Time frame to calculate the number of days left</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <returns>The number of days left in the time frame</returns>
        public static int DaysLeftIn(this DateTime date, TimeFrame timeFrame, CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.CurrentCulture;
            if (timeFrame == TimeFrame.Day)
                return 1;
            if (timeFrame == TimeFrame.Week)
                return 7 - ((int)date.DayOfWeek + 1);
            if (timeFrame == TimeFrame.Month)
                return date.DaysIn(TimeFrame.Month, culture) - date.Day;
            if (timeFrame == TimeFrame.Quarter)
                return date.DaysIn(TimeFrame.Quarter, culture) - (date.DayOfYear - date.BeginningOf(TimeFrame.Quarter, culture).DayOfYear);
            return date.DaysIn(TimeFrame.Year, culture) - date.DayOfYear;
        }

        /// <summary>
        /// Gets the number of days left in the time frame specified based on the date
        /// </summary>
        /// <param name="date">Date</param>
        /// <param name="timeFrame">Time frame to calculate the number of days left</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <param name="startOfQuarter1">Start of the first quarter</param>
        /// <returns>The number of days left in the time frame</returns>
        public static int DaysLeftIn(this DateTime date, TimeFrame timeFrame, DateTime startOfQuarter1, CultureInfo culture = null)
        {
            if (timeFrame != TimeFrame.Quarter)
                return date.DaysLeftIn(timeFrame, culture);
            culture = culture ?? CultureInfo.CurrentCulture;
            return date.DaysIn(TimeFrame.Quarter, startOfQuarter1, culture) - (date.DayOfYear - startOfQuarter1.DayOfYear);
        }

        /// <summary>
        /// End of a specific time frame
        /// </summary>
        /// <param name="date">Date to base off of</param>
        /// <param name="timeFrame">Time frame to use</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <returns>
        /// The end of a specific time frame (TimeFrame.Day is the only one that sets the time to
        /// 12: 59:59 PM, all else are the beginning of the day)
        /// </returns>
        public static DateTime EndOf(this DateTime date, TimeFrame timeFrame, CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.CurrentCulture;
            if (timeFrame == TimeFrame.Day)
                return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            if (timeFrame == TimeFrame.Week)
                return date.BeginningOf(TimeFrame.Week, culture).AddDays(6);
            if (timeFrame == TimeFrame.Month)
                return date.AddMonths(1).BeginningOf(TimeFrame.Month, culture).AddDays(-1).Date;
            if (timeFrame == TimeFrame.Quarter)
                return date.EndOf(TimeFrame.Quarter, date.BeginningOf(TimeFrame.Year, culture), culture);
            return new DateTime(date.Year, 12, 31);
        }

        /// <summary>
        /// End of a specific time frame
        /// </summary>
        /// <param name="date">Date to base off of</param>
        /// <param name="timeFrame">Time frame to use</param>
        /// <param name="culture">Culture to use for calculating (defaults to the current culture)</param>
        /// <param name="startOfQuarter1">Start of the first quarter</param>
        /// <returns>
        /// The end of a specific time frame (TimeFrame.Day is the only one that sets the time to
        /// 12: 59:59 PM, all else are the beginning of the day)
        /// </returns>
        public static DateTime EndOf(this DateTime date, TimeFrame timeFrame, DateTime startOfQuarter1, CultureInfo culture = null)
        {
            if (timeFrame != TimeFrame.Quarter)
                return date.EndOf(timeFrame, culture);
            culture = culture ?? CultureInfo.CurrentCulture;
            if (date.Between(startOfQuarter1, startOfQuarter1.AddMonths(3).AddDays(-1).EndOf(TimeFrame.Day, culture)))
                return startOfQuarter1.AddMonths(3).AddDays(-1).Date;
            else if (date.Between(startOfQuarter1.AddMonths(3), startOfQuarter1.AddMonths(6).AddDays(-1).EndOf(TimeFrame.Day, culture)))
                return startOfQuarter1.AddMonths(6).AddDays(-1).Date;
            else if (date.Between(startOfQuarter1.AddMonths(6), startOfQuarter1.AddMonths(9).AddDays(-1).EndOf(TimeFrame.Day, culture)))
                return startOfQuarter1.AddMonths(9).AddDays(-1).Date;
            return startOfQuarter1.AddYears(1).AddDays(-1).Date;
        }

        /// <summary>
        /// Determines if the date fulfills the comparison
        /// </summary>
        /// <param name="date">Date to check</param>
        /// <param name="comparison">
        /// Comparison type (can be combined, so you can do weekday in the future, etc)
        /// </param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is(this DateTime date, DateCompare comparison)
        {
            if (comparison.HasFlag(DateCompare.InFuture) && DateTime.Now >= date)
                return false;
            if (comparison.HasFlag(DateCompare.InPast) && DateTime.Now <= date)
                return false;
            if (comparison.HasFlag(DateCompare.Today) && DateTime.Today != date.Date)
                return false;
            if (comparison.HasFlag(DateCompare.WeekDay) && ((int)date.DayOfWeek == 6 || (int)date.DayOfWeek == 0))
                return false;
            if (comparison.HasFlag(DateCompare.WeekEnd) && (int)date.DayOfWeek != 6 && (int)date.DayOfWeek != 0)
                return false;
            return true;
        }

        /// <summary>
        /// Gets the local time zone
        /// </summary>
        /// <param name="date">Date object</param>
        /// <returns>The local time zone</returns>
        public static TimeZoneInfo LocalTimeZone(this DateTime date)
        {
            return TimeZoneInfo.Local;
        }

        /// <summary>
        /// Sets the time portion of a specific date
        /// </summary>
        /// <param name="date">Date input</param>
        /// <param name="hour">Hour to set</param>
        /// <param name="minutes">Minutes to set</param>
        /// <param name="seconds">Seconds to set</param>
        /// <returns>Sets the time portion of the specified date</returns>
        public static DateTime SetTime(this DateTime date, int hour, int minutes, int seconds)
        {
            return date.SetTime(new TimeSpan(hour, minutes, seconds));
        }

        /// <summary>
        /// Sets the time portion of a specific date
        /// </summary>
        /// <param name="date">Date input</param>
        /// <param name="time">Time to set</param>
        /// <returns>Sets the time portion of the specified date</returns>
        public static DateTime SetTime(this DateTime date, TimeSpan time)
        {
            return date.Date.Add(time);
        }

        /// <summary>
        /// Converts a DateTime to a specific time zone
        /// </summary>
        /// <param name="date">DateTime to convert</param>
        /// <param name="timeZone">Time zone to convert to</param>
        /// <returns>The converted DateTime</returns>
        public static DateTime To(this DateTime date, TimeZoneInfo timeZone)
        {
            timeZone = timeZone ?? TimeZoneInfo.Utc;
            return TimeZoneInfo.ConvertTime(date, timeZone);
        }

        /// <summary>
        /// Returns the date in int format based on an Epoch (defaults to unix epoch of 1/1/1970)
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <param name="epoch">Epoch to use (defaults to unix epoch of 1/1/1970)</param>
        /// <returns>The date in Unix format</returns>
        public static int To(this DateTime date, DateTime epoch = default(DateTime))
        {
            if (epoch == default(DateTime))
                epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (int)((date.ToUniversalTime() - epoch).Ticks / TimeSpan.TicksPerSecond);
        }

        /// <summary>
        /// Returns the date in DateTime format based on an Epoch (defaults to unix epoch of 1/1/1970)
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <param name="epoch">Epoch to use (defaults to unix epoch of 1/1/1970)</param>
        /// <returns>The Unix Date in DateTime format</returns>
        public static DateTime To(this int date, DateTime epoch = default(DateTime))
        {
            if (epoch == default(DateTime))
                epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return new DateTime((date * TimeSpan.TicksPerSecond) + epoch.Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Returns the date in DateTime format based on an Epoch (defaults to unix epoch of 1/1/1970)
        /// </summary>
        /// <param name="date">Date to convert</param>
        /// <param name="epoch">Epoch to use (defaults to unix epoch of 1/1/1970)</param>
        /// <returns>The Unix Date in DateTime format</returns>
        public static DateTime To(this long date, DateTime epoch = default(DateTime))
        {
            if (epoch == default(DateTime))
                epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return new DateTime((date * TimeSpan.TicksPerSecond) + epoch.Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Converts the DateTime object to string describing, relatively how long ago or how far in
        /// the future the input is based off of another DateTime object specified. ex: Input=March
        /// 21, 2013 Epoch=March 22, 2013 returns "1 day ago" Input=March 22, 2013 Epoch=March 21,
        /// 2013 returns "1 day from now"
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="epoch">DateTime object that the input is comparred to</param>
        /// <returns>The difference between the input and epoch expressed as a string</returns>
        public static string ToString(this DateTime input, DateTime epoch)
        {
            if (epoch == input)
                return "now";
            return epoch > input ? (epoch - input).ToStringFull() + " ago" : (input - epoch).ToStringFull() + " from now";
        }

        /// <summary>
        /// Gets the UTC offset
        /// </summary>
        /// <param name="date">Date to get the offset of</param>
        /// <returns>UTC offset</returns>
        public static double UTCOffset(this DateTime date)
        {
            return (date - date.ToUniversalTime()).TotalHours;
        }
    }
}