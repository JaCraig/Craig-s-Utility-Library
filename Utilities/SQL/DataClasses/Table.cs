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
using System.Data;
using System.Linq;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.SQL.DataClasses.Enums;
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
            Columns = new List<IColumn>();
            Triggers = new List<Trigger>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Table()
        {
            Columns = new List<IColumn>();
            Triggers = new List<Trigger>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Name of the table
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Columns in the table
        /// </summary>
        public virtual List<IColumn> Columns { get; set; }
        /// <summary>
        /// Parent database
        /// </summary>
        public virtual Database ParentDatabase { get; set; }

        /// <summary>
        /// List of triggers associated with the table
        /// </summary>
        public virtual List<Trigger> Triggers { get; set; }

        #endregion

        #region Public Functions

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
        /// <typeparam name="T">Column type</typeparam>
        public virtual IColumn AddColumn<T>(string ColumnName, DbType ColumnType, int Length = 0, bool Nullable = true,
            bool Identity = false, bool Index = false, bool PrimaryKey = false, bool Unique = false,
            string ForeignKeyTable = "", string ForeignKeyColumn = "", T DefaultValue = default(T))
        {
            return Columns.AddAndReturn(new Column<T>(ColumnName, ColumnType, Length, Nullable, Identity, Index, PrimaryKey, Unique, ForeignKeyTable, ForeignKeyColumn, DefaultValue, this));
        }

        /// <summary>
        /// Determines if a column exists in the table
        /// </summary>
        /// <param name="ColumnName">Column name</param>
        /// <returns>True if it exists, false otherwise</returns>
        public virtual bool ContainsColumn(string ColumnName)
        {
            return this[ColumnName] != null;
        }

        /// <summary>
        /// Adds a foreign key
        /// </summary>
        /// <param name="ColumnName">Column name</param>
        /// <param name="ForeignKeyTable">Foreign key table</param>
        /// <param name="ForeignKeyColumn">Foreign key column</param>
        public virtual void AddForeignKey(string ColumnName, string ForeignKeyTable, string ForeignKeyColumn)
        {
            this[ColumnName].Do(x => x.AddForeignKey(ForeignKeyTable, ForeignKeyColumn));
        }

        /// <summary>
        /// Sets up foreign keys
        /// </summary>
        public virtual void SetupForeignKeys()
        {
            this.Columns.ForEach(x => x.SetupForeignKeys());
        }

        /// <summary>
        /// Adds a trigger
        /// </summary>
        /// <param name="Name">Trigger name</param>
        /// <param name="Definition">Trigger definition</param>
        /// <param name="Type">The trigger type</param>
        public virtual Trigger AddTrigger(string Name, string Definition, TriggerType Type)
        {
            return Triggers.AddAndReturn(new Trigger(Name, Definition, Type, this));
        }

        /// <summary>
        /// Gets the column specified
        /// </summary>
        /// <param name="Name">Column name</param>
        /// <returns>The specified column</returns>
        public virtual IColumn this[string Name]
        {
            get
            {
                return this.Columns.FirstOrDefault(x => x.Name == Name);
            }
        }

        #endregion
    }
}
