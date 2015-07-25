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

namespace Utilities.IO.Encryption.Interfaces
{
    /// <summary>
    /// Hasher interface
    /// </summary>
    public interface IHasher
    {
        /// <summary>
        /// Hasher name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Can this handle the algorithm specified
        /// </summary>
        /// <param name="Algorithm">The algorithm name</param>
        /// <returns>True if it can, false otherwise</returns>
        bool CanHandle(string Algorithm);

        /// <summary>
        /// Hashes the data
        /// </summary>
        /// <param name="Data">Data to hash</param>
        /// <param name="Algorithm">Algorithm</param>
        /// <returns>The hashed data</returns>
        byte[] Hash(byte[] Data, string Algorithm);
    }
}