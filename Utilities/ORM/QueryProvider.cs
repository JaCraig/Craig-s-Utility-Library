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
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Globalization;
using Utilities.DataTypes;
using Utilities.DataTypes.Comparison;
using Utilities.ORM.Manager;
using Utilities.ORM.Manager.QueryProvider.Interfaces;

#endregion Usings

namespace Utilities.ORM
{
    /// <summary>
    /// Query provider
    /// </summary>
    public static class QueryProvider
    {
        /// <summary>
        /// Query manager
        /// </summary>
        private static Manager.QueryProvider.Manager QueryManager { get { return IoC.Manager.Bootstrapper.Resolve<Manager.QueryProvider.Manager>(); } }

        /// <summary>
        /// Source manager
        /// </summary>
        private static Manager.SourceProvider.Manager SourceManager { get { return IoC.Manager.Bootstrapper.Resolve<Manager.SourceProvider.Manager>(); } }

        /// <summary>
        /// Returns all objects based on the parameters provided
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Parameters">Parameters</param>
        /// <returns>The list of objects requested</returns>
        public static IEnumerable<ObjectType> All<ObjectType>(params IParameter[] Parameters)
            where ObjectType : class,new()
        {
            return new Session().All<ObjectType>(Parameters);
        }

        /// <summary>
        /// Returns an object based on the parameters provided
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Parameters">Parameters</param>
        /// <returns>An object requested</returns>
        public static ObjectType Any<ObjectType>(params IParameter[] Parameters)
            where ObjectType : class,new()
        {
            return new Session().Any<ObjectType>(Parameters);
        }

        /// <summary>
        /// Creates a batch object that can be used to run ad hoc queries
        /// </summary>
        /// <param name="ConnectionString">
        /// Connection string (can be the name of connection string in config file or the actual
        /// connection string)
        /// </param>
        /// <returns>An appropriate batch object</returns>
        public static IBatch Batch(string ConnectionString)
        {
            return QueryManager.Batch(SourceManager.GetSource(ConnectionString));
        }

        /// <summary>
        /// Deletes an object
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="Object">Object to delete</param>
        public static void Delete<ObjectType>(ObjectType Object)
            where ObjectType : class,new()
        {
            new Session().Delete<ObjectType>(Object);
            // new Session().PageCount new Session().Paged
        }

        /// <summary>
        /// Gets the page count based on the page size specified
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="PageSize">Page size</param>
        /// <param name="Parameters">Parameters</param>
        /// <returns>The number of pages</returns>
        public static int PageCount<ObjectType>(int PageSize = 25, params IParameter[] Parameters)
            where ObjectType : class,new()
        {
            return new Session().PageCount<ObjectType>(PageSize, Parameters);
        }

        /// <summary>
        /// Gets a specific page of objects
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <param name="PageSize">Page size</param>
        /// <param name="Parameters">Parameters</param>
        /// <param name="CurrentPage">Current page (0 based)</param>
        /// <returns>The objects specified</returns>
        public static IEnumerable<ObjectType> PageCount<ObjectType>(int PageSize = 25, int CurrentPage = 0, params IParameter[] Parameters)
            where ObjectType : class,new()
        {
            return new Session().Paged<ObjectType>(PageSize, CurrentPage, Parameters);
        }

        /// <summary>
        /// Saves an object
        /// </summary>
        /// <typeparam name="ObjectType">Object type</typeparam>
        /// <typeparam name="PrimaryKeyType">Primary key type</typeparam>
        /// <param name="Object">Object</param>
        public static void Save<ObjectType, PrimaryKeyType>(ObjectType Object)
            where ObjectType : class,new()
        {
            new Session().Save<ObjectType, PrimaryKeyType>(Object);
        }
    }
}