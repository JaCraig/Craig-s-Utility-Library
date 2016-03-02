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
using Utilities.DataTypes.Comparison;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;
using Utilities.ORM.Parameters;

namespace Utilities.ORM.Manager.QueryProvider.Default.SQLServer
{
    /// <summary>
    /// SQL Server generator
    /// </summary>
    /// <typeparam name="T">Class type</typeparam>
    public class SQLServerGenerator<T> : IGenerator<T>
        where T : class
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="QueryProvider">Query provider</param>
        /// <param name="Source">Source info</param>
        /// <param name="Mapping">Mapping info</param>
        /// <param name="Structure">The structure.</param>
        public SQLServerGenerator(SQLServerQueryProvider QueryProvider, ISourceInfo Source, IMapping Mapping, Graph<IMapping> Structure)
        {
            this.QueryProvider = QueryProvider;
            this.Source = Source;
            this.Mapping = Mapping;
            this.Structure = Structure;
        }

        /// <summary>
        /// Mapping that the generator uses
        /// </summary>
        protected IMapping Mapping { get; private set; }

        /// <summary>
        /// Query provider that the system uses
        /// </summary>
        protected SQLServerQueryProvider QueryProvider { get; private set; }

        /// <summary>
        /// Source used to connect
        /// </summary>
        protected ISourceInfo Source { get; private set; }

        /// <summary>
        /// Gets the structure.
        /// </summary>
        /// <value>The structure.</value>
        protected Graph<IMapping> Structure { get; private set; }

        /// <summary>
        /// Generates a batch that will get all items for the given type the parameters specified
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch All(params IParameter[] Parameters)
        {
            if (Mapping == null)
                return QueryProvider.Batch(Source);
            Parameters = Parameters.Check(new IParameter[] { });
            return QueryProvider.Batch(Source)
                .AddCommand(null,
                    null, string.Format(CultureInfo.InvariantCulture,
                    "{0}{1}",
                    Mapping.SelectAllCommand,
                    Parameters != null && Parameters.Length > 0 ? " WHERE " + Parameters.ToString(x => x.ToString(), " AND ") : ""),
                    Parameters != null && Parameters.Length > 0 ? CommandType.Text : Mapping.SelectAllCommandType,
                Parameters);
        }

        /// <summary>
        /// Generates a batch that will get all items for the given type the parameters specified
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <param name="Limit">Max number of items to return</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch All(int Limit, params IParameter[] Parameters)
        {
            if (Limit < 1)
                return All(Parameters);
            if (Mapping == null)
                return QueryProvider.Batch(Source);
            Parameters = Parameters.Check(new IParameter[] { });
            return QueryProvider.Batch(Source)
                .AddCommand(null,
                    null, string.Format(CultureInfo.InvariantCulture,
                    "SELECT TOP {0} {1} FROM {2}{3}",
                    Limit,
                    GetColumns(Mapping),
                    Mapping.TableName,
                    Parameters != null && Parameters.Length > 0 ? " WHERE " + Parameters.ToString(x => x.ToString(), " AND ") : ""),
                CommandType.Text,
                Parameters);
        }

        /// <summary>
        /// Generates a batch that will get the first item that satisfies the parameters specified
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Any(params IParameter[] Parameters)
        {
            if (Mapping == null)
                return QueryProvider.Batch(Source);
            Parameters = Parameters.Check(new IParameter[] { });
            return QueryProvider.Batch(Source)
                .AddCommand(null,
                    null, string.Format(CultureInfo.InvariantCulture,
                    "{0}{1}",
                    Mapping.SelectAnyCommand,
                    Parameters != null && Parameters.Length > 0 ? " WHERE " + Parameters.ToString(x => x.ToString(), " AND ") : ""),
                    Parameters != null && Parameters.Length > 0 ? CommandType.Text : Mapping.SelectAnyCommandType,
                Parameters);
        }

        /// <summary>
        /// Generates a batch that will delete the object
        /// </summary>
        /// <param name="Object">Object to delete</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Delete(T Object)
        {
            return QueryProvider
                .Batch(Source)
                .AddCommand(null,
                            null,
                            Mapping.DeleteCommand,
                            Mapping.DeleteCommandType,
                            Mapping.IDProperties.ToArray(x => x.GetParameter(Object)));
        }

