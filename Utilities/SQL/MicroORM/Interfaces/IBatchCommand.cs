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
using System.Linq.Expressions;
using Utilities.DataTypes.Patterns;
using Utilities.SQL.MicroORM.Enums;
using System.Data;
using System.Collections.Generic;
using Utilities.SQL.Interfaces;
#endregion

namespace Utilities.SQL.MicroORM.Interfaces
{
    /// <summary>
    /// Batch command interface
    /// </summary>
    public interface IBatchCommand
    {
        #region Properties

        /// <summary>
        /// Number of commands being batched
        /// </summary>
        int CommandCount { get; }

        #endregion

        #region Functions

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Commands">Commands to add</param>
        /// <returns>This</returns>
        IBatchCommand AddCommands(params Command[] Commands);

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="ParameterPrefix">Parameter prefix to use</param>
        /// <param name="Parameters">Parameters to add</param>
        /// <returns>This</returns>
        IBatchCommand AddCommand(string Command, CommandType CommandType, string ParameterPrefix, params object[] Parameters);

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters to add</param>
        /// <returns>This</returns>
        IBatchCommand AddCommand(string Command, CommandType CommandType, params IParameter[] Parameters);

        #endregion
    }
}