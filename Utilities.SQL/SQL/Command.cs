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
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.DataTypes.Patterns;
using Utilities.SQL.ExtensionMethods;
using Utilities.SQL.Interfaces;
using Utilities.DataTypes.Comparison;
#endregion

namespace Utilities.SQL
{
    /// <summary>
    /// Holds information for an individual SQL command
    /// </summary>
    public class Command
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SQLCommand">Actual SQL command</param>
        /// <param name="CommandType">Command type</param>
        /// <param name="Parameters">Parameters</param>
        public Command(string SQLCommand, CommandType CommandType, params object[] Parameters)
        {
            this.CommandType = CommandType;
            this.SQLCommand = SQLCommand;
            this.Parameters = new List<object>();
            if (Parameters != null)
                this.Parameters.AddRange(Parameters);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Actual SQL command
        /// </summary>
        public virtual string SQLCommand { get; protected set; }

        /// <summary>
        /// Command type
        /// </summary>
        public virtual CommandType CommandType { get; protected set; }

        /// <summary>
        /// Parameters associated with the command
        /// </summary>
        public virtual List<object> Parameters { get; protected set; }

        #endregion

        #region Functions

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>Determines if the commands are equal</returns>
        public override bool Equals(object obj)
        {
            Command OtherCommand = obj as Command;
            if (OtherCommand == null)
                return false;
            if (OtherCommand.SQLCommand != SQLCommand
                || OtherCommand.CommandType != CommandType
                || Parameters.Count != OtherCommand.Parameters.Count)
                return false;
            GenericEqualityComparer<object> Comparer = new GenericEqualityComparer<object>();
            for (int x = 0; x < Parameters.Count; ++x)
            {
                if (!Comparer.Equals(Parameters[x], OtherCommand.Parameters[x]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the hash code for the command
        /// </summary>
        /// <returns>The hash code for the object</returns>
        public override int GetHashCode()
        {
            return (SQLCommand.GetHashCode() + CommandType.GetHashCode()) % Parameters.Sum(x => x.GetHashCode());
        }

        /// <summary>
        /// Returns the string representation of the command
        /// </summary>
        /// <returns>The string representation of the command</returns>
        public override string ToString()
        {
            return SQLCommand;
        }

        #endregion
    }
}