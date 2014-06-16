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

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;

#endregion Usings

namespace Utilities.DataTypes
{
    /// <summary>
    /// Extension methods that add basic math functions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class MathExtensions
    {
        #region Public Static Functions

        #region Absolute

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="Value">Value</param>
        /// <returns>The absolute value</returns>
        public static decimal Absolute(this decimal Value)
        {
            return System.Math.Abs(Value);
        }

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="Value">Value</param>
        /// <returns>The absolute value</returns>
        public static double Absolute(this double Value)
        {
            return System.Math.Abs(Value);
        }

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="Value">Value</param>
        /// <returns>The absolute value</returns>
        public static float Absolute(this float Value)
        {
            return System.Math.Abs(Value);
        }

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="Value">Value</param>
        /// <returns>The absolute value</returns>
        public static int Absolute(this int Value)
        {
            Contract.Requires<ArgumentOutOfRangeException>(Value != Int32.MinValue, "Value can not be Int32.MinValue");
            return System.Math.Abs(Value);
        }

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="Value">Value</param>
        /// <returns>The absolute value</returns>
        public static long Absolute(this long Value)
        {
            Contract.Requires<ArgumentException>(Value != -9223372036854775808);
            return System.Math.Abs(Value);
        }

        /// <summary>
        /// Returns the absolute value
        /// </summary>
        /// <param name="Value">Value</param>
        /// <returns>The absolute value</returns>
        public static short Absolute(this short Value)
        {
            Contract.Requires<ArgumentException>(Value != -32768);
            return System.Math.Abs(Value);
        }

        #endregion Absolute

        #region Exp

        /// <summary>
        /// Returns E raised to the specified power
        /// </summary>
        /// <param name="Value">Power to raise E by</param>
        /// <returns>E raised to the specified power</returns>
        public static double Exp(this double Value)
        {
            return System.Math.Exp(Value);
        }

        #endregion Exp

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

        #endregion Factorial

        #region GreatestCommonDenominator

        /// <summary>
        /// Returns the greatest common denominator between value1 and value2
        /// </summary>
        /// <param name="Value1">Value 1</param>
        /// <param name="Value2">Value 2</param>
        /// <returns>The greatest common denominator if one exists</returns>
        public static int GreatestCommonDenominator(this int Value1, int Value2)
        {
            Contract.Requires<ArgumentOutOfRangeException>(Value1 != Int32.MinValue, "Value1 can not be Int32.MinValue");
            Contract.Requires<ArgumentOutOfRangeException>(Value2 != Int32.MinValue, "Value2 can not be Int32.MinValue");
            Value1 = Value1.Absolute();
            Value2 = Value2.Absolute();
            while (Value1 != 0 && Value2 != 0)
            {
                if (Value1 > Value2)
                    Value1 %= Value2;
                else
                    Value2 %= Value1;
            }
            return Value1 == 0 ? Value2 : Value1;
        }

        /// <summary>
        /// Returns the greatest common denominator between value1 and value2
        /// </summary>
        /// <param name="Value1">Value 1</param>
        /// <param name="Value2">Value 2</param>
        /// <returns>The greatest common denominator if one exists</returns>
        [CLSCompliant(false)]
        public static int GreatestCommonDenominator(this int Value1, uint Value2)
        {
            Contract.Requires<ArgumentOutOfRangeException>(Value1 != Int32.MinValue, "Value1 can not be Int32.MinValue");
            Contract.Requires<ArgumentException>(Value2 != 2147483648);
            return Value1.GreatestCommonDenominator((int)Value2);
        }

        /// <summary>
        /// Returns the greatest common denominator between value1 and value2
        /// </summary>
        /// <param name="Value1">Value 1</param>
        /// <param name="Value2">Value 2</param>
        /// <returns>The greatest common denominator if one exists</returns>
        [CLSCompliant(false)]
        public static int GreatestCommonDenominator(this uint Value1, uint Value2)
        {
            Contract.Requires<ArgumentException>(Value1 != 2147483648);
            Contract.Requires<ArgumentException>(Value2 != 2147483648);
            return ((int)Value1).GreatestCommonDenominator((int)Value2);
        }

        #endregion GreatestCommonDenominator

        #region Log

        /// <summary>
        /// Returns the natural (base e) logarithm of a specified number
        /// </summary>
        /// <param name="Value">Specified number</param>
        /// <returns>The natural logarithm of the specified number</returns>
        public static double Log(this double Value)
        {
            return System.Math.Log(Value);
        }

        /// <summary>
        /// Returns the logarithm of a specified number in a specified base
        /// </summary>
        /// <param name="Value">Value</param>
        /// <param name="Base">Base</param>
        /// <returns>The logarithm of a specified number in a specified base</returns>
        public static double Log(this double Value, double Base)
        {
            return System.Math.Log(Value, Base);
        }

        #endregion Log

        #region Log10

        /// <summary>
        /// Returns the base 10 logarithm of a specified number
        /// </summary>
        /// <param name="Value">Value</param>
        /// <returns>The base 10 logarithm of the specified number</returns>
        public static double Log10(this double Value)
        {
            return System.Math.Log10(Value);
        }

        #endregion Log10

        #region Median

        /// <summary>
        /// Gets the median from the list
        /// </summary>
        /// <typeparam name="T">The data type of the list</typeparam>
        /// <param name="Values">The list of values</param>
        /// <returns>The median value</returns>
        public static T Median<T>(this IEnumerable<T> Values)
        {
            if (Values == null)
                return default(T);
            if (Values.Count() == 0)
                return default(T);
            Values = Values.OrderBy(x => x);
            return Values.ElementAt((Values.Count() / 2));
        }

        #endregion Median

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

        #endregion Mode

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

        #endregion Pow

        #region Round

        /// <summary>
        /// Rounds the value to the number of digits
        /// </summary>
        /// <param name="Value">Value to round</param>
        /// <param name="Digits">Digits to round to</param>
        /// <param name="Rounding">Rounding mode to use</param>
        /// <returns></returns>
        public static double Round(this double Value, int Digits = 2, MidpointRounding Rounding = MidpointRounding.AwayFromZero)
        {
            Contract.Requires<ArgumentException>(Digits >= 0);
            Contract.Requires<ArgumentException>(Digits <= 15);
            return System.Math.Round(Value, Digits, Rounding);
        }

        #endregion Round

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

        #endregion StandardDeviation

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

        #endregion Sqrt

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

        #endregion Variance

        #endregion Public Static Functions
    }
}