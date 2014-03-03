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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataTypes;

namespace Ironman.Core.API.Manager.Interfaces
{
    /// <summary>
    /// API mapping interface
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    public interface IAPIMapping<ClassType>
        where ClassType : class,new()
    {
        /// <summary>
        /// Sets an ID property for the mapping
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>ID property object</returns>
        IAPIProperty<ClassType, DataType> ID<DataType>(Expression<Func<ClassType, DataType>> Expression);

        /// <summary>
        /// Sets an Map property for the mapping
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>Map property object</returns>
        IAPIProperty<ClassType, DataType> Map<DataType>(Expression<Func<ClassType, DataType>> Expression);

        /// <summary>
        /// Sets an Map property that is an IEnumerable for the mapping
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>List Map property object</returns>
        IAPIProperty<ClassType, IEnumerable<DataType>> MapList<DataType>(Expression<Func<ClassType, IEnumerable<DataType>>> Expression);

        /// <summary>
        /// Sets a Reference property for the mapping
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>Reference property object</returns>
        IAPIProperty<ClassType, DataType> Reference<DataType>(Expression<Func<ClassType, DataType>> Expression);

        /// <summary>
        /// Sets the function to get all items of the specified type
        /// </summary>
        /// <param name="Value">Function used to get all items of the specified type</param>
        /// <returns>This</returns>
        IAPIMapping<ClassType> SetAll(Func<IEnumerable<ClassType>> Value);

        /// <summary>
        /// Sets the function to get a specific item of the specified type
        /// </summary>
        /// <param name="Value">Function used to get a specific item of the specified type</param>
        /// <returns>This</returns>
        IAPIMapping<ClassType> SetAny(Func<string, ClassType> Value);

        /// <summary>
        /// Sets a function that determines if the object can be deleted
        /// </summary>
        /// <param name="Value">
        /// Function to use to determine if an object can be deleted (takes the object and returns
        /// true if it can be deleted, false otherwise)
        /// </param>
        /// <returns>This</returns>
        IAPIMapping<ClassType> SetCanDelete(Func<ClassType, bool> Value);

        /// <summary>
        /// Sets a function that determines if the object can be seen
        /// </summary>
        /// <param name="Value">
        /// Function to use to determine if an object can be seen (takes the object and returns true
        /// if it can be seen, false otherwise)
        /// </param>
        /// <returns>This</returns>
        IAPIMapping<ClassType> SetCanGet(Func<ClassType, bool> Value);

        /// <summary>
        /// Sets a function that determines if the object can be saved
        /// </summary>
        /// <param name="Value">
        /// Function to use to determine if an object can be saved (takes the object and returns
        /// true if it can be saved, false otherwise)
        /// </param>
        /// <returns>This</returns>
        IAPIMapping<ClassType> SetCanSave(Func<ClassType, bool> Value);

        /// <summary>
        /// Sets the delete function that is used by the mapping
        /// </summary>
        /// <param name="Value">
        /// Function to use to delete an object (takes the object and returns true if it is deleted,
        /// false otherwise)
        /// </param>
        /// <returns>This</returns>
        IAPIMapping<ClassType> SetDelete(Func<ClassType, bool> Value);

        /// <summary>
        /// Sets the save function that is used by the mapping
        /// </summary>
        /// <param name="Value">
        /// Function to use to save an object (takes the object and returns true if it is saved,
        /// false otherwise)
        /// </param>
        /// <returns>This</returns>
        IAPIMapping<ClassType> SetSave(Func<ClassType, bool> Value);
    }

    /// <summary>
    /// API mapping interface
    /// </summary>
    public interface IAPIMapping
    {
        /// <summary>
        /// Name of the model class
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Type of the model
        /// </summary>
        Type ObjectType { get; }

        /// <summary>
        /// Whitelisted properties for the mapping
        /// </summary>
        ICollection<IAPIProperty> Properties { get; }

        /// <summary>
        /// Version of the API this is used for
        /// </summary>
        IEnumerable<int> Versions { get; }

        /// <summary>
        /// Gets all items of the mapped type
        /// </summary>
        /// <param name="EmbeddedProperties">Properties to embed</param>
        /// <param name="Mappings">Mappings</param>
        /// <returns>All items of the mapped type</returns>
        IEnumerable<Dynamo> All(MappingHolder Mappings, params string[] EmbeddedProperties);

        /// <summary>
        /// Gets the item specified of the mapped type
        /// </summary>
        /// <param name="ID">ID of the item</param>
        /// <param name="EmbeddedProperties">Properties to embed</param>
        /// <param name="Mappings">Mappings</param>
        /// <returns>The item specified</returns>
        Dynamo Any(string ID, MappingHolder Mappings, params string[] EmbeddedProperties);

        /// <summary>
        /// Deletes the specified object
        /// </summary>
        /// <param name="ID">ID value</param>
        /// <returns>True if it is deleted, false otherwise</returns>
        bool Delete(string ID);

        /// <summary>
        /// Gets a specific property from an object
        /// </summary>
        /// <param name="ID">ID of the item</param>
        /// <param name="EmbeddedProperties">Properties to embed</param>
        /// <param name="Mappings">Mappings</param>
        /// <param name="Property">Property name</param>
        /// <returns>The property specified</returns>
        dynamic GetProperty(string ID, MappingHolder Mappings, string Property, params string[] EmbeddedProperties);

        /// <summary>
        /// Saves the specified object
        /// </summary>
        /// <param name="Object">Object to save</param>
        /// <returns>True if it is saved, false otherwise</returns>
        bool Save(Dynamo Object);
    }
}