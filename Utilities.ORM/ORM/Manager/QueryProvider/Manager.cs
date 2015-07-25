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
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

namespace Utilities.ORM.Manager.QueryProvider
{
    /// <summary>
    /// Query provider manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Providers">The providers.</param>
        public Manager(IEnumerable<Interfaces.IQueryProvider> Providers)
        {
            Contract.Requires<ArgumentNullException>(Providers != null, "Providers");
            this.Providers = Providers.ToDictionary(x => x.ProviderName);
        }

        /// <summary>
        /// Providers
        /// </summary>
        protected IDictionary<string, Interfaces.IQueryProvider> Providers { get; private set; }

        /// <summary>
        /// Creates a batch object
        /// </summary>
        /// <param name="Source">Source to use</param>
        /// <returns>The batch object</returns>
        public IBatch Batch(ISourceInfo Source)
        {
            Contract.Requires<ArgumentNullException>(Source != null, "Source");
            return Providers.ContainsKey(Source.SourceType) ? Providers[Source.SourceType].Batch(Source) : null;
        }

        /// <summary>
        /// Creates a generator object
        /// </summary>
        /// <typeparam name="T">Class type the generator uses</typeparam>
        /// <param name="Source">Source to use</param>
        /// <param name="Mapping">Mapping info</param>
        /// <returns>The generator object</returns>
        public IGenerator<T> Generate<T>(ISourceInfo Source, IMapping Mapping)
            where T : class,new()
        {
            Contract.Requires<ArgumentNullException>(Source != null, "Source");
            return Providers.ContainsKey(Source.SourceType) ? Providers[Source.SourceType].Generate<T>(Source, Mapping) : null;
        }

        /// <summary>
        /// Outputs the provider information as a string
        /// </summary>
        /// <returns>The provider information as a string</returns>
        public override string ToString()
        {
            return "Query providers: " + Providers.OrderBy(x => x.Key).ToString(x => x.Key) + "\r\n";
        }
    }
}