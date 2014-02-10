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

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.DataTypes;
using Utilities.ORM.Manager.Schema.Enums;
using Utilities.ORM.Manager.Schema.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

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
        }

        /// <summary>
        /// Provider name associated with the schema generator
        /// </summary>
        public string ProviderName { get { return "System.Data.SqlClient"; } }

        /// <summary>
        /// Query provider object
        /// </summary>
        protected static QueryProvider.Manager Provider { get { return Utilities.IoC.Manager.Bootstrapper.Resolve<QueryProvider.Manager>(); } }

        /// <summary>
        /// Generates a list of commands used to modify the source. If it does not exist prior, the
        /// commands will create the source from scratch. Otherwise the commands will only add new
        /// fields, tables, etc. It does not delete old fields.
        /// </summary>
        /// <param name="DesiredStructure">Desired source structure</param>
        /// <param name="Source">Source to use</param>
        /// <returns>List of commands generated</returns>
        public IEnumerable<string> GenerateSchema(ISource DesiredStructure, ISourceInfo Source)
        {
            ISource CurrentStructure = GetSourceStructure(Source);
            return BuildCommands(DesiredStructure, CurrentStructure).ToArray();
        }

        /// <summary>
        /// Gets the structure of a source
        /// </summary>
        /// <param name="Source">Source to use</param>
        /// <returns>The source structure</returns>
        public ISource GetSourceStructure(ISourceInfo Source)
        {
            string DatabaseName = Regex.Match(Source.Connection, "Initial Catalog=(.*?;)").Value.Replace("Initial Catalog=", "").Replace(";", "");
            if (!SourceExists(DatabaseName, Source))
                return null;
            Database Temp = new Database(DatabaseName);
            GetTables(Source, Temp);
            SetupTables(Source, Temp);
            SetupViews(Source, Temp);
            SetupStoredProcedures(Source, Temp);
            SetupFunctions(Source, Temp);
            return Temp;
        }

        /// <summary>
        /// Checks if a source exists
        /// </summary>
        /// <param name="Source">Source to check</param>
        /// <param name="Info">Source info to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool SourceExists(string Source, ISourceInfo Info)
        {
            return Exists("SELECT * FROM Master.sys.Databases WHERE name=@0", Source, Info);
        }

        /// <summary>
        /// Checks if a stored procedure exists
        /// </summary>
        /// <param name="StoredProcedure">Stored procedure to check</param>
        /// <param name="Source">Source to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool StoredProcedureExists(string StoredProcedure, ISourceInfo Source)
        {
            return Exists("SELECT * FROM sys.Procedures WHERE name=@0", StoredProcedure, Source);
        }

        /// <summary>
        /// Checks if a table exists
        /// </summary>
        /// <param name="Table">Table to check</param>
        /// <param name="Source">Source to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool TableExists(string Table, ISourceInfo Source)
        {
            return Exists("SELECT * FROM sys.Tables WHERE name=@0", Table, Source);
        }

        /// <summary>
        /// Checks if a trigger exists
        /// </summary>
        /// <param name="Trigger">Trigger to check</param>
        /// <param name="Source">Source to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool TriggerExists(string Trigger, ISourceInfo Source)
        {
            return Exists("SELECT * FROM sys.triggers WHERE name=@0", Trigger, Source);
        }

        /// <summary>
        /// Checks if a view exists
        /// </summary>
        /// <param name="View">View to check</param>
        /// <param name="Source">Source to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool ViewExists(string View, ISourceInfo Source)
        {
            return Exists("SELECT * FROM sys.views WHERE name=@0", View, Source);
        }

        private static IEnumerable<string> BuildCommands(ISource DesiredStructure, ISource CurrentStructure)
        {
            List<string> Commands = new List<string>();
            DesiredStructure = DesiredStructure.Check(new Database(""));
            if (CurrentStructure == null)
                Commands.Add(string.Format(CultureInfo.CurrentCulture,
                    "EXEC dbo.sp_executesql @statement = N'CREATE DATABASE {0}'",
                    DesiredStructure.Name));
            CurrentStructure = CurrentStructure.Check(new Database(DesiredStructure.Name));
            foreach (Table Table in DesiredStructure.Tables)
            {
                ITable CurrentTable = CurrentStructure[Table.Name];
                Commands.Add((CurrentTable == null) ? GetTableCommand(Table) : GetAlterTableCommand(Table, CurrentTable));
            }
            foreach (Table Table in DesiredStructure.Tables)
            {
                Commands.Add(GetForeignKeyCommand(Table));
                ITable CurrentTable = CurrentStructure[Table.Name];
                Commands.Add((CurrentTable == null) ? GetTriggerCommand(Table) : GetAlterTriggerCommand(Table, CurrentTable));
            }
            foreach (Function Function in DesiredStructure.Functions)
            {
                Function CurrentFunction = (Function)CurrentStructure.Functions.FirstOrDefault(x => x.Name == Function.Name);
                Commands.Add(CurrentFunction != null ? GetAlterFunctionCommand(Function, CurrentFunction) : GetFunctionCommand(Function));
            }
            foreach (View View in DesiredStructure.Views)
            {
                View CurrentView = (View)CurrentStructure.Views.FirstOrDefault(x => x.Name == View.Name);
                Commands.Add(CurrentView != null ? GetAlterViewCommand(View, CurrentView) : GetViewCommand(View));
            }
            foreach (StoredProcedure StoredProcedure in DesiredStructure.StoredProcedures)
            {
                StoredProcedure CurrentStoredProcedure = (StoredProcedure)CurrentStructure.StoredProcedures.FirstOrDefault(x => x.Name == StoredProcedure.Name);
                Commands.Add(CurrentStoredProcedure != null ? GetAlterStoredProcedure(StoredProcedure, CurrentStoredProcedure) : GetStoredProcedure(StoredProcedure));
            }
            return Commands;
        }

        private static bool Exists(string Command, string Value, ISourceInfo Source)
        {
            Contract.Requires<ArgumentNullException>(Source != null, "Source");
            return Provider.Batch(Source)
                           .AddCommand(null, null, Command, CommandType.Text, Value)
                           .Execute()[0]
                           .Count() > 0;
        }

        private static IEnumerable<string> GetAlterFunctionCommand(Function Function, Function CurrentFunction)
        {
            Contract.Requires<ArgumentNullException>(Function != null, "Function");
            Contract.Requires<ArgumentNullException>(CurrentFunction != null, "CurrentFunction");
            List<string> ReturnValue = new List<string>();
            if (Function.Definition != CurrentFunction.Definition)
            {
                ReturnValue.Add(string.Format(CultureInfo.CurrentCulture,
                    "EXEC dbo.sp_executesql @statement = N'DROP FUNCTION {0}'",
                    Function.Name));
                ReturnValue.Add(GetFunctionCommand(Function));
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetAlterStoredProcedure(StoredProcedure StoredProcedure, StoredProcedure CurrentStoredProcedure)
        {
            Contract.Requires<ArgumentNullException>(StoredProcedure != null, "StoredProcedure");
            Contract.Requires<ArgumentNullException>(CurrentStoredProcedure != null, "CurrentStoredProcedure");
            List<string> ReturnValue = new List<string>();
            if (StoredProcedure.Definition != CurrentStoredProcedure.Definition)
            {
                ReturnValue.Add(string.Format(CultureInfo.CurrentCulture,
                    "EXEC dbo.sp_executesql @statement = N'DROP PROCEDURE {0}'",
                    StoredProcedure.Name));
                ReturnValue.Add(GetStoredProcedure(StoredProcedure));
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetAlterTableCommand(Table Table, ITable CurrentTable)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            List<string> ReturnValue = new List<string>();
            foreach (IColumn Column in Table.Columns)
            {
                IColumn CurrentColumn = CurrentTable[Column.Name];
                string Command = "";
                if (CurrentColumn == null)
                {
                    Command = string.Format(CultureInfo.CurrentCulture,
                        "EXEC dbo.sp_executesql @statement = N'ALTER TABLE {0} ADD {1} {2}",
                        Table.Name,
                        Column.Name,
                        Column.DataType.To(SqlDbType.Int).ToString());
                    if (Column.DataType == SqlDbType.VarChar.To(DbType.Int32) || Column.DataType == SqlDbType.NVarChar.To(DbType.Int32))
                    {
                        Command += (Column.Length < 0 || Column.Length >= 4000) ?
                                        "(MAX)" :
                                        "(" + Column.Length.ToString(CultureInfo.InvariantCulture) + ")";
                    }
                    Command += "'";
                    ReturnValue.Add(Command);
                    foreach (IColumn ForeignKey in Column.ForeignKey)
                    {
                        Command = string.Format(CultureInfo.CurrentCulture,
                            "EXEC dbo.sp_executesql @statement = N'ALTER TABLE {0} ADD FOREIGN KEY ({1}) REFERENCES {2}({3}){4}{5}{6}'",
                            Table.Name,
                            Column.Name,
                            ForeignKey.ParentTable.Name,
                            ForeignKey.Name,
                            Column.OnDeleteCascade ? " ON DELETE CASCADE" : "",
                            Column.OnUpdateCascade ? " ON UPDATE CASCADE" : "",
                            Column.OnDeleteSetNull ? " ON DELETE SET NULL" : "");
                        ReturnValue.Add(Command);
                    }
                }
                else if (CurrentColumn.DataType != Column.DataType
                    || (CurrentColumn.DataType == Column.DataType
                        && CurrentColumn.DataType == SqlDbType.NVarChar.To(DbType.Int32)
                        && CurrentColumn.Length != Column.Length
                        && CurrentColumn.Length.Between(0, 4000)
                        && Column.Length.Between(0, 4000)))
                {
                    Command = string.Format(CultureInfo.CurrentCulture,
                        "EXEC dbo.sp_executesql @statement = N'ALTER TABLE {0} ALTER COLUMN {1} {2}",
                        Table.Name,
                        Column.Name,
                        Column.DataType.To(SqlDbType.Int).ToString());
                    if (Column.DataType == SqlDbType.VarChar.To(DbType.Int32) || Column.DataType == SqlDbType.NVarChar.To(DbType.Int32))
                    {
                        Command += (Column.Length < 0 || Column.Length >= 4000) ?
                            "(MAX)" :
                            "(" + Column.Length.ToString(CultureInfo.InvariantCulture) + ")";
                    }
                    Command += "'";
                    ReturnValue.Add(Command);
                }
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetAlterTriggerCommand(Table Table, ITable CurrentTable)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            List<string> ReturnValue = new List<string>();
            foreach (Trigger Trigger in Table.Triggers)
            {
                foreach (Trigger Trigger2 in CurrentTable.Triggers)
                {
                    if (Trigger.Name == Trigger2.Name && Trigger.Definition != Trigger2.Definition)
                    {
                        ReturnValue.Add(string.Format(CultureInfo.CurrentCulture,
                            "EXEC dbo.sp_executesql @statement = N'DROP TRIGGER {0}'",
                            Trigger.Name));
                        string Definition = Regex.Replace(Trigger.Definition, "-- (.*)", "");
                        ReturnValue.Add(Definition.Replace("\n", " ").Replace("\r", " "));
                        break;
                    }
                }
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetAlterViewCommand(View View, View CurrentView)
        {
            Contract.Requires<ArgumentNullException>(View != null, "View");
            Contract.Requires<ArgumentNullException>(CurrentView != null, "CurrentView");
            List<string> ReturnValue = new List<string>();
            if (View.Definition != CurrentView.Definition)
            {
                ReturnValue.Add(string.Format(CultureInfo.CurrentCulture,
                    "EXEC dbo.sp_executesql @statement = N'DROP VIEW {0}'",
                    View.Name));
                ReturnValue.Add(GetViewCommand(View));
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetForeignKeyCommand(Table Table)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            List<string> ReturnValue = new List<string>();
            foreach (IColumn Column in Table.Columns)
            {
                if (Column.ForeignKey.Count > 0)
                {
                    foreach (IColumn ForeignKey in Column.ForeignKey)
                    {
                        string Command = string.Format(CultureInfo.CurrentCulture,
                            "EXEC dbo.sp_executesql @statement = N'ALTER TABLE {0} ADD FOREIGN KEY ({1}) REFERENCES {2}({3})",
                            Column.ParentTable.Name,
                            Column.Name,
                            ForeignKey.ParentTable.Name,
                            ForeignKey.Name);
                        if (Column.OnDeleteCascade)
                            Command += " ON DELETE CASCADE";
                        if (Column.OnUpdateCascade)
                            Command += " ON UPDATE CASCADE";
                        if (Column.OnDeleteSetNull)
                            Command += " ON DELETE SET NULL";
                        Command += "'";
                        ReturnValue.Add(Command);
                    }
                }
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetFunctionCommand(Function Function)
        {
            Contract.Requires<ArgumentNullException>(Function != null, "Function");
            string Definition = Regex.Replace(Function.Definition, "-- (.*)", "");
            return new string[] { Definition.Replace("\n", " ").Replace("\r", " ") };
        }

        private static IEnumerable<string> GetStoredProcedure(StoredProcedure StoredProcedure)
        {
            Contract.Requires<ArgumentNullException>(StoredProcedure != null, "StoredProcedure");
            string Definition = Regex.Replace(StoredProcedure.Definition, "-- (.*)", "");
            return new string[] { Definition.Replace("\n", " ").Replace("\r", " ") };
        }

        private static IEnumerable<string> GetTableCommand(Table Table)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            List<string> ReturnValue = new List<string>();
            StringBuilder Builder = new StringBuilder();
            Builder.Append("EXEC dbo.sp_executesql @statement = N'CREATE TABLE ").Append(Table.Name).Append("(");
            string Splitter = "";
            foreach (IColumn Column in Table.Columns)
            {
                Builder.Append(Splitter).Append(Column.Name).Append(" ").Append(Column.DataType.To(SqlDbType.Int).ToString());
                if (Column.DataType == SqlDbType.VarChar.To(DbType.Int32) || Column.DataType == SqlDbType.NVarChar.To(DbType.Int32))
                {
                    if (Column.Length < 0 || Column.Length >= 4000)
                    {
                        Builder.Append("(MAX)");
                    }
                    else
                    {
                        Builder.Append("(").Append(Column.Length.ToString(CultureInfo.InvariantCulture)).Append(")");
                    }
                }
                if (!Column.Nullable)
                {
                    Builder.Append(" NOT NULL");
                }
                if (Column.Unique)
                {
                    Builder.Append(" UNIQUE");
                }
                if (Column.PrimaryKey)
                {
                    Builder.Append(" PRIMARY KEY");
                }
                if (!string.IsNullOrEmpty(Column.Default))
                {
                    Builder.Append(" DEFAULT ").Append(Column.Default.Replace("(", "").Replace(")", "").Replace("'", "''"));
                }
                if (Column.AutoIncrement)
                {
                    Builder.Append(" IDENTITY");
                }
                Splitter = ",";
            }
            Builder.Append(")'");
            ReturnValue.Add(Builder.ToString());
            int Counter = 0;
            foreach (IColumn Column in Table.Columns)
            {
                if (Column.Index && Column.Unique)
                {
                    ReturnValue.Add(string.Format(CultureInfo.CurrentCulture,
                        "EXEC dbo.sp_executesql @statement = N'CREATE UNIQUE INDEX Index_{0}{1} ON {2}({3})'",
                        Column.Name,
                        Counter.ToString(CultureInfo.InvariantCulture),
                        Column.ParentTable.Name,
                        Column.Name));
                }
                else if (Column.Index)
                {
                    ReturnValue.Add(string.Format(CultureInfo.CurrentCulture,
                        "EXEC dbo.sp_executesql @statement = N'CREATE INDEX Index_{0}{1} ON {2}({3})'",
                        Column.Name,
                        Counter.ToString(CultureInfo.InvariantCulture),
                        Column.ParentTable.Name,
                        Column.Name));
                }
                ++Counter;
            }
            return ReturnValue;
        }

        private static void GetTables(ISourceInfo Source, Database Temp)
        {
            Contract.Requires<ArgumentNullException>(Source != null, "Source");
            IEnumerable<dynamic> Values = Provider.Batch(Source)
                                                  .AddCommand(null, null, CommandType.Text, "SELECT TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE FROM INFORMATION_SCHEMA.TABLES")
                                                  .Execute()[0];
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

        private static IEnumerable<string> GetTriggerCommand(Table Table)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            List<string> ReturnValue = new List<string>();
            foreach (Trigger Trigger in Table.Triggers)
            {
                string Definition = Regex.Replace(Trigger.Definition, "-- (.*)", "");
                ReturnValue.Add(Definition.Replace("\n", " ").Replace("\r", " "));
            }
            return ReturnValue;
        }

        private static IEnumerable<string> GetViewCommand(View View)
        {
            Contract.Requires<ArgumentNullException>(View != null, "View");
            string Definition = Regex.Replace(View.Definition, "-- (.*)", "");
            return new string[] { Definition.Replace("\n", " ").Replace("\r", " ") };
        }

        private static void SetupColumns(Table Table, IEnumerable<dynamic> Values)
        {
            Contract.Requires<ArgumentNullException>(Values != null, "Values");
            foreach (dynamic Item in Values)
            {
                if (Table.ContainsColumn(Item.Column))
                {
                    Table.AddForeignKey(Item.Column, Item.FOREIGN_KEY_TABLE, Item.FOREIGN_KEY_COLUMN);
                }
                else
                {
                    Table.AddColumn<string>(Item.Column,
                        Utilities.DataTypes.TypeConversionExtensions.To(Utilities.DataTypes.TypeConversionExtensions.To<string, SqlDbType>(Item.COLUMN_TYPE), DbType.Int32),
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

        private static void SetupFunctions(ISourceInfo Source, Database Temp)
        {
            Contract.Requires<ArgumentNullException>(Source != null, "Source");
            IEnumerable<dynamic> Values = Provider.Batch(Source)
                                                      .AddCommand(null, null, CommandType.Text,
                                                            "SELECT SPECIFIC_NAME as NAME,ROUTINE_DEFINITION as DEFINITION FROM INFORMATION_SCHEMA.ROUTINES WHERE INFORMATION_SCHEMA.ROUTINES.ROUTINE_TYPE='FUNCTION'")
                                                      .Execute()[0];
            foreach (dynamic Item in Values)
            {
                Temp.AddFunction(Item.NAME, Item.DEFINITION);
            }
        }

        private static void SetupStoredProcedures(ISourceInfo Source, Database Temp)
        {
            Contract.Requires<ArgumentNullException>(Source != null, "Source");
            IEnumerable<dynamic> Values = Provider.Batch(Source)
                                                      .AddCommand(null, null, CommandType.Text,
                                                            "SELECT sys.procedures.name as NAME,OBJECT_DEFINITION(sys.procedures.object_id) as DEFINITION FROM sys.procedures")
                                                      .Execute()[0];
            foreach (dynamic Item in Values)
            {
                Temp.AddStoredProcedure(Item.NAME, Item.DEFINITION);
            }
            foreach (StoredProcedure Procedure in Temp.StoredProcedures)
            {
                Values = Provider.Batch(Source)
                                .AddCommand(null, null, @"SELECT sys.systypes.name as TYPE,sys.parameters.name as NAME,sys.parameters.max_length as LENGTH,sys.parameters.default_value as [DEFAULT VALUE] FROM sys.procedures INNER JOIN sys.parameters on sys.procedures.object_id=sys.parameters.object_id INNER JOIN sys.systypes on sys.systypes.xusertype=sys.parameters.system_type_id WHERE sys.procedures.name=@0 AND (sys.systypes.xusertype <> 256)",
                                        CommandType.Text,
                                        Procedure.Name)
                                .Execute()[0];
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

        private static void SetupTables(ISourceInfo Source, Database Temp)
        {
            Contract.Requires<ArgumentNullException>(Temp != null, "Temp");
            foreach (Table Table in Temp.Tables)
            {
                IEnumerable<dynamic> Values = Provider.Batch(Source)
                                                      .AddCommand(null, null, @"SELECT sys.columns.name AS [Column], sys.systypes.name AS [COLUMN_TYPE],
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
                                                      .Execute()[0];
                SetupColumns(Table, Values);
                SetupTriggers(Source, Table, Values);
            }
            foreach (Table Table in Temp.Tables)
            {
                Table.SetupForeignKeys();
            }
        }

        private static void SetupTriggers(ISourceInfo Source, Table Table, IEnumerable<dynamic> Values)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            Contract.Requires<ArgumentNullException>(Source != null, "Source");
            Values = Provider.Batch(Source)
                             .AddCommand(null, null, @"SELECT sys.triggers.name as Name,sys.trigger_events.type as Type,
                                                OBJECT_DEFINITION(sys.triggers.object_id) as Definition 
                                                FROM sys.triggers 
                                                INNER JOIN sys.trigger_events ON sys.triggers.object_id=sys.trigger_events.object_id 
                                                INNER JOIN sys.tables on sys.triggers.parent_id=sys.tables.object_id 
                                                where sys.tables.name=@0",
                                    CommandType.Text,
                                    Table.Name)
                             .Execute()[0];
            foreach (dynamic Item in Values)
            {
                string Name = Item.Name;
                int Type = Item.Type;
                string Definition = Item.Definition;
                Table.AddTrigger(Name, Definition, Type.ToString(CultureInfo.InvariantCulture).To<string, TriggerType>());
            }
        }

        private static void SetupViews(ISourceInfo Source, Database Temp)
        {
            Contract.Requires<ArgumentNullException>(Temp != null, "Temp");
            foreach (View View in Temp.Views)
            {
                IEnumerable<dynamic> Values = Provider.Batch(Source)
                                                      .AddCommand(null, null, @"SELECT OBJECT_DEFINITION(sys.views.object_id) as Definition FROM sys.views WHERE sys.views.name=@0",
                                                                CommandType.Text,
                                                                View.Name)
                                                      .Execute()[0];
                View.Definition = Values.First().Definition;
                Values = Provider.Batch(Source)
                                 .AddCommand(null, null, @"SELECT sys.columns.name AS [Column], sys.systypes.name AS [COLUMN_TYPE],
                                                        sys.columns.max_length as [MAX_LENGTH], sys.columns.is_nullable as [IS_NULLABLE] 
                                                        FROM sys.views 
                                                        INNER JOIN sys.columns on sys.columns.object_id=sys.views.object_id 
                                                        INNER JOIN sys.systypes ON sys.systypes.xtype = sys.columns.system_type_id 
                                                        WHERE (sys.views.name = @0) AND (sys.systypes.xusertype <> 256)",
                                        CommandType.Text,
                                        View.Name)
                                 .Execute()[0];
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