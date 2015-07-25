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

using System.Security.Cryptography;
using System.Text;
using Utilities.IO.Encryption.Interfaces;

namespace Utilities.IO.Encryption.BaseClasses
{
    /// <summary>
    /// Asymmetric base class
    /// </summary>
    public abstract class AsymmetricBase : IAsymmetric
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected AsymmetricBase()
        {
        }

        /// <summary>
        /// Name of the encryptor
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Creates a new set of keys
        /// </summary>
        /// <param name="PrivatePublic">True if private key should be included, false otherwise</param>
        /// <returns>XML representation of the key information</returns>
        public string CreateKey(bool PrivatePublic)
        {
            using (AsymmetricAlgorithm Provider = GetProvider())
            {
                return Provider == null ? "" : Provider.ToXmlString(PrivatePublic);
            }
        }

        /// <summary>
        /// Decrypts a byte array using RSA
        /// </summary>
        /// <param name="Input">
        /// Input byte array (should be small as anything over 128 bytes can not be decrypted)
        /// </param>
        /// <param name="Key">Key to use for decryption</param>
        /// <returns>A decrypted byte array</returns>
        public abstract byte[] Decrypt(byte[] Input, string Key);

        /// <summary>
        /// Encrypts a string using RSA
        /// </summary>
        /// <param name="Input">
        /// Input byte array (should be small as anything over 128 bytes can not be decrypted)
        /// </param>
        /// <param name="Key">Key to use for encryption</param>
        /// <returns>An encrypted byte array (64bit string)</returns>
        public abstract byte[] Encrypt(byte[] Input, string Key);

        /// <summary>
        /// Takes a string and creates a signed hash of it
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Key">Key to encrypt/sign with</param>
        /// <param name="Hash">This will be filled with the unsigned hash</param>
        /// <param name="EncodingUsing">Encoding that the input is using (defaults to UTF8)</param>
        /// <returns>A signed hash of the input (64bit string)</returns>
        public abstract string SignHash(string Input, string Key, out string Hash, Encoding EncodingUsing = null);

        /// <summary>
        /// Verifies a signed hash against the unsigned version
        /// </summary>
        /// <param name="Hash">The unsigned hash (should be 64bit string)</param>
        /// <param name="SignedHash">The signed hash (should be 64bit string)</param>
        /// <param name="Key">The key to use in decryption</param>
        /// <returns>True if it is verified, false otherwise</returns>
        public abstract bool VerifyHash(string Hash, string SignedHash, string Key);

        /// <summary>
        /// Gets the provider used
        /// </summary>
        /// <returns>Asymmetric algorithm</returns>
        protected abstract AsymmetricAlgorithm GetProvider();
    }
}