        /// <summary>
        /// Generates a batch that will delete the object
        /// </summary>
        /// <param name="Objects">Objects to delete</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Delete(IEnumerable<T> Objects)
        {
            IBatch TempBatch = QueryProvider.Batch(Source);
            foreach (T Object in Objects)
            {
                TempBatch.AddCommand(Delete(Object));
            }
            return TempBatch;
        }

        /// <summary>
        /// Generates a batch that will insert the data from the object
        /// </summary>
        /// <param name="Object">Object to insert</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Insert(T Object)
        {
            return QueryProvider.Batch(Source)
                                .AddCommand((x, y) => y[0].CopyTo(x.Object),
                                            Object,
                                            Mapping.InsertCommand,
                                            Mapping.InsertCommandType,
                                            Mapping.Properties
                                                   .Where(x => x is IMap || x is IReference)
                                                   .Concat(Mapping.IDProperties)
                                                   .Where(x => !x.AutoIncrement)
                                                   .ToArray(x => x.GetParameter(Object)));
        }

        /// <summary>
        /// Generates a batch that will insert the data from the objects
        /// </summary>
        /// <param name="Objects">Objects to insert</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Insert(IEnumerable<T> Objects)
        {
            IBatch TempBatch = QueryProvider.Batch(Source);
            foreach (T Object in Objects)
            {
                TempBatch.AddCommand(Insert(Object));
            }
            return TempBatch;
        }

