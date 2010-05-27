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
using Utilities.Events;
using Utilities.Events.EventArgs;
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
            Items = new T[DefaultSize];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InitialSize">Initial size of the vector</param>
        public Vector(int InitialSize)
        {
            if (InitialSize < 1) throw new ArgumentOutOfRangeException("InitialSize");

            Items = new T[InitialSize];
            DefaultSize = InitialSize;
        }

        #endregion

        #region IList<T> Members

        public int IndexOf(T item)
        {
            try
            {
                return Array.IndexOf<T>(this.Items, item, 0, this.NumberItems);
            }
            catch { throw; }
        }

        public void Insert(int index, T item)
        {
            if (index > this.NumberItems || index < 0) throw new ArgumentOutOfRangeException("index");

            try
            {
                if (this.NumberItems == this.Items.Length)
                {
                    Array.Resize<T>(ref this.Items, this.Items.Length * 2);
                }
                if (index < this.NumberItems)
                {
                    Array.Copy(this.Items, index, this.Items, index + 1, this.NumberItems - index);
                }
                this.Items[index] = item;
                ++this.NumberItems;
                EventHelper.Raise<ChangedEventArgs>(Changed, this, new ChangedEventArgs());
            }
            catch { throw; }
        }

        public void RemoveAt(int index)
        {
            if (index > this.NumberItems || index < 0) throw new ArgumentOutOfRangeException("index");

            try
            {
                if (index < this.NumberItems)
                {
                    Array.Copy(this.Items, index + 1, this.Items, index, this.NumberItems - (index + 1));
                }
                this.Items[this.NumberItems - 1] = default(T);
                --this.NumberItems;
                EventHelper.Raise<ChangedEventArgs>(Changed, this, new ChangedEventArgs());
            }
            catch { throw; }
        }

        public T this[int index]
        {
            get
            {
                try
                {
                    if (index > this.NumberItems || index < 0) throw new ArgumentOutOfRangeException("index");
                    return this.Items[index];
                }
                catch { throw; }
            }
            set
            {
                try
                {
                    if (index > this.NumberItems || index < 0) throw new ArgumentOutOfRangeException("index");
                    this.Items[index] = value;
                    EventHelper.Raise<ChangedEventArgs>(Changed, this, new ChangedEventArgs());
                }
                catch { throw; }
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            try
            {
                Insert(this.NumberItems, item);
            }
            catch { throw; }
        }

        public void Clear()
        {
            try
            {
                Array.Clear(this.Items, 0, this.Items.Length);
                this.NumberItems = 0;
                EventHelper.Raise<ChangedEventArgs>(Changed, this, new ChangedEventArgs());
            }
            catch { throw; }
        }

        public bool Contains(T item)
        {
            try
            {
                return (this.IndexOf(item) >= 0);
            }
            catch { throw; }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            try
            {
                Array.Copy(this.Items, 0, array, arrayIndex, this.NumberItems);
            }
            catch { throw; }
        }

        public int Count
        {
            get { return this.NumberItems; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            try
            {
                int Index = this.IndexOf(item);
                if (Index > 0)
                {
                    this.RemoveAt(Index);
                    return true;
                }
                return false;
            }
            catch { throw; }
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            for (int x = 0; x < this.NumberItems; ++x)
            {
                yield return this.Items[x];
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            for (int x = 0; x < this.NumberItems; ++x)
            {
                yield return this.Items[x];
            }
        }

        #endregion

        #region Protected Variables/Properties

        protected int DefaultSize = 2;
        protected T[] Items = null;
        protected int NumberItems { get; set; }

        #endregion

        #region Events
        public EventHandler<ChangedEventArgs> Changed;
        #endregion
    }
}