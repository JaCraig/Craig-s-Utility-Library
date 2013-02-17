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
using System.Globalization;
#endregion

namespace Utilities.Web.Email.MIME.CodeTypes
{
    /// <summary>
    /// Default base coder
    /// </summary>
    public class CodeBase:Code
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public CodeBase()
        {
        }
        #endregion

        #region Public Overridden Functions

        /// <summary>
        /// Decodes a string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Output">Output string</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", MessageId = "System.String.IndexOf(System.String,System.Int32,System.StringComparison)")]
        public override void Decode(string Input, out string Output)
        {
            Output = "";
            int Index = 0;
            while(Index<Input.Length)
            {
                int CurrentIndex = Input.IndexOf("=?", Index, StringComparison.InvariantCulture);
                if(CurrentIndex!=-1)
                {
                    Output+=Input.Substring(Index,CurrentIndex-Index);
                    int CurrentIndex2 = Input.IndexOf("?=", CurrentIndex + 2, StringComparison.InvariantCulture);
                    if(CurrentIndex2!=-1)
                    {
                        CurrentIndex+=2;
                        int CurrentIndex3 = Input.IndexOf('?', CurrentIndex);
                        if(CurrentIndex3!=-1&&Input[CurrentIndex3+2]=='?')
                        {
                            CharacterSet=Input.Substring(CurrentIndex,CurrentIndex3-CurrentIndex);
                            string DECString=Input.Substring(CurrentIndex3+3,CurrentIndex2-CurrentIndex3-3);
                            if(Input[CurrentIndex3+1]=='Q')
                            {
                                Code TempCode=CodeManager.Instance["quoted-printable"];
                                TempCode.CharacterSet=CharacterSet;
                                string TempString="";
                                TempCode.Decode(DECString,out TempString);
                                Output+=TempString;
                            }
                            else if(Input[CurrentIndex3+1]=='B')
                            {
                                Code TempCode=CodeManager.Instance["base64"];
                                TempCode.CharacterSet=CharacterSet;
                                string TempString="";
                                TempCode.Decode(DECString,out TempString);
                                Output+=TempString;
                            }
                            else
                            {
                                Output+=DECString;
                            }
                        }
                        else
                        {
                            Output+=Input.Substring(CurrentIndex3,CurrentIndex2-CurrentIndex3);
                        }
                        Index=CurrentIndex2+2;
                    }
                    else
                    {
                        Output+=Input.Substring(CurrentIndex,Input.Length-CurrentIndex);
                        break;
                    }
                }
                else
                {
                    Output+=Input.Substring(Index,Input.Length-Index);
                    break;
                }
            }
        }

        /// <summary>
        /// Encodes a string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>encoded string</returns>
        public override string Encode(string Input)
        {
            StringBuilder Builder = new StringBuilder();
            if (DelimeterNeeded)
            {
                Builder.Append(EncodeDelimeter(Input));
            }
            else
            {
                Builder.Append(EncodeNoDelimeter(Input));
            }

            if (IsAutoFold)
            {
                string[] FoldCharacters = this.FoldCharacters;
                foreach (string FoldCharacter in FoldCharacters)
                {
                    string NewFoldString = FoldCharacter + "\r\n\t";
                    Builder.Replace(FoldCharacter, NewFoldString);
                }
            }
            return Builder.ToString();
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// Fold characters
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        protected virtual string[] FoldCharacters
        {
            get { return null; }
        }

        /// <summary>
        /// Is folding used
        /// </summary>
        protected virtual bool IsAutoFold
        {
            get { return false; }
        }

        /// <summary>
        /// Are delimeter's needed
        /// </summary>
        protected virtual bool DelimeterNeeded
        {
            get { return false; }
        }

        /// <summary>
        /// Delimeter characters
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        protected virtual char[] DelimeterCharacters
        {
            get { return null; }
        }
        #endregion

        #region Protected Functions
        /// <summary>
        /// Encodes a string based on delimeters specified
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>A string encoded based off of delimeters</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", MessageId = "System.String.Equals(System.String,System.StringComparison)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        protected string EncodeDelimeter(string Input)
        {
            StringBuilder Builder = new StringBuilder();
            char[] Filter = DelimeterCharacters;
            string[] InputArray = Input.Split(Filter);
            int Index = 0;
            foreach (string TempString in InputArray)
            {
                if (TempString != null)
                {
                    Index += TempString.Length;
                    if (string.IsNullOrEmpty(CharacterSet))
                    {
                        CharacterSet = System.Text.Encoding.Default.BodyName;
                    }
                    string EncodingUsing = SelectEncoding(Input).ToLower(CultureInfo.InvariantCulture);
                    if (EncodingUsing.Equals("non", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Builder.Append(TempString);
                    }
                    else if (EncodingUsing.Equals("base64", StringComparison.InvariantCultureIgnoreCase)
                        || EncodingUsing.Equals("quoted-printable", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Code TempCode = CodeManager.Instance[EncodingUsing];
                        TempCode.CharacterSet = CharacterSet;
                        Builder.AppendFormat("=?{0}?Q?{1}?=", CharacterSet, TempCode.Encode(TempString));
                    }
                    if (Index < Input.Length)
                        Builder.Append(Input.Substring(Index, 1));
                    ++Index;
                }
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Encodes a string without the use of delimeters
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>An encoded string</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", MessageId = "System.String.Equals(System.String,System.StringComparison)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        protected string EncodeNoDelimeter(string Input)
        {
            StringBuilder Builder = new StringBuilder();
            if(string.IsNullOrEmpty(CharacterSet))
                CharacterSet = System.Text.Encoding.Default.BodyName;

            string EncodingUsing = SelectEncoding(Input).ToLower(CultureInfo.InvariantCulture);
            if (EncodingUsing.Equals("non", StringComparison.InvariantCultureIgnoreCase))
            {
                Builder.Append(Input);
            }
            else if (EncodingUsing.Equals("base64", StringComparison.InvariantCultureIgnoreCase)
                        || EncodingUsing.Equals("quoted-printable", StringComparison.InvariantCultureIgnoreCase))
            {
                Code TempCode = CodeManager.Instance[EncodingUsing];
                TempCode.CharacterSet = CharacterSet;
                Builder.AppendFormat("=?{0}?Q?{1}?=", CharacterSet, TempCode.Encode(Input));
            }
            return Builder.ToString();
        }

        /// <summary>
        /// Selects an encoding type
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>A string containing the encoding type that should be used</returns>
        protected static string SelectEncoding(string Input)
        {
            int NumberOfNonASCII = 0;
            for (int x = 0; x < Input.Length; ++x)
            {
                if (IsNonASCIICharacter(Input[x]))
                    ++NumberOfNonASCII;
            }
            if (NumberOfNonASCII == 0)
                return "non";
            int QuotableSize = Input.Length + NumberOfNonASCII * 2;
            int Base64Size = (Input.Length + 2) / 3 * 4;
            return (QuotableSize <= Base64Size || NumberOfNonASCII * 5 <= Input.Length) ? "quoted-printable" : "base64";
        }

        /// <summary>
        /// Determines if this is a non ASCII character (greater than 255)
        /// </summary>
        /// <param name="Input"></param>
        /// <returns>True if it is, false otherwise</returns>
        protected static bool IsNonASCIICharacter(char Input)
        {
            return (int)Input > 255;
        }
        #endregion
    }
}