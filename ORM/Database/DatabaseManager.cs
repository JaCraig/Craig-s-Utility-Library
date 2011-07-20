/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Linq;
using System.Text;
using Utilities.ORM.QueryProviders.Interfaces;
using Utilities.ORM.Mapping.Interfaces;
using Utilities.DataTypes;
using System.Data;
using System.Text.RegularExpressions;
using Utilities.SQL.DataClasses.Enums;
#endregion

namespace Utilities.ORM.Database
{
    /// <summary>
    /// Database manager (handles generation and mapping of stored procedures)
    /// </summary>
    public class DatabaseManager
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Mappings">Mappings associated with databases (provided by query provider)</param>
        public DatabaseManager(ListMapping<IDatabase, IMapping> Mappings)
        {
            this.Mappings = Mappings;
            this.DatabaseStructures = new System.Collections.Generic.List<SQL.DataClasses.Database>();
        }

        #endregion

        #region Functions

        #region Setup

        /// <summary>
        /// Sets up the databases
        /// </summary>
        public virtual void Setup()
        {
            foreach (IDatabase Key in Mappings.Keys)
            {
                if (Key.Update)
                {
                    string DatabaseName = Regex.Match(Key.ConnectionString, "Initial Catalog=(.*?;)").Value.Replace("Initial Catalog=", "").Replace(";", "");
                    Utilities.SQL.DataClasses.Database TempDatabase = new SQL.DataClasses.Database(DatabaseName);
                    SetupFunctions(TempDatabase);
                    SetupTables(Key, TempDatabase);
                    SetupJoiningTables(Key, TempDatabase);
                    SetupAuditTables(Key, TempDatabase);

                    DatabaseStructures.Add(TempDatabase);
                    foreach (Utilities.SQL.DataClasses.Table Table in TempDatabase.Tables)
                    {
                        Table.SetupForeignKeys();
                    }
                    Utilities.SQL.DataClasses.Database CurrentDatabase = SQL.SQLServer.SQLServer.GetDatabaseStructure(Key.ConnectionString);
                    SQL.SQLServer.SQLServer.UpdateDatabase(TempDatabase, CurrentDatabase, Key.ConnectionString);

                    foreach (IMapping Mapping in Mappings[Key])
                    {
                        foreach (IProperty Property in Mapping.Properties)
                        {
                            Property.SetupLoadCommands();
                        }
                    }
                }
            }
        }

        #endregion

        #region SetupAuditTables

        private void SetupAuditTables(IDatabase Key, SQL.DataClasses.Database TempDatabase)
        {
            System.Collections.Generic.List<Utilities.SQL.DataClasses.Table> TempTables = new System.Collections.Generic.List<Utilities.SQL.DataClasses.Table>();
            foreach (Utilities.SQL.DataClasses.Table Table in TempDatabase.Tables)
            {
                TempTables.Add(SetupAuditTables(Table));
                SetupInsertUpdateTrigger(Table);
                SetupDeleteTrigger(Table);
            }
            TempDatabase.Tables.AddRange(TempTables);
        }

        private SQL.DataClasses.Table SetupAuditTables(SQL.DataClasses.Table Table)
        {
            SQL.DataClasses.Table AuditTable = new Utilities.SQL.DataClasses.Table(Table.Name + "Audit", Table.ParentDatabase);
            AuditTable.AddColumn("ID", SqlDbType.Int.ToString(), 0, false, true, true, true, false, "", "", "");
            AuditTable.AddColumn("AuditType", SqlDbType.NVarChar.ToString(), 1, false, false, false, false, false, "", "", "");
            foreach (SQL.DataClasses.Column Column in Table.Columns)
                AuditTable.AddColumn(Column.Name, Column.DataType.ToString(), Column.Length, Column.Nullable, false, false, false, false, "", "", "");
            return AuditTable;
        }

        #endregion

        #region SetupDeleteTrigger

