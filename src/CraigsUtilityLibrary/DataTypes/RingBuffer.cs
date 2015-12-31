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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Utilities.DataTypes.Comparison;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Implements a ring buffer
    /// </summary>
    /// <typeparam name="T">Type of the data it holds</typeparam>
    public class RingBuffer<T> : ICollection<T>, ICollection
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RingBuffer()
            : this(10, false)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxCapacity">Max capacity for the circular buffer</param>
        /// <param name="allowOverflow">Is overflow allowed (defaults to false)</param>
        public RingBuffer(int maxCapacity, bool allowOverflow = false)
        {
            if (maxCapacity <= 0)
                maxCapacity = 1;
            Count = 0;
            IsReadOnly = false;
            AllowOverflow = allowOverflow;
            MaxCapacity = maxCapacity;
            IsSynchronized = false;
            ReadPosition = 0;
            WritePosition = 0;
            Buffer = new T[maxCapacity];
        }

        private object Root;

        /// <summary>
        /// Is overflow allowed?
        /// </summary>
        public bool AllowOverflow { get; protected set; }

        /// <summary>
        /// Item count for the circular buffer
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// Is this read only?
        /// </summary>
        public bool IsReadOnly { get; protected set; }

        /// <summary>
        /// Is this synchronized?
        /// </summary>
        public bool IsSynchronized { get; protected set; }

        /// <summary>
        /// Maximum capacity
        /// </summary>
        public int MaxCapacity { get; protected set; }

        /// <summary>
        /// Sync root
        /// </summary>
        public object SyncRoot
        {
            get
            {
                if (Root == null)
                    Interlocked.CompareExchange(ref Root, new object(), null);
                return Root;
            }
        }

        /// <summary>
        /// Buffer that the circular buffer uses
        /// </summary>
        protected T[] Buffer { get; set; }

        /// <summary>
        /// Read position
        /// </summary>
        protected int ReadPosition { get; set; }

        /// <summary>
        /// Write position
        /// </summary>
        protected int WritePosition { get; set; }

        /// <summary>
        /// Allows getting an item at a specific position in the buffer
        /// </summary>
        /// <param name="position">Position to look at</param>
        /// <returns>The specified item</returns>
        public T this[int position]
        {
            get
            {
                position %= Count;
                int FinalPosition = (ReadPosition + position) % MaxCapacity;
                return Buffer[FinalPosition];
            }
            set
            {
                position %= Count;
                int FinalPosition = (ReadPosition + position) % MaxCapacity;
                Buffer[FinalPosition] = value;
            }
        }

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>The value as a string</returns>
        public static implicit operator string(RingBuffer<T> value)
        {
            if (value == null)
                return "";
            return value.ToString();
        }

        /// <summary>
        /// Adds an item to the buffer
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(T item)
        {
            if (Count >= MaxCapacity && !AllowOverflow)
                throw new InvalidOperationException("Unable to add item to circular buffer because the buffer is full");
            Buffer[WritePosition] = item;
            ++Count;
            ++WritePosition;
            if (WritePosition >= MaxCapacity)
                WritePosition = 0;
            if (Count >= MaxCapacity)
                Count = MaxCapacity;
        }

        /// <summary>
        /// Adds a number of items to the buffer
        /// </summary>
        /// <param name="items">Items to add</param>
        public void Add(IEnumerable<T> items)
        {
            if (items == null)
                return;
            items.ForEach(x => Add(x));
        }

        /// <summary>
        /// Adds a number of items to the buffer
        /// </summary>
        /// <param name="buffer">Items to add</param>
        /// <param name="count">Number of items to add</param>
        /// <param name="offset">Offset to start at</param>
        public void Add(T[] buffer, int offset, int count)
        {
            buffer = buffer ?? new T[0];
            if (offset < 0)
                offset = 0;
            else if (offset >= buffer.Length)
                offset = buffer.Length - 1;
            if (count < 0)
                count = 0;
            else if (offset + count > buffer.Length)
                count = buffer.Length - offset;
            for (int x = offset; x < offset + count; ++x)
                Add(buffer[x]);
        }

        /// <summary>
        /// Clears the buffer
        /// </summary>
        public void Clear()
        {
            ReadPosition = 0;
            WritePosition = 0;
            Count = 0;
            for (int x = 0; x < MaxCapacity; ++x)
                Buffer[x] = default(T);
        }

        /// <summary>
        /// Determines if the buffer contains the item
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>True if the item is present, false otherwise</returns>
        public bool Contains(T item)
        {
            int y = ReadPosition;
            var Comparer = new GenericEqualityComparer<T>();
            for (int x = 0; x < Count; ++x)
            {
                if (Comparer.Equals(Buffer[y], item))
                    return true;
                ++y;
                if (y >= MaxCapacity)
                    y = 0;
            }
            return false;
        }

        /// <summary>
        /// Copies the buffer to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Array index to start at</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            int y = ReadPosition;
            int y2 = arrayIndex;
            int MaxLength = (array.Length - arrayIndex) < Count ? (array.Length - arrayIndex) : Count;
            for (int x = 0; x < MaxLength; ++x)
            {
                array[y2] = Buffer[y];
                ++y2;
                ++y;
                if (y >= MaxCapacity)
                    y = 0;
            }
        }

        /// <summary>
        /// Copies the buffer to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="index">Array index to start at</param>
        public void CopyTo(Array array, int index)
        {
            int y = ReadPosition;
            int y2 = index;
            int MaxLength = (array.Length - index) < Count ? (array.Length - index) : Count;
            for (int x = 0; x < MaxLength; ++x)
            {
                array.SetValue(Buffer[y], y2);
                ++y2;
                ++y;
                if (y >= MaxCapacity)
                    y = 0;
            }
        }

        /// <summary>
        /// Gets the enumerator for the buffer
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            int y = ReadPosition;
            for (int x = 0; x < Count; ++x)
            {
                yield return Buffer[y];
                ++y;
                if (y >= MaxCapacity)
                    y = 0;
            }
        }

        /// <summary>
        /// Gets the enumerator for the buffer
        /// </summary>
        /// <returns>The enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Reads the next item from the buffer
        /// </summary>
        /// <returns>The next item from the buffer</returns>
        public T Remove()
        {
            if (Count == 0)
                return default(T);
            T ReturnValue = Buffer[ReadPosition];
            Buffer[ReadPosition] = default(T);
            ++ReadPosition;
            ReadPosition %= MaxCapacity;
            --Count;
            return ReturnValue;
        }

        /// <summary>
        /// Reads the next X number of items from the buffer
        /// </summary>
        /// <param name="amount">Number of items to return</param>
        /// <returns>The next X items from the buffer</returns>
        public IEnumerable<T> Remove(int amount)
        {
            if (Count == 0)
                return new List<T>();
            var ReturnValue = new List<T>();
            for (int x = 0; x < amount; ++x)
                ReturnValue.Add(Remove());
            return ReturnValue;
        }

        /// <summary>
        /// Removes an item from the buffer
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(T item)
        {
            int y = ReadPosition;
            var Comparer = new GenericEqualityComparer<T>();
            for (int x = 0; x < Count; ++x)
            {
                if (Comparer.Equals(Buffer[y], item))
                {
                    Buffer[y] = default(T);
                    return true;
                }
                ++y;
                if (y >= MaxCapacity)
                    y = 0;
            }
            return false;
        }

        /// <summary>
        /// Reads the next X number of items and places them in the array passed in
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="offset">Offset to start at</param>
        /// <param name="count">Number of items to read</param>
        /// <returns>The number of items that were read</returns>
        public int Remove(T[] array, int offset, int count)
        {
            array = array ?? new T[0];
            if (offset < 0)
                offset = 0;
            else if (offset >= array.Length)
                offset = array.Length - 1;
            if (count < 0)
                count = 0;
            else if (offset + count > array.Length)
                count = array.Length - offset;
            if (Count == 0)
                return 0;
            int y = ReadPosition;
            int y2 = offset;
            int MaxLength = count < Count ? count : Count;
            for (int x = 0; x < MaxLength; ++x)
            {
                array[y2] = Buffer[y];
                ++y2;
                ++y;
                if (y >= MaxCapacity)
                    y = 0;
            }
            Count -= MaxLength;
            return MaxLength;
        }

        /// <summary>
        /// Skips ahead in the buffer
        /// </summary>
        /// <param name="count">Number of items in the buffer to skip</param>
        public void Skip(int count)
        {
            if (count > Count)
                count = Count;
            ReadPosition += count;
            Count -= count;
            if (ReadPosition >= MaxCapacity)
                ReadPosition %= MaxCapacity;
        }

        /// <summary>
        /// Returns the buffer as a string
        /// </summary>
        /// <returns>The buffer as a string</returns>
        public override string ToString()
        {
            return Buffer.ToString<T>();
        }
    }
}