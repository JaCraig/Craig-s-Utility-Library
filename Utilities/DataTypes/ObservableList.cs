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
    public class ObservableList<T> : List<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        public ObservableList()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public ObservableList(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public ObservableList(IEnumerable<T> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// The collection changed
        /// </summary>
        private NotifyCollectionChangedEventHandler collectionChanged_;

        /// <summary>
        /// The property changed
        /// </summary>
        private PropertyChangedEventHandler propertyChanged_;

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                collectionChanged_ -= value;
                collectionChanged_ += value;
            }
            remove
            {
                collectionChanged_ += value;
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
                propertyChanged_ += value;
            }
            remove
            {
                propertyChanged_ += value;
            }
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to be added to the end of the <see
        /// cref="T:System.Collections.Generic.List`1"/>. The value can be null for reference types.
        /// </param>
        public new void Add(T item)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            base.Add(item);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public new void AddRange(IEnumerable<T> collection)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection));
            base.AddRange(collection);
        }

        /// <summary>
        /// Removes all elements from the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        public new void Clear()
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            base.Clear();
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.Generic.List`1"/> at the
        /// specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item"/> should be inserted.
        /// </param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        public new void Insert(int index, T item)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            base.Insert(index, item);
        }

        /// <summary>
        /// Inserts the range.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="collection">The collection.</param>
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection.ToList(), index));
            base.InsertRange(index, collection);
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
        public new bool Remove(T item)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            return base.Remove(item);
        }

        /// <summary>
        /// Removes all.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public new int RemoveAll(Predicate<T> match)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, this.Where(x => match(x))));
            return base.RemoveAll(match);
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public new void RemoveAt(int index)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, this[index], index));
            base.RemoveAt(index);
        }

        /// <summary>
        /// Removes a range of elements from the <see cref="T:System.Collections.Generic.List`1"/>.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public new void RemoveRange(int index, int count)
        {
            NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                                                                                    this.ElementsBetween(index, index + count),
                                                                                    index));
            base.RemoveRange(index, count);
        }

        /// <summary>
        /// Notifies the collection changed.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="changedItem">The changed item.</param>
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