        private void SetupDeleteTrigger(SQL.DataClasses.Table Table)
        {
            StringBuilder Columns = new StringBuilder();
            StringBuilder Builder = new StringBuilder();
            Builder.Append("CREATE TRIGGER dbo.").Append(Table.Name).Append("_Audit_D ON dbo.")
                .Append(Table.Name).Append(" FOR DELETE AS IF @@rowcount=0 RETURN")
                .Append(" INSERT INTO dbo.").Append(Table.Name).Append("Audit").Append("(");
            string Splitter = "";
            foreach (Utilities.SQL.DataClasses.Column Column in Table.Columns)
            {
                Columns.Append(Splitter).Append(Column.Name);
                Splitter = ",";
            }
            Builder.Append(Columns.ToString());
            Builder.Append(",AuditType) SELECT ");
            Builder.Append(Columns.ToString());
            Builder.Append(",'D' FROM deleted");
            Table.AddTrigger(Table.Name + "_Audit_D", Builder.ToString(), (int)TriggerType.INSERT);
        }

        #endregion

        #region SetupInsertUpdateTrigger

        private void SetupInsertUpdateTrigger(SQL.DataClasses.Table Table)
        {
            StringBuilder Columns = new StringBuilder();
            StringBuilder Builder = new StringBuilder();
            Builder.Append("CREATE TRIGGER dbo.").Append(Table.Name).Append("_Audit_IU ON dbo.")
                .Append(Table.Name).Append(" FOR INSERT,UPDATE AS IF @@rowcount=0 RETURN declare @AuditType")
                .Append(" char(1) declare @DeletedCount int SELECT @DeletedCount=count(*) FROM DELETED IF @DeletedCount=0")
                .Append(" BEGIN SET @AuditType='I' END ELSE BEGIN SET @AuditType='U' END")
                .Append(" INSERT INTO dbo.").Append(Table.Name).Append("Audit").Append("(");
            string Splitter = "";
            foreach (Utilities.SQL.DataClasses.Column Column in Table.Columns)
            {
                Columns.Append(Splitter).Append(Column.Name);
                Splitter = ",";
            }
            Builder.Append(Columns.ToString());
            Builder.Append(",AuditType) SELECT ");
            Builder.Append(Columns.ToString());
            Builder.Append(",@AuditType FROM inserted");
            Table.AddTrigger(Table.Name + "_Audit_IU", Builder.ToString(), (int)TriggerType.INSERT);
        }

        #endregion

        #region SetupJoiningTables

        private void SetupJoiningTables(IDatabase Key, SQL.DataClasses.Database TempDatabase)
        {
            foreach (IMapping Mapping in Mappings[Key])
            {
                foreach (IProperty Property in Mapping.Properties)
                {
                    if (Property is IMap)
                    {
                        IMapping MapMapping = Mappings[Key].First(x => x.ObjectType == Property.Type);
                        TempDatabase[Mapping.TableName].AddColumn(Property.FieldName,
                            DataTypeConversion.NetTypeToSQLDbType(MapMapping.IDProperty.Type).ToString(),
                            MapMapping.IDProperty.MaxLength,
                            !Property.NotNull,
                            false,
                            Property.Index,
                            false,
                            false,
                            MapMapping.TableName,
                            MapMapping.IDProperty.FieldName,
                            "");
                    }
                    else if (Property is IManyToMany || Property is IIEnumerableManyToOne)
                    {
                        SetupJoiningTablesEnumerable(Mapping, Property, Key, TempDatabase);
                    }
                    else if (Property is IManyToOne)
                    {
                        SetupJoiningTablesClass(Mapping, Property, Key, TempDatabase);
                    }
                }
            }
        }

        #endregion

        #region SetupJoiningTablesEnumerable

        private void SetupJoiningTablesEnumerable(IMapping Mapping, IProperty Property, IDatabase Key, SQL.DataClasses.Database TempDatabase)
        {
            if (TempDatabase.Tables.FirstOrDefault(x => x.Name == Property.TableName) != null)
                return;
            IMapping MapMapping = Mappings[Key].First(x => x.ObjectType == Property.Type);
            TempDatabase.AddTable(Property.TableName);
            TempDatabase[Property.TableName].AddColumn("ID_", SqlDbType.Int.ToString(), 0, false, true, true, true, false, "", "", "");
            TempDatabase[Property.TableName].AddColumn(Mapping.TableName + Mapping.IDProperty.FieldName,
                DataTypeConversion.NetTypeToSQLDbType(Mapping.IDProperty.Type).ToString(),
                Mapping.IDProperty.MaxLength,
                false,
                false,
                false,
                false,
                false,
                Mapping.TableName,
                Mapping.IDProperty.FieldName,
                "");
            TempDatabase[Property.TableName].AddColumn(MapMapping.TableName + MapMapping.IDProperty.FieldName,
                DataTypeConversion.NetTypeToSQLDbType(MapMapping.IDProperty.Type).ToString(),
                MapMapping.IDProperty.MaxLength,
                false,
                false,
                false,
                false,
                false,
                MapMapping.TableName,
                MapMapping.IDProperty.FieldName,
                "");
        }

