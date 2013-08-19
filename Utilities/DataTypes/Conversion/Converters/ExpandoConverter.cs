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
using System.Dynamic;
using System.Reflection;
using Utilities.DataTypes.Conversion.Interfaces;

#endregion

namespace Utilities.DataTypes.Conversion.Converters
{
    /// <summary>
    /// Expando converter
    /// </summary>
    public class ExpandoConverter : IConverter<ExpandoObject>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Manager">Manager object</param>
        public ExpandoConverter(Manager Manager) { this.Manager = Manager; }

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
            return !type.IsValueType;
        }

        /// <summary>
        /// Converts the object to the specified type
        /// </summary>
        /// <param name="Item">Object to convert</param>
        /// <param name="ReturnType">Return type</param>
        /// <returns>The object as the type specified</returns>
        public object To(ExpandoObject Item, Type ReturnType)
        {
            try
            {
                object ReturnValue = Activator.CreateInstance(ReturnType);
                foreach (PropertyInfo Property in ReturnType.GetProperties())
                {
                    if (((IDictionary<String, Object>)Item).ContainsKey(Property.Name))
                    {
                        Property.SetValue(ReturnValue, ((IDictionary<String, Object>)Item)[Property.Name], null);
                    }
                }
                return ReturnValue;
            }
            catch { }
            return null;
        }
    }
}