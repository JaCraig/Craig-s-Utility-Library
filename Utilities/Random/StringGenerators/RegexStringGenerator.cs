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
using System.Text.RegularExpressions;
using Utilities.Random.BaseClasses;
using Utilities.Random.Interfaces;
#endregion

namespace Utilities.Random.StringGenerators
{
    /// <summary>
    /// Randomly generates strings based on a Regex
    /// </summary>
    public class RegexStringGenerator : GeneratorAttributeBase, IGenerator<string>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Length">Length of the string to generate</param>
        /// <param name="AllowedCharacters">Characters that are allowed</param>
        /// <param name="NumberOfNonAlphaNumericsAllowed">Number of non alphanumeric characters to allow</param>
        public RegexStringGenerator(int Length, string AllowedCharacters = ".", int NumberOfNonAlphaNumericsAllowed = int.MaxValue)
            : base("", "")
        {
            this.Length = Length;
            this.AllowedCharacters = AllowedCharacters;
            this.NumberOfNonAlphaNumericsAllowed = NumberOfNonAlphaNumericsAllowed;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Length to generate
        /// </summary>
        public virtual int Length { get; protected set; }

        /// <summary>
        /// Characters allowed
        /// </summary>
        public virtual string AllowedCharacters { get; protected set; }

        /// <summary>
        /// Number of non alpha numeric characters allowed
        /// </summary>
        public virtual int NumberOfNonAlphaNumericsAllowed { get; protected set; }

        #endregion

        #region Functions

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public string Next(System.Random Rand)
        {
            if (Length < 1)
                return "";
            StringBuilder TempBuilder = new StringBuilder();
            Regex Comparer = new Regex(AllowedCharacters);
            Regex AlphaNumbericComparer = new Regex("[0-9a-zA-Z]");
            int Counter = 0;
            while (TempBuilder.Length < Length)
            {
                string TempValue = new string(Convert.ToChar(Convert.ToInt32(System.Math.Floor(94 * Rand.NextDouble() + 32))), 1);
                if (Comparer.IsMatch(TempValue))
                {
                    if (!AlphaNumbericComparer.IsMatch(TempValue) && NumberOfNonAlphaNumericsAllowed > Counter)
                    {
                        TempBuilder.Append(TempValue);
                        ++Counter;
                    }
                    else if (AlphaNumbericComparer.IsMatch(TempValue))
                    {
                        TempBuilder.Append(TempValue);
                    }
                }
            }
            return TempBuilder.ToString();
        }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <param name="Min">Minimum value (inclusive)</param>
        /// <param name="Max">Maximum value (inclusive)</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public string Next(System.Random Rand, string Min, string Max)
        {
            return Next(Rand);
        }

        /// <summary>
        /// Generates next object
        /// </summary>
        /// <param name="Rand">Random number generator</param>
        /// <returns>The next object</returns>
        public override object NextObj(System.Random Rand)
        {
            return Next(Rand);
        }

        #endregion
    }
}