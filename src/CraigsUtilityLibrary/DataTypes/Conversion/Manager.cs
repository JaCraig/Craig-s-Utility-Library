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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using Utilities.DataTypes.Conversion.Converters.Interfaces;

namespace Utilities.DataTypes.Conversion
{
    /// <summary>
    /// Conversion manager
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Converters">The converters.</param>
        public Manager(IEnumerable<IConverter> Converters)
        {
            Contract.Requires<ArgumentNullException>(Converters != null, "Converters");
            foreach (IConverter TypeConverter in Converters)
            {
                TypeDescriptor.AddAttributes(TypeConverter.AssociatedType, new TypeConverterAttribute(TypeConverter.GetType()));
            }
        }

        /// <summary>
        /// Converts item from type T to R
        /// </summary>
        /// <typeparam name="T">Incoming type</typeparam>
        /// <typeparam name="R">Resulting type</typeparam>
        /// <param name="Item">Incoming object</param>
        /// <param name="DefaultValue">
        /// Default return value if the item is null or can not be converted
        /// </param>
        /// <returns>The value converted to the specified type</returns>
        public static R To<T, R>(T Item, R DefaultValue = default(R))
        {
            return (R)To(Item, typeof(R), DefaultValue);
        }

        /// <summary>
        /// Converts item from type T to R
        /// </summary>
        /// <typeparam name="T">Incoming type</typeparam>
        /// <param name="Item">Incoming object</param>
        /// <param name="ResultType">Result type</param>
        /// <param name="DefaultValue">
        /// Default return value if the item is null or can not be converted
        /// </param>
        /// <returns>The value converted to the specified type</returns>
        public static object To<T>(T Item, Type ResultType, object DefaultValue = null)
        {
            try
            {
                if (Item == null)
                {
                    return (DefaultValue == null && ResultType.IsValueType) ?
                        Activator.CreateInstance(ResultType) :
                        DefaultValue;
                }
                Type ObjectType = Item.GetType();
                if (ObjectType == typeof(DBNull))
                {
                    return (DefaultValue == null && ResultType.IsValueType) ?
                        Activator.CreateInstance(ResultType) :
                        DefaultValue;
                }
                if (ResultType.IsAssignableFrom(ObjectType))
                    return Item;
                if (Item as IConvertible != null && !ObjectType.IsEnum && !ResultType.IsEnum)
                    return Convert.ChangeType(Item, ResultType, CultureInfo.InvariantCulture);
                TypeConverter Converter = TypeDescriptor.GetConverter(Item);
                if (Converter.CanConvertTo(ResultType))
                    return Converter.ConvertTo(Item, ResultType);
                Converter = TypeDescriptor.GetConverter(ResultType);
                if (Converter.CanConvertFrom(ObjectType))
                    return Converter.ConvertFrom(Item);
                if (ResultType.IsEnum)
                {
                    if (ObjectType == ResultType.GetEnumUnderlyingType())
                        return System.Enum.ToObject(ResultType, Item);
                    if (ObjectType == typeof(string))
                        return System.Enum.Parse(ResultType, Item as string, true);
                }
                if (ResultType.IsClass)
                {
                    object ReturnValue = Activator.CreateInstance(ResultType);
                    var TempMapping = ObjectType.MapTo(ResultType);
                    if (TempMapping == null)
                        return ReturnValue;
                    TempMapping
                        .AutoMap()
                        .Copy(Item, ReturnValue);
                    return ReturnValue;
                }
            }
            catch
            {
            }
            return (DefaultValue == null && ResultType.IsValueType) ?
                Activator.CreateInstance(ResultType) :
                DefaultValue;
        }

        /// <summary>
        /// Outputs information about the manager as a string
        /// </summary>
        /// <returns>The string version of the manager</returns>
        public override string ToString()
        {
            return "Conversion Manager: Default\r\n";
        }
    }
}