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
using System.Collections.Generic;
using System.Linq;


#endregion

namespace Utilities.DataTypes
{
    /// <summary>
    /// Used to count the number of times something is added to the list
    /// </summary>
    /// <typeparam name="T">Type of data within the bag</typeparam>
    public class Bag<T> : ICollection<T>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Bag()
        {
            Items = new Dictionary<T, int>();
        }

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// Adds an item to the bag
        /// </summary>
        /// <param name="item">Item to add</param>
        public virtual void Add(T item)
        {
            if (Items.ContainsKey(item))
                ++Items[item];
            else
                Items.Add(item, 1);
        }

        /// <summary>
        /// Clears the bag
        /// </summary>
        public virtual void Clear()
        {
            Items.Clear();
        }

        /// <summary>
        /// Determines if the bag contains an item
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>True if it does, false otherwise</returns>
        public virtual bool Contains(T item)
        {
            return Items.ContainsKey(item);
        }

        /// <summary>
        /// Copies the bag to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this.Items.ToList().ToArray(x => x.Key), 0, array, arrayIndex, this.Count);
        }

        /// <summary>
        /// Number of items in the bag
        /// </summary>
        public virtual int Count
        {
            get { return Items.Count; }
        }

        /// <summary>
        /// Is this read only?
        /// </summary>
        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes an item from the bag
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public virtual bool Remove(T item)
        {
            return Items.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            foreach (T Key in this.Items.Keys)
                yield return Key;
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (T Key in this.Items.Keys)
                yield return Key;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a specified item
        /// </summary>
        /// <param name="index">Item to get</param>
        /// <returns>The number of this item in the bag</returns>
        public virtual int this[T index]
        {
            get { return Items[index]; }
            set { Items[index] = value; }
        }

        /// <summary>
        /// Actual internal container
        /// </summary>
        protected Dictionary<T, int> Items { get; private set; }

        #endregion
    }
}