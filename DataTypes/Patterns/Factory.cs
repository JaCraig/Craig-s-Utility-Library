/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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

#endregion

namespace Utilities.DataTypes.Patterns
{
    /// <summary>
    /// Factory class
    /// </summary>
    /// <typeparam name="Key">The "message" type</typeparam>
    /// <typeparam name="T">The class type that you want created</typeparam>
    public class Factory<Key, T>
    {
        #region Protected Variables

        /// <summary>
        /// List of constructors/initializers
        /// </summary>
        protected Dictionary<Key, Func<T>> Constructors = new Dictionary<Key, Func<T>>();

        #endregion

        #region Public Functions

        /// <summary>
        /// Registers an item
        /// </summary>
        /// <param name="Key">Item to register</param>
        /// <param name="Result">The object to be returned</param>
        public void Register(Key Key, T Result)
        {
            if (Constructors.ContainsKey(Key))
                Constructors[Key] = new Func<T>(() => Result);
            else
                Constructors.Add(Key, new Func<T>(() => Result));
        }

        /// <summary>
        /// Registers an item
        /// </summary>
        /// <param name="Key">Item to register</param>
        /// <param name="Constructor">The function to call when creating the item</param>
        public void Register(Key Key, Func<T> Constructor)
        {
            if (Constructors.ContainsKey(Key))
                Constructors[Key] = Constructor;
            else
                Constructors.Add(Key, Constructor);
        }

        /// <summary>
        /// Creates an instance associated with the key
        /// </summary>
        /// <param name="Key">Registered item</param>
        /// <returns>The type returned by the initializer</returns>
        public T Create(Key Key)
        {
            if (Constructors.ContainsKey(Key))
                return Constructors[Key]();
            return default(T);
        }

        #endregion
    }
}