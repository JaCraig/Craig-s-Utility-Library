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
    /// Utility class for doing DES Encryption
    /// </summary>
    public static class DESEncryption
    {
        #region Public Static Functions

        /// <summary>
        /// Encrypts a string using DES
        /// </summary>
        /// <param name="Input">String to encrypt</param>
        /// <param name="Key">Key to encrypt with (must be at least 8 bytes)</param>
        /// <returns>An encrypted string</returns>
        public static string Encrypt(string Input,string Key)
        {
            if (string.IsNullOrEmpty(Input) || string.IsNullOrEmpty(Key))
            {
                throw new ArgumentNullException("The input/key string can not be empty.");
            }
            try
            {
                ASCIIEncoding Encoding = new ASCIIEncoding();
                byte[] KeyHashArray = Encoding.GetBytes(Key);
                byte[] KeyArray = new byte[8];
                byte[] Key2Array = new byte[8];
                SHA1CryptoServiceProvider SHA = new SHA1CryptoServiceProvider();
                byte[] Hash = SHA.ComputeHash(KeyHashArray);
                SHA.Clear();
                for (int x = 0; x < 8; ++x)
                {
                    KeyArray[x] = Hash[x];
                    Key2Array[x] = Hash[x + 8];
                }
                byte[] Text = null;
                DESCryptoServiceProvider Encryptor = new DESCryptoServiceProvider();
                using (MemoryStream Stream = new MemoryStream())
                {
                    using (CryptoStream DESStream = new CryptoStream(Stream, Encryptor.CreateEncryptor(KeyArray, Key2Array), CryptoStreamMode.Write))
                    {
                        using (StreamWriter Writer = new StreamWriter(DESStream))
                        {
                            Writer.Write(Input);
                            Writer.Flush();
                            DESStream.FlushFinalBlock();
                            Writer.Flush();
                            Text = Stream.GetBuffer();
                        }
                    }
                }
                Encryptor.Clear();
                return Convert.ToBase64String(Text, 0, (int)Text.Length);
            }
            catch { throw; }
        }

        /// <summary>
        /// Decrypts a string using DES
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Key">Key to use in decryption (must be at least 8 bytes)</param>
        /// <returns>A decrypted string</returns>
        public static string Decrypt(string Input,string Key)
        {
            if (string.IsNullOrEmpty(Input) || string.IsNullOrEmpty(Key))
            {
                throw new ArgumentNullException("The input/key string can not be empty.");
            }
            try
            {
                ASCIIEncoding Encoding = new ASCIIEncoding();
                byte[] KeyHashArray = Encoding.GetBytes(Key);
                byte[] KeyArray = new byte[8];
                byte[] Key2Array = new byte[8];
                SHA1CryptoServiceProvider SHA = new SHA1CryptoServiceProvider();
                byte[] Hash = SHA.ComputeHash(KeyHashArray);
                SHA.Clear();
                for (int x = 0; x < 8; ++x)
                {
                    KeyArray[x] = Hash[x];
                    Key2Array[x] = Hash[x + 8];
                }
                string Text = null;
                DESCryptoServiceProvider Decryptor = new DESCryptoServiceProvider();
                using (MemoryStream Stream = new MemoryStream(Convert.FromBase64String(Input)))
                {
                    using (CryptoStream DESStream = new CryptoStream(Stream, Decryptor.CreateDecryptor(KeyArray, Key2Array), CryptoStreamMode.Read))
                    {
                        using (StreamReader Reader = new StreamReader(DESStream))
                        {
                            Text = Reader.ReadToEnd();
                        }
                    }
                }
                Decryptor.Clear();
                return Text;
            }
            catch { throw; }
        }

        #endregion
    }
}
