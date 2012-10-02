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
using System.Threading;
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// Generic extensions dealing with objects
    /// </summary>
    public static class GenericObjectExtensions
    {
        #region Functions

        #region Async

        /// <summary>
        /// Runs an action async
        /// </summary>
        /// <param name="Action">Action to run</param>
        public static void Async(this Action Action)
        {
            new Thread(Action.Invoke).Start();
        }

        #endregion

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
            Object = Object.NullCheck(DefaultObjectValue);
            if (Action.IsNull() || Object.IsNull())
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
            Object = Object.NullCheck(DefaultObjectValue);
            if (Function.IsNull() || Object.IsNull())
                return DefaultReturnValue;
            R ReturnValue = Function(Object);
            return ReturnValue.IsNull() ? DefaultReturnValue : ReturnValue;
        }

        #endregion

        #region Execute

        /// <summary>
        /// Executes a function, repeating it a number of times in case it fails
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="Function">Function to run</param>
        /// <param name="Attempts">Number of times to attempt it</param>
        /// <param name="RetryDelay">The amount of milliseconds to wait between tries</param>
        /// <param name="TimeOut">Max amount of time to wait for the function to run (waits for the current attempt to finish before checking)</param>
        /// <returns>The returned value from the function</returns>
        public static T Execute<T>(this Func<T> Function, int Attempts = 3, int RetryDelay = 0, int TimeOut = int.MaxValue)
        {
            Function.ThrowIfNull("Function");
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
        /// <param name="TimeOut">Max amount of time to wait for the function to run (waits for the current attempt to finish before checking)</param>
        public static void Execute(this Action Action, int Attempts = 3, int RetryDelay = 0, int TimeOut = int.MaxValue)
        {
            Action.ThrowIfNull("Action");
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
            if (Holder.IsNotNull())
                throw Holder;
        }

        #endregion

        #region If

        /// <summary>
        /// Determines if the object fullfills the predicate and if it does, returns itself. Otherwise the default value.
        /// If the predicate is null, it returns the default value.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="Object">Object to check</param>
        /// <param name="Predicate">Predicate to run on the object</param>
        /// <param name="DefaultValue">Default value to return if it does not succeed the predicate test</param>
        /// <returns>The original value if predicate is true, the default value otherwise</returns>
        public static T If<T>(this T Object, Predicate<T> Predicate, T DefaultValue = default(T))
        {
            if (Predicate.IsNull())
                return DefaultValue;
            return Predicate(Object) ? Object : DefaultValue;
        }

        #endregion

        #region NotIf

        /// <summary>
        /// Determines if the object fails the predicate and if it does, returns itself. Otherwise the default value.
        /// If the predicate is null, it returns the default value.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="Object">Object to check</param>
        /// <param name="Predicate">Predicate to run on the object</param>
        /// <param name="DefaultValue">Default value to return if it succeeds the predicate test</param>
        /// <returns>The original value if predicate is false, the default value otherwise</returns>
        public static T NotIf<T>(this T Object, Predicate<T> Predicate, T DefaultValue = default(T))
        {
            if (Predicate.IsNull())
                return DefaultValue;
            return Predicate(Object) ? DefaultValue : Object;
        }

        #endregion

        #region ThrowIfTrue

        /// <summary>
        /// Throws the specified exception if the predicate is true for the item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="Item">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is true</param>
        /// <returns>the original Item</returns>
        public static T ThrowIfTrue<T>(this T Item, Predicate<T> Predicate, Exception Exception)
        {
            Predicate.ThrowIfNull("Predicate");
            Exception.ThrowIfNull("Exception");
            if (Predicate(Item))
                throw Exception;
            return Item;
        }

        #endregion

        #region ThrowIfFalse

        /// <summary>
        /// Throws the specified exception if the predicate is false for the item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="Item">The item</param>
        /// <param name="Predicate">Predicate to check</param>
        /// <param name="Exception">Exception to throw if predicate is false</param>
        /// <returns>the original Item</returns>
        public static T ThrowIfFalse<T>(this T Item, Predicate<T> Predicate, Exception Exception)
        {
            Predicate.ThrowIfNull("Predicate");
            Exception.ThrowIfNull("Exception");
            return Item.ThrowIfTrue(x => !Predicate(x), Exception);
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