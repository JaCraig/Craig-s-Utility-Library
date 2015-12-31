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
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.DataTypes.Formatters;
using Utilities.DataTypes.Formatters.Interfaces;

namespace Utilities.DataTypes
{
    /// <summary>
    /// What sort of string capitalization should be used?
    /// </summary>
    public enum StringCase
    {
        /// <summary>
        /// Sentence capitalization
        /// </summary>
        SentenceCapitalize,

        /// <summary>
        /// First character upper case
        /// </summary>
        FirstCharacterUpperCase,

        /// <summary>
        /// Title case
        /// </summary>
        TitleCase
    }

    /// <summary>
    /// What type of string comparison are we doing?
    /// </summary>
    public enum StringCompare
    {
        /// <summary>
        /// Is this a credit card number?
        /// </summary>
        CreditCard,

        /// <summary>
        /// Is this an anagram?
        /// </summary>
        Anagram,

        /// <summary>
        /// Is this Unicode
        /// </summary>
        Unicode
    }

    /// <summary>
    /// Predefined filters
    /// </summary>
    [Flags]
    public enum StringFilter
    {
        /// <summary>
        /// Alpha characters
        /// </summary>
        Alpha = 1,

        /// <summary>
        /// Numeric characters
        /// </summary>
        Numeric = 2,

        /// <summary>
        /// Numbers with period, basically allows for decimal point
        /// </summary>
        FloatNumeric = 4,

        /// <summary>
        /// Multiple spaces
        /// </summary>
        ExtraSpaces = 8
    }

    /// <summary>
    /// String and StringBuilder extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StringExtensions
    {
        /// <summary>
        /// Does an AppendFormat and then an AppendLine on the StringBuilder
        /// </summary>
        /// <param name="builder">Builder object</param>
        /// <param name="format">Format string</param>
        /// <param name="objects">Objects to format</param>
        /// <param name="provider">Format provider (CultureInfo) to use</param>
        /// <returns>The StringBuilder passed in</returns>
        public static StringBuilder AppendLineFormat(this StringBuilder builder, IFormatProvider provider, string format, params object[] objects)
        {
            if (builder == null || string.IsNullOrEmpty(format))
                return builder;
            objects = objects ?? new object[0];
            provider = provider ?? CultureInfo.InvariantCulture;
            return builder.AppendFormat(provider, format, objects).AppendLine();
        }

        /// <summary>
        /// Does an AppendFormat and then an AppendLine on the StringBuilder
        /// </summary>
        /// <param name="builder">Builder object</param>
        /// <param name="format">Format string</param>
        /// <param name="objects">Objects to format</param>
        /// <returns>The StringBuilder passed in</returns>
        public static StringBuilder AppendLineFormat(this StringBuilder builder, string format, params object[] objects)
        {
            return builder.AppendLineFormat(CultureInfo.InvariantCulture, format, objects);
        }

        /// <summary>
        /// Centers the input string (if it's longer than the length) and pads it using the padding string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <param name="padding"></param>
        /// <returns>The centered string</returns>
        public static string Center(this string input, int length, string padding = " ")
        {
            if (string.IsNullOrEmpty(input))
                input = "";
            string Output = "";
            for (int x = 0; x < (length - input.Length) / 2; ++x)
            {
                Output += padding[x % padding.Length];
            }
            Output += input;
            for (int x = 0; x < (length - input.Length) / 2; ++x)
            {
                Output += padding[x % padding.Length];
            }
            return Output;
        }

        /// <summary>
        /// Converts a string to a string of another encoding
        /// </summary>
        /// <param name="input">input string</param>
        /// <param name="originalEncodingUsing">
        /// The type of encoding the string is currently using (defaults to ASCII)
        /// </param>
        /// <param name="encodingUsing">
        /// The type of encoding the string is converted into (defaults to UTF8)
        /// </param>
        /// <returns>string of the byte array</returns>
        public static string Encode(this string input, Encoding originalEncodingUsing = null, Encoding encodingUsing = null)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            originalEncodingUsing = originalEncodingUsing ?? Encoding.ASCII;
            encodingUsing = encodingUsing ?? Encoding.UTF8;
            return Encoding.Convert(originalEncodingUsing, encodingUsing, input.ToByteArray(originalEncodingUsing))
                           .ToString(encodingUsing);
        }

