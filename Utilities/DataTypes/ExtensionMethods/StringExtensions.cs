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
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.DataTypes.Formatters;
using Utilities.DataTypes.Formatters.Interfaces;
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// String and StringBuilder extensions
    /// </summary>
    public static class StringExtensions
    {
        #region Functions
        
        #region AppendLineFormat

        /// <summary>
        /// Does an AppendFormat and then an AppendLine on the StringBuilder
        /// </summary>
        /// <param name="Builder">Builder object</param>
        /// <param name="Format">Format string</param>
        /// <param name="Objects">Objects to format</param>
        /// <returns>The StringBuilder passed in</returns>
        public static StringBuilder AppendLineFormat(this StringBuilder Builder, string Format, params object[] Objects)
        {
            return Builder.AppendFormat(CultureInfo.InvariantCulture, Format, Objects).AppendLine();
        }

        #endregion

        #region Center

        /// <summary>
        /// Centers the input string (if it's longer than the length) and pads it using the padding string
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="Length"></param>
        /// <param name="Padding"></param>
        /// <returns>The centered string</returns>
        public static string Center(this string Input,int Length,string Padding=" ")
        {
            if (string.IsNullOrEmpty(Input))
                Input = "";
            string Output = "";
            for (int x = 0; x < (Length - Input.Length) / 2; ++x)
            {
                Output += Padding[x % Padding.Length];
            }
            Output += Input;
            for (int x = 0; x < (Length - Input.Length) / 2; ++x)
            {
                Output += Padding[x % Padding.Length];
            }
            return Output;
        }

        #endregion

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
            OriginalEncodingUsing = OriginalEncodingUsing.Check(new ASCIIEncoding());
            EncodingUsing = EncodingUsing.Check(new UTF8Encoding());
            return Encoding.Convert(OriginalEncodingUsing, EncodingUsing, Input.ToByteArray(OriginalEncodingUsing))
                           .ToEncodedString(EncodingUsing);
        }

        #endregion

        #region FromBase64

        /// <summary>
        /// Converts base 64 string based on the encoding passed in
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="EncodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <returns>string in the encoding format</returns>
        public static string FromBase64(this string Input, Encoding EncodingUsing)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            byte[] TempArray = Convert.FromBase64String(Input);
            return EncodingUsing.Check(()=>new UTF8Encoding()).GetString(TempArray);
        }

        /// <summary>
        /// Converts base 64 string to a byte array
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>A byte array equivalent of the base 64 string</returns>
        public static byte[] FromBase64(this string Input)
        {
            return string.IsNullOrEmpty(Input) ? new byte[0] : Convert.FromBase64String(Input);
        }

        #endregion

        #region Is

        /// <summary>
        /// Is this value of the specified type
        /// </summary>
        /// <param name="Value">Value to compare</param>
        /// <param name="ComparisonType">Comparison type</param>
        /// <returns>True if it is of the type specified, false otherwise</returns>
        public static bool Is(this string Value,StringCompare ComparisonType)
        {
            if (ComparisonType == StringCompare.CreditCard)
            {
                long CheckSum = 0;
                Value = Value.Replace("-", "").Reverse();
                for (int x = 0; x < Value.Length; ++x)
                {
                    if (!Value[x].IsDigit())
                        return false;
                    int TempValue = (Value[x] - '0') * (x % 2 == 1 ? 2 : 1);
                    while (TempValue > 0)
                    {
                        CheckSum += TempValue % 10;
                        TempValue /= 10;
                    }
                }
                return (CheckSum % 10) == 0;
            }
            if (ComparisonType == StringCompare.Unicode)
            {
                return string.IsNullOrEmpty(Value) ? true : Regex.Replace(Value, @"[^\u0000-\u007F]", "") != Value;
            }
            return Value.Is("", StringCompare.Anagram);
        }

        /// <summary>
        /// Is this value of the specified type
        /// </summary>
        /// <param name="Value1">Value 1 to compare</param>
        /// <param name="Value2">Value 2 to compare</param>
        /// <param name="ComparisonType">Comparison type</param>
        /// <returns>True if it is of the type specified, false otherwise</returns>
        public static bool Is(this string Value1, string Value2, StringCompare ComparisonType)
        {
            if (ComparisonType != StringCompare.Anagram)
                return Value1.Is(ComparisonType);
            return new string(Value1.OrderBy(x => x).ToArray()) == new string(Value2.OrderBy(x => x).ToArray());
        }

        #endregion

        #region Keep

        /// <summary>
        /// Removes everything that is not in the filter text from the input.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Filter">Regex expression of text to keep</param>
        /// <returns>The input text minus everything not in the filter text.</returns>
        public static string Keep(this string Input, string Filter)
        {
            if (string.IsNullOrEmpty(Input) || string.IsNullOrEmpty(Filter))
                return "";
            Regex TempRegex = new Regex(Filter);
            MatchCollection Collection = TempRegex.Matches(Input);
            StringBuilder Builder = new StringBuilder();
            foreach (Match Match in Collection)
                Builder.Append(Match.Value);
            return Builder.ToString();
        }

        /// <summary>
        /// Removes everything that is not in the filter text from the input.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Filter">Predefined filter to use (can be combined as they are flags)</param>
        /// <returns>The input text minus everything not in the filter text.</returns>
        public static string Keep(this string Input, StringFilter Filter)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            string Value = BuildFilter(Filter);
            return Input.Keep(Value);
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
            return string.IsNullOrEmpty(Input) ? "" : Input.Substring(0, Input.Length > Length ? Length : Input.Length);
        }

        #endregion

        #region LevenshteinDistance

        /// <summary>
        /// Calculates the Levenshtein distance
        /// </summary>
        /// <param name="Value1">Value 1</param>
        /// <param name="Value2">Value 2</param>
        /// <returns>The Levenshtein distance</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body")]
        public static int LevenshteinDistance(this string Value1, string Value2)
        {
            int[,] Matrix = new int[Value1.Length + 1, Value2.Length + 1];
            for (int x = 0; x <= Value1.Length; ++x)
                Matrix[x, 0] = x;
            for (int x = 0; x <= Value2.Length; ++x)
                Matrix[0, x] = x;

            for (int x = 1; x <= Value1.Length; ++x)
            {
                for (int y = 1; y <= Value2.Length; ++y)
                {
                    int Cost = Value1[x - 1] == Value2[y - 1] ? 0 : 1;
                    Matrix[x, y] = new int[] { Matrix[x - 1, y] + 1, Matrix[x, y - 1] + 1, Matrix[x - 1, y - 1] + Cost }.Min();
                    if (x > 1 && y > 1 && Value1[x - 1] == Value2[y - 2] && Value1[x - 2] == Value2[y - 1])
                        Matrix[x, y] = new int[] { Matrix[x, y], Matrix[x - 2, y - 2] + Cost }.Min();
                }
            }

            return Matrix[Value1.Length, Value2.Length];
        }        

        #endregion

        #region MaskLeft

        /// <summary>
        /// Masks characters to the left ending at a specific character
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="EndPosition">End position (counting from the left)</param>
        /// <param name="Mask">Mask character to use</param>
        /// <returns>The masked string</returns>
        public static string MaskLeft(this string Input, int EndPosition = 4, char Mask = '#')
        {
            string Appending = "";
            for (int x = 0; x < EndPosition; ++x)
                Appending += Mask;
            return Appending + Input.Remove(0, EndPosition);
        }

        #endregion

        #region MaskRight

        /// <summary>
        /// Masks characters to the right starting at a specific character
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="StartPosition">Start position (counting from the left)</param>
        /// <param name="Mask">Mask character to use</param>
        /// <returns>The masked string</returns>
        public static string MaskRight(this string Input, int StartPosition = 4, char Mask = '#')
        {
            if (StartPosition > Input.Length)
                return Input;
            string Appending = "";
            for (int x = 0; x < Input.Length - StartPosition; ++x)
                Appending += Mask;
            return Input.Remove(StartPosition) + Appending;
        }

        #endregion

        #region NextSequence

        /// <summary>
        /// Function that is useful for generating a string in a series. so a becomes b, b becomes c, etc. 
        /// and after hitting the max character, it goes to two characters (so ~ becomes aa, then ab, ac, etc).
        /// </summary>
        /// <param name="Sequence">Current sequence</param>
        /// <param name="Min">Min character</param>
        /// <param name="Max">Max character</param>
        /// <returns>The next item in the sequence</returns>
        public static string NextSequence(this string Sequence, char Min = ' ', char Max = '~')
        {
            byte[] Values = Sequence.ToByteArray();
            byte MaxValue = (byte)Max;
            byte Remainder = 1;
            for (int x = Sequence.Length - 1; x >= 0; --x)
            {
                Values[x] += Remainder;
                Remainder = 0;
                if (Values[x] > MaxValue)
                {
                    Remainder = 1;
                    Values[x] = (byte)Min;
                }
                else
                    break;
            }
            if (Remainder == 1)
                return Min + Values.ToEncodedString();
            return Values.ToEncodedString();
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
            return string.IsNullOrEmpty(Input) ? 0 : new Regex(Match).Matches(Input).Count;
        }

        #endregion

        #region Pluralize

        /// <summary>
        /// Pluralizes a word
        /// </summary>
        /// <param name="Word">Word to pluralize</param>
        /// <param name="Culture">Culture info used to pluralize the word (defaults to current culture)</param>
        /// <returns>The word pluralized</returns>
        public static string Pluralize(this string Word, CultureInfo Culture = null)
        {
            if (string.IsNullOrEmpty(Word))
                return "";
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            return PluralizationService.CreateService(Culture).Pluralize(Word);
        }

        #endregion
                
        #region Remove

        /// <summary>
        /// Removes everything that is in the filter text from the input.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Filter">Regex expression of text to remove</param>
        /// <returns>Everything not in the filter text.</returns>
        public static string Remove(this string Input, string Filter)
        {
            if (string.IsNullOrEmpty(Input) || string.IsNullOrEmpty(Filter))
                return Input;
            return new Regex(Filter).Replace(Input,"");
        }

        /// <summary>
        /// Removes everything that is in the filter text from the input.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Filter">Predefined filter to use (can be combined as they are flags)</param>
        /// <returns>Everything not in the filter text.</returns>
        public static string Remove(this string Input, StringFilter Filter)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            string Value = BuildFilter(Filter);
            return Input.Remove(Value);
        }

        #endregion

        #region Replace
        
        /// <summary>
        /// Replaces everything that is in the filter text with the value specified.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Value">Value to fill in</param>
        /// <param name="Filter">Predefined filter to use (can be combined as they are flags)</param>
        /// <returns>The input text with the various items replaced</returns>
        public static string Replace(this string Input, StringFilter Filter, string Value = "")
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            string FilterValue = BuildFilter(Filter);
            return new Regex(FilterValue).Replace(Input, Value);
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

        #region Singularize

        /// <summary>
        /// Singularizes a word
        /// </summary>
        /// <param name="Word">Word to singularize</param>
        /// <param name="Culture">Culture info used to singularize the word (defaults to current culture)</param>
        /// <returns>The word singularized</returns>
        public static string Singularize(this string Word, CultureInfo Culture = null)
        {
            if (string.IsNullOrEmpty(Word))
                return "";
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            return PluralizationService.CreateService(Culture).Singularize(Word);
        }

        #endregion

        #region StripLeft

        /// <summary>
        /// Strips out any of the characters specified starting on the left side of the input string (stops when a character not in the list is found)
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Characters">Characters to strip (defaults to a space)</param>
        /// <returns>The Input string with specified characters stripped out</returns>
        public static string StripLeft(this string Input, string Characters = " ")
        {
            if (string.IsNullOrEmpty(Input))
                return Input;
            if (string.IsNullOrEmpty(Characters))
                return Input;
            return Input.SkipWhile(x => Characters.Contains(x)).ToString(x => x.ToString(), "");
        }

        #endregion

        #region StripRight

        /// <summary>
        /// Strips out any of the characters specified starting on the right side of the input string (stops when a character not in the list is found)
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Characters">Characters to strip (defaults to a space)</param>
        /// <returns>The Input string with specified characters stripped out</returns>
        public static string StripRight(this string Input, string Characters = " ")
        {
            if (string.IsNullOrEmpty(Input))
                return Input;
            if (string.IsNullOrEmpty(Characters))
                return Input;
            int Position = Input.Length - 1;
            for (int x = Input.Length - 1; x >= 0; --x)
            {
                if (!Characters.Contains(Input[x]))
                {
                    Position = x + 1;
                    break;
                }
            }
            return Input.Left(Position);
        }

        #endregion

        #region StripIllegalXML

        /// <summary>
        /// Strips illegal characters for XML content
        /// </summary>
        /// <param name="Content">Content</param>
        /// <returns>The stripped string</returns>
        public static string StripIllegalXML(this string Content)
        {
            if (string.IsNullOrEmpty(Content))
                return "";
            StringBuilder Builder = new StringBuilder();
            foreach (char Char in Content)
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
            byte[] TempArray = OriginalEncodingUsing.Check(new UTF8Encoding()).GetBytes(Input);
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
            return string.IsNullOrEmpty(Input) ? null : EncodingUsing.Check(new UTF8Encoding()).GetBytes(Input);
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
                    InputChars[x] = char.ToUpper(InputChars[x], CultureInfo.InvariantCulture);
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

        #region ToString

        /// <summary>
        /// Formats a string based on a format string passed in.
        /// The default formatter uses the following format:
        /// # = digits
        /// @ = alpha characters
        /// \ = escape char
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Format">Format of the output string</param>
        /// <param name="Provider">String formatter provider (defaults to GenericStringFormatter)</param>
        /// <returns>The formatted string</returns>
        public static string ToString(this string Input, string Format, IStringFormatter Provider = null)
        {
            return Provider.Check(new GenericStringFormatter()).Format(Input, Format);
        }

        /// <summary>
        /// Formats a string based on the object's properties
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Object">Object to use to format the string</param>
        /// <param name="StartSeperator">Seperator character/string to use to describe the start of the property name</param>
        /// <param name="EndSeperator">Seperator character/string to use to describe the end of the property name</param>
        /// <returns>The formatted string</returns>
        public static string ToString(this string Input, object Object, string StartSeperator = "{", string EndSeperator = "}")
        {
            if (Object == null)
                return Input;
            Object.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanRead)
                .ForEach(x =>
                {
                    var Value = x.GetValue(Object, null);
                    Input = Input.Replace(StartSeperator + x.Name + EndSeperator, Value == null ? "" : Value.ToString());
                });
            return Input;
        }

        /// <summary>
        /// Formats a string based on the key/value pairs that are sent in
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Pairs">Key/value pairs. Replaces the key with the corresponding value.</param>
        /// <returns>The string after the changes have been made</returns>
        public static string ToString(this string Input, params KeyValuePair<string, string>[] Pairs)
        {
            if (string.IsNullOrEmpty(Input))
                return Input;
            foreach (KeyValuePair<string, string> Pair in Pairs)
            {
                Input = Input.Replace(Pair.Key, Pair.Value);
            }
            return Input;
        }

        /// <summary>
        /// Uses a regex to format the input string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Format">Regex string used to</param>
        /// <param name="OutputFormat">Output format</param>
        /// <param name="Options">Regex options</param>
        /// <returns>The input string formatted by using the regex string</returns>
        public static string ToString(this string Input, string Format, string OutputFormat, RegexOptions Options = RegexOptions.None)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(Input), "Input");
            return Regex.Replace(Input, Format, OutputFormat, Options);
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
                    && InputStrings[x].Length > 3)
                {
                    Regex TempRegex = new Regex(InputStrings[x].Replace(")", @"\)").Replace("(", @"\(").Replace("*", @"\*"));
                    InputStrings[x] = InputStrings[x].ToFirstCharacterUpperCase();
                    Input = TempRegex.Replace(Input, InputStrings[x]);
                }
            }
            return Input;
        }

        #endregion

        #endregion

        #region Private Functions

        private static string BuildFilter(StringFilter Filter)
        {
            string FilterValue = "";
            string Separator = "";
            if (Filter.HasFlag(StringFilter.Alpha))
            {
                FilterValue += Separator + "[a-zA-Z]";
                Separator = "|";
            }
            if (Filter.HasFlag(StringFilter.Numeric))
            {
                FilterValue += Separator + "[0-9]";
                Separator = "|";
            }
            if (Filter.HasFlag(StringFilter.FloatNumeric))
            {
                FilterValue += Separator + @"[0-9\.]";
                Separator = "|";
            }
            if (Filter.HasFlag(StringFilter.ExtraSpaces))
            {
                FilterValue += Separator + @"[ ]{2,}";
                Separator = "|";
            }
            return FilterValue;
        }

        #endregion
    }

    #region Enums

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

    #endregion
}