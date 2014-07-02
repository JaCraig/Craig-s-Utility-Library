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

using System.Collections.Generic;
using System.Data;
using System.Linq;
using Utilities.DataTypes;
using Utilities.ORM.Manager.Schema.Interfaces;

namespace Utilities.ORM.Manager.Schema.BaseClasses
{
    /// <summary>
    /// Table base class
    /// </summary>
    public abstract class TableBase : ITable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Name of the table</param>
        /// <param name="Source">Source that the table is from</param>
        protected TableBase(string Name, ISource Source)
        {
            this.Name = Name;
            this.Source = Source;
            this.Columns = new List<IColumn>();
            this.Triggers = new List<ITrigger>();
        }

        /// <summary>
        /// Columns
        /// </summary>
        public ICollection<IColumn> Columns { get; private set; }

        /// <summary>
        /// Name of the table
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Source/Parent
        /// </summary>
        public ISource Source { get; private set; }

        /// <summary>
        /// List of triggers associated with the table
        /// </summary>
        public ICollection<ITrigger> Triggers { get; private set; }

        /// <summary>
        /// The column specified
        /// </summary>
        /// <param name="Name">Name of the column</param>
        /// <returns>The column specified</returns>
        public IColumn this[string Name] { get { return Columns.FirstOrDefault(x => string.Equals(x.Name, Name, System.StringComparison.CurrentCultureIgnoreCase)); } }

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
        /// <param name="OnDeleteCascade">On Delete Cascade</param>
        /// <param name="OnUpdateCascade">On Update Cascade</param>
        /// <param name="OnDeleteSetNull">On Delete Set Null</param>
        /// <typeparam name="T">Column type</typeparam>
        public abstract IColumn AddColumn<T>(string ColumnName, DbType ColumnType, int Length = 0, bool Nullable = true,
            bool Identity = false, bool Index = false, bool PrimaryKey = false, bool Unique = false,
            string ForeignKeyTable = "", string ForeignKeyColumn = "", T DefaultValue = default(T),
            bool OnDeleteCascade = false, bool OnUpdateCascade = false, bool OnDeleteSetNull = false);

        /// <summary>
        /// Adds a foreign key
        /// </summary>
        /// <param name="ColumnName">Column name</param>
        /// <param name="ForeignKeyTable">Foreign key table</param>
        /// <param name="ForeignKeyColumn">Foreign key column</param>
        public abstract void AddForeignKey(string ColumnName, string ForeignKeyTable, string ForeignKeyColumn);

        /// <summary>
        /// Adds a trigger to the table
        /// </summary>
        /// <param name="Name">Name of the trigger</param>
        /// <param name="Definition">Definition of the trigger</param>
        /// <param name="Type">Trigger type</param>
        /// <returns>The trigger specified</returns>
        public abstract ITrigger AddTrigger(string Name, string Definition, Enums.TriggerType Type);

        /// <summary>
        /// Determines if a column exists in the table
        /// </summary>
        /// <param name="ColumnName">Column name</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool ContainsColumn(string ColumnName)
        {
            return this[ColumnName] != null;
        }

        /// <summary>
        /// Sets up foreign keys
        /// </summary>
        public void SetupForeignKeys()
        {
            this.Columns.ForEach(x => x.SetupForeignKeys());
        }
    }
}