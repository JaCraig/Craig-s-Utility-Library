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

using System.ComponentModel;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Extension methods relating to caching of data
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class CacheExtensions
    {
        /// <summary>
        /// Cacnes the object based on the key and cache specified
        /// </summary>
        /// <param name="Object">Object to cache</param>
        /// <param name="Key">Cache key</param>
        /// <param name="Cache">Name of the cache to use</param>
        public static void Cache(this object Object, string Key, string Cache = "Default")
        {
            if (IoC.Manager.Bootstrapper == null)
                return;
            IoC.Manager.Bootstrapper.Resolve<Utilities.DataTypes.Caching.Manager>().Cache(Cache).Add(Key, Object);
        }

        /// <summary>
        /// Gets the specified object from the cache
        /// </summary>
        /// <typeparam name="T">Type to convert the object to</typeparam>
        /// <param name="Key">Key to the object</param>
        /// <param name="DefaultValue">Default value if the key is not found</param>
        /// <param name="Cache">Cache to get the item from</param>
        /// <returns>The object specified or the default value if it is not found</returns>
        public static T GetFromCache<T>(this string Key, T DefaultValue = default(T), string Cache = "Default")
        {
            if (IoC.Manager.Bootstrapper == null)
                return DefaultValue;
            object Value = IoC.Manager.Bootstrapper.Resolve<Utilities.DataTypes.Caching.Manager>().Cache(Cache)[Key];
            return Value == null ? DefaultValue : Value.To(DefaultValue);
        }
    }
}