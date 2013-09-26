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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilities.IO.Messaging.BaseClasses;
using Utilities.IO.Messaging.Interfaces;
using Utilities.DataTypes;
using Utilities.IO.Compression.Interfaces;
using Utilities.IO.Encryption.Interfaces;
using System.Security.Cryptography;
#endregion

namespace Utilities.IO.Encryption
{
    /// <summary>
    /// Compression manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Manager()
        {
            AsymmetricAlgorithms = AppDomain.CurrentDomain.GetAssemblies().Objects<IAsymmetric>();
            HasherAlgorithms = AppDomain.CurrentDomain.GetAssemblies().Objects<IHasher>();
            ShiftAlgorithms = AppDomain.CurrentDomain.GetAssemblies().Objects<IShift>();
            SymmetricAlgorithms = AppDomain.CurrentDomain.GetAssemblies().Objects<ISymmetric>();
        }

        /// <summary>
        /// Asymmetric
        /// </summary>
        public IEnumerable<IAsymmetric> AsymmetricAlgorithms { get; private set; }

        /// <summary>
        /// Hashers
        /// </summary>
        public IEnumerable<IHasher> HasherAlgorithms { get; private set; }

        /// <summary>
        /// Shift
        /// </summary>
        public IEnumerable<IShift> ShiftAlgorithms { get; private set; }

        /// <summary>
        /// Symmetric
        /// </summary>
        public IEnumerable<ISymmetric> SymmetricAlgorithms { get; private set; }

        /// <summary>
        /// Encrypts a byte array
        /// </summary>
        /// <param name="Data">Data to be encrypted</param>
        /// <param name="Key">Password to encrypt with</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)</param>
        /// <param name="Algorithm">Algorithm</param>
        /// <returns>An encrypted byte array</returns>
        public byte[] Encrypt(byte[] Data, DeriveBytes Key,
            string Algorithm = "AES",
            string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            ISymmetric Found = SymmetricAlgorithms.FirstOrDefault(x => x.CanHandle(Algorithm));
            if (Found == null)
                throw new ArgumentException(Algorithm + " not found");
            return Found.Encrypt(Data, Key, Algorithm, InitialVector, KeySize);
        }

