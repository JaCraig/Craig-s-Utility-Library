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
using Utilities.DataTypes.Conversion.Converters.Interfaces;

namespace Utilities.DataTypes.Conversion.Converters.BaseClasses
{
    /// <summary>
    /// Type converter base class
    /// </summary>
    /// <typeparam name="T">Converter type</typeparam>
    public abstract class TypeConverterBase<T> : TypeConverter, IConverter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected TypeConverterBase()
        {
            ConvertToTypes = new Dictionary<Type, Func<object, object>>();
            ConvertFromTypes = new Dictionary<Type, Func<object, object>>();
            AssociatedType = typeof(T);
        }

        /// <summary>
        /// Associated type
        /// </summary>
        public Type AssociatedType { get; private set; }

        /// <summary>
        /// Types it can convert from and mapped functions
        /// </summary>
        protected IDictionary<Type, Func<object, object>> ConvertFromTypes { get; private set; }

        /// <summary>
        /// Types it can convert to and mapped functions
        /// </summary>
        protected IDictionary<Type, Func<object, object>> ConvertToTypes { get; private set; }

        /// <summary>
        /// Converter used internally if this can not convert the object
        /// </summary>
        protected abstract TypeConverter InternalConverter { get; }

        /// <summary>
        /// Can convert from
        /// </summary>
        /// <param name="context">Context object</param>
        /// <param name="sourceType">Source type</param>
        /// <returns>True if it can convert from it, false otherwise</returns>
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            return ConvertFromTypes.Keys.Contains(sourceType) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Can convert to
        /// </summary>
        /// <param name="context">Context object</param>
        /// <param name="destinationType">Destination type</param>
        /// <returns>True if it can convert from it, false otherwise</returns>
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
        {
            return ConvertToTypes.Keys.Contains(destinationType) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Convert from an object to a DbType
        /// </summary>
        /// <param name="context">Context object</param>
        /// <param name="culture">Culture info</param>
        /// <param name="value">Value</param>
        /// <returns>The DbType version</returns>
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value == null)
                return null;
            Type ValueType = value.GetType();
            if (ConvertFromTypes.ContainsKey(ValueType))
                return ConvertFromTypes[ValueType](value);
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the DbType object to another type
        /// </summary>
        /// <param name="context">Context type</param>
        /// <param name="culture">Culture info</param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
                return null;
            if (ConvertToTypes.ContainsKey(destinationType))
                return ConvertToTypes[destinationType](value);
            return base.ConvertFrom(context, culture, value);
        }
    }
}