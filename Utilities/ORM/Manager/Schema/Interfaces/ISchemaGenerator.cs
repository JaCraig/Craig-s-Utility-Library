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

using System.Collections.Generic;

#endregion Usings

namespace Utilities.ORM.Manager.Schema.Interfaces
{
    /// <summary>
    /// Schema generator interface
    /// </summary>
    public interface ISchemaGenerator
    {
        /// <summary>
        /// Provider name associated with the schema generator
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Generates a list of commands used to modify the source. If it does not exist prior, the
        /// commands will create the source from scratch. Otherwise the commands will only add new
        /// fields, tables, etc. It does not delete old fields.
        /// </summary>
        /// <param name="DesiredStructure">Desired source structure</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>List of commands generated</returns>
        IEnumerable<string> GenerateSchema(ISource DesiredStructure, string ConnectionString);

        /// <summary>
        /// Gets the structure of a source
        /// </summary>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>The source structure</returns>
        ISource GetSourceStructure(string ConnectionString);

        /// <summary>
        /// Checks if a source exists
        /// </summary>
        /// <param name="Source">Source to check</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>True if it exists, false otherwise</returns>
        bool SourceExists(string Source, string ConnectionString);

        /// <summary>
        /// Checks if a stored procedure exists
        /// </summary>
        /// <param name="StoredProcedure">Stored procedure to check</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>True if it exists, false otherwise</returns>
        bool StoredProcedureExists(string StoredProcedure, string ConnectionString);

        /// <summary>
        /// Checks if a table exists
        /// </summary>
        /// <param name="Table">Table to check</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>True if it exists, false otherwise</returns>
        bool TableExists(string Table, string ConnectionString);

        /// <summary>
        /// Checks if a trigger exists
        /// </summary>
        /// <param name="Trigger">Trigger to check</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>True if it exists, false otherwise</returns>
        bool TriggerExists(string Trigger, string ConnectionString);

        /// <summary>
        /// Checks if a view exists
        /// </summary>
        /// <param name="View">View to check</param>
        /// <param name="ConnectionString">Connection string name</param>
        /// <returns>True if it exists, false otherwise</returns>
        bool ViewExists(string View, string ConnectionString);
    }
}