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

using Utilities.Random.BaseClasses;
using Utilities.Random.Interfaces;
using Utilities.Random.NameGenerators;
using Utilities.Random.StringGenerators;

namespace Utilities.Random.ContactInfoGenerators
{
    /// <summary>
    /// Generates a random street address
    /// </summary>
    public class StreetAddressGenerator : GeneratorAttributeBase, IGenerator<string>
    {
        private string[] AddressFormats = { "#####", "####", "###" };

        private string[] SecondLineAddressFormat = { "Apt. #", "Apt. ##", "Apt. ###", "Apt. @", "Apt. @#", "Suite ###" };

        private string[] StreetSuffix = { "Avenue", "Bypass", "Center", "Circle", "Corner", "Court", "Cove", "Creek", "Crossing",
                                            "Drive", "Estates", "Expressway", "Freeway", "Highway", "Junction", "Lane", "Loop",
                                            "Park", "Parkway", "Pass", "Plaza", "Road", "Route", "Street", "Turnpike" };

        /// <summary>
        /// Constructor
        /// </summary>
        public StreetAddressGenerator()
            : base("", "")
        {
        }

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public string Next(System.Random Rand)
        {
            return new PatternGenerator(Rand.Next(AddressFormats)).Next(Rand) + " "
                + new LastNameGenerator().Next(Rand) + " " + Rand.Next(StreetSuffix)
                + (Rand.Next<bool>() ? ", " + new PatternGenerator(Rand.Next(SecondLineAddressFormat)).Next(Rand) : "");
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