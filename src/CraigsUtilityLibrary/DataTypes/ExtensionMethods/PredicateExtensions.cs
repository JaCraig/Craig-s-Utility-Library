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
using System.Linq;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Predicate extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class PredicateExtensions
    {
        /// <summary>
        /// Adds the given values to the predicate set
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Predicate">Predicate to add to</param>
        /// <param name="Values">Values to add</param>
        /// <returns>The resulting predicate set</returns>
        public static Predicate<T> AddToSet<T>(this Predicate<T> Predicate, params T[] Values)
        {
            return x => Values.Contains(x) || Predicate(x);
        }

        /// <summary>
        /// Treats the predicates as sets and does a cartesian product of them
        /// </summary>
        /// <typeparam name="T1">Type 1</typeparam>
        /// <typeparam name="T2">Type 2</typeparam>
        /// <param name="Predicate1">Predicate 1</param>
        /// <param name="Predicate2">Predicate 2</param>
        /// <returns>The cartesian product</returns>
        public static Func<T1, T2, bool> CartesianProduct<T1, T2>(this Predicate<T1> Predicate1, Predicate<T2> Predicate2)
        {
            return (x, y) => Predicate1(x) && Predicate2(y);
        }

        /// <summary>
        /// Treats the predicates as sets and does a difference
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="Predicate1">Predicate 1</param>
        /// <param name="Predicate2">Predicate 2</param>
        /// <returns>The difference of the two predicates</returns>
        public static Predicate<T> Difference<T>(this Predicate<T> Predicate1, Predicate<T> Predicate2)
        {
            return x => Predicate1(x) ^ Predicate2(x);
        }

        /// <summary>
        /// Treats predicates as sets and intersects them together
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Predicate1">Predicate 1</param>
        /// <param name="Predicate2">Predicate 2</param>
        /// <returns>The intersected predicate</returns>
        public static Predicate<T> Intersect<T>(this Predicate<T> Predicate1, Predicate<T> Predicate2)
        {
            return x => Predicate1(x) && Predicate2(x);
        }

        /// <summary>
        /// Treats predicates as sets and returns the relative complement
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Predicate1">Predicate 1</param>
        /// <param name="Predicate2">Predicate 2</param>
        /// <returns>The relative complement</returns>
        public static Predicate<T> RelativeComplement<T>(this Predicate<T> Predicate1, Predicate<T> Predicate2)
        {
            return x => Predicate1(x) && !Predicate2(x);
        }

        /// <summary>
        /// Removes the values from the predicate set
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Predicate">Predicate</param>
        /// <param name="Values">Values to remove</param>
        /// <returns>The resulting set</returns>
        public static Predicate<T> RemoveFromSet<T>(this Predicate<T> Predicate, params T[] Values)
        {
            return x => !Values.Contains(x) && Predicate(x);
        }

        /// <summary>
        /// Treats predicates as sets and unions them together
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Predicate1">Predicate 1</param>
        /// <param name="Predicate2">Predicate 2</param>
        /// <returns>The unioned predicate</returns>
        public static Predicate<T> Union<T>(this Predicate<T> Predicate1, Predicate<T> Predicate2)
        {
            return x => Predicate1(x) || Predicate2(x);
        }
    }
}