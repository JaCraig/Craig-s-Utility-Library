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
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.DataTypes.Conversion.Interfaces;
using Utilities.DataTypes.Formatters;
using Utilities.DataTypes.Formatters.Interfaces;
#endregion

namespace Utilities.DataTypes.Conversion.Converters
{
    /// <summary>
    /// Generic converter (last ditch effort class)
    /// </summary>
    public class GenericConverter : IConverter<object>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Manager">Manager object</param>
        public GenericConverter(Manager Manager) { this.Manager = Manager; }

        /// <summary>
        /// Manager object
        /// </summary>
        protected Manager Manager { get; private set; }

        /// <summary>
        /// Always returns true as it will attempt any return type
        /// </summary>
        /// <param name="type">Type asking about</param>
        /// <returns>True</returns>
        public bool CanConvert(Type type)
        {
            return true;
        }

        /// <summary>
        /// Converts the object to the specified type
        /// </summary>
        /// <param name="Item">Object to convert</param>
        /// <param name="ReturnType">Return type</param>
        /// <returns>The object as the type specified</returns>
        public object To(object Item, Type ReturnType)
        {
            string ObjectValue = Item as string;
            if (ObjectValue != null && ObjectValue.Length == 0)
                return null;
            Type ObjectType = Item.GetType();
            if (ReturnType.IsAssignableFrom(ObjectType))
                return Item;
            if (ReturnType.IsEnum)
                return System.Enum.Parse(ReturnType, ObjectValue, true);
            if ((Item as IConvertible) != null)
                return Convert.ChangeType(Item, ReturnType, CultureInfo.InvariantCulture);
            TypeConverter Converter = TypeDescriptor.GetConverter(ObjectType);
            if (Converter.CanConvertTo(ReturnType))
                return Converter.ConvertTo(Item, ReturnType);
            if (ObjectValue != null)
                return Manager.To<object, string>(Item);
            if (ReturnType == typeof(ExpandoObject))
            {
                ExpandoObject ReturnValue = new ExpandoObject();
                Type TempType = ObjectType;
                foreach (PropertyInfo Property in TempType.GetProperties())
                {
                    ((ICollection<KeyValuePair<String, Object>>)ReturnValue).Add(new KeyValuePair<string, object>(Property.Name, Property.GetValue(Item, null)));
                }
                return ReturnValue;
            }
            return null;
        }
    }
}