/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Linq;
using System.Text;
using Utilities.DataTypes.Patterns;
using System.Linq.Expressions;
using Utilities.SQL.MicroORM.Enums;
#endregion

namespace Utilities.SQL.MicroORM.Interfaces
{
    /// <summary>
    /// Mapping interface
    /// </summary>
    public interface IMapping : IDisposable
    {
    }

    /// <summary>
    /// Mapping interface
    /// </summary>
    public interface IMapping<ClassType> : IFluentInterface
    {
        #region Functions

        /// <summary>
        /// Maps a property to a database property name (required to actually get data from the database)
        /// </summary>
        /// <typeparam name="DataType">Data type of the property</typeparam>
        /// <param name="Property">Property to add a mapping for</param>
        /// <param name="DatabasePropertyName">Property name</param>
        /// <param name="Mode">This determines if the mapping should have read or write access</param>
        /// <returns>This mapping</returns>
        IMapping<ClassType> Map<DataType>(Expression<Func<ClassType, DataType>> Property, string DatabasePropertyName, Mode Mode = Mode.Read|Mode.Write);

        /// <summary>
        /// Maps a property to a database property name (required to actually get data from the database)
        /// </summary>
        /// <param name="Property">Property to add a mapping for</param>
        /// <param name="DatabasePropertyName">Property name</param>
        /// <param name="Length">Max length of the string</param>
        /// <param name="Mode">This determines if the mapping should have read or write access</param>
        /// <returns>This mapping</returns>
        IMapping<ClassType> Map(Expression<Func<ClassType, string>> Property, string DatabasePropertyName, int Length, Mode Mode = Mode.Read|Mode.Write);

        #endregion
    }
}
