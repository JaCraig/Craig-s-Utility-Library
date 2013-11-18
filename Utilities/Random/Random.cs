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

#endregion

namespace Utilities.Random
{
    /// <summary>
    /// Utility class for handling random
    /// information.
    /// </summary>
    public class Random : System.Random
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Random()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Seed">Seed value</param>
        public Random(int Seed)
            : base(Seed)
        {
        }

        #endregion

        #region Private Variables

        private static Random GlobalSeed = new Random();
        [ThreadStatic]
        private static Random Local;

        #endregion

        #region Static Functions

        #region ThreadSafeNext

        /// <summary>
        /// A thread safe version of a random number generation
        /// </summary>
        /// <param name="Min">Minimum value</param>
        /// <param name="Max">Maximum value</param>
        /// <returns>A randomly generated value</returns>
        public static int ThreadSafeNext(int Min = int.MinValue, int Max = int.MaxValue)
        {
            if (Local == null)
            {
                int Seed;
                lock (GlobalSeed)
                    Seed = GlobalSeed.Next();
                Local = new Random(Seed);
            }
            return Local.Next(Min, Max);
        }

        #endregion

        #endregion
    }
}