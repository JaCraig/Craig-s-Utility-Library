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

namespace Utilities.DataTypes.Patterns
{
    /// <summary>
    /// Factory class
    /// </summary>
    /// <typeparam name="Key">The "message" type</typeparam>
    /// <typeparam name="T">The class type that you want created</typeparam>
    public class Factory<Key, T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Factory()
        {
            Constructors = new Dictionary<Key, Func<T>>();
        }

        /// <summary>
        /// List of constructors/initializers
        /// </summary>
        protected Dictionary<Key, Func<T>> Constructors { get; private set; }

        /// <summary>
        /// Creates an instance associated with the key
        /// </summary>
        /// <param name="Key">Registered item</param>
        /// <returns>The type returned by the initializer</returns>
        public virtual T Create(Key Key)
        {
            return Constructors.GetValue(Key, () => default(T))();
        }

        /// <summary>
        /// Determines if a key has been registered
        /// </summary>
        /// <param name="Key">Key to check</param>
        /// <returns>True if it exists, false otherwise</returns>
        public virtual bool Exists(Key Key)
        {
            return Constructors.ContainsKey(Key);
        }

        /// <summary>
        /// Registers an item
        /// </summary>
        /// <param name="Key">Item to register</param>
        /// <param name="Result">The object to be returned</param>
        public virtual void Register(Key Key, T Result)
        {
            Register(Key, () => Result);
        }

        /// <summary>
        /// Registers an item
        /// </summary>
        /// <param name="Key">Item to register</param>
        /// <param name="Constructor">The function to call when creating the item</param>
        public virtual void Register(Key Key, Func<T> Constructor)
        {
            Constructors.SetValue(Key, Constructor);
        }
    }
}