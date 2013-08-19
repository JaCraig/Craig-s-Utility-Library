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
using System.Data;

#endregion

namespace Utilities.DataTypes.Conversion.Converters
{
    /// <summary>
    /// DbType to Type converter
    /// </summary>
    public class DbTypeTypeConverter : Utilities.DataTypes.Conversion.ConverterBase<Type, DbType>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Manager">Manager</param>
        public DbTypeTypeConverter(Manager Manager) : base(Manager) { }

        /// <summary>
        /// Converts the object to the specified type
        /// </summary>
        /// <param name="Item">Object to convert</param>
        /// <param name="ReturnType">Return type</param>
        /// <returns>The object as the type specified</returns>
        public override object To(Type Item, Type ReturnType)
        {
            if (Item.IsEnum) return To(Enum.GetUnderlyingType(Item), ReturnType);
            else if (Item == typeof(byte)) return DbType.Byte;
            else if (Item == typeof(sbyte)) return DbType.SByte;
            else if (Item == typeof(short)) return DbType.Int16;
            else if (Item == typeof(ushort)) return DbType.UInt16;
            else if (Item == typeof(int)) return DbType.Int32;
            else if (Item == typeof(uint)) return DbType.UInt32;
            else if (Item == typeof(long)) return DbType.Int64;
            else if (Item == typeof(ulong)) return DbType.UInt64;
            else if (Item == typeof(float)) return DbType.Single;
            else if (Item == typeof(double)) return DbType.Double;
            else if (Item == typeof(decimal)) return DbType.Decimal;
            else if (Item == typeof(bool)) return DbType.Boolean;
            else if (Item == typeof(string)) return DbType.String;
            else if (Item == typeof(char)) return DbType.StringFixedLength;
            else if (Item == typeof(Guid)) return DbType.Guid;
            else if (Item == typeof(DateTime)) return DbType.DateTime2;
            else if (Item == typeof(DateTimeOffset)) return DbType.DateTimeOffset;
            else if (Item == typeof(byte[])) return DbType.Binary;
            else if (Item == typeof(byte?)) return DbType.Byte;
            else if (Item == typeof(sbyte?)) return DbType.SByte;
            else if (Item == typeof(short?)) return DbType.Int16;
            else if (Item == typeof(ushort?)) return DbType.UInt16;
            else if (Item == typeof(int?)) return DbType.Int32;
            else if (Item == typeof(uint?)) return DbType.UInt32;
            else if (Item == typeof(long?)) return DbType.Int64;
            else if (Item == typeof(ulong?)) return DbType.UInt64;
            else if (Item == typeof(float?)) return DbType.Single;
            else if (Item == typeof(double?)) return DbType.Double;
            else if (Item == typeof(decimal?)) return DbType.Decimal;
            else if (Item == typeof(bool?)) return DbType.Boolean;
            else if (Item == typeof(char?)) return DbType.StringFixedLength;
            else if (Item == typeof(Guid?)) return DbType.Guid;
            else if (Item == typeof(DateTime?)) return DbType.DateTime2;
            else if (Item == typeof(DateTimeOffset?)) return DbType.DateTimeOffset;
            return DbType.Int32;
        }

        /// <summary>
        /// Converts the object to the specified type
        /// </summary>
        /// <param name="Item">Object to convert</param>
        /// <param name="ReturnType">Return type</param>
        /// <returns>The object as the type specified</returns>
        public override object To(DbType Item, Type ReturnType)
        {
            if (Item == DbType.Byte) return typeof(byte);
            else if (Item == DbType.SByte) return typeof(sbyte);
            else if (Item == DbType.Int16) return typeof(short);
            else if (Item == DbType.UInt16) return typeof(ushort);
            else if (Item == DbType.Int32) return typeof(int);
            else if (Item == DbType.UInt32) return typeof(uint);
            else if (Item == DbType.Int64) return typeof(long);
            else if (Item == DbType.UInt64) return typeof(ulong);
            else if (Item == DbType.Single) return typeof(float);
            else if (Item == DbType.Double) return typeof(double);
            else if (Item == DbType.Decimal) return typeof(decimal);
            else if (Item == DbType.Boolean) return typeof(bool);
            else if (Item == DbType.String) return typeof(string);
            else if (Item == DbType.StringFixedLength) return typeof(char);
            else if (Item == DbType.Guid) return typeof(Guid);
            else if (Item == DbType.DateTime2) return typeof(DateTime);
            else if (Item == DbType.DateTime) return typeof(DateTime);
            else if (Item == DbType.DateTimeOffset) return typeof(DateTimeOffset);
            else if (Item == DbType.Binary) return typeof(byte[]);
            return typeof(int);
        }
    }
}