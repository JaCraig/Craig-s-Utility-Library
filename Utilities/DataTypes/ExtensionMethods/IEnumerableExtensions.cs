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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilities.DataTypes.Comparison;
#endregion

namespace Utilities.DataTypes
{
    /// <summary>
    /// IEnumerable extensions
    /// </summary>
    public static class IEnumerableExtensions
    {
        #region Functions

        #region Concat

        /// <summary>
        /// Combines multiple IEnumerables together and returns a new IEnumerable containing all of the values
        /// </summary>
        /// <typeparam name="T">Type of the data in the IEnumerable</typeparam>
        /// <param name="Enumerable1">IEnumerable 1</param>
        /// <param name="Additions">IEnumerables to concat onto the first item</param>
        /// <returns>A new IEnumerable containing all values</returns>
        /// <example>
        /// <code>
        ///  int[] TestObject1 = new int[] { 1, 2, 3 };
        ///  int[] TestObject2 = new int[] { 4, 5, 6 };
        ///  int[] TestObject3 = new int[] { 7, 8, 9 };
        ///  TestObject1 = TestObject1.Concat(TestObject2, TestObject3).ToArray();
        /// </code>
        /// </example>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> Enumerable1, params IEnumerable<T>[] Additions)
        {
            Contract.Requires<ArgumentNullException>(Enumerable1 != null, "Enumerable1");
            Contract.Requires<ArgumentNullException>(Additions != null, "Additions");
            Contract.Requires<ArgumentNullException>(Contract.ForAll(Additions, x => x != null), "Additions");
            List<T> Results = new List<T>();
            Results.AddRange(Enumerable1);
            for (int x = 0; x < Additions.Length; ++x)
                Results.AddRange(Additions[x]);
            return Results;
        }

        #endregion

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
            if (List == null)
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
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Action != null, "Action");
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
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Function != null, "Function");
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
            List<R> ReturnValues = new List<R>();
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
            List<R> ReturnValues = new List<R>();
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
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(Action != null, "Action");
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
            Parallel.ForEach<T>(List, delegate(T Item)
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
            List<R> ReturnValues = new List<R>();
            Parallel.ForEach<T>(List, delegate(T Item)
            {
                try
                {
                    ReturnValues.Add(Function(Item));
                }
                catch (Exception e) { CatchAction(Item, e); }
            });
            return ReturnValues;
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
            Contract.Requires<ArgumentNullException>(List != null, "List");
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

        #endregion

        #region Remove

        /// <summary>
        /// Removes values from a list that meet the criteria set forth by the predicate
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="Value">List to cull items from</param>
        /// <param name="Predicate">Predicate that determines what items to remove</param>
        /// <returns>An IEnumerable with the objects that meet the criteria removed</returns>
        public static IEnumerable<T> Remove<T>(this IEnumerable<T> Value, Func<T, bool> Predicate)
        {
            Contract.Requires<ArgumentNullException>(Predicate != null, "Predicate");
            if (Value == null)
                return Value;
            return Value.Where(x => !Predicate(x));
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
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Contract.Requires<ArgumentNullException>(ConvertingFunction != null, "ConvertingFunction");
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

        #endregion

        #region ToList

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
            Contract.Requires<ArgumentNullException>(List != null, "List");
            Seperator = Seperator.Check("");
            ItemOutput = ItemOutput.Check(x => x.ToString());
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

        #region ThrowIfAll

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
            foreach (T Item in List)
            {
                if (!Predicate(Item))
                    return List;
            }
            throw Exception;
        }

        #endregion

        #region ThrowIfAny

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
            foreach (T Item in List)
            {
                if (Predicate(Item))
                    throw Exception;
            }
            return List;
        }

        #endregion

        #endregion
    }
}