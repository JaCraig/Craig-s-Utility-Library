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
using System.ComponentModel;
using System.Data;
using System.Linq.Expressions;
using Utilities.ORM.Manager.Mapper.Default;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

namespace Utilities.ORM.Manager.Mapper.Interfaces
{
    /// <summary>
    /// Class mapping interface
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    public interface IMapping<ClassType> : IMapping
        where ClassType : class,new()
    {
        /// <summary>
        /// Declares a property as an ID
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>the ID object</returns>
        ID<ClassType, DataType> ID<DataType>(Expression<Func<ClassType, DataType>> Expression);

        /// <summary>
        /// Many to many mapping
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the many to many property</param>
        /// <returns>The many to many object</returns>
        ManyToMany<ClassType, DataType> ManyToMany<DataType>(Expression<Func<ClassType, IEnumerable<DataType>>> Expression)
            where DataType : class,new();

        /// <summary>
        /// Many to many mapping
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the many to many property</param>
        /// <returns>The many to many object</returns>
        ListManyToMany<ClassType, DataType> ManyToMany<DataType>(Expression<Func<ClassType, List<DataType>>> Expression)
            where DataType : class,new();

        /// <summary>
        /// Many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>The many to one object</returns>
        ManyToOne<ClassType, DataType> ManyToOne<DataType>(Expression<Func<ClassType, DataType>> Expression)
            where DataType : class,new();

        /// <summary>
        /// IEnumerable many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointint to the property</param>
        /// <returns>The many to one object</returns>
        IEnumerableManyToOne<ClassType, DataType> ManyToOne<DataType>(Expression<Func<ClassType, IEnumerable<DataType>>> Expression)
            where DataType : class,new();

        /// <summary>
        /// IEnumerable many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointint to the property</param>
        /// <returns>The many to one object</returns>
        ListManyToOne<ClassType, DataType> ManyToOne<DataType>(Expression<Func<ClassType, List<DataType>>> Expression)
            where DataType : class,new();

        /// <summary>
        /// Sets a property as a map type
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>the map object</returns>
        Map<ClassType, DataType> Map<DataType>(Expression<Func<ClassType, DataType>> Expression)
            where DataType : class,new();

        /// <summary>
        /// Sets a property as a reference type
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>the reference object</returns>
        Reference<ClassType, DataType> Reference<DataType>(Expression<Func<ClassType, DataType>> Expression);
    }

    /// <summary>
    /// Mapping interface
    /// </summary>
    public interface IMapping
    {
        /// <summary>
        /// Database config type
        /// </summary>
        Type DatabaseConfigType { get; }

        /// <summary>
        /// Delete command
        /// </summary>
        string DeleteCommand { get; }

        /// <summary>
        /// Delete command type
        /// </summary>
        CommandType DeleteCommandType { get; }

        /// <summary>
        /// ID properties
        /// </summary>
        ICollection<IProperty> IDProperties { get; }

        /// <summary>
        /// Insert command
        /// </summary>
        string InsertCommand { get; }

        /// <summary>
        /// Insert command type
        /// </summary>
        CommandType InsertCommandType { get; }

        /// <summary>
        /// The object type associated with the mapping
        /// </summary>
        Type ObjectType { get; }

        /// <summary>
        /// Order that the mappings are initialized
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Prefix used for defining properties/table name
        /// </summary>
        string Prefix { get; }

        /// <summary>
        /// Properties list
        /// </summary>
        ICollection<IProperty> Properties { get; }

        /// <summary>
        /// Select all command
        /// </summary>
        string SelectAllCommand { get; }

        /// <summary>
        /// Select all command type
        /// </summary>
        CommandType SelectAllCommandType { get; }

        /// <summary>
        /// Select any command
        /// </summary>
        string SelectAnyCommand { get; }

        /// <summary>
        /// Select any command type
        /// </summary>
        CommandType SelectAnyCommandType { get; }

        /// <summary>
        /// Suffix used for defining properties/table name
        /// </summary>
        string Suffix { get; }

        /// <summary>
        /// Table name
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Update command
        /// </summary>
        string UpdateCommand { get; }

        /// <summary>
        /// Update command type
        /// </summary>
        CommandType UpdateCommandType { get; }

        /// <summary>
        /// Sets the default delete command
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>This</returns>
        IMapping SetDeleteCommand(string Command, CommandType CommandType);

        /// <summary>
        /// Sets the default insert command
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>This</returns>
        IMapping SetInsertCommand(string Command, CommandType CommandType);

        /// <summary>
        /// Sets the default select all command
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>This</returns>
        IMapping SetSelectAllCommand(string Command, CommandType CommandType);

        /// <summary>
        /// Sets the default select any command
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>This</returns>
        IMapping SetSelectAnyCommand(string Command, CommandType CommandType);

        /// <summary>
        /// Sets up the mapping
        /// </summary>
        /// <param name="Source">Source info</param>
        /// <param name="QueryProvider">Query provider</param>
        /// <param name="MappingProvider">Mapping provider</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void Setup(ISourceInfo Source, Utilities.ORM.Manager.Mapper.Manager MappingProvider, Utilities.ORM.Manager.QueryProvider.Manager QueryProvider);

        /// <summary>
        /// Sets the default update command
        /// </summary>
        /// <param name="Command">Command</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>This</returns>
        IMapping SetUpdateCommand(string Command, CommandType CommandType);
    }
}