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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.DataTypes;
using Utilities.DataTypes.ExtensionMethods;
#endregion

namespace Utilities.AI
{
    /// <summary>
    /// Anagram finder
    /// </summary>
    public class Anagram
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DictionaryOfWords">Dictionary of words to use to find anagrams</param>
        public Anagram(System.Collections.Generic.List<string> DictionaryOfWords)
        {
            DictionaryOfWords.ThrowIfNull("DictionaryOfWords");
            this.InitialDictionary = DictionaryOfWords;
            this.DictionaryOfAnagrams = new ListMapping<string, string>();
            FindAnagrams();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Dictionary of words used to find anagrams
        /// </summary>
        public virtual System.Collections.Generic.List<string> InitialDictionary { get; set; }

        /// <summary>
        /// Dictionary of anagram equivalents found in the original dictionary
        /// </summary>
        public virtual ListMapping<string, string> DictionaryOfAnagrams { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Finds the initial set of anagrams
        /// </summary>
        protected virtual void FindAnagrams()
        {
            InitialDictionary.ForEach(x => DictionaryOfAnagrams.Add(GetAnagramKey(x), x));
        }

        /// <summary>
        /// Gets the anagram key associated with the word
        /// </summary>
        /// <param name="Word">Word to get the anagram key for</param>
        /// <returns>The anagram key</returns>
        protected virtual string GetAnagramKey(string Word)
        {
            return new string(Word.OrderBy(x => x).ToArray());
        }

        /// <summary>
        /// Returns the list of equivalent anagrams
        /// </summary>
        /// <param name="Word">Word to check</param>
        /// <returns>A list of words that are anagrams of the word entered</returns>
        public virtual System.Collections.Generic.List<string> FindAnagrams(string Word)
        {
            System.Collections.Generic.List<string> ReturnValues = new System.Collections.Generic.List<string>();
            ReturnValues.AddRange(DictionaryOfAnagrams[GetAnagramKey(Word)]);
            return ReturnValues;
        }

        #endregion
    }
}