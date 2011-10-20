/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Utilities.DataTypes.Formatters;
using System.Collections;
#endregion

namespace Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// MatchCollection extensions
    /// </summary>
    public static class MatchCollectionExtensions
    {
        #region Functions

        #region Where

        /// <summary>
        /// Gets a list of items that satisfy the predicate from the collection
        /// </summary>
        /// <param name="Collection">Collection to search through</param>
        /// <param name="Predicate">Predicate that the items must satisfy</param>
        /// <returns>The matches that satisfy the predicate</returns>
        public static IEnumerable<Match> Where(this MatchCollection Collection, Predicate<Match> Predicate)
        {
            if (Collection.IsNull())
                return null;
            Predicate.ThrowIfNull("Predicate");
            List<Match> Matches = new List<Match>();
            foreach (Match Item in Collection)
                if (Predicate(Item))
                    Matches.Add(Item);
            return Matches;
        }

        #endregion

        #endregion
    }
}
