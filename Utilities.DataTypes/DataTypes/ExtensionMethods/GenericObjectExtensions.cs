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

        #region Return

        /// <summary>
        /// Used to determine if an object, or it's properties are null (Although can be used for other things)
        /// </summary>
        /// <typeparam name="T">Input type</typeparam>
        /// <typeparam name="R">Output type</typeparam>
        /// <param name="Object">Object to check</param>
        /// <param name="Function">Property, function, etc. to run</param>
        /// <param name="DefaultValue">Default value to return if Object is null</param>
        /// <returns>The value returned by the function or the default value if the object is null or the function returns a null value</returns>
        public static R Return<T, R>(this T Object, Func<T, R> Function, R DefaultValue = default(R))
        {
            if (Object.IsNull())
                return DefaultValue;
            R ReturnValue = Function(Object);
            return ReturnValue.IsNull() ? DefaultValue : ReturnValue;
        }

        #endregion

        #region Chain

        /// <summary>
        /// Allows actions to be chained together
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="Object">Object to run the action on</param>
        /// <param name="Action">Action to run</param>
        /// <returns>The original object</returns>
        public static T Chain<T>(this T Object, Action<T> Action)
        {
            Action(Object);
            return Object;
        }

        /// <summary>
        /// Allows actions to be chained together
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <typeparam name="R">Return type</typeparam>
        /// <param name="Object">Object to run the action on</param>
        /// <param name="Function">Function to run</param>
        /// <returns>The result from the function</returns>
        public static R Chain<T, R>(this T Object, Func<T, R> Function)
        {
            return Function(Object);
        }

        #endregion

        #region Do

        /// <summary>
        /// Similar to Chain, except checks if the Object or Action is null first and returns the default value if they are
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="Object">Object to run the action on</param>
        /// <param name="Action">Action to run</param>
        /// <param name="DefaultValue">Default value to return if the action or object is null</param>
        /// <returns>The original object or the default value</returns>
        public static T Do<T>(this T Object, Action<T> Action, T DefaultValue = default(T))
        {
            if (Object.IsNull() || Action.IsNull())
                return DefaultValue;
            return Object.Chain(Action);
        }

        /// <summary>
        /// Similar to Chain, except checks if the Object or Function is null first and returns the default value if they are
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="Object">Object to run the function on</param>
        /// <param name="Function">Function to run</param>
        /// <param name="DefaultValue">Default value to return if the action or object is null</param>
        /// <returns>The result of the function or the default value</returns>
        public static R Do<T, R>(this T Object, Func<T, R> Function, R DefaultValue = default(R))
        {
            if (Object.IsNull() || Function.IsNull())
                return DefaultValue;
            return Object.Chain(Function);
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

        #endregion
    }
}
