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
using Utilities.ORM.Manager.Schema.Enums;

namespace Utilities.ORM.Manager.Schema.Interfaces
{
    /// <summary>
    /// Interface for table like structures
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Columns
        /// </summary>
        ICollection<IColumn> Columns { get; }

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Parent of the table structure
        /// </summary>
        ISource Source { get; }

        /// <summary>
        /// Triggers associated with the table (if source supports them)
        /// </summary>
        ICollection<ITrigger> Triggers { get; }

        /// <summary>
        /// Returns the specified column
        /// </summary>
        /// <param name="Name">Name of the column</param>
        /// <returns>Column specified</returns>
        IColumn this[string Name] { get; }

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
        IColumn AddColumn<T>(string ColumnName, DbType ColumnType, int Length = 0, bool Nullable = true,
            bool Identity = false, bool Index = false, bool PrimaryKey = false, bool Unique = false,
            string ForeignKeyTable = "", string ForeignKeyColumn = "", T DefaultValue = default(T),
            bool OnDeleteCascade = false, bool OnUpdateCascade = false, bool OnDeleteSetNull = false);

        /// <summary>
        /// Adds a foreign key
        /// </summary>
        /// <param name="ColumnName">Column name</param>
        /// <param name="ForeignKeyTable">Foreign key table</param>
        /// <param name="ForeignKeyColumn">Foreign key column</param>
        void AddForeignKey(string ColumnName, string ForeignKeyTable, string ForeignKeyColumn);

        /// <summary>
        /// Adds a trigger to the table
        /// </summary>
        /// <param name="Name">Name of the trigger</param>
        /// <param name="Definition">Trigger definition</param>
        /// <param name="Type">Trigger type</param>
        /// <returns>Trigger added to the table</returns>
        ITrigger AddTrigger(string Name, string Definition, TriggerType Type);

        /// <summary>
        /// Determines if a column exists in the table
        /// </summary>
        /// <param name="ColumnName">Column name</param>
        /// <returns>True if it exists, false otherwise</returns>
        bool ContainsColumn(string ColumnName);

        /// <summary>
        /// Sets up foreign keys
        /// </summary>
        void SetupForeignKeys();
    }
}