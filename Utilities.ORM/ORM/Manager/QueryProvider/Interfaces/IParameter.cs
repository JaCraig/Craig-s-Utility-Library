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

using System.Data;
using System.Data.Common;

namespace Utilities.ORM.Manager.QueryProvider.Interfaces
{
    /// <summary>
    /// Parameter interface
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public interface IParameter<T> : IParameter
    {
        /// <summary>
        /// The value that the parameter is associated with
        /// </summary>
        T Value { get; set; }
    }

    /// <summary>
    /// Parameter interface
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// Database type
        /// </summary>
        DbType DatabaseType { get; set; }

        /// <summary>
        /// Direction of the parameter
        /// </summary>
        ParameterDirection Direction { get; set; }

        /// <summary>
        /// The name that the parameter goes by
        /// </summary>
        string ID { get; set; }

        /// <summary>
        /// Gets the internal value.
        /// </summary>
        /// <value>
        /// The internal value.
        /// </value>
        object InternalValue { get; }

        /// <summary>
        /// Adds this parameter to the SQLHelper
        /// </summary>
        /// <param name="Helper">SQLHelper</param>
        void AddParameter(DbCommand Helper);

        /// <summary>
        /// Finds itself in the string command and adds the value
        /// </summary>
        /// <param name="Command">Command to add to</param>
        /// <returns>The resulting string</returns>
        string AddParameter(string Command);

        /// <summary>
        /// Creates a copy of the parameter
        /// </summary>
        /// <param name="Suffix">Suffix to add to the parameter (for batching purposes)</param>
        /// <returns>A copy of the parameter</returns>
        IParameter CreateCopy(string Suffix);
    }
}