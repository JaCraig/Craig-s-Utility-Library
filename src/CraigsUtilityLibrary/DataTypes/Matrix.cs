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
using System.Text;

namespace Utilities.DataTypes
{
    /// <summary>
    /// Matrix used in linear algebra
    /// </summary>
    public class Matrix
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">Width of the matrix</param>
        /// <param name="height">Height of the matrix</param>
        /// <param name="values">Values to use in the matrix</param>
        public Matrix(int width, int height, double[,] values = null)
        {
            Width = width > -1 ? width : 0;
            Height = height > -1 ? height : 0;
            Values = values ?? new double[width, height];
        }

        /// <summary>
        /// Height of the matrix
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Values for the matrix
        /// </summary>
        public double[,] Values { get; private set; }

        /// <summary>
        /// Width of the matrix
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Sets the values of the matrix
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns>the value at a point in the matrix</returns>
        public double this[int x, int y]
        {
            get
            {
                if (x < 0)
                    x = 0;
                else if (x >= Width)
                    x = Width - 1;
                if (y < 0)
                    y = 0;
                else if (y >= Height)
                    y = Height - 1;
                return Values[x, y];
            }

            set
            {
                if (x < 0)
                    x = 0;
                else if (x >= Width)
                    x = Width - 1;
                if (y < 0)
                    y = 0;
                else if (y >= Height)
                    y = Height - 1;
                Values[x, y] = value;
            }
        }

