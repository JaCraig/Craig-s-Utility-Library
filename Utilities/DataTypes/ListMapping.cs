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

namespace Utilities.DataTypes
{
    /// <summary>
    /// Maps a key to a list of data
    /// </summary>
    /// <typeparam name="T1">Key value</typeparam>
    /// <typeparam name="T2">Type that the list should contain</typeparam>
    [Serializable]
    public class ListMapping<T1, T2> : IDictionary<T1, ConcurrentBag<T2>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ListMapping()
        {
            Items = new ConcurrentDictionary<T1, ConcurrentBag<T2>>();
        }

        /// <summary>
        /// The number of items in the listing
        /// </summary>
        public virtual int Count
        {
            get { return Items.Count; }
        }

        /// <summary>
        /// Not read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// The list of keys within the mapping
        /// </summary>
        public virtual ICollection<T1> Keys
        {
            get { return Items.Keys; }
        }

        /// <summary>
        /// List that contains the list of values
        /// </summary>
        public ICollection<ConcurrentBag<T2>> Values
        {
            get
            {
                List<ConcurrentBag<T2>> Lists = new List<ConcurrentBag<T2>>();
                foreach (T1 Key in Keys)
                    Lists.Add(this[Key]);
                return Lists;
            }
        }

        /// <summary>
        /// Container holding the data
        /// </summary>
        protected ConcurrentDictionary<T1, ConcurrentBag<T2>> Items { get; private set; }

        /// <summary>
        /// Gets a list of values associated with a key
        /// </summary>
        /// <param name="key">Key to look for</param>
        /// <returns>The list of values</returns>
        public virtual ConcurrentBag<T2> this[T1 key]
        {
            get { return Items.GetValue(key, new ConcurrentBag<T2>()); }
            set { Items.SetValue(key, value); }
        }

        /// <summary>
        /// Adds an item to the mapping
        /// </summary>
        /// <param name="Key">Key value</param>
        /// <param name="Value">The value to add</param>
        public virtual void Add(T1 Key, T2 Value)
        {
            Items.AddOrUpdate(Key,
                              x => new ConcurrentBag<T2>(),
                              (x, y) => y)
                 .Add(Value);
        }

        /// <summary>
        /// Adds a key value pair
        /// </summary>
        /// <param name="item">Key value pair to add</param>
        public virtual void Add(KeyValuePair<T1, ConcurrentBag<T2>> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Adds a list of items to the mapping
        /// </summary>
        /// <param name="Key">Key value</param>
        /// <param name="Value">The values to add</param>
        public virtual void Add(T1 Key, ConcurrentBag<T2> Value)
        {
            Items.AddOrUpdate(Key,
                              x => new ConcurrentBag<T2>(),
                              (x, y) => y)
                 .Add(Value);
        }

        /// <summary>
        /// Clears all items from the listing
        /// </summary>
        public virtual void Clear()
        {
            Items.Clear();
        }

        /// <summary>
        /// Does this contain the key value pairs?
        /// </summary>
        /// <param name="item">Key value pair to check</param>
        /// <returns>True if it exists, false otherwise</returns>
        public virtual bool Contains(KeyValuePair<T1, ConcurrentBag<T2>> item)
        {
            if (!ContainsKey(item.Key))
                return false;
            if (!Contains(item.Key, item.Value))
                return false;
            return true;
        }

        /// <summary>
        /// Does the list mapping contain the key value pairs?
        /// </summary>
        /// <param name="Key">Key value</param>
        /// <param name="Values">Value</param>
        /// <returns>True if it exists, false otherwise</returns>
        public virtual bool Contains(T1 Key, ConcurrentBag<T2> Values)
        {
            if (!ContainsKey(Key))
                return false;
            foreach (T2 Value in Values)
                if (!Contains(Key, Value))
                    return false;
            return true;
        }

        /// <summary>
        /// Does the list mapping contain the key value pair?
        /// </summary>
        /// <param name="Key">Key</param>
        /// <param name="Value">Value</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool Contains(T1 Key, T2 Value)
        {
            if (!ContainsKey(Key))
                return false;
            if (!this[Key].Contains(Value))
                return false;
            return true;
        }

        /// <summary>
        /// Determines if a key exists
        /// </summary>
        /// <param name="key">Key to check on</param>
        /// <returns>True if it exists, false otherwise</returns>
        public virtual bool ContainsKey(T1 key)
        {
            return Items.ContainsKey(key);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">array index</param>
        public void CopyTo(KeyValuePair<T1, ConcurrentBag<T2>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator for this object</returns>
        public IEnumerator<KeyValuePair<T1, ConcurrentBag<T2>>> GetEnumerator()
        {
            foreach (T1 Key in Keys)
                yield return new KeyValuePair<T1, ConcurrentBag<T2>>(Key, this[Key]);
        }

        /// <summary>
        /// Remove a list of items associated with a key
        /// </summary>
        /// <param name="key">Key to use</param>
        /// <returns>True if the key is found, false otherwise</returns>
        public virtual bool Remove(T1 key)
        {
            ConcurrentBag<T2> Value = new ConcurrentBag<T2>();
            return Items.TryRemove(key, out Value);
        }

        /// <summary>
        /// Removes a key value pair from the list mapping
        /// </summary>
        /// <param name="item">items to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public virtual bool Remove(KeyValuePair<T1, ConcurrentBag<T2>> item)
        {
            if (!Contains(item))
                return false;
            foreach (T2 Value in item.Value)
                if (!Remove(item.Key, Value))
                    return false;
            return true;
        }

        /// <summary>
        /// Removes a key value pair from the list mapping
        /// </summary>
        /// <param name="Key">Key to remove</param>
        /// <param name="Value">Value to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public virtual bool Remove(T1 Key, T2 Value)
        {
            if (!Contains(Key, Value))
                return false;
            List<T2> TempValue = Items[Key].ToList(z => z);
            TempValue.Remove(Value);
            Items.AddOrUpdate(Key,
                new ConcurrentBag<T2>(TempValue),
                (x, y) => new ConcurrentBag<T2>(TempValue));
            if (this[Key].Count == 0)
                Remove(Key);
            return true;
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator for this object</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (T1 Key in Keys)
                yield return this[Key];
        }

        /// <summary>
        /// Tries to get the value associated with the key
        /// </summary>
        /// <param name="Key">Key value</param>
        /// <param name="Value">The values getting</param>
        /// <returns>True if it was able to get the value, false otherwise</returns>
        public virtual bool TryGetValue(T1 Key, out ConcurrentBag<T2> Value)
        {
            Value = new ConcurrentBag<T2>();
            return Items.TryGetValue(Key, out Value);
        }
    }
}