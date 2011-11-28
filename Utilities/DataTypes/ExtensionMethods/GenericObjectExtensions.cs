/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// Generic extensions dealing with objects
    /// </summary>
    public static class GenericObjectExtensions
    {
        #region Functions

        #region If

        /// <summary>
        /// Determines if the object fullfills the predicate and if it does, returns itself. Otherwise the default value.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="Object">Object to check</param>
        /// <param name="Predicate">Predicate to run on the object</param>
        /// <param name="DefaultValue">Default value to return if it does not succeed the predicate test</param>
        /// <returns>The original value if predicate is true, the default value otherwise</returns>
        public static T If<T>(this T Object, Predicate<T> Predicate,T DefaultValue=default(T))
        {
            if (Object.IsNull())
                return DefaultValue;
            return Predicate(Object) ? Object : DefaultValue;
        }

        #endregion

        #region NotIf

        /// <summary>
        /// Determines if the object fails the predicate and if it does, returns itself. Otherwise the default value.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="Object">Object to check</param>
        /// <param name="Predicate">Predicate to run on the object</param>
        /// <param name="DefaultValue">Default value to return if it succeeds the predicate test</param>
        /// <returns>The original value if predicate is false, the default value otherwise</returns>
        public static T NotIf<T>(this T Object, Predicate<T> Predicate, T DefaultValue = default(T))
        {
            if (Object.IsNull())
                return DefaultValue;
            return Predicate(Object) ? DefaultValue : Object;
        }

        #endregion

        #endregion
    }
}
