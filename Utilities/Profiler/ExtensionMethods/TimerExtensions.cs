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
using System.ComponentModel;

namespace Utilities.Profiler
{
    /// <summary>
    /// Holds timing/profiling related extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TimerExtensions
    {
        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <param name="ActionToTime">Action to time</param>
        /// <param name="FunctionName">Name to associate with the action</param>
        public static void Time(this Action ActionToTime, string FunctionName = "")
        {
            if (ActionToTime == null)
                return;
            using (new Profiler(FunctionName))
                ActionToTime();
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <typeparam name="T">Action input type</typeparam>
        /// <param name="ActionToTime">Action to time</param>
        /// <param name="FunctionName">Name to associate with the action</param>
        /// <param name="Object1">Object 1</param>
        public static void Time<T>(this Action<T> ActionToTime, T Object1, string FunctionName = "")
        {
            if (ActionToTime == null)
                return;
            using (new Profiler(FunctionName))
                ActionToTime(Object1);
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <typeparam name="T1">Action input type 1</typeparam>
        /// <typeparam name="T2">Action input type 2</typeparam>
        /// <param name="ActionToTime">Action to time</param>
        /// <param name="FunctionName">Name to associate with the action</param>
        /// <param name="Object1">Object 1</param>
        /// <param name="Object2">Object 2</param>
        public static void Time<T1, T2>(this Action<T1, T2> ActionToTime, T1 Object1, T2 Object2, string FunctionName = "")
        {
            if (ActionToTime == null)
                return;
            using (new Profiler(FunctionName))
                ActionToTime(Object1, Object2);
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <typeparam name="T1">Action input type 1</typeparam>
        /// <typeparam name="T2">Action input type 2</typeparam>
        /// <typeparam name="T3">Action input type 3</typeparam>
        /// <param name="ActionToTime">Action to time</param>
        /// <param name="FunctionName">Name to associate with the action</param>
        /// <param name="Object1">Object 1</param>
        /// <param name="Object2">Object 2</param>
        /// <param name="Object3">Object 3</param>
        public static void Time<T1, T2, T3>(this Action<T1, T2, T3> ActionToTime, T1 Object1, T2 Object2, T3 Object3, string FunctionName = "")
        {
            if (ActionToTime == null)
                return;
            using (new Profiler(FunctionName))
                ActionToTime(Object1, Object2, Object3);
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <param name="FuncToTime">Action to time</param>
        /// <param name="FunctionName">Name to associate with the action</param>
        /// <typeparam name="R">Type of the value to return</typeparam>
        /// <returns>The value returned by the Func</returns>
        public static R Time<R>(this Func<R> FuncToTime, string FunctionName = "")
        {
            if (FuncToTime == null)
                return default(R);
            using (new Profiler(FunctionName))
                return FuncToTime();
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <param name="FuncToTime">Action to time</param>
        /// <param name="FunctionName">Name to associate with the action</param>
        /// <param name="Object1">Object 1</param>
        /// <typeparam name="T1">Object type 1</typeparam>
        /// <typeparam name="R">Type of the value to return</typeparam>
        /// <returns>The value returned by the Func</returns>
        public static R Time<T1, R>(this Func<T1, R> FuncToTime, T1 Object1, string FunctionName = "")
        {
            if (FuncToTime == null)
                return default(R);
            using (new Profiler(FunctionName))
                return FuncToTime(Object1);
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <param name="FuncToTime">Action to time</param>
        /// <param name="FunctionName">Name to associate with the action</param>
        /// <param name="Object1">Object 1</param>
        /// <param name="Object2">Object 2</param>
        /// <typeparam name="T1">Object type 1</typeparam>
        /// <typeparam name="T2">Object type 2</typeparam>
        /// <typeparam name="R">Type of the value to return</typeparam>
        /// <returns>The value returned by the Func</returns>
        public static R Time<T1, T2, R>(this Func<T1, T2, R> FuncToTime, T1 Object1, T2 Object2, string FunctionName = "")
        {
            if (FuncToTime == null)
                return default(R);
            using (new Profiler(FunctionName))
                return FuncToTime(Object1, Object2);
        }

        /// <summary>
        /// Times an action and places
        /// </summary>
        /// <param name="FuncToTime">Action to time</param>
        /// <param name="FunctionName">Name to associate with the action</param>
        /// <param name="Object1">Object 1</param>
        /// <param name="Object2">Object 2</param>
        /// <param name="Object3">Object 3</param>
        /// <typeparam name="T1">Object type 1</typeparam>
        /// <typeparam name="T2">Object type 2</typeparam>
        /// <typeparam name="T3">Object type 3</typeparam>
        /// <typeparam name="R">Type of the value to return</typeparam>
        /// <returns>The value returned by the Func</returns>
        public static R Time<T1, T2, T3, R>(this Func<T1, T2, T3, R> FuncToTime, T1 Object1, T2 Object2, T3 Object3, string FunctionName = "")
        {
            if (FuncToTime == null)
                return default(R);
            using (new Profiler(FunctionName))
                return FuncToTime(Object1, Object2, Object3);
        }
    }
}