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
using Utilities.Random.StringGenerators;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Random.Interfaces;
using Utilities.DataTypes.ExtensionMethods;
using Utilities.Random.StringGenerators;
using Utilities.Random.ExtensionMethods;
using Utilities.Random.NameGenerators;
using Utilities.Random.BaseClasses;
#endregion

namespace Utilities.Random.ContactInfoGenerators
{
    /// <summary>
    /// Generates a random state
    /// </summary>
    public class StateGenerator : GeneratorAttributeBase, IGenerator<string>
    {
        #region Functions

        /// <summary>
        /// Generates a random value of the specified type
        /// </summary>
        /// <param name="Rand">Random number generator that it can use</param>
        /// <returns>A randomly generated object of the specified type</returns>
        public string Next(System.Random Rand)
        {
            return Rand.Next(StatesAndDistricts);
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

        #endregion

        #region Private Variables

        private string[] StatesAndDistricts = { "Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado",
                                                  "Connecticut", "Delaware", "Florida", "Georgia", "Hawaii", "Idaho",
                                                  "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky", "Louisiana", "Maine",
                                                  "Maryland", "Massachusetts", "Michigan", "Minnesota", "Mississippi",
                                                  "Missouri", "Montana", "Nebraska", "Nevada", "New Hampshire", "New Jersey",
                                                  "New Mexico", "New York", "North Carolina", "North Dakota", "Ohio", "Oklahoma",
                                                  "Oregon", "Pennsylvania", "Rhode Island", "South Carolina", "South Dakota",
                                                  "Tennessee", "Texas", "Utah", "Vermont", "Virginia", "Washington", "West Virginia",
                                                  "Wisconsin", "Wyoming", "District of Columbia" };

        #endregion
    }
}
