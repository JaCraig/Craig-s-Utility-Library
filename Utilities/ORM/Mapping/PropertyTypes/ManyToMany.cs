/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Linq;
using System.Linq.Expressions;
using Utilities.ORM.Mapping.BaseClasses;
using Utilities.ORM.Mapping.Interfaces;
using Utilities.ORM.QueryProviders.Interfaces;
using Utilities.SQL.Interfaces;
using Utilities.SQL.MicroORM;
using Utilities.SQL;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.ORM.Mapping.PropertyTypes
{
    /// <summary>
    /// Many to many class
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type</typeparam>
    public class ManyToMany<ClassType, DataType> : PropertyBase<ClassType, IEnumerable<DataType>, IManyToMany<ClassType, DataType>>,
        IManyToMany<ClassType, DataType>
        where ClassType : class,new()
        where DataType : class,new()
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Expression">Expression pointing to the many to many</param>
        /// <param name="Mapping">Mapping the StringID is added to</param>
        public ManyToMany(Expression<Func<ClassType, IEnumerable<DataType>>> Expression, IMapping Mapping)
            : base(Expression, Mapping)
        {
            Type = typeof(DataType);
            SetDefaultValue(() => new List<DataType>());
            string Class1 = typeof(ClassType).Name;
            string Class2 = typeof(DataType).Name;
            if (Class1.CompareTo(Class2) < 0)
                SetTableName(Class1 + "_" + Class2);
            else
                SetTableName(Class2 + "_" + Class1);
        }

        #endregion

        #region Functions

        public override void SetupLoadCommands()
        {
            if (this.CommandToLoad != null)
                return;
            IMapping ForeignMapping = Mapping.Manager.Mappings[typeof(DataType)].First(x => x.DatabaseConfigType == Mapping.DatabaseConfigType);
            if (ForeignMapping == Mapping)
            {
                LoadUsingCommand(@"SELECT " + ForeignMapping.TableName + @".*
                                FROM " + ForeignMapping.TableName + @"
                                INNER JOIN " + TableName + " ON " + TableName + "." + ForeignMapping.TableName + ForeignMapping.IDProperty.FieldName + "2=" + ForeignMapping.TableName + "." + ForeignMapping.IDProperty.FieldName + @"
                                WHERE " + TableName + "." + Mapping.TableName + Mapping.IDProperty.FieldName + "=@ID", CommandType.Text);
            }
            else
            {
                LoadUsingCommand(@"SELECT " + ForeignMapping.TableName + @".*
                                FROM " + ForeignMapping.TableName + @"
                                INNER JOIN " + TableName + " ON " + TableName + "." + ForeignMapping.TableName + ForeignMapping.IDProperty.FieldName + "=" + ForeignMapping.TableName + "." + ForeignMapping.IDProperty.FieldName + @"
                                WHERE " + TableName + "." + Mapping.TableName + Mapping.IDProperty.FieldName + "=@ID", CommandType.Text);
            }
        }

        public override IEnumerable<Command> JoinsDelete(ClassType Object, MicroORM MicroORM)
        {
            if (Object == null)
                return new List<Command>();
            IEnumerable<DataType> List = CompiledExpression(Object);
            if (List == null)
                return new List<Command>();
            List<Command> Commands = new List<Command>();
            object CurrentIDParameter = ((IProperty<ClassType>)Mapping.IDProperty).GetAsObject(Object);
            Commands.AddIfUnique(new Command("DELETE FROM " + TableName + " WHERE " + Mapping.TableName + Mapping.IDProperty.FieldName + "=@0",
                    CommandType.Text,
                    CurrentIDParameter));
            return Commands;
        }

        public override IEnumerable<Command> JoinsSave(ClassType Object, MicroORM MicroORM)
        {
            if (Object == null)
                return new List<Command>();
            IEnumerable<DataType> List = CompiledExpression(Object);
            if (List == null)
                return new List<Command>();
            List<Command> Commands = new List<Command>();
            foreach (DataType Item in List)
            {
                if (Item != null)
                {
                    object CurrentIDParameter = ((IProperty<ClassType>)Mapping.IDProperty).GetAsObject(Object);
                    IMapping ForeignMapping = Mapping.Manager.Mappings[typeof(DataType)].First(x => x.DatabaseConfigType == Mapping.DatabaseConfigType);
                    object ForeignIDParameter = ((IProperty<DataType>)ForeignMapping.IDProperty).GetAsObject(Item);
                    string Parameters = "";
                    object[] Values = new object[2];
                    if (Mapping.TableName.CompareTo(ForeignMapping.TableName) <= 0)
                    {
                        Parameters = Mapping.TableName + Mapping.IDProperty.FieldName + "," + ForeignMapping.TableName + ForeignMapping.IDProperty.FieldName + ((ForeignMapping == Mapping) ? "2" : "");
                        Values[0] = CurrentIDParameter;
                        Values[1] = ForeignIDParameter;
                    }
                    else
                    {
                        Parameters = ForeignMapping.TableName + ForeignMapping.IDProperty.FieldName + "," + Mapping.TableName + Mapping.IDProperty.FieldName + ((ForeignMapping == Mapping) ? "2" : "");
                        Values[1] = CurrentIDParameter;
                        Values[0] = ForeignIDParameter;
                    }
                    Commands.AddIfUnique(new Command("INSERT INTO " + TableName + "(" + Parameters + ") VALUES (@0,@1)",
                            CommandType.Text,
                            Values));
                }
            }
            return Commands;
        }

        public override IEnumerable<Command> CascadeJoinsDelete(ClassType Object, MicroORM MicroORM)
        {
            if (Object == null)
                return new List<Command>();
            IEnumerable<DataType> List = CompiledExpression(Object);
            if (List == null)
                return new List<Command>();
            List<Command> Commands = new List<Command>();
            foreach (DataType Item in List)
            {
                if (Item != null)
                {
                    foreach (IProperty Property in Mapping.Manager.Mappings[typeof(DataType)].First(x => x.DatabaseConfigType == Mapping.DatabaseConfigType).Properties)
                    {
                        if (!Property.Cascade &&
                                (Property is IManyToMany
                                    || Property is IManyToOne
                                    || Property is IIEnumerableManyToOne
                                    || Property is IListManyToMany
                                    || Property is IListManyToOne))
                        {
                            Commands.AddIfUnique(((IProperty<DataType>)Property).JoinsDelete(Item, MicroORM));
                        }
                        if (Property.Cascade)
                        {
                            Commands.AddIfUnique(((IProperty<DataType>)Property).CascadeJoinsDelete(Item, MicroORM));
                        }
                    }
                }
            }
            Commands.AddIfUnique(JoinsDelete(Object, MicroORM));
            return Commands;
        }

        public override IEnumerable<Command> CascadeJoinsSave(ClassType Object, MicroORM MicroORM)
        {
            if (Object == null)
                return new List<Command>();
            IEnumerable<DataType> List = CompiledExpression(Object);
            if (List == null)
                return new List<Command>();
            List<Command> Commands = new List<Command>();
            foreach (DataType Item in List)
            {
                if (Item != null)
                {
                    foreach (IProperty Property in Mapping.Manager.Mappings[typeof(DataType)].First(x => x.DatabaseConfigType == Mapping.DatabaseConfigType).Properties)
                    {
                        if (!Property.Cascade &&
                                (Property is IManyToMany
                                    || Property is IManyToOne
                                    || Property is IIEnumerableManyToOne
                                    || Property is IListManyToMany
                                    || Property is IListManyToOne))
                        {
                            Commands.AddIfUnique(((IProperty<DataType>)Property).JoinsSave(Item, MicroORM));
                        }
                        if (Property.Cascade)
                        {
                            Commands.AddIfUnique(((IProperty<DataType>)Property).CascadeJoinsSave(Item, MicroORM));
                        }
                    }
                }
            }
            Commands.AddIfUnique(JoinsSave(Object, MicroORM));
            return Commands;
        }

        public override void CascadeDelete(ClassType Object, MicroORM MicroORM)
        {
            if (Object == null)
                return;
            IEnumerable<DataType> List = CompiledExpression(Object);
            if (List == null)
                return;
            foreach (DataType Item in List)
            {
                if (Item != null)
                {
                    foreach (IProperty Property in Mapping.Manager.Mappings[typeof(DataType)].First(x => x.DatabaseConfigType == Mapping.DatabaseConfigType).Properties)
                    {
                        if (Property.Cascade)
                            ((IProperty<DataType>)Property).CascadeDelete(Item, MicroORM);
                    }
                    ((IProperty<DataType>)Mapping.Manager.Mappings[typeof(DataType)].First(x => x.DatabaseConfigType == Mapping.DatabaseConfigType).IDProperty).CascadeDelete(Item, MicroORM);
                }
            }
        }

        public override void CascadeSave(ClassType Object, MicroORM MicroORM)
        {
            if (Object == null)
                return;
            IEnumerable<DataType> List = CompiledExpression(Object);
            if (List == null)
                return;
            foreach (DataType Item in List)
            {
                if (Item != null)
                {
                    foreach (IProperty Property in Mapping.Manager.Mappings[typeof(DataType)].First(x => x.DatabaseConfigType == Mapping.DatabaseConfigType).Properties)
                    {
                        if (Property.Cascade)
                            ((IProperty<DataType>)Property).CascadeSave(Item, MicroORM);
                    }
                    ((IProperty<DataType>)Mapping.Manager.Mappings[typeof(DataType)].First(x => x.DatabaseConfigType == Mapping.DatabaseConfigType).IDProperty).CascadeSave(Item, MicroORM);
                }
            }
        }

        public override IParameter GetAsParameter(ClassType Object)
        {
            return null;
        }

        public override object GetAsObject(ClassType Object)
        {
            return null;
        }

        public override IManyToMany<ClassType, DataType> LoadUsingCommand(string Command, System.Data.CommandType CommandType)
        {
            this.CommandToLoad = new Command(Command, CommandType);
            return (IManyToMany<ClassType, DataType>)this;
        }

        public override void AddToQueryProvider(IDatabase Database, Mapping<ClassType> Mapping)
        {
        }

        public override IManyToMany<ClassType, DataType> SetDefaultValue(Func<IEnumerable<DataType>> DefaultValue)
        {
            this.DefaultValue = DefaultValue;
            return (IManyToMany<ClassType, DataType>)this;
        }

        public override IManyToMany<ClassType, DataType> DoNotAllowNullValues()
        {
            this.NotNull = true;
            return (IManyToMany<ClassType, DataType>)this;
        }

        public override IManyToMany<ClassType, DataType> ThisShouldBeUnique()
        {
            this.Unique = true;
            return (IManyToMany<ClassType, DataType>)this;
        }

        public override IManyToMany<ClassType, DataType> TurnOnIndexing()
        {
            this.Index = true;
            return (IManyToMany<ClassType, DataType>)this;
        }

        public override IManyToMany<ClassType, DataType> TurnOnAutoIncrement()
        {
            this.AutoIncrement = true;
            return (IManyToMany<ClassType, DataType>)this;
        }

        public override IManyToMany<ClassType, DataType> SetFieldName(string FieldName)
        {
            this.FieldName = FieldName;
            return (IManyToMany<ClassType, DataType>)this;
        }


        public override IManyToMany<ClassType, DataType> SetTableName(string TableName)
        {
            this.TableName = TableName;
            return (IManyToMany<ClassType, DataType>)this;
        }

        public override IManyToMany<ClassType, DataType> TurnOnCascade()
        {
            this.Cascade = true;
            return (IManyToMany<ClassType, DataType>)this;
        }

        public override IManyToMany<ClassType, DataType> SetMaxLength(int MaxLength)
        {
            this.MaxLength = MaxLength;
            return (IManyToMany<ClassType, DataType>)this;
        }

        #endregion
    }
}