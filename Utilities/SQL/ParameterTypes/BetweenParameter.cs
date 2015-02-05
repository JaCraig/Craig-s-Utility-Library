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

using System.Data.Common;
using Utilities.SQL.ExtensionMethods;
using Utilities.SQL.Interfaces;

#endregion Usings

namespace Utilities.SQL.ParameterTypes
{
    /// <summary>
    /// Parameter class that checks if a value is between two other values
    /// </summary>
    /// <typeparam name="DataType">Type of the parameter</typeparam>
    public class BetweenParameter<DataType> : ParameterBase<DataType>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value of the parameter</param>
        /// <param name="Max">Max value of the parameter</param>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="ParameterStarter">
        /// What the database expects as the parameter starting string ("@" for SQL Server, ":" for
        /// Oracle, etc.)
        /// </param>
        public BetweenParameter(DataType Min, DataType Max, string ID, string ParameterStarter = "@")
            : base(ID, Min, System.Data.ParameterDirection.Input, ParameterStarter)
        {
            this.Min = Min;
            this.Max = Max;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Max value of the parameter
        /// </summary>
        public DataType Max { get; set; }

        /// <summary>
        /// Min value of the parameter
        /// </summary>
        public DataType Min { get; set; }

        #endregion Properties

        #region Functions

        /// <summary>
        /// Adds the parameter to the SQLHelper
        /// </summary>
        /// <param name="Helper">SQLHelper to add the parameter to</param>
        public override void AddParameter(DbCommand Helper)
        {
            Helper.AddParameter(ID + "Min", Min);
            Helper.AddParameter(ID + "Max", Max);
        }

        /// <summary>
        /// Creates a copy of the parameter
        /// </summary>
        /// <param name="Suffix">Suffix to add to the parameter (for batching purposes)</param>
        /// <returns>A copy of the parameter</returns>
        public override IParameter CreateCopy(string Suffix)
        {
            return new BetweenParameter<DataType>(Min, Max, OriginalID + Suffix, ParameterStarter);
        }

        /// <summary>
        /// Outputs the param as a string
        /// </summary>
        /// <returns>The param as a string</returns>
        public override string ToString()
        {
            return OriginalID + " BETWEEN " + ParameterStarter + ID + "Min AND " + ParameterStarter + ID + "Max";
        }

        #endregion Functions
    }
}