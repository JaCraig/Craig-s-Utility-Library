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
using Utilities.SQL.MicroORM.Interfaces;
using System.Collections.Concurrent;
#endregion

namespace Utilities.SQL.MicroORM
{
    /// <summary>
    /// Holds database information
    /// </summary>
    public class Database : IDisposable
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Connection">Connection string</param>
        /// <param name="Name">Database name</param>
        /// <param name="DbType">Database type, based on ADO.Net provider name</param>
        /// <param name="Profile">Determines if the calls should be profiled</param>
        public Database(string Connection, string Name, string DbType = "System.Data.SqlClient", bool Profile = false)
        {
            this.Connection = Connection;
            this.Name = Name;
            this.DbType = DbType;
            this.Profile = Profile;
            this.Mappings = new ConcurrentDictionary<Type, IMapping>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Connection string
        /// </summary>
        public virtual string Connection { get; set; }

        /// <summary>
        /// Name of the database
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Database type, based on ADO.Net provider name
        /// </summary>
        public virtual string DbType { get; set; }

        /// <summary>
        /// Should calls to this database be profiled?
        /// </summary>
        public virtual bool Profile { get; set; }

        /// <summary>
        /// Contains the mappings associated with this database
        /// </summary>
        public virtual ConcurrentDictionary<Type, IMapping> Mappings { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Disposes of the object
        /// </summary>
        public void Dispose()
        {
            foreach (Type Key in Mappings.Keys)
            {
                Mappings[Key].Dispose();
            }
        }

        #endregion
    }
}