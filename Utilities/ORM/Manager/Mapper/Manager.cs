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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Utilities.DataTypes;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

namespace Utilities.ORM.Manager.Mapper
{
    /// <summary>
    /// Mapping manager
    /// </summary>
    public class Manager : IEnumerable<IMapping>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Manager(IEnumerable<IMapping> Mappings)
        {
            Contract.Requires<ArgumentNullException>(Mappings != null, "Mappings");
            this.Mappings = new ListMapping<Type, IMapping>();
            Mappings.ForEach(x => this.Mappings.Add(x.ObjectType, x));
        }

        /// <summary>
        /// Mappings
        /// </summary>
        protected ListMapping<Type, IMapping> Mappings { get; private set; }

        /// <summary>
        /// Gets the mapping specified by the object type
        /// </summary>
        /// <param name="Key">The object type</param>
        /// <returns>The mapping specified</returns>
        public IEnumerable<IMapping> this[Type Key] { get { return Mappings.GetValue(Key).Check(new ConcurrentBag<IMapping>()); } }

        /// <summary>
        /// Gets the mapping specified by the object type and source
        /// </summary>
        /// <param name="Key">The object type</param>
        /// <param name="Source">Source information</param>
        /// <returns>The mapping specified</returns>
        public IMapping this[Type Key, ISourceInfo Source] { get { return this[Key].FirstOrDefault(x => x.DatabaseConfigType == Source.Database.GetType()); } }

        /// <summary>
        /// Gets the enumerator for the mappings
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<IMapping> GetEnumerator()
        {
            foreach (IEnumerable<IMapping> MappingList in Mappings.Values)
            {
                foreach (IMapping Mapping in MappingList)
                {
                    yield return Mapping;
                }
            }
        }

        /// <summary>
        /// Gets the enumerator for the mappings
        /// </summary>
        /// <returns>The enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (IEnumerable<IMapping> MappingList in Mappings.Values)
            {
                foreach (IMapping Mapping in MappingList)
                {
                    yield return Mapping;
                }
            }
        }

        /// <summary>
        /// Outputs the mapping information as a string
        /// </summary>
        /// <returns>The mapping information as a string</returns>
        public override string ToString()
        {
            return "Mappers: " + Mappings.ToString(x => x.Value.OrderBy(y => y.ToString()).ToString(y => y.ToString())) + "\r\n";
        }
    }
}