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

using System.Collections.Generic;

namespace Utilities.ORM.Manager.Schema.Interfaces
{
    /// <summary>
    /// Interface for source objects (like databases)
    /// </summary>
    public interface ISource
    {
        /// <summary>
        /// List of functions
        /// </summary>
        ICollection<IFunction> Functions { get; }

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// List of stored procedures
        /// </summary>
        ICollection<ITable> StoredProcedures { get; }

        /// <summary>
        /// List of tables
        /// </summary>
        ICollection<ITable> Tables { get; }

        /// <summary>
        /// List of views
        /// </summary>
        ICollection<ITable> Views { get; }

        /// <summary>
        /// Returns a table with the given name
        /// </summary>
        /// <param name="Name">Table name</param>
        /// <returns>The table specified</returns>
        ITable this[string Name] { get; }

        /// <summary>
        /// Adds a function to the database
        /// </summary>
        /// <param name="Name">Function name</param>
        /// <param name="Definition">Function definition</param>
        IFunction AddFunction(string Name, string Definition);

        /// <summary>
        /// Adds a stored procedure to the database
        /// </summary>
        /// <param name="ProcedureName">Procedure name</param>
        /// <param name="Definition">Definition</param>
        ITable AddStoredProcedure(string ProcedureName, string Definition);

        /// <summary>
        /// Adds a table to the database
        /// </summary>
        /// <param name="TableName">Table name</param>
        ITable AddTable(string TableName);

        /// <summary>
        /// Adds a view to the database
        /// </summary>
        /// <param name="ViewName">View name</param>
        ITable AddView(string ViewName);
    }
}