        /// <summary>
        /// Converts base 64 string based on the encoding passed in
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="encodingUsing">
        /// The type of encoding the string is using (defaults to UTF8)
        /// </param>
        /// <returns>string in the encoding format</returns>
        public static string FromBase64(this string input, Encoding encodingUsing)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            byte[] TempArray = Convert.FromBase64String(input);
            encodingUsing = encodingUsing ?? Encoding.UTF8;
            return encodingUsing.GetString(TempArray);
        }

        /// <summary>
        /// Converts base 64 string to a byte array
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>A byte array equivalent of the base 64 string</returns>
        public static byte[] FromBase64(this string input)
        {
            return string.IsNullOrEmpty(input) ? new byte[0] : Convert.FromBase64String(input);
        }

        /// <summary>
        /// Is this value of the specified type
        /// </summary>
        /// <param name="value">Value to compare</param>
        /// <param name="comparisonType">Comparison type</param>
        /// <returns>True if it is of the type specified, false otherwise</returns>
        public static bool Is(this string value, StringCompare comparisonType)
        {
            if (comparisonType == StringCompare.CreditCard)
            {
                long CheckSum = 0;
                value = value.Replace("-", "").Reverse();
                for (int x = 0; x < value.Length; ++x)
                {
                    if (!value[x].Is(CharIs.Digit))
                        return false;
                    int Tempvalue = (value[x] - '0') * (x % 2 == 1 ? 2 : 1);
                    while (Tempvalue > 0)
                    {
                        CheckSum += Tempvalue % 10;
                        Tempvalue /= 10;
                    }
                }
                return (CheckSum % 10) == 0;
            }
            if (comparisonType == StringCompare.Unicode)
            {
                return string.IsNullOrEmpty(value) || Regex.Replace(value, @"[^\u0000-\u007F]", "") != value;
            }
            return value.Is("", StringCompare.Anagram);
        }

        /// <summary>
        /// Is this value of the specified type
        /// </summary>
        /// <param name="value1">Value 1 to compare</param>
        /// <param name="value2">Value 2 to compare</param>
        /// <param name="comparisonType">Comparison type</param>
        /// <returns>True if it is of the type specified, false otherwise</returns>
        public static bool Is(this string value1, string value2, StringCompare comparisonType)
        {
            if (comparisonType != StringCompare.Anagram)
                return value1.Is(comparisonType);
            return new string(value1.OrderBy(x => x).ToArray()) == new string(value2.OrderBy(x => x).ToArray());
        }

        /// <summary>
        /// Removes everything that is not in the filter text from the input.
        /// </summary>
        /// <param name="input">Input text</param>
        /// <param name="filter">Regex expression of text to keep</param>
        /// <returns>The input text minus everything not in the filter text.</returns>
        public static string Keep(this string input, string filter)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(filter))
                return "";
            var TempRegex = new Regex(filter);
            MatchCollection Collection = TempRegex.Matches(input);
            var Builder = new StringBuilder();
            foreach (Match Match in Collection)
                Builder.Append(Match.Value);
            return Builder.ToString();
        }

        /// <summary>
        /// Removes everything that is not in the filter text from the input.
        /// </summary>
        /// <param name="input">Input text</param>
        /// <param name="filter">Predefined filter to use (can be combined as they are flags)</param>
        /// <returns>The input text minus everything not in the filter text.</returns>
        public static string Keep(this string input, StringFilter filter)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            string Value = BuildFilter(filter);
            return input.Keep(Value);
        }

        /// <summary>
        /// Gets the first x number of characters from the left hand side
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="length">x number of characters to return</param>
        /// <returns>The resulting string</returns>
        public static string Left(this string input, int length)
        {
            if (length <= 0)
                return "";
            return string.IsNullOrEmpty(input) ? "" : input.Substring(0, input.Length > length ? length : input.Length);
        }

        /// <summary>
        /// Calculates the Levenshtein distance
        /// </summary>
        /// <param name="value1">Value 1</param>
        /// <param name="value2">Value 2</param>
        /// <returns>The Levenshtein distance (-1 if null values found)</returns>
        public static int LevenshteinDistance(this string value1, string value2)
        {
            if (value1 == null || value2 == null)
                return -1;
            int[,] Matrix = new int[value1.Length + 1, value2.Length + 1];
            for (int x = 0; x <= value1.Length; ++x)
                Matrix[x, 0] = x;
            for (int x = 0; x <= value2.Length; ++x)
                Matrix[0, x] = x;
            for (int x = 1; x <= value1.Length; ++x)
            {
                for (int y = 1; y <= value2.Length; ++y)
                {
                    int Cost = value1[x - 1] == value2[y - 1] ? 0 : 1;
                    Matrix[x, y] = new int[] { Matrix[x - 1, y] + 1, Matrix[x, y - 1] + 1, Matrix[x - 1, y - 1] + Cost }.Min();
                    if (x > 1 && y > 1 && value1[x - 1] == value2[y - 2] && value1[x - 2] == value2[y - 1])
                        Matrix[x, y] = new int[] { Matrix[x, y], Matrix[x - 2, y - 2] + Cost }.Min();
                }
            }
            return Matrix[value1.Length, value2.Length];
        }

        /// <summary>
        /// Masks characters to the left ending at a specific character
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="endPosition">End position (counting from the left)</param>
        /// <param name="mask">Mask character to use</param>
        /// <returns>The masked string</returns>
        public static string MaskLeft(this string input, int endPosition = 4, char mask = '#')
        {
            string Appending = "";
            for (int x = 0; x < endPosition; ++x)
                Appending += mask;
            return Appending + input.Remove(0, endPosition);
        }

        /// <summary>
        /// Masks characters to the right starting at a specific character
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="startPosition">Start position (counting from the left)</param>
        /// <param name="mask">Mask character to use</param>
        /// <returns>The masked string</returns>
        public static string MaskRight(this string input, int startPosition = 4, char mask = '#')
        {
            if (string.IsNullOrEmpty(input))
                return "";
            if (startPosition > input.Length)
                return input;
            string Appending = "";
            for (int x = 0; x < input.Length - startPosition; ++x)
                Appending += mask;
            return input.Remove(startPosition) + Appending;
        }

        /// <summary>
        /// returns the number of times a string occurs within the text
        /// </summary>
        /// <param name="input">input text</param>
        /// <param name="match">The string to match (can be regex)</param>
        /// <returns>The number of times the string occurs</returns>
        public static int NumberTimesOccurs(this string input, string match)
        {
            return string.IsNullOrEmpty(input) ? 0 : new Regex(match).Matches(input).Count;
        }

        /// <summary>
        /// Removes everything that is in the filter text from the input.
        /// </summary>
        /// <param name="input">Input text</param>
        /// <param name="filter">Regex expression of text to remove</param>
        /// <returns>Everything not in the filter text.</returns>
        public static string Remove(this string input, string filter)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(filter))
                return input;
            return new Regex(filter).Replace(input, "");
        }

        /// <summary>
        /// Removes everything that is in the filter text from the input.
        /// </summary>
        /// <param name="input">Input text</param>
        /// <param name="filter">Predefined filter to use (can be combined as they are flags)</param>
        /// <returns>Everything not in the filter text.</returns>
        public static string Remove(this string input, StringFilter filter)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            string Value = BuildFilter(filter);
            return input.Remove(Value);
        }

        /// <summary>
        /// Replaces everything that is in the filter text with the value specified.
        /// </summary>
        /// <param name="input">Input text</param>
        /// <param name="value">Value to fill in</param>
        /// <param name="filter">Predefined filter to use (can be combined as they are flags)</param>
        /// <returns>The input text with the various items replaced</returns>
        public static string Replace(this string input, StringFilter filter, string value = "")
        {
            if (string.IsNullOrEmpty(input))
                return "";
            string FilterValue = BuildFilter(filter);
            return new Regex(FilterValue).Replace(input, value);
        }

        /// <summary>
        /// Reverses a string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>The reverse of the input string</returns>
        public static string Reverse(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            return new string(input.Reverse<char>().ToArray());
        }

        /// <summary>
        /// Gets the last x number of characters from the right hand side
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="length">x number of characters to return</param>
        /// <returns>The resulting string</returns>
        public static string Right(this string input, int length)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            if (length <= 0)
                return "";
            length = input.Length > length ? length : input.Length;
            return input.Substring(input.Length - length, length);
        }

        /// <summary>
        /// Strips illegal characters for XML content
        /// </summary>
        /// <param name="content">Content</param>
        /// <returns>The stripped string</returns>
        public static string StripIllegalXML(this string content)
        {
            if (string.IsNullOrEmpty(content))
                return "";
            var Builder = new StringBuilder();
            foreach (char Char in content)
            {
                if (Char == 0x9
                    || Char == 0xA
                    || Char == 0xD
                    || (Char >= 0x20 && Char <= 0xD7FF)
                    || (Char >= 0xE000 && Char <= 0xFFFD))
                    Builder.Append(Char);
            }
            return Builder.ToString().Replace('\u2013', '-').Replace('\u2014', '-')
                .Replace('\u2015', '-').Replace('\u2017', '_').Replace('\u2018', '\'')
                .Replace('\u2019', '\'').Replace('\u201a', ',').Replace('\u201b', '\'')
                .Replace('\u201c', '\"').Replace('\u201d', '\"').Replace('\u201e', '\"')
                .Replace("\u2026", "...").Replace('\u2032', '\'').Replace('\u2033', '\"')
                .Replace("`", "\'")
                .Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;")
                .Replace("\"", "&quot;").Replace("\'", "&apos;");
        }

        /// <summary>
        /// Strips out any of the characters specified starting on the left side of the input string
        /// (stops when a character not in the list is found)
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="characters">Characters to strip (defaults to a space)</param>
        /// <returns>The Input string with specified characters stripped out</returns>
        public static string StripLeft(this string input, string characters = " ")
        {
            if (string.IsNullOrEmpty(input))
                return input;
            if (string.IsNullOrEmpty(characters))
                return input;
            return input.SkipWhile(x => characters.Contains(x)).ToString(x => x.ToString(), "");
        }

        /// <summary>
        /// Strips out any of the characters specified starting on the right side of the input
        /// string (stops when a character not in the list is found)
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="characters">Characters to strip (defaults to a space)</param>
        /// <returns>The Input string with specified characters stripped out</returns>
        public static string StripRight(this string input, string characters = " ")
        {
            if (string.IsNullOrEmpty(input))
                return input;
            if (string.IsNullOrEmpty(characters))
                return input;
            int Position = input.Length - 1;
            for (int x = input.Length - 1; x >= 0; --x)
            {
                if (!characters.Contains(input[x]))
                {
                    Position = x + 1;
                    break;
                }
            }
            return input.Left(Position);
        }

        /// <summary>
        /// Converts from the specified encoding to a base 64 string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="options">Base 64 formatting options</param>
        /// <param name="originalEncodingUsing">
        /// The type of encoding the string is using (defaults to UTF8)
        /// </param>
        /// <returns>Bas64 string</returns>
        public static string ToBase64String(this string input, Encoding originalEncodingUsing = null)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            originalEncodingUsing = originalEncodingUsing ?? Encoding.UTF8;
            byte[] TempArray = originalEncodingUsing.GetBytes(input);
            return Convert.ToBase64String(TempArray);
        }

        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="input">input string</param>
        /// <param name="encodingUsing">
        /// The type of encoding the string is using (defaults to UTF8)
        /// </param>
        /// <returns>the byte array representing the string</returns>
        public static byte[] ToByteArray(this string input, Encoding encodingUsing = null)
        {
            encodingUsing = encodingUsing ?? Encoding.UTF8;
            return string.IsNullOrEmpty(input) ? new byte[0] : encodingUsing.GetBytes(input);
        }

        /// <summary>
        /// Formats the string based on the capitalization specified
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="caseOfString">Capitalization type to use</param>
        /// <param name="provider">CultureInfo to use. Defaults to InvariantCulture.</param>
        /// <returns>Capitalizes the string based on the case specified</returns>
        public static string ToString(this string input, StringCase caseOfString, CultureInfo provider = null)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            provider = provider ?? CultureInfo.InvariantCulture;
            if (caseOfString == StringCase.FirstCharacterUpperCase)
            {
                char[] InputChars = input.ToCharArray();
                for (int x = 0; x < InputChars.Length; ++x)
                {
                    if (InputChars[x] != ' ' && InputChars[x] != '\t')
                    {
                        InputChars[x] = provider.TextInfo.ToUpper(InputChars[x]);
                        break;
                    }
                }
                return new string(InputChars);
            }
            if (caseOfString == StringCase.SentenceCapitalize)
            {
                string[] Seperator = { ".", "?", "!" };
                string[] InputStrings = input.Split(Seperator, StringSplitOptions.None);
                for (int x = 0; x < InputStrings.Length; ++x)
                {
                    if (!string.IsNullOrEmpty(InputStrings[x]))
                    {
                        var TempRegex = new Regex(InputStrings[x]);
                        InputStrings[x] = InputStrings[x].ToString(StringCase.FirstCharacterUpperCase);
                        input = TempRegex.Replace(input, InputStrings[x]);
                    }
                }
                return input;
            }
            if (caseOfString == StringCase.TitleCase)
            {
                string[] Seperator = { " ", ".", "\t", Environment.NewLine, "!", "?" };
                string[] InputStrings = input.Split(Seperator, StringSplitOptions.None);
                for (int x = 0; x < InputStrings.Length; ++x)
                {
                    if (!string.IsNullOrEmpty(InputStrings[x])
                        && InputStrings[x].Length > 3)
                    {
                        var TempRegex = new Regex(InputStrings[x].Replace(")", @"\)").Replace("(", @"\(").Replace("*", @"\*"));
                        InputStrings[x] = InputStrings[x].ToString(StringCase.FirstCharacterUpperCase);
                        input = TempRegex.Replace(input, InputStrings[x]);
                    }
                }
                return input;
            }
            return input;
        }

        /// <summary>
        /// Formats a string based on a format string passed in. The default formatter uses the
        /// following format: # = digits @ = alpha characters \ = escape char
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="format">Format of the output string</param>
        /// <param name="provider">String formatter provider (defaults to GenericStringFormatter)</param>
        /// <returns>The formatted string</returns>
        public static string ToString(this string input, string format, IStringFormatter provider = null)
        {
            provider = provider ?? new GenericStringFormatter();
            return provider.Format(input, format);
        }

        /// <summary>
        /// Formats a string based on the object's properties
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="inputObject">Object to use to format the string</param>
        /// <param name="startSeperator">
        /// Seperator character/string to use to describe the start of the property name
        /// </param>
        /// <param name="endSeperator">
        /// Seperator character/string to use to describe the end of the property name
        /// </param>
        /// <returns>The formatted string</returns>
        public static string ToString(this string input, object inputObject, string startSeperator = "{", string endSeperator = "}")
        {
            if (inputObject == null)
                return input;
            inputObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanRead)
                .ForEach(x =>
                {
                    var Value = x.GetValue(inputObject, null);
                    input = input.Replace(startSeperator + x.Name + endSeperator, Value == null ? "" : Value.ToString());
                });
            return input;
        }

        /// <summary>
        /// Formats a string based on the key/value pairs that are sent in
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="pairs">Key/value pairs. Replaces the key with the corresponding value.</param>
        /// <returns>The string after the changes have been made</returns>
        public static string ToString(this string input, params KeyValuePair<string, string>[] pairs)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            foreach (KeyValuePair<string, string> Pair in pairs)
            {
                input = input.Replace(Pair.Key, Pair.Value);
            }
            return input;
        }

        /// <summary>
        /// Uses a regex to format the input string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="format">Regex string used to</param>
        /// <param name="outputFormat">Output format</param>
        /// <param name="options">Regex options</param>
        /// <returns>The input string formatted by using the regex string</returns>
        public static string ToString(this string input, string format, string outputFormat, RegexOptions options = RegexOptions.None)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(format) || string.IsNullOrEmpty(outputFormat))
                return "";
            return Regex.Replace(input, format, outputFormat, options);
        }

        private static string BuildFilter(StringFilter filter)
        {
            string FilterValue = "";
            string Separator = "";
            if (filter.HasFlag(StringFilter.Alpha))
            {
                FilterValue += Separator + "[a-zA-Z]";
                Separator = "|";
            }
            if (filter.HasFlag(StringFilter.Numeric))
            {
                FilterValue += Separator + "[0-9]";
                Separator = "|";
            }
            if (filter.HasFlag(StringFilter.FloatNumeric))
            {
                FilterValue += Separator + @"[0-9\.]";
                Separator = "|";
            }
            if (filter.HasFlag(StringFilter.ExtraSpaces))
            {
                FilterValue += Separator + @"[ ]{2,}";
                Separator = "|";
            }
            return FilterValue;
        }
    }
}