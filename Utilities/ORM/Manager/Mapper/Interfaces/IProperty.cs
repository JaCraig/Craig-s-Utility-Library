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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Utilities.DataTypes;
using Utilities.DataTypes.DataMapper.Interfaces;
using Utilities.DataTypes.Patterns;
using Utilities.DataTypes.Patterns.BaseClasses;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Utilities.ORM.Manager.Schema.Interfaces;

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
    public interface IProperty<ClassType, DataType> : IProperty
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
        /// Derived field name
        /// </summary>
        string DerivedFieldName { get; }

        /// <summary>
        /// Expression pointing to the property
        /// </summary>
        Expression<Func<ClassType, DataType>> Expression { get; }

        /// <summary>
        /// If the property is a class, this is the foreign mapping
        /// </summary>
        IMapping ForeignMapping { get; }

        /// <summary>
        /// Mapping
        /// </summary>
        IMapping Mapping { get; }

        /// <summary>
        /// Property name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Property type
        /// </summary>
        Type Type { get; }
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
        /// Field name
        /// </summary>
        string FieldName { get; }

        /// <summary>
        /// Index
        /// </summary>
        bool Index { get; }

        /// <summary>
        /// Max length
        /// </summary>
        int MaxLength { get; }

        /// <summary>
        /// Not null
        /// </summary>
        bool NotNull { get; }

        /// <summary>
        /// Table name
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Unique
        /// </summary>
        bool Unique { get; }
    }
}