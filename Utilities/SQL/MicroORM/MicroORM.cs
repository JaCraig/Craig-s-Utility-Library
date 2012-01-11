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
    public class MicroORM : SQLHelper
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ConnectionUsing">Connection string or name of the database
        /// connection string (if already defined)</param>
        public MicroORM(string ConnectionUsing="Default")
            : base("", "", CommandType.Text)
        {
            MappingsUsing = new List<IMapping>();
            if (Databases.ContainsKey(ConnectionUsing))
            {
                this.Connection.ConnectionString = Databases[ConnectionUsing].Connection;
                DatabaseUsing = ConnectionUsing;
            }
            else
            {
                this.Connection.ConnectionString = ConnectionUsing;
                DatabaseUsing = "Default";
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// List of database connections
        /// </summary>
        protected static Dictionary<string, Database> Databases = new Dictionary<string, Database>();

        /// <summary>
        /// Mappings using
        /// </summary>
        protected virtual List<IMapping> MappingsUsing { get; set; }

        /// <summary>
        /// Database using
        /// </summary>
        protected virtual string DatabaseUsing { get; set; }

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
        /// <param name="Database">Database to use</param>
        /// <returns>The created mapping (or an already created one if it exists</returns>
        public static Mapping<ClassType> Map<ClassType>(string TableName, string PrimaryKey, bool AutoIncrement = true, string ParameterStarter = "@", string Database = "Default") where ClassType : class,new()
        {
            if (!Databases.ContainsKey(Database))
                Databases.Add(Database, new Database("", Database));
            if(!Databases[Database].Mappings.ContainsKey(typeof(ClassType)))
                Databases[Database].Mappings.Add(typeof(ClassType), new Mapping<ClassType>(TableName, PrimaryKey, AutoIncrement, ParameterStarter));
            return (Mapping<ClassType>)Databases[Database].Mappings[typeof(ClassType)];
        }

        /// <summary>
        /// Returns a specific mapping
        /// </summary>
        /// <typeparam name="ClassType">Class type to get</typeparam>
        /// <returns>The mapping specified</returns>
        public Mapping<ClassType> Map<ClassType>() where ClassType : class,new()
        {
            if (!Databases[DatabaseUsing].Mappings.ContainsKey(typeof(ClassType)))
                throw new ArgumentOutOfRangeException(typeof(ClassType).Name + " not found");
            return new Mapping<ClassType>((Mapping<ClassType>)Databases[DatabaseUsing].Mappings[typeof(ClassType)], this);
        }

        /// <summary>
        /// Adds a database's info
        /// </summary>
        /// <param name="ConnectionString">Connection string to use for this database</param>
        /// <param name="Name">Name to associate with the database</param>
        public static void Database(string ConnectionString, string Name = "Default")
        {
            if (Databases.ContainsKey(Name))
                Databases[Name].Connection = ConnectionString;
            else
                Databases.Add(Name, new Database(ConnectionString, Name));
        }

        /// <summary>
        /// Clears a database object of all mappings
        /// </summary>
        /// <param name="Database">Database object to clear</param>
        public static void ClearMappings(string Database="Default")
        {
            if (Databases.ContainsKey(Database))
            {
                foreach (Type Key in Databases[Database].Mappings.Keys)
                    Databases[Database].Mappings[Key].Dispose();
                Databases[Database].Mappings.Clear();
            }
        }

        #endregion

        #region IDisposable Members

        public override void Dispose()
        {
            base.Dispose();
            foreach (IMapping Mapping in MappingsUsing)
            {
                Mapping.Dispose();
            }
        }

        #endregion
    }
}