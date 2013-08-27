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
using System.IO;
using Utilities.DataTypes;
using System.ComponentModel;
using Utilities.DataTypes.Conversion.Converters.Interfaces;
using System.Globalization;
using System.Dynamic;
using System.Reflection;
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
            foreach (IConverter TypeConverter in AppDomain.CurrentDomain.GetAssemblies().Objects<IConverter>())
            {
                TypeDescriptor.AddAttributes(TypeConverter.AssociatedType, new TypeConverterAttribute(TypeConverter.GetType()));
            }
        }

        #endregion
        
        #region Functions

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
            return (R)To(Item, typeof(R), DefaultValue);
        }


        /// <summary>
        /// Converts item from type T to R
        /// </summary>
        /// <typeparam name="T">Incoming type</typeparam>
        /// <param name="Item">Incoming object</param>
        /// <param name="ResultType">Result type</param>
        /// <param name="DefaultValue">Default return value if the item is null or can not be converted</param>
        /// <returns>The value converted to the specified type</returns>
        public object To<T>(T Item,Type ResultType, object DefaultValue = null)
        {
            try
            {
                if (Item == null)
                    return DefaultValue;
                Type ObjectType = Item.GetType();
                if (ResultType.IsAssignableFrom(ObjectType))
                    return Item;
                TypeConverter Converter = TypeDescriptor.GetConverter(Item);
                if (Converter.CanConvertTo(ResultType))
                    return Converter.ConvertTo(Item, ResultType);
                if (Item as IConvertible != null)
                    return Convert.ChangeType(Item, ResultType, CultureInfo.InvariantCulture);
                Converter = TypeDescriptor.GetConverter(ResultType);
                if (Converter.CanConvertFrom(ObjectType))
                    return Converter.ConvertFrom(Item);
                string ObjectValue = Item as string;
                if (string.IsNullOrEmpty(ObjectValue))
                    return null;
                if (ResultType.IsEnum)
                    return System.Enum.Parse(ResultType, ObjectValue, true);
                return null;
            }
            catch { return DefaultValue; }
        }

        #endregion
    }
}