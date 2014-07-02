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

using System.Drawing;
using Utilities.DataTypes;
using Utilities.Random.BaseClasses;
using Utilities.Random.Interfaces;

namespace Utilities.Random.DefaultClasses
{
    /// <summary>
    /// Randomly generates Colors
    /// </summary>
    public class ColorGenerator : GeneratorAttributeBase, IGenerator<Color>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ColorGenerator()
            : base(Color.Black, Color.White)
        {
        }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public Color Next(System.Random Rand)
        {
            return Next(Rand, Color.Black, Color.White);
        }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <param name="Min">Minimum value (inclusive)</param>
        /// <param name="Max">Maximum value (inclusive)</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public Color Next(System.Random Rand, Color Min, Color Max)
        {
            return Color.FromArgb(Rand.Next(Min.A.Min((byte)(Max.A + 1)), Min.A.Max((byte)(Max.A + 1))),
                Rand.Next(Min.R.Min((byte)(Max.R + 1)), Min.R.Max((byte)(Max.R + 1))),
                Rand.Next(Min.G.Min((byte)(Max.G + 1)), Min.G.Max((byte)(Max.G + 1))),
                Rand.Next(Min.B.Min((byte)(Max.B + 1)), Min.B.Max((byte)(Max.B + 1))));
        }

        /// <summary>
        /// Generates next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            if ((Color)Min != default(Color) || (Color)Max != default(Color))
                return Next(Rand, (Color)Min, (Color)Max);
            return Next(Rand);
        }
    }
}