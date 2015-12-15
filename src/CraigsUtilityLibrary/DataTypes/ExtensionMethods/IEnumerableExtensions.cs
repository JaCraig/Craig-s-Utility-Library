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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataTypes.Comparison;

namespace Utilities.DataTypes
{
    /// <summary>
    /// IEnumerable extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Combines multiple IEnumerables together and returns a new IEnumerable containing all of
        /// the values
        /// </summary>
        /// <typeparam name="T">Type of the data in the IEnumerable</typeparam>
        /// <param name="enumerable1">IEnumerable 1</param>
        /// <param name="additions">IEnumerables to concat onto the first item</param>
        /// <returns>A new IEnumerable containing all values</returns>
        /// <example>
        /// <code>
        /// int[] TestObject1 = new int[] { 1, 2, 3 }; int[] TestObject2 = new int[] { 4, 5, 6
        /// }; int[] TestObject3 = new int[] { 7, 8, 9 }; TestObject1 =
        /// TestObject1.Concat(TestObject2, TestObject3).ToArray();
        /// </code>
        /// </example>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable1, params IEnumerable<T>[] additions)
        {
            enumerable1 = enumerable1 ?? new T[0];
            if (additions == null)
                return enumerable1;
            var ActualAdditions = additions.Where(x => x != null).ToArray();
            var Results = new List<T>();
            Results.AddRange(enumerable1);
            for (int x = 0; x < ActualAdditions.Length; ++x)
                Results.AddRange(ActualAdditions[x]);
            return Results;
        }

        /// <summary>
        /// Returns only distinct items from the IEnumerable based on the predicate
        /// </summary>
        /// <typeparam name="T">Object type within the list</typeparam>
        /// <param name="enumerable">List of objects</param>
        /// <param name="predicate">
        /// Predicate that is used to determine if two objects are equal. True if they are the same,
        /// false otherwise
        /// </param>
        /// <returns>An IEnumerable of only the distinct items</returns>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> enumerable, Func<T, T, bool> predicate)
        {
            if (enumerable == null)
                yield break;
            var TempGenericComparer = new GenericEqualityComparer<T>();
            predicate = predicate ?? TempGenericComparer.Equals;
            var Results = new List<T>();
            foreach (T Item in enumerable)
            {
                if (!Results.Any(x => predicate(Item, x)))
                {
                    Results.Add(Item);
                    yield return Item;
                }
            }
        }

        /// <summary>
        /// Returns elements starting at the index and ending at the end index
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">List to search</param>
        /// <param name="start">Start index (inclusive)</param>
        /// <param name="end">End index (exclusive)</param>
        /// <returns>The items between the start and end index</returns>
        public static IEnumerable<T> ElementsBetween<T>(this IEnumerable<T> list, int start, int end)
        {
            if (list == null)
                yield break;
            if (end > list.Count())
                end = list.Count();
            if (start < 0)
                start = 0;
            if (start > end)
            {
                var Temp = start;
                start = end;
                end = Temp;
            }
            var TempList = list.ToArray();
            for (int x = start; x < end; ++x)
                yield return TempList[x];
        }

