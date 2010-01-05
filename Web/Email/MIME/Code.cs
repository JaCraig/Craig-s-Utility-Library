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

namespace Utilities.Web.Email.MIME
{
    /// <summary>
    /// Base coder class
    /// </summary>
    public class Code
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Code()
        {
        }
        #endregion

        #region Public Properties
        private string _CharacterSet = "";
        /// <summary>
        /// Character set this coder is using
        /// </summary>
        public string CharacterSet
        {
            get { return _CharacterSet; }
            set { _CharacterSet = value.Replace("\"",""); }
        }
        #endregion

        #region Public Functions

        /// <summary>
        /// Decodes a string to bytes
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Output">Bytes once decoded</param>
        public virtual void Decode(string Input, out byte[] Output)
        {
            if (string.IsNullOrEmpty(Input))
                throw new ArgumentNullException("Input can not be null");

            if (string.IsNullOrEmpty(CharacterSet))
            {
                CharacterSet = Encoding.Default.BodyName;
            }
            Output = Encoding.GetEncoding(CharacterSet).GetBytes(Input);
        }

        /// <summary>
        /// Decodes a string to another string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Output">Output string</param>
        public virtual void Decode(string Input, out string Output)
        {
            if (string.IsNullOrEmpty(Input))
                throw new ArgumentNullException("Input can not be null");
            if (string.IsNullOrEmpty(CharacterSet))
            {
                CharacterSet = Encoding.Default.BodyName;
            }
            byte[] TempBytes=null;
            Decode(Input,out TempBytes);
            Output=Encoding.GetEncoding(CharacterSet).GetString(TempBytes);
        }

        /// <summary>
        /// Encodes a byte array
        /// </summary>
        /// <param name="Input">Input array</param>
        /// <returns>A string of the bytes encoded</returns>
        public virtual string Encode(byte[] Input)
        {
            if (Input==null)
                throw new ArgumentNullException("Input can not be null");

            if (string.IsNullOrEmpty(CharacterSet))
            {
                CharacterSet = Encoding.Default.BodyName;
            }
            return Encoding.GetEncoding(CharacterSet).GetString(Input, 0, Input.Length);
        }

        /// <summary>
        /// Encodes a string into a string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>An encoded string</returns>
        public virtual string Encode(string Input)
        {
            if (string.IsNullOrEmpty(Input))
                throw new ArgumentNullException("Input can not be null");
            if (string.IsNullOrEmpty(CharacterSet))
            {
                CharacterSet = Encoding.Default.BodyName;
            }
            byte[] TempArray = Encoding.GetEncoding(CharacterSet).GetBytes(Input);
            return Encode(TempArray);
        }
        #endregion
    }
}