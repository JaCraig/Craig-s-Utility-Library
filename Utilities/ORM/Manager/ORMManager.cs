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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Utilities.DataTypes;
using Utilities.IoC.Interfaces;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.Schema.Enums;
using Utilities.ORM.Manager.Schema.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

#endregion Usings

namespace Utilities.ORM.Manager
{
    /// <summary>
    /// ORM Manager class
    /// </summary>
    public class ORMManager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ORMManager(IBootstrapper Bootstrapper)
        {
            Contract.Requires<ArgumentNullException>(Bootstrapper != null, "Bootstrapper");
            this.Mappings = new ListMapping<IDatabase, IMapping>();
            MapperProvider = Bootstrapper.Resolve<Mapper.Manager>();
            QueryProvider = Bootstrapper.Resolve<QueryProvider.Manager>();
            SchemaProvider = Bootstrapper.Resolve<Schema.Manager>();
            SourceProvider = Bootstrapper.Resolve<SourceProvider.Manager>();
            SetupMappings();
            SetupDatabases();
        }

        /// <summary>
        /// Mapper provider
        /// </summary>
        private Mapper.Manager MapperProvider { get; set; }

        /// <summary>
        /// Mappings associate with a source
        /// </summary>
        private ListMapping<IDatabase, IMapping> Mappings { get; set; }

        /// <summary>
        /// Query provider
        /// </summary>
        private QueryProvider.Manager QueryProvider { get; set; }

        /// <summary>
        /// Schema provider
        /// </summary>
        private Schema.Manager SchemaProvider { get; set; }

        /// <summary>
        /// Source provider
        /// </summary>
        private SourceProvider.Manager SourceProvider { get; set; }

        private static ITable SetupAuditTables(ITable Table)
        {
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
            ITable AuditTable = Table.Source.AddTable(Table.Name + "Audit");
            AuditTable.AddColumn("ID", DbType.Int32, 0, false, true, true, true, false, "", "", 0);
            AuditTable.AddColumn("AuditType", SqlDbType.NVarChar.To(DbType.Int32), 1, false, false, false, false, false, "", "", "");
            foreach (IColumn Column in Table.Columns)
                AuditTable.AddColumn(Column.Name, Column.DataType, Column.Length, Column.Nullable, false, false, false, false, "", "", "");
            return AuditTable;
        }

        private static void SetupAuditTables(IDatabase Key, Schema.Default.Database.Database TempDatabase)
        {
            Contract.Requires<ArgumentNullException>(Key != null, "Key");
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

        private static void SetupProperties(ITable Table, IMapping Mapping)
        {
            Contract.Requires<ArgumentNullException>(Mapping != null, "Mapping");
            Contract.Requires<ArgumentNullException>(Table != null, "Table");
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

        private void SetupDatabases()
        {
            List<Schema.Default.Database.Database> Databases = new List<Schema.Default.Database.Database>();
            foreach (IDatabase Key in Mappings.Keys)
            {
                if (Key.Update)
                {
                    Utilities.ORM.Manager.Schema.Default.Database.Database TempDatabase = new Schema.Default.Database.Database(Key.Name);
                    SetupTables(Key, TempDatabase);
                    SetupJoiningTables(Key, TempDatabase);
                    SetupAuditTables(Key, TempDatabase);

                    Databases.Add(TempDatabase);
                    foreach (ITable Table in TempDatabase.Tables)
                    {
                        Table.SetupForeignKeys();
                    }
                    List<string> Commands = SchemaProvider.GenerateSchema(TempDatabase, SourceProvider.GetSource(Key.Name)).ToList();
                    IBatch Batch = QueryProvider.Batch(SourceProvider.GetSource(Key.Name));
                    for (int x = 0; x < Commands.Count; ++x)
                    {
                        if (Commands[x].Contains("CREATE TRIGGER") || Commands[x].Contains("CREATE FUNCTION"))
                        {
                            if (Batch.CommandCount > 0)
                            {
                                Batch.Execute();
                                Batch = QueryProvider.Batch(SourceProvider.GetSource(Key.Name));
                            }
                            Batch.AddCommand(CommandType.Text, Commands[x]);
                            if (x < Commands.Count - 1)
                            {
                                Batch.Execute();
                                Batch = QueryProvider.Batch(SourceProvider.GetSource(Key.Name));
                            }
                        }
                        else
                        {
                            Batch.AddCommand(CommandType.Text, Commands[x]);
                        }
                    }
                    Batch.Execute();
                }
            }
        }

        private void SetupJoiningTables(IDatabase Key, Schema.Default.Database.Database TempDatabase)
        {
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
                        SetupJoiningTablesEnumerable(Mapping, Property, Key, TempDatabase);
                    }
                }
            }
        }

        private void SetupJoiningTablesEnumerable(IMapping Mapping, IProperty Property, IDatabase Key, Schema.Default.Database.Database TempDatabase)
        {
            Contract.Requires<ArgumentNullException>(TempDatabase != null, "TempDatabase");
            if (TempDatabase.Tables.FirstOrDefault(x => x.Name == Property.TableName) != null)
                return;
            IMapping MapMapping = Mappings[Key].FirstOrDefault(x => x.ObjectType == Property.Type);
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

        private void SetupMappings()
        {
            IEnumerable<IDatabase> Databases = AppDomain.CurrentDomain
                                                         .GetAssemblies()
                                                         .Objects<IDatabase>();
            foreach (IMapping Mapping in MapperProvider)
            {
                Mappings.Add(Databases.FirstOrDefault(x => x.GetType() == Mapping.DatabaseConfigType), Mapping);
            }

            foreach (IMapping Mapping in MapperProvider.OrderBy(x => x.Order))
            {
                foreach (IProperty Property in Mapping.Properties)
                {
                    if (Property is IManyToMany || Property is IManyToOne || Property is IMap || Property is IIEnumerableManyToOne || Property is IListManyToMany || Property is IListManyToOne)
                    {
                        Property.Setup();
                    }
                }
            }
        }

        private void SetupTables(IDatabase Key, Schema.Default.Database.Database TempDatabase)
        {
            foreach (IMapping Mapping in Mappings[Key])
            {
                TempDatabase.AddTable(Mapping.TableName);
                SetupProperties(TempDatabase[Mapping.TableName], Mapping);
            }
        }
    }
}