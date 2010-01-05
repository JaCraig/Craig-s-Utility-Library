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
using System.Text;
#endregion

namespace Utilities.Web.Email.MIME.CodeTypes
{
    /// <summary>
    /// Base 64 coder
    /// </summary>
    public class CodeBase64:Code
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public CodeBase64()
        {
        }
        #endregion

        #region Public Overridden Functions
        public override void Decode(string Input, out byte[] Output)
        {
            if (string.IsNullOrEmpty(Input))
                throw new ArgumentNullException("Input can not be null");
            Output = System.Convert.FromBase64String(Input);
        }

        public override string Encode(byte[] Input)
        {
            if (Input==null)
                throw new ArgumentNullException("Input can not be null");
            string TempString = System.Convert.ToBase64String(Input);
            int MAX = 76;
            int Index = 0;
            StringBuilder TempBuilder = new StringBuilder();
            while ((Index + MAX) < TempString.Length)
            {
                TempBuilder.AppendFormat("{0}\r\n", TempString.Substring(Index, MAX));
                Index += MAX;
            }
            TempBuilder.AppendFormat("{0}", TempString.Substring(Index, TempString.Length - Index));
            return TempBuilder.ToString();
        }
        #endregion
    }
}