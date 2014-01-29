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
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using Utilities.DataTypes;
using Utilities.DataTypes.Patterns.BaseClasses;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.QueryProvider.BaseClasses;
using Utilities.ORM.Manager.QueryProvider.Default;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.Schema.Interfaces;

#endregion Usings

namespace Utilities.ORM.Manager.QueryProvider.Default.SQLServer
{
    /// <summary>
    /// SQL Server generator
    /// </summary>
    /// <typeparam name="T">Class type</typeparam>
    public class SQLServerGenerator<T> : IGenerator<T>
        where T : class,new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SQLServerGenerator()
            : base()
        {
            Mapping = Utilities.IoC.Manager.Bootstrapper.Resolve<Mapper.Manager>()[typeof(T)];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="QueryProvider">Query provider</param>
        /// <param name="ConnectionString">Connection string</param>
        public SQLServerGenerator(SQLServerQueryProvider QueryProvider, string ConnectionString)
            : this()
        {
            this.QueryProvider = QueryProvider;
            this.ConnectionString = ConnectionString;
        }

        /// <summary>
        /// Connection string used to create the batch objects
        /// </summary>
        protected string ConnectionString { get; private set; }

        /// <summary>
        /// Mapping that the generator uses
        /// </summary>
        protected IMapping Mapping { get; private set; }

        /// <summary>
        /// Query provider that the system uses
        /// </summary>
        protected SQLServerQueryProvider QueryProvider { get; private set; }

        /// <summary>
        /// Generates a batch that will get all items for the given type the parameters specified
        /// </summary>
        /// <param name="Parameters">Parameters</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch All(params IParameter[] Parameters)
        {
            if (Mapping == null)
                return QueryProvider.Batch(ConnectionString);
            return QueryProvider.Batch(ConnectionString)
                .AddCommand(string.Format(CultureInfo.InvariantCulture,
                    "SELECT * FROM {0}{1}",
                    Mapping.TableName,
                    Parameters != null && Parameters.Length > 0 ? " WHERE " + Parameters.ToString(x => x.ToString(), " AND ") : ""),
                CommandType.Text,
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
                return QueryProvider.Batch(ConnectionString);
            return QueryProvider.Batch(ConnectionString)
                .AddCommand(string.Format(CultureInfo.InvariantCulture,
                    "SELECT TOP {0} * FROM {1}{2}",
                    Limit,
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
            return All(1, Parameters);
        }

        /// <summary>
        /// Generates a batch that will delete the object
        /// </summary>
        /// <param name="Object">Object to delete</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Delete(T Object)
        {
            string IDProperties = "";
            int Count = 0;
            string Separator = "";
            foreach (IProperty Property in Mapping.IDProperties)
            {
                IDProperties += Separator + Property.FieldName + "=@" + Count;
                Separator = " AND ";
                ++Count;
            }
            return QueryProvider
                .Batch(ConnectionString)
                .AddCommand(string.Format(CultureInfo.InvariantCulture,
                    "DELETE FROM {0} WHERE {1}",
                    Mapping.TableName,
                    IDProperties),
                CommandType.Text,
                Mapping.IDProperties.ToArray(x => ((IProperty<T>)x).GetValue(Object)));
        }

        /// <summary>
        /// Generates a batch that will delete the object
        /// </summary>
        /// <param name="Objects">Objects to delete</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Delete(IEnumerable<T> Objects)
        {
            IBatch TempBatch = QueryProvider.Batch(ConnectionString);
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
            string ParameterList = "";
            string ValueList = "";
            string Splitter = "";
            int Counter = 0;
            foreach (IProperty Property in Mapping.Properties)
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
            return QueryProvider.Batch(ConnectionString)
                .AddCommand(string.Format(CultureInfo.InvariantCulture,
                    "INSERT INTO {0}({1}) VALUES({2}) SELECT scope_identity() as [ID]",
                    Mapping.TableName,
                    ParameterList,
                    ValueList),
                CommandType.Text,
                Mapping.Properties.Concat(Mapping.IDProperties)
                        .Where(x => !x.AutoIncrement)
                        .ToArray(x => ((IProperty<T>)x).GetValue(Object)));
        }

        /// <summary>
        /// Generates a batch that will insert the data from the objects
        /// </summary>
        /// <param name="Objects">Objects to insert</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Insert(IEnumerable<T> Objects)
        {
            IBatch TempBatch = QueryProvider.Batch(ConnectionString);
            foreach (T Object in Objects)
            {
                TempBatch.AddCommand(Insert(Object));
            }
            return TempBatch;
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
                .Batch(ConnectionString)
                .AddCommand(string.Format(CultureInfo.InvariantCulture,
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
        /// <param name="Parameters">Parameters</param>
        /// <param name="CurrentPage">The current page (starting at 0)</param>
        /// <param name="PageSize">Page size</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Paged(int PageSize, int CurrentPage, params IParameter[] Parameters)
        {
            string WhereCommand = "";
            string OrderBy = Mapping.IDProperties.ToString(x => x.FieldName);
            int PageStart = CurrentPage * PageSize;
            if (Parameters != null && Parameters.Length > 0)
                WhereCommand += " WHERE " + Parameters.ToString(x => x.ToString(), " AND ");
            return QueryProvider
                .Batch(ConnectionString)
                .AddCommand(string.Format(CultureInfo.InvariantCulture,
                    "SELECT Paged.* FROM (SELECT ROW_NUMBER() OVER (ORDER BY {0}) AS Row, Query.* FROM (SELECT * FROM {1} {2}) as Query) AS Paged WHERE Row>{3} AND Row<={4}",
                    OrderBy,
                    Mapping.TableName,
                    WhereCommand,
                    PageStart,
                    PageStart + PageSize),
                CommandType.Text,
                Parameters);
        }

        /// <summary>
        /// Generates a batch that will update the data from the object
        /// </summary>
        /// <param name="Object">Object to update</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Update(T Object)
        {
            string ParameterList = "";
            string IDProperties = "";
            int Count = 0;
            string Separator = "";
            foreach (IProperty Property in Mapping.IDProperties)
            {
                IDProperties += Separator + Property.FieldName + "=@" + Count;
                Separator = " AND ";
                ++Count;
            }
            string Splitter = "";
            Count = 0;
            foreach (IProperty Property in Mapping.Properties)
            {
                if (!Property.AutoIncrement)
                {
                    ParameterList += Splitter + Property.FieldName + "=@" + Count;
                    Splitter = ",";
                    ++Count;
                }
            }
            return QueryProvider.Batch(ConnectionString)
                .AddCommand(string.Format(CultureInfo.InvariantCulture,
                    "UPDATE {0} SET {1} WHERE {2}",
                    Mapping.TableName,
                    ParameterList,
                    IDProperties),
                CommandType.Text,
                Mapping.Properties
                        .Where(x => !x.AutoIncrement)
                        .Concat(Mapping.IDProperties)
                        .ToArray(x => ((IProperty<T>)x).GetValue(Object)));
        }

        /// <summary>
        /// Generates a batch that will update the data from the objects
        /// </summary>
        /// <param name="Objects">Objects to update</param>
        /// <returns>Batch with the appropriate commands</returns>
        public IBatch Update(IEnumerable<T> Objects)
        {
            IBatch TempBatch = QueryProvider.Batch(ConnectionString);
            foreach (T Object in Objects)
            {
                TempBatch.AddCommand(Update(Object));
            }
            return TempBatch;
        }
    }
}