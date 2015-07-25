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
using Utilities.DataTypes.DataMapper.Interfaces;

namespace Utilities.DataTypes.DataMapper
{
    /// <summary>
    /// Data mapper manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DataMappers">The data mappers.</param>
        /// <param name="MapperModules">The mapper modules.</param>
        public Manager(IEnumerable<IDataMapper> DataMappers, IEnumerable<IMapperModule> MapperModules)
        {
            Contract.Requires<ArgumentNullException>(DataMappers != null, "DataMappers");
            Contract.Requires<ArgumentNullException>(MapperModules != null, "MapperModules");
            DataMapper = DataMappers.FirstOrDefault(x => !x.GetType().Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase));
            if (DataMapper == null)
                DataMapper = DataMappers.FirstOrDefault(x => x.GetType().Namespace.StartsWith("UTILITIES", StringComparison.OrdinalIgnoreCase));
            MapperModules.ForEach(x => x.Map(this));
        }

        /// <summary>
        /// Data mapper
        /// </summary>
        private IDataMapper DataMapper { get; set; }

        /// <summary>
        /// Adds or returns a mapping between two types
        /// </summary>
        /// <typeparam name="Left">Left type</typeparam>
        /// <typeparam name="Right">Right type</typeparam>
        /// <returns>A mapping object for the two types specified</returns>
        public ITypeMapping<Left, Right> Map<Left, Right>()
        {
            return DataMapper.Map<Left, Right>();
        }

        /// <summary>
        /// Adds or returns a mapping between two types
        /// </summary>
        /// <param name="Left">Left type</param>
        /// <param name="Right">Right type</param>
        /// <returns>A mapping object for the two types specified</returns>
        public ITypeMapping Map(Type Left, Type Right)
        {
            return DataMapper.Map(Left, Right);
        }

        /// <summary>
        /// Outputs the string information about the manager
        /// </summary>
        /// <returns>The string info about the manager</returns>
        public override string ToString()
        {
            return "Data mapper: " + DataMapper.ToString() + "\r\n";
        }
    }
}