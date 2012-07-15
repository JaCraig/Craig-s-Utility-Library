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
using System.Security.Cryptography;
using System.Text;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Encryption.ExtensionMethods;
#endregion

namespace Utilities.Encryption
{
    /// <summary>
    /// Utility class for doing RSA Encryption
    /// </summary>
    public static class RSAEncryption
    {
        #region Public Static Functions

        /// <summary>
        /// Encrypts a string using RSA
        /// </summary>
        /// <param name="Input">Input string (should be small as anything over 128 bytes can not be decrypted)</param>
        /// <param name="Key">Key to use for encryption</param>
        /// <param name="EncodingUsing">Encoding that the input string uses (defaults to UTF8)</param>
        /// <returns>An encrypted string (64bit string)</returns>
        public static string Encrypt(string Input, string Key, Encoding EncodingUsing = null)
        {
            Input.ThrowIfNullOrEmpty("Input");
            Key.ThrowIfNullOrEmpty("Key");
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(Key);
                byte[] EncryptedBytes = RSA.Encrypt(Input.ToByteArray(EncodingUsing), true);
                RSA.Clear();
                return EncryptedBytes.ToBase64String();
            }
        }

        /// <summary>
        /// Decrypts a string using RSA
        /// </summary>
        /// <param name="Input">Input string (should be small as anything over 128 bytes can not be decrypted)</param>
        /// <param name="Key">Key to use for decryption</param>
        /// <param name="EncodingUsing">Encoding that the result should use (defaults to UTF8)</param>
        /// <returns>A decrypted string</returns>
        public static string Decrypt(string Input, string Key, Encoding EncodingUsing = null)
        {
            Input.ThrowIfNullOrEmpty("Input");
            Key.ThrowIfNullOrEmpty("Key");
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(Key);
                byte[] EncryptedBytes = RSA.Decrypt(Input.FromBase64(), true);
                RSA.Clear();
                return EncryptedBytes.ToEncodedString(EncodingUsing);
            }
        }

        /// <summary>
        /// Creates a new set of keys
        /// </summary>
        /// <param name="PrivatePublic">True if private key should be included, false otherwise</param>
        /// <returns>XML representation of the key information</returns>
        public static string CreateKey(bool PrivatePublic)
        {
            return new RSACryptoServiceProvider().ToXmlString(PrivatePublic);
        }

        /// <summary>
        /// Takes a string and creates a signed hash of it
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Key">Key to encrypt/sign with</param>
        /// <param name="Hash">This will be filled with the unsigned hash</param>
        /// <param name="EncodingUsing">Encoding that the input is using (defaults to UTF8)</param>
        /// <returns>A signed hash of the input (64bit string)</returns>
        public static string SignHash(string Input, string Key, out string Hash, Encoding EncodingUsing = null)
        {
            Input.ThrowIfNullOrEmpty("Input");
            Key.ThrowIfNullOrEmpty("Key");
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(Key);
                byte[] HashBytes = Input.ToByteArray(EncodingUsing).Hash();
                byte[] SignedHash = RSA.SignHash(HashBytes, CryptoConfig.MapNameToOID("SHA1"));
                RSA.Clear();
                Hash = HashBytes.ToBase64String();
                return SignedHash.ToBase64String();
            }
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
            Hash.ThrowIfNullOrEmpty("Hash");
            SignedHash.ThrowIfNullOrEmpty("SignedHash");
            Key.ThrowIfNullOrEmpty("Key");
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(Key);
                byte[] InputArray = SignedHash.FromBase64();
                byte[] HashArray = Hash.FromBase64();
                bool Result = RSA.VerifyHash(HashArray, CryptoConfig.MapNameToOID("SHA1"), InputArray);
                RSA.Clear();
                return Result;
            }
        }

        #endregion
    }
}