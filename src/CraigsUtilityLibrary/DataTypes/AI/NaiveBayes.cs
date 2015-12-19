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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Utilities.DataTypes.AI
{
    /// <summary>
    /// Naive bayes classifier
    /// </summary>
    /// <typeparam name="T">The type of the individual tokens</typeparam>
    public class NaiveBayes<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aTokenWeight">Weight of each token in set A</param>
        /// <param name="bTokenWeight">Weight of each token in set B</param>
        /// <param name="maxInterestingTokenCount">
        /// After sorting, this is the maximum number of tokens that are picked to figure out the
        /// final probability
        /// </param>
        /// <param name="maxTokenProbability">Maximum token probability</param>
        /// <param name="MinTokenProbability">Minimum token probability</param>
        /// <param name="minCountForInclusion">
        /// Minimum number of times a token needs to be present for it to be included
        /// </param>
        public NaiveBayes(int aTokenWeight = 1, int bTokenWeight = 1,
            double minTokenProbability = 0.01, double maxTokenProbability = 0.999,
            int maxInterestingTokenCount = int.MaxValue,
            int minCountForInclusion = 1)
        {
            SetA = new Bag<T>();
            SetB = new Bag<T>();
            Probabilities = new ConcurrentDictionary<T, double>();
            Total = 0;
            TotalA = 0;
            TotalB = 0;
            ATokenWeight = aTokenWeight;
            BTokenWeight = bTokenWeight;
            MinCountForInclusion = minCountForInclusion;
            MinTokenProbability = MinTokenProbability;
            MaxTokenProbability = maxTokenProbability;
            MaxInterestingTokenCount = maxInterestingTokenCount;
        }

        /// <summary>
        /// Weight to give to the probabilities in set A
        /// </summary>
        public int ATokenWeight { get; set; }

        /// <summary>
        /// Weight to give the probabilities in set B
        /// </summary>
        public int BTokenWeight { get; set; }

        /// <summary>
        /// After sorting, this is the maximum number of tokens that are picked to figure out the
        /// final probability
        /// </summary>
        public int MaxInterestingTokenCount { get; set; }

        /// <summary>
        /// Maximum token probability (if greater than this amount, it becomes this amount)
        /// </summary>
        public double MaxTokenProbability { get; set; }

        /// <summary>
        /// Minimum count that an item needs to be found to be included in final probability
        /// </summary>
        public int MinCountForInclusion { get; set; }

        /// <summary>
        /// Minimum token probability (if less than this amount, it becomes this amount)
        /// </summary>
        public double MinTokenProbability { get; set; }

        /// <summary>
        /// Set A
        /// </summary>
        public Bag<T> SetA { get; private set; }

        /// <summary>
        /// Set B
        /// </summary>
        public Bag<T> SetB { get; private set; }

        /// <summary>
        /// Dictionary containing probabilities
        /// </summary>
        protected ConcurrentDictionary<T, double> Probabilities { get; private set; }

        /// <summary>
        /// Total number of tokens
        /// </summary>
        protected double Total { get; set; }

        /// <summary>
        /// Total number of tokens in set A
        /// </summary>
        protected double TotalA { get; set; }

        /// <summary>
        /// Total number of tokens in set B
        /// </summary>
        protected double TotalB { get; set; }

        /// <summary>
        /// Calculates the probability of the list of tokens being in set A
        /// </summary>
        /// <param name="items">List of items</param>
        /// <returns>The probability that the tokens are from set A</returns>
        public virtual double CalculateProbabilityOfTokens(IEnumerable<T> items)
        {
            if (items == null || Probabilities == null)
                return double.MinValue;
            var SortedProbabilities = new SortedList<string, double>();
            int x = 0;
            foreach (T Item in items)
            {
                double TokenProbability = 0.5;
                if (Probabilities.ContainsKey(Item))
                    TokenProbability = Probabilities[Item];
                string Difference = ((0.5 - Math.Abs(0.5 - TokenProbability))).ToString(".0000000", CultureInfo.InvariantCulture) + Item + x;
                SortedProbabilities.Add(Difference, TokenProbability);
                ++x;
            }
            double TotalProbability = 1;
            double NegativeTotalProbability = 1;
            int Count = 0;
            int MaxCount = SortedProbabilities.Count.Min(MaxInterestingTokenCount);
            foreach (string Probability in SortedProbabilities.Keys)
            {
                double TokenProbability = SortedProbabilities[Probability];
                TotalProbability *= TokenProbability;
                NegativeTotalProbability *= (1 - TokenProbability);
                ++Count;
                if (Count >= MaxCount)
                    break;
            }
            return TotalProbability / (TotalProbability + NegativeTotalProbability);
        }

        /// <summary>
        /// Loads a set of tokens
        /// </summary>
        /// <param name="setATokens">Set A</param>
        /// <param name="setBTokens">Set B</param>
        public virtual void LoadTokens(IEnumerable<T> setATokens, IEnumerable<T> setBTokens)
        {
            setATokens = setATokens ?? new List<T>();
            setBTokens = setBTokens ?? new List<T>();
            SetA = SetA.Check(() => new Bag<T>());
            SetB = SetB.Check(() => new Bag<T>());
            SetA.Add(setATokens);
            SetB.Add(setBTokens);
            TotalA = SetA.Sum(x => SetA[x]);
            TotalB = SetB.Sum(x => SetB[x]);
            Total = TotalA + TotalB;
            Probabilities = new ConcurrentDictionary<T, double>();
            Parallel.ForEach(SetA, Token =>
            {
                Probabilities.AddOrUpdate(Token, x => CalculateProbabilityOfToken(x), (x, y) => CalculateProbabilityOfToken(x));
            });
            Parallel.ForEach(SetB, Token =>
            {
                if (!Probabilities.ContainsKey(Token))
                    Probabilities.AddOrUpdate(Token, x => CalculateProbabilityOfToken(x), (x, y) => y);
            });
        }

        /// <summary>
        /// Calculates a single items probability of being in set A
        /// </summary>
        /// <param name="item">Item to calculate</param>
        /// <returns>The probability that the token is from set A</returns>
        protected virtual double CalculateProbabilityOfToken(T item)
        {
            if (SetA == null || SetB == null)
                return double.MinValue;
            double Probability = 0;
            int ACount = SetA.Contains(item) ? SetA[item] * ATokenWeight : 0;
            int BCount = SetB.Contains(item) ? SetB[item] * BTokenWeight : 0;
            if (ACount + BCount >= MinCountForInclusion)
            {
                double AProbability = ((double)ACount / (double)TotalA).Min(1);
                double BProbability = ((double)BCount / (double)TotalB).Min(1);
                Probability = MinTokenProbability.Max(MaxTokenProbability.Min(AProbability / (AProbability + BProbability)));
            }
            return Probability;
        }
    }
}