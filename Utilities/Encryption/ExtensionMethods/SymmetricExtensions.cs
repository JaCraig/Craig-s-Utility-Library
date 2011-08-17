/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.Encryption.ExtensionMethods
{
    /// <summary>
    /// Symmetric key extensions
    /// </summary>
    public static class SymmetricExtensions
    {
        #region Functions

        #region Encrypt

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="Data">Text to be encrypted</param>
        /// <param name="Key">Password to encrypt with</param>
        /// <param name="AlgorithmUsing">Algorithm to use for encryption (defaults to AES)</param>
        /// <param name="Salt">Salt to encrypt with</param>
        /// <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
        /// <param name="PasswordIterations">Number of iterations to do</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)</param>
        /// <param name="EncodingUsing">Encoding that the original string is using (defaults to UTF8)</param>
        /// <returns>An encrypted string (Base 64 string)</returns>
        public static string Encrypt(this string Data, string Key,
            Encoding EncodingUsing = null,
            SymmetricAlgorithm AlgorithmUsing = null, string Salt = "Kosher",
            string HashAlgorithm = "SHA1", int PasswordIterations = 2,
            string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            if (string.IsNullOrEmpty(Data))
                return "";
            return Data.ToByteArray(EncodingUsing).Encrypt(Key, AlgorithmUsing, Salt, HashAlgorithm, PasswordIterations, InitialVector, KeySize).ToBase64String();
        }

        /// <summary>
        /// Encrypts a byte array
        /// </summary>
        /// <param name="Data">Data to be encrypted</param>
        /// <param name="Key">Password to encrypt with</param>
        /// <param name="AlgorithmUsing">Algorithm to use for encryption (defaults to AES)</param>
        /// <param name="Salt">Salt to encrypt with</param>
        /// <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
        /// <param name="PasswordIterations">Number of iterations to do</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)</param>
        /// <returns>An encrypted byte array</returns>
        public static byte[] Encrypt(this byte[] Data, string Key,
            SymmetricAlgorithm AlgorithmUsing = null, string Salt = "Kosher",
            string HashAlgorithm = "SHA1", int PasswordIterations = 2,
            string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            if (Data == null)
                return null;
            if (AlgorithmUsing == null)
                AlgorithmUsing = new RijndaelManaged();
            if (string.IsNullOrEmpty(Key))
                throw new ArgumentNullException("Key");
            if (string.IsNullOrEmpty(Salt))
                throw new ArgumentNullException("Salt");
            if (string.IsNullOrEmpty(HashAlgorithm))
                throw new ArgumentNullException("HashAlgorithm");
            if (string.IsNullOrEmpty(InitialVector))
                throw new ArgumentNullException("InitialVector");
            using (PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Key, Salt.ToByteArray(), HashAlgorithm, PasswordIterations))
            {
                using (SymmetricAlgorithm SymmetricKey = AlgorithmUsing)
                {
                    SymmetricKey.Mode = CipherMode.CBC;
                    byte[] CipherTextBytes = null;
                    using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(DerivedPassword.GetBytes(KeySize / 8), InitialVector.ToByteArray()))
                    {
                        using (MemoryStream MemStream = new MemoryStream())
                        {
                            using (CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write))
                            {
                                CryptoStream.Write(Data, 0, Data.Length);
                                CryptoStream.FlushFinalBlock();
                                CipherTextBytes = MemStream.ToArray();
                                MemStream.Close();
                                CryptoStream.Close();
                            }
                        }
                    }
                    SymmetricKey.Clear();
                    return CipherTextBytes;
                }
            }
        }

        #endregion

        #region Decrypt

        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="Data">Text to be decrypted (Base 64 string)</param>
        /// <param name="Key">Password to decrypt with</param>
        /// <param name="EncodingUsing">Encoding that the output string should use (defaults to UTF8)</param>
        /// <param name="AlgorithmUsing">Algorithm to use for decryption (defaults to AES)</param>
        /// <param name="Salt">Salt to decrypt with</param>
        /// <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
        /// <param name="PasswordIterations">Number of iterations to do</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)</param>
        /// <returns>A decrypted string</returns>
        public static string Decrypt(this string Data, string Key,
            Encoding EncodingUsing = null,
            SymmetricAlgorithm AlgorithmUsing = null, string Salt = "Kosher",
            string HashAlgorithm = "SHA1", int PasswordIterations = 2,
            string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            if (string.IsNullOrEmpty(Data))
                return "";
            byte[] CipherTextBytes = Convert.FromBase64String(Data);
            return CipherTextBytes.Decrypt(Key, AlgorithmUsing, Salt, HashAlgorithm, PasswordIterations, InitialVector, KeySize).ToEncodedString(EncodingUsing);
        }


        /// <summary>
        /// Decrypts a byte array
        /// </summary>
        /// <param name="Data">Data to be decrypted</param>
        /// <param name="Key">Password to decrypt with</param>
        /// <param name="AlgorithmUsing">Algorithm to use for decryption</param>
        /// <param name="Salt">Salt to decrypt with</param>
        /// <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
        /// <param name="PasswordIterations">Number of iterations to do</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)</param>
        /// <returns>A decrypted byte array</returns>
        public static byte[] Decrypt(this byte[] Data, string Key,
            SymmetricAlgorithm AlgorithmUsing = null, string Salt = "Kosher",
            string HashAlgorithm = "SHA1", int PasswordIterations = 2,
            string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            if (Data == null)
                return null;
            if (AlgorithmUsing == null)
                AlgorithmUsing = new RijndaelManaged();
            if (string.IsNullOrEmpty(Key))
                throw new ArgumentNullException("Key");
            if (string.IsNullOrEmpty(Salt))
                throw new ArgumentNullException("Salt");
            if (string.IsNullOrEmpty(HashAlgorithm))
                throw new ArgumentNullException("HashAlgorithm");
            if (string.IsNullOrEmpty(InitialVector))
                throw new ArgumentNullException("InitialVector");
            using (PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Key, Salt.ToByteArray(), HashAlgorithm, PasswordIterations))
            {
                using (SymmetricAlgorithm SymmetricKey = AlgorithmUsing)
                {
                    SymmetricKey.Mode = CipherMode.CBC;
                    byte[] PlainTextBytes = new byte[Data.Length];
                    int ByteCount = 0;
                    using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(DerivedPassword.GetBytes(KeySize / 8), InitialVector.ToByteArray()))
                    {
                        using (MemoryStream MemStream = new MemoryStream(Data))
                        {
                            using (CryptoStream CryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read))
                            {

                                ByteCount = CryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
                                MemStream.Close();
                                CryptoStream.Close();
                            }
                        }
                    }
                    SymmetricKey.Clear();
                    Array.Resize(ref PlainTextBytes, ByteCount);
                    return PlainTextBytes;
                }
            }
        }

        #endregion

        #endregion
    }
}