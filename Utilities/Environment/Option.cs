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
using System.Text;
using System.Text.RegularExpressions;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.Environment
{
    /// <summary>
    /// Contains an individual option
    /// </summary>
    public class Option
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Text">Option text</param>
        /// <param name="OptionStarter">Starter text for an option ("/", "-", etc.)</param>
        public Option(string Text, string OptionStarter)
        {
            if (string.IsNullOrEmpty(Text))
                throw new ArgumentNullException("Text");
            if (string.IsNullOrEmpty(OptionStarter))
                throw new ArgumentNullException("OptionStarter");
            Regex CommandParser = new Regex(string.Format(@"{0}(?<Command>[^\s]*)\s(?<Parameters>.*)", OptionStarter));
            Regex ParameterParser = new Regex("(?<Parameter>\"[^\"]*\")[\\s]?|(?<Parameter>[^\\s]*)[\\s]?");
            Parameters = new List<string>();
            Match CommandMatch = CommandParser.Match(Text);
            Command = CommandMatch.Groups["Command"].Value;
            Text = CommandMatch.Groups["Parameters"].Value;
            ParameterParser.Matches(Text)
                           .Where(x => !string.IsNullOrEmpty(x.Value))
                           .ForEach(x => Parameters.Add(x.Groups["Parameter"].Value));
        }

        #endregion

        #region Functions

        /// <summary>
        /// Converts the options into a string
        /// </summary>
        /// <returns>The string representation of the option</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("Command: ")
                   .Append(Command)
                   .Append("\n")
                   .Append("Parameters: ");
            Parameters.ForEach(x => Builder.Append(x).Append(" "));
            return Builder.Append("\n")
                          .ToString();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Command string
        /// </summary>
        public virtual string Command { get; set; }

        /// <summary>
        /// List of parameters found
        /// </summary>
        public virtual List<string> Parameters { get; set; }

        #endregion
    }
}
