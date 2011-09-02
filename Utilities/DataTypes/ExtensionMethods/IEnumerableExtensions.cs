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

        #region ForEach

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> List, Action<T> Action)
        {
            if (List == null)
                throw new ArgumentNullException("List");
            if (Action == null)
                throw new ArgumentNullException("Action");
            foreach (T Item in List)
                Action(Item);
            return List;
        }

        #endregion

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

        #region ToArray

        /// <summary>
        /// Converts a list to an array
        /// </summary>
        /// <typeparam name="Source">Source type</typeparam>
        /// <typeparam name="Target">Target type</typeparam>
        /// <param name="List">List to convert</param>
        /// <param name="ConvertingFunction">Function used to convert each item</param>
        /// <returns>The array containing the items from the list</returns>
        public static Target[] ToArray<Source, Target>(this IEnumerable<Source> List, Func<Source, Target> ConvertingFunction)
        {
            if (List == null)
                throw new ArgumentNullException("List");
            if (ConvertingFunction == null)
                throw new ArgumentNullException("ConvertingFunction");
            return List.Select(ConvertingFunction).ToArray();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Converts the list to a string where each item is seperated by the Seperator
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">List to convert</param>
        /// <param name="Seperator">Seperator to use between items</param>
        /// <returns>The string version of the list</returns>
        public static string ToString<T>(this IEnumerable<T> List, string Seperator)
        {
            if (List == null)
                throw new ArgumentNullException("List");
            if (string.IsNullOrEmpty(Seperator))
                Seperator = "";
            StringBuilder Builder = new StringBuilder();
            string TempSeperator = "";
            foreach (T Item in List)
            {
                Builder.Append(TempSeperator).Append(Item.ToString());
                TempSeperator = Seperator;
            }
            return Builder.ToString();
        }

        #endregion

        #endregion
    }
}