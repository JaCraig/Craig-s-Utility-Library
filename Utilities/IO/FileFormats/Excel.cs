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
using System.Data;
using System.Data.OleDb;
using System.Diagnostics.Contracts;
using System.Globalization;
using Utilities.DataTypes;
using Utilities.IO.FileFormats.BaseClasses;

namespace Utilities.IO.FileFormats
{
    /// <summary>
    /// Excel doc helper
    /// </summary>
    public class Excel : StringListFormatBase<Excel, List<string>>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Excel()
        {
            ColumnNames = new List<string>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FilePath">FilePath</param>
        /// <param name="Sheet">Sheet to load</param>
        public Excel(string FilePath, string Sheet)
            : this()
        {
            Parse(FilePath, Sheet);
        }

        /// <summary>
        /// Names of each column
        /// </summary>
        public IList<string> ColumnNames { get; private set; }

        /// <summary>
        /// Gets the value based on the row and column name specified
        /// </summary>
        /// <param name="Value">Row to get</param>
        /// <param name="Name">Column name to look for</param>
        /// <returns>The value</returns>
        public string this[int Value, string Name]
        {
            get
            {
                Contract.Requires<ArgumentException>(Value >= 0, "Value must be greater than or equal to 0");
                Contract.Requires<NullReferenceException>(ColumnNames != null, "ColumnNames");
                return Records[Value][ColumnNames.IndexOf(Name)];
            }
        }

        /// <summary>
        /// Loads an excel doc/sheet
        /// </summary>
        /// <param name="Location">Location of the file to load</param>
        /// <param name="Sheet">Sheet of the document to load</param>
        /// <returns>The excel doc</returns>
        public static Excel Load(string Location, string Sheet)
        {
            return new Excel(Location, Sheet);
        }

        /// <summary>
        /// To string function
        /// </summary>
        /// <returns>A string containing the file information</returns>
        public override string ToString()
        {
            return ColumnNames.ToString(x => x, "\t")
                + System.Environment.NewLine
                + Records.ToString(x => x.ToString(y => y, "\t"), System.Environment.NewLine);
        }

        /// <summary>
        /// Loads data from the excel doc
        /// </summary>
        /// <param name="Location">Location of the file</param>
        /// <returns>The excel doc</returns>
        protected override Excel InternalLoad(string Location)
        {
            return Load(Location, "Sheet1");
        }

        /// <summary>
        /// Loads data from the excel doc
        /// </summary>
        /// <param name="Data">Data to load from</param>
        protected override void LoadFromData(string Data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses the file
        /// </summary>
        /// <param name="FilePath">File path</param>
        /// <param name="Sheet">Sheet to parse</param>
        protected void Parse(string FilePath, string Sheet)
        {
            string ConnectionString = string.Format(CultureInfo.CurrentCulture, "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;\"", FilePath);
            string Command = string.Format(CultureInfo.CurrentCulture, "select * from [{0}$]", Sheet);
            using (var Adapter = new OleDbDataAdapter(Command, ConnectionString))
            {
                using (var ds = new DataSet())
                {
                    ds.Locale = CultureInfo.CurrentCulture;
                    Adapter.Fill(ds, "something");
                    int y = 0;
                    foreach (DataColumn Column in ds.Tables["something"].Columns)
                        ColumnNames.Add(Column.ColumnName);
                    foreach (DataRow Row in ds.Tables["something"].Rows)
                    {
                        Records.Add(new List<string>());
                        for (int x = 0; x < Row.ItemArray.Length; ++x)
                        {
                            Records[y].Add(Row.ItemArray[x].ToString());
                        }
                        ++y;
                    }
                }
            }
        }
    }
}