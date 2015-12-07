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
    /// Array extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ArrayExtensions
    {
        /// <summary>
        /// Clears the array completely
        /// </summary>
        /// <param name="array">Array to clear</param>
        /// <returns>
        /// The final array
        /// </returns>
        public static Array Clear(this Array array)
        {
            if (array == null)
                return null;
            Array.Clear(array, 0, array.Length);
            return array;
        }

        /// <summary>
        /// Clears the array completely
        /// </summary>
        /// <typeparam name="ArrayType">Array type</typeparam>
        /// <param name="array">Array to clear</param>
        /// <returns>
        /// The final array
        /// </returns>
        public static ArrayType[] Clear<ArrayType>(this ArrayType[] array)
        {
            return (ArrayType[])((Array)array).Clear();
        }

        /// <summary>
        /// Combines two arrays and returns a new array containing both values
        /// </summary>
        /// <typeparam name="ArrayType">Type of the data in the array</typeparam>
        /// <param name="array1">Array 1</param>
        /// <param name="additions">Arrays to concat onto the first item</param>
        /// <returns>A new array containing both arrays' values</returns>
        public static ArrayType[] Concat<ArrayType>(this ArrayType[] array1, params ArrayType[][] additions)
        {
            if (array1 == null || additions == null)
                return null;
            var finalAdditions = additions.Where(x => x != null);
            ArrayType[] Result = new ArrayType[array1.Length + finalAdditions.Sum(x => x.Length)];
            int Offset = array1.Length;
            Array.Copy(array1, 0, Result, 0, array1.Length);
            foreach (var item in finalAdditions)
            {
                Array.Copy(item, 0, Result, Offset, item.Length);
                Offset += item.Length;
            }
            return Result;
        }
    }
}