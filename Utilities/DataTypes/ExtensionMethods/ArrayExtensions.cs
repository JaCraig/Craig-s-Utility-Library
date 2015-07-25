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
using System.Diagnostics.Contracts;
using System.Linq;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Array extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ArrayExtensions
    {
        /// <summary>
        /// Clears the array completely
        /// </summary>
        /// <param name="Array">Array to clear</param>
        /// <returns>The final array</returns>
        public static Array Clear(this Array Array)
        {
            if (Array == null)
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

        /// <summary>
        /// Combines two arrays and returns a new array containing both values
        /// </summary>
        /// <typeparam name="ArrayType">Type of the data in the array</typeparam>
        /// <param name="Array1">Array 1</param>
        /// <param name="Additions">Arrays to concat onto the first item</param>
        /// <returns>A new array containing both arrays' values</returns>
        public static ArrayType[] Concat<ArrayType>(this ArrayType[] Array1, params ArrayType[][] Additions)
        {
            Contract.Requires<ArgumentNullException>(Array1 != null, "Array1");
            Contract.Requires<ArgumentNullException>(Additions != null, "Additions");
            Contract.Requires<ArgumentNullException>(Contract.ForAll(Additions, x => x != null), "Additions");
            ArrayType[] Result = new ArrayType[Array1.Length + Additions.Sum(x => x.Length)];
            int Offset = Array1.Length;
            Array.Copy(Array1, 0, Result, 0, Array1.Length);
            for (int x = 0; x < Additions.Length; ++x)
            {
                Array.Copy(Additions[x], 0, Result, Offset, Additions[x].Length);
                Offset += Additions[x].Length;
            }
            return Result;
        }
    }
}