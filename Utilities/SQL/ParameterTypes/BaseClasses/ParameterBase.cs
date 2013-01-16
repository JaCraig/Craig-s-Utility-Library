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
using System.Data;
using System.Data.Common;
using Utilities.DataTypes.Comparison;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.SQL.Interfaces
{
    /// <summary>
    /// Parameter base class
    /// </summary>
    /// <typeparam name="DataType">Data type of the parameter</typeparam>
    public abstract class ParameterBase<DataType> : IParameter
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID of the parameter</param>
        /// <param name="Value">Value of the parameter</param>
        /// <param name="Direction">Direction of the parameter</param>
        /// <param name="ParameterStarter">What the database expects as the
        /// parameter starting string ("@" for SQL Server, ":" for Oracle, etc.)</param>
        protected ParameterBase(string ID, DataType Value, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
        {
            this.ID = ID;
            this.Value = Value;
            this.Direction = Direction;
            if (Value == null)
                this.DatabaseType = typeof(DataType).ToDbType();
            else
                this.DatabaseType = Value.GetType().ToDbType();
            this.BatchID = ID;
            this.ParameterStarter = ParameterStarter;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID of the parameter</param>
        /// <param name="Type">Database type</param>
        /// <param name="Value">Value of the parameter</param>
        /// <param name="Direction">Direction of the parameter</param>
        /// <param name="ParameterStarter">What the database expects as the
        /// parameter starting string ("@" for SQL Server, ":" for Oracle, etc.)</param>
        protected ParameterBase(string ID, SqlDbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
        {
            this.ID = ID;
            this.Value = (DataType)Value;
            this.DatabaseType = Type.ToDbType();
            this.Direction = Direction;
            this.BatchID = ID;
            this.ParameterStarter = ParameterStarter;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID of the parameter</param>
        /// <param name="Type">Database type</param>
        /// <param name="Value">Value of the parameter</param>
        /// <param name="Direction">Direction of the parameter</param>
        /// <param name="ParameterStarter">What the database expects as the
        /// parameter starting string ("@" for SQL Server, ":" for Oracle, etc.)</param>
        protected ParameterBase(string ID, DbType Type, object Value = null, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
        {
            this.ID = ID;
            this.Value = (DataType)Value;
            this.DatabaseType = Type;
            this.Direction = Direction;
            this.BatchID = ID;
            this.ParameterStarter = ParameterStarter;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The Name that the parameter goes by
        /// </summary>
        public virtual string ID { get; set; }

        /// <summary>
        /// Batch ID
        /// </summary>
        protected virtual string BatchID { get; set; }

        /// <summary>
        /// Parameter value
        /// </summary>
        public virtual DataType Value { get; set; }

        /// <summary>
        /// Direction of the parameter
        /// </summary>
        public virtual ParameterDirection Direction { get; set; }

        /// <summary>
        /// Database type
        /// </summary>
        public virtual DbType DatabaseType { get; set; }

        /// <summary>
        /// Starting string of the parameter
        /// </summary>
        public string ParameterStarter { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Adds this parameter to the SQLHelper
        /// </summary>
        /// <param name="Helper">SQLHelper</param>
        public abstract void AddParameter(DbCommand Helper);

        /// <summary>
        /// Creates a copy of the parameter
        /// </summary>
        /// <param name="Suffix">Suffix to add to the parameter (for batching purposes)</param>
        /// <returns>A copy of the parameter</returns>
        public abstract IParameter CreateCopy(string Suffix);

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            ParameterBase<DataType> OtherParameter = obj as ParameterBase<DataType>;
            return (OtherParameter != null
                && OtherParameter.DatabaseType == DatabaseType
                && OtherParameter.Direction == Direction
                && OtherParameter.ID == ID
                && new GenericEqualityComparer<DataType>().Equals(OtherParameter.Value, Value));
        }

        /// <summary>
        /// Gets the hash code for the object
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return (ID.GetHashCode() * 23 + Value.GetHashCode()) * 23 + DatabaseType.GetHashCode();
        }

        /// <summary>
        /// Returns the string version of the parameter
        /// </summary>
        /// <returns>The string representation of the parameter</returns>
        public override string ToString()
        {
            return ParameterStarter + ID;
        }

        /// <summary>
        /// The == operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>true if the first and second item are the same, false otherwise</returns>
        public static bool operator ==(ParameterBase<DataType> first, ParameterBase<DataType> second)
        {
            if (Object.ReferenceEquals(first, second))
                return true;

            if ((object)first == null || (object)second == null)
                return false;

            return first.GetHashCode() == second.GetHashCode();
        }

        /// <summary>
        /// != operator
        /// </summary>
        /// <param name="first">First item</param>
        /// <param name="second">Second item</param>
        /// <returns>returns true if they are not equal, false otherwise</returns>
        public static bool operator !=(ParameterBase<DataType> first, ParameterBase<DataType> second)
        {
            return !(first == second);
        }

        #endregion
    }
}