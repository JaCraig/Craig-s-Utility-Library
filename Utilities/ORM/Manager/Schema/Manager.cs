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
using System.Linq;
using Utilities.DataTypes;
using Utilities.DataTypes.Patterns.BaseClasses;
using Utilities.ORM.Manager.Schema.Interfaces;

#endregion Usings

namespace Utilities.ORM.Manager.Schema
{
    /// <summary>
    /// Profiler manager
    /// </summary>
    public class Manager
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Manager()
        {
            SchemaGenerators = AppDomain.CurrentDomain
                                        .GetAssemblies()
                                        .Objects<ISchemaGenerator>()
                                        .ToDictionary(x => x.ProviderName);
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Root profiler object
        /// </summary>
        protected IDictionary<string, ISchemaGenerator> SchemaGenerators { get; private set; }

        #endregion Properties

        #region Functions

        /// <summary>
        /// Generates a list of commands used to modify the source. If it does not exist prior, the
        /// commands will create the source from scratch. Otherwise the commands will only add new
        /// fields, tables, etc. It does not delete old fields.
        /// </summary>
        /// <param name="DesiredStructure">Desired source structure</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>List of commands generated</returns>
        public IEnumerable<string> GenerateSchema(ISource DesiredStructure, string ConnectionString)
        {
            ConnectionString = string.IsNullOrEmpty(ConnectionString) && ConfigurationManager.ConnectionStrings[0] != null ? ConfigurationManager.ConnectionStrings[0].Name : ConnectionString;
            string DbType = "System.Data.SqlClient";
            if (ConfigurationManager.ConnectionStrings[ConnectionString] != null)
            {
                DbType = ConfigurationManager.ConnectionStrings[ConnectionString].ProviderName;
            }
            if (string.IsNullOrEmpty(DbType))
                DbType = "System.Data.SqlClient";
            return SchemaGenerators[DbType].GenerateSchema(DesiredStructure, ConnectionString);
        }

        /// <summary>
        /// Outputs the schema generator information as a string
        /// </summary>
        /// <returns>The schema generator information as a string</returns>
        public override string ToString()
        {
            return SchemaGenerators.ToString(x => x.Key);
        }

        #endregion Functions
    }
}