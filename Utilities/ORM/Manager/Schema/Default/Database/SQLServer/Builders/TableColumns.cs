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

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.Schema.Default.Database.SQLServer.Builders.Interfaces;
using Utilities.ORM.Manager.Schema.Interfaces;

namespace Utilities.ORM.Manager.Schema.Default.Database.SQLServer.Builders
{
    /// <summary>
    /// Table column builder, gets info and does diffs for tables
    /// </summary>
    public class TableColumns : IBuilder
    {
        /// <summary>
        /// Fills the database.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="database">The database.</param>
        public void FillDatabase(IEnumerable<dynamic> values, Database database)
        {
            Contract.Requires<NullReferenceException>(database != null, "database");
            if (values == null || values.Count() == 0)
                return;
            foreach (dynamic Item in values)
            {
                string TableName = Item.TABLE;
                SetupColumns(database.Tables.FirstOrDefault(x => x.Name == Item.Table), Item);
            }
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="batch">The batch.</param>
        public void GetCommand(IBatch batch)
        {
            Contract.Requires<NullReferenceException>(batch != null, "batch");
            batch.AddCommand(null, null, CommandType.Text, @"SELECT sys.tables.name as [Table],sys.columns.name AS [Column], sys.systypes.name AS [COLUMN_TYPE],
                                                                sys.columns.max_length as [MAX_LENGTH], sys.columns.is_nullable as [IS_NULLABLE],
                                                                sys.columns.is_identity as [IS_IDENTITY], sys.index_columns.index_id as [IS_INDEX],
                                                                key_constraints.name as [PRIMARY_KEY], key_constraints_1.name as [UNIQUE],
                                                                tables_1.name as [FOREIGN_KEY_TABLE], columns_1.name as [FOREIGN_KEY_COLUMN],
                                                                sys.default_constraints.definition as [DEFAULT_VALUE]
                                                                FROM sys.tables
                                                                INNER JOIN sys.columns on sys.columns.object_id=sys.tables.object_id
                                                                INNER JOIN sys.systypes ON sys.systypes.xtype = sys.columns.system_type_id
                                                                LEFT OUTER JOIN sys.index_columns on sys.index_columns.object_id=sys.tables.object_id and sys.index_columns.column_id=sys.columns.column_id
                                                                LEFT OUTER JOIN sys.key_constraints on sys.key_constraints.parent_object_id=sys.tables.object_id and sys.key_constraints.parent_object_id=sys.index_columns.object_id and sys.index_columns.index_id=sys.key_constraints.unique_index_id and sys.key_constraints.type='PK'
                                                                LEFT OUTER JOIN sys.foreign_key_columns on sys.foreign_key_columns.parent_object_id=sys.tables.object_id and sys.foreign_key_columns.parent_column_id=sys.columns.column_id
                                                                LEFT OUTER JOIN sys.tables as tables_1 on tables_1.object_id=sys.foreign_key_columns.referenced_object_id
                                                                LEFT OUTER JOIN sys.columns as columns_1 on columns_1.column_id=sys.foreign_key_columns.referenced_column_id and columns_1.object_id=tables_1.object_id
                                                                LEFT OUTER JOIN sys.key_constraints as key_constraints_1 on key_constraints_1.parent_object_id=sys.tables.object_id and key_constraints_1.parent_object_id=sys.index_columns.object_id and sys.index_columns.index_id=key_constraints_1.unique_index_id and key_constraints_1.type='UQ'
                                                                LEFT OUTER JOIN sys.default_constraints on sys.default_constraints.object_id=sys.columns.default_object_id
                                                                WHERE sys.systypes.xusertype <> 256");
        }

        /// <summary>
        /// Setups the columns.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="item">The item.</param>
        private static void SetupColumns(ITable table, dynamic item)
        {
            Contract.Requires<ArgumentNullException>(((object)item) != null, "item");
            Contract.Requires<ArgumentNullException>(table != null, "table");
            if (table.ContainsColumn(item.Column))
            {
                table.AddForeignKey(item.Column, item.FOREIGN_KEY_TABLE, item.FOREIGN_KEY_COLUMN);
            }
            else
            {
                table.AddColumn<string>(item.Column,
                    Utilities.DataTypes.TypeConversionExtensions.To(Utilities.DataTypes.TypeConversionExtensions.To<string, SqlDbType>(item.COLUMN_TYPE), DbType.Int32),
                    (item.COLUMN_TYPE == "nvarchar") ? item.MAX_LENGTH / 2 : item.MAX_LENGTH,
                    item.IS_NULLABLE,
                    item.IS_IDENTITY,
                    item.IS_INDEX != 0,
                    !string.IsNullOrEmpty(item.PRIMARY_KEY),
                    !string.IsNullOrEmpty(item.UNIQUE),
                    item.FOREIGN_KEY_TABLE,
                    item.FOREIGN_KEY_COLUMN,
                    item.DEFAULT_VALUE);
            }
        }
    }
}