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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
            if (enumerable1 == null)
                return null;
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
                return null;
            var TempGenericComparer = new GenericEqualityComparer<T>();
            predicate = predicate ?? TempGenericComparer.Equals;
            var Results = new List<T>();
            foreach (T Item in enumerable)
            {
                bool Found = false;
                foreach (T Item2 in Results)
                {
                    if (predicate(Item, Item2))
                    {
                        Found = true;
                        break;
                    }
                }
                if (!Found)
                    Results.Add(Item);
            }
            return Results;
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
                return null;
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
            var ReturnList = new List<T>();
            for (int x = start; x < end; ++x)
                ReturnList.Add(TempList[x]);
            return ReturnList;
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
                return null;
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
                return null;
            var TempList = list.ElementsBetween(start, end).ToArray();
            for (int x = 0; x < TempList.Length; ++x)
            {
                action(TempList[x], x);
            }
            return list;
        }

        //TODO: Above has been changed, below is where we start with next batch of changes.

        /// <summary>
        /// Does a function for each item in the IEnumerable between the start and end indexes and
        /// returns an IEnumerable of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> For<T, R>(this IEnumerable<T> List, int Start, int End, Func<T, R> Function)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Function != null, "Function");
            Contract.Requires<ArgumentException>(End + 1 - Start >= 0, "End must be greater than start");
            var ReturnValues = new List<R>();
            foreach (T Item in List.ElementsBetween(Start, End + 1))
                ReturnValues.Add(Function(Item));
            return ReturnValues;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> List, Action<T> Action)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Action != null, "Action");
            foreach (T Item in List)
                Action(Item);
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> List, Func<T, R> Function)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Function != null, "Function");
            var ReturnValues = new List<R>();
            foreach (T Item in List)
                ReturnValues.Add(Function(Item));
            return ReturnValues;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <param name="CatchAction">Action that occurs if an exception occurs</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> List, Action<T> Action, Action<T, Exception> CatchAction)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Action != null, "Action");
            Contract.Requires<ArgumentNullException>(CatchAction != null, "CatchAction");
            foreach (T Item in List)
            {
                try
                {
                    Action(Item);
                }
                catch (Exception e) { CatchAction(Item, e); }
            }
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <param name="CatchAction">Action that occurs if an exception occurs</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEach<T, R>(this IEnumerable<T> List, Func<T, R> Function, Action<T, Exception> CatchAction)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Function != null, "Function");
            Contract.Requires<ArgumentNullException>(CatchAction != null, "CatchAction");
            var ReturnValues = new List<R>();
            foreach (T Item in List)
            {
                try
                {
                    ReturnValues.Add(Function(Item));
                }
                catch (Exception e) { CatchAction(Item, e); }
            }
            return ReturnValues;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEachParallel<T>(this IEnumerable<T> List, Action<T> Action)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Action != null, "Action");
            Parallel.ForEach(List, Action);
            return List;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The results in an IEnumerable list</returns>
        public static IEnumerable<R> ForEachParallel<T, R>(this IEnumerable<T> List, Func<T, R> Function)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Function != null, "Function");
            return List.ForParallel(0, List.Count() - 1, Function);
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <param name="CatchAction">Action that occurs if an exception occurs</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEachParallel<T>(this IEnumerable<T> List, Action<T> Action, Action<T, Exception> CatchAction)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Action != null, "Action");
            Contract.Requires<ArgumentNullException>(CatchAction != null, "CatchAction");
            Parallel.ForEach<T>(List, delegate (T Item)
            {
                try
                {
                    Action(Item);
                }
                catch (Exception e) { CatchAction(Item, e); }
            });
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable, returning a list of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Function">Function to do</param>
        /// <param name="CatchAction">Action that occurs if an exception occurs</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForEachParallel<T, R>(this IEnumerable<T> List, Func<T, R> Function, Action<T, Exception> CatchAction)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Function != null, "Function");
            Contract.Requires<ArgumentNullException>(CatchAction != null, "CatchAction");
            var ReturnValues = new List<R>();
            Parallel.ForEach<T>(List, delegate (T Item)
            {
                try
                {
                    ReturnValues.Add(Function(Item));
                }
                catch (Exception e) { CatchAction(Item, e); }
            });
            return ReturnValues;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForParallel<T>(this IEnumerable<T> List, int Start, int End, Action<T> Action)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Action != null, "Action");
            Contract.Requires<ArgumentException>(End + 1 - Start >= 0, "End must be greater than start");
            Parallel.For(Start, End + 1, new Action<int>(x => Action(List.ElementAt(x))));
            return List;
        }

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Results type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> ForParallel<T, R>(this IEnumerable<T> List, int Start, int End, Func<T, R> Function)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Function != null, "Function");
            Contract.Requires<ArgumentException>(End + 1 - Start >= 0, "End must be greater than start");
            R[] Results = new R[(End + 1) - Start];
            Parallel.For(Start, End + 1, new Action<int>(x => Results[x - Start] = Function(List.ElementAt(x))));
            return Results;
        }

        /// <summary>
        /// Returns the last X number of items from the list
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Count">Numbers of items to return</param>
        /// <returns>The last X items from the list</returns>
        public static IEnumerable<T> Last<T>(this IEnumerable<T> List, int Count)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            return List.ElementsBetween(List.Count() - Count, List.Count());
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
            Contract.Requires<ArgumentNullException>(inner != null, "inner");
            Contract.Requires<ArgumentNullException>(outerKeySelector != null, "outerKeySelector");
            Contract.Requires<ArgumentNullException>(innerKeySelector != null, "innerKeySelector");
            Contract.Requires<ArgumentNullException>(resultSelector != null, "resultSelector");

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
            Contract.Requires<ArgumentNullException>(inner != null, "inner");
            Contract.Requires<ArgumentNullException>(outerKeySelector != null, "outerKeySelector");
            Contract.Requires<ArgumentNullException>(innerKeySelector != null, "innerKeySelector");
            Contract.Requires<ArgumentNullException>(resultSelector != null, "resultSelector");

            var Left = outer.LeftJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            var Right = outer.RightJoin(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
            return Left.Union(Right);
        }

        /// <summary>
        /// Determines the position of an object if it is present, otherwise it returns -1
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">List of objects to search</param>
        /// <param name="Object">Object to find the position of</param>
        /// <param name="EqualityComparer">
        /// Equality comparer used to determine if the object is present
        /// </param>
        /// <returns>The position of the object if it is present, otherwise -1</returns>
        public static int PositionOf<T>(this IEnumerable<T> List, T Object, IEqualityComparer<T> EqualityComparer = null)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            EqualityComparer = EqualityComparer.Check(() => new GenericEqualityComparer<T>());
            int Count = 0;
            foreach (T Item in List)
            {
                if (EqualityComparer.Equals(Object, Item))
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
            Contract.Requires<ArgumentNullException>(outer != null, "outer");
            Contract.Requires<ArgumentNullException>(outerKeySelector != null, "outerKeySelector");
            Contract.Requires<ArgumentNullException>(innerKeySelector != null, "innerKeySelector");
            Contract.Requires<ArgumentNullException>(resultSelector != null, "resultSelector");

            comparer = comparer ?? new GenericEqualityComparer<Key>();
            return inner.ForEach(x => new { left = outer.FirstOrDefault(y => comparer.Equals(innerKeySelector(x), outerKeySelector(y))), right = x })
                        .ForEach(x => resultSelector(x.left, x.right));
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for all items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAll<T>(this IEnumerable<T> List, Predicate<T> Predicate, Func<Exception> Exception)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Predicate != null, "Predicate");
            Contract.Requires<ArgumentNullException>(Exception != null, "Exception");
            foreach (T Item in List)
            {
                if (!Predicate(Item))
                    return List;
            }
            throw Exception();
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for all items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAll<T>(this IEnumerable<T> List, Predicate<T> Predicate, Exception Exception)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Predicate != null, "Predicate");
            Contract.Requires<ArgumentNullException>(Exception != null, "Exception");
            foreach (T Item in List)
            {
                if (!Predicate(Item))
                    return List;
            }
            throw Exception;
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for any items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAny<T>(this IEnumerable<T> List, Predicate<T> Predicate, Func<Exception> Exception)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Predicate != null, "Predicate");
            Contract.Requires<ArgumentNullException>(Exception != null, "Exception");
            foreach (T Item in List)
            {
                if (Predicate(Item))
                    throw Exception();
            }
            return List;
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for any items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static IEnumerable<T> ThrowIfAny<T>(this IEnumerable<T> List, Predicate<T> Predicate, Exception Exception)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Predicate != null, "Predicate");
            Contract.Requires<ArgumentNullException>(Exception != null, "Exception");
            foreach (T Item in List)
            {
                if (Predicate(Item))
                    throw Exception;
            }
            return List;
        }

        /// <summary>
        /// Converts the IEnumerable to a DataTable
        /// </summary>
        /// <typeparam name="T">Type of the objects in the IEnumerable</typeparam>
        /// <param name="List">List to convert</param>
        /// <param name="Columns">Column names (if empty, uses property names)</param>
        /// <returns>The list as a DataTable</returns>
        public static DataTable To<T>(this IEnumerable<T> List, params string[] Columns)
        {
            var ReturnValue = new DataTable();
            ReturnValue.Locale = CultureInfo.CurrentCulture;
            if (List == null || List.Count() == 0)
                return ReturnValue;
            PropertyInfo[] Properties = typeof(T).GetProperties();
            if (Columns.Length == 0)
                Columns = Properties.ToArray(x => x.Name);
            Columns.ForEach(x => ReturnValue.Columns.Add(x, Properties.FirstOrDefault(z => z.Name == x).PropertyType));
            object[] Row = new object[Columns.Length];
            foreach (T Item in List)
            {
                for (int x = 0; x < Row.Length; ++x)
                {
                    Row[x] = Properties.FirstOrDefault(z => z.Name == Columns[x]).GetValue(Item, new object[] { });
                }
                ReturnValue.Rows.Add(Row);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Converts the IEnumerable to a DataTable
        /// </summary>
        /// <param name="List">List to convert</param>
        /// <param name="Columns">Column names (if empty, uses property names)</param>
        /// <returns>The list as a DataTable</returns>
        public static DataTable To(this IEnumerable List, params string[] Columns)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            var ReturnValue = new DataTable();
            ReturnValue.Locale = CultureInfo.CurrentCulture;
            int Count = 0;
            var i = List.GetEnumerator();
            while (i.MoveNext())
                ++Count;
            if (List == null || Count == 0)
                return ReturnValue;
            IEnumerator ListEnumerator = List.GetEnumerator();
            ListEnumerator.MoveNext();
            PropertyInfo[] Properties = ListEnumerator.Current.GetType().GetProperties();
            if (Columns.Length == 0)
                Columns = Properties.ToArray(x => x.Name);
            Columns.ForEach(x => ReturnValue.Columns.Add(x, Properties.FirstOrDefault(z => z.Name == x).PropertyType));
            object[] Row = new object[Columns.Length];
            foreach (object Item in List)
            {
                for (int x = 0; x < Row.Length; ++x)
                {
                    Row[x] = Properties.FirstOrDefault(z => z.Name == Columns[x]).GetValue(Item, new object[] { });
                }
                ReturnValue.Rows.Add(Row);
            }
            return ReturnValue;
        }

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
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(ConvertingFunction != null, "ConvertingFunction");
            return List.ForEach(ConvertingFunction).ToArray();
        }

        /// <summary>
        /// Converts an IEnumerable to a list
        /// </summary>
        /// <typeparam name="Source">Source type</typeparam>
        /// <typeparam name="Target">Target type</typeparam>
        /// <param name="List">IEnumerable to convert</param>
        /// <param name="ConvertingFunction">Function used to convert each item</param>
        /// <returns>The list containing the items from the IEnumerable</returns>
        public static List<Target> ToList<Source, Target>(this IEnumerable<Source> List, Func<Source, Target> ConvertingFunction)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(ConvertingFunction != null, "ConvertingFunction");
            return List.ForEach(ConvertingFunction).ToList();
        }

        /// <summary>
        /// Converts the IEnumerable to an observable list
        /// </summary>
        /// <typeparam name="Source">The type of the source.</typeparam>
        /// <typeparam name="Target">The type of the target.</typeparam>
        /// <param name="List">The list to convert</param>
        /// <param name="ConvertingFunction">The converting function.</param>
        /// <returns>The observable list version of the original list</returns>
        public static ObservableList<Target> ToObservableList<Source, Target>(this IEnumerable<Source> List, Func<Source, Target> ConvertingFunction)
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(ConvertingFunction != null, "ConvertingFunction");
            return new ObservableList<Target>(List.ForEach(ConvertingFunction));
        }

        /// <summary>
        /// Converts the list to a string where each item is seperated by the Seperator
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">List to convert</param>
        /// <param name="ItemOutput">
        /// Used to convert the item to a string (defaults to calling ToString)
        /// </param>
        /// <param name="Seperator">Seperator to use between items (defaults to ,)</param>
        /// <returns>The string version of the list</returns>
        public static string ToString<T>(this IEnumerable<T> List, Func<T, string> ItemOutput = null, string Seperator = ",")
        {
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Seperator = Seperator.Check("");
            ItemOutput = ItemOutput.Check(x => x.ToString());
            var Builder = new StringBuilder();
            string TempSeperator = "";
            List.ForEach(x =>
            {
                Builder.Append(TempSeperator).Append(ItemOutput(x));
                TempSeperator = Seperator;
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
            if (item == null)
                yield break;

            yield return item;

            foreach (T inner in Transverse(property(item), property))
                yield return inner;
        }
    }
}