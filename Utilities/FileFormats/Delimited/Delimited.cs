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
using System.Data;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.FileFormats.BaseClasses;
#endregion

namespace Utilities.FileFormats.Delimited
{
    /// <summary>
    /// Base classs for delimited files (CSV, etc.)
    /// </summary>
    /// <typeparam name="T">Delimited</typeparam>
    public abstract class Delimited<T> : StringListFormatBase<T, Row>
        where T : Delimited<T>, new()
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected Delimited()
            : base()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FileContent">File content</param>
        protected Delimited(string FileContent)
            : base()
        {
            Parse(FileContent);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The delimiter used to seperate values (must be overridden)
        /// </summary>
        protected abstract string Delimiter { get; }

        #endregion

        #region Functions

        /// <summary>
        /// Loads the object from the data specified
        /// </summary>
        /// <param name="Data">Data to load into the object</param>
        protected override void LoadFromData(string Data)
        {
            Parse(Data);
        }

        /// <summary>
        /// Parses file content and adds it to the delimited file
        /// </summary>
        /// <param name="FileContent">File content</param>
        public void Parse(string FileContent)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(FileContent), "FileContent");
            Regex TempSplitter = new Regex("[^\"\r\n]*(\r\n|\n|$)|(([^\"\r\n]*)(\"[^\"]*\")([^\"\r\n]*))*(\r\n|\n|$)");
            MatchCollection Matches = TempSplitter.Matches(FileContent);
            Matches.Where(x => !string.IsNullOrEmpty(x.Value))
                    .ForEach(x => Records.Add(new Row(x.Value, Delimiter)));
        }

        /// <summary>
        /// Converts the delimited file to a DataTable
        /// </summary>
        /// <param name="FirstRowIsHeader">Determines if the first row should be treated as a header or not</param>
        /// <param name="Headers">Headers for the columns if the first row is not a header</param>
        /// <returns>The delimited file as a DataTable</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public DataTable ToDataTable(bool FirstRowIsHeader = true, params string[] Headers)
        {
            DataTable ReturnValue = new DataTable();
            ReturnValue.Locale = CultureInfo.CurrentCulture;
            if (FirstRowIsHeader)
            {
                if (Records.Count > 0)
                    foreach (Cell Cell in Records[0])
                        ReturnValue.Columns.Add(Cell.Value);
            }
            else
            {
                foreach (string HeaderValue in Headers)
                    ReturnValue.Columns.Add(HeaderValue);
            }
            for (int y = FirstRowIsHeader ? 1 : 0; y < Records.Count; ++y)
            {
                object[] TempRow = new object[ReturnValue.Columns.Count];
                for (int x = 0; x < Records[y].Count; ++x)
                {
                    TempRow[x] = Records[y][x].Value;
                }
                ReturnValue.Rows.Add(TempRow);
            }
            return ReturnValue;
        }

        /// <summary>
        /// To string function
        /// </summary>
        /// <returns>A string containing the file information</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Records.ForEach<Row>(x => Builder.Append(x.ToString()));
            return Builder.ToString();
        }


        /// <summary>
        /// Converts the string to the format specified
        /// </summary>
        /// <param name="Value">Value to convert</param>
        /// <returns>The string as an object</returns>
        public static implicit operator Delimited<T>(DataTable Value)
        {
            T ReturnValue = new T();
            if (Value == null)
                return ReturnValue;
            Delimited.Row TempRow = new Delimited.Row(ReturnValue.Delimiter);
            foreach (DataColumn Column in Value.Columns)
            {
                TempRow.Add(new Delimited.Cell(Column.ColumnName));
            }
            ReturnValue.Add(TempRow);
            foreach (DataRow Row in Value.Rows)
            {
                TempRow = new Delimited.Row(ReturnValue.Delimiter);
                for (int x = 0; x < Row.ItemArray.Length; ++x)
                {
                    TempRow.Add(new Delimited.Cell(Row.ItemArray[x].ToString()));
                }
                ReturnValue.Records.Add(TempRow);
            }
            return ReturnValue;
        }

        #endregion
    }
}