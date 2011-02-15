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
using Utilities.DataTypes.Comparison;
#endregion

namespace Utilities.Math
{
    /// <summary>
    /// Various math related functions
    /// </summary>
    public static class MathHelper
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
        public static bool Between<T>(T Value, T Low, T High) where T : IComparable
        {
            GenericComparer<T> Comparer = new GenericComparer<T>();
            if (Comparer.Compare(High, Value) >= 0 || Comparer.Compare(Value, Low) >= 0)
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
        public static int Clamp(int Value, int Max, int Min)
        {
            Value = Value > Max ? Max : Value;
            return Value < Min ? Min : Value;
        }

        /// <summary>
        /// Clamps a value between two values
        /// </summary>
        /// <param name="Value">Value sent in</param>
        /// <param name="Max">Max value it can be (inclusive)</param>
        /// <param name="Min">Min value it can be (inclusive)</param>
        /// <returns>The value set between Min and Max</returns>
        public static double Clamp(double Value, double Max, double Min)
        {
            Value = Value > Max ? Max : Value;
            return Value < Min ? Min : Value;
        }

        /// <summary>
        /// Clamps a value between two values
        /// </summary>
        /// <param name="Value">Value sent in</param>
        /// <param name="Max">Max value it can be (inclusive)</param>
        /// <param name="Min">Min value it can be (inclusive)</param>
        /// <returns>The value set between Min and Max</returns>
        public static float Clamp(float Value, float Max, float Min)
        {
            Value = Value > Max ? Max : Value;
            return Value < Min ? Min : Value;
        }

        #endregion

        #region Factorial

        /// <summary>
        /// Calculates the factorial for a number
        /// </summary>
        /// <param name="Input">Input value (N!)</param>
        /// <returns>The factorial specified</returns>
        public static int Factorial(int Input)
        {
            int Value1 = 1;
            for (int x = 2; x <= Input; ++x)
            {
                Value1 = Value1 * x;
            }
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
        public static int Max(int InputA, int InputB)
        {
            return InputA > InputB ? InputA : InputB;
        }

        /// <summary>
        /// Returns the maximum value between the two
        /// </summary>
        /// <param name="InputA">Input A</param>
        /// <param name="InputB">Input B</param>
        /// <returns>The maximum value</returns>
        public static double Max(double InputA, double InputB)
        {
            return InputA > InputB ? InputA : InputB;
        }

        /// <summary>
        /// Returns the maximum value between the two
        /// </summary>
        /// <param name="InputA">Input A</param>
        /// <param name="InputB">Input B</param>
        /// <returns>The maximum value</returns>
        public static float Max(float InputA, float InputB)
        {
            return InputA > InputB ? InputA : InputB;
        }

        /// <summary>
        /// Returns the max value from the list
        /// </summary>
        /// <param name="Input">Input list</param>
        /// <returns>The maximum value</returns>
        public static float Max(System.Collections.Generic.List<float> Input)
        {
            if (Input == null)
                throw new ArgumentNullException("Input");
            float ReturnValue = float.MinValue;
            foreach (float Value in Input)
            {
                ReturnValue = Max(Value, ReturnValue);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Returns the max value from the list
        /// </summary>
        /// <param name="Input">Input list</param>
        /// <returns>The maximum value</returns>
        public static double Max(System.Collections.Generic.List<double> Input)
        {
            if (Input == null)
                throw new ArgumentNullException("Input");
            double ReturnValue = double.MinValue;
            foreach (double Value in Input)
            {
                ReturnValue = Max(Value, ReturnValue);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Returns the max value from the list
        /// </summary>
        /// <param name="Input">Input list</param>
        /// <returns>The maximum value</returns>
        public static int Max(System.Collections.Generic.List<int> Input)
        {
            if (Input == null)
                throw new ArgumentNullException("Input");
            int ReturnValue = int.MinValue;
            foreach (int Value in Input)
            {
                ReturnValue = Max(Value, ReturnValue);
            }
            return ReturnValue;
        }

        #endregion

        #region Mean

        /// <summary>
        /// Gets the mean value from a list
        /// </summary>
        /// <param name="Values">The list of values</param>
        /// <returns>The mean/average of the list</returns>
        public static double Mean(System.Collections.Generic.List<int> Values)
        {
            if (Values == null)
                return 0.0;
            if (Values.Count == 0)
                return 0.0;
            double ReturnValue = 0.0;
            for (int x = 0; x < Values.Count; ++x)
            {
                ReturnValue += Values[x];
            }
            return ReturnValue / (double)Values.Count;
        }

        /// <summary>
        /// Gets the mean value from a list
        /// </summary>
        /// <param name="Values">The list of values</param>
        /// <returns>The mean/average of the list</returns>
        public static double Mean(System.Collections.Generic.List<double> Values)
        {
            if (Values == null)
                return 0.0;
            if (Values.Count == 0)
                return 0.0;
            double ReturnValue = 0.0;
            for (int x = 0; x < Values.Count; ++x)
            {
                ReturnValue += Values[x];
            }
            return ReturnValue / (double)Values.Count;
        }

        #endregion

        #region Median

        /// <summary>
        /// Gets the median from the list
        /// </summary>
        /// <typeparam name="T">The data type of the list</typeparam>
        /// <param name="Values">The list of values</param>
        /// <returns>The median value</returns>
        public static T Median<T>(System.Collections.Generic.List<T> Values)
        {
            if (Values == null)
                return default(T);
            if (Values.Count == 0)
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
        public static int Min(int InputA, int InputB)
        {
            return InputA < InputB ? InputA : InputB;
        }

        /// <summary>
        /// Returns the minimum value between the two
        /// </summary>
        /// <param name="InputA">Input A</param>
        /// <param name="InputB">Input B</param>
        /// <returns>The minimum value</returns>
        public static double Min(double InputA, double InputB)
        {
            return InputA < InputB ? InputA : InputB;
        }

        /// <summary>
        /// Returns the minimum value between the two
        /// </summary>
        /// <param name="InputA">Input A</param>
        /// <param name="InputB">Input B</param>
        /// <returns>The minimum value</returns>
        public static float Min(float InputA, float InputB)
        {
            return InputA < InputB ? InputA : InputB;
        }

        /// <summary>
        /// Returns the min value from the list
        /// </summary>
        /// <param name="Input">Input list</param>
        /// <returns>The minimum value</returns>
        public static float Min(System.Collections.Generic.List<float> Input)
        {
            if (Input == null)
                throw new ArgumentNullException("Input");
            float ReturnValue = float.MaxValue;
            foreach (float Value in Input)
            {
                ReturnValue = Min(Value, ReturnValue);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Returns the min value from the list
        /// </summary>
        /// <param name="Input">Input list</param>
        /// <returns>The minimum value</returns>
        public static double Min(System.Collections.Generic.List<double> Input)
        {
            if (Input == null)
                throw new ArgumentNullException("Input");
            double ReturnValue = double.MaxValue;
            foreach (double Value in Input)
            {
                ReturnValue = Min(Value, ReturnValue);
            }
            return ReturnValue;
        }

        /// <summary>
        /// Returns the min value from the list
        /// </summary>
        /// <param name="Input">Input list</param>
        /// <returns>The minimum value</returns>
        public static int Min(System.Collections.Generic.List<int> Input)
        {
            if (Input == null)
                throw new ArgumentNullException("Input");
            int ReturnValue = int.MaxValue;
            foreach (int Value in Input)
            {
                ReturnValue = Min(Value, ReturnValue);
            }
            return ReturnValue;
        }

        #endregion

        #region Mode

        /// <summary>
        /// Gets the mode (item that occurs the most) from the list
        /// </summary>
        /// <typeparam name="T">The data type of the list</typeparam>
        /// <param name="Values">The list of values</param>
        /// <returns>The median value</returns>
        public static T Mode<T>(System.Collections.Generic.List<T> Values)
        {
            if (Values == null)
                return default(T);
            if (Values.Count == 0)
                return default(T);
            Bag<T> Items = new Bag<T>();
            for (int x = 0; x < Values.Count; ++x)
            {
                Items.Add(Values[x]);
            }

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

        #region StandardDeviation

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        /// <param name="Values">List of values</param>
        /// <returns>The standard deviation</returns>
        public static double StandardDeviation(System.Collections.Generic.List<double> Values)
        {
            return System.Math.Sqrt(Variance(Values));
        }

        #endregion

        #region Variance

        /// <summary>
        /// Calculates the variance of a list of values
        /// </summary>
        /// <param name="Values">List of values</param>
        /// <returns>The variance</returns>
        public static double Variance(System.Collections.Generic.List<double> Values)
        {
            if (Values == null || Values.Count == 0)
                return 0;
            double MeanValue = Mean(Values);
            double Sum = 0;
            for (int x = 0; x < Values.Count; ++x)
            {
                Sum += System.Math.Pow(Values[x] - MeanValue, 2);
            }
            return Sum / (double)Values.Count;
        }

        #endregion

        #endregion
    }
}