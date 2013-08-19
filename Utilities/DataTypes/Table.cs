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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
#endregion

namespace Utilities.DataTypes
{
    /// <summary>
    /// Holds tabular information
    /// </summary>
    public class Table
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ColumnNames">Column names</param>
        public Table(params string[] ColumnNames)
        {
            this.ColumnNames = (string[])ColumnNames.Clone();
            this.Rows = new List<Row>();
            this.ColumnNameHash = new Hashtable();
            int x = 0;
            foreach (string ColumnName in ColumnNames)
            {
                if (!this.ColumnNameHash.ContainsKey(ColumnName))
                    this.ColumnNameHash.Add(ColumnName, x++);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Reader">Data reader to get the data from</param>
        public Table(IDataReader Reader)
        {
            this.ColumnNames = new string[Reader.FieldCount];
            for (int x = 0; x < Reader.FieldCount; ++x)
            {
                this.ColumnNames[x] = Reader.GetName(x);
            }
            this.ColumnNameHash = new Hashtable();
            int y = 0;
            foreach (string ColumnName in ColumnNames)
            {
                if (!this.ColumnNameHash.ContainsKey(ColumnName))
                    this.ColumnNameHash.Add(ColumnName, y++);
            }
            this.Rows = new List<Row>();
            while (Reader.Read())
            {
                object[] Values = new object[ColumnNames.Length];
                for (int x = 0; x < Reader.FieldCount; ++x)
                {
                    Values[x] = Reader[x];
                }
                this.Rows.Add(new Row(this.ColumnNameHash, this.ColumnNames, Values));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Column names for the table
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] ColumnNames { get; protected set; }

        /// <summary>
        /// Column Name hash table
        /// </summary>
        public Hashtable ColumnNameHash { get; private set; }

        /// <summary>
        /// Rows within the table
        /// </summary>
        public ICollection<Row> Rows { get; private set; }

        /// <summary>
        /// Gets a specific row
        /// </summary>
        /// <param name="RowNumber">Row number</param>
        /// <returns>The row specified</returns>
        public Row this[int RowNumber]
        {
            get
            {
                return Rows.Count > RowNumber ? Rows.ElementAt(RowNumber) : null;
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Adds a row using the objects passed in
        /// </summary>
        /// <param name="Objects">Objects to create the row from</param>
        /// <returns>This</returns>
        public virtual Table AddRow(params object[] Objects)
        {
            this.Rows.Add(new Row(ColumnNameHash, ColumnNames, Objects));
            return this;
        }

        #endregion
    }

    /// <summary>
    /// Holds an individual row
    /// </summary>
    public class Row
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ColumnNames">Column names</param>
        /// <param name="ColumnValues">Column values</param>
        /// <param name="ColumnNameHash">Column name hash</param>
        public Row(Hashtable ColumnNameHash, string[] ColumnNames, params object[] ColumnValues)
        {
            this.ColumnNameHash = ColumnNameHash;
            this.ColumnNames = ColumnNames;
            this.ColumnValues = (object[])ColumnValues.Clone();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Column names
        /// </summary>
        public Hashtable ColumnNameHash { get; private set; }

        /// <summary>
        /// Column names
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] ColumnNames { get; protected set; }

        /// <summary>
        /// Column values
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public object[] ColumnValues { get; protected set; }

        /// <summary>
        /// Returns a column based on the column name specified
        /// </summary>
        /// <param name="ColumnName">Column name to search for</param>
        /// <returns>The value specified</returns>
        public object this[string ColumnName]
        {
            get
            {
                int Column = (int)ColumnNameHash[ColumnName];//.PositionOf(ColumnName);
                if (Column == -1)
                    throw new ArgumentOutOfRangeException(ColumnName + " is not present in the row");
                return this[Column];
            }
        }

        /// <summary>
        /// Returns a column based on the value specified
        /// </summary>
        /// <param name="Column">Column number</param>
        /// <returns>The value specified</returns>
        public object this[int Column]
        {
            get
            {
                Contract.Requires<ArgumentOutOfRangeException>(Column >= 0, "Column");
                if (ColumnValues.Length <= Column)
                    return null;
                return ColumnValues[Column];
            }
        }

        #endregion
    }
}