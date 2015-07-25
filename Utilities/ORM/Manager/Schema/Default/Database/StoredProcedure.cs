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

using System.Data;
using Utilities.DataTypes;
using Utilities.ORM.Manager.Schema.BaseClasses;
using Utilities.ORM.Manager.Schema.Enums;
using Utilities.ORM.Manager.Schema.Interfaces;

namespace Utilities.ORM.Manager.Schema.Default.Database
{
    /// <summary>
    /// StoredProcedure class
    /// </summary>
    public class StoredProcedure : TableBase, IFunction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="Source">Source</param>
        /// <param name="Definition">Definition of the stored procedure</param>
        public StoredProcedure(string Name, string Definition, ISource Source)
            : base(Name, Source)
        {
            this.Definition = Definition;
        }

        /// <summary>
        /// Definition of the stored procedure
        /// </summary>
        public string Definition { get; set; }

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
        public override IColumn AddColumn<T>(string ColumnName, DbType ColumnType, int Length = 0, bool Nullable = true, bool Identity = false, bool Index = false, bool PrimaryKey = false, bool Unique = false, string ForeignKeyTable = "", string ForeignKeyColumn = "", T DefaultValue = default(T), bool OnDeleteCascade = false, bool OnUpdateCascade = false, bool OnDeleteSetNull = false)
        {
            return Columns.AddAndReturn(new Column<T>(ColumnName, ColumnType, Length, Nullable, Identity, Index, PrimaryKey, Unique, ForeignKeyTable, ForeignKeyColumn, DefaultValue, OnDeleteCascade, OnUpdateCascade, OnDeleteSetNull, this));
        }

        /// <summary>
        /// Adds a foreign key
        /// </summary>
        /// <param name="ColumnName">Column name</param>
        /// <param name="ForeignKeyTable">Foreign key table</param>
        /// <param name="ForeignKeyColumn">Foreign key column</param>
        public override void AddForeignKey(string ColumnName, string ForeignKeyTable, string ForeignKeyColumn)
        {
        }

        /// <summary>
        /// Adds a trigger to the table
        /// </summary>
        /// <param name="Name">Name of the trigger</param>
        /// <param name="Definition">Definition of the trigger</param>
        /// <param name="Type">Trigger type</param>
        /// <returns>The trigger specified</returns>
        public override ITrigger AddTrigger(string Name, string Definition, TriggerType Type)
        {
            return null;
        }
    }
}