        /// <summary>
        /// Removes values from a list that meet the criteria set forth by the predicate
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="value">List to cull items from</param>
        /// <param name="predicate">Predicate that determines what items to remove</param>
        /// <returns>An IEnumerable with the objects that meet the criteria removed</returns>
        public static IEnumerable<T> Except<T>(this IEnumerable<T> value, Func<T, bool> predicate)
        {
            if (value == null)
                return new List<T>();
            if (predicate == null)
                return value;
            return value.Where(x => !predicate(x));
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">Item to start with (inclusive)</param>
        /// <param name="end">Item to end with (exclusive)</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> For<T>(this IEnumerable<T> list, int start, int end, Action<T, int> action)
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
        /// <param name="start">Item to start with</param>
        /// <param name="end">Item to end with</param>
        /// <param name="function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> For<T, R>(this IEnumerable<T> list, int start, int end, Func<T, int, R> function)
        {
            if (list == null || function == null)
                return new R[0];
            var TempList = list.ElementsBetween(start, end + 1).ToArray();
            var ReturnList = new R[TempList.Length];
            for (int x = 0; x < TempList.Length; ++x)
            {
                ReturnList[x] = function(TempList[x], x);
            }
            return ReturnList;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (list == null)
                return new List<T>();
            if (action == null)
                return list;
            foreach (T Item in list)
                action(Item);
            return list;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> list, Func<T, R> function)
        {
            if (list == null || function == null)
                return new List<R>();
            var ReturnList = new List<R>(list.Count());
            foreach (T Item in list)
                ReturnList.Add(function(Item));
            return ReturnList;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="action">Action to do</param>
        /// <param name="catchAction">Action that occurs if an exception occurs</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> action, Action<T, Exception> catchAction)
        {
            if (list == null)
                return new List<T>();
            if (action == null || catchAction == null)
                return list;
            foreach (T Item in list)
            {
                try
                {
                    action(Item);
                }
                catch (Exception e) { catchAction(Item, e); }
            }
            return list;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="function">Function to do</param>
        /// <param name="catchAction">Action that occurs if an exception occurs</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> list, Func<T, R> function, Action<T, Exception> catchAction)
        {
            if (list == null || function == null || catchAction == null)
                return new R[0];
            var ReturnValue = new List<R>();
            foreach (T Item in list)
            {
                try
                {
                    ReturnValue.Add(function(Item));
                }
                catch (Exception e) { catchAction(Item, e); }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEachParallel<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (list == null)
                return new List<T>();
            if (action == null)
                return list;
            Parallel.ForEach(list, action);
            return list;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="function">Function to do</param>
        /// <returns>The results in an IEnumerable list</returns>
        public static IEnumerable<R> ForEachParallel<T, R>(this IEnumerable<T> list, Func<T, R> function)
        {
            if (list == null || function == null)
                return new List<R>();
            return list.ForParallel(0, list.Count() - 1, (x, y) => function(x));
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="action">Action to do</param>
        /// <param name="catchAction">Action that occurs if an exception occurs</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEachParallel<T>(this IEnumerable<T> list, Action<T> action, Action<T, Exception> catchAction)
        {
            if (list == null)
                return new List<T>();
            if (action == null || catchAction == null)
                return list;
            Parallel.ForEach<T>(list, delegate (T Item)
            {
                try
                {
                    action(Item);
                }
                catch (Exception e) { catchAction(Item, e); }
            });
            return list;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="function">Function to do</param>
        /// <param name="catchAction">Action that occurs if an exception occurs</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEachParallel<T, R>(this IEnumerable<T> list, Func<T, R> function, Action<T, Exception> catchAction)
        {
            if (list == null || function == null || catchAction == null)
                return new List<R>();
            var ReturnValues = new ConcurrentBag<R>();
            Parallel.ForEach<T>(list, delegate (T Item)
            {
                try
                {
                    ReturnValues.Add(function(Item));
                }
                catch (Exception e) { catchAction(Item, e); }
            });
            return ReturnValues;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">Item to start with</param>
        /// <param name="end">Item to end with</param>
        /// <param name="action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForParallel<T>(this IEnumerable<T> list, int start, int end, Action<T, int> action)
        {
            if (list == null)
                return new List<T>();
            if (action == null)
                return list;
            if (end > list.Count())
                end = list.Count();
            if (start < 0)
                start = 0;
            if (start > end)
            {
                var Temp = start;
                start = end;
                end = Temp;
            }
            var TempArray = list.ToArray();
            Parallel.For(start, end + 1, new Action<int>(x => action(TempArray[x], x)));
            return list;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="start">Item to start with</param>
        /// <param name="end">Item to end with</param>
        /// <param name="function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForParallel<T, R>(this IEnumerable<T> list, int start, int end, Func<T, int, R> function)
        {
            if (list == null || function == null)
                return new List<R>();
            if (end > list.Count())
                end = list.Count();
            if (start < 0)
                start = 0;
            if (start > end)
            {
                var Temp = start;
                start = end;
                end = Temp;
            }
            var TempArray = list.ToArray();
            R[] Results = new R[(end + 1) - start];
            Parallel.For(start, end + 1, new Action<int>(x => Results[x - start] = function(TempArray[x], x)));
            return Results;
        }

        /// <summary>
        /// Returns the last X number of items from the list
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">IEnumerable to iterate over</param>
        /// <param name="count">Number of items to return</param>
        /// <returns>The last X items from the list</returns>
        public static IEnumerable<T> Last<T>(this IEnumerable<T> list, int count)
        {
            if (list == null)
                return new List<T>();
            return list.ElementsBetween(list.Count() - count, list.Count());
        }

        /// <summary>
        /// Does a left join on the two lists
        /// </summary>
        /// <typeparam name="T1">The type of outer list.</typeparam>
        /// <typeparam name="T2">The type of inner list.</typeparam>
        /// <typeparam name="Key">The type of the key.</typeparam>
        /// <typeparam name="R">The return type</typeparam>
        /// <param name="outer">The outer list.</param>
        /// <param name="inner">The inner list.</param>
        /// <param name="outerKeySelector">The outer key selector.</param>
        /// <param name="innerKeySelector">The inner key selector.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <param name="comparer">The comparer (if null, a generic comparer is used).</param>
        /// <returns>Returns a left join of the two lists</returns>
        public static IEnumerable<R> LeftJoin<T1, T2, Key, R>(this IEnumerable<T1> outer,
            IEnumerable<T2> inner,
            Func<T1, Key> outerKeySelector,
            Func<T2, Key> innerKeySelector,
            Func<T1, T2, R> resultSelector,
            IEqualityComparer<Key> comparer = null)
        {
            if (inner == null
                || outerKeySelector == null
                || innerKeySelector == null
                || resultSelector == null)
                return new List<R>();
            comparer = comparer ?? new GenericEqualityComparer<Key>();
            return outer.ForEach(x => new { left = x, right = inner.FirstOrDefault(y => comparer.Equals(innerKeySelector(y), outerKeySelector(x))) })
                        .ForEach(x => resultSelector(x.left, x.right));
        }

        /// <summary>
        /// Does an outer join on the two lists
        /// </summary>
        /// <typeparam name="T1">The type of outer list.</typeparam>
        /// <typeparam name="T2">The type of inner list.</typeparam>
        /// <typeparam name="Key">The type of the key.</typeparam>
        /// <typeparam name="R">The return type</typeparam>
        /// <param name="outer">The outer list.</param>
        /// <param name="inner">The inner list.</param>
        /// <param name="outerKeySelector">The outer key selector.</param>
        /// <param name="innerKeySelector">The inner key selector.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <param name="comparer">The comparer (if null, a generic comparer is used).</param>
        /// <returns>Returns an outer join of the two lists</returns>
        public static IEnumerable<R> OuterJoin<T1, T2, Key, R>(this IEnumerable<T1> outer,
            IEnumerable<T2> inner,
            Func<T1, Key> outerKeySelector,
            Func<T2, Key> innerKeySelector,
            Func<T1, T2, R> resultSelector,
            IEqualityComparer<Key> comparer = null)
        {
            if (inner == null
                || outer == null
                || outerKeySelector == null
                || innerKeySelector == null
                || resultSelector == null)
                return new List<R>();
            var Left = outer.LeftJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            var Right = outer.RightJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            return Left.Union(Right);
        }

        /// <summary>
        /// Determines the position of an object if it is present, otherwise it returns -1
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">List of objects to search</param>
        /// <param name="item">Object to find the position of</param>
        /// <param name="equalityComparer">
        /// Equality comparer used to determine if the object is present
        /// </param>
        /// <returns>The position of the object if it is present, otherwise -1</returns>
        public static int PositionOf<T>(this IEnumerable<T> list, T item, IEqualityComparer<T> equalityComparer = null)
        {
            if (list == null)
                return -1;
            equalityComparer = equalityComparer ?? new GenericEqualityComparer<T>();
            int Count = 0;
            foreach (T TempItem in list)
            {
                if (equalityComparer.Equals(item, TempItem))
                    return Count;
                ++Count;
            }
            return -1;
        }

        /// <summary>
        /// Does a right join on the two lists
        /// </summary>
        /// <typeparam name="T1">The type of outer list.</typeparam>
        /// <typeparam name="T2">The type of inner list.</typeparam>
        /// <typeparam name="Key">The type of the key.</typeparam>
        /// <typeparam name="R">The return type</typeparam>
        /// <param name="outer">The outer list.</param>
        /// <param name="inner">The inner list.</param>
        /// <param name="outerKeySelector">The outer key selector.</param>
        /// <param name="innerKeySelector">The inner key selector.</param>
        /// <param name="resultSelector">The result selector.</param>
        /// <param name="comparer">The comparer (if null, a generic comparer is used).</param>
        /// <returns>Returns a right join of the two lists</returns>
        public static IEnumerable<R> RightJoin<T1, T2, Key, R>(this IEnumerable<T1> outer,
            IEnumerable<T2> inner,
            Func<T1, Key> outerKeySelector,
            Func<T2, Key> innerKeySelector,
            Func<T1, T2, R> resultSelector,
            IEqualityComparer<Key> comparer = null)
        {
            if (outer == null
                || outerKeySelector == null
                || innerKeySelector == null
                || resultSelector == null)
                return new List<R>();
            comparer = comparer ?? new GenericEqualityComparer<Key>();
            return inner.ForEach(x => new { left = outer.FirstOrDefault(y => comparer.Equals(innerKeySelector(x), outerKeySelector(y))), right = x })
                        .ForEach(x => resultSelector(x.left, x.right));
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for all items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="list">The item</param>
        /// <param name="predicate">Predicate to check</param>
        /// <param name="exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAll<T>(this IEnumerable<T> list, Func<T, bool> predicate, Func<Exception> exception)
        {
            if (list == null)
                return new List<T>();
            if (predicate == null || exception == null)
                return list;
            if (list.All(predicate))
                throw exception();
            return list;
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for all items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="list">The item</param>
        /// <param name="predicate">Predicate to check</param>
        /// <param name="exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAll<T>(this IEnumerable<T> list, Func<T, bool> predicate, Exception exception)
        {
            if (list == null)
                return new List<T>();
            if (predicate == null || exception == null)
                return list;
            if (list.All(predicate))
                throw exception;
            return list;
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for any items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="list">The item</param>
        /// <param name="predicate">Predicate to check</param>
        /// <param name="exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAny<T>(this IEnumerable<T> list, Func<T, bool> predicate, Func<Exception> exception)
        {
            if (list == null)
                return new List<T>();
            if (predicate == null || exception == null)
                return list;
            if (list.Any(predicate))
                throw exception();
            return list;
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for any items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="list">The item</param>
        /// <param name="predicate">Predicate to check</param>
        /// <param name="exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAny<T>(this IEnumerable<T> list, Func<T, bool> predicate, Exception exception)
        {
            if (list == null)
                return new List<T>();
            if (predicate == null || exception == null)
                return list;
            if (list.Any(predicate))
                throw exception;
            return list;
        }

        /// <summary>
        /// Converts a list to an array
        /// </summary>
        /// <typeparam name="Source">Source type</typeparam>
        /// <typeparam name="Target">Target type</typeparam>
        /// <param name="list">List to convert</param>
        /// <param name="convertingFunction">Function used to convert each item</param>
        /// <returns>The array containing the items from the list</returns>
        public static Target[] ToArray<Source, Target>(this IEnumerable<Source> list, Func<Source, Target> convertingFunction)
        {
            if (list == null || convertingFunction == null)
                return new Target[0];
            return list.ForEach(convertingFunction).ToArray();
        }

        /// <summary>
        /// Converts an IEnumerable to a list
        /// </summary>
        /// <typeparam name="Source">Source type</typeparam>
        /// <typeparam name="Target">Target type</typeparam>
        /// <param name="list">IEnumerable to convert</param>
        /// <param name="convertingFunction">Function used to convert each item</param>
        /// <returns>The list containing the items from the IEnumerable</returns>
        public static List<Target> ToList<Source, Target>(this IEnumerable<Source> list, Func<Source, Target> convertingFunction)
        {
            if (list == null || convertingFunction == null)
                return new List<Target>();
            return list.ForEach(convertingFunction).ToList();
        }

        /// <summary>
        /// Converts the list to a string where each item is seperated by the Seperator
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="list">List to convert</param>
        /// <param name="itemOutput">
        /// Used to convert the item to a string (defaults to calling ToString)
        /// </param>
        /// <param name="seperator">Seperator to use between items (defaults to ,)</param>
        /// <returns>The string version of the list</returns>
        public static string ToString<T>(this IEnumerable<T> list, Func<T, string> itemOutput = null, string seperator = ",")
        {
            if (list == null)
                return "";
            seperator = seperator ?? "";
            itemOutput = itemOutput ?? (x => x.ToString());
            var Builder = new StringBuilder();
            string TempSeperator = "";
            list.ForEach(x =>
            {
                Builder.Append(TempSeperator).Append(itemOutput(x));
                TempSeperator = seperator;
            });
            return Builder.ToString();
        }

        /// <summary>
        /// Transverses a hierarchy given the child elements getter.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="collection">The collection hierarchy.</param>
        /// <param name="property">The child elements getter.</param>
        /// <returns>The transversed hierarchy.</returns>
        public static IEnumerable<T> Transverse<T>(this IEnumerable<T> collection, Func<T, IEnumerable<T>> property)
        {
            if (collection == null)
                yield break;
            foreach (T item in collection)
            {
                yield return item;
                foreach (T inner in Transverse(property(item), property))
                    yield return inner;
            }
        }

        /// <summary>
        /// Transverses a hierarchy given the child elements getter.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="item">The root node of the hierarchy.</param>
        /// <param name="property">The child elements getter.</param>
        /// <returns>The transversed hierarchy.</returns>
        public static IEnumerable<T> Transverse<T>(this T item, Func<T, IEnumerable<T>> property)
        {
            if (Equals(item, default(T)))
                yield break;
            yield return item;
            foreach (T inner in Transverse(property(item), property))
                yield return inner;
        }
    }
}