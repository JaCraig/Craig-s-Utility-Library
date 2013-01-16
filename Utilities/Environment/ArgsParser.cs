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
using System.Text.RegularExpressions;

#endregion

namespace Utilities.Environment
{
    /// <summary>
    /// Parses the args variables of an application
    /// </summary>
    public class ArgsParser
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="OptionStarter">The text to determine where an option starts</param>
        public ArgsParser(string OptionStarter = "/")
        {
            if (string.IsNullOrEmpty(OptionStarter))
                throw new ArgumentNullException("OptionStarter");
            this.OptionStarter = OptionStarter;
            OptionRegex = new Regex(string.Format(@"(?<Command>{0}[^\s]+)[\s|\S|$](?<Parameter>""[^""]*""|[^""{0}]*)", OptionStarter));
        }

        #endregion

        #region Functions

        /// <summary>
        /// Parses the args into individual options
        /// </summary>
        /// <param name="Args">Args to parse</param>
        /// <returns>A list of options</returns>
        public virtual IEnumerable<Option> Parse(string[] Args)
        {
            if (Args == null)
                return new List<Option>();
            List<Option> Result = new List<Option>();
            string Text = "";
            string Splitter = "";
            foreach (string Arg in Args)
            {
                Text += Splitter + Arg;
                Splitter = " ";
            }
            MatchCollection Matches = OptionRegex.Matches(Text);
            string Option = "";
            foreach (Match OptionMatch in Matches)
            {
                if (OptionMatch.Value.StartsWith(OptionStarter) && !string.IsNullOrEmpty(Option))
                {
                    Result.Add(new Option(Option, OptionStarter));
                    Option = "";
                }
                Option += OptionMatch.Value + " ";
            }
            Result.Add(new Option(Option, OptionStarter));
            return Result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Option regex
        /// </summary>
        protected virtual Regex OptionRegex { get; set; }

        /// <summary>
        /// String that starts an option
        /// </summary>
        protected virtual string OptionStarter { get; set; }

        #endregion
    }
}
