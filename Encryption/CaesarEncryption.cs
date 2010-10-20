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
using System.Text;
#endregion

namespace Utilities.Encryption
{
    /// <summary>
    /// Encrypts a string using Caesar encryption
    /// </summary>
    public static class CaesarEncryption
    {
        #region Public Static Functions

        /// <summary>
        /// Encrypts a string using Caesar encryption
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Key">Key to encrypt with (can be any length)</param>
        /// <returns>An encrypted string</returns>
        public static string Encrypt(string Input, string Key)
        {
            return Process(Input, Key);
        }

        /// <summary>
        /// Decrypts a string using Caesar encryption
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Key">Key to decrypt with (can be any length)</param>
        /// <returns>A decrypted string</returns>
        public static string Decrypt(string Input, string Key)
        {
            return Process(Input, Key);
        }

        #endregion

        #region Private Static Functions

        /// <summary>
        /// Actually does the encryption/decryption
        /// </summary>
        private static string Process(string Input, string Key)
        {
            ASCIIEncoding Encoding = new ASCIIEncoding();
            byte[] InputArray = Encoding.GetBytes(Input);
            byte[] KeyArray = Encoding.GetBytes(Key);
            byte[] OutputArray = new byte[InputArray.Length];
            int Position = 0;
            for (int x = 0; x < InputArray.Length; ++x)
            {
                OutputArray[x] = (byte)(InputArray[x] ^ Key[Position]);
                ++Position;
                if (Position >= Key.Length)
                {
                    Position = 0;
                }
            }
            return Encoding.GetString(OutputArray);
        }

        #endregion
    }
}