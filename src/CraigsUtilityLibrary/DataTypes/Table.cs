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
using System.Data.Common;
using System.Linq;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Holds an individual row
    /// </summary>
    public class Row
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnNames">Column names</param>
        /// <param name="columnValues">Column values</param>
        /// <param name="columnNameHash">Column name hash</param>
        public Row(Hashtable columnNameHash, string[] columnNames, params object[] columnValues)
        {
            columnValues = columnValues ?? new object[0];
            columnNames = columnNames ?? new string[0];
            columnNameHash = columnNameHash ?? new Hashtable();
            ColumnNameHash = columnNameHash;
            ColumnNames = columnNames;
            ColumnValues = (object[])columnValues.Clone();
        }

        /// <summary>
        /// Column names
        /// </summary>
        public Hashtable ColumnNameHash { get; private set; }

        /// <summary>
        /// Column names
        /// </summary>
        public string[] ColumnNames { get; protected set; }

        /// <summary>
        /// Column values
        /// </summary>
        public object[] ColumnValues { get; protected set; }

        /// <summary>
        /// Returns a column based on the column name specified
        /// </summary>
        /// <param name="columnName">Column name to search for</param>
        /// <returns>The value specified</returns>
        public object this[string columnName]
        {
            get
            {
                columnName = columnName ?? "";
                var Column = (int)ColumnNameHash[columnName];
                if (Column <= -1)
                    return null;
                return this[Column];
            }
        }

        /// <summary>
        /// Returns a column based on the value specified
        /// </summary>
        /// <param name="column">Column number</param>
        /// <returns>The value specified</returns>
        public object this[int column]
        {
            get
            {
                if (column < 0)
                    column = 0;
                if (ColumnValues.Length <= column)
                    return null;
                return ColumnValues[column];
            }
        }
    }

    /// <summary>
    /// Holds tabular information
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnNames">Column names</param>
        public Table(params string[] columnNames)
        {
            columnNames = columnNames ?? new string[0];
            ColumnNames = (string[])columnNames.Clone();
            Rows = new List<Row>();
            ColumnNameHash = new Hashtable();
            int x = 0;
            foreach (string ColumnName in columnNames)
            {
                if (!ColumnNameHash.ContainsKey(ColumnName))
                    ColumnNameHash.Add(ColumnName, x++);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reader">Data reader to get the data from</param>
        public Table(DbDataReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (reader.FieldCount == 0)
                throw new ArgumentOutOfRangeException(nameof(reader), "reader.FieldCount needs to have at least 0 fields");
            ColumnNames = new string[reader.FieldCount];
            for (int x = 0; x < reader.FieldCount; ++x)
            {
                ColumnNames[x] = reader.GetName(x);
            }
            ColumnNameHash = new Hashtable();
            int y = 0;
            foreach (string ColumnName in ColumnNames)
            {
                if (!ColumnNameHash.ContainsKey(ColumnName))
                    ColumnNameHash.Add(ColumnName, y++);
            }
            Rows = new List<Row>();
            while (reader.Read())
            {
                object[] Values = new object[ColumnNames.Length];
                for (int x = 0; x < reader.FieldCount; ++x)
                {
                    Values[x] = reader[x];
                }
                Rows.Add(new Row(ColumnNameHash, ColumnNames, Values));
            }
        }

        /// <summary>
        /// Column Name hash table
        /// </summary>
        public Hashtable ColumnNameHash { get; private set; }

        /// <summary>
        /// Column names for the table
        /// </summary>
        public string[] ColumnNames { get; protected set; }

        /// <summary>
        /// Rows within the table
        /// </summary>
        public ICollection<Row> Rows { get; private set; }

        /// <summary>
        /// Gets a specific row
        /// </summary>
        /// <param name="rowNumber">Row number</param>
        /// <returns>The row specified</returns>
        public Row this[int rowNumber]
        {
            get
            {
                if (Rows == null)
                    return null;
                return Rows.Count > rowNumber ? Rows.ElementAt(rowNumber) : null;
            }
        }

        /// <summary>
        /// Adds a row using the objects passed in
        /// </summary>
        /// <param name="objects">Objects to create the row from</param>
        /// <returns>This</returns>
        public Table AddRow(params object[] objects)
        {
            if (objects == null)
                return this;
            if (Rows == null)
                return this;
            Rows.Add(new Row(ColumnNameHash, ColumnNames, objects));
            return this;
        }
    }
}