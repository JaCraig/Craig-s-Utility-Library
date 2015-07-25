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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataTypes;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.Schema.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

namespace Utilities.ORM.Manager.Schema.Default.LDAP
{
    /// <summary>
    /// LDAP schema generator
    /// </summary>
    public class LDAPSchemaGenerator : ISchemaGenerator
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LDAPSchemaGenerator(QueryProvider.Manager Provider, SourceProvider.Manager SourceProvider)
        {
            this.Provider = Provider;
            this.SourceProvider = SourceProvider;
        }

        /// <summary>
        /// Provider name associated with the schema generator
        /// </summary>
        public string ProviderName { get { return "LDAP"; } }

        /// <summary>
        /// Query provider object
        /// </summary>
        protected QueryProvider.Manager Provider { get; private set; }

        /// <summary>
        /// Source provider object
        /// </summary>
        protected SourceProvider.Manager SourceProvider { get; private set; }

        /// <summary>
        /// Generates a list of commands used to modify the source. If it does not exist prior, the
        /// commands will create the source from scratch. Otherwise the commands will only add new
        /// fields, tables, etc. It does not delete old fields.
        /// </summary>
        /// <param name="DesiredStructure">Desired source structure</param>
        /// <param name="Source">Source to use</param>
        /// <returns>List of commands generated</returns>
        public IEnumerable<string> GenerateSchema(ISource DesiredStructure, ISourceInfo Source)
        {
            return new List<string>();
        }

        /// <summary>
        /// Gets the structure of a source
        /// </summary>
        /// <param name="Source">Source to use</param>
        /// <returns>The source structure</returns>
        public ISource GetSourceStructure(ISourceInfo Source)
        {
            return null;
        }

        /// <summary>
        /// Sets up the specified database schema
        /// </summary>
        /// <param name="Mappings">The mappings.</param>
        /// <param name="Database">The database.</param>
        /// <param name="QueryProvider">The query provider.</param>
        public void Setup(ListMapping<IDatabase, IMapping> Mappings, IDatabase Database, QueryProvider.Manager QueryProvider)
        {
        }

        /// <summary>
        /// Checks if a source exists
        /// </summary>
        /// <param name="Source">Source to check</param>
        /// <param name="Info">Source info to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool SourceExists(string Source, ISourceInfo Info)
        {
            return true;
        }

        /// <summary>
        /// Checks if a stored procedure exists
        /// </summary>
        /// <param name="StoredProcedure">Stored procedure to check</param>
        /// <param name="Source">Source to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool StoredProcedureExists(string StoredProcedure, ISourceInfo Source)
        {
            return false;
        }

        /// <summary>
        /// Checks if a table exists
        /// </summary>
        /// <param name="Table">Table to check</param>
        /// <param name="Source">Source to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool TableExists(string Table, ISourceInfo Source)
        {
            return false;
        }

        /// <summary>
        /// Checks if a trigger exists
        /// </summary>
        /// <param name="Trigger">Trigger to check</param>
        /// <param name="Source">Source to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool TriggerExists(string Trigger, ISourceInfo Source)
        {
            return false;
        }

        /// <summary>
        /// Checks if a view exists
        /// </summary>
        /// <param name="View">View to check</param>
        /// <param name="Source">Source to use</param>
        /// <returns>True if it exists, false otherwise</returns>
        public bool ViewExists(string View, ISourceInfo Source)
        {
            return false;
        }
    }
}