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
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Math.ExtensionMethods;
using System.Globalization;
#endregion

namespace Utilities.Math
{
    /// <summary>
    /// Represents a fraction
    /// </summary>
    public class Fraction
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Numerator">Numerator</param>
        /// <param name="Denominator">Denominator</param>
        public Fraction(int Numerator, int Denominator)
        {
            this.Numerator = Numerator;
            this.Denominator = Denominator;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Numerator">Numerator</param>
        /// <param name="Denominator">Denominator</param>
        public Fraction(double Numerator, double Denominator)
        {
            while (Numerator != System.Math.Round(Numerator, MidpointRounding.AwayFromZero)
                || Denominator != System.Math.Round(Denominator, MidpointRounding.AwayFromZero))
            {
                Numerator *= 10;
                Denominator *= 10;
            }
            this.Numerator = (int)Numerator;
            this.Denominator = (int)Denominator;
            this.Reduce();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Numerator">Numerator</param>
        /// <param name="Denominator">Denominator</param>
        public Fraction(decimal Numerator, decimal Denominator)
        {
            while (Numerator != System.Math.Round(Numerator, MidpointRounding.AwayFromZero)
                || Denominator != System.Math.Round(Denominator, MidpointRounding.AwayFromZero))
            {
                Numerator *= 10;
                Denominator *= 10;
            }
            this.Numerator = (int)Numerator;
            this.Denominator = (int)Denominator;
            this.Reduce();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Numerator">Numerator</param>
        /// <param name="Denominator">Denominator</param>
        public Fraction(float Numerator, float Denominator)
        {
            while (Numerator != System.Math.Round(Numerator, MidpointRounding.AwayFromZero)
                || Denominator != System.Math.Round(Denominator, MidpointRounding.AwayFromZero))
            {
                Numerator *= 10;
                Denominator *= 10;
            }
            this.Numerator = (int)Numerator;
            this.Denominator = (int)Denominator;
            this.Reduce();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Numerator of the faction
        /// </summary>
        public int Numerator { get; set; }

        /// <summary>
        /// Denominator of the fraction
        /// </summary>
        public int Denominator { get; set; }

        #endregion

        #region Functions

        #region ToString

        /// <summary>
        /// Displays the fraction as a string
        /// </summary>
        /// <returns>The fraction as a string</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", Numerator, Denominator);
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets the hash code of the fraction
        /// </summary>
        /// <returns>The hash code of the fraction</returns>
        public override int GetHashCode()
        {
            return Numerator.GetHashCode() % Denominator.GetHashCode();
        }

        #endregion

        #region Equals

        /// <summary>
        /// Determines if the fractions are equal
        /// </summary>
        /// <param name="obj">object to check</param>
        /// <returns>True if they are, false otherwise</returns>
        public override bool Equals(object obj)
        {
            Fraction Other = obj as Fraction;
            if (Other.IsNull())
                return false;
            decimal Value1 = this;
            decimal Value2 = Other;
            return Value1 == Value2;
        }

        #endregion

        #region Reduce

        /// <summary>
        /// Reduces the fraction (finds the greatest common denominator and divides the numerator/denominator by it).
        /// </summary>
        public void Reduce()
        {
            int GCD = Numerator.GreatestCommonDenominator(Denominator);
            if (GCD != 0)
            {
                this.Numerator /= GCD;
                this.Denominator /= GCD;
            }
        }

        #endregion

        #region Inverse

        /// <summary>
        /// Returns the inverse of the fraction
        /// </summary>
        /// <returns>The inverse</returns>
        public Fraction Inverse()
        {
            return new Fraction((int)Denominator, Numerator);
        }

        #endregion

        #endregion

        #region Operators

        #region Equals

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(Fraction First, Fraction Second)
        {
            return First.Equals(Second);
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(Fraction First, double Second)
        {
            return First.Equals(Second);
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator ==(double First, Fraction Second)
        {
            return Second.Equals(First);
        }

        #endregion

        #region Not Equals

        /// <summary>
        /// Not equals operator
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator !=(Fraction First, Fraction Second)
        {
            return !(First == Second);
        }

        /// <summary>
        /// Not equals operator
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator !=(Fraction First, double Second)
        {
            return !(First == Second);
        }

        /// <summary>
        /// Not equals operator
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>True if they are, false otherwise</returns>
        public static bool operator !=(double First, Fraction Second)
        {
            return !(First == Second);
        }

        #endregion

        #region ToDouble

        /// <summary>
        /// Converts the fraction to a double
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The fraction as a double</returns>
        public static implicit operator double(Fraction Fraction)
        {
            return ((double)Fraction.Numerator / (double)Fraction.Denominator);
        }

        #endregion

        #region ToDecimal

        /// <summary>
        /// Converts the fraction to a decimal
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The fraction as a decimal</returns>
        public static implicit operator decimal(Fraction Fraction)
        {
            return ((decimal)Fraction.Numerator / (decimal)Fraction.Denominator);
        }

        #endregion

        #region ToFloat

        /// <summary>
        /// Converts the fraction to a float
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The fraction as a float</returns>
        public static implicit operator float(Fraction Fraction)
        {
            return ((float)Fraction.Numerator / (float)Fraction.Denominator);
        }

        #endregion

        #region FromDouble

        /// <summary>
        /// Converts the double to a fraction
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The double as a fraction</returns>
        public static implicit operator Fraction(double Fraction)
        {
            return new Fraction(Fraction, 1.0);
        }

        #endregion

        #region FromDecimal

        /// <summary>
        /// Converts the decimal to a fraction
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The decimal as a fraction</returns>
        public static implicit operator Fraction(decimal Fraction)
        {
            return new Fraction(Fraction, 1.0m);
        }

        #endregion

        #region FromFloat

        /// <summary>
        /// Converts the float to a fraction
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The float as a fraction</returns>
        public static implicit operator Fraction(float Fraction)
        {
            return new Fraction(Fraction, 1.0);
        }

        #endregion

        #region FromInt

        /// <summary>
        /// Converts the int to a fraction
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The int as a fraction</returns>
        public static implicit operator Fraction(int Fraction)
        {
            return new Fraction(Fraction, 1);
        }

        #endregion

        #region FromUInt

        /// <summary>
        /// Converts the uint to a fraction
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The uint as a fraction</returns>
        [CLSCompliant(false)]
        public static implicit operator Fraction(uint Fraction)
        {
            return new Fraction((int)Fraction, 1);
        }

        #endregion

        #region ToString

        /// <summary>
        /// Converts the fraction to a string
        /// </summary>
        /// <param name="Fraction">Fraction</param>
        /// <returns>The fraction as a string</returns>
        public static implicit operator string(Fraction Fraction)
        {
            return Fraction.ToString();
        }

        #endregion

        #region Multiplication

        /// <summary>
        /// Multiplication
        /// </summary>
        /// <param name="First">First fraction</param>
        /// <param name="Second">Second fraction</param>
        /// <returns>The resulting fraction</returns>
        public static Fraction operator *(Fraction First, Fraction Second)
        {
            Fraction Result = new Fraction(First.Numerator * Second.Numerator, First.Denominator * Second.Denominator);
            Result.Reduce();
            return Result;
        }

        #endregion

        #region Addition

        /// <summary>
        /// Addition
        /// </summary>
        /// <param name="First">First fraction</param>
        /// <param name="Second">Second fraction</param>
        /// <returns>The added fraction</returns>
        public static Fraction operator +(Fraction First, Fraction Second)
        {
            Fraction Value1 = new Fraction(First.Numerator * (int)Second.Denominator, First.Denominator * Second.Denominator);
            Fraction Value2 = new Fraction(Second.Numerator * (int)First.Denominator, Second.Denominator * First.Denominator);
            Fraction Result = new Fraction(Value1.Numerator + Value2.Numerator, Value1.Denominator);
            Result.Reduce();
            return Result;
        }

        #endregion

        #region Subtraction

        /// <summary>
        /// Subtraction
        /// </summary>
        /// <param name="First">First fraction</param>
        /// <param name="Second">Second fraction</param>
        /// <returns>The subtracted fraction</returns>
        public static Fraction operator -(Fraction First, Fraction Second)
        {
            Fraction Value1 = new Fraction(First.Numerator * (int)Second.Denominator, First.Denominator * Second.Denominator);
            Fraction Value2 = new Fraction(Second.Numerator * (int)First.Denominator, Second.Denominator * First.Denominator);
            Fraction Result = new Fraction(Value1.Numerator - Value2.Numerator, Value1.Denominator);
            Result.Reduce();
            return Result;
        }

        #endregion

        #region Division

        /// <summary>
        /// Division
        /// </summary>
        /// <param name="First">First item</param>
        /// <param name="Second">Second item</param>
        /// <returns>The divided fraction</returns>
        public static Fraction operator /(Fraction First, Fraction Second)
        {
            return First * Second.Inverse();
        }

        #endregion

        #region Negation

        /// <summary>
        /// Negation of the fraction
        /// </summary>
        /// <param name="First">Fraction to negate</param>
        /// <returns>The negated fraction</returns>
        public static Fraction operator -(Fraction First)
        {
            return new Fraction(-First.Numerator, First.Denominator);
        }

        #endregion

        #endregion
    }
}