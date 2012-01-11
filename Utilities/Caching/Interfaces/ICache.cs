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
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace Utilities.Caching.Interfaces
{
    /// <summary>
    /// Cache interface
    /// </summary>
    public interface ICache<KeyType> : IEnumerable<object>
    {
        #region Functions

        /// <summary>
        /// Clears the cache
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="Key">Key to remove</param>
        void Remove(KeyType Key);

        /// <summary>
        /// Checks if a key exists in the cache
        /// </summary>
        /// <param name="Key">Key to check</param>
        /// <returns>True if it exists, false otherwise</returns>
        bool Exists(KeyType Key);

        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="Key">Key</param>
        /// <param name="Value">Value</param>
        void Add(KeyType Key, object Value);

        /// <summary>
        /// Gets a value
        /// </summary>
        /// <typeparam name="ValueType">Value type</typeparam>
        /// <param name="Key">Key</param>
        /// <returns>The value specified by the key</returns>
        ValueType Get<ValueType>(KeyType Key);

        #endregion

        #region Properties

        /// <summary>
        /// Keys used in the cache
        /// </summary>
        ICollection<KeyType> Keys { get; }

        /// <summary>
        /// Number of items in the cache
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets a specific item based on the key
        /// </summary>
        /// <param name="Key">Key to use</param>
        /// <returns>The value associated with the key</returns>
        object this[KeyType Key] { get; set; }

        #endregion
    }
}
