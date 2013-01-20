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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.IO.ExtensionMethods;
using System.Diagnostics.Contracts;
#endregion

namespace Utilities.Web.ExtensionMethods
{
    /// <summary>
    /// Extensions dealing with minification of data
    /// </summary>
    public static class Minification
    {
        #region Combine

        /// <summary>
        /// Combines and minifies various files
        /// </summary>
        /// <param name="Input">input strings (file contents)</param>
        /// <param name="Type">Type of minification</param>
        /// <returns>A minified/packed string</returns>
        public static string Combine(this IEnumerable<string> Input, MinificationType Type = MinificationType.HTML)
        {
            StringBuilder Output = new StringBuilder();
            foreach (string Temp in Input)
                Output.Append(Temp).Append("\n");
            return Minify(Output.ToString(), Type);
        }

        /// <summary>
        /// Combines and minifies various files
        /// </summary>
        /// <param name="Input">input strings (file contents)</param>
        /// <param name="Type">Type of minification</param>
        /// <returns>A minified/packed string</returns>
        public static string Combine(this IEnumerable<FileInfo> Input, MinificationType Type = MinificationType.HTML)
        {
            StringBuilder Output = new StringBuilder();
            foreach (FileInfo Temp in Input.Where(x => x.Exists))
                Output.Append(Temp.Read()).Append("\n");
            return Minify(Output.ToString(), Type);
        }

        #endregion

        #region Minify

        /// <summary>
        /// Minifies the file based on the data type specified
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Type">Type of minification to run</param>
        /// <returns>A stripped file</returns>
        public static string Minify(this string Input, MinificationType Type = MinificationType.HTML)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            if (Type == MinificationType.CSS)
                return CSSMinify(Input);
            if (Type == MinificationType.JavaScript)
                return JavaScriptMinify(Input);
            return HTMLMinify(Input);
        }

        /// <summary>
        /// Minifies the file based on the data type specified
        /// </summary>
        /// <param name="Input">Input file</param>
        /// <param name="Type">Type of minification to run</param>
        /// <returns>A stripped file</returns>
        public static string Minify(this FileInfo Input, MinificationType Type = MinificationType.HTML)
        {
            Contract.Requires<ArgumentNullException>(Input != null, "Input");
            if (!Input.Exists)
                throw new ArgumentException("Input file does not exist");
            return Input.Read().Minify(Type);
        }

        #endregion

        #region Private Functions

        private static string HTMLMinify(string Input)
        {
            Input = Regex.Replace(Input, "/// <.+>", "");
            if (string.IsNullOrEmpty(Input))
                return "";
            Input = Regex.Replace(Input, @">[\s\S]*?<", new MatchEvaluator(Evaluate));
            return Input;
        }

        private static string Evaluate(Match Matcher)
        {
            string MyString = Matcher.ToString();
            if (string.IsNullOrEmpty(MyString))
                return "";
            MyString = Regex.Replace(MyString, @"\r\n\s*", "");
            return MyString;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", MessageId = "System.String.StartsWith(System.String,System.StringComparison)")]
        private static string JavaScriptMinify(string Input)
        {
            string[] CodeLines = Input.Split(new string[] { System.Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder Builder = new StringBuilder();
            foreach (string Line in CodeLines)
            {
                string Temp = Line.Trim();
                if (Temp.Length > 0 && !Temp.StartsWith("//", StringComparison.InvariantCulture))
                    Builder.AppendLine(Temp);
            }

            Input = Builder.ToString();
            Input = Regex.Replace(Input, @"(/\*\*/)|(/\*[^!][\s\S]*?\*/)", string.Empty);
            Input = Regex.Replace(Input, @"^[\s]+|[ \f\r\t\v]+$", String.Empty);
            Input = Regex.Replace(Input, @"^[\s]+|[ \f\r\t\v]+$", String.Empty);
            Input = Regex.Replace(Input, @"([+-])\n\1", "$1 $1");
            Input = Regex.Replace(Input, @"([^+-][+-])\n", "$1");
            Input = Regex.Replace(Input, @"([^+]) ?(\+)", "$1$2");
            Input = Regex.Replace(Input, @"(\+) ?([^+])", "$1$2");
            Input = Regex.Replace(Input, @"([^-]) ?(\-)", "$1$2");
            Input = Regex.Replace(Input, @"(\-) ?([^-])", "$1$2");
            Input = Regex.Replace(Input, @"\n([{}()[\],<>/*%&|^!~?:=.;+-])", "$1");
            Input = Regex.Replace(Input, @"(\W(if|while|for)\([^{]*?\))\n", "$1");
            Input = Regex.Replace(Input, @"(\W(if|while|for)\([^{]*?\))((if|while|for)\([^{]*?\))\n", "$1$3");
            Input = Regex.Replace(Input, @"([;}]else)\n", "$1 ");
            Input = Regex.Replace(Input, @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&nbsp;)\s{2,}(?=[<])", String.Empty);

            return Input;
        }

        private static string CSSMinify(string Input)
        {
            Input = Regex.Replace(Input, @"(/\*\*/)|(/\*[^!][\s\S]*?\*/)", string.Empty);
            Input = Regex.Replace(Input, @"\s+", " ");
            Input = Regex.Replace(Input, @"(\s([\{:,;\}\(\)]))", "$2");
            Input = Regex.Replace(Input, @"(([\{:,;\}\(\)])\s)", "$2");
            Input = Regex.Replace(Input, ":0 0 0 0;", ":0;");
            Input = Regex.Replace(Input, ":0 0 0;", ":0;");
            Input = Regex.Replace(Input, ":0 0;", ":0;");
            Input = Regex.Replace(Input, ";}", "}");
            Input = Regex.Replace(Input, @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&nbsp;)\s{2,}(?=[<])", string.Empty);
            Input = Regex.Replace(Input, @"([!{}:;>+([,])\s+", "$1");
            Input = Regex.Replace(Input, @"([\s:])(0)(px|em|%|in|cm|mm|pc|pt|ex)", "$1$2");
            Input = Regex.Replace(Input, "background-position:0", "background-position:0 0");
            Input = Regex.Replace(Input, @"(:|\s)0+\.(\d+)", "$1.$2");
            Input = Regex.Replace(Input, @"[^\}]+\{;\}", "");
            return Input;
        }

        #endregion
    }

    #region Enums

    /// <summary>
    /// Defines the type of data that is being minified
    /// </summary>
    public enum MinificationType
    {
        /// <summary>
        /// CSS
        /// </summary>
        CSS,
        /// <summary>
        /// Javascript
        /// </summary>
        JavaScript,
        /// <summary>
        /// HTML
        /// </summary>
        HTML
    }

    #endregion
}