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
using System.IO;
using System.Security.Cryptography;
using System.Text;
#endregion

namespace Utilities.Encryption
{
    /// <summary>
    /// Utility class for doing Triple DES encryption
    /// </summary>
    public static class TripleDESEncryption
    {
        #region Public Static Functions
        /// <summary>
        /// Encrypts the string using Triple DES
        /// </summary>
        /// <param name="Input">String to be encrypted</param>
        /// <param name="Key">Key to encrypt with (must be at least 24 bytes)</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string Input, string Key)
        {
            if (string.IsNullOrEmpty(Input) || string.IsNullOrEmpty(Key))
            {
                throw new ArgumentNullException("The input/key string can not be empty.");
            }
            try
            {
                ASCIIEncoding Encoding = new ASCIIEncoding();
                byte[] Hash = Encoding.GetBytes(Key);
                byte[] KeyArray = new byte[24];
                byte[] Key2Array = new byte[8];
                for (int x = 0; x < 24; ++x)
                {
                    KeyArray[x] = Hash[x];
                }
                for (int x = 0; x < 8; ++x)
                {
                    Key2Array[x] = Hash[x + 8];
                }

                TripleDESCryptoServiceProvider Encryptor = new TripleDESCryptoServiceProvider();
                MemoryStream Stream = new MemoryStream();
                CryptoStream DESStream = new CryptoStream(Stream, Encryptor.CreateEncryptor(KeyArray, Key2Array), CryptoStreamMode.Write);
                StreamWriter Writer = new StreamWriter(DESStream);
                Writer.Write(Input);
                Writer.Flush();
                DESStream.FlushFinalBlock();
                Writer.Flush();
                byte[] Text = Stream.GetBuffer();
                Stream.Dispose();
                DESStream.Dispose();
                Writer.Dispose();
                return Convert.ToBase64String(Text, 0, (int)Text.Length);
            }
            catch (Exception a)
            {
                throw a;
            }
        }

        /// <summary>
        /// Decrypts the string using Triple DES
        /// </summary>
        /// <param name="Input">String to be decrypted</param>
        /// <param name="Key">Key to decrypt with (must be at least 24 bytes)</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(string Input, string Key)
        {
            if (string.IsNullOrEmpty(Input) || string.IsNullOrEmpty(Key))
            {
                throw new ArgumentNullException("The input/key string can not be empty.");
            }
            try
            {
                ASCIIEncoding Encoding = new ASCIIEncoding();
                byte[] Hash = Encoding.GetBytes(Key);
                byte[] KeyArray = new byte[24];
                byte[] Key2Array = new byte[8];
                for (int x = 0; x < 24; ++x)
                {
                    KeyArray[x] = Hash[x];
                }
                for (int x = 0; x < 8; ++x)
                {
                    Key2Array[x] = Hash[x + 8];
                }
                TripleDESCryptoServiceProvider Decryptor = new TripleDESCryptoServiceProvider();
                MemoryStream Stream = new MemoryStream(Convert.FromBase64String(Input));
                CryptoStream DESStream = new CryptoStream(Stream, Decryptor.CreateDecryptor(KeyArray, Key2Array), CryptoStreamMode.Read);
                StreamReader Reader = new StreamReader(DESStream);
                string Text = Reader.ReadToEnd();
                Stream.Dispose();
                DESStream.Dispose();
                Reader.Dispose();
                return Text;
            }
            catch (Exception a)
            {
                throw a;
            }
        }
        #endregion
    }
}
