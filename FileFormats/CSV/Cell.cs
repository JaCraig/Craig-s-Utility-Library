/*
Copyright (c) 2009 <a href="http://www.gutgames.com">James Craig</a>

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

#endregion

namespace Utilities.FileFormats.CSV
{
    /// <summary>
    /// Cell within a CSV file
    /// </summary>
    public class Cell
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Cell()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Content">Value within the cell</param>
        public Cell(string Content)
        {
            Value = Content;
        }

        #endregion

        #region Public Properties

        private string _Value = string.Empty;

        /// <summary>
        /// Value within the cell
        /// </summary>
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        #endregion

        #region Public Overridden Functions

        /// <summary>
        /// To string function
        /// </summary>
        /// <returns>The value of the cell</returns>
        public override string ToString()
        {
            return Value;
        }

        #endregion
    }
}
