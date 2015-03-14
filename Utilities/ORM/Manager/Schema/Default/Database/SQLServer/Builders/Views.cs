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
using Utilities.DataTypes;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.Schema.Default.Database.SQLServer.Builders.Interfaces;
using Utilities.ORM.Manager.Schema.Interfaces;

namespace Utilities.ORM.Manager.Schema.Default.Database.SQLServer.Builders
{
    /// <summary>
    /// View builder, gets info and does diffs for Views
    /// </summary>
    public class Views : IBuilder
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
                SetupViews(database.Views.FirstOrDefault(x => x.Name == Item.View), Item);
            }
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="batch">The batch.</param>
        public void GetCommand(IBatch batch)
        {
            Contract.Requires<NullReferenceException>(batch != null, "batch");
            batch.AddCommand(null, null, CommandType.Text, @"SELECT sys.views.name as [View],OBJECT_DEFINITION(sys.views.object_id) as Definition,
                                                        sys.columns.name AS [Column], sys.systypes.name AS [COLUMN_TYPE],
                                                        sys.columns.max_length as [MAX_LENGTH], sys.columns.is_nullable as [IS_NULLABLE]
                                                        FROM sys.views
                                                        INNER JOIN sys.columns on sys.columns.object_id=sys.views.object_id
                                                        INNER JOIN sys.systypes ON sys.systypes.xtype = sys.columns.system_type_id
                                                        WHERE sys.systypes.xusertype <> 256");
        }

        /// <summary>
        /// Setups the views.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="item">The item.</param>
        private static void SetupViews(ITable table, dynamic item)
        {
            Contract.Requires<ArgumentNullException>(((object)item) != null, "item");
            Contract.Requires<ArgumentNullException>(table != null, "table");
            View View = (View)table;
            View.Definition = item.Definition;
            string ColumnName = item.Column;
            string ColumnType = item.COLUMN_TYPE;
            int MaxLength = item.MAX_LENGTH;
            if (ColumnType == "nvarchar")
                MaxLength /= 2;
            bool Nullable = item.IS_NULLABLE;
            View.AddColumn<string>(ColumnName, ColumnType.To<string, SqlDbType>().To(DbType.Int32), MaxLength, Nullable);
        }
    }
}