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
using Utilities.DataMapper;
using System.Linq.Expressions;
using Utilities.SQL.MicroORM.Interfaces;
using System.Data;
#endregion

namespace Utilities.SQL.MicroORM
{
    /// <summary>
    /// Manager class that can be used to manage
    /// </summary>
    public class MicroORM : IDisposable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MicroORM()
        {
            Mappings = new Dictionary<Type, IMapping>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Mappings
        /// </summary>
        protected static Dictionary<Type, IMapping> Mappings { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Creates a mapping
        /// </summary>
        /// <typeparam name="ClassType">Class type to map</typeparam>
        /// <param name="TableName">Table name</param>
        /// <param name="PrimaryKey">Primary key</param>
        /// <param name="AutoIncrement">Auto incrementing primar key</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        /// <returns>The created mapping (or an already created one if it exists</returns>
        public static Mapping<ClassType> Map<ClassType>(string TableName, string PrimaryKey, bool AutoIncrement = true, string ParameterStarter = "@") where ClassType : class,new()
        {
            if (Mappings.ContainsKey(typeof(ClassType)))
                return (Mapping<ClassType>)Mappings[typeof(ClassType)];
            Mapping<ClassType> Mapping = new Mapping<ClassType>(TableName, PrimaryKey, AutoIncrement, ParameterStarter);
            Mappings.Add(typeof(ClassType), Mapping);
            return Mapping;
        }

        /// <summary>
        /// Creates a mapping
        /// </summary>
        /// <typeparam name="ClassType">Class type to map</typeparam>
        /// <param name="Connection">Connection string to use</param>
        /// <param name="TableName">Table name</param>
        /// <param name="PrimaryKey">Primary key</param>
        /// <param name="AutoIncrement">Auto incrementing primar key</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        /// <param name="DbType">Db type</param>
        /// <returns>The created mapping (or an already created one if it exists</returns>
        public static Mapping<ClassType> Map<ClassType>(string Connection, string TableName, string PrimaryKey, bool AutoIncrement = true, string ParameterStarter = "@", string DbType = "System.Data.SqlClient") where ClassType : class,new()
        {
            if (Mappings.ContainsKey(typeof(ClassType)))
                return (Mapping<ClassType>)Mappings[typeof(ClassType)];
            Mapping<ClassType> Mapping = new Mapping<ClassType>(Connection, TableName, PrimaryKey, AutoIncrement, ParameterStarter, DbType);
            Mappings.Add(typeof(ClassType), Mapping);
            return Mapping;
        }

        /// <summary>
        /// Returns a specific mapping
        /// </summary>
        /// <typeparam name="ClassType">Class type to get</typeparam>
        /// <returns>The mapping specified</returns>
        public static Mapping<ClassType> Map<ClassType>() where ClassType : class,new()
        {
            if (!Mappings.ContainsKey(typeof(ClassType)))
                throw new ArgumentOutOfRangeException(typeof(ClassType).Name + " not found");
            return (Mapping<ClassType>)Mappings[typeof(ClassType)];
        }

        public void Dispose()
        {
            foreach (Type Key in Mappings.Keys)
            {
                Mappings[Key].Dispose();
            }
            Mappings.Clear();
        }

        #endregion
    }
}