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
using System.Threading.Tasks;
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// IEnumerable extensions
    /// </summary>
    public static class IEnumerableExtensions
    {
        #region Functions

        #region Exists

        /// <summary>
        /// Used to determine if an item in the IEnumerable matches a predicate
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">List to search</param>
        /// <param name="Match">The predicate used to check if something exists</param>
        /// <returns>True if at least one item matches the predicate, false otherwise</returns>
        public static bool Exists<T>(this IEnumerable<T> List,Predicate<T> Match)
        {
            Match.ThrowIfNull("Match");
            if (List.IsNull())
                return false;
            foreach (T Item in List)
            {
                if (Match(Item))
                    return true;
            }
            return false;
        }

        #endregion

        #region For

        /// <summary>
        /// Does an action for each item in the IEnumerable between the start and end indexes
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> For<T>(this IEnumerable<T> List, int Start, int End, Action<T> Action)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
            int x = 0;
            foreach (T Item in List)
            {
                if (x.Between(Start, End))
                    Action(Item);
                ++x;
                if (x > End)
                    break;
            }
            return List;
        }

        /// <summary>
        /// Does a function for each item in the IEnumerable between the start and end indexes and returns an IEnumerable of the results
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Start">Item to start with</param>
        /// <param name="End">Item to end with</param>
        /// <param name="Function">Function to do</param>
        /// <returns>The resulting list</returns>
        public static IEnumerable<R> For<T,R>(this IEnumerable<T> List, int Start, int End, Func<T,R> Function)
        {
            List.ThrowIfNull("List");
            Function.ThrowIfNull("Function");
            int x = 0;
            List<R> ReturnValues = new List<R>();
            foreach (T Item in List)
            {
                if (x.Between(Start, End))
                    ReturnValues.Add(Function(Item));
                ++x;
                if (x > End)
                    break;
            }
            return ReturnValues;
        }

        #endregion

        #region ForEach

        /// <summary>
        /// Does an action for each item in the IEnumerable
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> List, Action<T> Action)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
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
            List.ThrowIfNull("List");
            Function.ThrowIfNull("Function");
            List<R> ReturnValues = new List<R>();
            foreach (T Item in List)
                ReturnValues.Add(Function(Item));
            return ReturnValues;
        }

        #endregion

        #region ForParallel

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
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
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
            List.ThrowIfNull("List");
            Function.ThrowIfNull("Function");
            R[] Results = new R[(End + 1) - Start];
            Parallel.For(Start, End + 1, new Action<int>(x => Results[x - Start] = Function(List.ElementAt(x))));
            return Results;
        }

        #endregion

        #region ForEachParallel

        /// <summary>
        /// Does an action for each item in the IEnumerable in parallel
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Action">Action to do</param>
        /// <returns>The original list</returns>
        public static IEnumerable<T> ForEachParallel<T>(this IEnumerable<T> List, Action<T> Action)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
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
            List.ThrowIfNull("List");
            Function.ThrowIfNull("Function");
            return List.ForParallel(0, List.Count() - 1, Function);
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
            return Value.IsNull() || Value.Count() == 0;
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
            if (Value.IsNull())
                yield break;
            EqualityComparer = EqualityComparer.NullCheck(new GenericEqualityComparer<T>());
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
            List.ThrowIfNull("List");
            ConvertingFunction.ThrowIfNull("ConvertingFunction");
            return List.ForEach(ConvertingFunction).ToArray();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Converts the list to a string where each item is seperated by the Seperator
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="List">List to convert</param>
        /// <param name="ItemOutput">Used to convert the item to a string (defaults to calling ToString)</param>
        /// <param name="Seperator">Seperator to use between items (defaults to ,)</param>
        /// <returns>The string version of the list</returns>
        public static string ToString<T>(this IEnumerable<T> List, Func<T, string> ItemOutput = null, string Seperator = ",")
        {
            List.ThrowIfNull("List");
            Seperator = Seperator.NullCheck("");
            ItemOutput = ItemOutput.NullCheck(x => x.ToString());
            StringBuilder Builder = new StringBuilder();
            string TempSeperator = "";
            List.ForEach(x =>
            {
                Builder.Append(TempSeperator).Append(ItemOutput(x));
                TempSeperator = Seperator;
            });
            return Builder.ToString();
        }

        #endregion

        #region TrueForAll

        /// <summary>
        /// Determines if a predicate is true for each item in a list
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="List">IEnumerable to look through</param>
        /// <param name="Predicate">Predicate to use to check the IEnumerable</param>
        /// <returns>True if they all pass the predicate, false otherwise</returns>
        public static bool TrueForAll<T>(this IEnumerable<T> List, Predicate<T> Predicate)
        {
            List.ThrowIfNull("List");
            Predicate.ThrowIfNull("Predicate");
            return !List.Any(x => !Predicate(x));
        }

        #endregion

        #region TryAll

        /// <summary>
        /// Tries to do the action on each item in the list. If an exception is thrown,
        /// it does the catch action on the item (if it is not null).
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="List">IEnumerable to look through</param>
        /// <param name="Action">Action to run on each item</param>
        /// <param name="CatchAction">Catch action (defaults to null)</param>
        /// <returns>The list after the action is run on everything</returns>
        public static IEnumerable<T> TryAll<T>(this IEnumerable<T> List, Action<T> Action, Action<T> CatchAction = null)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
            foreach (T Item in List)
            {
                try
                {
                    Action(Item);
                }
                catch { if (CatchAction != null) CatchAction(Item); }
            }
            return List;
        }

        #endregion

        #region TryAllParallel

        /// <summary>
        /// Tries to do the action on each item in the list. If an exception is thrown,
        /// it does the catch action on the item (if it is not null). This is done in
        /// parallel.
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="List">IEnumerable to look through</param>
        /// <param name="Action">Action to run on each item</param>
        /// <param name="CatchAction">Catch action (defaults to null)</param>
        /// <returns>The list after the action is run on everything</returns>
        public static IEnumerable<T> TryAllParallel<T>(this IEnumerable<T> List, Action<T> Action, Action<T> CatchAction = null)
        {
            List.ThrowIfNull("List");
            Action.ThrowIfNull("Action");
            Parallel.ForEach<T>(List, delegate(T Item)
            {
                try
                {
                    Action(Item);
                }
                catch { if (CatchAction != null) CatchAction(Item); }
            });
            return List;
        }

        #endregion

        #endregion
    }
}