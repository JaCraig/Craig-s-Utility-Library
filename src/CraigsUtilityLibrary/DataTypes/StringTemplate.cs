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

using System.Collections.Generic;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Acts as a template for a string
    /// </summary>
    public class StringTemplate : Dictionary<string, string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="template">Template</param>
        /// <param name="keyEnd">Ending signifier of a key</param>
        /// <param name="keyStart">Starting signifier of a key</param>
        public StringTemplate(string template, string keyStart = "{", string keyEnd = "}")
        {
            KeyStart = keyStart;
            KeyEnd = keyEnd;
            Template = template;
        }

        /// <summary>
        /// Ending signifier of a key
        /// </summary>
        public string KeyEnd { get; protected set; }

        /// <summary>
        /// Beginning signifier of a key
        /// </summary>
        public string KeyStart { get; protected set; }

        /// <summary>
        /// Template
        /// </summary>
        public string Template { get; protected set; }

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>The value as a string</returns>
        public static implicit operator string(StringTemplate value)
        {
            if (value == null)
                return "";
            return value.ToString();
        }

        /// <summary>
        /// Applies the key/values to the template and returns the resulting string
        /// </summary>
        /// <returns>The resulting string</returns>
        public override string ToString()
        {
            return Template.ToString(this.ToArray(x => new KeyValuePair<string, string>(KeyStart + x.Key + KeyEnd, x.Value)));
        }
    }
}