        /// <summary>
        /// Decrypts a byte array
        /// </summary>
        /// <param name="Data">Data to be decrypted</param>
        /// <param name="Key">Password to decrypt with</param>
        /// <param name="Algorithm">Algorithm to use for decryption</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)</param>
        /// <returns>A decrypted byte array</returns>
        public byte[] Decrypt(byte[] Data, DeriveBytes Key,
            string Algorithm = "AES",
            string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            ISymmetric Found = SymmetricAlgorithms.FirstOrDefault(x => x.CanHandle(Algorithm));
            if (Found == null)
                throw new ArgumentException(Algorithm + " not found");
            return Found.Decrypt(Data, Key, Algorithm, InitialVector, KeySize);
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
        /// <param name="KeySize">Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)</param>
        /// <param name="Algorithm">Algorithm</param>
        /// <returns>An encrypted byte array</returns>
        public byte[] Encrypt(byte[] Data, string Key,
            string Algorithm,
            string Salt = "Kosher",
            string HashAlgorithm = "SHA1",
            int PasswordIterations = 2,
            string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            ISymmetric Found = SymmetricAlgorithms.FirstOrDefault(x => x.CanHandle(Algorithm));
            if (Found == null)
                throw new ArgumentException(Algorithm + " not found");
            return Found.Encrypt(Data, Key, Algorithm, Salt, HashAlgorithm, PasswordIterations, InitialVector, KeySize);
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
        /// <param name="KeySize">Can be 64 (DES only), 128 (AES), 192 (AES and Triple DES), or 256 (AES)</param>
        /// <returns>A decrypted byte array</returns>
        public byte[] Decrypt(byte[] Data, string Key,
            string Algorithm,
            string Salt = "Kosher",
            string HashAlgorithm = "SHA1",
            int PasswordIterations = 2,
            string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            ISymmetric Found = SymmetricAlgorithms.FirstOrDefault(x => x.CanHandle(Algorithm));
            if (Found == null)
                throw new ArgumentException(Algorithm + " not found");
            return Found.Decrypt(Data, Key, Algorithm, Salt, HashAlgorithm, PasswordIterations, InitialVector, KeySize);
        }


        /// <summary>
        /// Encrypts the data based on the key
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use</param>
        /// <returns>The encrypted data</returns>
        public byte[] Encrypt(byte[] Data, byte[] Key)
        {
            IShift Found = ShiftAlgorithms.FirstOrDefault();
            if (Found == null)
                throw new ArgumentException("No shift based encryption algorithm found");
            return Found.Encrypt(Data, Key);
        }

        /// <summary>
        /// Decrypt the data based on the key
        /// </summary>
        /// <param name="Data">Data to encrypt</param>
        /// <param name="Key">Key to use</param>
        /// <returns>The decrypted data</returns>
        public byte[] Decrypt(byte[] Data, byte[] Key)
        {
            IShift Found = ShiftAlgorithms.FirstOrDefault();
            if (Found == null)
                throw new ArgumentException("No shift based encryption algorithm found");
            return Found.Decrypt(Data, Key);
        }

        /// <summary>
        /// Hashes the data
        /// </summary>
        /// <param name="Data">Data to hash</param>
        /// <param name="Algorithm">Algorithm</param>
        /// <returns>The hashed data</returns>
        public byte[] Hash(byte[] Data, string Algorithm)
        {
            IHasher Found = HasherAlgorithms.FirstOrDefault(x => x.CanHandle(Algorithm));
            if (Found == null)
                throw new ArgumentException(Algorithm + " not found");
            return Found.Hash(Data, Algorithm);
        }

        /// <summary>
        /// Encrypts a string using RSA
        /// </summary>
        /// <param name="Input">Input byte array (should be small as anything over 128 bytes can not be decrypted)</param>
        /// <param name="Key">Key to use for encryption</param>
        /// <returns>An encrypted byte array (64bit string)</returns>
        public byte[] Encrypt(byte[] Input, string Key)
        {
            IAsymmetric Found = AsymmetricAlgorithms.FirstOrDefault();
            if (Found == null)
                throw new ArgumentException("No asymmetric encryption algorithm found");
            return Found.Encrypt(Input, Key);
        }

        /// <summary>
        /// Decrypts a byte array using RSA
        /// </summary>
        /// <param name="Input">Input byte array (should be small as anything over 128 bytes can not be decrypted)</param>
        /// <param name="Key">Key to use for decryption</param>
        /// <returns>A decrypted byte array</returns>
        public byte[] Decrypt(byte[] Input, string Key)
        {
            IAsymmetric Found = AsymmetricAlgorithms.FirstOrDefault();
            if (Found == null)
                throw new ArgumentException("No asymmetric encryption algorithm found");
            return Found.Decrypt(Input, Key);
        }

        /// <summary>
        /// Creates a new set of keys
        /// </summary>
        /// <param name="PrivatePublic">True if private key should be included, false otherwise</param>
        /// <returns>XML representation of the key information</returns>
        public string CreateKey(bool PrivatePublic)
        {
            IAsymmetric Found = AsymmetricAlgorithms.FirstOrDefault();
            if (Found == null)
                throw new ArgumentException("No asymmetric encryption algorithm found");
            return Found.CreateKey(PrivatePublic);
        }

        /// <summary>
        /// Takes a string and creates a signed hash of it
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Key">Key to encrypt/sign with</param>
        /// <param name="Hash">This will be filled with the unsigned hash</param>
        /// <param name="EncodingUsing">Encoding that the input is using (defaults to UTF8)</param>
        /// <returns>A signed hash of the input (64bit string)</returns>
        public string SignHash(string Input, string Key, out string Hash, Encoding EncodingUsing = null)
        {
            IAsymmetric Found = AsymmetricAlgorithms.FirstOrDefault();
            if (Found == null)
                throw new ArgumentException("No asymmetric encryption algorithm found");
            return Found.SignHash(Input, Key, out Hash, EncodingUsing);
        }

        /// <summary>
        /// Verifies a signed hash against the unsigned version
        /// </summary>
        /// <param name="Hash">The unsigned hash (should be 64bit string)</param>
        /// <param name="SignedHash">The signed hash (should be 64bit string)</param>
        /// <param name="Key">The key to use in decryption</param>
        /// <returns>True if it is verified, false otherwise</returns>
        public bool VerifyHash(string Hash, string SignedHash, string Key)
        {
            IAsymmetric Found = AsymmetricAlgorithms.FirstOrDefault();
            if (Found == null)
                throw new ArgumentException("No asymmetric encryption algorithm found");
            return Found.VerifyHash(Hash, SignedHash, Key);
        }

        /// <summary>
        /// String info for the manager
        /// </summary>
        /// <returns>The string info that the manager contains</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Builder.AppendLineFormat("Asymmetric algorithms: {0}", AsymmetricAlgorithms.ToString(x => x.Name));
            Builder.AppendLineFormat("Hasher algorithms: {0}", HasherAlgorithms.ToString(x => x.Name));
            Builder.AppendLineFormat("Shift algorithms: {0}", ShiftAlgorithms.ToString(x => x.Name));
            Builder.AppendLineFormat("Symmetric algorithms: {0}", SymmetricAlgorithms.ToString(x => x.Name));
            return Builder.ToString();
        }
    }
}