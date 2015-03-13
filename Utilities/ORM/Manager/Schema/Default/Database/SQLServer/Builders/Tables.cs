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

namespace Utilities.ORM.Manager.Schema.Default.Database.SQLServer.Builders
{
    /// <summary>
    /// Table builder, gets info and does diffs for tables
    /// </summary>
    public class Tables : IBuilder
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
                string TableName = Item.TABLE_NAME;
                string TableType = Item.TABLE_TYPE;
                if (TableType == "BASE TABLE")
                    database.AddTable(TableName);
                else if (TableType == "VIEW")
                    database.AddView(TableName);
            }
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="batch">The batch.</param>
        public void GetCommand(IBatch batch)
        {
            Contract.Requires<NullReferenceException>(batch != null, "batch");
            batch.AddCommand(null, null, CommandType.Text, "SELECT TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE FROM INFORMATION_SCHEMA.TABLES");
        }
    }
}