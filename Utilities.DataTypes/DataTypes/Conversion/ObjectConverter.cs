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
using System.Linq;
using Utilities.DataTypes.Conversion.Interfaces;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.DataTypes.Conversion
{
    /// <summary>
    /// Converter class
    /// </summary>
    /// <typeparam name="T">Type of input</typeparam>
    public class ObjectConverter<T> : IObjectConverter
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Manager">Manager object</param>
        public ObjectConverter(Manager Manager)
        {
            ObjectType = typeof(T);
            Converters = new List<IConverter<T>>();
            this.Manager = Manager;
            foreach (Type Converter in AppDomain.CurrentDomain.GetAssemblies().Types<IConverter<T>>())
            {
                Converters.Add((IConverter<T>)Activator.CreateInstance(Converter, Manager));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Manager object
        /// </summary>
        protected Manager Manager { get; private set; }

        /// <summary>
        /// Object type
        /// </summary>
        public Type ObjectType { get; private set; }

        /// <summary>
        /// Converter list
        /// </summary>
        public ICollection<IConverter<T>> Converters { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Converts the object from type T to type R
        /// </summary>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="Item">Item to convert</param>
        /// <param name="DefaultValue">Default value to return if the value is not convertable</param>
        /// <returns>The object as the type specified</returns>
        public R To<R>(object Item, R DefaultValue = default(R))
        {
            return (R)To(Item, typeof(R), DefaultValue);
        }

        /// <summary>
        /// Converts the object from type T to type ReturnType
        /// </summary>
        /// <param name="Item">Item to convert</param>
        /// <param name="DefaultValue">Default value to return if the value is not convertable</param>
        /// <param name="ReturnType">Return type</param>
        /// <returns>The object as the type specified</returns>
        public object To(object Item, Type ReturnType, object DefaultValue = null)
        {
            IConverter<T> Converter = Converters.FirstOrDefault(x => x.CanConvert(ReturnType));
            object ReturnValue = Converter == null ? DefaultValue : Converter.To((T)Item, ReturnType);
            return ReturnValue == null ? DefaultValue : ReturnValue;
        }

        #endregion
    }
}