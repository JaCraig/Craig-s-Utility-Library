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
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Globalization;
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// Extensions converting between types, checking if something is null, etc.
    /// </summary>
    public static class TypeConversionExtensions
    {
        #region Functions

        #region FormatToString

        /// <summary>
        /// Calls the object's ToString function passing in the formatting
        /// </summary>
        /// <param name="Input">Input object</param>
        /// <param name="Format">Format of the output string</param>
        /// <returns>The formatted string</returns>
        public static string FormatToString(this object Input, string Format)
        {
            if (Input.IsNull())
                return "";
            return !string.IsNullOrEmpty(Format) ? (string)CallMethod("ToString", Input, Format) : Input.ToString();
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
            if (Data.IsNull())
                return new List<T>();
            Creator = Creator.NullCheck(() => new T());
            Type TType = typeof(T);
            PropertyInfo[] Properties = TType.GetProperties();
            System.Collections.Generic.List<T> Results = new System.Collections.Generic.List<T>();
            for (int x = 0; x < Data.Rows.Count; ++x)
            {
                T RowObject = Creator();

                for (int y = 0; y < Data.Columns.Count; ++y)
                {
                    PropertyInfo Property = Properties.FirstOrDefault(z => z.Name == Data.Columns[y].ColumnName);
                    if (!Property.IsNull())
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
                if (Object.IsNull())
                    return DefaultValue;
                string ObjectValue = Object as string;
                if (ObjectValue.IsNotNull() && ObjectValue.Length == 0)
                    return DefaultValue;
                Type ObjectType = Object.GetType();
                if (ResultType.IsAssignableFrom(ObjectType))
                    return Object;
                if(ResultType.IsEnum)
                    return System.Enum.Parse(ResultType, ObjectValue, true);
                if ((Object as IConvertible).IsNotNull())
                    return Convert.ChangeType(Object, ResultType, CultureInfo.InvariantCulture);
                TypeConverter Converter = TypeDescriptor.GetConverter(ObjectType);
                if (Converter.CanConvertTo(ResultType))
                    return Converter.ConvertTo(Object, ResultType);
                if (ObjectValue.IsNotNull())
                    return ObjectValue.TryTo<string>(ResultType, DefaultValue);
            }
            catch { }
            return DefaultValue;
        }

        #endregion

        #endregion

        #region Private Static Functions

        /// <summary>
        /// Calls a method on an object
        /// </summary>
        /// <param name="MethodName">Method name</param>
        /// <param name="Object">Object to call the method on</param>
        /// <param name="InputVariables">(Optional)input variables for the method</param>
        /// <returns>The returned value of the method</returns>
        private static object CallMethod(string MethodName, object Object, params object[] InputVariables)
        {
            if (string.IsNullOrEmpty(MethodName) || Object.IsNull())
                return null;
            Type ObjectType = Object.GetType();
            MethodInfo Method = null;
            if (InputVariables.IsNotNull())
            {
                Type[] MethodInputTypes = new Type[InputVariables.Length];
                for (int x = 0; x < InputVariables.Length; ++x)
                    MethodInputTypes[x] = InputVariables[x].GetType();
                Method = ObjectType.GetMethod(MethodName, MethodInputTypes);
                if (Method != null)
                    return Method.Invoke(Object, InputVariables);
            }
            Method = ObjectType.GetMethod(MethodName);
            return Method.IsNull() ? null : Method.Invoke(Object, null);
        }

        #endregion
    }
}