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
        /// <param name="Value">Value to check</param>
        /// <param name="CharacterType">Character type</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool Is(this char Value, CharIs CharacterType)
        {
            if (CharacterType.HasFlag(CharIs.WhiteSpace))
                return char.IsWhiteSpace(Value);
            if (CharacterType.HasFlag(CharIs.Upper))
                return char.IsUpper(Value);
            if (CharacterType.HasFlag(CharIs.Symbol))
                return char.IsSymbol(Value);
            if (CharacterType.HasFlag(CharIs.Surrogate))
                return char.IsSurrogate(Value);
            if (CharacterType.HasFlag(CharIs.Punctuation))
                return char.IsPunctuation(Value);
            if (CharacterType.HasFlag(CharIs.Number))
                return char.IsNumber(Value);
            if (CharacterType.HasFlag(CharIs.LowSurrogate))
                return char.IsLowSurrogate(Value);
            if (CharacterType.HasFlag(CharIs.Lower))
                return char.IsLower(Value);
            if (CharacterType.HasFlag(CharIs.LetterOrDigit))
                return char.IsLetterOrDigit(Value);
            if (CharacterType.HasFlag(CharIs.Letter))
                return char.IsLetter(Value);
            if (CharacterType.HasFlag(CharIs.HighSurrogate))
                return char.IsHighSurrogate(Value);
            if (CharacterType.HasFlag(CharIs.Digit))
                return char.IsDigit(Value);
            if (CharacterType.HasFlag(CharIs.Control))
                return char.IsControl(Value);
            return false;
        }

        /// <summary>
        /// Determines if a byte array is unicode
        /// </summary>
        /// <param name="Input">Input array</param>
        /// <returns>True if it's unicode, false otherwise</returns>
        public static bool IsUnicode(this byte[] Input)
        {
            return Input == null || Input.ToString(new UnicodeEncoding()).Is(StringCompare.Unicode);
        }

        /// <summary>
        /// Converts a byte array into a base 64 string
        /// </summary>
        /// <param name="Input">Input array</param>
        /// <param name="Count">
        /// Number of bytes starting at the index to convert (use -1 for the entire array starting
        /// at the index)
        /// </param>
        /// <param name="Index">Index to start at</param>
        /// <param name="Options">Base 64 formatting options</param>
        /// <returns>The equivalent byte array in a base 64 string</returns>
        public static string ToString(this byte[] Input, Base64FormattingOptions Options, int Index = 0, int Count = -1)
        {
            Contract.Requires<ArgumentException>(Index >= 0, "Index");
            if (Count == -1)
                Count = Input.Length - Index;
            return Input == null ? "" : Convert.ToBase64String(Input, Index, Count, Options);
        }

        /// <summary>
        /// Converts a byte array to a string
        /// </summary>
        /// <param name="Input">input array</param>
        /// <param name="EncodingUsing">
        /// The type of encoding the string is using (defaults to UTF8)
        /// </param>
        /// <param name="Count">
        /// Number of bytes starting at the index to convert (use -1 for the entire array starting
        /// at the index)
        /// </param>
        /// <param name="Index">Index to start at</param>
        /// <returns>string of the byte array</returns>
        public static string ToString(this byte[] Input, Encoding EncodingUsing, int Index = 0, int Count = -1)
        {
            if (Input == null)
                return "";
            if (Count == -1)
                Count = Input.Length - Index;
            return EncodingUsing.Check(new UTF8Encoding()).GetString(Input, Index, Count);
        }
    }
}