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

using Utilities.DataTypes;
using Utilities.Random.BaseClasses;
using Utilities.Random.Interfaces;

namespace Utilities.Random.DefaultClasses
{
    /// <summary>
    /// Randomly generates decimals
    /// </summary>
    public class DecimalGenerator<T> : IGenerator<T>
    {
        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public T Next(System.Random Rand)
        {
            return Rand.NextDouble().To(default(T));
        }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <param name="Min">Minimum value (inclusive)</param>
        /// <param name="Max">Maximum value (inclusive)</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public T Next(System.Random Rand, T Min, T Max)
        {
            return (Min.To(default(double)) + ((Max.To(default(double)) - Min.To(default(double))) * Rand.NextDouble())).To(default(T));
        }

        /// <summary>
        /// Randomly generates an object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>A randomly generated object</returns>
        public object NextObj(System.Random Rand)
        {
            return Next(Rand);
        }
    }

    /// <summary>
    /// Decimal generator
    /// </summary>
    public class DecimalGenerator : GeneratorAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value</param>
        /// <param name="Max">Max value</param>
        public DecimalGenerator(decimal Min, decimal Max)
            : base(Min, Max)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DecimalGenerator()
            : base(0m, 1m)
        {
        }

        /// <summary>
        /// Creates the next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return new DecimalGenerator<decimal>().Next(Rand, (decimal)Min, (decimal)Max);
        }
    }

    /// <summary>
    /// Double generator
    /// </summary>
    public class DoubleGenerator : GeneratorAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value</param>
        /// <param name="Max">Max value</param>
        public DoubleGenerator(double Min, double Max)
            : base(Min, Max)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DoubleGenerator()
            : base(0d, 1d)
        {
        }

        /// <summary>
        /// Creates the next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return new DecimalGenerator<double>().Next(Rand, (double)Min, (double)Max);
        }
    }

    /// <summary>
    /// Float generator
    /// </summary>
    public class FloatGenerator : GeneratorAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value</param>
        /// <param name="Max">Max value</param>
        public FloatGenerator(float Min, float Max)
            : base(Min, Max)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FloatGenerator()
            : base(0f, 1f)
        {
        }

        /// <summary>
        /// Creates the next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return new DecimalGenerator<float>().Next(Rand, (float)Min, (float)Max);
        }
    }
}