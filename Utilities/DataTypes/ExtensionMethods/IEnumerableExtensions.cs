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
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        #region ElementsBetween

        /// <summary>
        /// Returns elements starting at the index and ending at the end index
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">List to search</param>
        /// <param name="Start">Start index (inclusive)</param>
        /// <param name="End">End index (exclusive)</param>
        /// <returns>The items between the start and end index</returns>
        public static IEnumerable<T> ElementsBetween<T>(this IEnumerable<T> List, int Start, int End)
        {
            if (List.IsNull())
                return List;
            if (End > List.Count())
                End = List.Count();
            if (Start < 0)
                Start = 0;
            System.Collections.Generic.List<T> ReturnList = new System.Collections.Generic.List<T>();
            for (int x = Start; x < End; ++x)
                ReturnList.Add(List.ElementAt(x));
            return ReturnList;
        }

        #endregion

        #region FalseForAll

        /// <summary>
        /// Determines if the predicates are false for each item in a list
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="List">IEnumerable to look through</param>
        /// <param name="Predicates">Predicates to use to check the IEnumerable</param>
        /// <returns>True if they all fail all of the predicates, false otherwise</returns>
        public static bool FalseForAll<T>(this IEnumerable<T> List, params Predicate<T>[] Predicates)
        {
            List.ThrowIfNull("List");
            Predicates.ThrowIfNull("Predicate");
            foreach (Predicate<T> Predicate in Predicates)
                if (List.All(x => Predicate(x)))
                    return false;
            return true;
        }

        #endregion

        #region FalseForAny

        /// <summary>
        /// Determines if the predicates are false for any item in a list
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="List">IEnumerable to look through</param>
        /// <param name="Predicates">Predicates to use to check the IEnumerable</param>
        /// <returns>True if any fail any of the predicates, false otherwise</returns>
        public static bool FalseForAny<T>(this IEnumerable<T> List, params Predicate<T>[] Predicates)
        {
            List.ThrowIfNull("List");
            Predicates.ThrowIfNull("Predicate");
            foreach (Predicate<T> Predicate in Predicates)
                if (List.Any(x => !Predicate(x)))
                    return true;
            return false;
        }

        #endregion

        #region First

        /// <summary>
        /// Returns the first X number of items from the list
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Count">Numbers of items to return</param>
        /// <returns>The first X items from the list</returns>
        public static IEnumerable<T> First<T>(this IEnumerable<T> List, int Count)
        {
            List.ThrowIfNull("List");
            return List.ElementsBetween(0, Count);
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
            foreach (T Item in List.ElementsBetween(Start, End + 1))
                Action(Item);
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
        public static IEnumerable<R> For<T, R>(this IEnumerable<T> List, int Start, int End, Func<T, R> Function)
        {
            List.ThrowIfNull("List");
            Function.ThrowIfNull("Function");
            List<R> ReturnValues = new List<R>();
            foreach (T Item in List.ElementsBetween(Start, End + 1))
                    ReturnValues.Add(Function(Item));
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

        #region Last

        /// <summary>
        /// Returns the last X number of items from the list
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">IEnumerable to iterate over</param>
        /// <param name="Count">Numbers of items to return</param>
        /// <returns>The last X items from the list</returns>
        public static IEnumerable<T> Last<T>(this IEnumerable<T> List, int Count)
        {
            List.ThrowIfNull("List");
            return List.ElementsBetween(List.Count() - Count, List.Count());
        }

        #endregion

        #region PositionOf

        /// <summary>
        /// Determines the position of an object if it is present, otherwise it returns -1
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="List">List of objects to search</param>
        /// <param name="Object">Object to find the position of</param>
        /// <param name="EqualityComparer">Equality comparer used to determine if the object is present</param>
        /// <returns>The position of the object if it is present, otherwise -1</returns>
        public static int PositionOf<T>(this IEnumerable<T> List, T Object, IEqualityComparer<T> EqualityComparer = null)
        {
            List.ThrowIfNull("List");
            EqualityComparer = EqualityComparer.NullCheck(()=>new GenericEqualityComparer<T>());
            int Count = 0;
            foreach (T Item in List)
            {
                if (EqualityComparer.Equals(Object, Item))
                    return Count;
                ++Count;
            }
            return -1;
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
                return Value;
            EqualityComparer=EqualityComparer.NullCheck(()=>new GenericEqualityComparer<T>());
            return Value.Where(x => !x.IsDefault(EqualityComparer));
        }

        #endregion

        #region ThrowIfTrueForAll

        /// <summary>
        /// Throws the specified exception if the predicates are true for all items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <typeparam name="E">Exception type</typeparam>
        /// <param name="Items">The list</param>
        /// <param name="Predicates">Predicates to check</param>
        /// <param name="Exception">Exception to throw if predicates are true</param>
        /// <returns>the original IEnumerable</returns>
        public static IEnumerable<T> ThrowIfTrueForAll<T, E>(this IEnumerable<T> Items, E Exception, params Predicate<T>[] Predicates) where E : Exception
        {
            Predicates.ThrowIfNull("Predicates");
            Exception.ThrowIfNull("Exception");
            if (Items.TrueForAll(Predicates))
                throw Exception;
            return Items;
        }

        #endregion

        #region ThrowIfFalseForAll

        /// <summary>
        /// Throws the specified exception if the predicates are false for all items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <typeparam name="E">Exception type</typeparam>
        /// <param name="Items">The list</param>
        /// <param name="Predicates">Predicates to check</param>
        /// <param name="Exception">Exception to throw if predicates are false</param>
        /// <returns>the original list</returns>
        public static IEnumerable<T> ThrowIfFalseForAll<T, E>(this IEnumerable<T> Items, E Exception, params Predicate<T>[] Predicates) where E : Exception
        {
            Predicates.ThrowIfNull("Predicates");
            Exception.ThrowIfNull("Exception");
            if (Items.FalseForAll(Predicates))
                throw Exception;
            return Items;
        }

        #endregion

        #region ThrowIfTrueForAny

        /// <summary>
        /// Throws the specified exception if the predicate is true for any items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <typeparam name="E">Exception type</typeparam>
        /// <param name="Items">The list</param>
        /// <param name="Predicates">Predicates to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original IEnumerable</returns>
        public static IEnumerable<T> ThrowIfTrueForAny<T, E>(this IEnumerable<T> Items, E Exception, params Predicate<T>[] Predicates) where E : Exception
        {
            Predicates.ThrowIfNull("Predicates");
            Exception.ThrowIfNull("Exception");
            if (Items.TrueForAny(Predicates))
                throw Exception;
            return Items;
        }

        #endregion

        #region ThrowIfFalseForAny

        /// <summary>
        /// Throws the specified exception if the predicates are false for any items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <typeparam name="E">Exception type</typeparam>
        /// <param name="Items">The list</param>
        /// <param name="Predicates">Predicates to check</param>
        /// <param name="Exception">Exception to throw if predicates are false</param>
        /// <returns>the original list</returns>
        public static IEnumerable<T> ThrowIfFalseForAny<T, E>(this IEnumerable<T> Items, E Exception, params Predicate<T>[] Predicates) where E : Exception
        {
            Predicates.ThrowIfNull("Predicates");
            Exception.ThrowIfNull("Exception");
            if (Items.FalseForAny(Predicates))
                throw Exception;
            return Items;
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

        #region ToDataTable

        /// <summary>
        /// Converts the IEnumerable to a DataTable
        /// </summary>
        /// <typeparam name="T">Type of the objects in the IEnumerable</typeparam>
        /// <param name="List">List to convert</param>
        /// <param name="Columns">Column names (if empty, uses property names)</param>
        /// <returns>The list as a DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> List, params string[] Columns)
        {
            DataTable ReturnValue = new DataTable();
            if (List.IsNullOrEmpty())
                return ReturnValue;
            PropertyInfo[] Properties = typeof(T).GetProperties();
            if(Columns.Length==0)
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
        /// Determines if the predicates are true for each item in a list
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="List">IEnumerable to look through</param>
        /// <param name="Predicates">Predicates to use to check the IEnumerable</param>
        /// <returns>True if they all pass all of the predicates, false otherwise</returns>
        public static bool TrueForAll<T>(this IEnumerable<T> List, params Predicate<T>[] Predicates)
        {
            List.ThrowIfNull("List");
            Predicates.ThrowIfNull("Predicate");
            foreach (Predicate<T> Predicate in Predicates)
                if (List.Any(x => !Predicate(x)))
                    return false;
            return true;
        }

        #endregion

        #region TrueForAny

        /// <summary>
        /// Determines if the predicates are true for any item in a list
        /// </summary>
        /// <typeparam name="T">The type of the items in the list</typeparam>
        /// <param name="List">IEnumerable to look through</param>
        /// <param name="Predicates">Predicates to use to check the IEnumerable</param>
        /// <returns>True if any pass any of the predicates, false otherwise</returns>
        public static bool TrueForAny<T>(this IEnumerable<T> List, params Predicate<T>[] Predicates)
        {
            List.ThrowIfNull("List");
            Predicates.ThrowIfNull("Predicate");
            foreach (Predicate<T> Predicate in Predicates)
                if (List.Any(x => Predicate(x)))
                    return true;
            return false;
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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