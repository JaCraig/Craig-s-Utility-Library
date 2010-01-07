/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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
#endregion

namespace Utilities.FileFormats.CSV
{
    /// <summary>
    /// CSV loader
    /// </summary>
    public class CSV
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CSV()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FileContent">File content</param>
        public CSV(string FileContent)
        {
            Regex TempSplitter = new Regex("[^\"\r\n$]*(\r\n|\n|$)|(([^\"\r\n$]*)(\"[^\"]*\")([^\"\r\n$]*))*(\r\n|\n|$)");
            MatchCollection Matches = TempSplitter.Matches(FileContent);
            foreach (Match Match in Matches)
            {
                if (!string.IsNullOrEmpty(Match.Value))
                {
                    Rows.Add(new Row(Match.Value));
                }
            }
        }

        #endregion

        #region Public Properties

        private List<Row> _Rows = new List<Row>();

        /// <summary>
        /// The list of rows
        /// </summary>
        public List<Row> Rows
        {
            get { return _Rows; }
            set { _Rows = value; }
        }

        /// <summary>
        /// The number of rows within the file
        /// </summary>
        public int NumberOfRows
        {
            get { return Rows.Count; }
        }

        /// <summary>
        /// Individual rows
        /// </summary>
        /// <param name="Position">The row that you want to get</param>
        /// <returns>The row requested</returns>
        public Row this[int Position]
        {
            get { return _Rows[Position]; }
            set { _Rows[Position] = value; }
        }

        #endregion

        #region Public Overridden Function

        /// <summary>
        /// To string function
        /// </summary>
        /// <returns>A string containing the file information</returns>
        public override string ToString()
        {
            StringBuilder Builder=new StringBuilder();
            foreach (Row CurrentRow in Rows)
            {
                Builder.Append(CurrentRow.ToString());
            }
            return Builder.ToString();
        }

        #endregion
    }
}
