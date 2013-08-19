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
using Utilities.DataTypes.Formatters.Interfaces;

#endregion

namespace Utilities.DataTypes.Formatters
{
    /// <summary>
    /// Generic string formatter
    /// </summary>
    public class GenericStringFormatter : IFormatProvider, ICustomFormatter,IStringFormatter
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStringFormatter()
        {
            DigitChar = '#';
            AlphaChar = '@';
            EscapeChar='\\';
        }

        #endregion

        #region IFormatProvider Functions

        /// <summary>
        /// Gets the format associated with the type
        /// </summary>
        /// <param name="formatType">Format type</param>
        /// <returns>The appropriate formatter based on the type</returns>
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        #endregion

        #region ICustomFormatter Functions

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

        #endregion

        #region Functions

        #region Format

        /// <summary>
        /// Formats the string based on the pattern
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="FormatPattern">Format pattern</param>
        /// <returns>The formatted string</returns>
        public virtual string Format(string Input, string FormatPattern)
        {
            if (!IsValid(FormatPattern))
                throw new ArgumentException("FormatPattern is not valid");
            StringBuilder ReturnValue = new StringBuilder();
            for (int x = 0; x < FormatPattern.Length; ++x)
            {
                if (FormatPattern[x] == EscapeChar)
                {
                    ++x;
                    ReturnValue.Append(FormatPattern[x]);
                }
                else
                {
                    char NextValue=char.MinValue;
                    Input=GetMatchingInput(Input,FormatPattern[x],out NextValue);
                    if(NextValue!=char.MinValue)
                        ReturnValue.Append(NextValue);
                }
            }
            return ReturnValue.ToString();
        }

        #endregion

        #region GetMatchingInput

        /// <summary>
        /// Gets matching input
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="FormatChar">Current format character</param>
        /// <param name="MatchChar">The matching character found</param>
        /// <returns>The remainder of the input string left</returns>
        protected virtual string GetMatchingInput(string Input, char FormatChar,out char MatchChar)
        {
            bool Digit=FormatChar==DigitChar;
            bool Alpha=FormatChar==AlphaChar;
            if (!Digit && !Alpha)
            {
                MatchChar = FormatChar;
                return Input;
            }
            int Index=0;
            MatchChar = char.MinValue;
            for (int x = 0; x < Input.Length; ++x)
            {
                if ((Digit && char.IsDigit(Input[x])) || (Alpha && char.IsLetter(Input[x])))
                {
                    MatchChar = Input[x];
                    Index = x+1;
                    break;
                }
            }
            return Input.Substring(Index);
        }

        #endregion

        #region IsValid

        /// <summary>
        /// Checks if the format pattern is valid
        /// </summary>
        /// <param name="FormatPattern">Format pattern</param>
        /// <returns>Returns true if it's valid, otherwise false</returns>
        protected virtual bool IsValid(string FormatPattern)
        {
            bool EscapeCharFound = false;
            for (int x = 0; x < FormatPattern.Length; ++x)
            {
                if (EscapeCharFound && FormatPattern[x] != DigitChar
                        && FormatPattern[x] != AlphaChar
                        && FormatPattern[x] != EscapeChar)
                    return false;
                else if (EscapeCharFound)
                    EscapeCharFound = false;
                else if (FormatPattern[x] == EscapeChar)
                    EscapeCharFound = true;
            }
            if (EscapeCharFound)
                return false;
            return true;
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Represents digits (defaults to #)
        /// </summary>
        public virtual char DigitChar { get; protected set; }

        /// <summary>
        /// Represents alpha characters (defaults to @)
        /// </summary>
        public virtual char AlphaChar { get; protected set; }

        /// <summary>
        /// Represents the escape character (defaults to \)
        /// </summary>
        public virtual char EscapeChar { get; protected set; }

        #endregion
    }
}