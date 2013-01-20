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
using System.Threading;
using System.Diagnostics.Contracts;
using Utilities.DataTypes.Comparison;

#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// Generic extensions dealing with objects
    /// </summary>
    public static class GenericObjectExtensions
    {
        #region Functions

        #region Chain

        /// <summary>
        /// Allows actions to be chained together with the caveat that if Object is null,
        /// it is replaced with the DefaultObjectValue specified.
        /// If the Action or Object (once replaced with the default object value) is null, it will return the object.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="Object">Object to run the action on</param>
        /// <param name="Action">Action to run</param>
        /// <param name="DefaultObjectValue">Default object value</param>
        /// <returns>The original object</returns>
        public static T Chain<T>(this T Object, Action<T> Action, T DefaultObjectValue = default(T))
        {
            Object = Object.Check(DefaultObjectValue);
            if (Action == null || Object == null)
                return Object;
            Action(Object);
            return Object;
        }

        /// <summary>
        /// Allows actions to be chained together. It also has a couple of checks in there:
        /// 1) If the function is null, it returns the default return value specified.
        /// 2) If the object is null, it will replace it with the default object value specified.
        /// 3) If the object, once replaced with the default object value specified, is null, it will return the default return value specified.
        /// 4) If the return value from the function is null, it returns the default return value specified.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="Object">Object to run the action on</param>
        /// <param name="Function">Function to run</param>
        /// <param name="DefaultObjectValue">Default object value</param>
        /// <param name="DefaultReturnValue">Default return value</param>
        /// <returns>The result from the function</returns>
        public static R Chain<T, R>(this T Object, Func<T, R> Function, R DefaultReturnValue = default(R), T DefaultObjectValue = default(T))
        {
            Object = Object.Check(DefaultObjectValue);
            if (Function == null || Object == null)
                return DefaultReturnValue;
            return Function(Object).Check(DefaultReturnValue);
        }

        #endregion

        #region Check

        /// <summary>
        /// Checks to see if the object meets all the criteria. If it does, it returns the object. If it does not, it returns the default object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to check</param>
        /// <param name="DefaultValue">The default value to return</param>
        /// <param name="Predicate">Predicate to check the object against</param>
        /// <returns>The default object if it fails the criteria, the object otherwise</returns>
        public static T Check<T>(this T Object, Predicate<T> Predicate, T DefaultValue = default(T))
        {
            Contract.Requires<ArgumentNullException>(Predicate != null, "Predicate");
            return Predicate(Object) ? Object : DefaultValue;
        }

        /// <summary>
        /// Checks to see if the object meets all the criteria. If it does, it returns the object. If it does not, it returns the default object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to check</param>
        /// <param name="DefaultValue">The default value to return</param>
        /// <param name="Predicate">Predicate to check the object against</param>
        /// <returns>The default object if it fails the criteria, the object otherwise</returns>
        public static T Check<T>(this T Object, Predicate<T> Predicate, Func<T> DefaultValue)
        {
            Contract.Requires<ArgumentNullException>(Predicate != null, "Predicate");
            Contract.Requires<ArgumentNullException>(DefaultValue != null, "DefaultValue");
            return Predicate(Object) ? Object : DefaultValue();
        }

        /// <summary>
        /// Checks to see if the object is null. If it is, it returns the default object, otherwise the object is returned.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to check</param>
        /// <param name="DefaultValue">The default value to return</param>
        /// <returns>The default object if it is null, the object otherwise</returns>
        public static T Check<T>(this T Object, T DefaultValue = default(T))
        {
            return Object.Check(x => x != null, DefaultValue);
        }

        /// <summary>
        /// Checks to see if the object is null. If it is, it returns the default object, otherwise the object is returned.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to check</param>
        /// <param name="DefaultValue">The default value to return</param>
        /// <returns>The default object if it is null, the object otherwise</returns>
        public static T Check<T>(this T Object, Func<T> DefaultValue)
        {
            Contract.Requires<ArgumentNullException>(DefaultValue != null, "DefaultValue");
            return Object.Check(x => x != null, DefaultValue);
        }

        #endregion

        #region Is

        /// <summary>
        /// Determines if the object passes the set of predicates passed in
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to test</param>
        /// <param name="Predicates">Predicates to test</param>
        /// <returns>True if the object passes all of the predicates, false otherwise</returns>
        public static bool Is<T>(this T Object, params Predicate<T>[] Predicates)
        {
            Contract.Requires<ArgumentNullException>(Predicates != null, "Predicates");
            Contract.Requires<ArgumentNullException>(Contract.ForAll(Predicates, x => x != null), "Predicates");
            for (int x = 0; x < Predicates.Length; ++x)
            {
                if (!Predicates[x](Object))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines if the object is equal to a specific value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object"></param>
        /// <param name="ComparisonObject"></param>
        /// <param name="Comparer"></param>
        /// <returns></returns>
        public static bool Is<T>(this T Object, T ComparisonObject, IEqualityComparer<T> Comparer = null)
        {
            Comparer = Comparer.Check(() => new GenericEqualityComparer<T>());
            return Comparer.Equals(Object, ComparisonObject);
        }

        #endregion

        #region Times

        /// <summary>
        /// Runs a function based on the number of times specified and returns the results
        /// </summary>
        /// <typeparam name="T">Type that gets returned</typeparam>
        /// <param name="Count">Number of times the function should run</param>
        /// <param name="Function">The function that should run</param>
        /// <returns>The results from the function</returns>
        public static IEnumerable<T> Times<T>(this int Count, Func<int, T> Function)
        {
            System.Collections.Generic.List<T> ReturnValue = new System.Collections.Generic.List<T>();
            for (int x = 0; x < Count; ++x)
                ReturnValue.Add(Function(x));
            return ReturnValue;
        }

        /// <summary>
        /// Runs an action based on the number of times specified
        /// </summary>
        /// <param name="Count">Number of times to run the action</param>
        /// <param name="Action">Action to run</param>
        public static void Times(this int Count, Action<int> Action)
        {
            for (int x = 0; x < Count; ++x)
                Action(x);
        }

        #endregion

        #endregion
    }
}