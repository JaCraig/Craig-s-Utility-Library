/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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


#endregion

namespace Utilities.SQL.DataClasses
{
    /// <summary>
    /// Database class
    /// </summary>
    public class Database
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">Name of the database</param>
        public Database(string Name)
        {
            this.Name = Name;
            Tables = new List<Table>();
            StoredProcedures = new List<StoredProcedure>();
            Views = new List<View>();
            Functions = new List<Function>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Database()
        {
            Tables = new List<Table>();
            StoredProcedures = new List<StoredProcedure>();
            Views = new List<View>();
            Functions = new List<Function>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// List of tables
        /// </summary>
        public List<Table> Tables { get; set; }

        /// <summary>
        /// List of stored procedures
        /// </summary>
        public List<StoredProcedure> StoredProcedures { get; set; }

        /// <summary>
        /// List of views
        /// </summary>
        public List<View> Views { get; set; }

        /// <summary>
        /// List of functions
        /// </summary>
        public List<Function> Functions { get; set; }

        /// <summary>
        /// Returns a table with the given name
        /// </summary>
        /// <param name="Name">Table name</param>
        /// <returns>The table specified</returns>
        public Table this[string Name]
        {
            get
            {
                foreach (Table Table in Tables)
                {
                    if (Table.Name == Name)
                    {
                        return Table;
                    }
                }
                return null;
            }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Adds a table to the database
        /// </summary>
        /// <param name="TableName">Table name</param>
        public void AddTable(string TableName)
        {
            Tables.Add(new Table(TableName, this));
        }

        /// <summary>
        /// Adds a view to the database
        /// </summary>
        /// <param name="ViewName">View name</param>
        public void AddView(string ViewName)
        {
            Views.Add(new View(ViewName, this));
        }

        /// <summary>
        /// Adds a stored procedure to the database
        /// </summary>
        /// <param name="ProcedureName">Procedure name</param>
        /// <param name="Definition">Definition</param>
        public void AddStoredProcedure(string ProcedureName, string Definition)
        {
            StoredProcedures.Add(new StoredProcedure(ProcedureName, Definition,this));
        }

        /// <summary>
        /// Adds a function to the database
        /// </summary>
        /// <param name="Name">Function name</param>
        /// <param name="Definition">Function definition</param>
        public void AddFunction(string Name, string Definition)
        {
            Functions.Add(new Function(Name, Definition, this));
        }

        #endregion
    }
}
