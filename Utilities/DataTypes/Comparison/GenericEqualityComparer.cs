/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
#endregion

namespace Utilities.DataTypes.Comparison
{
    /// <summary>
    /// Generic equality comparer
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        #region Functions

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
            if (x is IEnumerable && y is IEnumerable)
            {
                GenericEqualityComparer<object> Comparer = new GenericEqualityComparer<object>();
                IEnumerator XEnumerator = ((IEnumerable)x).GetEnumerator();
                IEnumerator YEnumerator = ((IEnumerable)y).GetEnumerator();
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
            if (x is IEquatable<T>)
                return ((IEquatable<T>)x).Equals(y);
            if (x is IComparable<T>)
                return ((IComparable<T>)x).CompareTo(y) == 0;
            if (x is IComparable)
                return ((IComparable)x).CompareTo(y) == 0;
            return x.Equals(y);
        }

        public int GetHashCode(T obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}