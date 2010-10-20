/*
Copyright (c) 2010 <a href="http://www.gutgames.com">James Craig</a>

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
        }

        /// <summary>
        /// Calculates the probability of the list of tokens being in set A
        /// </summary>
        /// <param name="Items">List of items</param>
        /// <returns>The probability that the tokens are from set A</returns>
        public double CalculateProbabilityOfTokens(System.Collections.Generic.List<T> Items)
        {
            double TotalProbability = 1.0;
            double NegativeTotalProbability = 1.0;
            for (int x = 0; x < Items.Count; ++x)
            {
                double Probability = CalculateProbabilityOfToken(Items[x]);
                TotalProbability *= Probability;
                NegativeTotalProbability *= (1 - Probability);
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
            if (Total == 0.0 || TotalA == 0.0)
                return 0.0;
            double Percent = 0.0;
            if (SetA.Contains(Item) && SetB.Contains(Item))
            {
                Percent = (double)SetA[Item] / (double)(SetA[Item] + SetB[Item]);
            }
            else if (SetA.Contains(Item))
            {
                Percent = 1.0;
            }
            double PriorPercent = (double)TotalA / (double)Total;
            return Percent * PriorPercent;
        }

        #endregion
    }
}