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
    /// Holds the various services for a version
    /// </summary>
    public class ServiceHolder : IEnumerable<IService>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceHolder()
        {
            this.Services = new Dictionary<string, IService>();
        }

        /// <summary>
        /// Services that this holds
        /// </summary>
        public IDictionary<string, IService> Services { get; set; }

        /// <summary>
        /// Gets the specified Service
        /// </summary>
        /// <param name="Key">Name of the mapped type</param>
        /// <returns>The Service specified</returns>
        public IService this[string Key]
        {
            get
            {
                Contract.Requires<ArgumentNullException>(Services != null, "Services");
                return Services.GetValue(Key);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IService> GetEnumerator()
        {
            foreach (var Mapping in Services.Values)
                yield return Mapping;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Outputs Service holder info as a string
        /// </summary>
        /// <returns>String version of the Service holder</returns>
        public override string ToString()
        {
            return Services.ToString(x => x.Key);
        }
    }
}