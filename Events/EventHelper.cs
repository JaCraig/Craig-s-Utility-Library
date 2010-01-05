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

#endregion

namespace Utilities.Events
{
    /// <summary>
    /// Helps with events
    /// </summary>
    public static class EventHelper
    {
        #region Public Static Functions

        /// <summary>
        /// Raises an event
        /// </summary>
        /// <typeparam name="T">The type of the event args</typeparam>
        /// <param name="Delegate">The delegate</param>
        /// <param name="EventArgs">The event args</param>
        public static void Raise<T>(T EventArgs, Action<T> Delegate) where T : class
        {
            try
            {
                if (Delegate != null)
                {
                    Delegate(EventArgs);
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Raises an event
        /// </summary>
        /// <typeparam name="T">The type of the event args</typeparam>
        /// <param name="Delegate">The delegate</param>
        /// <param name="Sender">The sender</param>
        /// <param name="EventArg">The event args</param>
        public static void Raise<T>(EventHandler<T> Delegate, object Sender, T EventArg) where T : System.EventArgs
        {
            try
            {
                if (Delegate != null)
                {
                    Delegate(Sender, EventArg);
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Raises an event
        /// </summary>
        /// <typeparam name="T1">The event arg type</typeparam>
        /// <typeparam name="T2">The return type</typeparam>
        /// <param name="Delegate">The delegate</param>
        /// <param name="EventArgs">The event args</param>
        /// <returns>The value returned by the function</returns>
        public static T2 Raise<T1, T2>(T1 EventArgs, Func<T1, T2> Delegate) where T1 : class
        {
            try
            {
                if (Delegate != null)
                {
                    return Delegate(EventArgs);
                }
                return default(T2);
            }
            catch { throw; }
        }

        #endregion
    }
}