        #endregion

        #region SetupJoiningTablesClass

        private void SetupJoiningTablesClass(IMapping Mapping, IProperty Property, IDatabase Key, SQL.DataClasses.Database TempDatabase)
        {
            if (TempDatabase.Tables.FirstOrDefault(x => x.Name == Property.TableName) != null)
                return;
            IMapping MapMapping = Mappings[Key].First(x => x.ObjectType == Property.Type);
            TempDatabase.AddTable(Property.TableName);
            TempDatabase[Property.TableName].AddColumn("ID_", SqlDbType.Int.ToString(), 0, false, true, true, true, false, "", "", "");
            TempDatabase[Property.TableName].AddColumn(Mapping.TableName + Mapping.IDProperty.FieldName,
                DataTypeConversion.NetTypeToSQLDbType(Mapping.IDProperty.Type).ToString(),
                Mapping.IDProperty.MaxLength,
                false,
                false,
                false,
                false,
                false,
                Mapping.TableName,
                Mapping.IDProperty.FieldName,
                "");
            TempDatabase[Property.TableName].AddColumn(MapMapping.TableName + MapMapping.IDProperty.FieldName,
                DataTypeConversion.NetTypeToSQLDbType(MapMapping.IDProperty.Type).ToString(),
                MapMapping.IDProperty.MaxLength,
                false,
                false,
                false,
                false,
                false,
                MapMapping.TableName,
                MapMapping.IDProperty.FieldName,
                "");
        }

        #endregion

        #region SetupTables

        private void SetupTables(IDatabase Key, SQL.DataClasses.Database TempDatabase)
        {
            foreach (IMapping Mapping in Mappings[Key])
            {
                TempDatabase.AddTable(Mapping.TableName);
                SetupProperties(TempDatabase[Mapping.TableName], Mapping);
            }
        }

        #endregion

        #region SetupProperties

        private void SetupProperties(SQL.DataClasses.Table Table, IMapping Mapping)
        {
            Table.AddColumn(Mapping.IDProperty.FieldName,
                DataTypeConversion.NetTypeToSQLDbType(Mapping.IDProperty.Type).ToString(),
                Mapping.IDProperty.MaxLength,
                Mapping.IDProperty.NotNull,
                Mapping.IDProperty.AutoIncrement,
                Mapping.IDProperty.Index,
                true,
                Mapping.IDProperty.Unique,
                "",
                "",
                "");
            foreach (IProperty Property in Mapping.Properties)
            {
                if (!(Property is IManyToMany || Property is IManyToOne || Property is IMap || Property is IIEnumerableManyToOne))
                {
                    Table.AddColumn(Property.FieldName,
                    DataTypeConversion.NetTypeToSQLDbType(Property.Type).ToString(),
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

        #endregion

        #region SetupFunctions

        private void SetupFunctions(SQL.DataClasses.Database Database)
        {
            Database.AddFunction("Split", "CREATE FUNCTION dbo.Split(@List nvarchar(MAX),@SplitOn nvarchar(5)) RETURNS @ReturnValue table(Id int identity(1,1),Value nvarchar(200)) AS  BEGIN While (Charindex(@SplitOn,@List)>0) Begin Insert Into @ReturnValue (value) Select Value = ltrim(rtrim(Substring(@List,1,Charindex(@SplitOn,@List)-1))) Set @List = Substring(@List,Charindex(@SplitOn,@List)+len(@SplitOn),len(@List)) End Insert Into @ReturnValue (Value) Select Value = ltrim(rtrim(@List)) Return END");
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Mappings associated to databases
        /// </summary>
        public virtual ListMapping<IDatabase, IMapping> Mappings { get; set; }

        /// <summary>
        /// List of database structures
        /// </summary>
        public virtual System.Collections.Generic.List<Utilities.SQL.DataClasses.Database> DatabaseStructures { get; set; }

        #endregion
    }
}