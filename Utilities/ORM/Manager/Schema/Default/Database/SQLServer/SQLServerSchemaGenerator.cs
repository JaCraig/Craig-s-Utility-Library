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

#region Usings

using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.DataTypes;
using Utilities.DataTypes.Comparison;
using Utilities.ORM.Manager.Schema.Enums;
using Utilities.ORM.Manager.Schema.Interfaces;

#endregion Usings

namespace Utilities.ORM.Manager.Schema.Default.Database.SQLServer
{
    /// <summary>
    /// SQL Server schema generator
    /// </summary>
    public class SQLServerSchemaGenerator : ISchemaGenerator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SQLServerSchemaGenerator()
        {
            Provider = Utilities.IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>();
        }

        /// <summary>
        /// Provider name associated with the schema generator
        /// </summary>
        public string ProviderName { get { return "System.Data.SqlClient"; } }

        /// <summary>
        /// Query provider object
        /// </summary>
        protected QueryProvider.Manager Provider { get; private set; }

        /// <summary>
        /// Generates a list of commands used to modify the source. If it does not exist prior, the
        /// commands will create the source from scratch. Otherwise the commands will only add new
        /// fields, tables, etc. It does not delete old fields.
        /// </summary>
        /// <param name="DesiredStructure">Desired source structure</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>List of commands generated</returns>
        public IEnumerable<string> GenerateSchema(ISource DesiredStructure, string ConnectionString)
        {
            return new List<string>();
        }

        /// <summary>
        /// Gets the structure of a source
        /// </summary>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>The source structure</returns>
        public ISource GetSourceStructure(string ConnectionString)
        {
            string DatabaseName = Regex.Match(ConnectionString, "Initial Catalog=(.*?;)").Value.Replace("Initial Catalog=", "").Replace(";", "");
            if (!SourceExists(DatabaseName, ConnectionString))
                return null;
            Database Temp = new Database(DatabaseName);
            GetTables(ConnectionString, Temp);
            SetupTables(ConnectionString, Temp);
            SetupViews(ConnectionString, Temp);
            SetupStoredProcedures(ConnectionString, Temp);
            SetupFunctions(ConnectionString, Temp);
            return Temp;
        }

        /// <summary>
        /// Checks if a source exists
        /// </summary>
        /// <param name="Source">Source to check</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool SourceExists(string Source, string ConnectionString)
        {
            return Provider.Batch(ProviderName, ConnectionString)
                           .AddCommand("SELECT * FROM Master.sys.Databases WHERE name=@0", CommandType.Text, Source)
                           .Execute()
                           .Count() > 0;
        }

        /// <summary>
        /// Checks if a stored procedure exists
        /// </summary>
        /// <param name="StoredProcedure">Stored procedure to check</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool StoredProcedureExists(string StoredProcedure, string ConnectionString)
        {
            return Provider.Batch(ProviderName, ConnectionString)
                           .AddCommand("SELECT * FROM sys.Procedures WHERE name=@0", CommandType.Text, StoredProcedure)
                           .Execute()
                           .Count() > 0;
        }

        /// <summary>
        /// Checks if a table exists
        /// </summary>
        /// <param name="Table">Table to check</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool TableExists(string Table, string ConnectionString)
        {
            return Provider.Batch(ProviderName, ConnectionString)
                           .AddCommand("SELECT * FROM sys.Tables WHERE name=@0", CommandType.Text, Table)
                           .Execute()
                           .Count() > 0;
        }

        /// <summary>
        /// Checks if a trigger exists
        /// </summary>
        /// <param name="Trigger">Trigger to check</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool TriggerExists(string Trigger, string ConnectionString)
        {
            return Provider.Batch(ProviderName, ConnectionString)
                           .AddCommand("SELECT * FROM sys.triggers WHERE name=@0", CommandType.Text, Trigger)
                           .Execute()
                           .Count() > 0;
        }

