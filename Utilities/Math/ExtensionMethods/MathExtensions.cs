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
using Utilities.DataTypes;
using System;
using System.Linq;
using Utilities.DataTypes.Comparison;
using System.Collections.Generic;
#endregion


namespace Utilities.Math.ExtensionMethods
{
    /// <summary>
    /// Extension methods that add basic math functions
    /// </summary>
    public static class MathExtensions
    {
        #region Public Static Functions

        #region Between

        /// <summary>
        /// Determines if a value is between two values
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="Value">Value to check</param>
        /// <param name="Low">Low bound (inclusive)</param>
        /// <param name="High">High bound (inclusive)</param>
        /// <returns>True if it is between the low and high values</returns>
        public static bool Between<T>(this T Value, T Low, T High) where T : IComparable
        {
            GenericComparer<T> Comparer = new GenericComparer<T>();
            if (Comparer.Compare(High, Value) >= 0 && Comparer.Compare(Value, Low) >= 0)
                return true;
            return false;
        }

        #endregion

        #region Clamp

        /// <summary>
        /// Clamps a value between two values
        /// </summary>
        /// <param name="Value">Value sent in</param>
        /// <param name="Max">Max value it can be (inclusive)</param>
        /// <param name="Min">Min value it can be (inclusive)</param>
        /// <returns>The value set between Min and Max</returns>
        public static T Clamp<T>(this T Value, T Max, T Min) where T : IComparable
        {
            GenericComparer<T> Comparer = new GenericComparer<T>();
            if (Comparer.Compare(Max, Value) < 0)
                return Max;
            if (Comparer.Compare(Value, Min) < 0)
                return Min;
            return Value;
        }

        #endregion

        #region Factorial

        /// <summary>
        /// Calculates the factorial for a number
        /// </summary>
        /// <param name="Input">Input value (N!)</param>
        /// <returns>The factorial specified</returns>
        public static int Factorial(this int Input)
        {
            int Value1 = 1;
            for (int x = 2; x <= Input; ++x)
                Value1 = Value1 * x;
            return Value1;
        }

        #endregion

        #region Max

        /// <summary>
        /// Returns the maximum value between the two
        /// </summary>
        /// <param name="InputA">Input A</param>
        /// <param name="InputB">Input B</param>
        /// <returns>The maximum value</returns>
        public static T Max<T>(this T InputA, T InputB) where T : IComparable
        {
            GenericComparer<T> Comparer = new GenericComparer<T>();
            if (Comparer.Compare(InputA, InputB) < 0)
                return InputB;
            return InputA;
        }

        #endregion

        #region Median

        /// <summary>
        /// Gets the median from the list
        /// </summary>
        /// <typeparam name="T">The data type of the list</typeparam>
        /// <param name="Values">The list of values</param>
        /// <returns>The median value</returns>
        public static T Median<T>(this System.Collections.Generic.List<T> Values)
        {
            if (Values == null)
                return default(T);
            if (Values.Count() == 0)
                return default(T);
            Values.Sort();
            return Values[(Values.Count / 2)];
        }

        #endregion

        #region Min

        /// <summary>
        /// Returns the minimum value between the two
        /// </summary>
        /// <param name="InputA">Input A</param>
        /// <param name="InputB">Input B</param>
        /// <returns>The minimum value</returns>
        public static T Min<T>(this T InputA, T InputB) where T : IComparable
        {
            GenericComparer<T> Comparer = new GenericComparer<T>();
            if (Comparer.Compare(InputA, InputB) > 0)
                return InputB;
            return InputA;
        }

        #endregion

        #region Mode

        /// <summary>
        /// Gets the mode (item that occurs the most) from the list
        /// </summary>
        /// <typeparam name="T">The data type of the list</typeparam>
        /// <param name="Values">The list of values</param>
        /// <returns>The mode value</returns>
        public static T Mode<T>(this IEnumerable<T> Values)
        {
            if (Values == null)
                return default(T);
            if (Values.Count() == 0)
                return default(T);
            Bag<T> Items = new Bag<T>();
            foreach (T Value in Values)
                Items.Add(Value);
            int MaxValue = 0;
            T MaxIndex = default(T);
            foreach (T Key in Items)
            {
                if (Items[Key] > MaxValue)
                {
                    MaxValue = Items[Key];
                    MaxIndex = Key;
                }
            }
            return MaxIndex;
        }

        #endregion

        #region Pow

        /// <summary>
        /// Raises Value to the power of Power
        /// </summary>
        /// <param name="Value">Value to raise</param>
        /// <param name="Power">Power</param>
        /// <returns>The resulting value</returns>
        public static double Pow(this double Value, double Power)
        {
            return System.Math.Pow(Value, Power);
        }

        #endregion

        #region StandardDeviation

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <param name="Values">List of values</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation(this IEnumerable<double> Values)
        {
            return Values.Variance().Sqrt();
        }

        #endregion

        #region Sqrt

        /// <summary>
        /// Returns the square root of a value
        /// </summary>
        /// <param name="Value">Value to take the square root of</param>
        /// <returns>The square root</returns>
        public static double Sqrt(this double Value)
        {
            return System.Math.Sqrt(Value);
        }

        #endregion

        #region Variance

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="Values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(this IEnumerable<double> Values)
        {
            if (Values == null || Values.Count() == 0)
                return 0;
            double MeanValue = Values.Average();
            double Sum = 0;
            foreach (double Value in Values)
                Sum += (Value - MeanValue).Pow(2);
            return Sum / (double)Values.Count();
        }

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="Values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(this IEnumerable<int> Values)
        {
            if (Values == null || Values.Count() == 0)
                return 0;
            double MeanValue = Values.Average();
            double Sum = 0;
            foreach (int Value in Values)
                Sum += (Value - MeanValue).Pow(2);
            return Sum / (double)Values.Count();
        }

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="Values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(this IEnumerable<float> Values)
        {
            if (Values == null || Values.Count() == 0)
                return 0;
            double MeanValue = Values.Average();
            double Sum = 0;
            foreach (int Value in Values)
                Sum += (Value - MeanValue).Pow(2);
            return Sum / (double)Values.Count();
        }

        #endregion

        #endregion
    }
}