        /// <summary>
        /// Deletes items from the joining table for the property
        /// </summary>
        /// <param name="Property">Property</param>
        /// <param name="Object">Object</param>
        /// <typeparam name="P">Property type</typeparam>
        /// <returns>The batch with the appropriate commands</returns>
        public IBatch JoinsDelete<P>(IProperty<T, P> Property, T Object)
        {
            IBatch ReturnValue = QueryProvider.Batch(Source);

            if (Object == null)
                return ReturnValue;
            var List = (P)Property.GetValue(Object);
            if (List == null)
                return ReturnValue;
            object CurrentID = Mapping.IDProperties.FirstOrDefault().GetValue(Object);

            IMapping ForeignMapping = Property.ForeignMapping;
            if (string.Compare(Mapping.TableName, ForeignMapping.TableName, StringComparison.Ordinal) == 0
                && Property as IManyToOne != null)
            {
                ReturnValue.AddCommand(null,
                        Object,
                        "DELETE FROM " + Property.TableName + " WHERE " + Mapping.TableName + Mapping.IDProperties.FirstOrDefault().FieldName + "2=@0",
                        CommandType.Text,
                        CurrentID);
            }
            else
            {
                ReturnValue.AddCommand(null,
                        Object,
                        "DELETE FROM " + Property.TableName + " WHERE " + Mapping.TableName + Mapping.IDProperties.FirstOrDefault().FieldName + "=@0",
                        CommandType.Text,
                        CurrentID);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Saves items to the joining table for the property
        /// </summary>
        /// <param name="Property">Property</param>
        /// <param name="Object">Object</param>
        /// <typeparam name="P">Property type</typeparam>
        /// <typeparam name="ItemType">Item type</typeparam>
        /// <returns>The batch with the appropriate commands</returns>
        public IBatch JoinsSave<P, ItemType>(IProperty<T, P> Property, T Object)
        {
            IBatch ReturnValue = QueryProvider.Batch(Source);

            if (Object == null)
                return ReturnValue;
            if (Property as IManyToOne != null)
            {
                var Item = (P)Property.GetValue(Object);
                if (Item == null)
                    return ReturnValue;

                object CurrentID = ((IProperty<T>)Mapping.IDProperties.FirstOrDefault()).GetValue(Object);
                IMapping ForeignMapping = Property.ForeignMapping;
                object ForeignID = ForeignMapping.IDProperties.FirstOrDefault().GetValue(Item);
                string Parameters = "";
                object[] Values = new object[2];
                if (string.Compare(Mapping.TableName, ForeignMapping.TableName, StringComparison.InvariantCulture) == 0)
                {
                    Parameters = Mapping.TableName + Mapping.IDProperties.FirstOrDefault().FieldName + "," + ForeignMapping.TableName + ForeignMapping.IDProperties.FirstOrDefault().FieldName + "2";
                    Values[1] = CurrentID;
                    Values[0] = ForeignID;
                }
                else if (string.Compare(Mapping.TableName, ForeignMapping.TableName, StringComparison.InvariantCulture) <= 0)
                {
                    Parameters = Mapping.TableName + Mapping.IDProperties.FirstOrDefault().FieldName + "," + ForeignMapping.TableName + ForeignMapping.IDProperties.FirstOrDefault().FieldName;
                    Values[0] = CurrentID;
                    Values[1] = ForeignID;
                }
                else
                {
                    Parameters = ForeignMapping.TableName + ForeignMapping.IDProperties.FirstOrDefault().FieldName + "," + Mapping.TableName + Mapping.IDProperties.FirstOrDefault().FieldName;
                    Values[1] = CurrentID;
                    Values[0] = ForeignID;
                }
                ReturnValue.AddCommand(null, Object,
                        "INSERT INTO " + Property.TableName + "(" + Parameters + ") VALUES (@0,@1)",
                        CommandType.Text,
                        Values);
                return ReturnValue;
            }
            var List = (IEnumerable<ItemType>)Property.GetValue(Object);
            if (List == null)
                return ReturnValue;
            foreach (ItemType Item in List)
            {
                if (Item != null)
                {
                    object CurrentID = Mapping.IDProperties.FirstOrDefault().GetValue(Object);
                    IMapping ForeignMapping = Property.ForeignMapping;
                    object ForeignID = ForeignMapping.IDProperties.FirstOrDefault().GetValue(Item);
                    string Parameters = "";
                    object[] Values = new object[2];
                    if (string.Compare(Mapping.TableName, ForeignMapping.TableName, StringComparison.InvariantCulture) < 0)
                    {
                        Parameters = Mapping.TableName + Mapping.IDProperties.FirstOrDefault().FieldName + "," + ForeignMapping.TableName + ForeignMapping.IDProperties.FirstOrDefault().FieldName;
                        Values[0] = CurrentID;
                        Values[1] = ForeignID;
                    }
                    else if (string.Compare(Mapping.TableName, ForeignMapping.TableName, StringComparison.InvariantCulture) == 0)
                    {
                        Parameters = Mapping.TableName + Mapping.IDProperties.FirstOrDefault().FieldName + "," + ForeignMapping.TableName + ForeignMapping.IDProperties.FirstOrDefault().FieldName + "2";
                        Values[0] = CurrentID;
                        Values[1] = ForeignID;
                    }
                    else
                    {
                        Parameters = ForeignMapping.TableName + ForeignMapping.IDProperties.FirstOrDefault().FieldName + "," + Mapping.TableName + Mapping.IDProperties.FirstOrDefault().FieldName;
                        Values[1] = CurrentID;
                        Values[0] = ForeignID;
                    }
                    ReturnValue.AddCommand(null,
                            Object,
                            "INSERT INTO " + Property.TableName + "(" + Parameters + ") VALUES (@0,@1)",
                            CommandType.Text,
                            Values);
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Generates a batch that will get the specific property for the object
        /// </summary>
        /// <typeparam name="P">Property type</typeparam>
        /// <param name="Object">Object to get the property for</param>
        /// <param name="Property">Property to get</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch LoadProperty<P>(T Object, IProperty Property)
        {
            return QueryProvider.Batch(Source)
                .AddCommand(null, null,
                            Property.LoadCommand,
                            Property.LoadCommandType,
                            Mapping.IDProperties.FirstOrDefault().GetValue(Object));
        }

        /// <summary>
        /// Generates a batch that will get the number of pages for a given page size given the
        /// parameters specified
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <param name="PageSize">Page size</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch PageCount(int PageSize, params IParameter[] Parameters)
        {
            string WhereCommand = "";
            if (Parameters != null && Parameters.Length > 0)
                WhereCommand += " WHERE " + Parameters.ToString(x => x.ToString(), " AND ");
            return QueryProvider
                .Batch(Source)
                .AddCommand(null, null, string.Format(CultureInfo.InvariantCulture,
                    "SELECT COUNT(*) as Total FROM (SELECT {0} FROM {1} {2}) as Query",
                    Mapping.IDProperties.ToString(x => x.FieldName),
                    Mapping.TableName,
                    WhereCommand),
                CommandType.Text,
                Parameters);
        }

        /// <summary>
        /// Generates a batch that will get a specific page of data that satisfies the parameters specified
        /// </summary>
        /// <param name="PageSize">Page size</param>
        /// <param name="CurrentPage">The current page (starting at 0)</param>
        /// <param name="OrderBy">The order by portion of the query</param>
        /// <param name="Parameters">Parameters</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Paged(int PageSize, int CurrentPage, string OrderBy, params IParameter[] Parameters)
        {
            string WhereCommand = "";
            string FinalOrderBy = Mapping.IDProperties.ToString(x => x.Name);
            if (!string.IsNullOrEmpty(OrderBy))
                FinalOrderBy = OrderBy;
            int PageStart = CurrentPage * PageSize;
            if (Parameters != null && Parameters.Length > 0)
                WhereCommand += " WHERE " + Parameters.ToString(x => x.ToString(), " AND ");
            return QueryProvider
                .Batch(Source)
                .AddCommand(null,
                    null, string.Format(CultureInfo.InvariantCulture,
                    "SELECT Paged.* FROM (SELECT ROW_NUMBER() OVER (ORDER BY {0}) AS Row, Query.* FROM (SELECT {1} FROM {2} {3}) as Query) AS Paged WHERE Row>{4} AND Row<={5}",
                    FinalOrderBy,
                    GetColumns(Mapping),
                    Mapping.TableName,
                    WhereCommand,
                    PageStart,
                    PageStart + PageSize),
                CommandType.Text,
                Parameters);
        }

        /// <summary>
        /// Saves the object to the source
        /// </summary>
        /// <typeparam name="PrimaryKeyType">Primary key type</typeparam>
        /// <param name="Object">Object to save</param>
        public IBatch Save<PrimaryKeyType>(T Object)
        {
            IBatch TempBatch = QueryProvider.Batch(Source);
            IProperty<T> IDProperty = ((IProperty<T>)Mapping.IDProperties.FirstOrDefault());
            PrimaryKeyType IDValue = IDProperty.GetValue(Object).To(default(PrimaryKeyType));

            var Comparer = new GenericEqualityComparer<PrimaryKeyType>();
            IParameter Param1 = null;
            if (Comparer.Equals(IDValue, default(PrimaryKeyType)))
            {
                return TempBatch.AddCommand(Insert(Object));
            }
            if (IDProperty.AutoIncrement)
            {
                return TempBatch.AddCommand(Update(Object));
            }
            Param1 = typeof(PrimaryKeyType).Is(typeof(string)) ? (IParameter)new StringEqualParameter(IDValue.ToString(), IDProperty.FieldName, IDValue.ToString().Length, IDProperty.FieldName, Source.ParameterPrefix) : (IParameter)new EqualParameter<PrimaryKeyType>(IDValue, IDProperty.FieldName, IDProperty.FieldName, Source.ParameterPrefix);
            if (Any(Param1).Execute()[0].Count == 0)
            {
                return TempBatch.AddCommand(Insert(Object));
            }
            return TempBatch.AddCommand(Update(Object));
        }

        /// <summary>
        /// Sets up the various default commands for the mapping
        /// </summary>
        /// <param name="Mapping"></param>
        public void SetupCommands(IMapping<T> Mapping)
        {
            var FinalNodes = FindChildren(Mapping, Structure.Copy());
            SetupUpdate(Mapping, FinalNodes);
            SetupInsert(Mapping, FinalNodes);
            SetupDelete(Mapping, FinalNodes);
            SetupAllSelect(Mapping, FinalNodes);
            SetupAnySelect(Mapping, FinalNodes);
        }

        /// <summary>
        /// Sets up the default load command for a map property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">Map property</param>
        public void SetupLoadCommands<D>(Mapper.Default.Map<T, D> Property) where D : class
        {
            if (string.IsNullOrEmpty(Property.LoadCommand))
            {
                IMapping ForeignMapping = Property.ForeignMapping;
                Property.SetLoadUsingCommand(string.Format(CultureInfo.CurrentCulture, ForeignMapping.TableName == Mapping.TableName ?
                        "SELECT {0} FROM {1} AS {1}2 INNER JOIN {2} ON {2}2.{3}={1}.{4} WHERE {2}2.{5}=@0" :
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{3}={1}.{4} WHERE {2}.{5}=@0",
                    GetColumns(ForeignMapping),
                    ForeignMapping.TableName,
                    Mapping.TableName,
                    Property.FieldName,
                    ForeignMapping.IDProperties.FirstOrDefault().FieldName,
                    Mapping.IDProperties.FirstOrDefault().FieldName),
                CommandType.Text);
            }
        }

        /// <summary>
        /// Sets up the default load command for a IEnumerableManyToOne property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">IEnumerableManyToOne property</param>
        public void SetupLoadCommands<D>(Mapper.Default.IEnumerableManyToOne<T, D> Property)
            where D : class
        {
            if (string.IsNullOrEmpty(Property.LoadCommand))
            {
                IMapping ForeignMapping = Property.ForeignMapping;
                Property.SetLoadUsingCommand(string.Format(CultureInfo.CurrentCulture, ForeignMapping.TableName == Mapping.TableName ?
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}2={1}.{3} WHERE {2}.{4}{5}=@0" :
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}={1}.{3} WHERE {2}.{4}{5}=@0",
                    GetColumns(ForeignMapping),
                    ForeignMapping.TableName,
                    Property.TableName,
                    ForeignMapping.IDProperties.FirstOrDefault().FieldName,
                    Mapping.TableName,
                    Mapping.IDProperties.FirstOrDefault().FieldName),
                CommandType.Text);
            }
        }

        /// <summary>
        /// Sets up the default load command for a ListManyToOne property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ListManyToOne property</param>
        public void SetupLoadCommands<D>(Mapper.Default.ListManyToOne<T, D> Property)
                    where D : class
        {
            if (string.IsNullOrEmpty(Property.LoadCommand))
            {
                IMapping ForeignMapping = Property.ForeignMapping;
                Property.SetLoadUsingCommand(string.Format(CultureInfo.CurrentCulture, ForeignMapping.TableName == Mapping.TableName ?
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}2={1}.{3} WHERE {2}.{4}{5}=@0" :
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}={1}.{3} WHERE {2}.{4}{5}=@0",
                    GetColumns(ForeignMapping),
                    ForeignMapping.TableName,
                    Property.TableName,
                    ForeignMapping.IDProperties.FirstOrDefault().FieldName,
                    Mapping.TableName,
                    Mapping.IDProperties.FirstOrDefault().FieldName),
                CommandType.Text);
            }
        }

        /// <summary>
        /// Sets up the default load command for a ListManyToMany property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ListManyToMany property</param>
        public void SetupLoadCommands<D>(Mapper.Default.ListManyToMany<T, D> Property)
            where D : class
        {
            if (string.IsNullOrEmpty(Property.LoadCommand))
            {
                IMapping ForeignMapping = Property.ForeignMapping;
                Property.SetLoadUsingCommand(string.Format(CultureInfo.CurrentCulture, ForeignMapping.TableName == Mapping.TableName ?
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}2={1}.{3} WHERE {2}.{4}{5}=@0" :
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}={1}.{3} WHERE {2}.{4}{5}=@0",
                    GetColumns(ForeignMapping),
                    ForeignMapping.TableName,
                    Property.TableName,
                    ForeignMapping.IDProperties.FirstOrDefault().FieldName,
                    Mapping.TableName,
                    Mapping.IDProperties.FirstOrDefault().FieldName),
                CommandType.Text);
            }
        }

        /// <summary>
        /// Sets up the default load command for a ManyToOne property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ManyToOne property</param>
        public void SetupLoadCommands<D>(Mapper.Default.ManyToOne<T, D> Property)
            where D : class
        {
            if (string.IsNullOrEmpty(Property.LoadCommand))
            {
                IMapping ForeignMapping = Property.ForeignMapping;
                Property.SetLoadUsingCommand(string.Format(CultureInfo.CurrentCulture, ForeignMapping.TableName == Mapping.TableName ?
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}={1}.{3} WHERE {2}.{4}{5}2=@0" :
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}={1}.{3} WHERE {2}.{4}{5}=@0",
                    GetColumns(ForeignMapping),
                    ForeignMapping.TableName,
                    Property.TableName,
                    ForeignMapping.IDProperties.FirstOrDefault().FieldName,
                    Mapping.TableName,
                    Mapping.IDProperties.FirstOrDefault().FieldName),
                CommandType.Text);
            }
        }

        /// <summary>
        /// Sets up the default load command for a ManyToMany property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">ManyToMany property</param>
        public void SetupLoadCommands<D>(Mapper.Default.ManyToMany<T, D> Property)
            where D : class
        {
            if (string.IsNullOrEmpty(Property.LoadCommand))
            {
                IMapping ForeignMapping = Property.ForeignMapping;
                Property.SetLoadUsingCommand(string.Format(CultureInfo.CurrentCulture, ForeignMapping.TableName == Mapping.TableName ?
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}2={1}.{3} WHERE {2}.{4}{5}=@0" :
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}={1}.{3} WHERE {2}.{4}{5}=@0",
                    GetColumns(ForeignMapping),
                    ForeignMapping.TableName,
                    Property.TableName,
                    ForeignMapping.IDProperties.FirstOrDefault().FieldName,
                    Mapping.TableName,
                    Mapping.IDProperties.FirstOrDefault().FieldName),
                CommandType.Text);
            }
        }

        /// <summary>
        /// Sets up the default load command for a map property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">Map property</param>
        public void SetupLoadCommands<D>(Mapper.Default.IListManyToMany<T, D> Property)
            where D : class
        {
            if (string.IsNullOrEmpty(Property.LoadCommand))
            {
                IMapping ForeignMapping = Property.ForeignMapping;
                Property.SetLoadUsingCommand(string.Format(CultureInfo.CurrentCulture, ForeignMapping.TableName == Mapping.TableName ?
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}2={1}.{3} WHERE {2}.{4}{5}=@0" :
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}={1}.{3} WHERE {2}.{4}{5}=@0",
                    GetColumns(ForeignMapping),
                    ForeignMapping.TableName,
                    Property.TableName,
                    ForeignMapping.IDProperties.FirstOrDefault().FieldName,
                    Mapping.TableName,
                    Mapping.IDProperties.FirstOrDefault().FieldName),
                CommandType.Text);
            }
        }

        /// <summary>
        /// Sets up the default load command for a map property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">Map property</param>
        public void SetupLoadCommands<D>(Mapper.Default.IListManyToOne<T, D> Property) where D : class
        {
            if (string.IsNullOrEmpty(Property.LoadCommand))
            {
                IMapping ForeignMapping = Property.ForeignMapping;
                Property.SetLoadUsingCommand(string.Format(CultureInfo.CurrentCulture, ForeignMapping.TableName == Mapping.TableName ?
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}2={1}.{3} WHERE {2}.{4}{5}=@0" :
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}={1}.{3} WHERE {2}.{4}{5}=@0",
                    GetColumns(ForeignMapping),
                    ForeignMapping.TableName,
                    Property.TableName,
                    ForeignMapping.IDProperties.FirstOrDefault().FieldName,
                    Mapping.TableName,
                    Mapping.IDProperties.FirstOrDefault().FieldName),
                CommandType.Text);
            }
        }

        /// <summary>
        /// Sets up the default load command for a map property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">Map property</param>
        public void SetupLoadCommands<D>(Mapper.Default.ICollectionManyToMany<T, D> Property)
            where D : class
        {
            if (string.IsNullOrEmpty(Property.LoadCommand))
            {
                IMapping ForeignMapping = Property.ForeignMapping;
                Property.SetLoadUsingCommand(string.Format(CultureInfo.CurrentCulture, ForeignMapping.TableName == Mapping.TableName ?
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}2={1}.{3} WHERE {2}.{4}{5}=@0" :
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}={1}.{3} WHERE {2}.{4}{5}=@0",
                    GetColumns(ForeignMapping),
                    ForeignMapping.TableName,
                    Property.TableName,
                    ForeignMapping.IDProperties.FirstOrDefault().FieldName,
                    Mapping.TableName,
                    Mapping.IDProperties.FirstOrDefault().FieldName),
                CommandType.Text);
            }
        }

        /// <summary>
        /// Sets up the default load command for a map property
        /// </summary>
        /// <typeparam name="D">Data type</typeparam>
        /// <param name="Property">Map property</param>
        public void SetupLoadCommands<D>(Mapper.Default.ICollectionManyToOne<T, D> Property)
            where D : class
        {
            if (string.IsNullOrEmpty(Property.LoadCommand))
            {
                IMapping ForeignMapping = Property.ForeignMapping;
                Property.SetLoadUsingCommand(string.Format(CultureInfo.CurrentCulture, ForeignMapping.TableName == Mapping.TableName ?
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}2={1}.{3} WHERE {2}.{4}{5}=@0" :
                        "SELECT {0} FROM {1} INNER JOIN {2} ON {2}.{1}{3}={1}.{3} WHERE {2}.{4}{5}=@0",
                    GetColumns(ForeignMapping),
                    ForeignMapping.TableName,
                    Property.TableName,
                    ForeignMapping.IDProperties.FirstOrDefault().FieldName,
                    Mapping.TableName,
                    Mapping.IDProperties.FirstOrDefault().FieldName),
                CommandType.Text);
            }
        }

        /// <summary>
        /// Generates a batch that will update the data from the object
        /// </summary>
        /// <param name="Object">Object to update</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Update(T Object)
        {
            return QueryProvider.Batch(Source)
                .AddCommand(null,
                null, Mapping.UpdateCommand, Mapping.UpdateCommandType,
                Mapping.Properties
                        .Where(x => x is IMap || x is IReference)
                        .Where(x => !x.AutoIncrement)
                        .Concat(Mapping.IDProperties)
                        .ToArray(x => x.GetParameter(Object)));
        }

        /// <summary>
        /// Generates a batch that will update the data from the objects
        /// </summary>
        /// <param name="Objects">Objects to update</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Update(IEnumerable<T> Objects)
        {
            IBatch TempBatch = QueryProvider.Batch(Source);
            foreach (T Object in Objects)
            {
                TempBatch.AddCommand(Update(Object));
            }
            return TempBatch;
        }

        private static IEnumerable<IMapping> FindChildren(IMapping Mapping, Graph<IMapping> graph)
        {
            List<IMapping> ResultList = new List<IMapping>();
            var StartingNodes = new Vertex<IMapping>[] { graph.Vertices.First(x => x.Data == Mapping) }.ToList();
            while (StartingNodes.Count > 0)
            {
                var Vertex = StartingNodes.First();
                StartingNodes.Remove(Vertex);
                ResultList.AddIfUnique(Vertex.Data);
                foreach (Edge<IMapping> Edge in Vertex.OutgoingEdges.ToList())
                {
                    Vertex<IMapping> Sink = Edge.Sink;
                    Edge.Remove();
                    if (Sink.IncomingEdges.Count == 0)
                    {
                        ResultList.AddIfUnique(Sink.Data);
                        StartingNodes.AddIfUnique(Sink);
                    }
                }
            }
            return ResultList;
        }

        private static string GetColumns(IMapping Mapping)
        {
            Contract.Requires<ArgumentNullException>(Mapping != null, "Mapping");
            Contract.Requires<ArgumentNullException>(Mapping.Properties != null, "Mapping.Properties");
            Contract.Requires<ArgumentNullException>(Mapping.IDProperties != null, "Mapping.IDProperties");
            return Mapping.Properties
                          .Where(x => (x as IReference) != null)
                          .Concat(Mapping.IDProperties)
                          .ToString(x => x.TableName + "." + x.FieldName + " AS [" + x.Name + "]");
        }

        private static void SetupAllSelect(IMapping<T> Mapping, IEnumerable<IMapping> ParentMappings)
        {
            Contract.Requires<ArgumentNullException>(Mapping != null, "Mapping");
            if (!string.IsNullOrEmpty(Mapping.SelectAllCommand))
                return;
            Mapping.SetSelectAllCommand(string.Format(CultureInfo.InvariantCulture,
                    "SELECT {0} FROM {1}",
                    GetColumns(Mapping),
                    Mapping.TableName),
                CommandType.Text);
        }

        private static void SetupAnySelect(IMapping<T> Mapping, IEnumerable<IMapping> ParentMappings)
        {
            Contract.Requires<ArgumentNullException>(Mapping != null, "Mapping");
            if (!string.IsNullOrEmpty(Mapping.SelectAnyCommand))
                return;
            Mapping.SetSelectAnyCommand(string.Format(CultureInfo.InvariantCulture,
                    "SELECT TOP 1 {0} FROM {1}",
                    GetColumns(Mapping),
                    Mapping.TableName),
                CommandType.Text);
        }

        private static void SetupDelete(IMapping<T> Mapping, IEnumerable<IMapping> ParentMappings)
        {
            Contract.Requires<ArgumentNullException>(Mapping != null, "Mapping");
            if (!string.IsNullOrEmpty(Mapping.DeleteCommand))
                return;
            string IDProperties = "";
            int Count = 0;
            string Separator = "";
            foreach (IProperty Property in Mapping.IDProperties)
            {
                IDProperties += Separator + Property.FieldName + "=@" + Count;
                Separator = " AND ";
                ++Count;
            }
            Mapping.SetDeleteCommand(string.Format(CultureInfo.InvariantCulture,
                    "DELETE FROM {0} WHERE {1}",
                    Mapping.TableName,
                    IDProperties),
                CommandType.Text);
        }

        private static void SetupInsert(IMapping Mapping, IEnumerable<IMapping> ParentMappings)
        {
            Contract.Requires<ArgumentNullException>(Mapping != null, "Mapping");
            if (!string.IsNullOrEmpty(Mapping.InsertCommand))
                return;

            string ParameterList = "";
            string ValueList = "";
            string Splitter = "";
            int Counter = 0;
            foreach (IProperty Property in Mapping.Properties.Where(x => x is IMap || x is IReference))
            {
                if (!Property.AutoIncrement)
                {
                    ParameterList += Splitter + Property.FieldName;
                    ValueList += Splitter + "@" + Counter;
                    Splitter = ",";
                    ++Counter;
                }
            }
            foreach (IProperty Property in Mapping.IDProperties)
            {
                if (!Property.AutoIncrement)
                {
                    ParameterList += Splitter + Property.FieldName;
                    ValueList += Splitter + "@" + Counter;
                    Splitter = ",";
                    ++Counter;
                }
            }
            Mapping.SetInsertCommand(string.Format(CultureInfo.InvariantCulture,
                    "INSERT INTO {0}({1}) VALUES({2}) SELECT scope_identity() as [{3}]",
                    Mapping.TableName,
                    ParameterList,
                    ValueList,
                    Mapping.IDProperties.Count > 0 ? Mapping.IDProperties.FirstOrDefault().Name : "ID"),
                CommandType.Text);
        }

        private static void SetupUpdate(IMapping<T> Mapping, IEnumerable<IMapping> ParentMappings)
        {
            Contract.Requires<ArgumentNullException>(Mapping != null, "Mapping");
            if (!string.IsNullOrEmpty(Mapping.UpdateCommand))
                return;
            string ParameterList = "";
            string IDProperties = "";
            int Count = 0;
            string Separator = "";
            string Splitter = "";
            foreach (IProperty Property in Mapping.Properties.Where(x => x is IMap || x is IReference))
            {
                if (!Property.AutoIncrement)
                {
                    ParameterList += Splitter + Property.FieldName + "=@" + Count;
                    Splitter = ",";
                    ++Count;
                }
            }
            foreach (IProperty Property in Mapping.IDProperties)
            {
                IDProperties += Separator + Property.FieldName + "=@" + Count;
                Separator = " AND ";
                ++Count;
            }

            Mapping.SetUpdateCommand(string.Format(CultureInfo.InvariantCulture,
                    "UPDATE {0} SET {1} WHERE {2}",
                    Mapping.TableName,
                    ParameterList,
                    IDProperties),
                CommandType.Text);
        }
    }
}