        /// <summary>
        /// Checks if a view exists
        /// </summary>
        /// <param name="View">View to check</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool ViewExists(string View, string ConnectionString)
        {
            return Provider.Batch(ProviderName, ConnectionString)
                           .AddCommand("SELECT * FROM sys.views WHERE name=@0", CommandType.Text, View)
                           .Execute()
                           .Count() > 0;
        }

        private static void SetupColumns(Table Table, IEnumerable<dynamic> Values)
        {
            foreach (dynamic Item in Values)
            {
                if (Table.ContainsColumn(Item.Column))
                {
                    Table.AddForeignKey(Item.Column, Item.FOREIGN_KEY_TABLE, Item.FOREIGN_KEY_COLUMN);
                }
                else
                {
                    Table.AddColumn(Item.Column,
                        Item.COLUMN_TYPE.To<string, SqlDbType>().To(DbType.Int32),
                        (Item.COLUMN_TYPE == "nvarchar") ? Item.MAX_LENGTH / 2 : Item.MAX_LENGTH,
                        Item.IS_NULLABLE,
                        Item.IS_IDENTITY,
                        Item.IS_INDEX != 0,
                        !string.IsNullOrEmpty(Item.PRIMARY_KEY),
                        !string.IsNullOrEmpty(Item.UNIQUE),
                        Item.FOREIGN_KEY_TABLE,
                        Item.FOREIGN_KEY_COLUMN,
                        Item.DEFAULT_VALUE);
                }
            }
        }

        private void GetTables(string ConnectionString, Database Temp)
        {
            IEnumerable<dynamic> Values = Provider.Batch(ProviderName, ConnectionString)
                                                  .AddCommand(CommandType.Text, "SELECT TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE FROM INFORMATION_SCHEMA.TABLES")
                                                  .Execute();
            foreach (dynamic Item in Values)
            {
                string TableName = Item.TABLE_NAME;
                string TableType = Item.TABLE_TYPE;
                if (TableType == "BASE TABLE")
                    Temp.AddTable(TableName);
                else if (TableType == "VIEW")
                    Temp.AddView(TableName);
            }
        }

        private void SetupFunctions(string ConnectionString, Database Temp)
        {
            IEnumerable<dynamic> Values = Provider.Batch(ProviderName, ConnectionString)
                                                      .AddCommand(CommandType.Text,
                                                            "SELECT SPECIFIC_NAME as NAME,ROUTINE_DEFINITION as DEFINITION FROM INFORMATION_SCHEMA.ROUTINES WHERE INFORMATION_SCHEMA.ROUTINES.ROUTINE_TYPE='FUNCTION'")
                                                      .Execute();
            foreach (dynamic Item in Values)
            {
                Temp.AddFunction(Item.NAME, Item.DEFINITION);
            }
        }

        private void SetupStoredProcedures(string ConnectionString, Database Temp)
        {
            IEnumerable<dynamic> Values = Provider.Batch(ProviderName, ConnectionString)
                                                      .AddCommand(CommandType.Text,
                                                            "SELECT sys.procedures.name as NAME,OBJECT_DEFINITION(sys.procedures.object_id) as DEFINITION FROM sys.procedures")
                                                      .Execute();
            foreach (dynamic Item in Values)
            {
                Temp.AddStoredProcedure(Item.NAME, Item.DEFINITION);
            }
            foreach (StoredProcedure Procedure in Temp.StoredProcedures)
            {
                Values = Provider.Batch(ProviderName, ConnectionString)
                                .AddCommand(@"SELECT sys.systypes.name as TYPE,sys.parameters.name as NAME,sys.parameters.max_length as LENGTH,sys.parameters.default_value as [DEFAULT VALUE] FROM sys.procedures INNER JOIN sys.parameters on sys.procedures.object_id=sys.parameters.object_id INNER JOIN sys.systypes on sys.systypes.xusertype=sys.parameters.system_type_id WHERE sys.procedures.name=@0 AND (sys.systypes.xusertype <> 256)",
                                        CommandType.Text,
                                        Procedure.Name)
                                .Execute();
                foreach (dynamic Item in Values)
                {
                    string Type = Item.TYPE;
                    string Name = Item.NAME;
                    int Length = Item.LENGTH;
                    if (Type == "nvarchar")
                        Length /= 2;
                    string Default = Item.DEFAULT_VALUE;
                    Procedure.AddColumn<string>(Name, Type.To<string, SqlDbType>().To(DbType.Int32), Length, DefaultValue: Default);
                }
            }
        }

