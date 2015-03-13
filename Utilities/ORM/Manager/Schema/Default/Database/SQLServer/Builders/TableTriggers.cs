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
using System.Globalization;
using System.Linq;
using Utilities.DataTypes;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.Schema.Default.Database.SQLServer.Builders.Interfaces;
using Utilities.ORM.Manager.Schema.Enums;
using Utilities.ORM.Manager.Schema.Interfaces;

namespace Utilities.ORM.Manager.Schema.Default.Database.SQLServer.Builders
{
    /// <summary>
    /// Table trigger builder, gets info and does diffs for tables
    /// </summary>
    public class TableTriggers : IBuilder
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
                SetupTriggers(database.Tables.FirstOrDefault(x => x.Name == Item.Table), Item);
            }
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="batch">The batch.</param>
        public void GetCommand(IBatch batch)
        {
            Contract.Requires<NullReferenceException>(batch != null, "batch");
            batch.AddCommand(null, null, CommandType.Text, @"SELECT sys.tables.name as [Table],sys.triggers.name as Name,sys.trigger_events.type as Type,
                                                                OBJECT_DEFINITION(sys.triggers.object_id) as Definition
                                                                FROM sys.triggers
                                                                INNER JOIN sys.trigger_events ON sys.triggers.object_id=sys.trigger_events.object_id
                                                                INNER JOIN sys.tables on sys.triggers.parent_id=sys.tables.object_id");
        }

        /// <summary>
        /// Setups the columns.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="item">The item.</param>
        private static void SetupTriggers(ITable table, dynamic item)
        {
            Contract.Requires<ArgumentNullException>(((object)item) != null, "item");
            Contract.Requires<ArgumentNullException>(table != null, "table");
            string Name = item.Name;
            int Type = item.Type;
            string Definition = item.Definition;
            table.AddTrigger(Name, Definition, Type.ToString(CultureInfo.InvariantCulture).To<string, TriggerType>());
        }
    }
}