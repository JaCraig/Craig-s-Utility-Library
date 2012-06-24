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
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using Utilities.DataTypes;
using Utilities.DataTypes.ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Utilities.Random.ExtensionMethods;
#endregion

namespace Utilities.Random
{
    /// <summary>
    /// Utility class for handling random
    /// information.
    /// </summary>
    public class Random : System.Random
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Random()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Seed">Seed value</param>
        public Random(int Seed)
            : base(Seed)
        {
        }

        #endregion

        #region Public Functions

        //NextAddress,NextCity,NextState,NextZipCode
        //NextEmailAddress

        #endregion

        #region Private Variables

        private static Random GlobalSeed = new Random();
        [ThreadStatic]
        private static Random Local;

        private string[] CityPrefix = { "North", "South", "East", "West", "New", "Lake", "Old", "Port", "Fort","Mount" };

        private string[] CitySuffix = { "Cove", "Manor", "City", "Park", "Springs", "Canyon", "Fork", "Center", "Mill",
                                          "Beach","Glen", "Valley","Heights", "Harbor","Grove","Haven","Island", "Pass",
                                          "Hills", "Creek", "Crest", "Dale", "Falls","Flats","Gardens","Landing","Meadows",
                                          "Pines" };

        private string[] CityEndings = { "deen", "town", "ville", "berg", "view", "bury", "ton", "land", "mouth", "haven",
                                           "shire", "don", "creek", "worth", "son", "mont", "wood", "dale","cliff","bridge" };

        private string[] StatesAndDistricts = { "Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado",
                                                  "Connecticut", "Delaware", "Florida", "Georgia", "Hawaii", "Idaho",
                                                  "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky", "Louisiana", "Maine",
                                                  "Maryland", "Massachusetts", "Michigan", "Minnesota", "Mississippi",
                                                  "Missouri", "Montana", "Nebraska", "Nevada", "New Hampshire", "New Jersey",
                                                  "New Mexico", "New York", "North Carolina", "North Dakota", "Ohio", "Oklahoma",
                                                  "Oregon", "Pennsylvania", "Rhode Island", "South Carolina", "South Dakota",
                                                  "Tennessee", "Texas", "Utah", "Vermont", "Virginia", "Washington", "West Virginia",
                                                  "Wisconsin", "Wyoming", "District of Columbia" };

        private string[] StateAndDistrictAbbreviations = { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "HI",
                                                             "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI",
                                                             "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC",
                                                             "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT",
                                                             "VT", "VA", "WA", "WV", "WI", "WY", "DC" };

        private string[] ZipCodeFormats = { "#####", "#####-####" };

        private string[] AddressFormats = { "#####", "####", "###" };

        private string[] SecondLineAddressFormat = { "Apt. #", "Apt. ##", "Apt. ###", "Apt. @", "Apt. @#", "Suite ###" };

        private string[] StreetSuffix = { "Avenue", "Bypass", "Center", "Circle", "Corner", "Court", "Cove", "Creek", "Crossing",
                                            "Drive", "Estates", "Expressway", "Freeway", "Highway", "Junction", "Lane", "Loop",
                                            "Park", "Parkway", "Pass", "Plaza", "Road", "Route", "Street", "Turnpike" };
        
        #endregion

        #region Static Functions

        #region ThreadSafeNext

        public static int ThreadSafeNext(int Min = int.MinValue, int Max = int.MaxValue)
        {
            if (Local == null)
            {
                int Seed;
                lock (GlobalSeed) 
                    Seed = GlobalSeed.Next();
                Local = new Random(Seed);
            }
            return Local.Next(Min, Max);
        }

        #endregion

        #endregion
    }
}