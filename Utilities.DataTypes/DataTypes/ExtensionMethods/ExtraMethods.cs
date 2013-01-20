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
using System.Threading;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Data.Entity.Design.PluralizationServices;
using System.Dynamic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Threading.Tasks;

#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// Various extension methods
    /// </summary>
    public static class ExtraMethods
    {
        #region Functions

        #region Async

        /// <summary>
        /// Runs an action async
        /// </summary>
        /// <param name="Action">Action to run</param>
        public static void Async(this Action Action)
        {
            new Thread(Action.Invoke).Start();
        }

        #endregion

        #region Execute

        /// <summary>
        /// Executes a function, repeating it a number of times in case it fails
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="Function">Function to run</param>
        /// <param name="Attempts">Number of times to attempt it</param>
        /// <param name="RetryDelay">The amount of milliseconds to wait between tries</param>
        /// <param name="TimeOut">Max amount of time to wait for the function to run (waits for the current attempt to finish before checking)</param>
        /// <returns>The returned value from the function</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static T Execute<T>(this Func<T> Function, int Attempts = 3, int RetryDelay = 0, int TimeOut = int.MaxValue)
        {
            Contract.Requires<ArgumentNullException>(Function != null, "Function");
            Exception Holder = null;
            long Start = System.Environment.TickCount;
            while (Attempts > 0)
            {
                try
                {
                    return Function();
                }
                catch (Exception e) { Holder = e; }
                if (System.Environment.TickCount - Start > TimeOut)
                    break;
                Thread.Sleep(RetryDelay);
                --Attempts;
            }
            throw Holder;
        }

        /// <summary>
        /// Executes an action, repeating it a number of times in case it fails
        /// </summary>
        /// <param name="Action">Action to run</param>
        /// <param name="Attempts">Number of times to attempt it</param>
        /// <param name="RetryDelay">The amount of milliseconds to wait between tries</param>
        /// <param name="TimeOut">Max amount of time to wait for the function to run (waits for the current attempt to finish before checking)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void Execute(this Action Action, int Attempts = 3, int RetryDelay = 0, int TimeOut = int.MaxValue)
        {
            Contract.Requires<ArgumentNullException>(Action != null, "Action");
            Exception Holder = null;
            long Start = System.Environment.TickCount;
            while (Attempts > 0)
            {
                try
                {
                    Action();
                }
                catch (Exception e) { Holder = e; }
                if (System.Environment.TickCount - Start > TimeOut)
                    break;
                Thread.Sleep(RetryDelay);
                --Attempts;
            }
            if (Holder!=null)
                throw Holder;
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

        #region Pluralize

        /// <summary>
        /// Pluralizes a word
        /// </summary>
        /// <param name="Word">Word to pluralize</param>
        /// <param name="Culture">Culture info used to pluralize the word (defaults to current culture)</param>
        /// <returns>The word pluralized</returns>
        public static string Pluralize(this string Word, CultureInfo Culture = null)
        {
            if (string.IsNullOrEmpty(Word))
                return "";
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            return PluralizationService.CreateService(Culture).Pluralize(Word);
        }

        #endregion

        #region Singularize

        /// <summary>
        /// Singularizes a word
        /// </summary>
        /// <param name="Word">Word to singularize</param>
        /// <param name="Culture">Culture info used to singularize the word (defaults to current culture)</param>
        /// <returns>The word singularized</returns>
        public static string Singularize(this string Word, CultureInfo Culture = null)
        {
            if (string.IsNullOrEmpty(Word))
                return "";
            Culture = Culture.Check(CultureInfo.CurrentCulture);
            return PluralizationService.CreateService(Culture).Singularize(Word);
        }

        #endregion

        #region ToSQLDbType

        /// <summary>
        /// Converts a .Net type to SQLDbType value
        /// </summary>
        /// <param name="Type">.Net Type</param>
        /// <returns>The corresponding SQLDbType</returns>
        public static SqlDbType ToSQLDbType(this Type Type)
        {
            return Type.ToDbType().ToSQLDbType();
        }

        /// <summary>
        /// Converts a DbType to a SqlDbType
        /// </summary>
        /// <param name="Type">Type to convert</param>
        /// <returns>The corresponding SqlDbType (if it exists)</returns>
        public static SqlDbType ToSQLDbType(this DbType Type)
        {
            SqlParameter Parameter = new SqlParameter();
            Parameter.DbType = Type;
            return Parameter.SqlDbType;
        }

        #endregion

        #region ToDbType

        /// <summary>
        /// Converts a .Net type to DbType value
        /// </summary>
        /// <param name="Type">.Net Type</param>
        /// <returns>The corresponding DbType</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static DbType ToDbType(this Type Type)
        {
            if (Type.IsEnum) return Enum.GetUnderlyingType(Type).ToDbType();
            else if (Type == typeof(byte)) return DbType.Byte;
            else if (Type == typeof(sbyte)) return DbType.SByte;
            else if (Type == typeof(short)) return DbType.Int16;
            else if (Type == typeof(ushort)) return DbType.UInt16;
            else if (Type == typeof(int)) return DbType.Int32;
            else if (Type == typeof(uint)) return DbType.UInt32;
            else if (Type == typeof(long)) return DbType.Int64;
            else if (Type == typeof(ulong)) return DbType.UInt64;
            else if (Type == typeof(float)) return DbType.Single;
            else if (Type == typeof(double)) return DbType.Double;
            else if (Type == typeof(decimal)) return DbType.Decimal;
            else if (Type == typeof(bool)) return DbType.Boolean;
            else if (Type == typeof(string)) return DbType.String;
            else if (Type == typeof(char)) return DbType.StringFixedLength;
            else if (Type == typeof(Guid)) return DbType.Guid;
            else if (Type == typeof(DateTime)) return DbType.DateTime2;
            else if (Type == typeof(DateTimeOffset)) return DbType.DateTimeOffset;
            else if (Type == typeof(byte[])) return DbType.Binary;
            else if (Type == typeof(byte?)) return DbType.Byte;
            else if (Type == typeof(sbyte?)) return DbType.SByte;
            else if (Type == typeof(short?)) return DbType.Int16;
            else if (Type == typeof(ushort?)) return DbType.UInt16;
            else if (Type == typeof(int?)) return DbType.Int32;
            else if (Type == typeof(uint?)) return DbType.UInt32;
            else if (Type == typeof(long?)) return DbType.Int64;
            else if (Type == typeof(ulong?)) return DbType.UInt64;
            else if (Type == typeof(float?)) return DbType.Single;
            else if (Type == typeof(double?)) return DbType.Double;
            else if (Type == typeof(decimal?)) return DbType.Decimal;
            else if (Type == typeof(bool?)) return DbType.Boolean;
            else if (Type == typeof(char?)) return DbType.StringFixedLength;
            else if (Type == typeof(Guid?)) return DbType.Guid;
            else if (Type == typeof(DateTime?)) return DbType.DateTime2;
            else if (Type == typeof(DateTimeOffset?)) return DbType.DateTimeOffset;
            return DbType.Int32;
        }

        /// <summary>
        /// Converts SqlDbType to DbType
        /// </summary>
        /// <param name="Type">Type to convert</param>
        /// <returns>The corresponding DbType (if it exists)</returns>
        public static DbType ToDbType(this SqlDbType Type)
        {
            SqlParameter Parameter = new SqlParameter();
            Parameter.SqlDbType = Type;
            return Parameter.DbType;
        }

        #endregion

        #region ToList

        /// <summary>
        /// Attempts to convert the DataTable to a list of objects
        /// </summary>
        /// <typeparam name="T">Type the objects should be in the list</typeparam>
        /// <param name="Data">DataTable to convert</param>
        /// <param name="Creator">Function used to create each object</param>
        /// <returns>The DataTable converted to a list of objects</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static System.Collections.Generic.List<T> ToList<T>(this DataTable Data, Func<T> Creator = null) where T : new()
        {
            if (Data==null)
                return new List<T>();
            Creator = Creator.Check(() => new T());
            Type TType = typeof(T);
            PropertyInfo[] Properties = TType.GetProperties();
            System.Collections.Generic.List<T> Results = new System.Collections.Generic.List<T>();
            for (int x = 0; x < Data.Rows.Count; ++x)
            {
                T RowObject = Creator();

                for (int y = 0; y < Data.Columns.Count; ++y)
                {
                    PropertyInfo Property = Properties.FirstOrDefault(z => z.Name == Data.Columns[y].ColumnName);
                    if (Property!=null)
                        Property.SetValue(RowObject, Data.Rows[x][Data.Columns[y]].TryTo(Property.PropertyType, null), new object[] { });
                }
                Results.Add(RowObject);
            }
            return Results;
        }

        #endregion

        #region ToType

        /// <summary>
        /// Converts a SQLDbType value to .Net type
        /// </summary>
        /// <param name="Type">SqlDbType Type</param>
        /// <returns>The corresponding .Net type</returns>
        public static Type ToType(this SqlDbType Type)
        {
            return Type.ToDbType().ToType();
        }

        /// <summary>
        /// Converts a DbType value to .Net type
        /// </summary>
        /// <param name="Type">DbType</param>
        /// <returns>The corresponding .Net type</returns>
        public static Type ToType(this DbType Type)
        {
            if (Type == DbType.Byte) return typeof(byte);
            else if (Type == DbType.SByte) return typeof(sbyte);
            else if (Type == DbType.Int16) return typeof(short);
            else if (Type == DbType.UInt16) return typeof(ushort);
            else if (Type == DbType.Int32) return typeof(int);
            else if (Type == DbType.UInt32) return typeof(uint);
            else if (Type == DbType.Int64) return typeof(long);
            else if (Type == DbType.UInt64) return typeof(ulong);
            else if (Type == DbType.Single) return typeof(float);
            else if (Type == DbType.Double) return typeof(double);
            else if (Type == DbType.Decimal) return typeof(decimal);
            else if (Type == DbType.Boolean) return typeof(bool);
            else if (Type == DbType.String) return typeof(string);
            else if (Type == DbType.StringFixedLength) return typeof(char);
            else if (Type == DbType.Guid) return typeof(Guid);
            else if (Type == DbType.DateTime2) return typeof(DateTime);
            else if (Type == DbType.DateTime) return typeof(DateTime);
            else if (Type == DbType.DateTimeOffset) return typeof(DateTimeOffset);
            else if (Type == DbType.Binary) return typeof(byte[]);
            return typeof(int);
        }

        #endregion

        #region ToExpando

        /// <summary>
        /// Converts the object to a dynamic object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">The object to convert</param>
        /// <returns>The object as an expando object</returns>
        public static ExpandoObject ToExpando<T>(this T Object)
        {
            ExpandoObject ReturnValue = new ExpandoObject();
            Type TempType = typeof(T);
            foreach (PropertyInfo Property in TempType.GetProperties())
            {
                ((ICollection<KeyValuePair<String, Object>>)ReturnValue).Add(new KeyValuePair<string, object>(Property.Name, Property.GetValue(Object, null)));
            }
            return ReturnValue;
        }

        #endregion

        #region TryTo

        /// <summary>
        /// Attempts to convert the object to another type and returns the value
        /// </summary>
        /// <typeparam name="T">Type to convert from</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="Object">Object to convert</param>
        /// <param name="DefaultValue">Default value to return if there is an issue or it can't be converted</param>
        /// <returns>The object converted to the other type or the default value if there is an error or can't be converted</returns>
        public static R TryTo<T, R>(this T Object, R DefaultValue = default(R))
        {
            return (R)Object.TryTo(typeof(R), DefaultValue);
        }

        /// <summary>
        /// Converts an expando object to the specified type
        /// </summary>
        /// <typeparam name="R">Type to convert to</typeparam>
        /// <param name="Object">Object to convert</param>
        /// <param name="DefaultValue">Default value in case it can't convert the expando object</param>
        /// <returns>The object as the specified type</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static R TryTo<R>(this ExpandoObject Object, R DefaultValue = default(R))
            where R : class,new()
        {
            try
            {
                R ReturnValue = new R();
                Type TempType = typeof(R);
                foreach (PropertyInfo Property in TempType.GetProperties())
                {
                    if (((IDictionary<String, Object>)Object).ContainsKey(Property.Name))
                    {
                        Property.SetValue(ReturnValue, ((IDictionary<String, Object>)Object)[Property.Name], null);
                    }
                }
                return ReturnValue;
            }
            catch { }
            return DefaultValue;
        }

        /// <summary>
        /// Attempts to convert the object to another type and returns the value
        /// </summary>
        /// <typeparam name="T">Type to convert from</typeparam>
        /// <param name="ResultType">Result type</param>
        /// <param name="Object">Object to convert</param>
        /// <param name="DefaultValue">Default value to return if there is an issue or it can't be converted</param>
        /// <returns>The object converted to the other type or the default value if there is an error or can't be converted</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static object TryTo<T>(this T Object, Type ResultType, object DefaultValue = null)
        {
            try
            {
                if (Object == null || ((object)Object) == DBNull.Value)
                    return DefaultValue;
                string ObjectValue = Object as string;
                if (ObjectValue != null && ObjectValue.Length == 0)
                    return DefaultValue;
                Type ObjectType = Object.GetType();
                if (ResultType.IsAssignableFrom(ObjectType))
                    return Object;
                if (ResultType.IsEnum)
                    return System.Enum.Parse(ResultType, ObjectValue, true);
                if ((Object as IConvertible)!=null)
                    return Convert.ChangeType(Object, ResultType, CultureInfo.InvariantCulture);
                TypeConverter Converter = TypeDescriptor.GetConverter(ObjectType);
                if (Converter.CanConvertTo(ResultType))
                    return Converter.ConvertTo(Object, ResultType);
                if (ObjectValue!=null)
                    return ObjectValue.TryTo<string>(ResultType, DefaultValue);
            }
            catch { }
            return DefaultValue;
        }

        #endregion

        #region IsHighSurrogate

        /// <summary>
        /// Is the character a high surrogate character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsHighSurrogate(this char Value)
        {
            return char.IsHighSurrogate(Value);
        }

        #endregion

        #region IsLowSurrogate

        /// <summary>
        /// Is the character a low surrogate character
        /// </summary>
        /// <param name="Value">Value to check</param>
        /// <returns>True if it is, false otherwise</returns>
        public static bool IsLowSurrogate(this char Value)
        {
            return char.IsLowSurrogate(Value);
        }

        #endregion

        #endregion
    }
}
