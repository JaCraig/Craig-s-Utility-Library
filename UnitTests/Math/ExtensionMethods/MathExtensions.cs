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

using System.Linq;
using Utilities.Math.ExtensionMethods;
using Xunit;

namespace UnitTests.Math.ExtensionMethods
{
    public class MathExtensions
    {
        
        [Fact]
        public void FactorialTest()
        {
            int Value = 8;
            Assert.Equal(40320, Value.Factorial());
        }

        [Fact]
        public void MedianTest()
        {
            Assert.Equal(10, new int[] { 9, 11, 10 }.ToList().Median());
        }

        [Fact]
        public void ModeTest()
        {
            Assert.Equal(20, new int[] { 5, 2, 20, 5, 20, 8, 9, 20, 10 }.ToList().Mode());
        }

        [Fact]
        public void PowTest()
        {
            double Value = 4;
            Assert.Equal(256, Value.Pow(4));
        }

        [Fact]
        public void StandardDeviationTest()
        {
            Assert.InRange(new double[] { 5, 4, 2, 4, 7, 9, 1, 2, 0 }.ToList().StandardDeviation(), 2.73, 2.74);
        }

        [Fact]
        public void VarianceTest()
        {
            Assert.InRange(new double[] { 5, 4, 2, 4, 7, 9, 1, 2, 0 }.ToList().Variance(), 7.5, 7.6);
        }

        [Fact]
        public void SqrtTest()
        {
            double Value=4;
            Assert.Equal(2, Value.Sqrt());
        }

        [Fact]
        public void GCD()
        {
            Assert.Equal(3, 9.GreatestCommonDenominator(12));
        }

        [Fact]
        public void Round()
        {
            double Value = 4.1234;
            Assert.Equal(4.12, Value.Round());
        }
    }
}