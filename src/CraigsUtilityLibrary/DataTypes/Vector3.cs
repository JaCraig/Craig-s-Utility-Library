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

namespace Utilities.DataTypes
{
    /// <summary>
    /// Vector class (holds three items)
    /// </summary>
    public class Vector3
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">X direction</param>
        /// <param name="y">Y direction</param>
        /// <param name="z">Z direction</param>
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Used for converting this to an array and back
        /// </summary>
        public double[] Array
        {
            get { return new double[] { X, Y, Z }; }
            set
            {
                if (value != null && value.Length == 3)
                {
                    X = value[0];
                    Y = value[1];
                    Z = value[2];
                }
            }
        }

        /// <summary>
        /// Returns the magnitude of the vector
        /// </summary>
        public double Magnitude => ((X * X) + (Y * Y) + (Z * Z)).Sqrt();

        /// <summary>
        /// X value
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y Value
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Z value
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// Determines the angle between the vectors
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        /// <returns>Angle between the vectors</returns>
        public static double Angle(Vector3 v1, Vector3 v2)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            v2 = v2 ?? new Vector3(0, 0, 0);
            v1.Normalize();
            v2.Normalize();
            return Math.Acos(DotProduct(v1, v2));
        }

        /// <summary>
        /// The distance between two vectors
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        /// <returns>Distance between the vectors</returns>
        public static double Distance(Vector3 v1, Vector3 v2)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            v2 = v2 ?? new Vector3(0, 0, 0);
            return (((v1.X - v2.X) * (v1.X - v2.X)) + ((v1.Y - v2.Y) * (v1.Y - v2.Y)) + ((v1.Z - v2.Z) * (v1.Z - v2.Z))).Sqrt();
        }

        /// <summary>
        /// Does a dot product
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        /// <returns>a dot product</returns>
        public static double DotProduct(Vector3 v1, Vector3 v2)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            v2 = v2 ?? new Vector3(0, 0, 0);
            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        /// <summary>
        /// Interpolates between the vectors
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        /// <param name="control">Percent to move between 1 and 2</param>
        /// <returns>The interpolated vector</returns>
        public static Vector3 Interpolate(Vector3 v1, Vector3 v2, double control)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            v2 = v2 ?? new Vector3(0, 0, 0);
            var TempVector = new Vector3(0.0, 0.0, 0.0);
            TempVector.X = (v1.X * (1 - control)) + (v2.X * control);
            TempVector.Y = (v1.Y * (1 - control)) + (v2.Y * control);
            TempVector.Z = (v1.Z * (1 - control)) - (v2.Z * control);
            return TempVector;
        }

        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="v1">Item 1</param>
        /// <param name="v2">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            v2 = v2 ?? new Vector3(0, 0, 0);
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        /// Negation
        /// </summary>
        /// <param name="v1">Item 1</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator -(Vector3 v1)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            return new Vector3(-v1.X, -v1.Y, -v1.Z);
        }

        /// <summary>
        /// Not equals
        /// </summary>
        /// <param name="v1">Item 1</param>
        /// <param name="v2">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="v1">Item 1</param>
        /// <param name="d">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator *(Vector3 v1, double d)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            return new Vector3(v1.X * d, v1.Y * d, v1.Z * d);
        }

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="v1">Item 1</param>
        /// <param name="d">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator *(double d, Vector3 v1)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            return new Vector3(v1.X * d, v1.Y * d, v1.Z * d);
        }

        /// <summary>
        /// Does a cross product
        /// </summary>
        /// <param name="v1">Item 1</param>
        /// <param name="v2">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            v2 = v2 ?? new Vector3(0, 0, 0);
            var TempVector = new Vector3(0.0, 0.0, 0.0);
            TempVector.X = (v1.Y * v2.Z) - (v1.Z * v2.Y);
            TempVector.Y = (v1.Z * v2.X) - (v1.X * v2.Z);
            TempVector.Z = (v1.X * v2.Y) - (v1.Y * v2.X);
            return TempVector;
        }

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="v1">Item 1</param>
        /// <param name="d">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator /(Vector3 v1, double d)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            return new Vector3(v1.X / d, v1.Y / d, v1.Z / d);
        }

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="v1">Item 1</param>
        /// <param name="v2">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            v2 = v2 ?? new Vector3(0, 0, 0);
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        /// Less than
        /// </summary>
        /// <param name="v1">Item 1</param>
        /// <param name="v2">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static bool operator <(Vector3 v1, Vector3 v2)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            v2 = v2 ?? new Vector3(0, 0, 0);
            return v1.Magnitude < v2.Magnitude;
        }

        /// <summary>
        /// Less than or equal
        /// </summary>
        /// <param name="v1">Item 1</param>
        /// <param name="v2">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static bool operator <=(Vector3 v1, Vector3 v2)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            v2 = v2 ?? new Vector3(0, 0, 0);
            return v1.Magnitude <= v2.Magnitude;
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="v1">Item 1</param>
        /// <param name="v2">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            if (ReferenceEquals(v1, null) && ReferenceEquals(v2, null))
                return true;
            if (ReferenceEquals(v1, null) || ReferenceEquals(v2, null))
                return false;
            return Equals(v1.X, v2.X) && Equals(v1.Y, v2.Y) && Equals(v1.Z, v2.Z);
        }

        /// <summary>
        /// Greater than
        /// </summary>
        /// <param name="v1">Item 1</param>
        /// <param name="v2">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static bool operator >(Vector3 v1, Vector3 v2)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            v2 = v2 ?? new Vector3(0, 0, 0);
            return v1.Magnitude > v2.Magnitude;
        }

        /// <summary>
        /// Greater than or equal
        /// </summary>
        /// <param name="v1">Item 1</param>
        /// <param name="v2">Item 2</param>
        /// <returns>The resulting vector</returns>
        public static bool operator >=(Vector3 v1, Vector3 v2)
        {
            v1 = v1 ?? new Vector3(0, 0, 0);
            v2 = v2 ?? new Vector3(0, 0, 0);
            return v1.Magnitude >= v2.Magnitude;
        }

        /// <summary>
        /// Determines if the items are equal
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>true if they are, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var Tempobj = obj as Vector3;
            return (object)Tempobj != null && this == Tempobj;
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return (int)(X + Y + Z) % int.MaxValue;
        }

        /// <summary>
        /// Normalizes the vector
        /// </summary>
        public void Normalize()
        {
            double Normal = Magnitude;
            if (Normal > 0)
            {
                Normal = 1 / Normal;
                X *= Normal;
                Y *= Normal;
                Z *= Normal;
            }
        }

        /// <summary>
        /// To string function
        /// </summary>
        /// <returns>String representation of the vector</returns>
        public override string ToString()
        {
            return string.Format("({0},{1},{2})", X, Y, Z);
        }
    }
}