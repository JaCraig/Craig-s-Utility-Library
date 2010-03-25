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
    /// Table class
    /// </summary>
    public class Table:ITable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="ParentDatabase">Parent database</param>
        public Table(string Name,Database ParentDatabase)
        {
            this.Name = Name;
            this.ParentDatabase = ParentDatabase;
            Columns = new List<Column>();
            Triggers = new List<Trigger>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Table()
        {
            Columns = new List<Column>();
            Triggers = new List<Trigger>();
        }

        #endregion

        #region Public Properties

        public string Name { get; set; }
        public List<Column> Columns { get; set; }
        public Database ParentDatabase { get; set; }

        /// <summary>
        /// List of triggers associated with the table
        /// </summary>
        public List<Trigger> Triggers { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Adds a column
        /// </summary>
        /// <param name="ColumnName">Column Name</param>
        /// <param name="ColumnType">Data type</param>
        /// <param name="Length">Data length</param>
        /// <param name="Nullable">Nullable?</param>
        /// <param name="Identity">Identity?</param>
        /// <param name="Index">Index?</param>
        /// <param name="PrimaryKey">Primary key?</param>
        /// <param name="Unique">Unique?</param>
        /// <param name="ForeignKeyTable">Foreign key table</param>
        /// <param name="ForeignKeyColumn">Foreign key column</param>
        /// <param name="DefaultValue">Default value</param>
        public void AddColumn(string ColumnName,string ColumnType,int Length,bool Nullable,
            bool Identity,bool Index,bool PrimaryKey,bool Unique,string ForeignKeyTable,string ForeignKeyColumn,
            string DefaultValue)
        {
            Columns.Add(new Column(ColumnName, ColumnType, Length, Nullable, Identity, Index, PrimaryKey,Unique,ForeignKeyTable,ForeignKeyColumn,DefaultValue, this));
        }

        /// <summary>
        /// Determines if a column exists in the table
        /// </summary>
        /// <param name="ColumnName">Column name</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool ContainsColumn(string ColumnName)
        {
            foreach (Column Column in this.Columns)
            {
                if (Column.Name == ColumnName)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a foreign key
        /// </summary>
        /// <param name="ColumnName">Column name</param>
        /// <param name="ForeignKeyTable">Foreign key table</param>
        /// <param name="ForeignKeyColumn">Foreign key column</param>
        public void AddForeignKey(string ColumnName, string ForeignKeyTable, string ForeignKeyColumn)
        {
            foreach (Column Column in this.Columns)
            {
                if (Column.Name == ColumnName)
                {
                    Column.AddForeignKey(ForeignKeyTable, ForeignKeyColumn);
                    break;
                }
            }
        }

        /// <summary>
        /// Sets up foreign keys
        /// </summary>
        public void SetupForeignKeys()
        {
            foreach (Column Column in this.Columns)
            {
                Column.SetupForeignKeys();
            }
        }

        /// <summary>
        /// Adds a trigger
        /// </summary>
        /// <param name="Name">Trigger name</param>
        /// <param name="Definition">Trigger definition</param>
        /// <param name="Type">The trigger type</param>
        public void AddTrigger(string Name, string Definition, int Type)
        {
            Triggers.Add(new Trigger(Name, Definition, Type, this));
        }

        /// <summary>
        /// Gets the column specified
        /// </summary>
        /// <param name="Name">Column name</param>
        /// <returns>The specified column</returns>
        public Column this[string Name]
        {
            get
            {
                foreach (Column Column in this.Columns)
                {
                    if (Column.Name == Name)
                    {
                        return Column;
                    }
                }
                return null;
            }
        }

        #endregion
    }
}
