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
using System.Text;
using System.Xml.Serialization;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.Math
{
    /// <summary>
    /// Matrix used in linear algebra
    /// </summary>
    [Serializable()]
    public class Matrix
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Width">Width of the matrix</param>
        /// <param name="Height">Height of the matrix</param>
        /// <param name="Values">Values to use in the matrix</param>
        public Matrix(int Width, int Height, double[,] Values = null)
        {
            _Width = Width;
            _Height = Height;
            this.Values = (Values == null) ? new double[Width, Height] : Values;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Width of the matrix
        /// </summary>
        [XmlElement]
        public virtual int Width
        {
            get { return _Width; }
            set { _Width = value; Values = new double[Width, Height]; }
        }

        /// <summary>
        /// Height of the matrix
        /// </summary>
        [XmlElement]
        public virtual int Height
        {
            get { return _Height; }
            set { _Height = value; Values = new double[Width, Height]; }
        }

        /// <summary>
        /// Sets the values of the matrix
        /// </summary>
        /// <param name="X">X position</param>
        /// <param name="Y">Y position</param>
        /// <returns>the value at a point in the matrix</returns>
        public virtual double this[int X, int Y]
        {
            get
            {
                if (!X.Between(0, _Width))
                    throw new ArgumentOutOfRangeException("X");
                if (!Y.Between(0, _Height))
                    throw new ArgumentOutOfRangeException("Y");
                return Values[X, Y];
            }

            set
            {
                if (!X.Between(0, _Width))
                    throw new ArgumentOutOfRangeException("X");
                if (!Y.Between(0, _Height))
                    throw new ArgumentOutOfRangeException("Y");
                Values[X, Y] = value;
            }
        }

        /// <summary>
        /// Values for the matrix
        /// </summary>
        [XmlElement]
        public virtual double[,] Values { get; set; }

        #endregion

        #region Private Variables
        private int _Width = 1;
        private int _Height = 1;
        #endregion

        #region Operators

        /// <summary>
        /// Determines if two matrices are equal
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="M2">Matrix 2</param>
        /// <returns>True if they are equal, false otherwise</returns>
        public static bool operator ==(Matrix M1, Matrix M2)
        {
            if ((object)M1 == null && (object)M2 == null)
                return true;
            if ((object)M1 == null)
                return false;
            if ((object)M2 == null)
                return false;
            if (M1.Width != M2.Width || M1.Height != M2.Height)
                return false;
            for (int x = 0; x <= M1.Width; ++x)
                for (int y = 0; y <= M1.Height; ++y)
                    if (M1[x, y] != M2[x, y])
                        return false;
            return true;
        }

        /// <summary>
        /// Determines if two matrices are unequal
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="M2">Matrix 2</param>
        /// <returns>True if they are not equal, false otherwise</returns>
        public static bool operator !=(Matrix M1, Matrix M2)
        {
            return !(M1 == M2);
        }

        /// <summary>
        /// Adds two matrices
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="M2">Matrix 2</param>
        /// <returns>The result</returns>
        public static Matrix operator +(Matrix M1, Matrix M2)
        {
            if (M1 == null)
                throw new ArgumentNullException("M1");
            if (M2 == null)
                throw new ArgumentNullException("M2");
            if (M1.Width != M2.Width || M1.Height != M2.Height)
                throw new ArgumentException("Both matrices must be the same dimensions.");
            Matrix TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = M1[x, y] + M2[x, y];
            return TempMatrix;
        }

        /// <summary>
        /// Subtracts two matrices
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="M2">Matrix 2</param>
        /// <returns>The result</returns>
        public static Matrix operator -(Matrix M1, Matrix M2)
        {
            if (M1 == null)
                throw new ArgumentNullException("M1");
            if (M2 == null)
                throw new ArgumentNullException("M2");
            if (M1.Width != M2.Width || M1.Height != M2.Height)
                throw new ArgumentException("Both matrices must be the same dimensions.");
            Matrix TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = M1[x, y] - M2[x, y];
            return TempMatrix;
        }

        /// <summary>
        /// Negates a matrix
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <returns>The result</returns>
        public static Matrix operator -(Matrix M1)
        {
            if (M1 == null)
                throw new ArgumentNullException("M1");
            Matrix TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = -M1[x, y];
            return TempMatrix;
        }

        /// <summary>
        /// Multiplies two matrices
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="M2">Matrix 2</param>
        /// <returns>The result</returns>
        public static Matrix operator *(Matrix M1, Matrix M2)
        {
            if (M1 == null)
                throw new ArgumentNullException("M1");
            if (M2 == null)
                throw new ArgumentNullException("M2");
            if (M1.Width != M2.Width || M1.Height != M2.Height)
                throw new ArgumentException("Dimensions for the matrices are incorrect.");
            Matrix TempMatrix = new Matrix(M2.Width, M1.Height);
            for (int x = 0; x < M2.Width; ++x)
            {
                for (int y = 0; y < M1.Height; ++y)
                {
                    TempMatrix[x, y] = 0.0;
                    for (int i = 0; i < M1.Width; ++i)
                        for (int j = 0; j < M2.Height; ++j)
                            TempMatrix[x, y] += (M1[i, y] * M2[x, j]);
                }
            }
            return TempMatrix;
        }

        /// <summary>
        /// Multiplies a matrix by a value
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="D">Value to multiply by</param>
        /// <returns>The result</returns>
        public static Matrix operator *(Matrix M1, double D)
        {
            if (M1 == null)
                throw new ArgumentNullException("M1");
            Matrix TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = M1[x, y] * D;
            return TempMatrix;
        }

        /// <summary>
        /// Multiplies a matrix by a value
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="D">Value to multiply by</param>
        /// <returns>The result</returns>
        public static Matrix operator *(double D, Matrix M1)
        {
            if (M1 == null)
                throw new ArgumentNullException("M1");
            Matrix TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = M1[x, y] * D;
            return TempMatrix;
        }

        /// <summary>
        /// Divides a matrix by a value
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="D">Value to divide by</param>
        /// <returns>The result</returns>
        public static Matrix operator /(Matrix M1, double D)
        {
            if (M1 == null)
                throw new ArgumentNullException("M1");
            return M1 * (1 / D);
        }

        /// <summary>
        /// Divides a matrix by a value
        /// </summary>
        /// <param name="M1">Matrix 1</param>
        /// <param name="D">Value to divide by</param>
        /// <returns>The result</returns>
        public static Matrix operator /(double D, Matrix M1)
        {
            if (M1 == null)
                throw new ArgumentNullException("M1");
            return M1 * (1 / D);
        }

        #endregion

        #region Public Overridden Functions

        /// <summary>
        /// Determines if the objects are equal
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if they are, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj is Matrix)
                return this == (Matrix)obj;
            return false;
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
            StringBuilder Builder = new StringBuilder();
            string Seperator = "";
            Builder.Append("{").Append(System.Environment.NewLine);
            for (int x = 0; x < Width; ++x)
            {
                Builder.Append("{");
                for (int y = 0; y < Height; ++y)
                {
                    Builder.Append(Seperator).Append(this[x, y]);
                    Seperator = ",";
                }
                Builder.Append("}").Append(System.Environment.NewLine);
                Seperator = "";
            }
            Builder.Append("}");
            return Builder.ToString();
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Transposes the matrix
        /// </summary>
        /// <returns>Returns a new transposed matrix</returns>
        public virtual Matrix Transpose()
        {
            Matrix TempValues = new Matrix(Height, Width);
            for (int x = 0; x < Width; ++x)
                for (int y = 0; y < Height; ++y)
                    TempValues[y, x] = Values[x, y];
            return TempValues;
        }

        /// <summary>
        /// Gets the determinant of a square matrix
        /// </summary>
        /// <returns>The determinant of a square matrix</returns>
        public virtual double Determinant()
        {
            if (Width != Height)
                throw new InvalidOperationException("The determinant can not be calculated for a non square matrix");
            if (Width == 2)
                return (this[0, 0] * this[1, 1]) - (this[0, 1] * this[1, 0]);
            double Answer = 0.0;
            for (int x = 0; x < Width; ++x)
            {
                Matrix TempMatrix = new Matrix(Width - 1, Height - 1);
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

        #endregion
    }
}