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

#endregion

using System;
using System.Runtime.Serialization;
namespace Utilities.Web.Email.Pop3
{
    /// <summary>
    /// Exception class used by the Pop3Client class
    /// </summary>
    [Serializable]
    public class Pop3Exception:System.Exception
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Pop3Exception()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ExceptionText">Message</param>
        /// <param name="Exception">Inner exception</param>
        public Pop3Exception(string ExceptionText, Exception Exception)
            : base(ExceptionText, Exception)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ExceptionText">Text to put inside the exception</param>
        public Pop3Exception(string ExceptionText)
            : base(ExceptionText)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Info">Serialization info</param>
        /// <param name="Context">Streaming context</param>
        protected Pop3Exception(SerializationInfo Info, StreamingContext Context) : base(Info, Context) { }
        #endregion
    }
}