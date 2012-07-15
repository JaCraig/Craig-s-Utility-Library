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
using Utilities.Random.BaseClasses;
using Utilities.Random.Interfaces;
#endregion

namespace Utilities.Random.NameGenerators
{
    /// <summary>
    /// Female name generator
    /// </summary>
    public class FemaleNameGenerator : GeneratorAttributeBase, IGenerator<string>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Prefix">Should a prefix be generated</param>
        /// <param name="MiddleName">Should a middle name be generated</param>
        /// <param name="LastName">Should a last name be generated</param>
        /// <param name="Suffix">Should a suffix be generated</param>
        public FemaleNameGenerator(bool Prefix = false, bool MiddleName = false, bool LastName = true, bool Suffix = false)
            : base("", "")
        {
            this.Prefix = Prefix;
            this.MiddleName = MiddleName;
            this.Suffix = Suffix;
            this.LastName = LastName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Should a prefix be generated?
        /// </summary>
        public virtual bool Prefix { get; protected set; }

        /// <summary>
        /// Should a middle name be generated?
        /// </summary>
        public virtual bool MiddleName { get; protected set; }

        /// <summary>
        /// Should a suffix be generated?
        /// </summary>
        public virtual bool Suffix { get; protected set; }

        /// <summary>
        /// Should a last name be generated?
        /// </summary>
        public virtual bool LastName { get; protected set; }

        #endregion

        #region Functions

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public virtual string Next(System.Random Rand)
        {
            return (Prefix ? new FemaleNamePrefixGenerator().Next(Rand) + " " : "")
                + new FemaleFirstNameGenerator().Next(Rand)
                + (MiddleName ? " " + new FemaleFirstNameGenerator().Next(Rand) : "")
                + (LastName ? " " + new LastNameGenerator().Next(Rand) : "")
                + (Suffix ? " " + new NameSuffixGenerator().Next(Rand) : "");
        }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <param name="Min">Minimum value (inclusive)</param>
        /// <param name="Max">Maximum value (inclusive)</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public virtual string Next(System.Random Rand, string Min, string Max)
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