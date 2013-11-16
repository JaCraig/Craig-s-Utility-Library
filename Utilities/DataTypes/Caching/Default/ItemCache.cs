/*
Copyright (c) 2013 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Web;
using Utilities.DataTypes.Patterns.BaseClasses;
#endregion

namespace Utilities.DataTypes.Caching.Default
{
    /// <summary>
    /// Cache used in ASP.Net for item level cache
    /// </summary>
    public class ItemCache : SafeDisposableBaseClass, ICache
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ItemCache()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get { return "Item"; } }

        /// <summary>
        /// Keys
        /// </summary>
        public ICollection<string> Keys 
        { 
            get 
            { 
                if(HttpContext.Current==null)
                    return new List<string>();
                List<string> Temp = new List<string>();
                foreach (string Key in HttpContext.Current.Items.Keys)
                {
                    Temp.Add(Key);
                }
                return Temp;
            } 
        }

        /// <summary>
        /// Values
        /// </summary>
        public ICollection<object> Values 
        {
            get
            {
                if (HttpContext.Current == null)
                    return new List<object>();
                List<object> Temp = new List<object>();
                foreach (string Key in HttpContext.Current.Items.Keys)
                {
                    Temp.Add(HttpContext.Current.Items[Key]);
                }
                return Temp;
            }
        }

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
        public int Count 
        { 
            get 
            {
                if (HttpContext.Current == null)
                    return 0;
                return HttpContext.Current.Items.Count;
            } 
        }

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
            if (HttpContext.Current == null)
                return;
            HttpContext.Current.Items[key] = value;
        }

        /// <summary>
        /// Checks if the cache contains the key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it is there, false otherwise</returns>
        public bool ContainsKey(string key)
        {
            if (HttpContext.Current == null)
                return false;
            return HttpContext.Current.Items.Contains(key);
        }

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="key">key to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(string key)
        {
            if (HttpContext.Current == null)
                return false;
            HttpContext.Current.Items.Remove(key);
            return true;
        }

        /// <summary>
        /// Attempt to get a value
        /// </summary>
        /// <param name="key">Key to get</param>
        /// <param name="value">Value of the item</param>
        /// <returns>True if it is found, false otherwise</returns>
        public bool TryGetValue(string key, out object value)
        {
            if (HttpContext.Current == null)
            {
                value = null;
                return false;
            }
            value = (ContainsKey(key)) ? HttpContext.Current.Items[key] : null;
            return true;
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
            if (HttpContext.Current == null)
                return;
            HttpContext.Current.Items.Clear();
        }

        /// <summary>
        /// Determines if the item is in the cache
        /// </summary>
        /// <param name="item">item to check for</param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, object> item)
        {
            return ContainsKey(item.Key);
        }

        /// <summary>
        /// Copies to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            if (HttpContext.Current == null)
                return;
            HttpContext.Current.Items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes an item from an array
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(KeyValuePair<string, object> item)
        {
            if (HttpContext.Current == null)
                return false;
            HttpContext.Current.Items.Remove(item.Key);
            return true;
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            if (HttpContext.Current == null)
                return new List<KeyValuePair<string, object>>().GetEnumerator();
            List<KeyValuePair<string, object>> Temp = new List<KeyValuePair<string, object>>();
            foreach (string Key in HttpContext.Current.Items.Keys)
            {
                Temp.Add(new KeyValuePair<string, object>(Key, HttpContext.Current.Items[Key]));
            }
            return Temp.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Disposes of the cache
        /// </summary>
        /// <param name="Managed">Managed or not</param>
        protected override void Dispose(bool Managed)
        {
            if (HttpContext.Current == null)
                return;
            HttpContext.Current.Items.Clear();
        }

        #endregion
    }
}