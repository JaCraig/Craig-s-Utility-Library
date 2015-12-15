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

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Extension methods for Streams
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StreamExtensions
    {
        /// <summary>
        /// Takes all of the data in the stream and returns it as a string
        /// </summary>
        /// <param name="Input">Input stream</param>
        /// <param name="EncodingUsing">
        /// Encoding that the string should be in (defaults to UTF8)
        /// </param>
        /// <returns>A string containing the content of the stream</returns>
        public static string ReadAll(this Stream Input, Encoding EncodingUsing = null)
        {
            Contract.Requires<ArgumentNullException>(Input != null, "Input");
            return Input.ReadAllBinary().ToString(EncodingUsing);
        }

        /// <summary>
        /// Takes all of the data in the stream and returns it as an array of bytes
        /// </summary>
        /// <param name="Input">Input stream</param>
        /// <returns>A byte array</returns>
        public static byte[] ReadAllBinary(this Stream Input)
        {
            Contract.Requires<ArgumentNullException>(Input != null, "Input");
            var TempInput = Input as MemoryStream;
            if (TempInput != null)
                return TempInput.ToArray();
            byte[] Buffer = new byte[1024];
            byte[] ReturnValue = null;
            using (MemoryStream Temp = new MemoryStream())
            {
                while (true)
                {
                    int Count = Input.Read(Buffer, 0, Buffer.Length);
                    if (Count <= 0)
                    {
                        ReturnValue = Temp.ToArray();
                        break;
                    }
                    Temp.Write(Buffer, 0, Count);
                }
            }
            return ReturnValue;
        }
    }
}