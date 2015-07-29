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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Utilities.DataTypes;

namespace Ironman.Core.API.Manager.Properties
{
    /// <summary>
    /// Map class
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type</typeparam>
    public class Map<ClassType, DataType> : APIPropertyBaseClass<ClassType, DataType>, IMap
        where ClassType : class, new()
        where DataType : class, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Expression">Expression</param>
        public Map(Expression<Func<ClassType, DataType>> Expression)
            : base(Expression)
        {
            Contract.Requires<ArgumentNullException>(Expression != null, "Expression");
        }

        /// <summary>
        /// Deletes the property item
        /// </summary>
        /// <param name="MappingHolder">Mappings holder</param>
        /// <param name="Object">Object</param>
        /// <param name="PropertyID">Property ID</param>
        /// <returns>The result</returns>
        public override bool DeleteValue(MappingHolder MappingHolder, dynamic Object, string PropertyID)
        {
            if (MappingHolder == null)
                return false;
            ClassType TempItem = Object;
            if (TempItem == null)
                return false;
            var Mapping = (IAPIMapping<DataType>)MappingHolder[typeof(DataType).Name];
            if (Mapping == null)
                return false;
            DataType Value = CompiledExpression(TempItem);
            if (!Mapping.CanDelete(Value))
                return false;
            return Mapping.Delete(PropertyID);
        }

        /// <summary>
        /// Gets the value of the property from an object
        /// </summary>
        /// <param name="Object">Object to get the property from</param>
        /// <param name="Mappings">Mappings</param>
        /// <returns>The property specified</returns>
        public override dynamic GetValue(MappingHolder Mappings, dynamic Object)
        {
            if (Mappings == null)
                return false;
            ClassType TempItem = Object;
            if (TempItem == null)
                return null;
            var Mapping = (IAPIMapping<DataType>)Mappings[typeof(DataType).Name];
            if (Mapping == null)
                return new Dynamo(CompiledExpression(TempItem));
            DataType Value = CompiledExpression(TempItem);
            if (!Mapping.CanGet(Value))
                return null;
            var ReturnValue = new Dynamo(Value);
            return ReturnValue.SubSet(Mapping.Properties.Where(x => x is IReference || x is IID)
                                                           .Select(x => x.Name)
                                                           .ToArray());
        }

        /// <summary>
        /// Saves the property item
        /// </summary>
        /// <param name="MappingHolder">Mapping holder</param>
        /// <param name="Object">Object</param>
        /// <param name="Models">Models</param>
        /// <returns>True if it is saved, false otherwise</returns>
        public override bool SaveValue(MappingHolder MappingHolder, dynamic Object, IEnumerable<Dynamo> Models)
        {
            if (MappingHolder == null)
                return false;
            ClassType TempItem = Object;
            if (TempItem == null)
                return false;
            if (AssignExpression == null)
                return false;
            var Mapping = (IAPIMapping<DataType>)MappingHolder[typeof(DataType).Name];
            var ClassMapping = (IAPIMapping<ClassType>)MappingHolder[typeof(ClassType).Name];
            if (Mapping == null || ClassMapping == null || Models == null)
                return false;
            Dynamo Model = Models.FirstOrDefault();
            if (Model == null)
                return false;
            dynamic TempModel = Model.SubSet(Mapping.Properties.Where(x => x is IReference || x is IID)
                                                           .Select(x => x.Name)
                                                           .ToArray());
            DataType Property = TempModel;
            DataType Value = CompiledExpression(TempItem);
            if (Value == null)
            {
                AssignExpression(TempItem, Property);
                Mapping.SaveFunc(Property);
                return ClassMapping.SaveFunc(TempItem);
            }
            else if (Value.Equals(Property))
            {
                Model.SubSet(Mapping.Properties.Where(x => x is IReference || x is IID)
                                                           .Select(x => x.Name)
                                                           .ToArray())
                     .CopyTo(Value);
                return Mapping.SaveFunc(Value);
            }
            AssignExpression(TempItem, Property);
            return ClassMapping.SaveFunc(TempItem);
        }
    }
}