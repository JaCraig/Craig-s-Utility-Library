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
using Utilities.DataTypes;
using Utilities.IO.Encryption.BaseClasses;

namespace Utilities.IO.Encryption.Default
{
    /// <summary>
    /// Hash
    /// </summary>
    public class Hash : HasherBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Hash()
        {
            ImplementedAlgorithms.Add("SHA1", () => new SHA1CryptoServiceProvider());
            ImplementedAlgorithms.Add("SHA256", () => new SHA256CryptoServiceProvider());
            ImplementedAlgorithms.Add("SHA384", () => new SHA384CryptoServiceProvider());
            ImplementedAlgorithms.Add("SHA512", () => new SHA512CryptoServiceProvider());
            ImplementedAlgorithms.Add("HMACSHA1", () => new HMACSHA1());
            ImplementedAlgorithms.Add("HMACSHA256", () => new HMACSHA256());
            ImplementedAlgorithms.Add("HMACSHA384", () => new HMACSHA384());
            ImplementedAlgorithms.Add("HMACSHA512", () => new HMACSHA512());
            ImplementedAlgorithms.Add("HMACMD5", () => new HMACMD5());
            ImplementedAlgorithms.Add("HMACRIPEMD160", () => new HMACRIPEMD160());
            ImplementedAlgorithms.Add("MACTRIPLEDES", () => new MACTripleDES());
            ImplementedAlgorithms.Add("MD5", () => new MD5CryptoServiceProvider());
            ImplementedAlgorithms.Add("RIPEMD160", () => new RIPEMD160Managed());
        }

        /// <summary>
        /// Name
        /// </summary>
        public override string Name
        {
            get { return ImplementedAlgorithms.ToString(x => x.Key); }
        }
    }
}