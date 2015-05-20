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

namespace Utilities.DataTypes.Comparison
{
    /// <summary>
    /// Simple equality comparer
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class SimpleEqualityComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="comparisonFunction">The comparison function.</param>
        /// <param name="hashFunction">The hash function.</param>
        public SimpleEqualityComparer(Func<T, T, bool> comparisonFunction, Func<T, int> hashFunction)
        {
            ComparisonFunction = comparisonFunction;
            HashFunction = hashFunction;
        }

        /// <summary>
        /// Gets or sets the comparison function.
        /// </summary>
        /// <value>The comparison function.</value>
        protected Func<T, T, bool> ComparisonFunction { get; set; }

        /// <summary>
        /// Gets or sets the hash function.
        /// </summary>
        /// <value>The hash function.</value>
        protected Func<T, int> HashFunction { get; set; }

        /// <summary>
        /// Determines if the two items are equal
        /// </summary>
        /// <param name="x">Object 1</param>
        /// <param name="y">Object 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public bool Equals(T x, T y)
        {
            return ComparisonFunction(x, y);
        }

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <param name="obj">Object to get the hash code of</param>
        /// <returns>The object's hash code</returns>
        public int GetHashCode(T obj)
        {
            return HashFunction(obj);
        }
    }
}