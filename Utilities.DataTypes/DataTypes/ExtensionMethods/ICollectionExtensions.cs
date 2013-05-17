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
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// ICollection extensions
    /// </summary>
    public static class ICollectionExtensions
    {
        #region Functions

        #region Add

        /// <summary>
        /// Adds a list of items to the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="Collection">Collection</param>
        /// <param name="Items">Items to add</param>
        /// <returns>The collection with the added items</returns>
        public static ICollection<T> Add<T>(this ICollection<T> Collection, IEnumerable<T> Items)
        {
            Collection.ThrowIfNull("Collection");
            if (Items==null)
                return Collection;
            Items.ForEach(x => Collection.Add(x));
            return Collection;
        }

        /// <summary>
        /// Adds a list of items to the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="Collection">Collection</param>
        /// <param name="Items">Items to add</param>
        /// <returns>The collection with the added items</returns>
        public static ICollection<T> Add<T>(this ICollection<T> Collection, params T[] Items)
        {
            Collection.ThrowIfNull("Collection");
            if (Items==null)
                return Collection;
            Items.ForEach(x => Collection.Add(x));
            return Collection;
        }

        #endregion

        #region AddAndReturn

        /// <summary>
        /// Adds an item to a list and returns the item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="Collection">Collection to add to</param>
        /// <param name="Item">Item to add to the collection</param>
        /// <returns>The original item</returns>
        public static T AddAndReturn<T>(this ICollection<T> Collection, T Item)
        {
            Collection.ThrowIfNull("Collection");
            Item.ThrowIfNull("Item");
            Collection.Add(Item);
            return Item;
        }

        #endregion

        #region AddIf

        /// <summary>
        /// Adds items to the collection if it passes the predicate test
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="Collection">Collection to add to</param>
        /// <param name="Items">Items to add to the collection</param>
        /// <param name="Predicate">Predicate that an item needs to satisfy in order to be added</param>
        /// <returns>True if any are added, false otherwise</returns>
        public static bool AddIf<T>(this ICollection<T> Collection, Predicate<T> Predicate, params T[] Items)
        {
            Collection.ThrowIfNull("Collection");
            Predicate.ThrowIfNull("Predicate");
            bool ReturnValue = false;
            foreach (T Item in Items)
            {
                if (Predicate(Item))
                {
                    Collection.Add(Item);
                    ReturnValue = true;
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="Collection">Collection to add to</param>
        /// <param name="Items">Items to add to the collection</param>
        /// <param name="Predicate">Predicate that an item needs to satisfy in order to be added</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIf<T>(this ICollection<T> Collection, Predicate<T> Predicate, IEnumerable<T> Items)
        {
            Collection.ThrowIfNull("Collection");
            Predicate.ThrowIfNull("Predicate");
            return Collection.AddIf(Predicate, Items.ToArray());
        }

        #endregion

        #region AddIfUnique

        /// <summary>
        /// Adds an item to the collection if it isn't already in the collection
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="Collection">Collection to add to</param>
        /// <param name="Items">Items to add to the collection</param>
        /// <returns>True if it is added, false otherwise</returns>
        public static bool AddIfUnique<T>(this ICollection<T> Collection, params T[] Items)
        {
            Collection.ThrowIfNull("Collection");
            return Collection.AddIf(x => !Collection.Contains(x), Items);
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
            Collection.ThrowIfNull("Collection");
            return Collection.AddIf(x => !Collection.Contains(x), Items);
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes all items that fit the predicate passed in
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="Collection">Collection to remove items from</param>
        /// <param name="Predicate">Predicate used to determine what items to remove</param>
        public static ICollection<T> Remove<T>(this ICollection<T> Collection, Func<T, bool> Predicate)
        {
            Collection.ThrowIfNull("Collection");
            return Collection.Where(x => !Predicate(x)).ToList();
        }

        /// <summary>
        /// Removes all items in the list from the collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="Collection">Collection</param>
        /// <param name="Items">Items to remove</param>
        /// <returns>The collection with the items removed</returns>
        public static ICollection<T> Remove<T>(this ICollection<T> Collection, IEnumerable<T> Items)
        {
            Collection.ThrowIfNull("Collection");
            if (Items==null)
                return Collection;
            return Collection.Where(x => !Items.Contains(x)).ToList();
        }

        #endregion

        #endregion
    }
}