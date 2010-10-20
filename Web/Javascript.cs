/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Text;
using System.Text.RegularExpressions;
#endregion

namespace Utilities.Web
{
    /// <summary>
    /// Utility class used for javascript
    /// </summary>
    public static class Javascript
    {
        #region Public Static Functions

        /// <summary>
        /// Minifies an input string
        /// </summary>
        /// <param name="Input">Javascript code</param>
        /// <returns>A minified javascript string</returns>
        public static string Minify(string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            string[] CodeLines = Input.Split(new string[] { System.Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder Builder = new StringBuilder();
            foreach (string Line in CodeLines)
            {
                string Temp = Line.Trim();
                if (Temp.Length > 0 && !Temp.StartsWith("//"))
                    Builder.AppendLine(Temp);
            }

            Input = Builder.ToString();
            Input = Regex.Replace(Input, "(/" + Regex.Escape("*") + ".*?" + Regex.Escape("*") + "/)", string.Empty);
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
            Input = Regex.Replace(Input, @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&ndsp;)\s{2,}(?=[<])", String.Empty);

            return Input;
        }

        /// <summary>
        /// Combines and minifies javascript
        /// </summary>
        /// <param name="Input">input strings</param>
        /// <returns>A minified/packed javascript string</returns>
        public static string Combine(List<string> Input)
        {
            StringBuilder Output = new StringBuilder();
            foreach (string Temp in Input)
            {
                Output.Append(Temp).Append("\n");
            }
            return Minify(Output.ToString());
        }

        #endregion
    }
}