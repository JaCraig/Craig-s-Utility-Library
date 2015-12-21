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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Observable List class
    /// </summary>
    /// <typeparam name="T">Object type that the list holds</typeparam>
    public class ObservableList<T> : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a virtual instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        public ObservableList()
        {
            BaseList = new List<T>();
        }

        /// <summary>
        /// Initializes a virtual instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        /// <param name="capacity">
        /// The number of elements that the virtual list can initially store.
        /// </param>
        public ObservableList(int capacity)
        {
            BaseList = new List<T>(capacity);
        }

        /// <summary>
        /// Initializes a virtual instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public ObservableList(IEnumerable<T> collection)
        {
            BaseList = new List<T>(collection);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        public int Count => BaseList.Count;

        /// <summary>
        /// Gets a value indicating whether the <see
        /// cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets or sets the base list.
        /// </summary>
        /// <value>The base list.</value>
        private List<T> BaseList { get; set; }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return BaseList[index];
            }

            set
            {
                NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, BaseList[index]));
                BaseList[index] = value;
            }
        }

        /// <summary>
        /// The collection changed
        /// </summary>
        private NotifyCollectionChangedEventHandler collectionChanged_;

        /// <summary>
        /// The delegates_
        /// </summary>
        private List<NotifyCollectionChangedEventHandler> CollectionChangedDelegates = new List<NotifyCollectionChangedEventHandler>();

        /// <summary>
        /// The property changed
        /// </summary>
        private PropertyChangedEventHandler propertyChanged_;

        /// <summary>
        /// The property changed delegates
        /// </summary>
        private List<PropertyChangedEventHandler> PropertyChangedDelegates = new List<PropertyChangedEventHandler>();

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                collectionChanged_ -= value;
                CollectionChangedDelegates.Remove(value);
                collectionChanged_ += value;
                CollectionChangedDelegates.Add(value);
            }
            remove
            {
                collectionChanged_ += value;
                CollectionChangedDelegates.Add(value);
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                propertyChanged_ -= value;
                PropertyChangedDelegates.Remove(value);
                propertyChanged_ += value;
                PropertyChangedDelegates.Add(value);
            }
            remove
            {
                propertyChanged_ += value;
                PropertyChangedDelegates.Add(value);
            }
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to be added to the end of the <see
        /// cref="T:System.Collections.Generic.List`1"/>. The value can be null for reference types.
        /// </param>
        public virtual void Add(T item)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            NotifyPropertyChanged("Count");
            BaseList.Add(item);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public virtual void AddRange(IEnumerable<T> collection)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection));
            NotifyPropertyChanged("Count");
            BaseList.AddRange(collection);
        }

        /// <summary>
        /// Removes all elements from the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        public virtual void Clear()
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            NotifyPropertyChanged("Count");
            BaseList.Clear();
        }

        /// <summary>
        /// Clears the delegates from the list.
        /// </summary>
        public void ClearDelegates()
        {
            PropertyChangedDelegates.ForEach(x => propertyChanged_ -= x);
            PropertyChangedDelegates.Clear();
            CollectionChangedDelegates.ForEach(x => collectionChanged_ -= x);
            CollectionChangedDelegates.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains
        /// a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see
        /// cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            return BaseList.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            BaseList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate
        /// through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return BaseList.GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        /// <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            return BaseList.IndexOf(item);
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.Generic.List`1"/> at the
        /// specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item"/> should be inserted.
        /// </param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        public virtual void Insert(int index, T item)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            NotifyPropertyChanged("Count");
            BaseList.Insert(index, item);
        }

        /// <summary>
        /// Inserts the range.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="collection">The collection.</param>
        public virtual void InsertRange(int index, IEnumerable<T> collection)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection.ToList(), index));
            NotifyPropertyChanged("Count");
            BaseList.InsertRange(index, collection);
        }

        /// <summary>
        /// Notifies the list that an item in the list has been modified.
        /// </summary>
        /// <param name="itemChanged">The item that was changed.</param>
        public void NotifyObjectChanged(object itemChanged)
        {
            var Handler = collectionChanged_;
            if (Handler != null)
                Handler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, itemChanged, itemChanged));
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the <see cref="T:System.Collections.Generic.List`1"/>. The
        /// value can be null for reference types.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/> is successfully removed; otherwise, false. This method
        /// also returns false if <paramref name="item"/> was not found in the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </returns>
        public virtual bool Remove(T item)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            NotifyPropertyChanged("Count");
            return BaseList.Remove(item);
        }

        /// <summary>
        /// Removes all.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public virtual int RemoveAll(Predicate<T> match)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, this.Where(x => match(x))));
            NotifyPropertyChanged("Count");
            return BaseList.RemoveAll(match);
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public virtual void RemoveAt(int index)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, this[index], index));
            NotifyPropertyChanged("Count");
            BaseList.RemoveAt(index);
        }

        /// <summary>
        /// Removes a range of elements from the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public virtual void RemoveRange(int index, int count)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                                                                                    this.ElementsBetween(index, index + count),
                                                                                    index));
            NotifyPropertyChanged("Count");
            BaseList.RemoveRange(index, count);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate
        /// through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return BaseList.GetEnumerator();
        }

        /// <summary>
        /// Notifies the collection changed.
        /// </summary>
        /// <param name="args">
        /// The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.
        /// </param>
        protected void NotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var Handler = collectionChanged_;
            if (Handler != null)
                Handler(this, args);
        }

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            var Handler = propertyChanged_;
            if (Handler != null)
                Handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}