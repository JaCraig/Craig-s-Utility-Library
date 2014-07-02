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
using System.Data;
using Utilities.ORM.Manager.QueryProvider.Default;

namespace Utilities.ORM.Manager.QueryProvider.Interfaces
{
    /// <summary>
    /// Batch interface, used to add commands and executes them
    /// </summary>
    public interface IBatch
    {
        /// <summary>
        /// Number of commands being batched
        /// </summary>
        int CommandCount { get; }

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="CallBack">Callback action</param>
        /// <param name="Object">Object used in the callback action</param>
        /// <returns>This</returns>
        IBatch AddCommand(Action<Command, IList<dynamic>> CallBack, object Object, CommandType CommandType, string Command);

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters to add</param>
        /// <param name="CallBack">Callback action</param>
        /// <param name="Object">Object used in the callback action</param>
        /// <returns>This</returns>
        IBatch AddCommand(Action<Command, IList<dynamic>> CallBack, object Object, string Command, CommandType CommandType, params object[] Parameters);

        /// <summary>
        /// Adds a command to be batched
        /// </summary>
        /// <param name="Command">Command (SQL or stored procedure) to run</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters to add</param>
        /// <param name="CallBack">Callback action</param>
        /// <param name="Object">Object used in the callback action</param>
        /// <returns>This</returns>
        IBatch AddCommand(Action<Command, IList<dynamic>> CallBack, object Object, string Command, CommandType CommandType, params IParameter[] Parameters);

        /// <summary>
        /// Adds a batch's commands to the current batch
        /// </summary>
        /// <param name="Batch">Batch to add</param>
        /// <returns>This</returns>
        IBatch AddCommand(IBatch Batch);

        /// <summary>
        /// Executes the commands and returns the results
        /// </summary>
        /// <returns>The results of the batched commands</returns>
        IList<IList<dynamic>> Execute();

        /// <summary>
        /// Removes duplicate commands from the batch
        /// </summary>
        /// <returns>This</returns>
        IBatch RemoveDuplicateCommands();
    }
}