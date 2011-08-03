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

#endregion

namespace Utilities.DataTypes
{
    /// <summary>
    /// Used to count the number of times something is added to the list
    /// </summary>
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

        public virtual void Add(T item)
        {
            if (Items.ContainsKey(item))
                ++Items[item];
            else
                Items.Add(item, 1);
        }

        public virtual void Clear()
        {
            Items.Clear();
        }

        public virtual bool Contains(T item)
        {
            return Items.ContainsKey(item);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="array">Not used</param>
        /// <param name="arrayIndex">Not used</param>
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public virtual int Count
        {
            get { return Items.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool Remove(T item)
        {
            return Items.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public virtual IEnumerator<T> GetEnumerator()
        {
            foreach (T Key in this.Items.Keys)
                yield return Key;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (T Key in this.Items.Keys)
                yield return Key;
        }

        #endregion

        #region Properties

        public virtual int this[T index]
        {
            get { return Items[index]; }
            set { Items[index] = value; }
        }

        /// <summary>
        /// Actual internal container
        /// </summary>
        protected virtual Dictionary<T, int> Items { get; set; }

        #endregion
    }
}