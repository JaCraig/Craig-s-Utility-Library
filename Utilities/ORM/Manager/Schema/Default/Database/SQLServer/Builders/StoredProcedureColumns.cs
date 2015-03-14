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
    /// StoredProcedure column builder, gets info and does diffs for StoredProcedures
    /// </summary>
    public class StoredProcedureColumns : IBuilder
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
                SetupStoredProcedures(database.StoredProcedures.FirstOrDefault(x => x.Name == Item.Procedure), Item);
            }
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="batch">The batch.</param>
        public void GetCommand(IBatch batch)
        {
            Contract.Requires<NullReferenceException>(batch != null, "batch");
            batch.AddCommand(null, null, CommandType.Text, @"SELECT sys.procedures.name as [Procedure],sys.systypes.name as TYPE,sys.parameters.name as NAME,
sys.parameters.max_length as LENGTH,sys.parameters.default_value as [DEFAULT VALUE]
FROM sys.procedures
INNER JOIN sys.parameters on sys.procedures.object_id=sys.parameters.object_id
INNER JOIN sys.systypes on sys.systypes.xusertype=sys.parameters.system_type_id
WHERE sys.systypes.xusertype <> 256");
        }

        /// <summary>
        /// Setups the stored procedures.
        /// </summary>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="item">The item.</param>
        private void SetupStoredProcedures(ITable storedProcedure, dynamic item)
        {
            Contract.Requires<ArgumentNullException>(storedProcedure != null, "storedProcedure");
            Contract.Requires<NullReferenceException>(item != null, "item");
            string Type = item.TYPE;
            string Name = item.NAME;
            int Length = item.LENGTH;
            if (Type == "nvarchar")
                Length /= 2;
            string Default = item.DEFAULT_VALUE;
            storedProcedure.AddColumn<string>(Name, Type.To<string, SqlDbType>().To(DbType.Int32), Length, DefaultValue: Default);
        }
    }
}