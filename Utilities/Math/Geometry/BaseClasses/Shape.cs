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
using System;
#endregion

namespace Utilities.Math.Geometry.BaseClasses
{
    /// <summary>
    /// Base shape class
    /// </summary>
    public abstract class Shape
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Max">Max X,Y value</param>
        /// <param name="Min">Min X,Y value</param>
        /// <param name="Center">Center of the shape</param>
        public Shape(Point Min,Point Max,Point Center)
        {
            this.Min = Min;
            this.Max = Max;
            this.Center = Center;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Min X,Y value
        /// </summary>
        public Point Min { get; protected set; }

        /// <summary>
        /// Max X,Y value
        /// </summary>
        public Point Max { get; protected set; }

        /// <summary>
        /// Center coordinate
        /// </summary>
        public Point Center { get; protected set; }

        /// <summary>
        /// The predicate/set that can be used to determine if a point is within the shape
        /// </summary>
        public virtual Predicate<Point> Set { get { return x => x.X >= Min.X && x.X <= Max.X && x.Y >= Min.Y && x.Y <= Max.Y; } }

        #endregion

        #region Functions

        /// <summary>
        /// Determines if the point is within the shape and returns true if it is, false otherwise
        /// </summary>
        /// <param name="X">X Coordinate</param>
        /// <param name="Y">Y Coordinate</param>
        /// <returns>True if it is contained, false otherwise</returns>
        public bool Contains(double X, double Y)
        {
            return Set(new Point(X, Y));
        }

        /// <summary>
        /// Determines the Euclidean distance between two points
        /// </summary>
        /// <param name="X1">X1 coordinate</param>
        /// <param name="Y1">Y1 coordinate</param>
        /// <param name="X2">X2 coordinate</param>
        /// <param name="Y2">Y2 coordinate</param>
        /// <returns>The Euclidean distance between the two points</returns>
        protected double EuclideanDistance(double X1, double Y1, double X2, double Y2)
        {
            return ((X1 - X2).Pow(2) + (Y1 - Y2).Pow(2)).Sqrt();
        }

        #endregion
    }
}