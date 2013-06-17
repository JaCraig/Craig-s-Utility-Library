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
using System.Collections.Concurrent;
using System.Collections.Generic;
using Utilities.Caching.Interfaces;
using Utilities.DataTypes.ExtensionMethods;

#endregion

namespace Utilities.Caching
{
    /// <summary>
    /// Acts as a cache
    /// </summary>
    public class Cache<KeyType> : ICache<KeyType>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Cache()
        {
            if (InternalCache == null)
                InternalCache = new ConcurrentDictionary<KeyType, object>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Internal cache
        /// </summary>
        protected static ConcurrentDictionary<KeyType, object> InternalCache { get;private set; }

        /// <summary>
        /// Collection of keys
        /// </summary>
        public virtual ICollection<KeyType> Keys
        {
            get
            {
                return InternalCache.Keys;
            }
        }

        /// <summary>
        /// The number of items in the cache
        /// </summary>
        public virtual int Count { get { return InternalCache.Count; } }

        /// <summary>
        /// Gets the item associated with the key
        /// </summary>
        /// <param name="Key">Key</param>
        /// <returns>The item associated with the key</returns>
        public virtual object this[KeyType Key]
        {
            get
            {
                return Get<object>(Key);
            }
            set
            {
                Add(Key, value);
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<object> GetEnumerator()
        {
            foreach (KeyType Key in InternalCache.Keys)
                yield return (object)InternalCache[Key];
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (KeyType Key in InternalCache.Keys)
                yield return InternalCache[Key];
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        public virtual void Clear()
        {
            InternalCache.Clear();
        }

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="Key">Key associated with the item to remove</param>
        public virtual void Remove(KeyType Key)
        {
            if (Exists(Key))
            {
                object TempItem = null;
                InternalCache.TryRemove(Key, out TempItem);
            }
        }

        /// <summary>
        /// Determines if the item exists
        /// </summary>
        /// <param name="Key">The key associated with the item</param>
        /// <returns>True if it does, false otherwise</returns>
        public virtual bool Exists(KeyType Key)
        {
            return InternalCache.ContainsKey(Key);
        }

        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        /// <param name="Key">Key</param>
        /// <param name="Value">Value</param>
        public virtual void Add(KeyType Key, object Value)
        {
            InternalCache.AddOrUpdate(Key, Value, (x, y) => Value);
        }

        /// <summary>
        /// Gets an item from the cache
        /// </summary>
        /// <typeparam name="ValueType">Item type</typeparam>
        /// <param name="Key">Key to search for</param>
        /// <returns>The item associated with the key</returns>
        public virtual ValueType Get<ValueType>(KeyType Key)
        {
            object TempItem = null;
            return InternalCache.TryGetValue(Key, out TempItem) ? TempItem.To(default(ValueType)) : default(ValueType);
        }

        #endregion
    }
}