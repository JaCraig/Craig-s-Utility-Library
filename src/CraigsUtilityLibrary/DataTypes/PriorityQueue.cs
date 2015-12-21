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
using System.Linq;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Helper class that implements a priority queue
    /// </summary>
    /// <typeparam name="T">The type of the values placed in the queue</typeparam>
    public class PriorityQueue<T> : IDictionary<int, ICollection<T>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PriorityQueue()
        {
            HighestKey = int.MinValue;
            Items = new Dictionary<int, ICollection<T>>();
        }

        /// <summary>
        /// The number of items in the listing
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// Not read only
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// The list of keys within the mapping
        /// </summary>
        public ICollection<int> Keys => Items.Keys;

        /// <summary>
        /// List that contains the list of values
        /// </summary>
        public ICollection<ICollection<T>> Values
        {
            get
            {
                var Lists = new List<ICollection<T>>();
                foreach (int Key in Keys)
                    Lists.Add(this[Key]);
                return Lists;
            }
        }

        /// <summary>
        /// Highest value key
        /// </summary>
        protected int HighestKey { get; set; }

        /// <summary>
        /// Container holding the data
        /// </summary>
        protected IDictionary<int, ICollection<T>> Items { get; private set; }

        /// <summary>
        /// Gets a list of values associated with a key
        /// </summary>
        /// <param name="key">Key to look for</param>
        /// <returns>The list of values</returns>
        public ICollection<T> this[int key]
        {
            get { return Items.GetValue(key, new List<T>()); }
            set { Items.SetValue(key, value); }
        }

        /// <summary>
        /// Adds an item to the mapping
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="value">The value to add</param>
        public void Add(int key, T value)
        {
            if (key > HighestKey)
                HighestKey = key;
            Items.SetValue(key, Items.GetValue(key, new List<T>()).Add(new T[] { value }));
        }

        /// <summary>
        /// Adds a key value pair
        /// </summary>
        /// <param name="item">Key value pair to add</param>
        public void Add(KeyValuePair<int, ICollection<T>> item)
        {
            if (item.Key > HighestKey)
                HighestKey = item.Key;
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Adds a list of items to the mapping
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="value">The values to add</param>
        public void Add(int key, ICollection<T> value)
        {
            if (key > HighestKey)
                HighestKey = key;
            Items.SetValue(key, Items.GetValue(key, new List<T>()).Add(value));
        }

        /// <summary>
        /// Clears all items from the listing
        /// </summary>
        public void Clear()
        {
            Items.Clear();
        }

        /// <summary>
        /// Does this contain the key value pairs?
        /// </summary>
        /// <param name="item">Key value pair to check</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool Contains(KeyValuePair<int, ICollection<T>> item)
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
        /// <param name="key">Key value</param>
        /// <param name="values">Value</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool Contains(int key, ICollection<T> values)
        {
            if (!ContainsKey(key))
                return false;
            foreach (T Value in values)
                if (!Contains(key, Value))
                    return false;
            return true;
        }

        /// <summary>
        /// Does the list mapping contain the key value pair?
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool Contains(int key, T value)
        {
            if (!ContainsKey(key))
                return false;
            if (!this[key].Contains(value))
                return false;
            return true;
        }

        /// <summary>
        /// Determines if a key exists
        /// </summary>
        /// <param name="key">Key to check on</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool ContainsKey(int key)
        {
            return Items.ContainsKey(key);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">array index</param>
        public void CopyTo(KeyValuePair<int, ICollection<T>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator for this object</returns>
        public IEnumerator<KeyValuePair<int, ICollection<T>>> GetEnumerator()
        {
            foreach (int Key in Keys)
                yield return new KeyValuePair<int, ICollection<T>>(Key, this[Key]);
        }

        /// <summary>
        /// Peek at the next thing in the queue
        /// </summary>
        /// <returns>The next item in queue or default(T) if it is empty</returns>
        public T Peek()
        {
            if (Items.ContainsKey(HighestKey))
                return Items[HighestKey].FirstOrDefault();
            return default(T);
        }

        /// <summary>
        /// Removes an item from the queue and returns it
        /// </summary>
        /// <returns>The next item in the queue</returns>
        public T Pop()
        {
            T ReturnValue = default(T);
            if (Items.ContainsKey(HighestKey) && Items[HighestKey].Count > 0)
            {
                ReturnValue = Items[HighestKey].FirstOrDefault();
                Remove(HighestKey, ReturnValue);
                if (!ContainsKey(HighestKey))
                {
                    HighestKey = int.MinValue;
                    foreach (int Key in Items.Keys)
                        if (Key > HighestKey)
                            HighestKey = Key;
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Remove a list of items associated with a key
        /// </summary>
        /// <param name="key">Key to use</param>
        /// <returns>True if the key is found, false otherwise</returns>
        public bool Remove(int key)
        {
            return Items.Remove(key);
        }

        /// <summary>
        /// Removes a key value pair from the list mapping
        /// </summary>
        /// <param name="item">items to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(KeyValuePair<int, ICollection<T>> item)
        {
            if (!Contains(item))
                return false;
            foreach (T Value in item.Value)
                if (!Remove(item.Key, Value))
                    return false;
            return true;
        }

        /// <summary>
        /// Removes a key value pair from the list mapping
        /// </summary>
        /// <param name="key">Key to remove</param>
        /// <param name="Value">Value to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(int key, T Value)
        {
            if (!Contains(key, Value))
                return false;
            Items[key].Remove(Value);
            if (this[key].Count == 0)
                Remove(key);
            return true;
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator for this object</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (int Key in Keys)
                yield return this[Key];
        }

        /// <summary>
        /// Tries to get the value associated with the key
        /// </summary>
        /// <param name="Key">Key value</param>
        /// <param name="value">The values getting</param>
        /// <returns>True if it was able to get the value, false otherwise</returns>
        public bool TryGetValue(int Key, out ICollection<T> value)
        {
            value = new List<T>();
            return Items.TryGetValue(Key, out value);
        }
    }
}