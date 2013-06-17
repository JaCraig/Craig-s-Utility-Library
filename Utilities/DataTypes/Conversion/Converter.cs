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
using System.Data.Entity.Design.PluralizationServices;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.DataTypes.Formatters;
using Utilities.DataTypes.Formatters.Interfaces;
#endregion

namespace Utilities.DataTypes.Conversion
{
    /// <summary>
    /// Converter class
    /// </summary>
    /// <typeparam name="T">Type of input</typeparam>
    /// <typeparam name="R">Type of output</typeparam>
    public class Converter<T, R> : IConverter
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Function">Function that the converter uses</param>
        public Converter(Func<T, R> Function) { Convert = Function; }

        #endregion

        #region Properties

        /// <summary>
        /// Converts the object from type T to R
        /// </summary>
        protected Func<T, R> Convert { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Converts the object from type T to type R
        /// </summary>
        /// <param name="Item">Item to convert</param>
        /// <param name="DefaultValue">Default value to return if the value is not convertable</param>
        /// <returns>The object as the type specified</returns>
        public R To(T Item, R DefaultValue = default(R))
        {
            return Convert(Item);
        }

        #endregion
    }
}