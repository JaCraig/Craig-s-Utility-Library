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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.FileFormats.FixedLength.Interfaces;
#endregion

namespace Utilities.FileFormats.FixedLength.BaseClasses
{
    /// <summary>
    /// Parses and creates a fixed length file
    /// </summary>
    public class FixedLengthBase<T>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public FixedLengthBase()
        {
            Records = new List<IRecord<T>>();
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Parses the string into fields
        /// </summary>
        /// <param name="Value">The string value</param>
        public virtual void Parse(string Value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses the string into fields
        /// </summary>
        /// <param name="Value">The string value</param>
        /// <param name="Length">Max length for the record</param>
        public virtual void Parse(string Value, int Length)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Protected Variables

        protected List<IRecord<T>> Records { get; set; }

        #endregion

        #region Public Overridden Functions

        public override string ToString()
        {
            StringBuilder Builder = new StringBuilder();
            foreach (IRecord<T> Record in Records)
            {
                Builder.Append(Record.ToString());
            }
            return Builder.ToString();
        }

        #endregion
    }
}
