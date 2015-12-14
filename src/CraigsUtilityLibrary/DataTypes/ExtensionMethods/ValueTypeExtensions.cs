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
using System.Text;

namespace Utilities.DataTypes
{
    /// <summary>
    /// What type of character is this
    /// </summary>
    [Flags]
    public enum CharIs
    {
        /// <summary>
        /// White space
        /// </summary>
        WhiteSpace = 1,

        /// <summary>
        /// Upper case
        /// </summary>
        Upper = 2,

        /// <summary>
        /// Symbol
        /// </summary>
        Symbol = 4,

        /// <summary>
        /// Surrogate
        /// </summary>
        Surrogate = 8,

        /// <summary>
        /// Punctuation
        /// </summary>
        Punctuation = 16,

        /// <summary>
        /// Number
        /// </summary>
        Number = 32,

        /// <summary>
        /// Low surrogate
        /// </summary>
        LowSurrogate = 64,

        /// <summary>
        /// Lower
        /// </summary>
        Lower = 128,

        /// <summary>
        /// letter or digit
        /// </summary>
        LetterOrDigit = 256,

        /// <summary>
        /// Letter
        /// </summary>
        Letter = 512,

        /// <summary>
        /// High surrogate
        /// </summary>
        HighSurrogate = 1024,

        /// <summary>
        /// Digit
        /// </summary>
        Digit = 2048,

        /// <summary>
        /// Control
        /// </summary>
        Control = 4096
    }

    /// <summary>
    /// Value type extension methods
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ValueTypeExtensions
    {
        /// <summary>
        /// Is the character of a specific type
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="characterType">Character type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is(this char value, CharIs characterType)
        {
            if (characterType.HasFlag(CharIs.WhiteSpace))
                return char.IsWhiteSpace(value);
            if (characterType.HasFlag(CharIs.Upper))
                return char.IsUpper(value);
            if (characterType.HasFlag(CharIs.Symbol))
                return char.IsSymbol(value);
            if (characterType.HasFlag(CharIs.Surrogate))
                return char.IsSurrogate(value);
            if (characterType.HasFlag(CharIs.Punctuation))
                return char.IsPunctuation(value);
            if (characterType.HasFlag(CharIs.Number))
                return char.IsNumber(value);
            if (characterType.HasFlag(CharIs.LowSurrogate))
                return char.IsLowSurrogate(value);
            if (characterType.HasFlag(CharIs.Lower))
                return char.IsLower(value);
            if (characterType.HasFlag(CharIs.LetterOrDigit))
                return char.IsLetterOrDigit(value);
            if (characterType.HasFlag(CharIs.Letter))
                return char.IsLetter(value);
            if (characterType.HasFlag(CharIs.HighSurrogate))
                return char.IsHighSurrogate(value);
            if (characterType.HasFlag(CharIs.Digit))
                return char.IsDigit(value);
            if (characterType.HasFlag(CharIs.Control))
                return char.IsControl(value);
            return false;
        }

        /// <summary>
        /// Determines if a byte array is unicode
        /// </summary>
        /// <param name="input">Input array</param>
        /// <returns>True if it's unicode, false otherwise</returns>
        public static bool IsUnicode(this byte[] input)
        {
            return input == null || input.ToString(Encoding.Unicode).Is(StringCompare.Unicode);
        }

        /// <summary>
        /// Converts a byte array into a base 64 string
        /// </summary>
        /// <param name="input">Input array</param>
        /// <param name="count">
        /// Number of bytes starting at the index to convert (use -1 for the entire array starting
        /// at the index)
        /// </param>
        /// <param name="index">Index to start at</param>
        /// <param name="options">Base 64 formatting options</param>
        /// <returns>The equivalent byte array in a base 64 string</returns>
        public static string ToString(this byte[] input, int index = 0, int count = -1)
        {
            if (input == null)
                return "";
            if (index < 0)
                index = 0;
            if (index > input.Length - 1)
                index = input.Length - 1;
            if (count < 0)
                count = input.Length - index;
            return Convert.ToBase64String(input, index, count);
        }

        /// <summary>
        /// Converts a byte array to a string
        /// </summary>
        /// <param name="input">input array</param>
        /// <param name="encodingUsing">
        /// The type of encoding the string is using (defaults to UTF8)
        /// </param>
        /// <param name="count">
        /// Number of bytes starting at the index to convert (use -1 for the entire array starting
        /// at the index)
        /// </param>
        /// <param name="index">Index to start at</param>
        /// <returns>string of the byte array</returns>
        public static string ToString(this byte[] input, Encoding encodingUsing, int index = 0, int count = -1)
        {
            if (input == null)
                return "";
            if (count == -1)
                count = input.Length - index;
            encodingUsing = encodingUsing ?? Encoding.UTF8;
            return encodingUsing.GetString(input, index, count);
        }
    }
}