        /// <summary>
        /// Subtracts two matrices
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="m2">Matrix 2</param>
        /// <returns>The result</returns>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            m1 = m1 ?? new Matrix(0, 0);
            m2 = m2 ?? new Matrix(0, 0);
            if (m1.Width != m2.Width || m1.Height != m2.Height)
                throw new ArgumentException("Both matrices must be the same dimensions.");
            var TempMatrix = new Matrix(m1.Width, m1.Height);
            for (int x = 0; x < m1.Width; ++x)
                for (int y = 0; y < m1.Height; ++y)
                    TempMatrix[x, y] = m1[x, y] - m2[x, y];
            return TempMatrix;
        }

        /// <summary>
        /// Negates a matrix
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <returns>The result</returns>
        public static Matrix operator -(Matrix m1)
        {
            m1 = m1 ?? new Matrix(0, 0);
            var TempMatrix = new Matrix(m1.Width, m1.Height);
            for (int x = 0; x < m1.Width; ++x)
                for (int y = 0; y < m1.Height; ++y)
                    TempMatrix[x, y] = -m1[x, y];
            return TempMatrix;
        }

        /// <summary>
        /// Determines if two matrices are unequal
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="m2">Matrix 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return !(m1 == m2);
        }

        /// <summary>
        /// Multiplies two matrices
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="m2">Matrix 2</param>
        /// <returns>The result</returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            m1 = m1 ?? new Matrix(0, 0);
            m2 = m2 ?? new Matrix(0, 0);
            if (m1.Width != m2.Width || m1.Height != m2.Height)
                throw new ArgumentException("Both matrices must be the same dimensions.");
            var TempMatrix = new Matrix(m2.Width, m1.Height);
            for (int x = 0; x < m2.Width; ++x)
            {
                for (int y = 0; y < m1.Height; ++y)
                {
                    TempMatrix[x, y] = 0.0;
                    for (int i = 0; i < m1.Width; ++i)
                        for (int j = 0; j < m2.Height; ++j)
                            TempMatrix[x, y] += (m1[i, y] * m2[x, j]);
                }
            }
            return TempMatrix;
        }

        /// <summary>
        /// Multiplies a matrix by a value
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="d">Value to multiply by</param>
        /// <returns>The result</returns>
        public static Matrix operator *(Matrix m1, double d)
        {
            m1 = m1 ?? new Matrix(0, 0);
            var TempMatrix = new Matrix(m1.Width, m1.Height);
            for (int x = 0; x < m1.Width; ++x)
                for (int y = 0; y < m1.Height; ++y)
                    TempMatrix[x, y] = m1[x, y] * d;
            return TempMatrix;
        }

        /// <summary>
        /// Multiplies a matrix by a value
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="d">Value to multiply by</param>
        /// <returns>The result</returns>
        public static Matrix operator *(double d, Matrix m1)
        {
            m1 = m1 ?? new Matrix(0, 0);
            var TempMatrix = new Matrix(m1.Width, m1.Height);
            for (int x = 0; x < m1.Width; ++x)
                for (int y = 0; y < m1.Height; ++y)
                    TempMatrix[x, y] = m1[x, y] * d;
            return TempMatrix;
        }

        /// <summary>
        /// Divides a matrix by a value
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="d">Value to divide by</param>
        /// <returns>The result</returns>
        public static Matrix operator /(Matrix m1, double d)
        {
            m1 = m1 ?? new Matrix(0, 0);
            return m1 * (1 / d);
        }

        /// <summary>
        /// Divides a matrix by a value
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="d">Value to divide by</param>
        /// <returns>The result</returns>
        public static Matrix operator /(double d, Matrix m1)
        {
            m1 = m1 ?? new Matrix(0, 0);
            return m1 * (1 / d);
        }

        /// <summary>
        /// Adds two matrices
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="m2">Matrix 2</param>
        /// <returns>The result</returns>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            m1 = m1 ?? new Matrix(0, 0);
            m2 = m2 ?? new Matrix(0, 0);
            if (m1.Width != m2.Width || m1.Height != m2.Height)
                throw new ArgumentException("Both matrices must be the same dimensions.");
            var TempMatrix = new Matrix(m1.Width, m1.Height);
            for (int x = 0; x < m1.Width; ++x)
                for (int y = 0; y < m1.Height; ++y)
                    TempMatrix[x, y] = m1[x, y] + m2[x, y];
            return TempMatrix;
        }

        /// <summary>
        /// Determines if two matrices are equal
        /// </summary>
        /// <param name="m1">Matrix 1</param>
        /// <param name="m2">Matrix 2</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public static bool operator ==(Matrix m1, Matrix m2)
        {
            if ((object)m1 == null && (object)m2 == null)
                return true;
            if ((object)m1 == null)
                return false;
            if ((object)m2 == null)
                return false;
            if (m1.Width != m2.Width || m1.Height != m2.Height)
                return false;
            for (int x = 0; x <= m1.Width; ++x)
                for (int y = 0; y <= m1.Height; ++y)
                    if (m1[x, y] != m2[x, y])
                        return false;
            return true;
        }

        /// <summary>
        /// Gets the determinant of a square matrix
        /// </summary>
        /// <returns>The determinant of a square matrix</returns>
        public double Determinant()
        {
            if (Width != Height)
                throw new InvalidOperationException("The determinant can not be calculated for a non square matrix");
            if (Width == 2)
                return (this[0, 0] * this[1, 1]) - (this[0, 1] * this[1, 0]);
            double Answer = 0.0;
            for (int x = 0; x < Width; ++x)
            {
                var TempMatrix = new Matrix(Width - 1, Height - 1);
                int WidthCounter = 0;
                for (int y = 0; y < Width; ++y)
                {
                    if (y != x)
                    {
                        for (int z = 1; z < Height; ++z)
                            TempMatrix[WidthCounter, z - 1] = this[y, z];
                        ++WidthCounter;
                    }
                }
                if (x % 2 == 0)
                {
                    Answer += TempMatrix.Determinant();
                }
                else
                {
                    Answer -= TempMatrix.Determinant();
                }
            }
            return Answer;
        }

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if they are, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var Tempobj = obj as Matrix;
            return Tempobj != null && this == Tempobj;
        }

        /// <summary>
        /// Gets the hash code for the object
        /// </summary>
        /// <returns>The hash code for the object</returns>
        public override int GetHashCode()
        {
            double Hash = 0;
            for (int x = 0; x < Width; ++x)
                for (int y = 0; y < Height; ++y)
                    Hash += this[x, y];
            return (int)Hash;
        }

        /// <summary>
        /// Gets the string representation of the matrix
        /// </summary>
        /// <returns>The matrix as a string</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            string Seperator = "";
            Builder.Append("{").Append(Environment.NewLine);
            for (int x = 0; x < Width; ++x)
            {
                Builder.Append("{");
                for (int y = 0; y < Height; ++y)
                {
                    Builder.Append(Seperator).Append(this[x, y]);
                    Seperator = ",";
                }
                Builder.Append("}").Append(Environment.NewLine);
                Seperator = "";
            }
            Builder.Append("}");
            return Builder.ToString();
        }

        /// <summary>
        /// Transposes the matrix
        /// </summary>
        /// <returns>Returns a new transposed matrix</returns>
        public Matrix Transpose()
        {
            var TempValues = new Matrix(Height, Width);
            for (int x = 0; x < Width; ++x)
                for (int y = 0; y < Height; ++y)
                    TempValues[y, x] = Values[x, y];
            return TempValues;
        }
    }
}