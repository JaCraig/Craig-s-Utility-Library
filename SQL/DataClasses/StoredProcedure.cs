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
    /// Stored Procedure
    /// </summary>
    public class StoredProcedure : ITable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="Definition">Definition</param>
        /// <param name="ParentDatabase">Parent database</param>
        public StoredProcedure(string Name, string Definition, Database ParentDatabase)
        {
            this.Name = Name;
            this.Definition = Definition;
            this.ParentDatabase = ParentDatabase;
            Columns = new List<Column>();
        }

        #endregion

        #region Public Properties

        public string Name { get; set; }
        public Database ParentDatabase { get; set; }
        public List<Column> Columns { get; set; }

        /// <summary>
        /// Definition
        /// </summary>
        public string Definition { get; set; }

        #endregion

        #region Public Functions

        /// <summary>
        /// Adds a column to the stored procedure (input and output properties accepted)
        /// </summary>
        /// <param name="Name">Name of the column</param>
        /// <param name="Type">Data type</param>
        /// <param name="Length">Max data length</param>
        /// <param name="Default">Default value</param>
        public void AddColumn(string Name, string Type, int Length, string Default)
        {
            Columns.Add(new Column(Name, Type, Length, false, false, false, false, false, "", "", Default, this));
        }

        #endregion
    }
}
