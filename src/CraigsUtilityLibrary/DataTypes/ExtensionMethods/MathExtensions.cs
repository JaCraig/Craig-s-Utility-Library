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
using System.Linq;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Extension methods that add basic math functions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class MathExtensions
    {
        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The absolute value</returns>
        public static decimal Absolute(this decimal value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The absolute value</returns>
        public static double Absolute(this double value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The absolute value</returns>
        public static float Absolute(this float value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The absolute value</returns>
        public static int Absolute(this int value)
        {
            if (value == int.MinValue)
                throw new ArgumentOutOfRangeException(nameof(value), "value can not be int.MinValue");
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The absolute value</returns>
        public static long Absolute(this long value)
        {
            if (value == -9223372036854775808)
                throw new ArgumentOutOfRangeException(nameof(value), "value can not be -9223372036854775808");
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The absolute value</returns>
        public static short Absolute(this short value)
        {
            if (value == -32768)
                throw new ArgumentOutOfRangeException(nameof(value), "value can not be -32768");
            return Math.Abs(value);
        }

        /// <summary>
        /// Returns E raised to the specified power
        /// </summary>
        /// <param name="value">Power to raise E by</param>
        /// <returns>E raised to the specified power</returns>
        public static double Exp(this double value)
        {
            return Math.Exp(value);
        }

        /// <summary>
        /// Calculates the factorial for a number
        /// </summary>
        /// <param name="input">Input value (N!)</param>
        /// <returns>The factorial specified</returns>
        public static int Factorial(this int input)
        {
            int Value1 = 1;
            for (int x = 2; x <= input; ++x)
                Value1 = Value1 * x;
            return Value1;
        }

        /// <summary>
        /// Returns the greatest common denominator between value1 and value2
        /// </summary>
        /// <param name="value1">Value 1</param>
        /// <param name="value2">Value 2</param>
        /// <returns>The greatest common denominator if one exists</returns>
        public static int GreatestCommonDenominator(this int value1, int value2)
        {
            if (value1 == int.MinValue)
                throw new ArgumentOutOfRangeException(nameof(value1), "value1 can not be int.MinValue");
            if (value2 == int.MinValue)
                throw new ArgumentOutOfRangeException(nameof(value2), "value2 can not be int.MinValue");
            value1 = value1.Absolute();
            value2 = value2.Absolute();
            while (value1 != 0 && value2 != 0)
            {
                if (value1 > value2)
                    value1 %= value2;
                else
                    value2 %= value1;
            }
            return value1 == 0 ? value2 : value1;
        }

        /// <summary>
        /// Returns the greatest common denominator between value1 and value2
        /// </summary>
        /// <param name="value1">Value 1</param>
        /// <param name="value2">Value 2</param>
        /// <returns>The greatest common denominator if one exists</returns>
        public static int GreatestCommonDenominator(this int value1, uint value2)
        {
            if (value1 == int.MinValue)
                throw new ArgumentOutOfRangeException(nameof(value1), "value1 can not be int.MinValue");
            if (value2 == 2147483648)
                throw new ArgumentOutOfRangeException(nameof(value2), "value2 can not be 2147483648");
            return value1.GreatestCommonDenominator((int)value2);
        }

        /// <summary>
        /// Returns the greatest common denominator between value1 and value2
        /// </summary>
        /// <param name="value1">Value 1</param>
        /// <param name="value2">Value 2</param>
        /// <returns>The greatest common denominator if one exists</returns>
        public static int GreatestCommonDenominator(this uint value1, uint value2)
        {
            if (value1 == 2147483648)
                throw new ArgumentOutOfRangeException(nameof(value1), "value1 can not be 2147483648");
            if (value2 == 2147483648)
                throw new ArgumentOutOfRangeException(nameof(value2), "value2 can not be 2147483648");
            return ((int)value1).GreatestCommonDenominator((int)value2);
        }

        /// <summary>
        /// Returns the natural (base e) logarithm of a specified number
        /// </summary>
        /// <param name="value">Specified number</param>
        /// <returns>The natural logarithm of the specified number</returns>
        public static double Log(this double value)
        {
            return Math.Log(value);
        }

        /// <summary>
        /// Returns the logarithm of a specified number in a specified base
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="baseValue">Base</param>
        /// <returns>The logarithm of a specified number in a specified base</returns>
        public static double Log(this double value, double baseValue)
        {
            return Math.Log(value, baseValue);
        }

        /// <summary>
        /// Returns the base 10 logarithm of a specified number
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>The base 10 logarithm of the specified number</returns>
        public static double Log10(this double value)
        {
            return Math.Log10(value);
        }

        /// <summary>
        /// Gets the median from the list
        /// </summary>
        /// <typeparam name="T">The data type of the list</typeparam>
        /// <param name="values">The list of values</param>
        /// <param name="orderBy">Function used to order the values</param>
        /// <returns>
        /// The median value
        /// </returns>
        public static T Median<T>(this IEnumerable<T> values, Func<T, T> orderBy = null)
        {
            if (values == null)
                return default(T);
            if (values.Count() == 0)
                return default(T);
            orderBy = orderBy ?? (x => x);
            values = values.OrderBy(orderBy);
            return values.ElementAt((values.Count() / 2));
        }

        /// <summary>
        /// Gets the mode (item that occurs the most) from the list
        /// </summary>
        /// <typeparam name="T">The data type of the list</typeparam>
        /// <param name="values">The list of values</param>
        /// <returns>
        /// The mode value
        /// </returns>
        public static T Mode<T>(this IEnumerable<T> values)
        {
            if (values == null)
                return default(T);
            if (values.Count() == 0)
                return default(T);
            var Items = new Bag<T>();
            foreach (T Value in values)
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

        /// <summary>
        /// Raises Value to the power of Power
        /// </summary>
        /// <param name="value">Value to raise</param>
        /// <param name="power">Power</param>
        /// <returns>The resulting value</returns>
        public static double Pow(this double value, double power)
        {
            return Math.Pow(value, power);
        }

        /// <summary>
        /// Raises Value to the power of Power
        /// </summary>
        /// <param name="value">Value to raise</param>
        /// <param name="power">Power</param>
        /// <returns>The resulting value</returns>
        public static double Pow(this decimal value, decimal power)
        {
            return Math.Pow((double)value, (double)power);
        }

        /// <summary>
        /// Rounds the value to the number of digits
        /// </summary>
        /// <param name="value">Value to round</param>
        /// <param name="digits">Digits to round to</param>
        /// <param name="rounding">Rounding mode to use</param>
        /// <returns></returns>
        public static double Round(this double value, int digits = 2, MidpointRounding rounding = MidpointRounding.AwayFromZero)
        {
            if (digits < 0)
                digits = 0;
            if (digits > 15)
                digits = 15;
            return Math.Round(value, digits, rounding);
        }

        /// <summary>
        /// Returns the square root of a value
        /// </summary>
        /// <param name="value">Value to take the square root of</param>
        /// <returns>The square root</returns>
        public static double Sqrt(this double value)
        {
            return Math.Sqrt(value);
        }

        /// <summary>
        /// Returns the square root of a value
        /// </summary>
        /// <param name="value">Value to take the square root of</param>
        /// <returns>The square root</returns>
        public static double Sqrt(this float value)
        {
            return Math.Sqrt(value);
        }

        /// <summary>
        /// Returns the square root of a value
        /// </summary>
        /// <param name="value">Value to take the square root of</param>
        /// <returns>The square root</returns>
        public static double Sqrt(this int value)
        {
            return Math.Sqrt(value);
        }

        /// <summary>
        /// Returns the square root of a value
        /// </summary>
        /// <param name="value">Value to take the square root of</param>
        /// <returns>The square root</returns>
        public static double Sqrt(this long value)
        {
            return Math.Sqrt(value);
        }

        /// <summary>
        /// Returns the square root of a value
        /// </summary>
        /// <param name="value">Value to take the square root of</param>
        /// <returns>The square root</returns>
        public static double Sqrt(this short value)
        {
            return Math.Sqrt(value);
        }

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation(this IEnumerable<double> values)
        {
            return values.StandardDeviation(x => x);
        }

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation(this IEnumerable<decimal> values)
        {
            return values.StandardDeviation(x => x);
        }

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation(this IEnumerable<float> values)
        {
            return values.StandardDeviation(x => x);
        }

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation(this IEnumerable<long> values)
        {
            return values.StandardDeviation(x => x);
        }

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation(this IEnumerable<int> values)
        {
            return values.StandardDeviation(x => x);
        }

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// The standard deviation
        /// </returns>
        public static double StandardDeviation<T>(this IEnumerable<T> values, Func<T, double> selector = null)
        {
            return values.Variance(selector).Sqrt();
        }

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// The standard deviation
        /// </returns>
        public static double StandardDeviation<T>(this IEnumerable<T> values, Func<T, decimal> selector)
        {
            return values.Variance(selector).Sqrt();
        }

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// The standard deviation
        /// </returns>
        public static double StandardDeviation<T>(this IEnumerable<T> values, Func<T, float> selector)
        {
            return values.Variance(selector).Sqrt();
        }

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// The standard deviation
        /// </returns>
        public static double StandardDeviation<T>(this IEnumerable<T> values, Func<T, long> selector)
        {
            return values.Variance(selector).Sqrt();
        }

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// The standard deviation
        /// </returns>
        public static double StandardDeviation<T>(this IEnumerable<T> values, Func<T, int> selector)
        {
            return values.Variance(selector).Sqrt();
        }

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(this IEnumerable<double> values)
        {
            return values.Variance(x => x);
        }

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(this IEnumerable<int> values)
        {
            return values.Variance(x => x);
        }

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(this IEnumerable<long> values)
        {
            return values.Variance(x => x);
        }

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(this IEnumerable<decimal> values)
        {
            return values.Variance(x => (double)x);
        }

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(this IEnumerable<float> values)
        {
            return values.Variance(x => x);
        }

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="values">List of values</param>
        /// <param name="Selector">The selector.</param>
        /// <returns>
        /// The variance
        /// </returns>
        public static double Variance<T>(this IEnumerable<T> values, Func<T, double> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            return values.Variance(x => (decimal)selector(x));
        }

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// The variance
        /// </returns>
        public static double Variance<T>(this IEnumerable<T> values, Func<T, int> selector)
        {
            return values.Variance(x => (decimal)selector(x));
        }

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// The variance
        /// </returns>
        public static double Variance<T>(this IEnumerable<T> values, Func<T, long> selector)
        {
            return values.Variance(x => (decimal)selector(x));
        }

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// The variance
        /// </returns>
        public static double Variance<T>(this IEnumerable<T> values, Func<T, float> selector)
        {
            return values.Variance(x => (decimal)selector(x));
        }

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="values">List of values</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// The variance
        /// </returns>
        public static double Variance<T>(this IEnumerable<T> values, Func<T, decimal> selector)
        {
            if (values == null || values.Count() == 0)
                return 0;
            decimal MeanValue = values.Average(selector);
            double Sum = values.Sum(x => (selector(x) - MeanValue).Pow(2));
            return Sum / values.Count();
        }
    }
}