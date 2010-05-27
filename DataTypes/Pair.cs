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
    /// Pairs two items together
    /// </summary>
    /// <typeparam name="T1">Type for the left hand side</typeparam>
    /// <typeparam name="T2">Type for the right hand side</typeparam>
    public class Pair<T1, T2>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Pair()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Left">Left hand side of the pair</param>
        /// <param name="Right">Right hand side of the pair</param>
        public Pair(T1 Left, T2 Right)
        {
            this.Left = Left;
            this.Right = Right;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Left hand item
        /// </summary>
        public T1 Left { get; set; }

        /// <summary>
        /// Right hand item
        /// </summary>
        public T2 Right { get; set; }

        #endregion

        #region Public Overridden Properties

        public override int GetHashCode()
        {
            try
            {
                if (Left != null && Right != null)
                {
                    return Left.GetHashCode() ^ Right.GetHashCode();
                }
                return 0;
            }
            catch { throw; }
        }

        public override bool Equals(object obj)
        {
            try
            {
                if (obj != null && obj is Pair<T1, T2>)
                {
                    return Equals(Left, ((Pair<T1, T2>)obj).Left) && Equals(Right, ((Pair<T1, T2>)obj).Right);
                }
                return false;
            }
            catch { throw; }
        }

        #endregion
    }
}
