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
using Utilities.DataTypes;
#endregion

namespace Utilities.Math
{
    /// <summary>
    /// Various math related functions
    /// </summary>
    public static class MathHelper
    {
        #region Public Static Functions

        /// <summary>
        /// Gets the mean value from a list
        /// </summary>
        /// <param name="Values">The list of values</param>
        /// <returns>The mean/average of the list</returns>
        public static double Mean(System.Collections.Generic.List<int> Values)
        {
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
        /// Gets the median from the list
        /// </summary>
        /// <typeparam name="T">The data type of the list</typeparam>
        /// <param name="Values">The list of values</param>
        /// <returns>The median value</returns>
        public static T Median<T>(System.Collections.Generic.List<T> Values)
        {
            if (Values.Count == 0)
                return default(T);
            Values.Sort();
            return Values[(Values.Count / 2)];
        }

        /// <summary>
        /// Gets the mode (item that occurs the most) from the list
        /// </summary>
        /// <typeparam name="T">The data type of the list</typeparam>
        /// <param name="Values">The list of values</param>
        /// <returns>The median value</returns>
        public static T Mode<T>(System.Collections.Generic.List<T> Values)
        {
            if (Values.Count == 0)
                return default(T);
            Bag<T> Items = new Bag<T>();
            for(int x=0;x<Values.Count;++x)
            {
                Items.Add(Values[x]);
            }

            int MaxValue = 0;
            T MaxIndex = default(T);
            foreach(T Key in Items)
            {
                if(Items[Key]>MaxValue)
                {
                    MaxValue = Items[Key];
                    MaxIndex = Key;
                }
            }
            return MaxIndex;
        }

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
    }
}
