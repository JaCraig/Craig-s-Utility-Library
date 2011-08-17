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
    /// IEnumerable extensions
    /// </summary>
    public static class IEnumerableExtensions
    {
        #region Functions

        #region IsNullOrEmpty

        /// <summary>
        /// Determines if a list is null or empty
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Value">List to check</param>
        /// <returns>True if it is null or empty, false otherwise</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> Value)
        {
            return Value == null || Value.Count() == 0;
        }

        #endregion

        #region RemoveDefaults

        /// <summary>
        /// Removes default values from a list
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="Value">List to cull items from</param>
        /// <param name="EqualityComparer">Equality comparer used (defaults to GenericEqualityComparer)</param>
        /// <returns>An IEnumerable with the default values removed</returns>
        public static IEnumerable<T> RemoveDefaults<T>(this IEnumerable<T> Value, IEqualityComparer<T> EqualityComparer = null)
        {
            if (Value == null)
                yield break;
            if (EqualityComparer == null)
                EqualityComparer = new GenericEqualityComparer<T>();
            foreach (T Item in Value.Where(x => !EqualityComparer.Equals(x, default(T))))
                yield return Item;
        }

        #endregion

        #endregion
    }
}