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
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// Array extensions
    /// </summary>
    public static class ArrayExtensions
    {
        #region Functions

        #region Clear

        /// <summary>
        /// Clears the array completely
        /// </summary>
        /// <param name="Array">Array to clear</param>
        /// <returns>The final array</returns>
        public static Array Clear(this Array Array)
        {
            if (Array.IsNull())
                return null;
            System.Array.Clear(Array, 0, Array.Length);
            return Array;
        }

        /// <summary>
        /// Clears the array completely
        /// </summary>
        /// <param name="Array">Array to clear</param>
        /// <typeparam name="ArrayType">Array type</typeparam>
        /// <returns>The final array</returns>
        public static ArrayType[] Clear<ArrayType>(this ArrayType[] Array)
        {
            return (ArrayType[])((Array)Array).Clear();
        }

        #endregion

        #region Combine

        /// <summary>
        /// Combines two arrays and returns a new array containing both values
        /// </summary>
        /// <typeparam name="ArrayType">Type of the data in the array</typeparam>
        /// <param name="Array1">Array 1</param>
        /// <param name="Array2">Array 2</param>
        /// <returns>A new array containing both arrays' values</returns>
        public static ArrayType[] Combine<ArrayType>(this ArrayType[] Array1, ArrayType[] Array2)
        {
            if (Array1.IsNull() && Array2.IsNull())
                return null;
            int ResultLength = (Array1.IsNull() ? 0 : Array1.Length) + (Array2.IsNull() ? 0 : Array2.Length);
            ArrayType[] ReturnValue = new ArrayType[ResultLength];
            int StartPosition = 0;
            if (Array1.IsNotNull())
            {
                Array.Copy(Array1, ReturnValue, Array1.Length);
                StartPosition = Array1.Length;
            }
            if (Array2.IsNotNull())
                Array.Copy(Array2, 0, ReturnValue, StartPosition, Array2.Length);
            return ReturnValue;
        }

        #endregion

        #endregion
    }
}