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

using Ironman.Core.API.Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataTypes;

namespace Ironman.Core.API.Manager.BaseClasses
{
    /// <summary>
    /// API property base class
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type</typeparam>
    public abstract class APIPropertyBaseClass<ClassType, DataType> : IAPIProperty<ClassType>, IAPIProperty<ClassType, DataType>
        where ClassType : class,new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Expression">Expression</param>
        protected APIPropertyBaseClass(Expression<Func<ClassType, DataType>> Expression)
        {
            Contract.Requires<ArgumentNullException>(Expression != null, "Expression");
            CompiledExpression = Expression.Compile();
            Name = Expression.PropertyName();
            Expression<Action<ClassType, DataType>> Assign = Expression.PropertySetter<ClassType, DataType>();
            if (Assign != null)
                AssignExpression = Assign.Compile();
        }

        /// <summary>
        /// Name of the property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Assign compiled expression
        /// </summary>
        protected Action<ClassType, DataType> AssignExpression { get; private set; }

        /// <summary>
        /// Compiled expression
        /// </summary>
        protected Func<ClassType, DataType> CompiledExpression { get; private set; }

        /// <summary>
        /// Deletes the property item
        /// </summary>
        /// <param name="MappingHolder">Mappings holder</param>
        /// <param name="Object">Object</param>
        /// <param name="PropertyID">Property ID</param>
        /// <returns>The result</returns>
        public abstract bool DeleteValue(MappingHolder MappingHolder, dynamic Object, string PropertyID);

        /// <summary>
        /// Gets the value of the property from an object
        /// </summary>
        /// <param name="Object">Object to get the property from</param>
        /// <param name="Mappings">Mappings</param>
        /// <returns>The property specified</returns>
        public abstract dynamic GetValue(MappingHolder Mappings, dynamic Object);

        /// <summary>
        /// Saves the property item
        /// </summary>
        /// <param name="MappingHolder">Mapping holder</param>
        /// <param name="Object">Object</param>
        /// <param name="Models">Models</param>
        /// <returns>True if it is saved, false otherwise</returns>
        public abstract bool SaveValue(MappingHolder MappingHolder, dynamic Object, IEnumerable<Dynamo> Models);
    }
}