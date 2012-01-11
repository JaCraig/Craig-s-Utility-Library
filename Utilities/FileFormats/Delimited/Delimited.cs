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
using System.Text;
using System.Text.RegularExpressions;
using System;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.FileFormats.Delimited
{
    /// <summary>
    /// Base classs for delimited files (CSV, etc.)
    /// </summary>
    public abstract class Delimited
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Delimited()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FileContent">File content</param>
        public Delimited(string FileContent)
        {
            FileContent.ThrowIfNullOrEmpty("FileContent");
            Regex TempSplitter = new Regex("[^\"\r\n]*(\r\n|\n|$)|(([^\"\r\n]*)(\"[^\"]*\")([^\"\r\n]*))*(\r\n|\n|$)");
            MatchCollection Matches = TempSplitter.Matches(FileContent);
            Matches.Where(x => !string.IsNullOrEmpty(x.Value))
                    .ForEach(x => Rows.Add(new Row(x.Value, Delimiter)));
        }

        #endregion

        #region Public Properties

        private List<Row> _Rows = new List<Row>();

        /// <summary>
        /// The list of rows
        /// </summary>
        public virtual List<Row> Rows
        {
            get { return _Rows; }
            set { _Rows = value; }
        }

        /// <summary>
        /// The number of rows within the file
        /// </summary>
        public virtual int NumberOfRows
        {
            get { return Rows.Count; }
        }

        /// <summary>
        /// Individual rows
        /// </summary>
        /// <param name="Position">The row that you want to get</param>
        /// <returns>The row requested</returns>
        public virtual Row this[int Position]
        {
            get { return _Rows[Position]; }
            set { _Rows[Position] = value; }
        }

        /// <summary>
        /// The delimiter used to seperate values (must be overridden)
        /// </summary>
        protected abstract string Delimiter { get; }

        #endregion

        #region Public Overridden Function

        /// <summary>
        /// To string function
        /// </summary>
        /// <returns>A string containing the file information</returns>
        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            Rows.ForEach<Row>(x => Builder.Append(x.ToString()));
            return Builder.ToString();
        }

        #endregion
    }
}