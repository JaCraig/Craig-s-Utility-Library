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
            switch (DayOfWeek.ToLower())
            {
                case "sunday":
                    return 1;
                case "monday":
                    return 2;
                case "tuesday":
                    return 3;
                case "wednesday":
                    return 4;
                case "thursday":
                    return 5;
                case "friday":
                    return 6;
                case "saturday":
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
            switch (Month.ToLower())
            {
                case "january":
                    return 1;
                case "february":
                    return 2;
                case "march":
                    return 3;
                case "april":
                    return 4;
                case "may":
                    return 5;
                case "june":
                    return 6;
                case "july":
                    return 7;
                case "august":
                    return 8;
                case "september":
                    return 9;
                case "october":
                    return 10;
                case "november":
                    return 11;
                case "december":
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
            if (string.IsNullOrEmpty(Input))
                return "";
            byte[] TempArray = Convert.FromBase64String(Input);
            return Encoding.UTF8.GetString(TempArray);
        }

        /// <summary>
        /// Converts a normal string to base 64 string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>A base 64 string</returns>
        public static string StringToBase64(string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            byte[] TempArray = Encoding.UTF8.GetBytes(Input);
            return Convert.ToBase64String(TempArray);
        }

        /// <summary>
        /// Converts a byte array to ASCII string
        /// </summary>
        /// <param name="Input">input array</param>
        /// <returns>ASCII string of the byte array</returns>
        public static string ByteArrayToASCIIString(byte[] Input)
        {
            if (Input == null)
                return "";
            ASCIIEncoding Encoding = new ASCIIEncoding();
            return Encoding.GetString(Input);
        }

        /// <summary>
        /// Converts a byte array to unicode string
        /// </summary>
        /// <param name="Input">Input array</param>
        /// <returns>Unicode string of the byte array</returns>
        public static string ByteArrayToUnicodeString(byte[] Input)
        {
            if (Input == null)
                return "";
            UnicodeEncoding Encoding = new UnicodeEncoding();
            return Encoding.GetString(Input);
        }

        /// <summary>
        /// Converts an ASCII string to a byte array
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>Byte array of the string</returns>
        public static byte[] ASCIIStringToByteArray(string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return null;
            ASCIIEncoding Encoding = new ASCIIEncoding();
            return Encoding.GetBytes(Input);
        }

        /// <summary>
        /// Converts a unicode string to a byte array
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>Byte array of the string</returns>
        public static byte[] UnicodeStringToByteArray(string Input)
        {
            if(string.IsNullOrEmpty(Input))
                return null;
            UnicodeEncoding Encoding = new UnicodeEncoding();
            return Encoding.GetBytes(Input);
        }

        /// <summary>
        /// Converts a .Net type to SQLDbType value
        /// </summary>
        /// <param name="Type">.Net Type</param>
        /// <returns>The corresponding SQLDbType</returns>
        public static SqlDbType NetTypeToSQLDbType(Type Type)
        {
            if (Type==typeof(string))
                return SqlDbType.NVarChar;
            else if (Type==typeof(long))
                return SqlDbType.BigInt;
            else if (Type == typeof(bool))
                return SqlDbType.Bit;
            else if (Type == typeof(char))
                return SqlDbType.NChar;
            else if (Type == typeof(DateTime))
                return SqlDbType.DateTime;
            else if (Type == typeof(decimal))
                return SqlDbType.Decimal;
            else if (Type == typeof(float) || Type == typeof(double))
                return SqlDbType.Float;
            else if (Type == typeof(Single))
                return SqlDbType.Real;
            else if (Type == typeof(short))
                return SqlDbType.SmallInt;
            else if (Type == typeof(Guid))
                return SqlDbType.UniqueIdentifier;
            return SqlDbType.Int;
        }

        /// <summary>
        /// Converts a SQLDbType value to .Net type
        /// </summary>
        /// <param name="Type">SqlDbType Type</param>
        /// <returns>The corresponding .Net type</returns>
        public static Type SQLDbTypeToNetType(SqlDbType Type)
        {
            if (Type == SqlDbType.NVarChar)
                return typeof(string);
            else if (Type == SqlDbType.BigInt)
                return typeof(long);
            else if (Type == SqlDbType.Bit)
                return typeof(bool);
            else if (Type == SqlDbType.NChar)
                return typeof(char);
            else if (Type == SqlDbType.DateTime)
                return typeof(DateTime);
            else if (Type == SqlDbType.Decimal)
                return typeof(decimal);
            else if (Type==SqlDbType.Float)
                return typeof(float);
            else if (Type == SqlDbType.Real)
                return typeof(Single);
            else if (Type == SqlDbType.SmallInt)
                return typeof(short);
            else if (Type == SqlDbType.UniqueIdentifier)
                return typeof(Guid);
            return typeof(int);
        }

        #endregion
    }
}