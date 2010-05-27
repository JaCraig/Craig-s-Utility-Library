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
using System.Text;
#endregion


namespace Utilities.DataTypes
{
    /// <summary>
    /// Utility class for converting between datatypes
    /// </summary>
    public static class DataTypeConversion
    {
        #region Static Public Functions
        /// <summary>
        /// Turns a bool into an int
        /// </summary>
        /// <param name="Input">Bool value</param>
        /// <returns>Int equivalent</returns>
        public static int BoolToInt(bool Input)
        {
            return Input ? 1 : 0;
        }

        /// <summary>
        /// Turns an int into a bool
        /// </summary>
        /// <param name="Input">Int value</param>
        /// <returns>bool equivalent</returns>
        public static bool IntToBool(int Input)
        {
            return Input > 0 ? true : false;
        }

        /// <summary>
        /// Takes the numeric value and returns the day of the week.
        /// </summary>
        /// <param name="DayOfWeek">The int representation of the day of the week</param>
        /// <returns>returns the string value for the day of the week</returns>
        public static string IntToDay(int DayOfWeek)
        {
            switch (DayOfWeek)
            {
                case 1:
                    return "Sunday";
                case 2:
                    return "Monday";
                case 3:
                    return "Tuesday";
                case 4:
                    return "Wednesday";
                case 5:
                    return "Thursday";
                case 6:
                    return "Friday";
                case 7:
                    return "Saturday";
            }
            return "";
        }

        /// <summary>
        /// Takes the day of the week and returns the numeric value (1-7).
        /// </summary>
        /// <param name="DayOfWeek">The string representation of the day of the week</param>
        /// <returns>returns the int value for the day of the week</returns>
        public static int DayToInt(string DayOfWeek)
        {
            switch (DayOfWeek)
            {
                case "Sunday":
                    return 1;
                case "Monday":
                    return 2;
                case "Tuesday":
                    return 3;
                case "Wednesday":
                    return 4;
                case "Thursday":
                    return 5;
                case "Friday":
                    return 6;
                case "Saturday":
                    return 7;
            }
            return 0;
        }

        /// <summary>
        /// Takes the int value for the month and returns the name of the month
        /// </summary>
        /// <param name="Month">The month in int form</param>
        /// <returns>returns the string value for the month</returns>
        public static string IntMonthToString(int Month)
        {
            switch (Month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "";       
            }
        }

        /// <summary>
        /// Takes the int value for the month and returns the name of the month
        /// </summary>
        /// <param name="Month">The month in int form</param>
        /// <returns>returns the string value for the month</returns>
        public static int StringToIntMonth(string Month)
        {
            switch (Month)
            {
                case "January":
                    return 1;
                case "February":
                    return 2;
                case "March":
                    return 3;
                case "April":
                    return 4;
                case "May":
                    return 5;
                case "June":
                    return 6;
                case "July":
                    return 7;
                case "August":
                    return 8;
                case "September":
                    return 9;
                case "October":
                    return 10;
                case "November":
                    return 11;
                case "December":
                    return 12;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Converts base 64 string to normal string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>normal string (non base 64)</returns>
        public static string Base64ToString(string Input)
        {
            try
            {
                if (string.IsNullOrEmpty(Input))
                    return "";
                byte[] TempArray = Convert.FromBase64String(Input);
                return Encoding.UTF8.GetString(TempArray);
            }
            catch { throw; }
        }

        /// <summary>
        /// Converts a normal string to base 64 string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>A base 64 string</returns>
        public static string StringToBase64(string Input)
        {
            try
            {
                if (string.IsNullOrEmpty(Input))
                    return "";
                byte[] TempArray = Encoding.UTF8.GetBytes(Input);
                return Convert.ToBase64String(TempArray);
            }
            catch { throw; }
        }
        #endregion
    }
}
