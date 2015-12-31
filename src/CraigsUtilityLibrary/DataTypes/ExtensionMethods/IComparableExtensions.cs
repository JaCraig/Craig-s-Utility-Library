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
using Utilities.DataTypes.Comparison;

namespace Utilities.DataTypes
{
    /// <summary>
    /// IComparable extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IComparableExtensions
    {
        /// <summary>
        /// Checks if an item is between two values
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="value">Value to check</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <param name="comparer">Comparer used to compare the values (defaults to GenericComparer)"</param>
        /// <returns>True if it is between the values, false otherwise</returns>
        public static bool Between<T>(this T value, T min, T max, IComparer<T> comparer = null)
            where T : IComparable
        {
            comparer = comparer ?? new GenericComparer<T>();
            return comparer.Compare(max, value) >= 0 && comparer.Compare(value, min) >= 0;
        }

        /// <summary>
        /// Clamps a value between two values
        /// </summary>
        /// <param name="value">Value sent in</param>
        /// <param name="max">Max value it can be (inclusive)</param>
        /// <param name="Min">Min value it can be (inclusive)</param>
        /// <param name="comparer">Comparer to use (defaults to GenericComparer)</param>
        /// <returns>The value set between Min and Max</returns>
        public static T Clamp<T>(this T value, T max, T min, IComparer<T> comparer = null)
            where T : IComparable
        {
            comparer = comparer ?? new GenericComparer<T>();
            if (comparer.Compare(max, value) < 0)
                return max;
            if (comparer.Compare(value, min) < 0)
                return min;
            return value;
        }

        /// <summary>
        /// Returns the maximum value between the two
        /// </summary>
        /// <param name="inputA">Input A</param>
        /// <param name="inputB">Input B</param>
        /// <param name="comparer">Comparer to use (defaults to GenericComparer)</param>
        /// <returns>The maximum value</returns>
        public static T Max<T>(this T inputA, T inputB, IComparer<T> comparer = null)
            where T : IComparable
        {
            comparer = comparer ?? new GenericComparer<T>();
            return comparer.Compare(inputA, inputB) < 0 ? inputB : inputA;
        }

        /// <summary>
        /// Returns the minimum value between the two
        /// </summary>
        /// <param name="inputA">Input A</param>
        /// <param name="inputB">Input B</param>
        /// <param name="comparer">Comparer to use (defaults to GenericComparer)</param>
        /// <returns>The minimum value</returns>
        public static T Min<T>(this T inputA, T inputB, IComparer<T> comparer = null)
            where T : IComparable
        {
            comparer = comparer ?? new GenericComparer<T>();
            return comparer.Compare(inputA, inputB) > 0 ? inputB : inputA;
        }
    }
}