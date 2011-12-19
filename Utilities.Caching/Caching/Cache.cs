/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using Utilities.Caching.Interfaces;
#endregion

namespace Utilities.Caching 
{
    /// <summary>
    /// Acts as a cache
    /// </summary>
    public class Cache<KeyType>:ICache<KeyType>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Cache()
        {
            InternalCache = new Dictionary<KeyType, ICacheItem>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Internal cache
        /// </summary>
        protected virtual Dictionary<KeyType, ICacheItem> InternalCache { get; set; }

        public virtual ICollection<KeyType> Keys
        {
            get
            {
                lock (InternalCache)
                {
                    return InternalCache.Keys;
                }
            }
        }

        public virtual int Count { get { return InternalCache.Count; } }

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

        public IEnumerator<object> GetEnumerator()
        {
            lock (InternalCache)
            {
                foreach (KeyType Key in this.InternalCache.Keys)
                    yield return (object)this.InternalCache[Key].Value;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            lock (InternalCache)
            {
                foreach (KeyType Key in this.InternalCache.Keys)
                    yield return this.InternalCache[Key].Value;
            }
        }

        public virtual void Clear()
        {
            lock (InternalCache)
            {
                InternalCache.Clear();
            }
        }

        public virtual void Remove(KeyType Key)
        {
            if (Exists(Key))
            {
                lock (InternalCache)
                {
                    if (Exists(Key))
                    {
                        InternalCache.Remove(Key);
                    }
                }
            }
        }

        public virtual bool Exists(KeyType Key)
        {
            return InternalCache.ContainsKey(Key);
        }

        public virtual void Add(KeyType Key, object Value)
        {
            lock (InternalCache)
            {
                if (Exists(Key))
                {
                    InternalCache[Key].Value = Value;
                }
                else
                {
                    InternalCache.Add(Key, new CacheItem<KeyType>(Key, Value));
                }
            }
        }

        public virtual ValueType Get<ValueType>(KeyType Key)
        {
            lock (InternalCache)
            {
                return Exists(Key) ? (ValueType)InternalCache[Key].Value : default(ValueType);
            }
        }

        #endregion
    }
}
