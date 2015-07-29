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
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using Utilities.DataTypes.Comparison;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Generic extensions dealing with objects
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class GenericObjectExtensions
    {
        /// <summary>
        /// Allows actions to be chained together with the caveat that if Object is null, it is
        /// replaced with the DefaultObjectValue specified. If the Action or Object (once replaced
        /// with the default object value) is null, it will return the object.
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
        /// 3) If the object, once replaced with the default object value specified, is null, it
        ///    will return the default return value specified.
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

        /// <summary>
        /// Checks to see if the object meets all the criteria. If it does, it returns the object.
        /// If it does not, it returns the default object
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
        /// Checks to see if the object meets all the criteria. If it does, it returns the object.
        /// If it does not, it returns the default object
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
        /// Checks to see if the object is null. If it is, it returns the default object, otherwise
        /// the object is returned.
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
        /// Checks to see if the object is null. If it is, it returns the default object, otherwise
        /// the object is returned.
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

        /// <summary>
        /// Executes a function, repeating it a number of times in case it fails
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="Function">Function to run</param>
        /// <param name="Attempts">Number of times to attempt it</param>
        /// <param name="RetryDelay">The amount of milliseconds to wait between tries</param>
        /// <param name="TimeOut">
        /// Max amount of time to wait for the function to run (waits for the current attempt to
        /// finish before checking)
        /// </param>
        /// <returns>The returned value from the function</returns>
        public static T Execute<T>(this Func<T> Function, int Attempts = 3, int RetryDelay = 0, int TimeOut = int.MaxValue)
        {
            Contract.Requires<ArgumentNullException>(Function != null, "Function");
            Exception Holder = null;
            long Start = System.Environment.TickCount;
            while (Attempts > 0)
            {
                try
                {
                    return Function();
                }
                catch (Exception e) { Holder = e; }
                if (System.Environment.TickCount - Start > TimeOut)
                    break;
                Thread.Sleep(RetryDelay);
                --Attempts;
            }
            throw Holder;
        }

        /// <summary>
        /// Executes an action, repeating it a number of times in case it fails
        /// </summary>
        /// <param name="Action">Action to run</param>
        /// <param name="Attempts">Number of times to attempt it</param>
        /// <param name="RetryDelay">The amount of milliseconds to wait between tries</param>
        /// <param name="TimeOut">
        /// Max amount of time to wait for the function to run (waits for the current attempt to
        /// finish before checking)
        /// </param>
        public static void Execute(this Action Action, int Attempts = 3, int RetryDelay = 0, int TimeOut = int.MaxValue)
        {
            Contract.Requires<ArgumentNullException>(Action != null, "Action");
            Exception Holder = null;
            long Start = System.Environment.TickCount;
            while (Attempts > 0)
            {
                try
                {
                    Action();
                }
                catch (Exception e) { Holder = e; }
                if (System.Environment.TickCount - Start > TimeOut)
                    break;
                Thread.Sleep(RetryDelay);
                --Attempts;
            }
            if (Holder != null)
                throw Holder;
        }

        /// <summary>
        /// Determines if the object passes the predicate passed in
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to test</param>
        /// <param name="Predicate">Predicate to test</param>
        /// <returns>True if the object passes the predicate, false otherwise</returns>
        public static bool Is<T>(this T Object, Predicate<T> Predicate)
        {
            Contract.Requires<ArgumentNullException>(Predicate != null, "Predicate");
            return Predicate(Object);
        }

        /// <summary>
        /// Determines if the object is equal to a specific value
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Object">Object to test</param>
        /// <param name="ComparisonObject">Comparison object</param>
        /// <param name="Comparer">Comparer</param>
        /// <returns>True if the object passes the predicate, false otherwise</returns>
        public static bool Is<T>(this T Object, T ComparisonObject, IEqualityComparer<T> Comparer = null)
        {
            Comparer = Comparer.Check(() => new GenericEqualityComparer<T>());
            return Comparer.Equals(Object, ComparisonObject);
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for the item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="Item">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static T ThrowIf<T>(this T Item, Predicate<T> Predicate, Func<Exception> Exception)
        {
            Contract.Requires<ArgumentNullException>(Predicate != null, "Predicate");
            if (Predicate(Item))
                throw Exception();
            return Item;
        }

        /// <summary>
        /// Throws the specified exception if the predicate is true for the item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="Item">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static T ThrowIf<T>(this T Item, Predicate<T> Predicate, Exception Exception)
        {
            Contract.Requires<ArgumentNullException>(Predicate != null, "Predicate");
            if (Predicate(Item))
                throw Exception;
            return Item;
        }

        /// <summary>
        /// Determines if the object is equal to default value and throws an ArgumentNullException
        /// if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="EqualityComparer">
        /// Equality comparer used to determine if the object is equal to default
        /// </param>
        /// <param name="Name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfDefault<T>(this T Item, string Name, IEqualityComparer<T> EqualityComparer = null)
        {
            return Item.ThrowIfDefault(new ArgumentNullException(Name), EqualityComparer);
        }

        /// <summary>
        /// Determines if the object is equal to default value and throws the exception that is
        /// passed in if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="EqualityComparer">
        /// Equality comparer used to determine if the object is equal to default
        /// </param>
        /// <param name="Exception">Exception to throw</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfDefault<T>(this T Item, Exception Exception, IEqualityComparer<T> EqualityComparer = null)
        {
            return Item.ThrowIf(x => EqualityComparer.Check(() => new GenericEqualityComparer<T>()).Equals(x, default(T)), Exception);
        }

        /// <summary>
        /// Throws the specified exception if the predicate is false for the item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="Item">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is false</param>
        /// <returns>the original Item</returns>
        public static T ThrowIfNot<T>(this T Item, Predicate<T> Predicate, Exception Exception)
        {
            return Item.ThrowIf(x => !Predicate(x), Exception);
        }

        /// <summary>
        /// Determines if the object is not equal to default value and throws an ArgumentException
        /// if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="EqualityComparer">
        /// Equality comparer used to determine if the object is equal to default
        /// </param>
        /// <param name="Name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNotDefault<T>(this T Item, string Name, IEqualityComparer<T> EqualityComparer = null)
        {
            return Item.ThrowIfNotDefault(new ArgumentException(Name), EqualityComparer);
        }

        /// <summary>
        /// Determines if the object is not equal to default value and throws the exception that is
        /// passed in if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="EqualityComparer">
        /// Equality comparer used to determine if the object is equal to default
        /// </param>
        /// <param name="Exception">Exception to throw</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNotDefault<T>(this T Item, Exception Exception, IEqualityComparer<T> EqualityComparer = null)
        {
            return Item.ThrowIf(x => !EqualityComparer.Check(() => new GenericEqualityComparer<T>()).Equals(x, default(T)), Exception);
        }

        /// <summary>
        /// Determines if the object is not null and throws an ArgumentException if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="Name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNotNull<T>(this T Item, string Name)
        {
            return Item.ThrowIfNotNull(new ArgumentException(Name));
        }

        /// <summary>
        /// Determines if the object is not null and throws the exception passed in if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="Exception">Exception to throw</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNotNull<T>(this T Item, Exception Exception)
        {
            return Item.ThrowIf(x => x != null && !Convert.IsDBNull(x), Exception);
        }

        /// <summary>
        /// Determines if the IEnumerable is not null or empty and throws an ArgumentException if it is
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="Item">The object to check</param>
        /// <param name="Name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static IEnumerable<T> ThrowIfNotNullOrEmpty<T>(this IEnumerable<T> Item, string Name)
        {
            return Item.ThrowIfNotNullOrEmpty(new ArgumentException(Name));
        }

        /// <summary>
        /// Determines if the IEnumerable is not null or empty and throws the exception passed in if
        /// it is
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="Item">The object to check</param>
        /// <param name="Exception">Exception to throw</param>
        /// <returns>Returns Item</returns>
        public static IEnumerable<T> ThrowIfNotNullOrEmpty<T>(this IEnumerable<T> Item, Exception Exception)
        {
            return Item.ThrowIf(x => x != null && x.Count() > 0, Exception);
        }

        /// <summary>
        /// Determines if the object is null and throws an ArgumentNullException if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="Name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNull<T>(this T Item, string Name)
        {
            return Item.ThrowIfNull(new ArgumentNullException(Name));
        }

        /// <summary>
        /// Determines if the object is null and throws the exception passed in if it is
        /// </summary>
        /// <param name="Item">The object to check</param>
        /// <param name="Exception">Exception to throw</param>
        /// <returns>Returns Item</returns>
        public static T ThrowIfNull<T>(this T Item, Exception Exception)
        {
            return Item.ThrowIf(x => x == null || Convert.IsDBNull(x), Exception);
        }

        /// <summary>
        /// Determines if the IEnumerable is null or empty and throws an ArgumentNullException if it is
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="Item">The object to check</param>
        /// <param name="Name">Name of the argument</param>
        /// <returns>Returns Item</returns>
        public static IEnumerable<T> ThrowIfNullOrEmpty<T>(this IEnumerable<T> Item, string Name)
        {
            return Item.ThrowIfNullOrEmpty(new ArgumentNullException(Name));
        }

        /// <summary>
        /// Determines if the IEnumerable is null or empty and throws the exception passed in if it is
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="Item">The object to check</param>
        /// <param name="Exception">Exception to throw</param>
        /// <returns>Returns Item</returns>
        public static IEnumerable<T> ThrowIfNullOrEmpty<T>(this IEnumerable<T> Item, Exception Exception)
        {
            return Item.ThrowIf(x => x == null || x.Count() == 0, Exception);
        }

        /// <summary>
        /// Runs a function based on the number of times specified and returns the results
        /// </summary>
        /// <typeparam name="T">Type that gets returned</typeparam>
        /// <param name="Count">Number of times the function should run</param>
        /// <param name="Function">The function that should run</param>
        /// <returns>The results from the function</returns>
        public static IEnumerable<T> Times<T>(this int Count, Func<int, T> Function)
        {
            var ReturnValue = new System.Collections.Generic.List<T>();
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
    }
}