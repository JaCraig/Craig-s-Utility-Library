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
using System.Dynamic;
using System.Reflection;
using Utilities.DataTypes.Conversion.Converters.BaseClasses;
using Utilities.DataTypes.Conversion.Converters.Interfaces;
#endregion

namespace Utilities.DataTypes.Conversion.Converters
{
    /// <summary>
    /// ExpandoObject converter
    /// </summary>
    public class ExpandoTypeConverter : TypeConverter, IConverter
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ExpandoTypeConverter()
            : base()
        {
            AssociatedType = typeof(ExpandoObject);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Associated type
        /// </summary>
        public Type AssociatedType { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Can convert from
        /// </summary>
        /// <param name="context">Context object</param>
        /// <param name="sourceType">Source type</param>
        /// <returns>True if it can convert from it, false otherwise</returns>
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType.IsClass || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Can convert to
        /// </summary>
        /// <param name="context">Context object</param>
        /// <param name="destinationType">Destination type</param>
        /// <returns>True if it can convert from it, false otherwise</returns>
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType.IsClass || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Convert from an object to a ExpandoObject
        /// </summary>
        /// <param name="context">Context object</param>
        /// <param name="culture">Culture info</param>
        /// <param name="value">Value</param>
        /// <returns>The DbType version</returns>
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            try
            {
                IDictionary<string, object> ReturnValue = new ExpandoObject();
                foreach (PropertyInfo Property in value.GetType().GetProperties())
                {
                    ReturnValue.Add(Property.Name, Property.GetValue(value));
                }
                return ReturnValue;
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Converts the ExpandoObject object to another type
        /// </summary>
        /// <param name="context">Context type</param>
        /// <param name="culture">Culture info</param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            try
            {
                IDictionary<String, Object> TempValue = value as IDictionary<String, Object>;
                object ReturnValue = Activator.CreateInstance(destinationType);
                IoC.Manager.Bootstrapper.Resolve<Utilities.DataTypes.DataMapper.Manager>().Map(typeof(IDictionary<String, Object>), destinationType)
                        .AutoMap()
                        .Copy(TempValue, ReturnValue);
                return ReturnValue;
            }
            catch { }
            return null;
        }

        #endregion
    }
}