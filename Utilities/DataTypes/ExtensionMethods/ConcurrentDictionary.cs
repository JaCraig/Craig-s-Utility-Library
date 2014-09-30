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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Utilities.DataTypes.Comparison;

namespace Utilities.DataTypes
{
    /// <summary>
    /// ConcurrentDictionary extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ConcurrentDictionaryExtensions
    {
        /// <summary>
        /// Copies the dictionary to another dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="Dictionary">The dictionary.</param>
        /// <param name="Target">The target dictionary.</param>
        /// <returns>
        /// This
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the dictionary is null</exception>
        public static ConcurrentDictionary<TKey, TValue> CopyTo<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> Dictionary, ConcurrentDictionary<TKey, TValue> Target)
        {
            Contract.Requires<ArgumentNullException>(Dictionary != null, "Dictionary");
            Contract.Requires<ArgumentNullException>(Target != null, "Target");
            Parallel.ForEach(Dictionary, x => Target.SetValue(x.Key, x.Value));
            return Dictionary;
        }

        /// <summary>
        /// Gets the value from a dictionary or the default value if it isn't found
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="Dictionary">Dictionary to get the value from</param>
        /// <param name="Key">Key to look for</param>
        /// <param name="Default">Default value if the key is not found</param>
        /// <returns>
        /// The value associated with the key or the default value if the key is not found
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the dictionary is null</exception>
        public static TValue GetValue<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> Dictionary, TKey Key, TValue Default = default(TValue))
        {
            Contract.Requires<ArgumentNullException>(Dictionary != null, "Dictionary");
            TValue ReturnValue = Default;
            return Dictionary.TryGetValue(Key, out ReturnValue) ? ReturnValue : Default;
        }

        /// <summary>
        /// Sets the value in a dictionary
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="Dictionary">Dictionary to set the value in</param>
        /// <param name="Key">Key to look for</param>
        /// <param name="Value">Value to add</param>
        /// <returns>The dictionary</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the dictionary is null</exception>
        public static ConcurrentDictionary<TKey, TValue> SetValue<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> Dictionary, TKey Key, TValue Value)
        {
            Contract.Requires<ArgumentNullException>(Dictionary != null, "Dictionary");
            Dictionary.AddOrUpdate(Key, Value, (x, y) => Value);
            return Dictionary;
        }
    }
}