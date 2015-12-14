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
using System.Text;
using Utilities.DataTypes.Formatters.Interfaces;

namespace Utilities.DataTypes.Formatters
{
    /// <summary>
    /// Generic string formatter
    /// </summary>
    public class GenericStringFormatter : IFormatProvider, ICustomFormatter, IStringFormatter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStringFormatter()
            : this('#', '@', '\\')
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericStringFormatter"/> class.
        /// </summary>
        /// <param name="digitChar">The digit character.</param>
        /// <param name="alphaChar">The alpha character.</param>
        /// <param name="escapeChar">The escape character.</param>
        public GenericStringFormatter(char digitChar, char alphaChar, char escapeChar)
        {
            DigitChar = digitChar;
            AlphaChar = alphaChar;
            EscapeChar = escapeChar;
        }

        /// <summary>
        /// Represents alpha characters (defaults to @)
        /// </summary>
        public char AlphaChar { get; protected set; }

        /// <summary>
        /// Represents digits (defaults to #)
        /// </summary>
        public char DigitChar { get; protected set; }

        /// <summary>
        /// Represents the escape character (defaults to \)
        /// </summary>
        public char EscapeChar { get; protected set; }

        /// <summary>
        /// Formats the string
        /// </summary>
        /// <param name="format">Format to use</param>
        /// <param name="arg">Argument object to use</param>
        /// <param name="formatProvider">Format provider to use</param>
        /// <returns>The formatted string</returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return Format(arg.ToString(), format);
        }

        /// <summary>
        /// Formats the string based on the pattern
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="formatPattern">Format pattern</param>
        /// <returns>The formatted string</returns>
        public string Format(string input, string formatPattern)
        {
            if (!IsValid(formatPattern))
                throw new ArgumentException("FormatPattern is not valid");
            var ReturnValue = new StringBuilder();
            for (int x = 0; x < formatPattern.Length; ++x)
            {
                if (formatPattern[x] == EscapeChar)
                {
                    ++x;
                    ReturnValue.Append(formatPattern[x]);
                }
                else
                {
                    char NextValue = char.MinValue;
                    input = GetMatchingInput(input, formatPattern[x], out NextValue);
                    if (NextValue != char.MinValue)
                        ReturnValue.Append(NextValue);
                }
            }
            return ReturnValue.ToString();
        }

        /// <summary>
        /// Gets the format associated with the type
        /// </summary>
        /// <param name="formatType">Format type</param>
        /// <returns>The appropriate formatter based on the type</returns>
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        /// <summary>
        /// Gets matching input
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="formatChar">Current format character</param>
        /// <param name="matchChar">The matching character found</param>
        /// <returns>The remainder of the input string left</returns>
        protected string GetMatchingInput(string input, char formatChar, out char matchChar)
        {
            bool Digit = formatChar == DigitChar;
            bool Alpha = formatChar == AlphaChar;
            if (!Digit && !Alpha)
            {
                matchChar = formatChar;
                return input;
            }
            int Index = 0;
            matchChar = char.MinValue;
            for (int x = 0; x < input.Length; ++x)
            {
                if ((Digit && char.IsDigit(input[x])) || (Alpha && char.IsLetter(input[x])))
                {
                    matchChar = input[x];
                    Index = x + 1;
                    break;
                }
            }
            return input.Substring(Index);
        }

        /// <summary>
        /// Checks if the format pattern is valid
        /// </summary>
        /// <param name="formatPattern">Format pattern</param>
        /// <returns>Returns true if it's valid, otherwise false</returns>
        protected bool IsValid(string formatPattern)
        {
            if (string.IsNullOrEmpty(formatPattern))
                return false;
            bool EscapeCharFound = false;
            for (int x = 0; x < formatPattern.Length; ++x)
            {
                if (EscapeCharFound && formatPattern[x] != DigitChar
                        && formatPattern[x] != AlphaChar
                        && formatPattern[x] != EscapeChar)
                    return false;
                if (EscapeCharFound)
                    EscapeCharFound = false;
                else EscapeCharFound |= formatPattern[x] == EscapeChar;
            }
            return !EscapeCharFound;
        }
    }
}