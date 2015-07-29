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
using System.Data;
using System.Data.SqlClient;
using Utilities.DataTypes.Conversion.Converters.BaseClasses;

namespace Utilities.DataTypes.Conversion.Converters
{
    /// <summary>
    /// SqlDbType converter
    /// </summary>
    public class SqlDbTypeTypeConverter : TypeConverterBase<SqlDbType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlDbTypeTypeConverter()
            : base()
        {
            ConvertToTypes.Add(typeof(Type), SqlDbTypeToType);
            ConvertToTypes.Add(typeof(DbType), SqlDbTypeToDbType);
            ConvertFromTypes.Add(typeof(Type).GetType(), TypeToSqlDbType);
            ConvertFromTypes.Add(typeof(DbType), DbTypeToSqlDbType);
            Conversions = new ConcurrentDictionary<Type, DbType>();
            Conversions.AddOrUpdate(typeof(byte), DbType.Byte, (x, y) => y);
            Conversions.AddOrUpdate(typeof(byte?), DbType.Byte, (x, y) => y);
            Conversions.AddOrUpdate(typeof(sbyte), DbType.SByte, (x, y) => y);
            Conversions.AddOrUpdate(typeof(sbyte?), DbType.SByte, (x, y) => y);
            Conversions.AddOrUpdate(typeof(short), DbType.Int16, (x, y) => y);
            Conversions.AddOrUpdate(typeof(short?), DbType.Int16, (x, y) => y);
            Conversions.AddOrUpdate(typeof(ushort), DbType.UInt16, (x, y) => y);
            Conversions.AddOrUpdate(typeof(ushort?), DbType.UInt16, (x, y) => y);
            Conversions.AddOrUpdate(typeof(int), DbType.Int32, (x, y) => y);
            Conversions.AddOrUpdate(typeof(int?), DbType.Int32, (x, y) => y);
            Conversions.AddOrUpdate(typeof(uint), DbType.UInt32, (x, y) => y);
            Conversions.AddOrUpdate(typeof(uint?), DbType.UInt32, (x, y) => y);
            Conversions.AddOrUpdate(typeof(long), DbType.Int64, (x, y) => y);
            Conversions.AddOrUpdate(typeof(long?), DbType.Int64, (x, y) => y);
            Conversions.AddOrUpdate(typeof(ulong), DbType.UInt64, (x, y) => y);
            Conversions.AddOrUpdate(typeof(ulong?), DbType.UInt64, (x, y) => y);
            Conversions.AddOrUpdate(typeof(float), DbType.Single, (x, y) => y);
            Conversions.AddOrUpdate(typeof(float?), DbType.Single, (x, y) => y);
            Conversions.AddOrUpdate(typeof(double), DbType.Double, (x, y) => y);
            Conversions.AddOrUpdate(typeof(double?), DbType.Double, (x, y) => y);
            Conversions.AddOrUpdate(typeof(decimal), DbType.Decimal, (x, y) => y);
            Conversions.AddOrUpdate(typeof(decimal?), DbType.Decimal, (x, y) => y);
            Conversions.AddOrUpdate(typeof(bool), DbType.Boolean, (x, y) => y);
            Conversions.AddOrUpdate(typeof(bool?), DbType.Boolean, (x, y) => y);
            Conversions.AddOrUpdate(typeof(string), DbType.String, (x, y) => y);
            Conversions.AddOrUpdate(typeof(char), DbType.StringFixedLength, (x, y) => y);
            Conversions.AddOrUpdate(typeof(char?), DbType.StringFixedLength, (x, y) => y);
            Conversions.AddOrUpdate(typeof(Guid), DbType.Guid, (x, y) => y);
            Conversions.AddOrUpdate(typeof(Guid?), DbType.Guid, (x, y) => y);
            Conversions.AddOrUpdate(typeof(DateTime), DbType.DateTime2, (x, y) => y);
            Conversions.AddOrUpdate(typeof(DateTime?), DbType.DateTime2, (x, y) => y);
            Conversions.AddOrUpdate(typeof(DateTimeOffset), DbType.DateTimeOffset, (x, y) => y);
            Conversions.AddOrUpdate(typeof(DateTimeOffset?), DbType.DateTimeOffset, (x, y) => y);
            Conversions.AddOrUpdate(typeof(byte[]), DbType.Binary, (x, y) => y);
        }

        /// <summary>
        /// Conversions
        /// </summary>
        protected static ConcurrentDictionary<Type, DbType> Conversions { get; private set; }

        /// <summary>
        /// Internal converter
        /// </summary>
        protected override TypeConverter InternalConverter { get { return new EnumConverter(typeof(SqlDbType)); } }

        private static object DbTypeToSqlDbType(object value)
        {
            if (!(value is DbType))
                return SqlDbType.Int;
            var TempValue = (DbType)value;
            var Parameter = new SqlParameter();
            Parameter.DbType = TempValue;
            return Parameter.SqlDbType;
        }

        private static object SqlDbTypeToDbType(object sqlDbType)
        {
            if (!(sqlDbType is SqlDbType))
                return DbType.Int32;
            var Temp = (SqlDbType)sqlDbType;
            var Parameter = new SqlParameter();
            Parameter.SqlDbType = Temp;
            return Parameter.DbType;
        }

        private static object SqlDbTypeToType(object arg)
        {
            if (!(arg is SqlDbType))
                return typeof(int);
            var Item = (SqlDbType)arg;
            var Parameter = new SqlParameter();
            Parameter.SqlDbType = Item;
            switch (Parameter.DbType)
            {
                case DbType.Byte:
                    return typeof(byte);
                case DbType.SByte:
                    return typeof(sbyte);
                case DbType.Int16:
                    return typeof(short);
                case DbType.UInt16:
                    return typeof(ushort);
                case DbType.Int32:
                    return typeof(int);
                case DbType.UInt32:
                    return typeof(uint);
                case DbType.Int64:
                    return typeof(long);
                case DbType.UInt64:
                    return typeof(ulong);
                case DbType.Single:
                    return typeof(float);
                case DbType.Double:
                    return typeof(double);
                case DbType.Decimal:
                    return typeof(decimal);
                case DbType.Boolean:
                    return typeof(bool);
                case DbType.String:
                    return typeof(string);
                case DbType.StringFixedLength:
                    return typeof(char);
                case DbType.Guid:
                    return typeof(Guid);
                case DbType.DateTime2:
                    return typeof(DateTime);
                case DbType.DateTime:
                    return typeof(DateTime);
                case DbType.DateTimeOffset:
                    return typeof(DateTimeOffset);
                case DbType.Binary:
                    return typeof(byte[]);
            }

            return typeof(int);
        }

        private static object TypeToSqlDbType(object arg)
        {
            var TempValue = arg as Type;
            if (TempValue == null)
                return SqlDbType.Int;
            DbType Item = DbType.Int32;
            if (TempValue.IsEnum)
                TempValue = Enum.GetUnderlyingType(TempValue);
            Item = Conversions.GetValue(TempValue, DbType.Int32);
            var Parameter = new SqlParameter();
            Parameter.DbType = Item;
            return Parameter.SqlDbType;
        }
    }
}