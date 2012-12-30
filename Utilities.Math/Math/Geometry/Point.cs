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
using Utilities.Math.ExtensionMethods;
using Utilities.Math.Geometry.BaseClasses;
#endregion

namespace Utilities.Math.Geometry
{
    /// <summary>
    /// Contains an individual point's information
    /// </summary>
    public class Point
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        public Point(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        #endregion

        #region Properties

        /// <summary>
        /// X coordinate
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public double Y { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Translates the point by X and Y
        /// </summary>
        /// <param name="X">X direction translation</param>
        /// <param name="Y">Y direction translation</param>
        public void Translate(double X, double Y)
        {
            this.X += X;
            this.Y += Y;
        }

        #endregion
    }
}