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

namespace Utilities.ORM.Manager.Schema.Interfaces
{
    /// <summary>
    /// Column interface
    /// </summary>
    public interface IColumn
    {
        /// <summary>
        /// Auto increment?
        /// </summary>
        bool AutoIncrement { get; set; }

        /// <summary>
        /// Data type
        /// </summary>
        DbType DataType { get; set; }

        /// <summary>
        /// Default value
        /// </summary>
        string Default { get; set; }

        /// <summary>
        /// Foreign keys
        /// </summary>
        ICollection<IColumn> ForeignKey { get; }

        /// <summary>
        /// Index?
        /// </summary>
        bool Index { get; set; }

        /// <summary>
        /// Data length
        /// </summary>
        int Length { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Nullable?
        /// </summary>
        bool Nullable { get; set; }

        /// <summary>
        /// On Delete Cascade
        /// </summary>
        bool OnDeleteCascade { get; set; }

        /// <summary>
        /// On Delete Set Null
        /// </summary>
        bool OnDeleteSetNull { get; set; }

        /// <summary>
        /// On Update Cascade
        /// </summary>
        bool OnUpdateCascade { get; set; }

        /// <summary>
        /// Parent table
        /// </summary>
        ITable ParentTable { get; set; }

        /// <summary>
        /// Primary key?
        /// </summary>
        bool PrimaryKey { get; set; }

        /// <summary>
        /// Unique?
        /// </summary>
        bool Unique { get; set; }

        /// <summary>
        /// Add foreign key
        /// </summary>
        /// <param name="ForeignKeyTable">Table of the foreign key</param>
        /// <param name="ForeignKeyColumn">Column of the foreign key</param>
        void AddForeignKey(string ForeignKeyTable, string ForeignKeyColumn);

        /// <summary>
        /// Sets up the foreign key list
        /// </summary>
        void SetupForeignKeys();
    }
}