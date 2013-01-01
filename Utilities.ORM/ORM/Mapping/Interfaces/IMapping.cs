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
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using Utilities.ORM.QueryProviders.Interfaces;
using Utilities.SQL.MicroORM;
#endregion

namespace Utilities.ORM.Mapping.Interfaces
{
    /// <summary>
    /// Class mapping interface
    /// </summary>
    /// <typeparam name="ClassType">Class type</typeparam>
    public interface IMapping<ClassType>
    {
        #region Functions

        #region ID

        /// <summary>
        /// Declares a property as an ID
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>the ID object</returns>
        IID<ClassType, DataType> ID<DataType>(Expression<Func<ClassType, DataType>> Expression);

        /// <summary>
        /// Declares a property as an ID
        /// </summary>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>the ID object</returns>
        IID<ClassType, string> ID(Expression<Func<ClassType, string>> Expression);

        #endregion

        #region Reference

        /// <summary>
        /// Sets a property as a reference type
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>the reference object</returns>
        IReference<ClassType, DataType> Reference<DataType>(Expression<Func<ClassType, DataType>> Expression);

        /// <summary>
        /// Sets a property as a reference type
        /// </summary>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>the reference object</returns>
        IReference<ClassType, string> Reference(Expression<Func<ClassType, string>> Expression);

        #endregion

        #region Map

        /// <summary>
        /// Sets a property as a map type
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>the map object</returns>
        IMap<ClassType, DataType> Map<DataType>(Expression<Func<ClassType, DataType>> Expression) where DataType : class,new();

        #endregion

        #region ManyToOne

        /// <summary>
        /// Many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the property</param>
        /// <returns>The many to one object</returns>
        IManyToOne<ClassType, DataType> ManyToOne<DataType>(Expression<Func<ClassType, DataType>> Expression) where DataType : class,new();

        /// <summary>
        /// IEnumerable many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointint to the property</param>
        /// <returns>The many to one object</returns>
        IIEnumerableManyToOne<ClassType, DataType> ManyToOne<DataType>(Expression<Func<ClassType, IEnumerable<DataType>>> Expression) where DataType : class,new();

        /// <summary>
        /// IEnumerable many to one
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointint to the property</param>
        /// <returns>The many to one object</returns>
        IListManyToOne<ClassType, DataType> ManyToOne<DataType>(Expression<Func<ClassType, List<DataType>>> Expression) where DataType : class,new();

        #endregion

        #region ManyToMany

        /// <summary>
        /// Many to many mapping
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the many to many property</param>
        /// <returns>The many to many object</returns>
        IManyToMany<ClassType, DataType> ManyToMany<DataType>(Expression<Func<ClassType, IEnumerable<DataType>>> Expression) where DataType : class,new();

        /// <summary>
        /// Many to many mapping
        /// </summary>
        /// <typeparam name="DataType">Data type</typeparam>
        /// <param name="Expression">Expression pointing to the many to many property</param>
        /// <returns>The many to many object</returns>
        IListManyToMany<ClassType, DataType> ManyToMany<DataType>(Expression<Func<ClassType, List<DataType>>> Expression) where DataType : class,new();

        #endregion

        #region Any

        /// <summary>
        /// Sets the default command to call when Any is called (optional)
        /// </summary>
        /// <param name="Command">Command to call</param>
        /// <param name="CommandType">Command type</param>
        void Any(string Command, CommandType CommandType);

        #endregion

        #region All

        /// <summary>
        /// Sets the default command to call when All is called (optional)
        /// </summary>
        /// <param name="Command">Command to call</param>
        /// <param name="CommandType">Command type</param>
        void All(string Command, CommandType CommandType);

        #endregion

        //#region SetupValidation

        ///// <summary>
        ///// Used to set up validation, using the class used internally by the system
        ///// </summary>
        ///// <param name="Validator">Validator</param>
        //void SetupValidation(Validator<ClassType> Validator);

        //#endregion

        #endregion
    }

    /// <summary>
    /// Mapping interface
    /// </summary>
    public interface IMapping
    {
        #region Functions

        /// <summary>
        /// Internal function used to add to
        /// the query provider
        /// </summary>
        /// <param name="Database">Database object</param>
        void AddToQueryProvider(IDatabase Database);

        /// <summary>
        /// Should be overwritten to initialize values in the 
        /// database. This is run after the initial setup but prior to
        /// returning to the user.
        /// </summary>
        void Initialize();

        #endregion

        #region Properties

        /// <summary>
        /// Table name
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Suffix used for defining properties/table name
        /// </summary>
        string Suffix { get; }

        /// <summary>
        /// Prefix used for defining properties/table name
        /// </summary>
        string Prefix { get;  }

        /// <summary>
        /// Database config type
        /// </summary>
        Type DatabaseConfigType { get; }

        /// <summary>
        /// Properties list
        /// </summary>
        List<IProperty> Properties { get; }

        /// <summary>
        /// ID property
        /// </summary>
        IProperty IDProperty { get; set; }

        /// <summary>
        /// Mapping manager
        /// </summary>
        IMappingManager Manager { get; set; }

        /// <summary>
        /// The object type associated with the mapping
        /// </summary>
        Type ObjectType { get; }

        /// <summary>
        /// Any command
        /// </summary>
        Command AnyCommand { get; set; }

        /// <summary>
        /// All command
        /// </summary>
        Command AllCommand { get; set; }

        /// <summary>
        /// Order that the mappings are initialized
        /// </summary>
        int Order { get; set; }

        #endregion
    }
}
