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

using System.Data;
using System.Data.Common;
using Utilities.ORM.Manager.QueryProvider.BaseClasses;
using Utilities.ORM.Manager.QueryProvider.Interfaces;

namespace Utilities.ORM.Manager.QueryProvider.Default
{
    /// <summary>
    /// Holds parameter information
    /// </summary>
    public class StringParameter : ParameterBase<string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID of the parameter</param>
        /// <param name="Value">Value of the parameter</param>
        /// <param name="Direction">Direction of the parameter</param>
        /// <param name="ParameterStarter">Parameter starter</param>
        public StringParameter(string ID, string Value, ParameterDirection Direction = ParameterDirection.Input, string ParameterStarter = "@")
            : base(ID, Value, Direction, ParameterStarter)
        {
        }

        /// <summary>
        /// Adds this parameter to the SQLHelper
        /// </summary>
        /// <param name="Helper">SQLHelper</param>
        public override void AddParameter(DbCommand Helper)
        {
            Helper.AddParameter(ID, Value, Direction);
        }

        /// <summary>
        /// Creates a copy of the parameter
        /// </summary>
        /// <param name="Suffix">Suffix to add to the parameter (for batching purposes)</param>
        /// <returns>A copy of the parameter</returns>
        public override IParameter CreateCopy(string Suffix)
        {
            return new StringParameter(ID + Suffix, Value, Direction, ParameterStarter);
        }
    }
}