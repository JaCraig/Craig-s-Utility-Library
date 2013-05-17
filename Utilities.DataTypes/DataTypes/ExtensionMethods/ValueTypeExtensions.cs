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

#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// Value type extension methods
    /// </summary>
    public static class ValueTypeExtensions
    {
        #region Functions

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

        #region ToBase64String

        /// <summary>
        /// Converts a byte array into a base 64 string
        /// </summary>
        /// <param name="Input">Input array</param>
        /// <returns>The equivalent byte array in a base 64 string</returns>
        public static string ToBase64String(this byte[] Input)
        {
            return Input==null ? "" : Convert.ToBase64String(Input);
        }

        #endregion

        #region ToEncodedString

        /// <summary>
        /// Converts a byte array to a string
        /// </summary>
        /// <param name="Input">input array</param>
        /// <param name="EncodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <param name="Count">Number of bytes starting at the index to convert (use -1 for the entire array starting at the index)</param>
        /// <param name="Index">Index to start at</param>
        /// <returns>string of the byte array</returns>
        public static string ToEncodedString(this byte[] Input, Encoding EncodingUsing = null, int Index = 0, int Count = -1)
        {
            if (Input==null)
                return "";
            if (Count == -1)
                Count = Input.Length - Index;
            return EncodingUsing.NullCheck(new UTF8Encoding()).GetString(Input, Index, Count);
        }

        #endregion

        #region IsControl

        /// <summary>
        /// Is the character a control character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsControl(this char Value)
        {
            return char.IsControl(Value);
        }

        #endregion

        #region IsDigit

        /// <summary>
        /// Is the character a digit character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsDigit(this char Value)
        {
            return char.IsDigit(Value);
        }

        #endregion

        #region IsHighSurrogate

        /// <summary>
        /// Is the character a high surrogate character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsHighSurrogate(this char Value)
        {
            return char.IsHighSurrogate(Value);
        }

        #endregion

        #region IsLetter

        /// <summary>
        /// Is the character a letter character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsLetter(this char Value)
        {
            return char.IsLetter(Value);
        }

        #endregion

        #region IsLetterOrDigit

        /// <summary>
        /// Is the character a letter or digit character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsLetterOrDigit(this char Value)
        {
            return char.IsLetterOrDigit(Value);
        }

        #endregion

        #region IsLower

        /// <summary>
        /// Is the character a lower case character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsLower(this char Value)
        {
            return char.IsLower(Value);
        }

        #endregion

        #region IsLowSurrogate

        /// <summary>
        /// Is the character a low surrogate character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsLowSurrogate(this char Value)
        {
            return char.IsLowSurrogate(Value);
        }

        #endregion

        #region IsNumber

        /// <summary>
        /// Is the character a number character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsNumber(this char Value)
        {
            return char.IsNumber(Value);
        }

        #endregion

        #region IsPunctuation

        /// <summary>
        /// Is the character a punctuation character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsPunctuation(this char Value)
        {
            return char.IsPunctuation(Value);
        }

        #endregion

        #region IsSurrogate

        /// <summary>
        /// Is the character a surrogate character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsSurrogate(this char Value)
        {
            return char.IsSurrogate(Value);
        }

        #endregion

        #region IsSymbol

        /// <summary>
        /// Is the character a symbol character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsSymbol(this char Value)
        {
            return char.IsSymbol(Value);
        }

        #endregion

        #region IsUpper

        /// <summary>
        /// Is the character an upper case character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsUpper(this char Value)
        {
            return char.IsUpper(Value);
        }

        #endregion

        #region IsWhiteSpace

        /// <summary>
        /// Is the character a whitespace character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsWhiteSpace(this char Value)
        {
            return char.IsWhiteSpace(Value);
        }

        #endregion

        #region IsUnicode

        /// <summary>
        /// Determines if a byte array is unicode
        /// </summary>
        /// <param name="Input">Input array</param>
        /// <returns>True if it's unicode, false otherwise</returns>
        public static bool IsUnicode(this byte[] Input)
        {
            return Input==null ? true : Input.ToEncodedString(new UnicodeEncoding()).IsUnicode();
        }

        #endregion

        #endregion
    }
}