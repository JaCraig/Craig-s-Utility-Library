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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Utilities.DataTypes.Caching.Interfaces;
#endregion

namespace Utilities.DataTypes.Caching.Default
{
    /// <summary>
    /// Built in cache
    /// </summary>
    public class Cache : ICache
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Cache()
        {
            InternalCache = new ConcurrentDictionary<string, object>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Internal cache
        /// </summary>
        protected ConcurrentDictionary<string, object> InternalCache { get; private set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get { return "Default"; } }

        /// <summary>
        /// Keys
        /// </summary>
        public ICollection<string> Keys { get { return InternalCache.Keys; } }

        /// <summary>
        /// Values
        /// </summary>
        public ICollection<object> Values { get { return InternalCache.Values; } }

        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>The object specified</returns>
        public object this[string key]
        {
            get
            {
                object Value = null;
                TryGetValue(key, out Value);
                return Value;
            }
            set
            {
                Add(key, value);
            }
        }

        /// <summary>
        /// The number of items in the cache
        /// </summary>
        public int Count { get { return InternalCache.Count; } }

        /// <summary>
        /// Read only
        /// </summary>
        public bool IsReadOnly { get { return false; } }

        #endregion

        #region Functions

        /// <summary>
        /// Add item to the cache
        /// </summary>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Value to add</param>
        public void Add(string key, object value)
        {
            InternalCache.AddOrUpdate(key, value, (x, y) => value);
        }

        /// <summary>
        /// Checks if the cache contains the key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it is there, false otherwise</returns>
        public bool ContainsKey(string key)
        {
            return InternalCache.ContainsKey(key);
        }

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="key">key to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(string key)
        {
            object Value = null;
            return InternalCache.TryRemove(key, out Value);
        }

        /// <summary>
        /// Attempt to get a value
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="value">Value of the item</param>
        /// <returns>True if it is found, false otherwise</returns>
        public bool TryGetValue(string key, out object value)
        {
            return InternalCache.TryGetValue(key, out value);
        }

        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="item">item to add</param>
        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        public void Clear()
        {
            InternalCache.Clear();
        }

        /// <summary>
        /// Determines if the item is in the cache
        /// </summary>
        /// <param name="item">item to check for</param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, object> item)
        {
            return InternalCache.Contains(item);
        }

        /// <summary>
        /// Copies to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            InternalCache.ToArray().CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes an item from an array
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(KeyValuePair<string, object> item)
        {
            object Value = null;
            return InternalCache.TryRemove(item.Key, out Value);
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return InternalCache.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return InternalCache.GetEnumerator();
        }

        /// <summary>
        /// Disposes the cache
        /// </summary>
        public void Dispose()
        {
            if (InternalCache != null)
            {
                foreach (IDisposable Item in InternalCache.Values.OfType<IDisposable>())
                {
                    Item.Dispose();
                }
                InternalCache.Clear();
                InternalCache = null;
            }
        }

        #endregion
    }
}