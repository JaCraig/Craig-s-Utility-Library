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
using Utilities.SQL.DataClasses.Interfaces;
#endregion

namespace Utilities.SQL.DataClasses
{
    /// <summary>
    /// View class
    /// </summary>
    public class View:ITable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="ParentDatabase">Parent dataabse</param>
        public View(string Name,Database ParentDatabase)
        {
            this.Name = Name;
            this.ParentDatabase = ParentDatabase;
            Columns = new List<Column>();
        }

        #endregion

        #region Public Properties

        public string Name { get; set; }
        public Database ParentDatabase { get; set; }
        public List<Column> Columns { get; set; }

        /// <summary>
        /// Definition of the view
        /// </summary>
        public string Definition { get; set; }

        #endregion

        #region Public Function

        /// <summary>
        /// Adds a column to the view
        /// </summary>
        /// <param name="ColumnName">Column name</param>
        /// <param name="ColumnType">Data type</param>
        /// <param name="MaxLength">max length</param>
        /// <param name="Nullable">Nullable?</param>
        public void AddColumn(string ColumnName, string ColumnType, int MaxLength, bool Nullable)
        {
            Columns.Add(new Column(ColumnName, ColumnType, MaxLength, Nullable, false, false, false, false, "", "", "", this));
        }

        #endregion
    }
}
