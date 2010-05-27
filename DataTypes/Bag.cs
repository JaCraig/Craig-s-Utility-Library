/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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
    public class Bag<T>:ICollection<T>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Bag()
        {
        }

        #endregion

        #region Private Variables

        protected Dictionary<T, int> Items = new Dictionary<T, int>();

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            try
            {
                if (Items.ContainsKey(item))
                    ++Items[item];
                else
                    Items.Add(item,1);
            }
            catch { throw; }
        }

        public void Clear()
        {
            try
            {
                Items.Clear();
            }
            catch { throw; }
        }

        public bool Contains(T item)
        {
            try
            {
                return Items.ContainsKey(item);
            }
            catch { throw; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="array">Not used</param>
        /// <param name="arrayIndex">Not used</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return Items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return Items.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T Key in this.Items.Keys)
            {
                yield return Key;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (T Key in this.Items.Keys)
            {
                yield return Key;
            }
        }

        #endregion

        #region Properties

        public int this[T index]
        {
            get { try { return Items[index]; } catch { throw; } }
            set { try { Items[index] = value; } catch { throw; } }
        }

        #endregion
    }
}
