/*
Copyright (c) 2013 <a href="http://www.gutgames.com">James Craig</a>

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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Utilities.DataTypes;
using Utilities.IO.Encryption;
#endregion

namespace Utilities.IO
{
    /// <summary>
    /// Extension methods dealing with encryption
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EncryptionExtensions
    {
        #region Functions

        #region GenerateSalt

        /// <summary>
        /// Generates salt
        /// </summary>
        /// <param name="Random">Randomization object</param>
        /// <param name="Size">Size of the salt byte array</param>
        /// <returns>A byte array as salt</returns>
        public static byte[] GenerateSalt(this System.Random Random, int Size)
        {
            Contract.Requires<ArgumentException>(Size > 0, "Size must be greater than 0");
            byte[] Salt = new byte[Size];
            using (RNGCryptoServiceProvider CryptoProvider = new RNGCryptoServiceProvider())
            {
                CryptoProvider.GetNonZeroBytes(Salt);
            }
            return Salt;
        }

        #endregion

        #region Hash

        /// <summary>
        /// Computes the hash of a byte array
        /// </summary>
        /// <param name="Data">Byte array to hash</param>
        /// <param name="Algorithm">Hash algorithm to use (defaults to SHA1)</param>
        /// <returns>The hash of the byte array</returns>
        public static byte[] Hash(this byte[] Data, string Algorithm = "SHA1")
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Hash(Data, Algorithm);
        }

        /// <summary>
        /// Computes the hash of a string
        /// </summary>
        /// <param name="Data">string to hash</param>
        /// <param name="Algorithm">Algorithm to use (defaults to SHA1)</param>
        /// <param name="EncodingUsing">Encoding used by the string (defaults to UTF8)</param>
        /// <returns>The hash of the string</returns>
        public static string Hash(this string Data, string Algorithm = "SHA1", Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Data))
                return "";
            return BitConverter.ToString(Data.ToByteArray(EncodingUsing).Hash(Algorithm)).Replace("-", "").Encode(null, EncodingUsing);
        }

        #endregion

        #region Encrypt
        
        /// <summary>
        /// Encrypts a byte array using RSA
        /// </summary>
        /// <param name="Input">Input (should be small as anything over 128 bytes can not be decrypted)</param>
        /// <param name="Key">Key to use for encryption</param>
        /// <returns>An encrypted string (64bit string)</returns>
        public static string Encrypt(byte[] Input, string Key)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Encrypt(Input, Key).ToString(Base64FormattingOptions.None);
        }

        /// <summary>
        /// Encrypts the data using a basic xor of the key (not very secure unless doing a one time pad)
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use</param>
        /// <returns>The encrypted data</returns>
        public static byte[] Encrypt(this byte[] Data, byte[] Key)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Encrypt(Data,Key);
        }

        /// <summary>
        /// Encrypts a byte array
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use to encrypt the data (can use PasswordDeriveBytes, Rfc2898DeriveBytes, etc. Really anything that implements DeriveBytes)</param>
        /// <param name="AlgorithmUsing">Algorithm to use for encryption (defaults to AES)</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)</param>
        /// <param name="EncodingUsing">Encoding that the original string is using (defaults to UTF8)</param>
        /// <returns>An encrypted byte array</returns>
        public static string Encrypt(this string Data,
            DeriveBytes Key,
            Encoding EncodingUsing = null,
            string AlgorithmUsing = "AES",
            string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            if (string.IsNullOrEmpty(Data))
                return "";
            return Data.ToByteArray(EncodingUsing)
                       .Encrypt(Key, AlgorithmUsing, InitialVector, KeySize)
                       .ToString(Base64FormattingOptions.None);
        }

        /// <summary>
        /// Encrypts a byte array
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use to encrypt the data (can use PasswordDeriveBytes, Rfc2898DeriveBytes, etc. Really anything that implements DeriveBytes)</param>
        /// <param name="AlgorithmUsing">Algorithm to use for encryption (defaults to AES)</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)</param>
        /// <returns>An encrypted byte array</returns>
        public static byte[] Encrypt(this byte[] Data,
            DeriveBytes Key,
            string AlgorithmUsing = "AES",
            string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Encrypt(Data, Key, AlgorithmUsing, InitialVector, KeySize);
        }

        #endregion

        #region Decrypt

        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="Data">Text to be decrypted (Base 64 string)</param>
        /// <param name="Key">Key to use to encrypt the data (can use PasswordDeriveBytes, Rfc2898DeriveBytes, etc. Really anything that implements DeriveBytes)</param>
        /// <param name="EncodingUsing">Encoding that the output string should use (defaults to UTF8)</param>
        /// <param name="AlgorithmUsing">Algorithm to use for decryption (defaults to AES)</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)</param>
        /// <returns>A decrypted string</returns>
        public static string Decrypt(this string Data,
            DeriveBytes Key,
            Encoding EncodingUsing = null,
            string AlgorithmUsing = "AES",
            string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            if (string.IsNullOrEmpty(Data))
                return "";
            return Data.FromBase64()
                        .Decrypt(Key, AlgorithmUsing, InitialVector, KeySize)
                        .ToString(EncodingUsing);
        }

        /// <summary>
        /// Decrypts a byte array
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use to encrypt the data (can use PasswordDeriveBytes, Rfc2898DeriveBytes, etc. Really anything that implements DeriveBytes)</param>
        /// <param name="AlgorithmUsing">Algorithm to use for encryption (defaults to AES)</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)</param>
        /// <returns>An encrypted byte array</returns>
        public static byte[] Decrypt(this byte[] Data,
            DeriveBytes Key,
            string AlgorithmUsing = "AES",
            string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Decrypt(Data, Key, AlgorithmUsing, InitialVector, KeySize);
        }

        /// <summary>
        /// Decrypts the data using a basic xor of the key (not very secure unless doing a one time pad)
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use</param>
        /// <returns>The decrypted data</returns>
        public static byte[] Decrypt(this byte[] Data, byte[] Key)
        {
            Contract.Requires<ArgumentNullException>(Key != null, "Key");
            if (Data == null)
                return null;
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Decrypt(Data, Key);
        }

        /// <summary>
        /// Decrypts a string using RSA
        /// </summary>
        /// <param name="Input">Input string (should be small as anything over 128 bytes can not be decrypted)</param>
        /// <param name="Key">Key to use for decryption</param>
        /// <param name="EncodingUsing">Encoding that the result should use (defaults to UTF8)</param>
        /// <returns>A decrypted string</returns>
        public static string Decrypt(this byte[] Input, string Key, Encoding EncodingUsing = null)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().Decrypt(Input, Key).ToString(EncodingUsing);
        }

        #endregion

        #region CreateKey

        /// <summary>
        /// Creates a new set of keys
        /// </summary>
        /// <param name="Random">Random object</param>
        /// <param name="PrivatePublic">True if private key should be included, false otherwise</param>
        /// <returns>XML representation of the key information</returns>
        public static string CreateKey(this System.Random Random, bool PrivatePublic)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().CreateKey(PrivatePublic);
        }

        #endregion

        #region SignHash

        /// <summary>
        /// Takes a string and creates a signed hash of it
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Key">Key to encrypt/sign with</param>
        /// <param name="Hash">This will be filled with the unsigned hash</param>
        /// <param name="EncodingUsing">Encoding that the input is using (defaults to UTF8)</param>
        /// <returns>A signed hash of the input (64bit string)</returns>
        public static string SignHash(this string Input, string Key, out string Hash, Encoding EncodingUsing = null)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().SignHash(Input, Key, out Hash, EncodingUsing);
        }

        #endregion

        #region VerifyHash

        /// <summary>
        /// Verifies a signed hash against the unsigned version
        /// </summary>
        /// <param name="Hash">The unsigned hash (should be 64bit string)</param>
        /// <param name="SignedHash">The signed hash (should be 64bit string)</param>
        /// <param name="Key">The key to use in decryption</param>
        /// <returns>True if it is verified, false otherwise</returns>
        public static bool VerifyHash(this string Hash, string SignedHash, string Key)
        {
            return IoC.Manager.Bootstrapper.Resolve<Manager>().VerifyHash(Hash, SignedHash, Key);
        }

        #endregion

        #endregion
    }
}