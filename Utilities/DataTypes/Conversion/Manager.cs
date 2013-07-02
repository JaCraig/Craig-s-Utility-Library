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
using Utilities.DataTypes.Conversion.Interfaces;
using Utilities.DataTypes.ExtensionMethods;
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
            Converters = new Dictionary<Type, IObjectConverter>();
            new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).LoadAssemblies(false).ForEach(x => x);
            foreach (Type ObjectConverter in AppDomain.CurrentDomain.GetAssemblies().Types<IObjectConverter>())
            {
                foreach (Type Converter in AppDomain.CurrentDomain.GetAssemblies().Types<IConverter>())
                {
                    Type Key = Converter;
                    while (Key != null)
                    {
                        foreach (Type Interface in Converter.GetInterfaces())
                        {
                            if (Interface.GetGenericArguments().Length > 0)
                                AddConverter(Interface.GetGenericArguments()[0], ObjectConverter);
                        }
                        Key = Key.BaseType;
                    }
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Converters
        /// </summary>
        protected Dictionary<Type, IObjectConverter> Converters { get; private set; }

        #endregion

        #region Functions

        /// <summary>
        /// Adds a converter to the system
        /// </summary>
        /// <param name="ObjectConverter">Object converter</param>
        /// <param name="ObjectType">Object type</param>
        /// <returns>This</returns>
        protected Manager AddConverter(Type ObjectType, Type ObjectConverter)
        {
            if (!Converters.ContainsKey(ObjectType))
            {
                Type FinalType = ObjectConverter.MakeGenericType(ObjectType);
                Converters.Add(ObjectType, (IObjectConverter)Activator.CreateInstance(FinalType, this));
            }
            return this;
        }

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
                if (Item == null || Convert.IsDBNull(Item))
                    return DefaultValue;
                Type Key = typeof(T);
                while (Key != null)
                {
                    if (Converters.ContainsKey(Key))
                        return Converters[Key].To(Item, ResultType, DefaultValue);
                    foreach (Type Interface in Key.GetInterfaces())
                    {
                        if (Converters.ContainsKey(Interface))
                            return Converters[Interface].To(Item, ResultType, DefaultValue);
                    }
                    Key = Key.BaseType;
                }
                return DefaultValue;
            }
            catch { return DefaultValue; }
        }

        #endregion
    }
}