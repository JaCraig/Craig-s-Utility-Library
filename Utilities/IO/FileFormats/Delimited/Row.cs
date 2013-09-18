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
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
#endregion

namespace Utilities.IO.FileFormats.Delimited
{
    /// <summary>
    /// Individual row within a delimited file
    /// </summary>
    public class Row : IList<Cell>
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Row()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Delimiter">Delimiter to parse the individual cells</param>
        public Row(string Delimiter)
        {
            this.Cells = new List<Cell>();
            this.Delimiter = Delimiter;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Content">Content of the row</param>
        /// <param name="Delimiter">Delimiter to parse the individual cells</param>
        public Row(string Content, string Delimiter)
        {
            this.Cells = new List<Cell>();
            this.Delimiter = Delimiter;
            Regex TempSplitter = new Regex(string.Format(CultureInfo.InvariantCulture, "(?<Value>\"(?:[^\"]|\"\")*\"|[^{0}\r\n]*?)(?<Delimiter>{0}|\r\n|\n|$)", Regex.Escape(Delimiter)));
            MatchCollection Matches = TempSplitter.Matches(Content);
            bool Finished = false;
            foreach (Match Match in Matches)
            {
                if (!Finished)
                {
                    Cells.Add(new Cell(Match.Groups["Value"].Value));
                }
                Finished = string.IsNullOrEmpty(Match.Groups["Delimiter"].Value) || Match.Groups["Delimiter"].Value == "\r\n" || Match.Groups["Delimiter"].Value == "\n";
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Cells within the row
        /// </summary>
        protected IList<Cell> Cells { get; private set; }

        /// <summary>
        /// Returns a cell within the row
        /// </summary>
        /// <param name="Position">The position of the cell</param>
        /// <returns>The specified cell</returns>
        public Cell this[int Position]
        {
            get { return Cells[Position]; }
            set { Cells[Position] = value; }
        }

        /// <summary>
        /// Delimiter used
        /// </summary>
        public string Delimiter { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// To string function
        /// </summary>
        /// <returns>The content of the row in string form</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            string Seperator = "";
            foreach (Cell CurrentCell in Cells)
            {
                Builder.Append(Seperator).Append(CurrentCell);
                Seperator = Delimiter;
            }
            return Builder.Append(System.Environment.NewLine).ToString();
        }

        /// <summary>
        /// Gets the enumerator for the delimited file
        /// </summary>
        /// <returns>The enumerator for this file</returns>
        public IEnumerator<Cell> GetEnumerator()
        {
            return Cells.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator for the delimited file
        /// </summary>
        /// <returns>The enumerator for this file</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Cells.GetEnumerator();
        }

        /// <summary>
        /// Adds a Cell to the file
        /// </summary>
        /// <param name="item">Cell to add</param>
        public void Add(Cell item)
        {
            Cells.Add(item);
        }

        /// <summary>
        /// Clears the file
        /// </summary>
        public void Clear()
        {
            Cells.Clear();
        }

        /// <summary>
        /// Determines if the file contains a Cell
        /// </summary>
        /// <param name="item">Cell to check for</param>
        /// <returns>True if it does, false otherwise</returns>
        public bool Contains(Cell item)
        {
            return Cells.Contains(item);
        }

        /// <summary>
        /// Copies the delimited file to an array
        /// </summary>
        /// <param name="array">Array to copy to</param>
        /// <param name="arrayIndex">Index to start at</param>
        public void CopyTo(Cell[] array, int arrayIndex)
        {
            Cells.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Number of Cells
        /// </summary>
        public int Count
        {
            get { return Cells.Count; }
        }

        /// <summary>
        /// Is the file read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return Cells.IsReadOnly; }
        }

        /// <summary>
        /// Removes a Cell from the file
        /// </summary>
        /// <param name="item">Cell to remove</param>
        /// <returns>True if it is removed, false otherwise</returns>
        public bool Remove(Cell item)
        {
            return Cells.Remove(item);
        }

        /// <summary>
        /// Index of a specific Cell
        /// </summary>
        /// <param name="item">Cell to search for</param>
        /// <returns>The index of a specific Cell</returns>
        public int IndexOf(Cell item)
        {
            return Cells.IndexOf(item);
        }

        /// <summary>
        /// Inserts a Cell at a specific index
        /// </summary>
        /// <param name="index">Index to insert at</param>
        /// <param name="item">Cell to insert</param>
        public void Insert(int index, Cell item)
        {
            Cells.Insert(index, item);
        }

        /// <summary>
        /// Removes a Cell at a specific index
        /// </summary>
        /// <param name="index">Index of the Cell to remove</param>
        public void RemoveAt(int index)
        {
            Cells.RemoveAt(index);
        }

        #endregion
    }
}