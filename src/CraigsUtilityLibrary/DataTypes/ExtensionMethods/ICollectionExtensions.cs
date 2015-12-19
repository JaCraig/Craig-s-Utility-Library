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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Utilities.DataTypes
{
    /// <summary>
    /// ICollection extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Adds a list of items to the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="items">Items to add</param>
        /// <returns>The collection with the added items</returns>
        public static ICollection<T> Add<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            return collection.Add(items.ToArray());
        }

        /// <summary>
        /// Adds a list of items to the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="items">Items to add</param>
        /// <returns>The collection with the added items</returns>
        public static ICollection<T> Add<T>(this ICollection<T> collection, params T[] items)
        {
            if (collection == null)
                return new List<T>();
            if (items == null)
                return collection;
            items.ForEach(x => collection.Add(x));
            return collection;
        }

        /// <summary>
        /// Adds an item to a list and returns the item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="item">Item to add to the collection</param>
        /// <returns>The original item</returns>
        public static T AddAndReturn<T>(this ICollection<T> collection, T item)
        {
            if (collection == null)
                return item;
            collection.Add(item);
            return item;
        }

        /// <summary>
        /// Adds items to the collection if it passes the predicate test
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <param name="predicate">Predicate that an item needs to satisfy in order to be added</param>
        /// <returns>True if any are added, false otherwise</returns>
        public static bool AddIf<T>(this ICollection<T> collection, Predicate<T> predicate, params T[] items)
        {
            if (collection == null || predicate == null)
                return false;
            if (items == null || items.Length == 0)
                return true;
            bool ReturnValue = false;
            foreach (T Item in items)
            {
                if (predicate(Item))
                {
                    collection.Add(Item);
                    ReturnValue = true;
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <param name="predicate">Predicate that an item needs to satisfy in order to be added</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIf<T>(this ICollection<T> collection, Predicate<T> predicate, IEnumerable<T> items)
        {
            if (collection == null || predicate == null)
                return false;
            if (items == null || items.Count() == 0)
                return true;
            return collection.AddIf(predicate, items.ToArray());
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ICollection<T> collection, params T[] items)
        {
            if (collection == null)
                return false;
            if (items == null)
                return true;
            return collection.AddIf(x => !collection.Contains(x), items);
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <param name="predicate">
        /// Predicate used to determine if two values are equal. Return true if they are the same,
        /// false otherwise
        /// </param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ICollection<T> collection, Func<T, T, bool> predicate, params T[] items)
        {
            if (collection == null || predicate == null)
                return false;
            if (items == null)
                return true;
            return collection.AddIf(x => !collection.Any(y => predicate(x, y)), items);
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                return false;
            if (items == null)
                return true;
            return collection.AddIf(x => !collection.Contains(x), items);
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="collection">Collection to add to</param>
        /// <param name="items">Items to add to the collection</param>
        /// <param name="predicate">
        /// Predicate used to determine if two values are equal. Return true if they are the same,
        /// false otherwise
        /// </param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ICollection<T> collection, Func<T, T, bool> predicate, IEnumerable<T> items)
        {
            if (collection == null || predicate == null)
                return false;
            if (items == null)
                return true;
            return collection.AddIf(x => !collection.Any(y => predicate(x, y)), items);
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">0 based item to start with (inclusive)</param>
        /// <param name="end">0 based item to end with (exclusive)</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IList<T> For<T>(this IList<T> list, int start, int end, Action<T, int> action)
        {
            if (list == null)
                return new List<T>();
            var TempList = list.ElementsBetween(start, end + 1).ToArray();
            for (int x = 0; x < TempList.Length; ++x)
            {
                action(TempList[x], x);
            }
            return list;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable between the start and end indexes and
        /// returns an IEnumerable of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">0 based item to start with (inclusive)</param>
        /// <param name="end">0 based item to end with (exclusive)</param>
        /// <param name="function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IList<R> For<T, R>(this IList<T> list, int start, int end, Func<T, int, R> function)
        {
            if (list == null || function == null)
                return new List<R>();
            var TempList = list.ElementsBetween(start, end + 1).ToArray();
            var ReturnList = new List<R>();
            for (int x = 0; x < TempList.Length; ++x)
            {
                ReturnList.Add(function(TempList[x], x));
            }
            return ReturnList;
        }

        /// <summary>
        /// Removes all items that fit the predicate passed in
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">Collection to remove items from</param>
        /// <param name="predicate">Predicate used to determine what items to remove</param>
        public static ICollection<T> Remove<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            if (collection == null)
                return new List<T>();
            return collection.Where(x => !predicate(x)).ToList();
        }

        /// <summary>
        /// Removes all items in the list from the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="items">Items to remove</param>
        /// <returns>The collection with the items removed</returns>
        public static ICollection<T> Remove<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                return new List<T>();
            if (items == null)
                return collection;
            return collection.Where(x => !items.Contains(x)).ToList();
        }
    }
}