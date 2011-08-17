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
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// ICollection extensions
    /// </summary>
    public static class ICollectionExtensions
    {
        #region Functions

        #region AddIfUnique

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="Collection">Collection to add to</param>
        /// <param name="Item">Item to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ICollection<T> Collection, T Item)
        {
            if (Collection == null)
                throw new ArgumentNullException("Collection");
            if (Collection.Contains(Item))
                return false;
            Collection.Add(Item);
            return true;
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="Collection">Collection to add to</param>
        /// <param name="Items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ICollection<T> Collection, IEnumerable<T> Items)
        {
            if (Collection == null)
                throw new ArgumentNullException("Collection");
            bool ReturnValue = false;
            foreach (T Item in Items)
            {
                if (!Collection.Contains(Item))
                {
                    Collection.Add(Item);
                    ReturnValue = true;
                }
            }
            return ReturnValue;
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes all items that fit the predicate passed in
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="Collection">Collection to remove items from</param>
        /// <param name="Predicate">Predicate used to determine what items to remove</param>
        public static void Remove<T>(this ICollection<T> Collection, Func<T, bool> Predicate)
        {
            if (Collection == null)
                throw new ArgumentNullException("Collection");
            Collection.Where(Predicate).ToList().ForEach(x => Collection.Remove(x));
        }

        #endregion

        #endregion
    }
}