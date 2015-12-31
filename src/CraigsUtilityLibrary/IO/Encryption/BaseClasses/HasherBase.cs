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

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using Utilities.IO.Encryption.Interfaces;

namespace Utilities.IO.Encryption.BaseClasses
{
    /// <summary>
    /// Hasher base class
    /// </summary>
    public abstract class HasherBase : IHasher
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected HasherBase()
        {
            ImplementedAlgorithms = new Dictionary<string, Func<HashAlgorithm>>();
        }

        /// <summary>
        /// Name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Algorithms this implements
        /// </summary>
        protected IDictionary<string, Func<HashAlgorithm>> ImplementedAlgorithms { get; private set; }

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
        /// Hashes the data
        /// </summary>
        /// <param name="Data">Data to hash</param>
        /// <param name="Algorithm">Algorithm to use</param>
        /// <returns>The hashed version of the data</returns>
        public byte[] Hash(byte[] Data, string Algorithm)
        {
            if (Data == null)
                return null;
            using (HashAlgorithm Hasher = GetAlgorithm(Algorithm))
            {
                byte[] HashedArray = new byte[0];
                if (Hasher != null)
                {
                    HashedArray = Hasher.ComputeHash(Data);
                    Hasher.Clear();
                }
                return HashedArray;
            }
        }

        /// <summary>
        /// Gets the hash algorithm that the system uses
        /// </summary>
        /// <param name="Algorithm">Algorithm</param>
        /// <returns>The hash algorithm</returns>
        protected HashAlgorithm GetAlgorithm(string Algorithm)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(Algorithm), "Algorithm");
            Contract.Requires<NullReferenceException>(ImplementedAlgorithms != null, "ImplementedAlgorithms");
            return ImplementedAlgorithms[Algorithm.ToUpperInvariant()]();
        }
    }
}