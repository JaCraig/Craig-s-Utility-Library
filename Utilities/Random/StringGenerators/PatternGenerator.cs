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

using System.Text;
using Utilities.Random.BaseClasses;
using Utilities.Random.Interfaces;

namespace Utilities.Random.StringGenerators
{
    /// <summary>
    /// Randomly generates strings based on a pattern
    /// </summary>
    public class PatternGenerator : GeneratorAttributeBase, IGenerator<string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Pattern">Pattern to use: # = Number @ = Alpha character</param>
        public PatternGenerator(string Pattern)
            : base("", "")
        {
            this.Pattern = Pattern;
        }

        /// <summary>
        /// Pattern to use
        /// </summary>
        public virtual string Pattern { get; protected set; }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public string Next(System.Random Rand)
        {
            if (string.IsNullOrEmpty(Pattern))
                return "";
            var TempBuilder = new StringBuilder();
            for (int x = 0; x < Pattern.Length; ++x)
            {
                if (Pattern[x] == '#')
                {
                    TempBuilder.Append(Rand.Next(0, 9));
                }
                else if (Pattern[x] == '@')
                {
                    TempBuilder.Append(Rand.Next<string>(new RegexStringGenerator(1, "[a-zA-Z]", 0)));
                }
                else
                {
                    TempBuilder.Append(Pattern[x]);
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
    }
}