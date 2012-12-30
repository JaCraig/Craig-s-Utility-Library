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
    /// Represents a 2D circle
    /// </summary>
    public class Circle : Shape
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        /// <param name="Radius">Radius of the circle</param>
        public Circle(double X, double Y, double Radius)
            : base(new Point(X - Radius, Y - Radius), new Point(X + Radius, Y + Radius), new Point(X, Y))
        {
            this.Radius = Radius;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Radius of the circle
        /// </summary>
        public double Radius { get; protected set; }

        /// <summary>
        /// Diameter of the circle
        /// </summary>
        public double Diameter { get { return Radius * 2; } }

        /// <summary>
        /// Circumference of the circle
        /// </summary>
        public double Circumference { get { return Diameter * System.Math.PI; } }

        /// <summary>
        /// Area of the circle
        /// </summary>
        public double Area { get { return System.Math.PI * Radius.Pow(2); } }

        /// <summary>
        /// The predicate/set that can be used to determine if a point is within the shape
        /// </summary>
        public override System.Predicate<Point> Set { get { return x => EuclideanDistance(this.Center.X, this.Center.Y, x.X, x.Y) <= Radius; } }

        #endregion

        #region Functions

        /// <summary>
        /// Determines if the two circles overlap
        /// </summary>
        /// <param name="Object">Object to check</param>
        /// <returns>True if they overlap, false otherwise</returns>
        public bool Overlap(Circle Object)
        {
            return EuclideanDistance(this.Center.X, this.Center.Y, Object.Center.X, Object.Center.Y) <= Radius + Object.Radius;
        }

        #endregion
    }
}