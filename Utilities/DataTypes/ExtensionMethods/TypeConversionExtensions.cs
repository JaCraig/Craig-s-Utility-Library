/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Reflection;
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// Extensions converting between types
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
            if (Input == null)
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
            return Type.ToDbType().ToSqlDbType();
        }

        /// <summary>
        /// Converts a DbType to a SqlDbType
        /// </summary>
        /// <param name="Type">Type to convert</param>
        /// <returns>The corresponding SqlDbType (if it exists)</returns>
        public static SqlDbType ToSqlDbType(this DbType Type)
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
        public static DbType ToDbType(this Type Type)
        {
            if (Type == typeof(byte)) return DbType.Byte;
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
            else if (Type == typeof(DateTime)) return DbType.DateTime;
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
            else if (Type == typeof(DateTime?)) return DbType.DateTime;
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
            else if (Type == DbType.DateTime) return typeof(DateTime);
            else if (Type == DbType.DateTimeOffset) return typeof(DateTimeOffset);
            else if (Type == DbType.Binary) return typeof(byte[]);
            return typeof(int);
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
            if (string.IsNullOrEmpty(MethodName) || Object == null)
                return null;
            Type ObjectType = Object.GetType();
            MethodInfo Method = null;
            if (InputVariables != null)
            {
                Type[] MethodInputTypes = new Type[InputVariables.Length];
                for (int x = 0; x < InputVariables.Length; ++x)
                    MethodInputTypes[x] = InputVariables[x].GetType();
                Method = ObjectType.GetMethod(MethodName, MethodInputTypes);
                if (Method != null)
                    return Method.Invoke(Object, InputVariables);
            }
            Method = ObjectType.GetMethod(MethodName);
            if (Method != null)
                return Method.Invoke(Object, null);
            return null;
        }

        #endregion
    }
}