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
using System.Security.Cryptography;
using System.Text;
using Utilities.DataTypes.ExtensionMethods;

#endregion

namespace Utilities.Encryption.ExtensionMethods
{
    /// <summary>
    /// Hash based extensions
    /// </summary>
    public static class HashExtensions
    {
        #region Functions

        #region GenerateSalt

        /// <summary>
        /// Generates salt
        /// </summary>
        /// <param name="Random">Randomization object</param>
        /// <param name="Size">Size of the salt byte array</param>
        /// <returns>A byte array as salt</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "Random")]
        public static byte[] GenerateSalt(this System.Random Random, int Size)
        {
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
        public static byte[] Hash(this byte[] Data, HashAlgorithm Algorithm = null)
        {
            if (Data==null)
                return null;
            using (HashAlgorithm Hasher = Algorithm.NullCheck(()=>new SHA1CryptoServiceProvider()))
            {
                byte[] HashedArray = Hasher.ComputeHash(Data);
                Hasher.Clear();
                return HashedArray;
            }
        }

        /// <summary>
        /// Computes the hash of a string
        /// </summary>
        /// <param name="Data">string to hash</param>
        /// <param name="Algorithm">Algorithm to use (defaults to SHA1)</param>
        /// <param name="EncodingUsing">Encoding used by the string (defaults to UTF8)</param>
        /// <returns>The hash of the string</returns>
        public static string Hash(this string Data, HashAlgorithm Algorithm = null, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Data))
                return "";
            return BitConverter.ToString(Data.ToByteArray(EncodingUsing).Hash(Algorithm)).Replace("-", "").Encode(null, EncodingUsing);
        }

        #endregion

        #endregion
    }
}