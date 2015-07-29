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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace Utilities.DataTypes
{
    /// <summary>
    /// MatchCollection extensions
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class MatchCollectionExtensions
    {
        /// <summary>
        /// Gets a list of items that satisfy the predicate from the collection
        /// </summary>
        /// <param name="Collection">Collection to search through</param>
        /// <param name="Predicate">Predicate that the items must satisfy</param>
        /// <returns>The matches that satisfy the predicate</returns>
        public static IEnumerable<Match> Where(this MatchCollection Collection, Predicate<Match> Predicate)
        {
            Contract.Requires<ArgumentNullException>(Predicate != null, "Predicate");
            if (Collection == null)
                return null;
            var Matches = new List<Match>();
            foreach (Match Item in Collection)
                if (Predicate(Item))
                    Matches.Add(Item);
            return Matches;
        }
    }
}