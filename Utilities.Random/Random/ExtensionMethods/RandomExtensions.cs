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

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using Utilities.DataTypes;
using Utilities.Random.DefaultClasses;
using Utilities.Random.Interfaces;

#endregion Usings

namespace Utilities.Random
{
    /// <summary>
    /// Extension methods for the Random class
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class RandomExtensions
    {
        #region Functions

        #region Next

        /// <summary>
        /// Randomly generates a value of the specified type
        /// </summary>
        /// <typeparam name="T">Type to generate</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static T Next<T>(this System.Random Random, IGenerator<T> Generator = null)
        {
            Contract.Requires<ArgumentNullException>(Random != null, "Random");
            SetupGenerators();
            if (Generator == null)
            {
                if (!Generators.ContainsKey(typeof(T)))
                    throw new ArgumentOutOfRangeException("The type specified, " + typeof(T).Name + ", does not have a default generator.");
                Generator = (IGenerator<T>)Generators[typeof(T)];
            }
            return Generator.Next(Random);
        }

        /// <summary>
        /// Randomly generates a value of the specified type
        /// </summary>
        /// <typeparam name="T">Type to generate</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Max">Maximum value (inclusive)</param>
        /// <param name="Min">Minimum value (inclusive)</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static T Next<T>(this System.Random Random, T Min, T Max, IGenerator<T> Generator = null)
        {
            Contract.Requires<ArgumentNullException>(Random != null, "Random");
            SetupGenerators();
            if (Generator == null)
            {
                if (!Generators.ContainsKey(typeof(T)))
                    throw new ArgumentOutOfRangeException("The type specified, " + typeof(T).Name + ", does not have a default generator.");
                Generator = (IGenerator<T>)Generators[typeof(T)];
            }
            return Generator.Next(Random, Min, Max);
        }

        /// <summary>
        /// Randomly generates a list of values of the specified type
        /// </summary>
        /// <typeparam name="T">Type to the be generated</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Amount">Number of items to generate</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static IEnumerable<T> Next<T>(this System.Random Random, int Amount, IGenerator<T> Generator = null)
        {
            Contract.Requires<ArgumentNullException>(Random != null, "Random");
            SetupGenerators();
            if (Generator == null)
            {
                if (!Generators.ContainsKey(typeof(T)))
                    throw new ArgumentOutOfRangeException("The type specified, " + typeof(T).Name + ", does not have a default generator.");
                Generator = (IGenerator<T>)Generators[typeof(T)];
            }
            return Amount.Times(x => Generator.Next(Random));
        }

        /// <summary>
        /// Randomly generates a list of values of the specified type
        /// </summary>
        /// <typeparam name="T">Type to the be generated</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Amount">Number of items to generate</param>
        /// <param name="Max">Maximum value (inclusive)</param>
        /// <param name="Min">Minimum value (inclusive)</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static IEnumerable<T> Next<T>(this System.Random Random, int Amount, T Min, T Max, IGenerator<T> Generator = null)
        {
            Contract.Requires<ArgumentNullException>(Random != null, "Random");
            SetupGenerators();
            if (Generator == null)
            {
                if (!Generators.ContainsKey(typeof(T)))
                    throw new ArgumentOutOfRangeException("The type specified, " + typeof(T).Name + ", does not have a default generator.");
                Generator = (IGenerator<T>)Generators[typeof(T)];
            }
            return Amount.Times(x => Generator.Next(Random, Min, Max));
        }

        /// <summary>
        /// Picks a random item from the list
        /// </summary>
        /// <typeparam name="T">Type of object in the list</typeparam>
        /// <param name="Random">Random number generator</param>
        /// <param name="List">List to pick from</param>
        /// <returns>Item that is returned</returns>
        public static T Next<T>(this System.Random Random, IEnumerable<T> List)
        {
            Contract.Requires<ArgumentNullException>(Random != null, "Random");
            if (List == null)
                return default(T);
            int x = 0;
            int Position = Random.Next(0, List.Count());
            foreach (T Item in List)
            {
                if (x == Position)
                    return Item;
                ++x;
            }
            return default(T);
        }

        /// <summary>
        /// Randomly generates a value of the specified type
        /// </summary>
        /// <typeparam name="T">Type to generate</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static T NextClass<T>(this System.Random Random, IGenerator<T> Generator = null)
            where T : class,new()
        {
            Contract.Requires<ArgumentNullException>(Random != null, "Random");
            SetupGenerators();
            if (Generator == null)
            {
                Generator = new ClassGenerator<T>();
            }
            return Generator.Next(Random);
        }

        /// <summary>
        /// Randomly generates a list of values of the specified type
        /// </summary>
        /// <typeparam name="T">Type to the be generated</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Amount">Number of items to generate</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static IEnumerable<T> NextClass<T>(this System.Random Random, int Amount, IGenerator<T> Generator = null)
            where T : class,new()
        {
            Contract.Requires<ArgumentNullException>(Random != null, "Random");
            SetupGenerators();
            if (Generator == null)
            {
                Generator = new ClassGenerator<T>();
            }
            return Amount.Times(x => Generator.Next(Random));
        }

        #endregion Next

        #region NextEnum

        /// <summary>
        /// Randomly generates a value of the specified enum type
        /// </summary>
        /// <typeparam name="T">Type to generate</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static T NextEnum<T>(this System.Random Random, IGenerator<T> Generator = null)
        {
            Contract.Requires<ArgumentNullException>(Random != null, "Random");
            SetupGenerators();
            Generator = Generator.Check(new EnumGenerator<T>());
            return Generator.Next(Random);
        }

        /// <summary>
        /// Randomly generates a list of values of the specified enum type
        /// </summary>
        /// <typeparam name="T">Type to the be generated</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="Amount">Number of items to generate</param>
        /// <param name="Generator">
        /// Generator to be used (if not included, default generator is used)
        /// </param>
        /// <returns>The randomly generated value</returns>
        public static IEnumerable<T> NextEnum<T>(this System.Random Random, int Amount, IGenerator<T> Generator = null)
        {
            Contract.Requires<ArgumentNullException>(Random != null, "Random");
            SetupGenerators();
            Generator = Generator.Check(new EnumGenerator<T>());
            return Amount.Times(x => Generator.Next(Random));
        }

        #endregion NextEnum

        #region RegisterGenerator

        /// <summary>
        /// Registers a generator with a type
        /// </summary>
        /// <typeparam name="T">Type to associate with the generator</typeparam>
        /// <param name="Rand">Random number generator</param>
        /// <param name="Generator">Generator to associate with the type</param>
        /// <returns>The random number generator</returns>
        public static System.Random RegisterGenerator<T>(this System.Random Rand, IGenerator Generator)
        {
            return Rand.RegisterGenerator(typeof(T), Generator);
        }

        /// <summary>
        /// Registers a generator with a type
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <param name="Generator">Generator to associate with the type</param>
        /// <param name="Type">Type to associate with the generator</param>
        /// <returns>The random number generator</returns>
        public static System.Random RegisterGenerator(this System.Random Rand, Type Type, IGenerator Generator)
        {
            if (Generators.ContainsKey(Type))
                Generators[Type] = Generator;
            else
                Generators.Add(Type, Generator);
            return Rand;
        }

        #endregion RegisterGenerator

        #region ResetGenerators

        /// <summary>
        /// Resets the generators to the defaults
        /// </summary>
        /// <param name="Random">Random object</param>
        /// <returns>The random object sent in</returns>
        public static System.Random ResetGenerators(this System.Random Random)
        {
            Generators = null;
            SetupGenerators();
            return Random;
        }

        #endregion ResetGenerators

        #region Shuffle

        /// <summary>
        /// Shuffles a list randomly
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="Random">Random object</param>
        /// <param name="List">List of objects to shuffle</param>
        /// <returns>The shuffled list</returns>
        public static IEnumerable<T> Shuffle<T>(this System.Random Random, IEnumerable<T> List)
        {
            if (List == null || List.Count() == 0)
                return List;
            return List.OrderBy(x => Random.Next());
        }

        #endregion Shuffle

        #endregion Functions

        #region Private Functions/Variables

        private static Dictionary<Type, IGenerator> Generators = null;

        private static void SetupGenerators()
        {
            if (Generators != null)
                return;
            Generators = new Dictionary<Type, IGenerator>();
            Generators.Add(typeof(bool), new BoolGenerator());
            Generators.Add(typeof(decimal), new DecimalGenerator<decimal>());
            Generators.Add(typeof(double), new DecimalGenerator<double>());
            Generators.Add(typeof(float), new DecimalGenerator<float>());
            Generators.Add(typeof(byte), new IntegerGenerator<byte>());
            Generators.Add(typeof(char), new IntegerGenerator<char>());
            Generators.Add(typeof(int), new IntegerGenerator<int>());
            Generators.Add(typeof(long), new IntegerGenerator<long>());
            Generators.Add(typeof(sbyte), new IntegerGenerator<sbyte>());
            Generators.Add(typeof(short), new IntegerGenerator<short>());
            Generators.Add(typeof(uint), new IntegerGenerator<uint>());
            Generators.Add(typeof(ulong), new IntegerGenerator<ulong>());
            Generators.Add(typeof(ushort), new IntegerGenerator<ushort>());
            Generators.Add(typeof(DateTime), new DateTimeGenerator());
            Generators.Add(typeof(TimeSpan), new TimeSpanGenerator());
            Generators.Add(typeof(Color), new ColorGenerator());
            Generators.Add(typeof(string), new StringGenerator());
        }

        #endregion Private Functions/Variables
    }
}