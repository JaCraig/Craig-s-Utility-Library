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

using Ironman.Core.API.Manager.BaseClasses;
using Ironman.Core.API.Manager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataTypes;

namespace Ironman.Core.API.Manager.Properties
{
    /// <summary>
    /// Map class
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type</typeparam>
    public class Map<ClassType, DataType> : APIPropertyBaseClass<ClassType, DataType>, IMap
        where ClassType : class,new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Expression">Expression</param>
        public Map(Expression<Func<ClassType, DataType>> Expression)
            : base(Expression)
        {
        }

        /// <summary>
        /// Gets the value of the property from an object
        /// </summary>
        /// <param name="Object">Object to get the property from</param>
        /// <param name="Mappings">Mappings</param>
        /// <returns>The property specified</returns>
        public override dynamic GetValue(MappingHolder Mappings, dynamic Object)
        {
            ClassType TempItem = Object;
            if (TempItem == null)
                return null;
            IAPIMapping Mapping = Mappings[typeof(DataType).Name];
            if (Mapping == null)
                return new Dynamo(CompiledExpression(TempItem));
            Dynamo ReturnValue = new Dynamo(CompiledExpression(TempItem));
            return ReturnValue.SubSet(Mapping.Properties.Where(x => x is IReference || x is IID)
                                                           .Select(x => x.Name)
                                                           .ToArray());
        }
    }
}