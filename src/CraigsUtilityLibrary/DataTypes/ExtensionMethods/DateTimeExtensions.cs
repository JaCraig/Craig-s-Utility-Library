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
using System.Diagnostics.Contracts;
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
        /// <param name="Date">Date input</param>
        /// <param name="NumberOfWeeks">Number of weeks to add</param>
        /// <returns>The date after the number of weeks are added</returns>
        public static DateTime AddWeeks(this DateTime Date, int NumberOfWeeks)
        {
            return Date.AddDays(NumberOfWeeks * 7);
        }

        /// <summary>
        /// Calculates age based on date supplied
        /// </summary>
        /// <param name="Date">Birth date</param>
        /// <param name="CalculateFrom">Date to calculate from</param>
        /// <returns>The total age in years</returns>
        public static int Age(this DateTime Date, DateTime CalculateFrom = default(DateTime))
        {
            if (CalculateFrom == default(DateTime))
                CalculateFrom = DateTime.Now;
            return (CalculateFrom - Date).Years();
        }

        /// <summary>
        /// Beginning of a specific time frame
        /// </summary>
        /// <param name="Date">Date to base off of</param>
        /// <param name="TimeFrame">Time frame to use</param>
        /// <param name="Culture">
        /// Culture to use for calculating (defaults to the current culture)
        /// </param>
        /// <returns>The beginning of a specific time frame</returns>
        public static DateTime BeginningOf(this DateTime Date, TimeFrame TimeFrame, CultureInfo Culture = null)
        {
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            if (TimeFrame == TimeFrame.Day)
                return Date.Date;
            if (TimeFrame == TimeFrame.Week)
                return Date.AddDays(Culture.DateTimeFormat.FirstDayOfWeek - Date.DayOfWeek).Date;
            if (TimeFrame == TimeFrame.Month)
                return new DateTime(Date.Year, Date.Month, 1);
            if (TimeFrame == TimeFrame.Quarter)
                return Date.BeginningOf(TimeFrame.Quarter, Date.BeginningOf(TimeFrame.Year, Culture), Culture);
            return new DateTime(Date.Year, 1, 1);
        }

        /// <summary>
        /// Beginning of a specific time frame
        /// </summary>
        /// <param name="Date">Date to base off of</param>
        /// <param name="TimeFrame">Time frame to use</param>
        /// <param name="Culture">
        /// Culture to use for calculating (defaults to the current culture)
        /// </param>
        /// <param name="StartOfQuarter1">Start of the first quarter</param>
        /// <returns>The beginning of a specific time frame</returns>
        public static DateTime BeginningOf(this DateTime Date, TimeFrame TimeFrame, DateTime StartOfQuarter1, CultureInfo Culture = null)
        {
            if (TimeFrame != TimeFrame.Quarter)
                return Date.BeginningOf(TimeFrame, Culture);
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            if (Date.Between(StartOfQuarter1, StartOfQuarter1.AddMonths(3).AddDays(-1).EndOf(TimeFrame.Day, CultureInfo.CurrentCulture)))
                return StartOfQuarter1.Date;
            else if (Date.Between(StartOfQuarter1.AddMonths(3), StartOfQuarter1.AddMonths(6).AddDays(-1).EndOf(TimeFrame.Day, CultureInfo.CurrentCulture)))
                return StartOfQuarter1.AddMonths(3).Date;
            else if (Date.Between(StartOfQuarter1.AddMonths(6), StartOfQuarter1.AddMonths(9).AddDays(-1).EndOf(TimeFrame.Day, CultureInfo.CurrentCulture)))
                return StartOfQuarter1.AddMonths(6).Date;
            return StartOfQuarter1.AddMonths(9).Date;
        }

        /// <summary>
        /// Gets the number of days in the time frame specified based on the date
        /// </summary>
        /// <param name="Date">Date</param>
        /// <param name="TimeFrame">Time frame to calculate the number of days from</param>
        /// <param name="Culture">
        /// Culture to use for calculating (defaults to the current culture)
        /// </param>
        /// <returns>The number of days in the time frame</returns>
        public static int DaysIn(this DateTime Date, TimeFrame TimeFrame, CultureInfo Culture = null)
        {
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            if (TimeFrame == TimeFrame.Day)
                return 1;
            if (TimeFrame == TimeFrame.Week)
                return 7;
            if (TimeFrame == TimeFrame.Month)
                return Culture.Calendar.GetDaysInMonth(Date.Year, Date.Month);
            if (TimeFrame == TimeFrame.Quarter)
                return Date.EndOf(TimeFrame.Quarter, Culture).DayOfYear - Date.BeginningOf(TimeFrame.Quarter, Culture).DayOfYear;
            return Culture.Calendar.GetDaysInYear(Date.Year);
        }

        /// <summary>
        /// Gets the number of days in the time frame specified based on the date
        /// </summary>
        /// <param name="Date">Date</param>
        /// <param name="TimeFrame">Time frame to calculate the number of days from</param>
        /// <param name="Culture">
        /// Culture to use for calculating (defaults to the current culture)
        /// </param>
        /// <param name="StartOfQuarter1">Start of the first quarter</param>
        /// <returns>The number of days in the time frame</returns>
        public static int DaysIn(this DateTime Date, TimeFrame TimeFrame, DateTime StartOfQuarter1, CultureInfo Culture = null)
        {
            if (TimeFrame != TimeFrame.Quarter)
                Date.DaysIn(TimeFrame, Culture);
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            return Date.EndOf(TimeFrame.Quarter, Culture).DayOfYear - StartOfQuarter1.DayOfYear;
        }

        /// <summary>
        /// Gets the number of days left in the time frame specified based on the date
        /// </summary>
        /// <param name="Date">Date</param>
        /// <param name="TimeFrame">Time frame to calculate the number of days left</param>
        /// <param name="Culture">
        /// Culture to use for calculating (defaults to the current culture)
        /// </param>
        /// <returns>The number of days left in the time frame</returns>
        public static int DaysLeftIn(this DateTime Date, TimeFrame TimeFrame, CultureInfo Culture = null)
        {
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            if (TimeFrame == TimeFrame.Day)
                return 1;
            if (TimeFrame == TimeFrame.Week)
                return 7 - ((int)Date.DayOfWeek + 1);
            if (TimeFrame == TimeFrame.Month)
                return Date.DaysIn(TimeFrame.Month, Culture) - Date.Day;
            if (TimeFrame == TimeFrame.Quarter)
                return Date.DaysIn(TimeFrame.Quarter, Culture) - (Date.DayOfYear - Date.BeginningOf(TimeFrame.Quarter, Culture).DayOfYear);
            return Date.DaysIn(TimeFrame.Year, Culture) - Date.DayOfYear;
        }

        /// <summary>
        /// Gets the number of days left in the time frame specified based on the date
        /// </summary>
        /// <param name="Date">Date</param>
        /// <param name="TimeFrame">Time frame to calculate the number of days left</param>
        /// <param name="Culture">
        /// Culture to use for calculating (defaults to the current culture)
        /// </param>
        /// <param name="StartOfQuarter1">Start of the first quarter</param>
        /// <returns>The number of days left in the time frame</returns>
        public static int DaysLeftIn(this DateTime Date, TimeFrame TimeFrame, DateTime StartOfQuarter1, CultureInfo Culture = null)
        {
            if (TimeFrame != TimeFrame.Quarter)
                return Date.DaysLeftIn(TimeFrame, Culture);
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            return Date.DaysIn(TimeFrame.Quarter, StartOfQuarter1, Culture) - (Date.DayOfYear - StartOfQuarter1.DayOfYear);
        }

        /// <summary>
        /// End of a specific time frame
        /// </summary>
        /// <param name="Date">Date to base off of</param>
        /// <param name="TimeFrame">Time frame to use</param>
        /// <param name="Culture">
        /// Culture to use for calculating (defaults to the current culture)
        /// </param>
        /// <returns>
        /// The end of a specific time frame (TimeFrame.Day is the only one that sets the time to
        /// 12: 59:59 PM, all else are the beginning of the day)
        /// </returns>
        public static DateTime EndOf(this DateTime Date, TimeFrame TimeFrame, CultureInfo Culture = null)
        {
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            if (TimeFrame == TimeFrame.Day)
                return new DateTime(Date.Year, Date.Month, Date.Day, 23, 59, 59);
            if (TimeFrame == TimeFrame.Week)
                return Date.BeginningOf(TimeFrame.Week, Culture).AddDays(6);
            if (TimeFrame == TimeFrame.Month)
                return Date.AddMonths(1).BeginningOf(TimeFrame.Month, Culture).AddDays(-1).Date;
            if (TimeFrame == TimeFrame.Quarter)
                return Date.EndOf(TimeFrame.Quarter, Date.BeginningOf(TimeFrame.Year, Culture), Culture);
            return new DateTime(Date.Year, 12, 31);
        }

        /// <summary>
        /// End of a specific time frame
        /// </summary>
        /// <param name="Date">Date to base off of</param>
        /// <param name="TimeFrame">Time frame to use</param>
        /// <param name="Culture">
        /// Culture to use for calculating (defaults to the current culture)
        /// </param>
        /// <param name="StartOfQuarter1">Start of the first quarter</param>
        /// <returns>
        /// The end of a specific time frame (TimeFrame.Day is the only one that sets the time to
        /// 12: 59:59 PM, all else are the beginning of the day)
        /// </returns>
        public static DateTime EndOf(this DateTime Date, TimeFrame TimeFrame, DateTime StartOfQuarter1, CultureInfo Culture = null)
        {
            if (TimeFrame != TimeFrame.Quarter)
                return Date.EndOf(TimeFrame, Culture);
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            if (Date.Between(StartOfQuarter1, StartOfQuarter1.AddMonths(3).AddDays(-1).EndOf(TimeFrame.Day, Culture)))
                return StartOfQuarter1.AddMonths(3).AddDays(-1).Date;
            else if (Date.Between(StartOfQuarter1.AddMonths(3), StartOfQuarter1.AddMonths(6).AddDays(-1).EndOf(TimeFrame.Day, Culture)))
                return StartOfQuarter1.AddMonths(6).AddDays(-1).Date;
            else if (Date.Between(StartOfQuarter1.AddMonths(6), StartOfQuarter1.AddMonths(9).AddDays(-1).EndOf(TimeFrame.Day, Culture)))
                return StartOfQuarter1.AddMonths(9).AddDays(-1).Date;
            return StartOfQuarter1.AddYears(1).AddDays(-1).Date;
        }

        /// <summary>
        /// Determines if the date fulfills the comparison
        /// </summary>
        /// <param name="Date">Date to check</param>
        /// <param name="Comparison">
        /// Comparison type (can be combined, so you can do weekday in the future, etc)
        /// </param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is(this DateTime Date, DateCompare Comparison)
        {
            if (Comparison.HasFlag(DateCompare.InFuture) && DateTime.Now >= Date)
                return false;
            if (Comparison.HasFlag(DateCompare.InPast) && DateTime.Now <= Date)
                return false;
            if (Comparison.HasFlag(DateCompare.Today) && DateTime.Today != Date.Date)
                return false;
            if (Comparison.HasFlag(DateCompare.WeekDay) && ((int)Date.DayOfWeek == 6 || (int)Date.DayOfWeek == 0))
                return false;
            if (Comparison.HasFlag(DateCompare.WeekEnd) && (int)Date.DayOfWeek != 6 && (int)Date.DayOfWeek != 0)
                return false;
            return true;
        }

        /// <summary>
        /// Gets the local time zone
        /// </summary>
        /// <param name="Date">Date object</param>
        /// <returns>The local time zone</returns>
        public static TimeZoneInfo LocalTimeZone(this DateTime Date)
        {
            return TimeZoneInfo.Local;
        }

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

        /// <summary>
        /// Converts a DateTime to a specific time zone
        /// </summary>
        /// <param name="Date">DateTime to convert</param>
        /// <param name="TimeZone">Time zone to convert to</param>
        /// <returns>The converted DateTime</returns>
        public static DateTime To(this DateTime Date, TimeZoneInfo TimeZone)
        {
            Contract.Requires<ArgumentNullException>(TimeZone != null, "TimeZone");
            return TimeZoneInfo.ConvertTime(Date, TimeZone);
        }

        /// <summary>
        /// Returns the date in int format based on an Epoch (defaults to unix epoch of 1/1/1970)
        /// </summary>
        /// <param name="Date">Date to convert</param>
        /// <param name="Epoch">Epoch to use (defaults to unix epoch of 1/1/1970)</param>
        /// <returns>The date in Unix format</returns>
        public static int To(this DateTime Date, DateTime Epoch = default(DateTime))
        {
            Epoch = Epoch.Check(x => x != default(DateTime), () => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return (int)((Date.ToUniversalTime() - Epoch).Ticks / TimeSpan.TicksPerSecond);
        }

        /// <summary>
        /// Returns the date in DateTime format based on an Epoch (defaults to unix epoch of 1/1/1970)
        /// </summary>
        /// <param name="Date">Date to convert</param>
        /// <param name="Epoch">Epoch to use (defaults to unix epoch of 1/1/1970)</param>
        /// <returns>The Unix Date in DateTime format</returns>
        public static DateTime To(this int Date, DateTime Epoch = default(DateTime))
        {
            Epoch = Epoch.Check(x => x != default(DateTime), () => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return new DateTime((Date * TimeSpan.TicksPerSecond) + Epoch.Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Returns the date in DateTime format based on an Epoch (defaults to unix epoch of 1/1/1970)
        /// </summary>
        /// <param name="Date">Date to convert</param>
        /// <param name="Epoch">Epoch to use (defaults to unix epoch of 1/1/1970)</param>
        /// <returns>The Unix Date in DateTime format</returns>
        public static DateTime To(this long Date, DateTime Epoch = default(DateTime))
        {
            Epoch = Epoch.Check(x => x != default(DateTime), () => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return new DateTime((Date * TimeSpan.TicksPerSecond) + Epoch.Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Converts the DateTime object to string describing, relatively how long ago or how far in
        /// the future the input is based off of another DateTime object specified.
        /// ex:
        /// Input=March 21, 2013 Epoch=March 22, 2013 returns "1 day ago" Input=March 22, 2013
        /// Epoch=March 21, 2013 returns "1 day from now"
        /// </summary>
        /// <param name="Input">Input</param>
        /// <param name="Epoch">DateTime object that the input is comparred to</param>
        /// <returns>The difference between the input and epoch expressed as a string</returns>
        public static string ToString(this DateTime Input, DateTime Epoch)
        {
            if (Epoch == Input)
                return "now";
            return Epoch > Input ? (Epoch - Input).ToStringFull() + " ago" : (Input - Epoch).ToStringFull() + " from now";
        }

        /// <summary>
        /// Gets the UTC offset
        /// </summary>
        /// <param name="Date">Date to get the offset of</param>
        /// <returns>UTC offset</returns>
        public static double UTCOffset(this DateTime Date)
        {
            return (Date - Date.ToUniversalTime()).TotalHours;
        }
    }
}