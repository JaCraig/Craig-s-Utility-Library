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
using System.Collections.Generic;
#endregion

namespace Utilities.FileFormats.BaseClasses
{
    /// <summary>
    /// Format base class for objects that are string based and list of records
    /// </summary>
    public abstract class StringListFormatBase<FormatType, RecordType> : StringFormatBase<FormatType>, IList<RecordType>
        where FormatType : StringListFormatBase<FormatType,RecordType>, new()
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected StringListFormatBase()
            : base()
        {
            Records = new List<RecordType>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The list of records
        /// </summary>
        protected IList<RecordType> Records { get; private set; }

        /// <summary>
        /// Individual records
        /// </summary>
        /// <param name="Position">The record that you want to get</param>
        /// <returns>The record requested</returns>
        public RecordType this[int Position]
        {
            get { return Records[Position]; }
            set { Records[Position] = value; }
        }
        
        /// <summary>
        /// Count of records
        /// </summary>
        public int Count
        {
            get { return Records.Count; }
        }

        /// <summary>
        /// Is read only?
        /// </summary>
        public bool IsReadOnly
        {
            get { return Records.IsReadOnly; }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Gets the enumerator for the delimited file
        /// </summary>
        /// <returns>The enumerator for this file</returns>
        public IEnumerator<RecordType> GetEnumerator()
        {
            return Records.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for the delimited file
        /// </summary>
        /// <returns>The enumerator for this file</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Records.GetEnumerator();
        }

        /// <summary>
        /// Adds a Record to the file
        /// </summary>
        /// <param name="item">Record to add</param>
        public void Add(RecordType item)
        {
            Records.Add(item);
        }

        /// <summary>
        /// Clears the file
        /// </summary>
        public void Clear()
        {
            Records.Clear();
        }

        /// <summary>
        /// Determines if the file contains a Record
        /// </summary>
        /// <param name="item">Record to check for</param>
        /// <returns>True if it does, false otherwise</returns>
        public bool Contains(RecordType item)
        {
            return Records.Contains(item);
        }

        /// <summary>
        /// Copies the delimited file to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public void CopyTo(RecordType[] array, int arrayIndex)
        {
            Records.CopyTo(array, arrayIndex);
        }
        
        /// <summary>
        /// Removes a Record from the file
        /// </summary>
        /// <param name="item">Record to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(RecordType item)
        {
            return Records.Remove(item);
        }

        /// <summary>
        /// Index of a specific Record
        /// </summary>
        /// <param name="item">Record to search for</param>
        /// <returns>The index of a specific Record</returns>
        public int IndexOf(RecordType item)
        {
            return Records.IndexOf(item);
        }

        /// <summary>
        /// Inserts a Record at a specific index
        /// </summary>
        /// <param name="index">Index to insert at</param>
        /// <param name="item">Record to insert</param>
        public void Insert(int index, RecordType item)
        {
            Records.Insert(index, item);
        }

        /// <summary>
        /// Removes a Record at a specific index
        /// </summary>
        /// <param name="index">Index of the Record to remove</param>
        public void RemoveAt(int index)
        {
            Records.RemoveAt(index);
        }

        #endregion
    }
}
