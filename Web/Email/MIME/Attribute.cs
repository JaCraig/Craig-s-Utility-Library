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
using System.Text.RegularExpressions;
#endregion

namespace Utilities.Web.Email.MIME
{
    /// <summary>
    /// Attributes associated with fields
    /// </summary>
    public class Attribute
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Attribute()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="AttributeText">Text of the attribute</param>
        public Attribute(string AttributeText)
        {
            string[] Splitter = { "=" };
            string[] Values = AttributeText.Split(Splitter, StringSplitOptions.None);
            if (Values.Length == 2)
            {
                Name = Values[0];
                Value = Values[1];
            }
            else if (Values.Length > 2)
            {
                Name = Values[0];
                Value = Values[1];
                for (int x = 2; x < Values.Length; ++x)
                {
                    Value += "=" + Values[x];
                }
            }
            else
            {
                Value = Values[0];
            }
            Regex TempReg = new Regex("\r\n*");
            Name = TempReg.Replace(Name, "");
            TempReg = new Regex("\t*");
            Name = TempReg.Replace(Name, "");
            TempReg = new Regex(Regex.Escape(" ") + "*");
            Name = TempReg.Replace(Name, "");
        }
        #endregion

        #region Public Properties
        private string _Name="";
        private string _Value="";
        /// <summary>
        /// Name of the attribute
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// Value of the attribtue
        /// </summary>
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        #endregion
    }
}
