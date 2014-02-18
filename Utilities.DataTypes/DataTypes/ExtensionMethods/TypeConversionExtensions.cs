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

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using Utilities.DataTypes.Conversion;
using Utilities.DataTypes.DataMapper.Interfaces;

#endregion Usings

namespace Utilities.DataTypes
{
    /// <summary>
    /// Extensions converting between types, checking if something is null, etc.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
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
            return !string.IsNullOrEmpty(Format) ? Input.Call<string>("ToString", Format) : Input.ToString();
        }

        #endregion FormatToString

        #region MapTo

        /// <summary>
        /// Sets up a mapping between two types
        /// </summary>
        /// <param name="LeftType">Left type</param>
        /// <param name="RightType">Right type</param>
        /// <returns>The type mapping</returns>
        public static ITypeMapping MapTo(this Type LeftType, Type RightType)
        {
            return IoC.Manager.Bootstrapper.Resolve<Utilities.DataTypes.DataMapper.Manager>().Map(LeftType, RightType);
        }

        /// <summary>
        /// Sets up a mapping between two types
        /// </summary>
        /// <typeparam name="Left">Left type</typeparam>
        /// <typeparam name="Right">Right type</typeparam>
        /// <param name="Object">Object to set up mapping for</param>
        /// <returns>The type mapping</returns>
        public static ITypeMapping<Left, Right> MapTo<Left, Right>(this Left Object)
        {
            return IoC.Manager.Bootstrapper.Resolve<Utilities.DataTypes.DataMapper.Manager>().Map<Left, Right>();
        }

        /// <summary>
        /// Sets up a mapping between two types
        /// </summary>
        /// <typeparam name="Left">Left type</typeparam>
        /// <typeparam name="Right">Right type</typeparam>
        /// <param name="ObjectType">Object type to set up mapping for</param>
        /// <returns>The type mapping</returns>
        public static ITypeMapping<Left, Right> MapTo<Left, Right>(this Type ObjectType)
        {
            return IoC.Manager.Bootstrapper.Resolve<Utilities.DataTypes.DataMapper.Manager>().Map<Left, Right>();
        }

        #endregion MapTo

        #region To

        /// <summary>
        /// Attempts to convert the DataTable to a list of objects
        /// </summary>
        /// <typeparam name="T">Type the objects should be in the list</typeparam>
        /// <param name="Data">DataTable to convert</param>
        /// <param name="Creator">Function used to create each object</param>
        /// <returns>The DataTable converted to a list of objects</returns>
        public static List<T> To<T>(this DataTable Data, Func<T> Creator)
            where T : class,new()
        {
            if (Data == null)
                return new List<T>();
            Creator = Creator.Check(() => new T());
            Type TType = typeof(T);
            PropertyInfo[] Properties = TType.GetProperties();
            List<T> Results = new List<T>();
            for (int x = 0; x < Data.Rows.Count; ++x)
            {
                T RowObject = Creator();
                for (int y = 0; y < Data.Columns.Count; ++y)
                {
                    PropertyInfo Property = Properties.FirstOrDefault(z => z.Name == Data.Columns[y].ColumnName);
                    if (Property != null)
                        Property.SetValue(RowObject, Data.Rows[x][Data.Columns[y]].To(Property.PropertyType, null), new object[] { });
                }
                Results.Add(RowObject);
            }
            return Results;
        }

        /// <summary>
        /// Attempts to convert the object to another type and returns the value
        /// </summary>
        /// <typeparam name="T">Type to convert from</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="Object">Object to convert</param>
        /// <param name="DefaultValue">
        /// Default value to return if there is an issue or it can't be converted
        /// </param>
        /// <returns>
        /// The object converted to the other type or the default value if there is an error or
        /// can't be converted
        /// </returns>
        public static R To<T, R>(this T Object, R DefaultValue = default(R))
        {
            return Manager.To(Object, DefaultValue);
        }

        /// <summary>
        /// Attempts to convert the object to another type and returns the value
        /// </summary>
        /// <typeparam name="T">Type to convert from</typeparam>
        /// <param name="ResultType">Result type</param>
        /// <param name="Object">Object to convert</param>
        /// <param name="DefaultValue">
        /// Default value to return if there is an issue or it can't be converted
        /// </param>
        /// <returns>
        /// The object converted to the other type or the default value if there is an error or
        /// can't be converted
        /// </returns>
        public static object To<T>(this T Object, Type ResultType, object DefaultValue = null)
        {
            return Manager.To(Object, ResultType, DefaultValue);
        }

        #endregion To

        #endregion Functions
    }
}