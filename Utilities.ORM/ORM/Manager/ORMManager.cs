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

namespace Utilities.ORM.Manager
{
    /// <summary>
    /// ORM Manager class
    /// </summary>
    public class ORMManager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MapperProvider">The mapper provider.</param>
        /// <param name="QueryProvider">The query provider.</param>
        /// <param name="SchemaProvider">The schema provider.</param>
        /// <param name="SourceProvider">The source provider.</param>
        /// <param name="Databases">The databases.</param>
        public ORMManager(Mapper.Manager MapperProvider,
            QueryProvider.Manager QueryProvider,
            Schema.Manager SchemaProvider,
            SourceProvider.Manager SourceProvider,
            IEnumerable<IDatabase> Databases)
        {
            Contract.Requires<ArgumentNullException>(MapperProvider != null, "MapperProvider");
            Contract.Requires<ArgumentNullException>(QueryProvider != null, "QueryProvider");
            Contract.Requires<ArgumentNullException>(SchemaProvider != null, "SchemaProvider");
            Contract.Requires<ArgumentNullException>(SourceProvider != null, "SourceProvider");
            Contract.Requires<ArgumentNullException>(Databases != null, "Databases");
            this.Mappings = new ListMapping<IDatabase, IMapping>();
            this.MapperProvider = MapperProvider;
            this.QueryProvider = QueryProvider;
            this.SchemaProvider = SchemaProvider;
            this.SourceProvider = SourceProvider;
            SetupMappings(Databases);
            foreach (IDatabase Database in Mappings.Keys.Where(x => x.Update))
            {
                this.SchemaProvider.Setup(Mappings, QueryProvider, Database, SourceProvider.GetSource(Database.GetType()));
            }
        }

        /// <summary>
        /// Mapper provider
        /// </summary>
        private Mapper.Manager MapperProvider { get; set; }

        /// <summary>
        /// Mappings associate with a source
        /// </summary>
        private ListMapping<IDatabase, IMapping> Mappings { get; set; }

        /// <summary>
        /// Query provider
        /// </summary>
        private QueryProvider.Manager QueryProvider { get; set; }

        /// <summary>
        /// Schema provider
        /// </summary>
        private Schema.Manager SchemaProvider { get; set; }

        /// <summary>
        /// Source provider
        /// </summary>
        private SourceProvider.Manager SourceProvider { get; set; }

        /// <summary>
        /// Outputs information from the manager
        /// </summary>
        /// <returns>String version of the ORMManager</returns>
        public override string ToString()
        {
            return "ORM Manager\r\n";
        }

        private void SetupMappings(IEnumerable<IDatabase> Databases)
        {
            Contract.Requires<NullReferenceException>(MapperProvider != null, "MapperProvider");
            foreach (IMapping Mapping in MapperProvider)
            {
                Mappings.Add(Databases.FirstOrDefault(x => x.GetType() == Mapping.DatabaseConfigType), Mapping);
            }

            foreach (IDatabase Database in Mappings.Keys)
            {
                foreach (IMapping Mapping in Mappings[Database])
                {
                    Mapping.Setup(SourceProvider.GetSource(Database.GetType()), MapperProvider, QueryProvider);
                }
            }
        }
    }
}