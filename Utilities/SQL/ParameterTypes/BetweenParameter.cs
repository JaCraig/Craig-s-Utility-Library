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
using System.Linq;
using System.Text;
using Utilities.SQL.MicroORM.Interfaces;
using Utilities.Reflection.ExtensionMethods;
using Utilities.SQL.Interfaces;
#endregion

namespace Utilities.SQL.ParameterTypes
{
    /// <summary>
    /// Parameter class that checks if a value is between two other values
    /// </summary>
    /// <typeparam name="DataType">Type of the parameter</typeparam>
    public class BetweenParameter<DataType> : IParameter
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value of the parameter</param>
        /// <param name="Max">Max value of the parameter</param>
        /// <param name="ID">Name of the parameter</param>
        /// <param name="ParameterStarter">What the database expects as the
        /// parameter starting string ("@" for SQL Server, ":" for Oracle, etc.)</param>
        public BetweenParameter(DataType Min,DataType Max, string ID, string ParameterStarter = "@")
        {
            this.Min = Min;
            this.Max = Max;
            this.ID = ID;
            this.ParameterStarter = ParameterStarter;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Min value of the parameter
        /// </summary>
        public DataType Min { get; set; }

        /// <summary>
        /// Max value of the parameter
        /// </summary>
        public DataType Max { get; set; }

        /// <summary>
        /// Name of the parameter
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Starting string of the parameter
        /// </summary>
        public string ParameterStarter { get; set; }

        #endregion

        #region Functions

        public void AddParameter(SQLHelper Helper)
        {
            Helper.AddParameter(ID + "Min", Min);
            Helper.AddParameter(ID + "Max", Max);
        }

        public override string ToString() { return ID + " BETWEEN " + ParameterStarter + ID + "Min AND " + ParameterStarter + ID + "Max"; }

        #endregion
    }
}