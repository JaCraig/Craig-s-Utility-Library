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
using System.Collections.Generic;
using System.Data;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.DataTypes.Patterns;
using Utilities.SQL.Interfaces;
using Utilities.SQL.MicroORM;
using System.Linq;
using System.Globalization;
#endregion

namespace Utilities.SQL
{
    /// <summary>
    /// SQL command builder
    /// </summary>
    public class SQLCommand:IFluentInterface
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="CommandType">Command type</param>
        /// <param name="Columns">Columns</param>
        /// <param name="Table">Table name</param>
        protected SQLCommand(string CommandType,string Table,params string[]Columns)
        {
            this.CommandType = CommandType;
            this.Columns = new List<string>();
            if (Columns == null || Columns.Length == 0)
                this.Columns.Add("*");
            else
                this.Columns.Add(Columns);
            this.Joins = new List<SQLJoin>();
            this.Parameters = new List<IParameter>();
            this.OrderByColumns = new List<string>();
            this.IsDistinct = false;
            this.TopNumber = -1;
            this.Table = Table;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Command type (SELECT, DELETE, UPDATE, INSERT)
        /// </summary>
        protected string CommandType { get; set; }

        /// <summary>
        /// Columns
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected List<string> Columns { get; private set; }

        /// <summary>
        /// Joins
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected List<SQLJoin> Joins { get; private set; }

        /// <summary>
        /// Base table name
        /// </summary>
        protected string Table { get; set; }

        /// <summary>
        /// Parameters
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected List<IParameter> Parameters { get; private set; }

        /// <summary>
        /// Where clause
        /// </summary>
        protected string WhereClause { get; set; }

        /// <summary>
        /// Order by
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        protected List<string> OrderByColumns { get; private set; }

        /// <summary>
        /// Limits the results to the top X amount
        /// </summary>
        protected int TopNumber { get; set; }

        /// <summary>
        /// Determines if the items should be distinct
        /// </summary>
        protected bool IsDistinct { get; set; }

        #endregion

        #region Static Functions

        #region Select

        /// <summary>
        /// Creates a select command
        /// </summary>
        /// <param name="Columns">Columns to return</param>
        /// <param name="Table">Base table name</param>
        /// <returns>An SQLCommand object</returns>
        public static SQLCommand Select(string Table, params string[] Columns)
        {
            return new SQLCommand("SELECT", Table, Columns);
        }

        #endregion

        #endregion

        #region Functions

        #region Build

        /// <summary>
        /// Builds a command
        /// </summary>
        /// <returns>The resulting command</returns>
        public Command Build()
        {
            string Command = (TopNumber > 0 ? "SELECT TOP " + TopNumber : "SELECT") + (IsDistinct ? " DISTINCT" : "") + " {0} FROM {1} {2} {3} {4}";
            string Where = "";
            string OrderBy = "";
            if (!string.IsNullOrEmpty(WhereClause))
                Where += WhereClause.Trim().ToUpperInvariant().StartsWith("WHERE", StringComparison.CurrentCulture) ? WhereClause.Trim() : "WHERE " + WhereClause.Trim();
            else if (Parameters != null && Parameters.Count > 0)
            {
                Where += "WHERE ";
                Where += Parameters[0];
                for (int x = 1; x < Parameters.Count; ++x)
                {
                    Where += " AND " + Parameters[x];
                }
            }
            if (OrderByColumns != null && OrderByColumns.Count > 0)
            {
                OrderBy += " ORDER BY " + OrderByColumns[0];
                for (int x = 1; x < OrderByColumns.Count; ++x)
                {
                    OrderBy += "," + OrderByColumns[1];
                }
            }
            return new Command(string.Format(CultureInfo.InvariantCulture, Command, Columns.ToString(x => x), Table, Joins.ToString(x => x.ToString(), " "), Where, OrderBy).Trim(), System.Data.CommandType.Text, Parameters.ToArray());
        }

        #endregion

        #region Distinct

        /// <summary>
        /// Sets the command to return distinct rows
        /// </summary>
        /// <returns>this</returns>
        public SQLCommand Distinct()
        {
            this.IsDistinct = true;
            return this;
        }

        #endregion

        #region Join

        /// <summary>
        /// Joins another table to the command
        /// </summary>
        /// <param name="Table">Table name</param>
        /// <param name="Type">Type of join</param>
        /// <param name="ONClause">ON Clause</param>
        /// <returns>this</returns>
        public SQLCommand Join(string Table, string Type, string ONClause)
        {
            this.Joins.Add(new SQLJoin(Table, Type, ONClause));
            return this;
        }

        #endregion

        #region OrderBy

        /// <summary>
        /// Sets up the order by clause
        /// </summary>
        /// <param name="Columns">Columns to order by</param>
        /// <returns>this</returns>
        public SQLCommand OrderBy(params string[] Columns)
        {
            this.OrderByColumns.Add(Columns);
            return this;
        }

        #endregion

        #region Top

        /// <summary>
        /// Limits the number of items to the top X items
        /// </summary>
        /// <param name="Amount">The number of items to limit it to</param>
        /// <returns>this</returns>
        public SQLCommand Top(int Amount)
        {
            this.TopNumber = Amount;
            return this;
        }

        #endregion

        #region Where

        /// <summary>
        /// Generates the where clause based on the parameters passed in
        /// </summary>
        /// <param name="Parameters">Parameters to use</param>
        /// <returns>this</returns>
        public SQLCommand Where(params IParameter[] Parameters)
        {
            this.WhereClause = "";
            this.Parameters.Add(Parameters);
            return this;
        }

        /// <summary>
        /// Sets the where clause
        /// </summary>
        /// <param name="WhereClause">Where clause</param>
        /// <param name="Parameters">Parameters to use</param>
        /// <returns>this</returns>
        public SQLCommand Where(string WhereClause, params IParameter[] Parameters)
        {
            this.WhereClause = WhereClause;
            this.Parameters.Add(Parameters);
            return this;
        }

        /// <summary>
        /// Sets the where clause
        /// </summary>
        /// <param name="WhereClause">Where clause</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        /// <param name="Parameters">Parameters to use</param>
        /// <returns>this</returns>
        public SQLCommand Where(string WhereClause, string ParameterStarter, params object[] Parameters)
        {
            this.WhereClause = WhereClause;
            foreach (object Parameter in Parameters)
            {
                string TempParameter = Parameter as string;
                if (Parameter == null)
                    this.Parameters.Add(new Parameter<object>(this.Parameters.Count.ToString(CultureInfo.InvariantCulture), default(DbType), null, ParameterDirection.Input, ParameterStarter));
                else if (TempParameter!=null)
                    this.Parameters.Add(new StringParameter(this.Parameters.Count.ToString(CultureInfo.InvariantCulture), TempParameter, ParameterDirection.Input, ParameterStarter));
                else
                    this.Parameters.Add(new Parameter<object>(this.Parameters.Count.ToString(CultureInfo.InvariantCulture), Parameter, ParameterDirection.Input, ParameterStarter));
            }
            return this;
        }

        #endregion

        #endregion

        #region Classes

        #region SQLJoin

        /// <summary>
        /// Handles join information
        /// </summary>
        protected class SQLJoin
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="Table">Table name</param>
            /// <param name="Type">Type of join</param>
            /// <param name="ONClause">ON Clause</param>
            public SQLJoin(string Table, string Type,string ONClause)
            {
                this.Table = Table;
                this.Type = Type;
                this.ONClause = ONClause;
            }

            /// <summary>
            /// Table name
            /// </summary>
            public string Table { get; set; }

            /// <summary>
            /// Join type
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// ON Clause
            /// </summary>
            public string ONClause { get; set; }

            /// <summary>
            /// returns the join as a string
            /// </summary>
            /// <returns>The join as a string</returns>
            public override string ToString()
            {
                return Type + " " + Table + (ONClause.Trim().ToUpperInvariant().StartsWith("ON", StringComparison.CurrentCulture) ? " " + ONClause : " ON " + ONClause);
            }
        }

        #endregion

        #endregion
    }
}
