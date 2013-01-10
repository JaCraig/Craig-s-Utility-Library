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
using System.Collections.Concurrent;
using System.Configuration;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.SQL.MicroORM.Interfaces;
#endregion

namespace Utilities.SQL.MicroORM
{
    /// <summary>
    /// Holds database information
    /// </summary>
    public class Database
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Connection">Connection string (if not specified, uses Name to get the connection string from the configuration manager)</param>
        /// <param name="Name">Database name (if not specified, it uses the first connection string it finds in the configuration manager)</param>
        /// <param name="DbType">Database type, based on ADO.Net provider name (will override if it pulls info from the configuration manager)</param>
        /// <param name="ParameterPrefix">Parameter prefix to use (if empty, it will override with its best guess based on database type)</param>
        /// <param name="Profile">Determines if the calls should be profiled</param>
        /// <param name="Readable">Should this database be used to read data?</param>
        /// <param name="Writable">Should this database be used to write data?</param>
        public Database(string Connection, string Name, string DbType = "System.Data.SqlClient",
                        string ParameterPrefix = "@", bool Profile = false, bool Writable = true,
                        bool Readable = true)
        {
            this.Name = Name.IsNullOrEmpty() && ConfigurationManager.ConnectionStrings[0] != null ? ConfigurationManager.ConnectionStrings[0].Name : Name;
            if (Connection.IsNullOrEmpty() && ConfigurationManager.ConnectionStrings[this.Name] != null)
            {
                this.Connection = ConfigurationManager.ConnectionStrings[this.Name].ConnectionString;
                this.DbType = ConfigurationManager.ConnectionStrings[this.Name].ProviderName;
            }
            else
            {
                this.Connection = Connection;
                this.DbType = DbType;
            }
            if (ParameterPrefix.IsNullOrEmpty())
            {
                this.ParameterPrefix = "@";
                if (DbType.Contains("MySql"))
                    this.ParameterPrefix = "?";
                else if (DbType.Contains("Oracle"))
                    this.ParameterPrefix = ":";
            }
            else
                this.ParameterPrefix = ParameterPrefix;
            this.Profile = Profile;
            this.Mappings = new ConcurrentDictionary<Type, IMapping>();
            this.Writable = Writable;
            this.Readable = Readable;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Connection string
        /// </summary>
        public string Connection { get; protected set; }

        /// <summary>
        /// Name of the database
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Database type, based on ADO.Net provider name
        /// </summary>
        public string DbType { get; protected set; }

        /// <summary>
        /// Parameter prefix that the database uses
        /// </summary>
        public string ParameterPrefix { get; protected set; }

        /// <summary>
        /// Should calls to this database be profiled?
        /// </summary>
        public bool Profile { get; protected set; }

        /// <summary>
        /// Contains the mappings associated with this database
        /// </summary>
        public ConcurrentDictionary<Type, IMapping> Mappings { get; private set; }

        /// <summary>
        /// Should this database be used to write data?
        /// </summary>
        public bool Writable { get; protected set; }

        /// <summary>
        /// Should this database be used to read data?
        /// </summary>
        public bool Readable { get; protected set; }

        #endregion
    }
}