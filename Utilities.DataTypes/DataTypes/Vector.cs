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
using System;
using System.Collections.Generic;
using Utilities.DataTypes.EventArgs;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.DataTypes
{
    /// <summary>
    /// Vector class
    /// </summary>
    /// <typeparam name="T">The type of item the vector should hold</typeparam>
    public class Vector<T> : IList<T>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Vector()
        {
            DefaultSize = 2;
            Items = new T[DefaultSize];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InitialSize">Initial size of the vector</param>
        public Vector(int InitialSize)
        {
            if (InitialSize < 1) throw new ArgumentOutOfRangeException("InitialSize");
            DefaultSize = InitialSize;
            Items = new T[InitialSize];
        }

        #endregion

        #region IList<T> Members

        /// <summary>
        /// Determines the index of an item
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>The index that an item is at</returns>
        public virtual int IndexOf(T item)
        {
            return Array.IndexOf<T>(this.Items, item, 0, this.NumberItems);
        }

        /// <summary>
        /// Inserts an item into the vector
        /// </summary>
        /// <param name="index">Index to insert at</param>
        /// <param name="item">Item to insert</param>
        public virtual void Insert(int index, T item)
        {
            if (index > this.NumberItems || index < 0) throw new ArgumentOutOfRangeException("index");

            if (this.NumberItems == this.Items.Length)
                Array.Resize<T>(ref this.Items, this.Items.Length * 2);
            if (index < this.NumberItems)
                Array.Copy(this.Items, index, this.Items, index + 1, this.NumberItems - index);
            this.Items[index] = item;
            ++this.NumberItems;
            Changed.Raise(this, new ChangedEventArgs());
        }

        /// <summary>
        /// Removes an item from the vector
        /// </summary>
        /// <param name="index">Index at which the item is removed</param>
        public virtual void RemoveAt(int index)
        {
            if (index > this.NumberItems || index < 0) throw new ArgumentOutOfRangeException("index");

            if (index < this.NumberItems)
                Array.Copy(this.Items, index + 1, this.Items, index, this.NumberItems - (index + 1));
            this.Items[this.NumberItems - 1] = default(T);
            --this.NumberItems;
            Changed.Raise(this, new ChangedEventArgs());
        }

        /// <summary>
        /// Gets an item at the specified index
        /// </summary>
        /// <param name="index">Index to get</param>
        /// <returns>The specified item</returns>
        public virtual T this[int index]
        {
            get
            {
                if (index > this.NumberItems || index < 0) throw new ArgumentOutOfRangeException("index");
                return this.Items[index];
            }
            set
            {
                if (index > this.NumberItems || index < 0) throw new ArgumentOutOfRangeException("index");
                this.Items[index] = value;
                Changed.Raise(this, new ChangedEventArgs());
            }
        }

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// Adds an item to the vector
        /// </summary>
        /// <param name="item">Item to add</param>
        public virtual void Add(T item)
        {
            Insert(this.NumberItems, item);
        }

        /// <summary>
        /// Clears the vector
        /// </summary>
        public virtual void Clear()
        {
            Array.Clear(this.Items, 0, this.Items.Length);
            this.NumberItems = 0;
            Changed.Raise(this, new ChangedEventArgs());
        }

        /// <summary>
        /// Determines if the vector contains an item
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public virtual bool Contains(T item)
        {
            return (this.IndexOf(item) >= 0);
        }

        /// <summary>
        /// Copies the vector to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this.Items, 0, array, arrayIndex, this.NumberItems);
        }

        /// <summary>
        /// Number of items in the vector
        /// </summary>
        public virtual int Count
        {
            get { return this.NumberItems; }
        }

        /// <summary>
        /// Is this read only?
        /// </summary>
        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes an item from the vector
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public virtual bool Remove(T item)
        {
            int Index = this.IndexOf(item);
            if (Index >= 0)
            {
                this.RemoveAt(Index);
                return true;
            }
            return false;
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            for (int x = 0; x < this.NumberItems; ++x)
                yield return this.Items[x];
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            for (int x = 0; x < this.NumberItems; ++x)
                yield return this.Items[x];
        }

        #endregion

        #region Protected Variables/Properties

        /// <summary>
        /// Default size
        /// </summary>
        protected virtual int DefaultSize { get; set; }

        /// <summary>
        /// Internal list of items
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected T[] Items = null;

        /// <summary>
        /// Number of items in the list
        /// </summary>
        protected virtual int NumberItems { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Event that is fired when the vector is changed
        /// </summary>
        public virtual EventHandler<ChangedEventArgs> Changed { get; set; }

        #endregion
    }
}