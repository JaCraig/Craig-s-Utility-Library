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

#endregion

namespace Utilities.DataTypes
{
    /// <summary>
    /// Combines three items together
    /// </summary>
    /// <typeparam name="T1">Type for the first item</typeparam>
    /// <typeparam name="T2">Type for the second item</typeparam>
    /// <typeparam name="T3">Type for the third item</typeparam>
    public class Triple<T1, T2, T3>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Triple()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <param name="Third">Third item</param>
        public Triple(T1 First, T2 Second, T3 Third)
        {

            this.First = First;
            this.Second = Second;
            this.Third = Third;
        }

        #endregion

        #region Properties

        /// <summary>
        /// First item
        /// </summary>
        public T1 First { get; set; }

        /// <summary>
        /// Second item
        /// </summary>
        public T2 Second { get; set; }

        /// <summary>
        /// Third item
        /// </summary>
        public T3 Third { get; set; }

        #endregion

        #region Public Overridden Properties

        public override int GetHashCode()
        {
            try
            {
                if (First != null && Second != null && Third != null)
                {
                    return (First.GetHashCode() ^ Second.GetHashCode()) % Third.GetHashCode();
                }
                return 0;
            }
            catch { throw; }
        }

        public override bool Equals(object obj)
        {
            try
            {
                if (obj != null && obj is Triple<T1, T2, T3>)
                {
                    return Equals(First, ((Triple<T1, T2, T3>)obj).First)
                        && Equals(Second, ((Triple<T1, T2, T3>)obj).Second)
                        && Equals(Third, ((Triple<T1, T2, T3>)obj).Third);
                }
                return false;
            }
            catch { throw; }
        }

        #endregion
    }
}
