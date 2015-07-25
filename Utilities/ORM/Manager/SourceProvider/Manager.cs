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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using Utilities.DataTypes;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

namespace Utilities.ORM.Manager.SourceProvider
{
    /// <summary>
    /// Database manager
    /// </summary>
    public class Manager : IEnumerable<ISourceInfo>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Sources">The sources.</param>
        public Manager(IEnumerable<IDatabase> Sources)
        {
            Contract.Requires<ArgumentNullException>(Sources != null, "Sources");
            this.Sources = Sources.OrderBy(x => x.Order)
                             .ToDictionary(x => x.Name, x => (ISourceInfo)new SourceInfo("", x.Name, "", "", x.Writable, x.Readable, x));
            foreach (ConnectionStringSettings ConnectionString in ConfigurationManager.ConnectionStrings)
            {
                if (!this.Sources.ContainsKey(ConnectionString.Name))
                    this.Sources.Add(ConnectionString.Name, new SourceInfo(ConnectionString.ConnectionString, ConnectionString.Name, "", "", true, true));
            }
        }

        /// <summary>
        /// Source information
        /// </summary>
        protected IDictionary<string, ISourceInfo> Sources { get; private set; }

        /// <summary>
        /// Gets the enumerator for the sources
        /// </summary>
        /// <returns>The source enumerator</returns>
        public IEnumerator<ISourceInfo> GetEnumerator()
        {
            return Sources.ToList(x => x.Value).GetEnumerator();
        }

        /// <summary>
        /// Gets the source info specified
        /// </summary>
        /// <param name="Name">Name of the source to get</param>
        /// <returns>The source specified</returns>
        public ISourceInfo GetSource(string Name)
        {
            if (!Sources.ContainsKey(Name))
                Sources.Add(Name, new SourceInfo(Name, Name));
            return Sources.GetValue(Name, null);
        }

        /// <summary>
        /// Getsthe source info specified
        /// </summary>
        /// <param name="DatabaseType">Database type</param>
        /// <returns>The source specified</returns>
        public ISourceInfo GetSource(Type DatabaseType)
        {
            return Sources.FirstOrDefault(x => x.Value.Database.GetType() == DatabaseType).Value;
        }

        /// <summary>
        /// Gets the enumerator for the sources
        /// </summary>
        /// <returns>The source enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Sources.ToList(x => x.Value).GetEnumerator();
        }

        /// <summary>
        /// Outputs the source information as a string
        /// </summary>
        /// <returns>The source information as a string</returns>
        public override string ToString()
        {
            return "Sources: " + Sources.Where(x => !x.Key.Contains("Data Source")).ToString(x => x.Key) + "\r\n";
        }
    }
}