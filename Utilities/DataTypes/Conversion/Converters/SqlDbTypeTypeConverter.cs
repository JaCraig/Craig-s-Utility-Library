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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using Utilities.DataTypes.Conversion.Converters.BaseClasses;
#endregion

namespace Utilities.DataTypes.Conversion.Converters
{
    /// <summary>
    /// SqlDbType converter
    /// </summary>
    public class SqlDbTypeTypeConverter : TypeConverterBase<SqlDbType>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlDbTypeTypeConverter()
            : base()
        {
            ConvertToTypes.Add(typeof(Type), SqlDbTypeToType);
            ConvertToTypes.Add(typeof(DbType), SqlDbTypeToDbType);
            ConvertFromTypes.Add(typeof(Type), TypeToSqlDbType);
            ConvertFromTypes.Add(typeof(DbType), DbTypeToSqlDbType);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Internal converter
        /// </summary>
        protected override TypeConverter InternalConverter { get { return new EnumConverter(typeof(SqlDbType)); } }

        #endregion

        #region Functions

        private static object TypeToSqlDbType(object arg)
        {
            Type TempValue = arg as Type;
            if (TempValue == null)
                return SqlDbType.Int;
            DbType Item = DbType.Int32;
            if (TempValue.IsEnum)
                TempValue = Enum.GetUnderlyingType(TempValue);
            if (TempValue == typeof(byte)) Item = DbType.Byte;
            else if (TempValue == typeof(sbyte)) Item = DbType.SByte;
            else if (TempValue == typeof(short)) Item = DbType.Int16;
            else if (TempValue == typeof(ushort)) Item = DbType.UInt16;
            else if (TempValue == typeof(int)) Item = DbType.Int32;
            else if (TempValue == typeof(uint)) Item = DbType.UInt32;
            else if (TempValue == typeof(long)) Item = DbType.Int64;
            else if (TempValue == typeof(ulong)) Item = DbType.UInt64;
            else if (TempValue == typeof(float)) Item = DbType.Single;
            else if (TempValue == typeof(double)) Item = DbType.Double;
            else if (TempValue == typeof(decimal)) Item = DbType.Decimal;
            else if (TempValue == typeof(bool)) Item = DbType.Boolean;
            else if (TempValue == typeof(string)) Item = DbType.String;
            else if (TempValue == typeof(char)) Item = DbType.StringFixedLength;
            else if (TempValue == typeof(Guid)) Item = DbType.Guid;
            else if (TempValue == typeof(DateTime)) Item = DbType.DateTime2;
            else if (TempValue == typeof(DateTimeOffset)) Item = DbType.DateTimeOffset;
            else if (TempValue == typeof(byte[])) Item = DbType.Binary;
            else if (TempValue == typeof(byte?)) Item = DbType.Byte;
            else if (TempValue == typeof(sbyte?)) Item = DbType.SByte;
            else if (TempValue == typeof(short?)) Item = DbType.Int16;
            else if (TempValue == typeof(ushort?)) Item = DbType.UInt16;
            else if (TempValue == typeof(int?)) Item = DbType.Int32;
            else if (TempValue == typeof(uint?)) Item = DbType.UInt32;
            else if (TempValue == typeof(long?)) Item = DbType.Int64;
            else if (TempValue == typeof(ulong?)) Item = DbType.UInt64;
            else if (TempValue == typeof(float?)) Item = DbType.Single;
            else if (TempValue == typeof(double?)) Item = DbType.Double;
            else if (TempValue == typeof(decimal?)) Item = DbType.Decimal;
            else if (TempValue == typeof(bool?)) Item = DbType.Boolean;
            else if (TempValue == typeof(char?)) Item = DbType.StringFixedLength;
            else if (TempValue == typeof(Guid?)) Item = DbType.Guid;
            else if (TempValue == typeof(DateTime?)) Item = DbType.DateTime2;
            else if (TempValue == typeof(DateTimeOffset?)) Item = DbType.DateTimeOffset;
            Item = DbType.Int32;
            SqlParameter Parameter = new SqlParameter();
            Parameter.DbType = Item;
            return Parameter.SqlDbType;
        }

        private static object SqlDbTypeToType(object arg)
        {
            if (!(arg is SqlDbType))
                return typeof(int);
            SqlDbType Item = (SqlDbType)arg;
            SqlParameter Parameter = new SqlParameter();
            Parameter.SqlDbType = Item;
            if (Parameter.DbType == DbType.Byte) return typeof(byte);
            else if (Parameter.DbType == DbType.SByte) return typeof(sbyte);
            else if (Parameter.DbType == DbType.Int16) return typeof(short);
            else if (Parameter.DbType == DbType.UInt16) return typeof(ushort);
            else if (Parameter.DbType == DbType.Int32) return typeof(int);
            else if (Parameter.DbType == DbType.UInt32) return typeof(uint);
            else if (Parameter.DbType == DbType.Int64) return typeof(long);
            else if (Parameter.DbType == DbType.UInt64) return typeof(ulong);
            else if (Parameter.DbType == DbType.Single) return typeof(float);
            else if (Parameter.DbType == DbType.Double) return typeof(double);
            else if (Parameter.DbType == DbType.Decimal) return typeof(decimal);
            else if (Parameter.DbType == DbType.Boolean) return typeof(bool);
            else if (Parameter.DbType == DbType.String) return typeof(string);
            else if (Parameter.DbType == DbType.StringFixedLength) return typeof(char);
            else if (Parameter.DbType == DbType.Guid) return typeof(Guid);
            else if (Parameter.DbType == DbType.DateTime2) return typeof(DateTime);
            else if (Parameter.DbType == DbType.DateTime) return typeof(DateTime);
            else if (Parameter.DbType == DbType.DateTimeOffset) return typeof(DateTimeOffset);
            else if (Parameter.DbType == DbType.Binary) return typeof(byte[]);
            return typeof(int);
        }

        private static object DbTypeToSqlDbType(object value)
        {
            if (!(value is DbType))
                return SqlDbType.Int;
            DbType TempValue = (DbType)value;
            SqlParameter Parameter = new SqlParameter();
            Parameter.DbType = TempValue;
            return Parameter.SqlDbType;
        }

        private static object SqlDbTypeToDbType(object sqlDbType)
        {
            if (!(sqlDbType is SqlDbType))
                return DbType.Int32;
            SqlDbType Temp = (SqlDbType)sqlDbType;
            SqlParameter Parameter = new SqlParameter();
            Parameter.SqlDbType = Temp;
            return Parameter.DbType;
        }

        #endregion
    }
}