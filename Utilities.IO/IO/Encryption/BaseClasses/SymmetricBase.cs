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

#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Cryptography;
using Utilities.DataTypes;
using Utilities.IO.Encryption.Interfaces;

#endregion Usings

namespace Utilities.IO.Encryption.BaseClasses
{
    /// <summary>
    /// Symmetric base class
    /// </summary>
    public abstract class SymmetricBase : ISymmetric
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected SymmetricBase()
        {
            ImplementedAlgorithms = new Dictionary<string, Func<SymmetricAlgorithm>>();
        }

        /// <summary>
        /// Name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Algorithms this implements
        /// </summary>
        protected IDictionary<string, Func<SymmetricAlgorithm>> ImplementedAlgorithms { get; private set; }

        /// <summary>
        /// Can this handle the algorithm specified
        /// </summary>
        /// <param name="Algorithm">The algorithm name</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanHandle(string Algorithm)
        {
            return ImplementedAlgorithms.ContainsKey(Algorithm.ToUpperInvariant());
        }

        /// <summary>
        /// Decrypts a byte array
        /// </summary>
        /// <param name="Data">Data to be decrypted</param>
        /// <param name="Key">Password to decrypt with</param>
        /// <param name="Algorithm">Algorithm to use for decryption</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">
        /// Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)
        /// </param>
        /// <returns>A decrypted byte array</returns>
        public byte[] Decrypt(byte[] Data, DeriveBytes Key, string Algorithm = "AES", string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            if (string.IsNullOrEmpty(InitialVector))
                throw new ArgumentNullException("InitialVector");
            if (Data == null)
                return null;
            using (SymmetricAlgorithm SymmetricKey = GetProvider(Algorithm))
            {
                byte[] PlainTextBytes = new byte[0];
                if (SymmetricKey != null)
                {
                    SymmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(Key.GetBytes(KeySize / 8), InitialVector.ToByteArray()))
                    {
                        using (MemoryStream MemStream = new MemoryStream(Data))
                        {
                            using (CryptoStream CryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read))
                            {
                                PlainTextBytes = CryptoStream.ReadAllBinary();
                            }
                        }
                    }
                    SymmetricKey.Clear();
                }
                return PlainTextBytes;
            }
        }

        /// <summary>
        /// Decrypts a byte array
        /// </summary>
        /// <param name="Data">Data to be decrypted</param>
        /// <param name="Key">Password to decrypt with</param>
        /// <param name="Algorithm">Algorithm to use for decryption</param>
        /// <param name="Salt">Salt to decrypt with</param>
        /// <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
        /// <param name="PasswordIterations">Number of iterations to do</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">
        /// Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)
        /// </param>
        /// <returns>A decrypted byte array</returns>
        public byte[] Decrypt(byte[] Data, string Key, string Algorithm, string Salt = "Kosher", string HashAlgorithm = "SHA1", int PasswordIterations = 2, string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            using (PasswordDeriveBytes TempKey = new PasswordDeriveBytes(Key, Salt.ToByteArray(), HashAlgorithm, PasswordIterations))
            {
                return Decrypt(Data, TempKey, Algorithm, InitialVector, KeySize);
            }
        }

        /// <summary>
        /// Encrypts a byte array
        /// </summary>
        /// <param name="Data">Data to be encrypted</param>
        /// <param name="Key">Password to encrypt with</param>
        /// <param name="Salt">Salt to encrypt with</param>
        /// <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
        /// <param name="PasswordIterations">Number of iterations to do</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">
        /// Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)
        /// </param>
        /// <param name="Algorithm">Algorithm to use</param>
        /// <returns>The encrypted byte array</returns>
        public byte[] Encrypt(byte[] Data, string Key, string Algorithm, string Salt = "Kosher", string HashAlgorithm = "SHA1", int PasswordIterations = 2, string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            using (PasswordDeriveBytes TempKey = new PasswordDeriveBytes(Key, Salt.ToByteArray(), HashAlgorithm, PasswordIterations))
            {
                return Encrypt(Data, TempKey, Algorithm, InitialVector, KeySize);
            }
        }

        /// <summary>
        /// Encrypts a byte array
        /// </summary>
        /// <param name="Data">Data to be encrypted</param>
        /// <param name="Key">Password to encrypt with</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">
        /// Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)
        /// </param>
        /// <param name="Algorithm">Algorithm to use</param>
        /// <returns>The encrypted byte array</returns>
        public byte[] Encrypt(byte[] Data, DeriveBytes Key, string Algorithm = "AES", string InitialVector = "OFRna73m*aze01xY", int KeySize = 256)
        {
            if (string.IsNullOrEmpty(InitialVector))
                throw new ArgumentNullException("InitialVector");
            if (Data == null)
                return null;
            using (SymmetricAlgorithm SymmetricKey = GetProvider(Algorithm))
            {
                byte[] CipherTextBytes = new byte[0];
                if (SymmetricKey != null)
                {
                    SymmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(Key.GetBytes(KeySize / 8), InitialVector.ToByteArray()))
                    {
                        using (MemoryStream MemStream = new MemoryStream())
                        {
                            using (CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write))
                            {
                                CryptoStream.Write(Data, 0, Data.Length);
                                CryptoStream.FlushFinalBlock();
                                CipherTextBytes = MemStream.ToArray();
                            }
                        }
                    }
                    SymmetricKey.Clear();
                }
                return CipherTextBytes;
            }
        }

        /// <summary>
        /// Gets the symmetric algorithm
        /// </summary>
        /// <returns>The symmetric algorithm</returns>
        protected SymmetricAlgorithm GetProvider(string Algorithm)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(Algorithm), "Algorithm");
            Contract.Requires<NullReferenceException>(ImplementedAlgorithms != null, "ImplementedAlgorithms");
            return ImplementedAlgorithms[Algorithm.ToUpperInvariant()]();
        }
    }
}