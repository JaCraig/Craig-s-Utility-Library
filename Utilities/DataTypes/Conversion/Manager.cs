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
using System.Data.Entity.Design.PluralizationServices;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.DataTypes.Formatters;
using Utilities.DataTypes.Formatters.Interfaces;
#endregion

namespace Utilities.DataTypes.Conversion
{
    /// <summary>
    /// Conversion manager
    /// </summary>
    public class Manager
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Manager()
        {
            Converters = new Dictionary<Tuple<Type, Type>, IConverter>();
            AddConverter<DbType, SqlDbType>(ToSQLDbType);
            AddConverter<Type, SqlDbType>(ToSQLDbType);
            AddConverter<SqlDbType, DbType>(ToDbType);
            AddConverter<Type, DbType>(ToDbType);
            
            
            AddConverter<short, int>(x => (int)x);
            AddConverter<byte, int>(x => (int)x);

            try
            {
                if (Object == null || Convert.IsDBNull(Object))
                    return DefaultValue;
                string ObjectValue = Object as string;
                if (ObjectValue != null && ObjectValue.Length == 0)
                    return DefaultValue;
                Type ObjectType = Object.GetType();
                if (ResultType.IsAssignableFrom(ObjectType))
                    return Object;
                if (ResultType.IsEnum)
                    return System.Enum.Parse(ResultType, ObjectValue, true);
                if ((Object as IConvertible) != null)
                    return Convert.ChangeType(Object, ResultType, CultureInfo.InvariantCulture);
                TypeConverter Converter = TypeDescriptor.GetConverter(ObjectType);
                if (Converter.CanConvertTo(ResultType))
                    return Converter.ConvertTo(Object, ResultType);
                if (ObjectValue != null)
                    return ObjectValue.To<string>(ResultType, DefaultValue);
                if (ResultType == typeof(ExpandoObject))
                {
                    ExpandoObject ReturnValue = new ExpandoObject();
                    Type TempType = typeof(T);
                    foreach (PropertyInfo Property in TempType.GetProperties())
                    {
                        ((ICollection<KeyValuePair<String, Object>>)ReturnValue).Add(new KeyValuePair<string, object>(Property.Name, Property.GetValue(Object, null)));
                    }
                    return ReturnValue;
                }
            }
            catch { }

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
        }

        #endregion

        #region Properties

        /// <summary>
        /// Converters
        /// </summary>
        protected Dictionary<Tuple<Type, Type>, IConverter> Converters { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Adds a converter to the system
        /// </summary>
        /// <typeparam name="T">Input type</typeparam>
        /// <typeparam name="R">Output type</typeparam>
        /// <param name="Function">Conversion function to use</param>
        /// <returns>This</returns>
        public Manager AddConverter<T, R>(Func<T, R> Function)
        {
            Tuple<Type, Type> Key = new Tuple<Type, Type>(typeof(T), typeof(R));
            if (Converters.ContainsKey(Key))
                Converters[Key] = new Converter<T, R>(Function);
            else
                Converters.Add(Key, new Converter<T, R>(Function));
            return this;
        }

        /// <summary>
        /// Converts item from type T to R
        /// </summary>
        /// <typeparam name="T">Incoming type</typeparam>
        /// <typeparam name="R">Resulting type</typeparam>
        /// <param name="Item">Incoming object</param>
        /// <param name="DefaultValue">Default return value if the item is null or can not be converted</param>
        /// <returns>The value converted to the specified type</returns>
        public R To<T, R>(T Item, R DefaultValue = default(R))
        {
            if (Item==null||Convert.IsDBNull(Item))
                    return DefaultValue;
            return ((Converter<T, R>)Converters[new Tuple<Type, Type>(typeof(T), typeof(R))]).To(Item, DefaultValue);
        }

        #region InternalConverters

        #region ToSQLDbType

        /// <summary>
        /// Converts a .Net type to SQLDbType value
        /// </summary>
        /// <param name="Type">.Net Type</param>
        /// <returns>The corresponding SQLDbType</returns>
        private SqlDbType ToSQLDbType(this Type Type)
        {
            return ToSQLDbType(ToDbType(Type));
        }

        /// <summary>
        /// Converts a DbType to a SqlDbType
        /// </summary>
        /// <param name="Type">Type to convert</param>
        /// <returns>The corresponding SqlDbType (if it exists)</returns>
        private SqlDbType ToSQLDbType(this DbType Type)
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
        private DbType ToDbType(this Type Type)
        {
            if (Type.IsEnum) return ToDbType(Enum.GetUnderlyingType(Type);
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
        private DbType ToDbType(this SqlDbType Type)
        {
            SqlParameter Parameter = new SqlParameter();
            Parameter.SqlDbType = Type;
            return Parameter.DbType;
        }

        #endregion

        #endregion

        #endregion
    }
}