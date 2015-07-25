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

using Utilities.DataTypes;
using Utilities.ORM.Manager.Schema.BaseClasses;
using Utilities.ORM.Manager.Schema.Interfaces;

namespace Utilities.ORM.Manager.Schema.Default.Database
{
    /// <summary>
    /// Database class
    /// </summary>
    public class Database : SourceBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Name of the database</param>
        public Database(string Name)
            : base(Name)
        {
        }

        /// <summary>
        /// Adds a function to the database
        /// </summary>
        /// <param name="Name">Function name</param>
        /// <param name="Definition">Function definition</param>
        public override IFunction AddFunction(string Name, string Definition)
        {
            return Functions.AddAndReturn(new Function(Name, Definition, this));
        }

        /// <summary>
        /// Adds a stored procedure to the database
        /// </summary>
        /// <param name="ProcedureName">Procedure name</param>
        /// <param name="Definition">Definition</param>
        public override ITable AddStoredProcedure(string ProcedureName, string Definition)
        {
            return StoredProcedures.AddAndReturn(new StoredProcedure(ProcedureName, Definition, this));
        }

        /// <summary>
        /// Adds a table to the database
        /// </summary>
        /// <param name="TableName">Table name</param>
        public override ITable AddTable(string TableName)
        {
            return Tables.AddAndReturn(new Table(TableName, this));
        }

        /// <summary>
        /// Adds a view to the database
        /// </summary>
        /// <param name="ViewName">View name</param>
        public override ITable AddView(string ViewName)
        {
            return Views.AddAndReturn(new View(ViewName, this));
        }
    }
}