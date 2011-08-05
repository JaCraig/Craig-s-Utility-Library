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
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
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

        #region Left

        /// <summary>
        /// Gets the first x number of characters from the left hand side
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Length">x number of characters to return</param>
        /// <returns>The resulting string</returns>
        public static string Left(this string Input, int Length)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            return Input.Substring(0, Input.Length > Length ? Length : Input.Length);
        }

        #endregion

        #region Right

        /// <summary>
        /// Gets the last x number of characters from the right hand side
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Length">x number of characters to return</param>
        /// <returns>The resulting string</returns>
        public static string Right(this string Input, int Length)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            Length = Input.Length > Length ? Length : Input.Length;
            return Input.Substring(Input.Length - Length, Length);
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

        #region ToFirstCharacterUpperCase

        /// <summary>
        /// Takes the first character of an input string and makes it uppercase
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>String with the first character capitalized</returns>
        public static string ToFirstCharacterUpperCase(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            char[] InputChars = Input.ToCharArray();
            for (int x = 0; x < InputChars.Length; ++x)
            {
                if (InputChars[x] != ' ' && InputChars[x] != '\t')
                {
                    InputChars[x] = char.ToUpper(InputChars[x]);
                    break;
                }
            }
            return new string(InputChars);
        }

        #endregion

        #region ToSentenceCapitalize

        /// <summary>
        /// Capitalizes each sentence within the string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>String with each sentence capitalized</returns>
        public static string ToSentenceCapitalize(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            string[] Seperator = { ".", "?", "!" };
            string[] InputStrings = Input.Split(Seperator, StringSplitOptions.None);
            for (int x = 0; x < InputStrings.Length; ++x)
            {
                if (!string.IsNullOrEmpty(InputStrings[x]))
                {
                    Regex TempRegex = new Regex(InputStrings[x]);
                    InputStrings[x] = InputStrings[x].ToFirstCharacterUpperCase();
                    Input = TempRegex.Replace(Input, InputStrings[x]);
                }
            }
            return Input;
        }

        #endregion

        #region ToTitleCase

        /// <summary>
        /// Capitalizes the first character of each word
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>String with each word capitalized</returns>
        public static string ToTitleCase(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            string[] Seperator = { " ", ".", "\t", System.Environment.NewLine, "!", "?" };
            string[] InputStrings = Input.Split(Seperator, StringSplitOptions.None);
            for (int x = 0; x < InputStrings.Length; ++x)
            {
                if (!string.IsNullOrEmpty(InputStrings[x])
                    && InputStrings[x].Length>3)
                {
                    Regex TempRegex = new Regex(InputStrings[x]);
                    InputStrings[x] = InputStrings[x].ToFirstCharacterUpperCase();
                    Input = TempRegex.Replace(Input, InputStrings[x]);
                }
            }
            return Input;
        }

        #endregion

        #region NumberTimesOccurs

        /// <summary>
        /// returns the number of times a string occurs within the text
        /// </summary>
        /// <param name="Input">input text</param>
        /// <param name="Match">The string to match (can be regex)</param>
        /// <returns>The number of times the string occurs</returns>
        public static int NumberTimesOccurs(this string Input, string Match)
        {
            if (string.IsNullOrEmpty(Input))
                return 0;
            return new Regex(Match).Matches(Input).Count;
        }

        #endregion

        #region Reverse

        /// <summary>
        /// Reverses a string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>The reverse of the input string</returns>
        public static string Reverse(this string Input)
        {
            return new string(Input.Reverse<char>().ToArray());
        }

        #endregion

        #region FilterOutText

        /// <summary>
        /// Removes the filter text from the input.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Filter">Regex expression of text to filter out</param>
        /// <returns>The input text minus the filter text.</returns>
        public static string FilterOutText(this string Input, string Filter)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            if (string.IsNullOrEmpty(Filter))
                return Input;
            return new Regex(Filter).Replace(Input, "");
        }

        #endregion

        #region KeepFilterText

        /// <summary>
        /// Removes everything that is not in the filter text from the input.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Filter">Regex expression of text to keep</param>
        /// <returns>The input text minus everything not in the filter text.</returns>
        public static string KeepFilterText(this string Input, string Filter)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            if (string.IsNullOrEmpty(Filter))
                return "";
            Regex TempRegex = new Regex(Filter);
            MatchCollection Collection = TempRegex.Matches(Input);
            StringBuilder Builder = new StringBuilder();
            foreach (Match Match in Collection)
                Builder.Append(Match.Value);
            return Builder.ToString();
        }

        #endregion

        #region AlphaNumericOnly

        /// <summary>
        /// Keeps only alphanumeric characters
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>the string only containing alphanumeric characters</returns>
        public static string AlphaNumericOnly(this string Input)
        {
            return Input.KeepFilterText("[a-zA-Z0-9]");
        }

        #endregion

        #region AlphaCharactersOnly

        /// <summary>
        /// Keeps only alpha characters
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>the string only containing alpha characters</returns>
        public static string AlphaCharactersOnly(this string Input)
        {
            return Input.KeepFilterText("[a-zA-Z]");
        }

        #endregion

        #region NumericOnly

        /// <summary>
        /// Keeps only numeric characters
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="KeepNumericPunctuation">Determines if decimal places should be kept</param>
        /// <returns>the string only containing numeric characters</returns>
        public static string NumericOnly(this string Input, bool KeepNumericPunctuation = true)
        {
            return KeepNumericPunctuation ? Input.KeepFilterText(@"[0-9\.]") : Input.KeepFilterText("[0-9]");
        }

        #endregion

        #region IsUnicode

        /// <summary>
        /// Determines if a string is unicode
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>True if it's unicode, false otherwise</returns>
        public static bool IsUnicode(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return true;
            return Input.Encode(new UnicodeEncoding(), new UnicodeEncoding()) != Input.Encode(new ASCIIEncoding(), new ASCIIEncoding());
        }

        #endregion

        #endregion
    }
}