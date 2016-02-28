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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Utilities.DataTypes.Caching.BaseClasses;

namespace Utilities.DataTypes.Caching.Default
{
    /// <summary>
    /// Built in cache
    /// </summary>
    public class Cache : CacheBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Cache()
        {
            InternalCache = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// The number of items in the cache
        /// </summary>
        public override int Count { get { return InternalCache.Count; } }

        /// <summary>
        /// Keys
        /// </summary>
        public override ICollection<string> Keys { get { return InternalCache.Keys; } }

        /// <summary>
        /// Name
        /// </summary>
        public override string Name { get { return "Default"; } }

        /// <summary>
        /// Values
        /// </summary>
        public override ICollection<object> Values { get { return InternalCache.Values; } }

        /// <summary>
        /// Internal cache
        /// </summary>
        protected ConcurrentDictionary<string, object> InternalCache { get; private set; }

        /// <summary>
        /// Add item to the cache
        /// </summary>
        /// <param name="key">Key of the item</param>
        /// <param name="value">Value to add</param>
        public override void Add(string key, object value)
        {
            InternalCache.AddOrUpdate(key, x => value, (x, y) => value);
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        public override void Clear()
        {
            InternalCache.Clear();
        }

        /// <summary>
        /// Determines if the item is in the cache
        /// </summary>
        /// <param name="item">item to check for</param>
        /// <returns></returns>
        public override bool Contains(KeyValuePair<string, object> item)
        {
            return InternalCache.Contains(item);
        }

        /// <summary>
        /// Checks if the cache contains the key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it is there, false otherwise</returns>
        public override bool ContainsKey(string key)
        {
            return InternalCache.ContainsKey(key);
        }

        /// <summary>
        /// Copies to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public override void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            InternalCache.ToArray().CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return InternalCache.GetEnumerator();
        }

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="key">key to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public override bool Remove(string key)
        {
            object Value = null;
            return InternalCache.TryRemove(key, out Value);
        }

        /// <summary>
        /// Removes an item from an array
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public override bool Remove(KeyValuePair<string, object> item)
        {
            object Value = null;
            return InternalCache.TryRemove(item.Key, out Value);
        }

        /// <summary>
        /// Attempt to get a value
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="value">Value of the item</param>
        /// <returns>True if it is found, false otherwise</returns>
        public override bool TryGetValue(string key, out object value)
        {
            return InternalCache.TryGetValue(key, out value);
        }

        /// <summary>
        /// Disposes the cache
        /// </summary>
        /// <param name="Managed">Managed objects or just unmanaged</param>
        protected override void Dispose(bool Managed)
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
    }
}