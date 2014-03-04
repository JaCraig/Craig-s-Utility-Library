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
using System.Linq;
using System.Text;
using Utilities.DataTypes;

namespace Ironman.Core.API.Manager.Interfaces
{
    /// <summary>
    /// API property
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type</typeparam>
    public interface IAPIProperty<ClassType, DataType>
        where ClassType : class,new()
    {
    }

    /// <summary>
    /// API property
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    public interface IAPIProperty<ClassType> : IAPIProperty
        where ClassType : class,new()
    {
    }

    /// <summary>
    /// API property
    /// </summary>
    public interface IAPIProperty
    {
        /// <summary>
        /// Name of the property
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Deletes the property item
        /// </summary>
        /// <param name="MappingHolder">Mappings holder</param>
        /// <param name="Object">Object</param>
        /// <param name="PropertyID">Property ID</param>
        /// <returns>True if it is saved, false otherwise</returns>
        bool DeleteValue(MappingHolder MappingHolder, dynamic Object, string PropertyID);

        /// <summary>
        /// Gets the property as a Dynamo object or list of dynamo objects
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Mappings">Mappings holder</param>
        /// <returns>The property</returns>
        dynamic GetValue(MappingHolder Mappings, dynamic Object);

        /// <summary>
        /// Saves the property item
        /// </summary>
        /// <param name="MappingHolder">Mapping holder</param>
        /// <param name="Object">Object</param>
        /// <param name="Models">Models</param>
        /// <returns>True if it is saved, false otherwise</returns>
        bool SaveValue(MappingHolder MappingHolder, dynamic Object, IEnumerable<Dynamo> Models);
    }
}