        private void SetupTables(string ConnectionString, Database Temp)
        {
            foreach (Table Table in Temp.Tables)
            {
                IEnumerable<dynamic> Values = Provider.Batch(ProviderName, ConnectionString)
                                                      .AddCommand(@"SELECT sys.columns.name AS [Column], sys.systypes.name AS [COLUMN_TYPE],
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
                                                                WHERE (sys.tables.name = @0) AND (sys.systypes.xusertype <> 256)",
                                                                CommandType.Text,
                                                                Table.Name)
                                                      .Execute();
                SetupColumns(Table, Values);
                SetupTriggers(ConnectionString, Table, Values);
            }
            foreach (Table Table in Temp.Tables)
            {
                Table.SetupForeignKeys();
            }
        }

        private void SetupTriggers(string ConnectionString, Table Table, IEnumerable<dynamic> Values)
        {
            Values = Provider.Batch(ProviderName, ConnectionString)
                             .AddCommand(@"SELECT sys.triggers.name as Name,sys.trigger_events.type as Type,
                                                OBJECT_DEFINITION(sys.triggers.object_id) as Definition 
                                                FROM sys.triggers 
                                                INNER JOIN sys.trigger_events ON sys.triggers.object_id=sys.trigger_events.object_id 
                                                INNER JOIN sys.tables on sys.triggers.parent_id=sys.tables.object_id 
                                                where sys.tables.name=@0",
                                    CommandType.Text,
                                    Table.Name)
                             .Execute();
            foreach (dynamic Item in Values)
            {
                string Name = Item.Name;
                int Type = Item.Type;
                string Definition = Item.Definition;
                Table.AddTrigger(Name, Definition, Type.ToString(CultureInfo.InvariantCulture).To<string, TriggerType>());
            }
        }

        private void SetupViews(string ConnectionString, Database Temp)
        {
            foreach (View View in Temp.Views)
            {
                IEnumerable<dynamic> Values = Provider.Batch(ProviderName, ConnectionString)
                                                      .AddCommand(@"SELECT OBJECT_DEFINITION(sys.views.object_id) as Definition FROM sys.views WHERE sys.views.name=@0",
                                                                CommandType.Text,
                                                                View.Name)
                                                      .Execute();
                View.Definition = Values.First().Definition;
                Values = Provider.Batch(ProviderName, ConnectionString)
                                 .AddCommand(@"SELECT sys.columns.name AS [Column], sys.systypes.name AS [COLUMN_TYPE],
                                                        sys.columns.max_length as [MAX_LENGTH], sys.columns.is_nullable as [IS_NULLABLE] 
                                                        FROM sys.views 
                                                        INNER JOIN sys.columns on sys.columns.object_id=sys.views.object_id 
                                                        INNER JOIN sys.systypes ON sys.systypes.xtype = sys.columns.system_type_id 
                                                        WHERE (sys.views.name = @0) AND (sys.systypes.xusertype <> 256)",
                                        CommandType.Text,
                                        View.Name)
                                 .Execute();
                foreach (dynamic Item in Values)
                {
                    string ColumnName = Item.Column;
                    string ColumnType = Item.COLUMN_TYPE;
                    int MaxLength = Item.MAX_LENGTH;
                    if (ColumnType == "nvarchar")
                        MaxLength /= 2;
                    bool Nullable = Item.IS_NULLABLE;
                    View.AddColumn<string>(ColumnName, ColumnType.To<string, SqlDbType>().To(DbType.Int32), MaxLength, Nullable);
                }
            }
        }
    }
}