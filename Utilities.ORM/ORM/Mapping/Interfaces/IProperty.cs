/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using Utilities.ORM.QueryProviders.Interfaces;
using Utilities.SQL.Interfaces;
using Utilities.SQL.MicroORM;
using Utilities.SQL;
using System.Collections.Generic;
#endregion

namespace Utilities.ORM.Mapping.Interfaces
{
    /// <summary>
    /// Property interface
    /// </summary>
    public interface IProperty
    {
        #region Functions

        /// <summary>
        /// Sets up the various load commands
        /// </summary>
        void SetupLoadCommands();

        /// <summary>
        /// Gets the property as an object
        /// </summary>
        /// <param name="Object">Object to get the property from</param>
        /// <returns>The property as an object</returns>
        object GetAsObject(object Object);

        #endregion

        #region Properties

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
        /// Foreign key link
        /// </summary>
        IMapping ForeignKey { get; set; }

        /// <summary>
        /// Index
        /// </summary>
        bool Index { get; }

        /// <summary>
        /// Max length (used in strings)
        /// </summary>
        int MaxLength { get; }

        /// <summary>
        /// Name of the property
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Can the item be null?
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
        /// Is this a unique field?
        /// </summary>
        bool Unique { get; }

        /// <summary>
        /// Command used to load the property
        /// </summary>
        Command CommandToLoad { get; }

        #endregion
    }

    /// <summary>
    /// Property interface
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    public interface IProperty<ClassType>
        where ClassType : class,new()
    {
        #region Functions

        /// <summary>
        /// Adds to query provider
        /// </summary>
        /// <param name="Database">Database associated with mapping</param>
        /// <param name="Mapping">Mapping to add the property to</param>
        void AddToQueryProvider(IDatabase Database, Mapping<ClassType> Mapping);

        /// <summary>
        /// Gets the property as a parameter
        /// </summary>
        /// <param name="Object">Object to get the property from</param>
        /// <returns>The property as a parameter</returns>
        IParameter GetAsParameter(ClassType Object);

        /// <summary>
        /// Gets the property as an object
        /// </summary>
        /// <param name="Object">Object to get the property from</param>
        /// <returns>The property as an object</returns>
        object GetAsObject(ClassType Object);

        /// <summary>
        /// Cascades the save
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">Micro ORM</param>
        void CascadeSave(ClassType Object, MicroORM MicroORM);

        /// <summary>
        /// Cascades the delete
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">Micro ORM</param>
        void CascadeDelete(ClassType Object, MicroORM MicroORM);

        /// <summary>
        /// Cascade the deleting of joins
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">Micro ORM</param>
        /// <returns>Returns the list of commands</returns>
        IEnumerable<Command> CascadeJoinsDelete(ClassType Object, MicroORM MicroORM);

        /// <summary>
        /// Cascade the saving of joins
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">MicroORM</param>
        /// <returns>Returns the list of commands</returns>
        IEnumerable<Command> CascadeJoinsSave(ClassType Object, MicroORM MicroORM);

        /// <summary>
        /// Deletes the joins
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">Micro ORM</param>
        /// <returns>Returns the list of commands</returns>
        IEnumerable<Command> JoinsDelete(ClassType Object, MicroORM MicroORM);

        /// <summary>
        /// Saves the joins
        /// </summary>
        /// <param name="Object">Object</param>
        /// <param name="MicroORM">MicroORM</param>
        /// <returns>Returns the list of commands</returns>
        IEnumerable<Command> JoinsSave(ClassType Object, MicroORM MicroORM);

        #endregion
    }

    /// <summary>
    /// Property interface
    /// </summary>
    public interface IProperty<ClassType, DataType>
    {
        #region Properties

        /// <summary>
        /// Default value for this property
        /// </summary>
        Func<DataType> DefaultValue { get; }

        /// <summary>
        /// Expression pointing to the property
        /// </summary>
        Expression<Func<ClassType, DataType>> Expression { get; }

        /// <summary>
        /// Compiled version of the expression
        /// </summary>
        Func<ClassType, DataType> CompiledExpression { get; }

        #endregion
    }

    /// <summary>
    /// Property interface
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    /// <typeparam name="DataType">Data type of the property</typeparam>
    /// <typeparam name="ReturnType">Return type</typeparam>
    public interface IProperty<ClassType, DataType, ReturnType> : IFluentInterface
    {
        #region Functions

        /// <summary>
        /// Sets the default value of the property
        /// </summary>
        /// <param name="DefaultValue">Default value</param>
        /// <returns>This IProperty object</returns>
        ReturnType SetDefaultValue(Func<DataType> DefaultValue);

        /// <summary>
        /// Sets the field to not null
        /// </summary>
        /// <returns>this</returns>
        ReturnType DoNotAllowNullValues();

        /// <summary>
        /// Sets the unique field to true
        /// </summary>
        /// <returns>this</returns>
        ReturnType ThisShouldBeUnique();

        /// <summary>
        /// Turns on indexing for this property
        /// </summary>
        /// <returns>This</returns>
        ReturnType TurnOnIndexing();

        /// <summary>
        /// Turns on autoincrement for this property
        /// </summary>
        /// <returns>This</returns>
        ReturnType TurnOnAutoIncrement();

        /// <summary>
        /// Sets the name of the field in the database
        /// </summary>
        /// <param name="FieldName">Field name</param>
        /// <returns>this</returns>
        ReturnType SetFieldName(string FieldName);

        /// <summary>
        /// Set database table name
        /// </summary>
        /// <param name="TableName">Table name</param>
        /// <returns>this</returns>
        ReturnType SetTableName(string TableName);

        /// <summary>
        /// Turns on cascade for saving/deleting
        /// </summary>
        /// <returns>this</returns>
        ReturnType TurnOnCascade();

        /// <summary>
        /// Sets the max length for the string
        /// </summary>
        /// <param name="MaxLength">Max length</param>
        /// <returns>this</returns>
        ReturnType SetMaxLength(int MaxLength);

        /// <summary>
        /// Allows you to load a property based on a specified command
        /// </summary>
        /// <param name="Command">Command used to load the property</param>
        /// <param name="CommandType">Command type</param>
        /// <returns>this</returns>
        ReturnType LoadUsingCommand(string Command, CommandType CommandType);

        #endregion
    }
}
