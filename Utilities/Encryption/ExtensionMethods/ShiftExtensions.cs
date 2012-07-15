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
using System.Text;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.Encryption.ExtensionMethods
{
    /// <summary>
    /// Extensions that deal with bit xoring
    /// </summary>
    public static class ShiftExtensions
    {
        #region Functions

        #region Encrypt

        /// <summary>
        /// Encrypts the data using a basic xor of the key (not very secure unless doing a one time pad)
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use</param>
        /// <param name="OneTimePad">Is this a one time pad?</param>
        /// <returns>The encrypted data</returns>
        public static byte[] Encrypt(this byte[] Data, byte[] Key, bool OneTimePad)
        {
            if (Data.IsNull())
                return null;
            Key.ThrowIfNull("Key");
            if (OneTimePad && Key.Length < Data.Length)
                throw new ArgumentException("Key is not long enough");
            return Process(Data, Key);
        }

        /// <summary>
        /// Encrypts the data using a basic xor of the key (not very secure unless doing a one time pad)
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use</param>
        /// <param name="OneTimePad">Is this a one time pad?</param>
        /// <param name="EncodingUsing">Encoding that the Data uses (defaults to UTF8)</param>
        /// <returns>The encrypted data</returns>
        public static string Encrypt(this string Data, string Key, bool OneTimePad, Encoding EncodingUsing = null)
        {
            if (Data.IsNull())
                return "";
            Key.ThrowIfNull("Key");
            return Data.ToByteArray(EncodingUsing).Encrypt(Key.ToByteArray(EncodingUsing), OneTimePad).ToEncodedString(EncodingUsing);
        }

        #endregion

        #region Decrypt

        /// <summary>
        /// Decrypts the data using a basic xor of the key (not very secure unless doing a one time pad)
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use</param>
        /// <param name="OneTimePad">Is this a one time pad?</param>
        /// <returns>The decrypted data</returns>
        public static byte[] Decrypt(this byte[] Data, byte[] Key, bool OneTimePad)
        {
            if (Data.IsNull())
                return null;
            Key.ThrowIfNull("Key");
            if (OneTimePad && Key.Length < Data.Length)
                throw new ArgumentException("Key is not long enough");
            return Process(Data, Key);
        }

        /// <summary>
        /// Decrypts the data using a basic xor of the key (not very secure unless doing a one time pad)
        /// </summary>
        /// <param name="Data">Data to decrypt</param>
        /// <param name="Key">Key to use</param>
        /// <param name="OneTimePad">Is this a one time pad?</param>
        /// <param name="EncodingUsing">Encoding that the Data uses (defaults to UTF8)</param>
        /// <returns>The encrypted data</returns>
        public static string Decrypt(this string Data, string Key, bool OneTimePad, Encoding EncodingUsing = null)
        {
            if (Data.IsNull())
                return "";
            Key.ThrowIfNull("Key");
            return Data.ToByteArray(EncodingUsing).Decrypt(Key.ToByteArray(EncodingUsing), OneTimePad).ToEncodedString(EncodingUsing);
        }

        #endregion

        #region Process

        /// <summary>
        /// Actually does the encryption/decryption
        /// </summary>
        private static byte[] Process(byte[] Input, byte[] Key)
        {
            byte[] OutputArray = new byte[Input.Length];
            int Position = 0;
            for (int x = 0; x < Input.Length; ++x)
            {
                OutputArray[x] = (byte)(Input[x] ^ Key[Position]);
                ++Position;
                if (Position >= Key.Length)
                    Position = 0;
            }
            return OutputArray;
        }

        #endregion

        #region XOr

        /// <summary>
        /// XOrs two strings together, returning the result
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Key">Key to use</param>
        /// <param name="EncodingUsing">Encoding that the data uses (defaults to UTF8)</param>
        /// <returns>The XOred string</returns>
        public static string XOr(this string Input, string Key, Encoding EncodingUsing = null)
        {
            if (Input.IsNullOrEmpty())
                return "";
            if (Key.IsNullOrEmpty())
                return Input;
            byte[] InputArray = Input.ToByteArray(EncodingUsing);
            byte[] KeyArray = Key.ToByteArray(EncodingUsing);
            byte[] OutputArray = new byte[InputArray.Length];
            for (int x = 0; x < InputArray.Length; ++x)
            {
                byte KeyByte = x < KeyArray.Length ? KeyArray[x] : (byte)0;
                OutputArray[x] = (byte)(InputArray[x] ^ KeyByte);
            }
            return OutputArray.ToEncodedString(EncodingUsing);
        }

        #endregion

        #endregion
    }
}