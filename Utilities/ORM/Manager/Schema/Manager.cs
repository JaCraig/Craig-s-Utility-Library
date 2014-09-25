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
using System.Diagnostics.Contracts;
using System.Linq;
using Utilities.DataTypes;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.Schema.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

namespace Utilities.ORM.Manager.Schema
{
    /// <summary>
    /// Schema manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SchemaGenerators">The schema generators.</param>
        public Manager(IEnumerable<ISchemaGenerator> SchemaGenerators)
        {
            Contract.Requires<ArgumentNullException>(SchemaGenerators != null, "SchemaGenerators");
            this.SchemaGenerators = SchemaGenerators.ToDictionary(x => x.ProviderName);
        }

        /// <summary>
        /// Schema generators
        /// </summary>
        protected IDictionary<string, ISchemaGenerator> SchemaGenerators { get; private set; }

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
            Contract.Requires<ArgumentNullException>(Source != null, "Source");
            return SchemaGenerators.ContainsKey(Source.SourceType) ?
                SchemaGenerators[Source.SourceType].GenerateSchema(DesiredStructure, Source) :
                new List<string>();
        }

        /// <summary>
        /// Sets up the specified databases
        /// </summary>
        /// <param name="Mappings">The mappings.</param>
        /// <param name="QueryProvider">The query provider.</param>
        /// <param name="Database">The database.</param>
        /// <param name="Source">The source.</param>
        public void Setup(ListMapping<IDatabase, IMapping> Mappings, QueryProvider.Manager QueryProvider, IDatabase Database, ISourceInfo Source)
        {
            Contract.Requires<NullReferenceException>(Mappings != null, "Mappings");
            SchemaGenerators[Source.SourceType].Setup(Mappings, Database, QueryProvider);
        }

        /// <summary>
        /// Outputs the schema generator information as a string
        /// </summary>
        /// <returns>The schema generator information as a string</returns>
        public override string ToString()
        {
            return "Schema Providers: " + SchemaGenerators.OrderBy(x => x.Key).ToString(x => x.Key) + "\r\n";
        }
    }
}