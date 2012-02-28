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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.DataTypes.ExtensionMethods;
using System.Data;
#endregion

namespace Utilities.FileFormats.ExtensionMethods
{
    /// <summary>
    /// Extension methods pertaining to file formats
    /// </summary>
    public static class ExtensionMethods
    {
        #region Functions

        #region ToCSV
        
        /// <summary>
        /// Converts an IEnumerable to a CSV file
        /// </summary>
        /// <typeparam name="T">Type of the items within the list</typeparam>
        /// <param name="List">The list to convert</param>
        /// <returns>The CSV file containing the list</returns>
        public static CSV.CSV ToCSV<T>(this IEnumerable<T> List)
        {
            return List.ToDataTable().ToCSV();
        }

        /// <summary>
        /// Converts an IEnumerable to a CSV file
        /// </summary>
        /// <param name="Data">The DataTable to convert</param>
        /// <returns>The CSV file containing the list</returns>
        public static CSV.CSV ToCSV(this DataTable Data)
        {
            CSV.CSV ReturnValue = new CSV.CSV();
            if (Data.IsNull())
                return ReturnValue;
            Delimited.Row TempRow = new Delimited.Row(",");
            foreach (DataColumn Column in Data.Columns)
            {
                TempRow.Cells.Add(new Delimited.Cell(Column.ColumnName));
            }
            ReturnValue.Rows.Add(TempRow);
            foreach (DataRow Row in Data.Rows)
            {
                TempRow = new Delimited.Row(",");
                for (int x = 0; x < Data.Columns.Count; ++x)
                {
                    TempRow.Cells.Add(new Delimited.Cell(Row.ItemArray[x].ToString()));
                }
                ReturnValue.Rows.Add(TempRow);
            }
            return ReturnValue;
        }

        #endregion

        #region ToDelimitedFile

        /// <summary>
        /// Converts an IEnumerable to a delimited file
        /// </summary>
        /// <typeparam name="T">Type of the items within the list</typeparam>
        /// <param name="List">The list to convert</param>
        /// <returns>The delimited file containing the list</returns>
        public static Delimited.Delimited ToDelimitedFile<T>(this IEnumerable<T> List, string Delimiter = "\t")
        {
            return List.ToDataTable().ToDelimitedFile(Delimiter);
        }

        /// <summary>
        /// Converts an IEnumerable to a delimited file
        /// </summary>
        /// <param name="Data">The DataTable to convert</param>
        /// <returns>The delimited file containing the list</returns>
        public static Delimited.Delimited ToDelimitedFile(this DataTable Data, string Delimiter = "\t")
        {
            GenericDelimited.GenericDelimited ReturnValue = new GenericDelimited.GenericDelimited(Delimiter);
            if (Data.IsNull())
                return ReturnValue;
            Delimited.Row TempRow = new Delimited.Row(Delimiter);
            foreach (DataColumn Column in Data.Columns)
            {
                TempRow.Cells.Add(new Delimited.Cell(Column.ColumnName));
            }
            ReturnValue.Rows.Add(TempRow);
            foreach (DataRow Row in Data.Rows)
            {
                TempRow = new Delimited.Row(Delimiter);
                for (int x = 0; x < Data.Columns.Count; ++x)
                {
                    TempRow.Cells.Add(new Delimited.Cell(Row.ItemArray[x].ToString()));
                }
                ReturnValue.Rows.Add(TempRow);
            }
            return ReturnValue;
        }

        #endregion

        #endregion
    }
}
