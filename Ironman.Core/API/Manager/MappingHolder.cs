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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ironman.Core.API.Manager.Interfaces;
using Utilities.DataTypes;

namespace Ironman.Core.API.Manager
{
    /// <summary>
    /// Holds the various mappings for a version
    /// </summary>
    public class MappingHolder : IEnumerable<IAPIMapping>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MappingHolder()
        {
            this.Mappings = new Dictionary<string, IAPIMapping>();
        }

        /// <summary>
        /// Mappings that this holds
        /// </summary>
        public IDictionary<string, IAPIMapping> Mappings { get; set; }

        /// <summary>
        /// Gets the specified mapping
        /// </summary>
        /// <param name="Key">Name of the mapped type</param>
        /// <returns>The mapping specified</returns>
        public IAPIMapping this[string Key]
        {
            get
            {
                Contract.Requires<ArgumentNullException>(Mappings != null, "Mappings");
                return Mappings.GetValue(Key);
            }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IAPIMapping> GetEnumerator()
        {
            foreach (var Mapping in Mappings.Values)
                yield return Mapping;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Outputs mapping holder info as a string
        /// </summary>
        /// <returns>String version of the mapping holder</returns>
        public override string ToString()
        {
            return Mappings.ToString(x => x.Key);
        }
    }
}