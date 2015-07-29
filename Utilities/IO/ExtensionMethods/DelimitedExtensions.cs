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
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using Utilities.DataTypes;

namespace Utilities.IO
{
    /// <summary>
    /// Extension methods pertaining to file formats
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class FileFormatExtensions
    {
        /// <summary>
        /// Converts an IEnumerable to a delimited file
        /// </summary>
        /// <typeparam name="T">Type of the items within the list</typeparam>
        /// <param name="List">The list to convert</param>
        /// <param name="Delimiter">Delimiter to use</param>
        /// <returns>The delimited file containing the list</returns>
        public static Utilities.IO.FileFormats.Delimited.Delimited ToDelimitedFile<T>(this IEnumerable<T> List, string Delimiter = "\t")
        {
            return List.To().ToDelimitedFile(Delimiter);
        }

        /// <summary>
        /// Converts an IEnumerable to a delimited file
        /// </summary>
        /// <param name="List">The list to convert</param>
        /// <param name="Delimiter">Delimiter to use</param>
        /// <returns>The delimited file containing the list</returns>
        public static Utilities.IO.FileFormats.Delimited.Delimited ToDelimitedFile(this IEnumerable List, string Delimiter = "\t")
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            return List.To().ToDelimitedFile(Delimiter);
        }

        /// <summary>
        /// Converts an IEnumerable to a delimited file
        /// </summary>
        /// <param name="Data">The DataTable to convert</param>
        /// <param name="Delimiter">Delimiter to use</param>
        /// <returns>The delimited file containing the list</returns>
        public static Utilities.IO.FileFormats.Delimited.Delimited ToDelimitedFile(this DataTable Data, string Delimiter = ",")
        {
            var ReturnValue = new Utilities.IO.FileFormats.Delimited.Delimited("", Delimiter);
            if (Data == null)
                return ReturnValue;
            var TempRow = new FileFormats.Delimited.Row(Delimiter);
            foreach (DataColumn Column in Data.Columns)
            {
                TempRow.Add(new Utilities.IO.FileFormats.Delimited.Cell(Column.ColumnName));
            }
            ReturnValue.Add(TempRow);
            foreach (DataRow Row in Data.Rows)
            {
                TempRow = new Utilities.IO.FileFormats.Delimited.Row(Delimiter);
                for (int x = 0; x < Data.Columns.Count; ++x)
                {
                    TempRow.Add(new Utilities.IO.FileFormats.Delimited.Cell(Row.ItemArray[x].ToString()));
                }
                ReturnValue.Add(TempRow);
            }
            return ReturnValue;
        }
    }
}