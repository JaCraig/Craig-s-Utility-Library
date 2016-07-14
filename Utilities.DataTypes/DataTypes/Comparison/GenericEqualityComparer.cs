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
using System.Collections;
using System.Collections.Generic;

namespace Utilities.DataTypes.Comparison
{
    /// <summary>
    /// Generic equality comparer
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Determines if the two items are equal
        /// </summary>
        /// <param name="x">Object 1</param>
        /// <param name="y">Object 2</param>
        /// <returns>True if they are, false otherwise</returns>
        public bool Equals(T x, T y)
        {
            if (!typeof(T).IsValueType
                || (typeof(T).IsGenericType
                && typeof(T).GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>))))
            {
                if (Object.Equals(x, default(T)))
                    return Object.Equals(y, default(T));
                if (Object.Equals(y, default(T)))
                    return false;
            }
            if (x.GetType() != y.GetType())
                return false;
            var IEnumerablex = x as IEnumerable;
            var IEnumerabley = y as IEnumerable;
            if (IEnumerablex != null && IEnumerabley != null)
            {
                var Comparer = new GenericEqualityComparer<object>();
                var XEnumerator = IEnumerablex.GetEnumerator();
                var YEnumerator = IEnumerabley.GetEnumerator();
                while (true)
                {
                    bool XFinished = !XEnumerator.MoveNext();
                    bool YFinished = !YEnumerator.MoveNext();
                    if (XFinished || YFinished)
                        return XFinished & YFinished;
                    if (!Comparer.Equals(XEnumerator.Current, YEnumerator.Current))
                        return false;
                }
            }
            var TempEquality = x as IEqualityComparer<T>;
            if (TempEquality != null)
                return TempEquality.Equals(y);
            var TempComparable = x as IComparable<T>;
            if (TempComparable != null)
                return TempComparable.CompareTo(y) == 0;
            var TempComparable2 = x as IComparable;
            if (TempComparable2 != null)
                return TempComparable2.CompareTo(y) == 0;
            return x.Equals(y);
        }

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <param name="obj">Object to get the hash code of</param>
        /// <returns>The object's hash code</returns>
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}