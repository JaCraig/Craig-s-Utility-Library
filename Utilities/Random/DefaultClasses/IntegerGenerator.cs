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
using Utilities.DataTypes;
using Utilities.Random.BaseClasses;
using Utilities.Random.Interfaces;

namespace Utilities.Random.DefaultClasses
{
    /// <summary>
    /// Byte generator
    /// </summary>
    public class ByteGenerator : GeneratorAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value</param>
        /// <param name="Max">Max value</param>
        public ByteGenerator(byte Min, byte Max)
            : base(Min, Max)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ByteGenerator()
            : base(0, byte.MaxValue)
        {
        }

        /// <summary>
        /// Creates the next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return new IntegerGenerator<byte>().Next(Rand, (byte)Min, (byte)Max);
        }
    }

    /// <summary>
    /// Char generator
    /// </summary>
    public class CharGenerator : GeneratorAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value</param>
        /// <param name="Max">Max value</param>
        public CharGenerator(char Min, char Max)
            : base(Min, Max)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CharGenerator()
            : base(0, char.MaxValue)
        {
        }

        /// <summary>
        /// Creates the next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return new IntegerGenerator<char>().Next(Rand, (char)Min, (char)Max);
        }
    }

    /// <summary>
    /// Randomly generates ints
    /// </summary>
    public class IntegerGenerator<T> : IGenerator<T>
    {
        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public T Next(System.Random Rand)
        {
            return Rand.Next().To(default(T));
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
            return Rand.Next(Min.To(0), Max.To(0)).To(default(T));
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
    /// Int generator
    /// </summary>
    public class IntGenerator : GeneratorAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value</param>
        /// <param name="Max">Max value</param>
        public IntGenerator(int Min, int Max)
            : base(Min, Max)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public IntGenerator()
            : base(0, int.MaxValue)
        {
        }

        /// <summary>
        /// Creates the next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return new IntegerGenerator<int>().Next(Rand, (int)Min, (int)Max);
        }
    }

    /// <summary>
    /// Long generator
    /// </summary>
    public class LongGenerator : GeneratorAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value</param>
        /// <param name="Max">Max value</param>
        public LongGenerator(long Min, long Max)
            : base(Min, Max)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LongGenerator()
            : base(0, long.MaxValue)
        {
        }

        /// <summary>
        /// Creates the next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return new IntegerGenerator<long>().Next(Rand, (long)Min, (long)Max);
        }
    }

    /// <summary>
    /// sbyte generator
    /// </summary>
    [CLSCompliant(false)]
    public class SByteGenerator : GeneratorAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value</param>
        /// <param name="Max">Max value</param>
        public SByteGenerator(sbyte Min, sbyte Max)
            : base(Min, Max)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SByteGenerator()
            : base(0, sbyte.MaxValue)
        {
        }

        /// <summary>
        /// Creates the next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return new IntegerGenerator<sbyte>().Next(Rand, (sbyte)Min, (sbyte)Max);
        }
    }

    /// <summary>
    /// Short generator
    /// </summary>
    public class ShortGenerator : GeneratorAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value</param>
        /// <param name="Max">Max value</param>
        public ShortGenerator(short Min, short Max)
            : base(Min, Max)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShortGenerator()
            : base(0, short.MaxValue)
        {
        }

        /// <summary>
        /// Creates the next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return new IntegerGenerator<short>().Next(Rand, (short)Min, (short)Max);
        }
    }

    /// <summary>
    /// uint generator
    /// </summary>
    [CLSCompliant(false)]
    public class UIntGenerator : GeneratorAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value</param>
        /// <param name="Max">Max value</param>
        public UIntGenerator(uint Min, uint Max)
            : base(Min, Max)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UIntGenerator()
            : base(0, uint.MaxValue)
        {
        }

        /// <summary>
        /// Creates the next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return new IntegerGenerator<uint>().Next(Rand, (uint)Min, (uint)Max);
        }
    }

    /// <summary>
    /// ulong generator
    /// </summary>
    [CLSCompliant(false)]
    public class ULongGenerator : GeneratorAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value</param>
        /// <param name="Max">Max value</param>
        public ULongGenerator(ulong Min, ulong Max)
            : base(Min, Max)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ULongGenerator()
            : base(0, ulong.MaxValue)
        {
        }

        /// <summary>
        /// Creates the next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return new IntegerGenerator<ulong>().Next(Rand, (ulong)Min, (ulong)Max);
        }
    }

    /// <summary>
    /// ushort generator
    /// </summary>
    [CLSCompliant(false)]
    public class UShortGenerator : GeneratorAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Min">Min value</param>
        /// <param name="Max">Max value</param>
        public UShortGenerator(ushort Min, ushort Max)
            : base(Min, Max)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UShortGenerator()
            : base(0, ushort.MaxValue)
        {
        }

        /// <summary>
        /// Creates the next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return new IntegerGenerator<ushort>().Next(Rand, (ushort)Min, (ushort)Max);
        }
    }
}