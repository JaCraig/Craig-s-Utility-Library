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
using Utilities.DataTypes;
using System.Collections.Generic;
using Utilities.Math;
#endregion

namespace Utilities.Classifier.NaiveBayes
{
    /// <summary>
    /// Naive bayes classifier
    /// </summary>
    /// <typeparam name="T">The type of the individual tokens</typeparam>
    public class NaiveBayes<T>
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public NaiveBayes()
        {
            SetA = new Bag<T>();
            SetB = new Bag<T>();
            Probabilities = new Dictionary<T, double>();
            Total = 0;
            TotalA = 0;
            TotalB = 0;
            ATokenWeight = 1;
            BTokenWeight = 1;
            MinCountForInclusion = 1;
            MinTokenProbability = 0.01;
            MaxTokenProbability = 0.999;
            MaxInterestingTokenCount = int.MaxValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Set A
        /// </summary>
        public Bag<T> SetA { get; set; }

        /// <summary>
        /// Set B
        /// </summary>
        public Bag<T> SetB { get; set; }

        private double Total { get; set; }
        private double TotalA { get; set; }
        private double TotalB { get; set; }
        private Dictionary<T, double> Probabilities { get; set; }

        /// <summary>
        /// Weight to give to the probabilities in set A
        /// </summary>
        public int ATokenWeight { get; set; }

        /// <summary>
        /// Weight to give the probabilities in set B
        /// </summary>
        public int BTokenWeight { get; set; }

        /// <summary>
        /// Minimum count that an item needs to be found to be included in final probability
        /// </summary>
        public int MinCountForInclusion { get; set; }

        /// <summary>
        /// Minimum token probability (if less than this amount, it becomes this amount)
        /// </summary>
        public double MinTokenProbability { get; set; }

        /// <summary>
        /// Maximum token probability (if greater than this amount, it becomes this amount)
        /// </summary>
        public double MaxTokenProbability { get; set; }

        /// <summary>
        /// After sorting, this is the maximum number of tokens that are picked to figure out the final probability
        /// </summary>
        public int MaxInterestingTokenCount { get; set; }

        #endregion

        #region Public Functions

        /// <summary>
        /// Loads a set of tokens
        /// </summary>
        /// <param name="SetATokens">Set A</param>
        /// <param name="SetBTokens">Set B</param>
        public void LoadTokens(System.Collections.Generic.List<T> SetATokens, System.Collections.Generic.List<T> SetBTokens)
        {
            foreach (T TokenA in SetATokens)
            {
                SetA.Add(TokenA);
            }
            foreach (T TokenB in SetBTokens)
            {
                SetB.Add(TokenB);
            }
            TotalA = 0;
            TotalB = 0;
            foreach (T Token in SetA)
            {
                TotalA += SetA[Token];
            }
            foreach (T Token in SetB)
            {
                TotalB += SetB[Token];
            }
            Total = TotalA + TotalB;
            Probabilities = new Dictionary<T, double>();
            foreach (T Token in SetA)
            {
                Probabilities.Add(Token, CalculateProbabilityOfToken(Token));
            }
            foreach (T Token in SetB)
            {
                if (!Probabilities.ContainsKey(Token))
                {
                    Probabilities.Add(Token, CalculateProbabilityOfToken(Token));
                }
            }
        }

        /// <summary>
        /// Calculates the probability of the list of tokens being in set A
        /// </summary>
        /// <param name="Items">List of items</param>
        /// <returns>The probability that the tokens are from set A</returns>
        public double CalculateProbabilityOfTokens(System.Collections.Generic.List<T> Items)
        {
            SortedList<string, double> SortedProbabilities = new SortedList<string, double>();
            for (int x = 0; x < Items.Count; ++x)
            {
                double TokenProbability = 0.5;
                if (Probabilities.ContainsKey(Items[x]))
                {
                    TokenProbability = Probabilities[Items[x]];
                }
                string Difference = ((0.5 - System.Math.Abs(0.5 - TokenProbability))).ToString(".0000000") + Items[x] + x;
                SortedProbabilities.Add(Difference, TokenProbability);
            }
            double TotalProbability = 1;
            double NegativeTotalProbability = 1;
            int Count = 0;
            int MaxCount=MathHelper.Min(SortedProbabilities.Count, MaxInterestingTokenCount);
            foreach(string Probability in SortedProbabilities.Keys)
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

        #endregion

        #region Private Functions

        /// <summary>
        /// Calculates a single items probability of being in set A
        /// </summary>
        /// <param name="Item">Item to calculate</param>
        /// <returns>The probability that the token is from set A</returns>
        private double CalculateProbabilityOfToken(T Item)
        {
            double Probability = 0;
            int ACount = SetA.Contains(Item) ? SetA[Item] * ATokenWeight : 0;
            int BCount = SetB.Contains(Item) ? SetB[Item] * BTokenWeight : 0;
            if (ACount + BCount >= MinCountForInclusion)
            {
                double AProbability=MathHelper.Min(1,(double)ACount/(double)TotalA);
                double BProbability=MathHelper.Min(1,(double)BCount/(double)TotalB);
                Probability = MathHelper.Max(MinTokenProbability, 
                    MathHelper.Min(MaxTokenProbability, AProbability / (AProbability + BProbability)));
            }
            return Probability;
        }

        #endregion
    }
}