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
using Utilities.DataTypes.Conversion.Interfaces;

#endregion

namespace Utilities.DataTypes.Conversion
{
    /// <summary>
    /// Hack class needed since you can't inherit from interfaces twice
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    public abstract class ConverterBaseHack<T> : IConverter<T>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        protected ConverterBaseHack() { }

        #endregion

        #region Functions

        /// <summary>
        /// Can the converter convert to the type specified
        /// </summary>
        /// <param name="type">Type to convert to</param>
        /// <returns>True if it can, false otherwise</returns>
        public abstract bool CanConvert(Type type);

        /// <summary>
        /// Converts the object to the specified type
        /// </summary>
        /// <param name="Item">Object to convert</param>
        /// <param name="ReturnType">Return type</param>
        /// <returns>The object as the type specified</returns>
        public abstract object To(T Item, Type ReturnType);

        #endregion
    }
}
