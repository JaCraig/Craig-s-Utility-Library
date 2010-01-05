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
using System.Text.RegularExpressions;
#endregion

namespace Utilities.Web
{
    /// <summary>
    /// CSS utility class
    /// </summary>
    public static class CSS
    {
        #region Static Public Functions
        /// <summary>
        /// Strips whitespace from a CSS file
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <returns>A stripped CSS file</returns>
        public static string StripWhitespace(string Input)
        {
            Input = Input.Replace("  ", string.Empty);
            Input = Input.Replace(System.Environment.NewLine, string.Empty);
            Input = Input.Replace("\t", string.Empty);
            Input = Input.Replace(" {", "{");
            Input = Input.Replace(" :", ":");
            Input = Input.Replace(": ", ":");
            Input = Input.Replace(", ", ",");
            Input = Input.Replace("; ", ";");
            Input = Input.Replace(";}", "}");
            Input = Regex.Replace(Input, @"(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,}(?=&nbsp;)|(?<=&ndsp;)\s{2,}(?=[<])", string.Empty);
            Input = Regex.Replace(Input, "([!{}:;>+([,])s+", "$1");
            Input = Regex.Replace(Input, "([^;}])}", "$1;}");
            Input = Regex.Replace(Input, "([s:])(0)(px|em|%|in|cm|mm|pc|pt|ex)", "$1$2");
            Input = Regex.Replace(Input, ":0 0 0 0;", ":0;");
            Input = Regex.Replace(Input, ":0 0 0;", ":0;");
            Input = Regex.Replace(Input, ":0 0;", ":0;");
            Input = Regex.Replace(Input, "background-position:0;", "background-position:0 0;");
            Input = Regex.Replace(Input, "(:|s)0+.(d+)", "$1.$2");
            Input = Regex.Replace(Input, "[^}]+{;}", "");
            Input = Regex.Replace(Input, "(/" + Regex.Escape("*") + ".*?" + Regex.Escape("*") + "/)", string.Empty);
            return Input;
        }
        #endregion
    }
}