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
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// Value type extension methods
    /// </summary>
    public static class ValueTypeExtensions
    {
        #region Functions

        #region Encode

        /// <summary>
        /// Converts a string to a string of another encoding
        /// </summary>
        /// <param name="Input">input string</param>
        /// <param name="OriginalEncodingUsing">The type of encoding the string is currently using (defaults to ASCII)</param>
        /// <param name="EncodingUsing">The type of encoding the string is converted into (defaults to UTF8)</param>
        /// <returns>string of the byte array</returns>
        public static string Encode(this string Input, Encoding OriginalEncodingUsing = null, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            if (OriginalEncodingUsing == null)
                OriginalEncodingUsing = new ASCIIEncoding();
            if (EncodingUsing == null)
                EncodingUsing = new UTF8Encoding();
            return Input.ToByteArray(OriginalEncodingUsing).ToEncodedString(EncodingUsing);
        }

        #endregion

        #region FromBase64

        /// <summary>
        /// Converts base 64 string based on the encoding passed in
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="EncodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <returns>string in the encoding format</returns>
        public static string FromBase64(this string Input, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            if (EncodingUsing == null)
                EncodingUsing = new UTF8Encoding();
            byte[] TempArray = Convert.FromBase64String(Input);
            return EncodingUsing.GetString(TempArray);
        }

        #endregion

        #region ToBase64

        /// <summary>
        /// Converts from the specified encoding to a base 64 string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="OriginalEncodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <returns>Bas64 string</returns>
        public static string ToBase64(this string Input, Encoding OriginalEncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            if (OriginalEncodingUsing == null)
                OriginalEncodingUsing = new UTF8Encoding();
            byte[] TempArray = OriginalEncodingUsing.GetBytes(Input);
            return Convert.ToBase64String(TempArray);
        }

        #endregion

        #region ToByteArray

        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="Input">input string</param>
        /// <param name="EncodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <returns>the byte array representing the string</returns>
        public static byte[] ToByteArray(this string Input, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Input))
                return null;
            if (EncodingUsing == null)
                EncodingUsing = new UTF8Encoding();
            return EncodingUsing.GetBytes(Input);
        }

        #endregion

        #region ToBool

        /// <summary>
        /// Turns an int into a bool
        /// </summary>
        /// <param name="Input">Int value</param>
        /// <returns>bool equivalent</returns>
        public static bool ToBool(this int Input)
        {
            return Input > 0 ? true : false;
        }

        #endregion

        #region ToInt

        /// <summary>
        /// Converts the bool to an integer
        /// </summary>
        /// <param name="Value">Value to convert</param>
        /// <returns>The integer equivalent</returns>
        public static int ToInt(this bool Value)
        {
            return Value ? 1 : 0;
        }

        #endregion

        #region ToEncodedString

        /// <summary>
        /// Converts a byte array to a string
        /// </summary>
        /// <param name="Input">input array</param>
        /// <param name="EncodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <returns>string of the byte array</returns>
        public static string ToEncodedString(this byte[] Input, Encoding EncodingUsing = null)
        {
            if (Input == null)
                return "";
            if (EncodingUsing == null)
                EncodingUsing = new UTF8Encoding();
            return EncodingUsing.GetString(Input);
        }

        #endregion

        #endregion
    }
}
