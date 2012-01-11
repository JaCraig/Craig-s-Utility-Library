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
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Utilities.DataTypes.Comparison;
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// IDictionary extensions
    /// </summary>
    public static class IDictionaryExtensions
    {
        #region Functions

        #region Sort

        /// <summary>
        /// Sorts a dictionary
        /// </summary>
        /// <typeparam name="T1">Key type</typeparam>
        /// <typeparam name="T2">Value type</typeparam>
        /// <param name="Dictionary">Dictionary to sort</param>
        /// <param name="Comparer">Comparer used to sort (defaults to GenericComparer)</param>
        /// <returns>The sorted dictionary</returns>
        public static IDictionary<T1, T2> Sort<T1, T2>(this IDictionary<T1, T2> Dictionary, IComparer<T1> Comparer = null) where T1 : IComparable
        {
            Dictionary.ThrowIfNull("Dictionary");
            return new SortedDictionary<T1, T2>(Dictionary, Comparer.NullCheck(new GenericComparer<T1>()));
        }

        #endregion

        #region SortByValue

        /// <summary>
        /// Sorts a dictionary by value
        /// </summary>
        /// <typeparam name="T1">Key type</typeparam>
        /// <typeparam name="T2">Value type</typeparam>
        /// <param name="Dictionary">Dictionary to sort</param>
        /// <param name="Comparer">Comparer used to sort (defaults to GenericComparer)</param>
        /// <returns>The sorted dictionary</returns>
        public static IDictionary<T1, T2> SortByValue<T1, T2>(this IDictionary<T1, T2> Dictionary, IComparer<T1> Comparer = null) where T1 : IComparable
        {
            Dictionary.ThrowIfNull("Dictionary");
            return new SortedDictionary<T1, T2>(Dictionary, Comparer.NullCheck(new GenericComparer<T1>()))
                            .OrderBy(x => x.Value)
                            .ToDictionary(x => x.Key, x => x.Value);
        }

        #endregion

        #endregion
    }
}