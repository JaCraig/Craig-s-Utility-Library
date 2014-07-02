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
using System.Linq;
using Utilities.ORM.Manager.Schema.Interfaces;

namespace Utilities.ORM.Manager.Schema.BaseClasses
{
    /// <summary>
    /// Source base class
    /// </summary>
    public abstract class SourceBase : ISource
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected SourceBase(string Name)
        {
            this.Name = Name;
            this.Tables = new List<ITable>();
            this.StoredProcedures = new List<ITable>();
            this.Views = new List<ITable>();
            this.Functions = new List<IFunction>();
        }

        /// <summary>
        /// Functions with the source
        /// </summary>
        public ICollection<IFunction> Functions { get; private set; }

        /// <summary>
        /// Name of the source
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Stored procedures within the source
        /// </summary>
        public ICollection<ITable> StoredProcedures { get; private set; }

        /// <summary>
        /// Tables within the source
        /// </summary>
        public ICollection<ITable> Tables { get; private set; }

        /// <summary>
        /// Views within the source
        /// </summary>
        public ICollection<ITable> Views { get; private set; }

        /// <summary>
        /// Gets a specific table based on the name
        /// </summary>
        /// <param name="Name">Name of the table</param>
        /// <returns>The table specified</returns>
        public ITable this[string Name] { get { return Tables.FirstOrDefault(x => string.Equals(x.Name, Name, System.StringComparison.CurrentCultureIgnoreCase)); } }

        /// <summary>
        /// Adds a function to the source
        /// </summary>
        /// <param name="Name">Name of the function</param>
        /// <param name="Definition">Definition of the function</param>
        /// <returns>Function that was created/added</returns>
        public abstract IFunction AddFunction(string Name, string Definition);

        /// <summary>
        /// Adds a stored procedure to the source
        /// </summary>
        /// <param name="ProcedureName">Procedure name</param>
        /// <param name="Definition">Definition of the stored procedure</param>
        /// <returns>Stored procedure that was created/added</returns>
        public abstract ITable AddStoredProcedure(string ProcedureName, string Definition);

        /// <summary>
        /// Adds a table to the source
        /// </summary>
        /// <param name="TableName">Table name</param>
        /// <returns>Table that was created/added</returns>
        public abstract ITable AddTable(string TableName);

        /// <summary>
        /// Adds a view to the source
        /// </summary>
        /// <param name="ViewName">View name</param>
        /// <returns>View that was created/added</returns>
        public abstract ITable AddView(string ViewName);
    }
}