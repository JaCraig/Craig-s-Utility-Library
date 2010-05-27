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
using System.Security.Cryptography;
using System.Text;

#endregion

namespace Utilities.Encryption
{
    /// <summary>
    /// Utility class for doing RSA Encryption
    /// </summary>
    public static class RSAEncryption
    {
        #region Private Static Variables
        private static RSACryptoServiceProvider RSA = null;
        #endregion

        #region Public Static Functions

        /// <summary>
        /// Encrypts a string using RSA
        /// </summary>
        /// <param name="Input">Input string (should be small as anything over 128 bytes can not be decrypted)</param>
        /// <param name="Key">Key to use for encryption</param>
        /// <returns>An encrypted string (64bit string)</returns>
        public static string Encrypt(string Input, string Key)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(Key);
            ASCIIEncoding Encoding = new ASCIIEncoding();
            byte[] InputArray = Encoding.GetBytes(Input);
            byte[] EncryptedBytes = RSA.Encrypt(InputArray, true);
            RSA.Clear();
            return Convert.ToBase64String(EncryptedBytes);
        }

        /// <summary>
        /// Decrypts a string using RSA
        /// </summary>
        /// <param name="Input">Input string (should be small as anything over 128 bytes can not be decrypted)</param>
        /// <param name="Key">Key to use for decryption</param>
        /// <returns>A decrypted string</returns>
        public static string Decrypt(string Input, string Key)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(Key);
            byte[] InputArray = Convert.FromBase64String(Input);
            byte[] EncryptedBytes = RSA.Decrypt(InputArray, true);
            RSA.Clear();
            return Encoding.UTF8.GetString(EncryptedBytes);
        }

        /// <summary>
        /// Creates a new set of keys
        /// </summary>
        /// <param name="PrivatePublic">True if private key should be included, false otherwise</param>
        /// <returns>XML representation of the key information</returns>
        public static string CreateKey(bool PrivatePublic)
        {
            if (RSA == null)
            {
                RSA = new RSACryptoServiceProvider();
            }
            return RSA.ToXmlString(PrivatePublic);
        }

        /// <summary>
        /// Takes a string and creates a signed hash of it
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Key">Key to encrypt/sign with</param>
        /// <param name="Hash">This will be filled with the unsigned hash</param>
        /// <returns>A signed hash of the input (64bit string)</returns>
        public static string SignHash(string Input,string Key,out string Hash)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(Key);
            ASCIIEncoding Encoding = new ASCIIEncoding();
            byte[] InputArray = Encoding.GetBytes(Input);
            System.Security.Cryptography.SHA1 SHA = System.Security.Cryptography.SHA1.Create();
            byte[]HashBytes=SHA.ComputeHash(InputArray);
            byte[] SignedHash = RSA.SignHash(HashBytes, CryptoConfig.MapNameToOID("SHA1"));
            SHA.Clear();
            RSA.Clear();
            Hash = Convert.ToBase64String(HashBytes);
            return Convert.ToBase64String(SignedHash);
        }

        /// <summary>
        /// Verifies a signed hash against the unsigned version
        /// </summary>
        /// <param name="Hash">The unsigned hash (should be 64bit string)</param>
        /// <param name="SignedHash">The signed hash (should be 64bit string)</param>
        /// <param name="Key">The key to use in decryption</param>
        /// <returns>True if it is verified, false otherwise</returns>
        public static bool VerifyHash(string Hash, string SignedHash, string Key)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(Key);
            byte[] InputArray = Convert.FromBase64String(SignedHash);
            byte[] HashArray = Convert.FromBase64String(Hash);
            bool Result=RSA.VerifyHash(HashArray, CryptoConfig.MapNameToOID("SHA1"), InputArray);
            RSA.Clear();
            return Result;
        }
        #endregion
    }
}