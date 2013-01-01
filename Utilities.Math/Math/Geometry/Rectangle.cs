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
using Utilities.Math.Geometry.BaseClasses;
#endregion

namespace Utilities.Math.Geometry
{
    /// <summary>
    /// Represents a rectangle
    /// </summary>
    public class Rectangle : Shape
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="X">X coordinate (lower left)</param>
        /// <param name="Y">Y coordinate (lower left)</param>
        /// <param name="Width">Width of the rectangle</param>
        /// <param name="Height">Height of the rectangle</param>
        public Rectangle(double X, double Y, double Width, double Height)
            : base(new Point(X, Y), new Point(X + Width, Y + Height), new Point(X + (Width / 2), Y + (Height / 2)))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="LowerLeft">lower left</param>
        /// <param name="UpperRight">upper right</param>
        public Rectangle(Point LowerLeft, Point UpperRight)
            : base(LowerLeft, UpperRight, new Point((UpperRight.X + LowerLeft.X) / 2, (UpperRight.Y + LowerLeft.Y) / 2))
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Height of the rectangle
        /// </summary>
        public double Height { get { return Max.Y - Min.Y; } }

        /// <summary>
        /// Width of the rectangle
        /// </summary>
        public double Width { get { return Max.X - Min.X; } }

        /// <summary>
        /// Area of the rectangle
        /// </summary>
        public double Area { get { return Width * Height; } }

        #endregion

        #region Functions

        /// <summary>
        /// Determines if the two rectangles overlap
        /// </summary>
        /// <param name="Object">Object to check</param>
        /// <returns>True if they overlap, false otherwise</returns>
        public bool Overlap(Rectangle Object)
        {
            return Contains(Object.Min.X, Object.Min.Y)
                    || Contains(Object.Min.X, Object.Max.Y)
                    || Contains(Object.Max.X, Object.Min.Y)
                    || Contains(Object.Max.X, Object.Max.Y)
                    || Object.Contains(Min.X, Min.Y)
                    || Object.Contains(Min.X, Max.Y)
                    || Object.Contains(Max.X, Min.Y)
                    || Object.Contains(Max.X, Max.Y);
        }

        #endregion
    }
}