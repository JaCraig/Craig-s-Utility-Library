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
using Utilities.DataTypes.ExtensionMethods;
using Utilities.SQL.DataClasses.Interfaces;
#endregion

namespace Utilities.SQL.DataClasses
{
    /// <summary>
    /// Column class
    /// </summary>
    /// <typeparam name="T">Data type of the column</typeparam>
    public class Column<T> : IColumn
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Column()
        {
            this.ForeignKey = new List<IColumn>();
            this.ForeignKeyColumns = new List<string>();
            this.ForeignKeyTables = new List<string>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Name of the column</param>
        /// <param name="ColumnType">The data type</param>
        /// <param name="Length">The data length</param>
        /// <param name="Nullable">Is it nullable?</param>
        /// <param name="Identity">Is it an identity?</param>
        /// <param name="Index">Is it the index?</param>
        /// <param name="PrimaryKey">Is it the primary key?</param>
        /// <param name="Unique">Is it unique?</param>
        /// <param name="ForeignKeyTable">Foreign key table</param>
        /// <param name="ForeignKeyColumn">Foreign key column</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="ParentTable">Parent table</param>
        /// <param name="OnDeleteCascade">Cascade on delete</param>
        /// <param name="OnDeleteSetNull">Set null on delete</param>
        /// <param name="OnUpdateCascade">Cascade on update</param>
        public Column(string Name, DbType ColumnType, int Length, bool Nullable,
            bool Identity, bool Index, bool PrimaryKey, bool Unique, string ForeignKeyTable,
            string ForeignKeyColumn, T DefaultValue,bool OnDeleteCascade, bool OnUpdateCascade,
            bool OnDeleteSetNull, ITable ParentTable)
        {
            this.Name = Name;
            this.ForeignKey = new List<IColumn>();
            this.ForeignKeyColumns = new List<string>();
            this.ForeignKeyTables = new List<string>();
            this.ParentTable = ParentTable;
            this.DataType = ColumnType;
            this.Length = Length;
            this.Nullable = Nullable;
            this.AutoIncrement = Identity;
            this.Index = Index;
            this.PrimaryKey = PrimaryKey;
            this.Unique = Unique;
            this.Default = DefaultValue.IsDefault() ? "" : DefaultValue.ToString();
            this.OnDeleteCascade = OnDeleteCascade;
            this.OnUpdateCascade = OnUpdateCascade;
            this.OnDeleteSetNull = OnDeleteSetNull;
            AddForeignKey(ForeignKeyTable, ForeignKeyColumn);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// On Delete Cascade
        /// </summary>
        public bool OnDeleteCascade { get; set; }

        /// <summary>
        /// On Update Cascade
        /// </summary>
        public bool OnUpdateCascade { get; set; }

        /// <summary>
        /// On Delete Set Null
        /// </summary>
        public bool OnDeleteSetNull { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Data type
        /// </summary>
        public DbType DataType { get; set; }

        /// <summary>
        /// Data length
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Foreign keys
        /// </summary>
        public List<IColumn> ForeignKey { get;private set; }

        /// <summary>
        /// Primary key?
        /// </summary>
        public bool PrimaryKey { get; set; }

        /// <summary>
        /// Nullable?
        /// </summary>
        public bool Nullable { get; set; }

        /// <summary>
        /// Unique?
        /// </summary>
        public bool Unique { get; set; }

        /// <summary>
        /// Index?
        /// </summary>
        public bool Index { get; set; }

        /// <summary>
        /// Auto increment?
        /// </summary>
        public bool AutoIncrement { get; set; }

        /// <summary>
        /// Default value
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// Parent table
        /// </summary>
        public ITable ParentTable { get; set; }

        #endregion

        #region Private Variables

        private List<string> ForeignKeyTables { get; set; }
        private List<string> ForeignKeyColumns { get; set; }

        #endregion

        #region Public Functions

        /// <summary>
        /// Add foreign key
        /// </summary>
        /// <param name="ForeignKeyTable">Table of the foreign key</param>
        /// <param name="ForeignKeyColumn">Column of the foreign key</param>
        public virtual void AddForeignKey(string ForeignKeyTable, string ForeignKeyColumn)
        {
            if (string.IsNullOrEmpty(ForeignKeyTable) || string.IsNullOrEmpty(ForeignKeyColumn))
                return;
            ForeignKeyColumns.Add(ForeignKeyColumn);
            ForeignKeyTables.Add(ForeignKeyTable);
        }

        /// <summary>
        /// Sets up the foreign key list
        /// </summary>
        public virtual void SetupForeignKeys()
        {
            for (int x = 0; x < ForeignKeyColumns.Count; ++x)
            {
                Database TempDatabase = ParentTable.ParentDatabase;
                if (TempDatabase != null)
                {
                    foreach (Table Table in TempDatabase.Tables)
                    {
                        if (Table.Name == ForeignKeyTables[x])
                        {
                            foreach (IColumn Column in Table.Columns)
                            {
                                if (Column.Name == ForeignKeyColumns[x])
                                {
                                    ForeignKey.Add(Column);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}