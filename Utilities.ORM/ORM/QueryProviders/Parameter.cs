/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using Utilities.SQL.MicroORM;
using Utilities.ORM.QueryProviders.Interfaces;
#endregion

namespace Utilities.ORM.QueryProviders
{
    /// <summary>
    /// Parameter class
    /// </summary>
    public class Parameter<DataType> : Utilities.SQL.MicroORM.Parameter<DataType>,IParameter
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Value">Value of the parameter</param>
        /// <param name="Name">Name associated with the parameter (field name)</param>
        /// <param name="ParameterStarter">Symbol used by the database to denote a parameter</param>
        /// <param name="Length">Max length of the string (if it is a string)</param>
        public Parameter(DataType Value, string Name, int Length = 0, string ParameterStarter = "@")
            : base(Value, Name, Length, ParameterStarter)
        {
        }

        #endregion
    }
}