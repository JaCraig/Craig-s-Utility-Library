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
using System.Text;
using System.Text.RegularExpressions;
using Utilities.DataTypes;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.Schema.Default.Database.SQLServer.Builders;
using Utilities.ORM.Manager.Schema.Default.Database.SQLServer.Builders.Interfaces;
using Utilities.ORM.Manager.Schema.Enums;
using Utilities.ORM.Manager.Schema.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

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
        /// <param name="Provider">The provider.</param>
        /// <param name="SourceProvider">The source provider.</param>
        public SQLServerSchemaGenerator(QueryProvider.Manager Provider, SourceProvider.Manager SourceProvider)
        {
            this.Provider = Provider;
            this.SourceProvider = SourceProvider;
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
        /// Source provider object
        /// </summary>
        protected SourceProvider.Manager SourceProvider { get; private set; }

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
            ISourceInfo DatabaseSource = SourceProvider.GetSource(Regex.Replace(Source.Connection, "Initial Catalog=(.*?;)", ""));
            if (!SourceExists(DatabaseName, DatabaseSource))
                return null;
            Database Temp = new Database(DatabaseName);
            IBatch Batch = Provider.Batch(Source);
            IBuilder[] Builders = new IBuilder[]{
                new Tables(),
                new TableColumns(),
                new TableTriggers(),
                new TableForeignKeys()
            };
            Builders.ForEach(x => x.GetCommand(Batch));
            var Results = Batch.Execute();
            Builders.For(0, Builders.Length - 1, (x, y) => y.FillDatabase(Results[x], Temp));
            SetupViews(Source, Temp);
            SetupStoredProcedures(Source, Temp);
            SetupFunctions(Source, Temp);
            return Temp;
        }

        /// <summary>
        /// Sets up the specified database schema
        /// </summary>
        /// <param name="Mappings">The mappings.</param>
        /// <param name="Database">The database.</param>
        /// <param name="QueryProvider">The query provider.</param>
        public void Setup(ListMapping<IDatabase, IMapping> Mappings, IDatabase Database, QueryProvider.Manager QueryProvider)
        {
            ISourceInfo TempSource = SourceProvider.GetSource(Database.Name);
            Utilities.ORM.Manager.Schema.Default.Database.Database TempDatabase = new Schema.Default.Database.Database(Regex.Match(TempSource.Connection, "Initial Catalog=(.*?;)").Value.Replace("Initial Catalog=", "").Replace(";", ""));
            SetupTables(Mappings, Database, TempDatabase);
            SetupJoiningTables(Mappings, Database, TempDatabase);
            SetupAuditTables(Database, TempDatabase);

            foreach (ITable Table in TempDatabase.Tables)
            {
                Table.SetupForeignKeys();
            }
            List<string> Commands = GenerateSchema(TempDatabase, SourceProvider.GetSource(Database.Name)).ToList();
            IBatch Batch = QueryProvider.Batch(SourceProvider.GetSource(Database.Name));
            for (int x = 0; x < Commands.Count; ++x)
            {
                if (Commands[x].ToUpperInvariant().Contains("CREATE DATABASE"))
                {
                    QueryProvider.Batch(SourceProvider.GetSource(Regex.Replace(SourceProvider.GetSource(Database.Name).Connection, "Initial Catalog=(.*?;)", ""))).AddCommand(null, null, CommandType.Text, Commands[x]).Execute();
                }
                else if (Commands[x].Contains("CREATE TRIGGER") || Commands[x].Contains("CREATE FUNCTION"))
                {
                    if (Batch.CommandCount > 0)
                    {
                        Batch.Execute();
                        Batch = QueryProvider.Batch(SourceProvider.GetSource(Database.Name));
                    }
                    Batch.AddCommand(null, null, CommandType.Text, Commands[x]);
                    if (x < Commands.Count - 1)
                    {
                        Batch.Execute();
                        Batch = QueryProvider.Batch(SourceProvider.GetSource(Database.Name));
                    }
                }
                else
                {
                    Batch.AddCommand(null, null, CommandType.Text, Commands[x]);
                }
            }
            Batch.Execute();
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
                ITable CurrentTable = CurrentStructure[Table.Name];
                Commands.Add((CurrentTable == null) ? GetForeignKeyCommand(Table) : GetForeignKeyCommand(Table, CurrentTable));
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

        private static IEnumerable<string> GetAlterFunctionCommand(Function Function, Function CurrentFunction)
        {
            Contract.Requires<ArgumentNullException>(Function != null, "Function");
            Contract.Requires<ArgumentNullException>(CurrentFunction != null, "CurrentFunction");
            Contract.Requires<ArgumentException>(Function.Definition == CurrentFunction.Definition || !string.IsNullOrEmpty(Function.Definition));
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
            Contract.Requires<ArgumentException>(StoredProcedure.Definition == CurrentStoredProcedure.Definition || !string.IsNullOrEmpty(StoredProcedure.Definition));
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
            Contract.Requires<ArgumentNullException>(Table.Columns != null, "Table.Columns");
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
                    if (Column.DataType == SqlDbType.VarChar.To(DbType.Int32)
                        || Column.DataType == SqlDbType.NVarChar.To(DbType.Int32)
                        || Column.DataType == SqlDbType.Binary.To(DbType.Int32))
                    {
                        Command += (Column.Length < 0 || Column.Length >= 4000) ?
                                        "(MAX)" :
                                        "(" + Column.Length.ToString(CultureInfo.InvariantCulture) + ")";
                    }
                    else if (Column.DataType == SqlDbType.Decimal.To(DbType.Int32))
                    {
                        int Precision = (Column.Length * 2).Clamp(38, 18);
                        Command += "(" + Precision.ToString(CultureInfo.InvariantCulture) + "," + Column.Length.Clamp(38, 0).ToString(CultureInfo.InvariantCulture) + ")";
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
                    if (Column.DataType == SqlDbType.VarChar.To(DbType.Int32)
                        || Column.DataType == SqlDbType.NVarChar.To(DbType.Int32)
                        || Column.DataType == SqlDbType.Binary.To(DbType.Int32))
                    {
                        Command += (Column.Length < 0 || Column.Length >= 4000) ?
                                        "(MAX)" :
                                        "(" + Column.Length.ToString(CultureInfo.InvariantCulture) + ")";
                    }
                    else if (Column.DataType == SqlDbType.Decimal.To(DbType.Int32))
                    {
                        int Precision = (Column.Length * 2).Clamp(38, 18);
                        Command += "(" + Precision.ToString(CultureInfo.InvariantCulture) + "," + Column.Length.Clamp(38, 0).ToString(CultureInfo.InvariantCulture) + ")";
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
            Contract.Requires<ArgumentNullException>(Table.Triggers != null, "Table.Triggers");
            List<string> ReturnValue = new List<string>();
            foreach (Trigger Trigger in Table.Triggers)
            {
                foreach (Trigger Trigger2 in CurrentTable.Triggers)
                {
                    string Definition1 = Trigger.Definition;
                    string Definition2 = Trigger2.Definition.Replace("Command0", "");
                    if (Trigger.Name == Trigger2.Name && string.Equals(Definition1, Definition2, StringComparison.InvariantCultureIgnoreCase))
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
            Contract.Requires<ArgumentException>(View.Definition == CurrentView.Definition || !string.IsNullOrEmpty(View.Definition));
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

        private static IEnumerable<string> GetForeignKeyCommand(Table Table, ITable CurrentTable)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            Contract.Requires<ArgumentNullException>(Table.Columns != null, "Table.Columns");
            List<string> ReturnValue = new List<string>();
            foreach (IColumn Column in Table.Columns)
            {
                IColumn CurrentColumn = CurrentTable[Column.Name];
                if (Column.ForeignKey.Count > 0
                    && (CurrentColumn == null || CurrentColumn.ForeignKey.Count != Column.ForeignKey.Count))
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

        private static IEnumerable<string> GetForeignKeyCommand(Table Table)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            Contract.Requires<ArgumentNullException>(Table.Columns != null, "Table.Columns");
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
            Contract.Requires<ArgumentNullException>(Function.Definition != null, "Function.Definition");
            string Definition = Regex.Replace(Function.Definition, "-- (.*)", "");
            return new string[] { Definition.Replace("\n", " ").Replace("\r", " ") };
        }

        private static IEnumerable<string> GetStoredProcedure(StoredProcedure StoredProcedure)
        {
            Contract.Requires<ArgumentNullException>(StoredProcedure != null, "StoredProcedure");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(StoredProcedure.Definition), "StoredProcedure.Definition");
            string Definition = Regex.Replace(StoredProcedure.Definition, "-- (.*)", "");
            return new string[] { Definition.Replace("\n", " ").Replace("\r", " ") };
        }

        private static IEnumerable<string> GetTableCommand(Table Table)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            Contract.Requires<ArgumentNullException>(Table.Columns != null, "Table.Columns");
            List<string> ReturnValue = new List<string>();
            StringBuilder Builder = new StringBuilder();
            Builder.Append("EXEC dbo.sp_executesql @statement = N'CREATE TABLE ").Append(Table.Name).Append("(");
            string Splitter = "";
            foreach (IColumn Column in Table.Columns)
            {
                Builder.Append(Splitter).Append(Column.Name).Append(" ").Append(Column.DataType.To(SqlDbType.Int).ToString());
                if (Column.DataType == SqlDbType.VarChar.To(DbType.Int32)
                        || Column.DataType == SqlDbType.NVarChar.To(DbType.Int32)
                        || Column.DataType == SqlDbType.Binary.To(DbType.Int32))
                {
                    Builder.Append((Column.Length < 0 || Column.Length >= 4000) ?
                                    "(MAX)" :
                                    "(" + Column.Length.ToString(CultureInfo.InvariantCulture) + ")");
                }
                else if (Column.DataType == SqlDbType.Decimal.To(DbType.Int32))
                {
                    int Precision = (Column.Length * 2).Clamp(38, 18);
                    Builder.Append("(").Append(Precision).Append(",").Append(Column.Length.Clamp(38, 0)).Append(")");
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

        private static IEnumerable<string> GetTriggerCommand(Table Table)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            Contract.Requires<ArgumentNullException>(Table.Triggers != null, "Table.Triggers");
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
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(View.Definition), "View.Definition");
            string Definition = Regex.Replace(View.Definition, "-- (.*)", "");
            return new string[] { Definition.Replace("\n", " ").Replace("\r", " ") };
        }

        private static ITable SetupAuditTables(ITable Table)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            ITable AuditTable = new Schema.Default.Database.Table(Table.Name + "Audit", Table.Source);
            string IDName = Table.Columns.Any(x => string.Equals(x.Name, "ID", StringComparison.InvariantCultureIgnoreCase)) ? "AuditID" : "ID";
            AuditTable.AddColumn(IDName, DbType.Int32, 0, false, true, true, true, false, "", "", 0);
            AuditTable.AddColumn("AuditType", SqlDbType.NVarChar.To(DbType.Int32), 1, false, false, false, false, false, "", "", "");
            foreach (IColumn Column in Table.Columns)
                AuditTable.AddColumn(Column.Name, Column.DataType, Column.Length, Column.Nullable, false, false, false, false, "", "", "");
            return AuditTable;
        }

        private static void SetupAuditTables(IDatabase Key, Schema.Default.Database.Database TempDatabase)
        {
            Contract.Requires<ArgumentNullException>(Key != null, "Key");
            Contract.Requires<ArgumentNullException>(TempDatabase != null, "TempDatabase");
            Contract.Requires<ArgumentNullException>(TempDatabase.Tables != null, "TempDatabase.Tables");
            if (!Key.Audit)
                return;
            List<ITable> TempTables = new List<ITable>();
            foreach (ITable Table in TempDatabase.Tables)
            {
                TempTables.Add(SetupAuditTables(Table));
                SetupInsertUpdateTrigger(Table);
                SetupDeleteTrigger(Table);
            }
            TempDatabase.Tables.Add(TempTables);
        }

        private static void SetupDeleteTrigger(ITable Table)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            Contract.Requires<ArgumentNullException>(Table.Columns != null, "Table.Columns");
            StringBuilder Columns = new StringBuilder();
            StringBuilder Builder = new StringBuilder();
            Builder.Append("CREATE TRIGGER dbo.").Append(Table.Name).Append("_Audit_D ON dbo.")
                .Append(Table.Name).Append(" FOR DELETE AS IF @@rowcount=0 RETURN")
                .Append(" INSERT INTO dbo.").Append(Table.Name).Append("Audit").Append("(");
            string Splitter = "";
            foreach (IColumn Column in Table.Columns)
            {
                Columns.Append(Splitter).Append(Column.Name);
                Splitter = ",";
            }
            Builder.Append(Columns.ToString());
            Builder.Append(",AuditType) SELECT ");
            Builder.Append(Columns.ToString());
            Builder.Append(",'D' FROM deleted");
            Table.AddTrigger(Table.Name + "_Audit_D", Builder.ToString(), TriggerType.Delete);
        }

        private static void SetupInsertUpdateTrigger(ITable Table)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            Contract.Requires<ArgumentNullException>(Table.Columns != null, "Table.Columns");
            StringBuilder Columns = new StringBuilder();
            StringBuilder Builder = new StringBuilder();
            Builder.Append("CREATE TRIGGER dbo.").Append(Table.Name).Append("_Audit_IU ON dbo.")
                .Append(Table.Name).Append(" FOR INSERT,UPDATE AS IF @@rowcount=0 RETURN declare @AuditType")
                .Append(" char(1) declare @DeletedCount int SELECT @DeletedCount=count(*) FROM DELETED IF @DeletedCount=0")
                .Append(" BEGIN SET @AuditType='I' END ELSE BEGIN SET @AuditType='U' END")
                .Append(" INSERT INTO dbo.").Append(Table.Name).Append("Audit").Append("(");
            string Splitter = "";
            foreach (IColumn Column in Table.Columns)
            {
                Columns.Append(Splitter).Append(Column.Name);
                Splitter = ",";
            }
            Builder.Append(Columns.ToString());
            Builder.Append(",AuditType) SELECT ");
            Builder.Append(Columns.ToString());
            Builder.Append(",@AuditType FROM inserted");
            Table.AddTrigger(Table.Name + "_Audit_IU", Builder.ToString(), TriggerType.Insert);
        }

        private static void SetupJoiningTables(ListMapping<IDatabase, IMapping> Mappings, IDatabase Key, Schema.Default.Database.Database TempDatabase)
        {
            Contract.Requires<NullReferenceException>(Mappings != null, "Mappings");
            foreach (IMapping Mapping in Mappings[Key])
            {
                foreach (IProperty Property in Mapping.Properties)
                {
                    if (Property is IMap)
                    {
                        IMapping MapMapping = Mappings[Key].FirstOrDefault(x => x.ObjectType == Property.Type);
                        foreach (IProperty IDProperty in MapMapping.IDProperties)
                        {
                            TempDatabase[Mapping.TableName].AddColumn(Property.FieldName,
                                IDProperty.Type.To(DbType.Int32),
                                IDProperty.MaxLength,
                                !Property.NotNull,
                                false,
                                Property.Index,
                                false,
                                false,
                                MapMapping.TableName,
                                IDProperty.FieldName,
                                "",
                                false,
                                false,
                                Mapping.Properties.Count(x => x.Type == Property.Type) == 1 && Mapping.ObjectType != Property.Type);
                        }
                    }
                    else if (Property is IManyToOne || Property is IManyToMany || Property is IIEnumerableManyToOne || Property is IListManyToMany || Property is IListManyToOne)
                    {
                        SetupJoiningTablesEnumerable(Mappings, Mapping, Property, Key, TempDatabase);
                    }
                }
            }
        }

        private static void SetupJoiningTablesEnumerable(ListMapping<IDatabase, IMapping> Mappings, IMapping Mapping, IProperty Property, IDatabase Key, Schema.Default.Database.Database TempDatabase)
        {
            Contract.Requires<ArgumentNullException>(TempDatabase != null, "TempDatabase");
            Contract.Requires<ArgumentNullException>(TempDatabase.Tables != null, "TempDatabase.Tables");
            if (TempDatabase.Tables.FirstOrDefault(x => x.Name == Property.TableName) != null)
                return;
            IMapping MapMapping = Mappings[Key].FirstOrDefault(x => x.ObjectType == Property.Type);
            if (MapMapping == null)
                return;
            if (MapMapping == Mapping)
            {
                TempDatabase.AddTable(Property.TableName);
                TempDatabase[Property.TableName].AddColumn("ID_", DbType.Int32, 0, false, true, true, true, false, "", "", "");
                TempDatabase[Property.TableName].AddColumn(Mapping.TableName + Mapping.IDProperties.First().FieldName,
                    Mapping.IDProperties.First().Type.To(DbType.Int32),
                    Mapping.IDProperties.First().MaxLength,
                    false,
                    false,
                    false,
                    false,
                    false,
                    Mapping.TableName,
                    Mapping.IDProperties.First().FieldName,
                    "",
                    false,
                    false,
                    false);
                TempDatabase[Property.TableName].AddColumn(MapMapping.TableName + MapMapping.IDProperties.First().FieldName + "2",
                    MapMapping.IDProperties.First().Type.To(DbType.Int32),
                    MapMapping.IDProperties.First().MaxLength,
                    false,
                    false,
                    false,
                    false,
                    false,
                    MapMapping.TableName,
                    MapMapping.IDProperties.First().FieldName,
                    "",
                    false,
                    false,
                    false);
            }
            else
            {
                TempDatabase.AddTable(Property.TableName);
                TempDatabase[Property.TableName].AddColumn("ID_", DbType.Int32, 0, false, true, true, true, false, "", "", "");
                TempDatabase[Property.TableName].AddColumn(Mapping.TableName + Mapping.IDProperties.First().FieldName,
                    Mapping.IDProperties.First().Type.To(DbType.Int32),
                    Mapping.IDProperties.First().MaxLength,
                    false,
                    false,
                    false,
                    false,
                    false,
                    Mapping.TableName,
                    Mapping.IDProperties.First().FieldName,
                    "",
                    true,
                    false,
                    false);
                TempDatabase[Property.TableName].AddColumn(MapMapping.TableName + MapMapping.IDProperties.First().FieldName,
                    MapMapping.IDProperties.First().Type.To(DbType.Int32),
                    MapMapping.IDProperties.First().MaxLength,
                    false,
                    false,
                    false,
                    false,
                    false,
                    MapMapping.TableName,
                    MapMapping.IDProperties.First().FieldName,
                    "",
                    true,
                    false,
                    false);
            }
        }

        private static void SetupProperties(ITable Table, IMapping Mapping)
        {
            Contract.Requires<ArgumentNullException>(Mapping != null, "Mapping");
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            Contract.Requires<ArgumentNullException>(Mapping.IDProperties != null, "Mapping.IDProperties");
            foreach (IProperty Property in Mapping.IDProperties)
            {
                Table.AddColumn(Property.FieldName,
                    Property.Type.To(DbType.Int32),
                    Property.MaxLength,
                    Property.NotNull,
                    Property.AutoIncrement,
                    Property.Index,
                    true,
                    Property.Unique,
                    "",
                    "",
                    "");
            }
            foreach (IProperty Property in Mapping.Properties)
            {
                if (!(Property is IManyToMany || Property is IManyToOne || Property is IMap || Property is IIEnumerableManyToOne || Property is IListManyToMany || Property is IListManyToOne))
                {
                    Table.AddColumn(Property.FieldName,
                    Property.Type.To(DbType.Int32),
                    Property.MaxLength,
                    !Property.NotNull,
                    Property.AutoIncrement,
                    Property.Index,
                    false,
                    Property.Unique,
                    "",
                    "",
                    "");
                }
            }
        }

        private static void SetupTables(ListMapping<IDatabase, IMapping> Mappings, IDatabase Key, Schema.Default.Database.Database TempDatabase)
        {
            Contract.Requires<NullReferenceException>(Mappings != null, "Mappings");
            foreach (IMapping Mapping in Mappings[Key])
            {
                TempDatabase.AddTable(Mapping.TableName);
                SetupProperties(TempDatabase[Mapping.TableName], Mapping);
            }
        }

        private bool Exists(string Command, string Value, ISourceInfo Source)
        {
            Contract.Requires<ArgumentNullException>(Source != null, "Source");
            Contract.Requires<NullReferenceException>(Provider != null, "Provider");
            return Provider.Batch(Source)
                           .AddCommand(null, null, Command, CommandType.Text, Value)
                           .Execute()[0]
                           .Count() > 0;
        }

        private void SetupFunctions(ISourceInfo Source, Database Temp)
        {
            Contract.Requires<ArgumentNullException>(Source != null, "Source");
            Contract.Requires<NullReferenceException>(Provider != null, "Provider");
            IEnumerable<dynamic> Values = Provider.Batch(Source)
                                                      .AddCommand(null, null, CommandType.Text,
                                                            "SELECT SPECIFIC_NAME as NAME,ROUTINE_DEFINITION as DEFINITION FROM INFORMATION_SCHEMA.ROUTINES WHERE INFORMATION_SCHEMA.ROUTINES.ROUTINE_TYPE='FUNCTION'")
                                                      .Execute()[0];
            foreach (dynamic Item in Values)
            {
                Temp.AddFunction(Item.NAME, Item.DEFINITION);
            }
        }

        private void SetupStoredProcedures(ISourceInfo Source, Database Temp)
        {
            Contract.Requires<ArgumentNullException>(Source != null, "Source");
            Contract.Requires<NullReferenceException>(Provider != null, "Provider");
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

        private void SetupViews(ISourceInfo Source, Database Temp)
        {
            Contract.Requires<ArgumentNullException>(Temp != null, "Temp");
            Contract.Requires<ArgumentNullException>(Temp.Views != null, "Temp.Views");
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