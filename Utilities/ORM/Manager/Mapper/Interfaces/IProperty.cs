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

#region Usings

using System;
using System.Data;
using System.Linq.Expressions;
using Utilities.DataTypes.Patterns;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;

#endregion Usings

namespace Utilities.ORM.Manager.Mapper.Interfaces
{
    /// <summary>
    /// Property interface
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type</typeparam>
    /// <typeparam name="ReturnType">Return type</typeparam>
    public interface IProperty<ClassType, DataType, ReturnType> : IFluentInterface
        where ClassType : class,new()
        where ReturnType : IProperty<ClassType, DataType, ReturnType>
    {
        /// <summary>
        /// Turns on autoincrement for this property
        /// </summary>
        /// <returns>This</returns>
        ReturnType SetAutoIncrement();

        /// <summary>
        /// Turns on cascade for saving/deleting
        /// </summary>
        /// <returns>this</returns>
        ReturnType SetCascade();

        /// <summary>
        /// Sets the default value of the property
        /// </summary>
        /// <param name="Value">Default value</param>
        /// <returns>This IProperty object</returns>
        ReturnType SetDefaultValue(Func<DataType> Value);

        /// <summary>
        /// Sets the name of the field in the database
        /// </summary>
        /// <param name="FieldName">Field name</param>
        /// <returns>this</returns>
        ReturnType SetFieldName(string FieldName);

        /// <summary>
        /// Turns on indexing for this property
        /// </summary>
        /// <returns>This</returns>
        ReturnType SetIndex();

        /// <summary>
        /// Allows you to load a property based on a specified command
        /// </summary>
        /// <param name="Command">Command used to load the property</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>this</returns>
        ReturnType SetLoadUsingCommand(string Command, CommandType CommandType);

        /// <summary>
        /// Sets the max length for the property (or precision for items like decimal values)
        /// </summary>
        /// <param name="MaxLength">Max length</param>
        /// <returns>this</returns>
        ReturnType SetMaxLength(int MaxLength);

        /// <summary>
        /// Sets the field such that null values are not allowed
        /// </summary>
        /// <returns>this</returns>
        ReturnType SetNotNull();

        /// <summary>
        /// Set database table name
        /// </summary>
        /// <param name="TableName">Table name</param>
        /// <returns>this</returns>
        ReturnType SetTableName(string TableName);

        /// <summary>
        /// Ensures the field is unique
        /// </summary>
        /// <returns>this</returns>
        ReturnType SetUnique();
    }

    /// <summary>
    /// Property interface
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType"></typeparam>
    public interface IProperty<ClassType, DataType> : IProperty<ClassType>
        where ClassType : class,new()
    {
        /// <summary>
        /// Compiled version of the expression
        /// </summary>
        Func<ClassType, DataType> CompiledExpression { get; }

        /// <summary>
        /// Default value for this property
        /// </summary>
        Func<DataType> DefaultValue { get; }

        /// <summary>
        /// Expression pointing to the property
        /// </summary>
        Expression<Func<ClassType, DataType>> Expression { get; }
    }

    /// <summary>
    /// Property interface
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    public interface IProperty<ClassType> : IProperty
        where ClassType : class,new()
    {
        /// <summary>
        /// Does a cascade delete of an object for this property
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <returns>Batch object with the appropriate commands</returns>
        IBatch CascadeDelete(ClassType Object, ISourceInfo Source);

        /// <summary>
        /// Called to create a batch that deletes items from the joining tables
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <returns>Batch object with the appropriate commands</returns>
        IBatch CascadeJoinsDelete(ClassType Object, ISourceInfo Source);

        /// <summary>
        /// Called to create a batch that saves items from the joining tables
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <returns>Batch object with the appropriate commands</returns>
        IBatch CascadeJoinsSave(ClassType Object, ISourceInfo Source);

        /// <summary>
        /// Does a cascade save of an object for this property
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <returns>Batch object with the appropriate commands</returns>
        IBatch CascadeSave(ClassType Object, ISourceInfo Source);

        /// <summary>
        /// Gets the property's value from the object sent in
        /// </summary>
        /// <param name="Object">Object to get the value from</param>
        /// <param name="Source">Source info</param>
        /// <returns>The value of the property</returns>
        object GetValue(ClassType Object);

        /// <summary>
        /// Called to create a batch that deletes items from the joining table
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <returns>Batch object with the appropriate commands</returns>
        IBatch JoinsDelete(ClassType Object, ISourceInfo Source);

        /// <summary>
        /// Called to create a batch that saves items from the joining table
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="Source">Source info</param>
        /// <returns>Batch object with the appropriate commands</returns>
        IBatch JoinsSave(ClassType Object, ISourceInfo Source);
    }

    /// <summary>
    /// Property interface
    /// </summary>
    public interface IProperty
    {
        /// <summary>
        /// Auto increment
        /// </summary>
        bool AutoIncrement { get; }

        /// <summary>
        /// Cascade
        /// </summary>
        bool Cascade { get; }

        /// <summary>
        /// Derived field name
        /// </summary>
        string DerivedFieldName { get; }

        /// <summary>
        /// Field name
        /// </summary>
        string FieldName { get; }

        /// <summary>
        /// If the property is a class, this is the foreign mapping
        /// </summary>
        IMapping ForeignMapping { get; }

        /// <summary>
        /// Index
        /// </summary>
        bool Index { get; }

        /// <summary>
        /// Load command
        /// </summary>
        string LoadCommand { get; }

        /// <summary>
        /// Load command type
        /// </summary>
        CommandType LoadCommandType { get; }

        /// <summary>
        /// Mapping
        /// </summary>
        IMapping Mapping { get; }

        /// <summary>
        /// Max length
        /// </summary>
        int MaxLength { get; }

        /// <summary>
        /// Property name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Not null
        /// </summary>
        bool NotNull { get; }

        /// <summary>
        /// Table name
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Property type
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Unique
        /// </summary>
        bool Unique { get; }

        /// <summary>
        /// Sets up the property (used internally)
        /// </summary>
        /// <param name="MappingProvider">Mapping provider</param>
        /// <param name="QueryProvider">Query provider</param>
        /// <param name="Source">Source to use</param>
        void Setup(ISourceInfo Source, Mapper.Manager MappingProvider, QueryProvider.Manager QueryProvider);
    }
}