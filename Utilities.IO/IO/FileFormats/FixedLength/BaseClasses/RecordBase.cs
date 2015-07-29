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

using System.Collections.Generic;
using System.Text;
using Utilities.IO.FileFormats.FixedLength.Interfaces;

namespace Utilities.IO.FileFormats.FixedLength.BaseClasses
{
    /// <summary>
    /// Record base class
    /// </summary>
    /// <typeparam name="T">Field type</typeparam>
    public abstract class RecordBase<T> : IRecord<T>, IList<IField<T>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected RecordBase()
        {
            Fields = new List<IField<T>>();
        }

        /// <summary>
        /// Number of Fields
        /// </summary>
        public int Count
        {
            get { return Fields.Count; }
        }

        /// <summary>
        /// Is the file read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return Fields.IsReadOnly; }
        }

        /// <summary>
        /// Length
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// The list of fields
        /// </summary>
        protected IList<IField<T>> Fields { get; private set; }

        /// <summary>
        /// Individual records
        /// </summary>
        /// <param name="Position">The record that you want to get</param>
        /// <returns>The record requested</returns>
        public IField<T> this[int Position]
        {
            get { return Fields[Position]; }
            set { Fields[Position] = value; }
        }

        /// <summary>
        /// Adds a Field to the file
        /// </summary>
        /// <param name="item">Field to add</param>
        public void Add(IField<T> item)
        {
            Fields.Add(item);
        }

        /// <summary>
        /// Clears the file
        /// </summary>
        public void Clear()
        {
            Fields.Clear();
        }

        /// <summary>
        /// Determines if the file contains a Field
        /// </summary>
        /// <param name="item">Field to check for</param>
        /// <returns>True if it does, false otherwise</returns>
        public bool Contains(IField<T> item)
        {
            return Fields.Contains(item);
        }

        /// <summary>
        /// Copies the delimited file to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public void CopyTo(IField<T>[] array, int arrayIndex)
        {
            Fields.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the enumerator for the file
        /// </summary>
        /// <returns>The enumerator for this file</returns>
        public IEnumerator<IField<T>> GetEnumerator()
        {
            return Fields.GetEnumerator();
        }

        /// <summary>
        /// Index of a specific Field
        /// </summary>
        /// <param name="item">Field to search for</param>
        /// <returns>The index of a specific Field</returns>
        public int IndexOf(IField<T> item)
        {
            return Fields.IndexOf(item);
        }

        /// <summary>
        /// Inserts a Field at a specific index
        /// </summary>
        /// <param name="index">Index to insert at</param>
        /// <param name="item">Field to insert</param>
        public void Insert(int index, IField<T> item)
        {
            Fields.Insert(index, item);
        }

        /// <summary>
        /// Parses the record
        /// </summary>
        /// <param name="Value">Value</param>
        /// <param name="Length">Length of the record</param>
        public abstract void Parse(string Value, int Length = -1);

        /// <summary>
        /// Removes a Field from the file
        /// </summary>
        /// <param name="item">Field to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(IField<T> item)
        {
            return Fields.Remove(item);
        }

        /// <summary>
        /// Removes a Field at a specific index
        /// </summary>
        /// <param name="index">Index of the Field to remove</param>
        public void RemoveAt(int index)
        {
            Fields.RemoveAt(index);
        }

        /// <summary>
        /// Gets the enumerator for the file
        /// </summary>
        /// <returns>The enumerator for this file</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Fields.GetEnumerator();
        }

        /// <summary>
        /// Converts the record to a string
        /// </summary>
        /// <returns>The record as a string</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            foreach (IField<T> Field in Fields)
                Builder.Append(Field.ToString());
            return Builder.ToString();
        }
    }
}