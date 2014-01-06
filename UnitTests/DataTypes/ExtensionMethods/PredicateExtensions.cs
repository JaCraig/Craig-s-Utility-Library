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
using Utilities.DataTypes;
using Xunit;

namespace UnitTests.Math.ExtensionMethods
{
    /// <summary>
    /// Predicate extensions
    /// </summary>
    public class PredicateExtensions
    {
        [Fact]
        public void AddToSet()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Even = Even.AddToSet(1, 3, 5);
            Assert.True(Even(1));
            Assert.True(Even(3));
            Assert.True(Even(2));
            Assert.True(Even(10));
            Assert.True(Even(5));
        }

        [Fact]
        public void CartesianProduct()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Predicate<int> Multiple3 = x => x % 3 == 0;
            Func<int, int, bool> CartesianProduct = Even.CartesianProduct(Multiple3);
            Assert.True(CartesianProduct(6, 12));
            Assert.False(CartesianProduct(3, 9));
        }

        [Fact]
        public void Difference()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Predicate<int> Multiple3 = x => x % 3 == 0;
            Predicate<int> Diff = Even.Difference(Multiple3);
            Assert.True(Diff(2));
            Assert.True(Diff(4));
            Assert.False(Diff(6));
        }

        [Fact]
        public void Intersect()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Predicate<int> Multiple3 = x => x % 3 == 0;
            Predicate<int> Inter = Even.Intersect(Multiple3);
            Assert.True(Inter(6));
            Assert.True(Inter(12));
            Assert.False(Inter(2));
            Assert.False(Inter(3));
        }

        [Fact]
        public void RelativeComplement()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Predicate<int> Multiple3 = x => x % 3 == 0;
            Predicate<int> Compliement = Even.RelativeComplement(Multiple3);
            Assert.True(Compliement(2));
            Assert.False(Compliement(6));
        }

        [Fact]
        public void RemoveFromSet()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Even = Even.RemoveFromSet(2, 4, 6);
            Assert.True(Even(10));
            Assert.True(Even(8));
            Assert.False(Even(6));
            Assert.False(Even(4));
            Assert.False(Even(2));
        }

        [Fact]
        public void Union()
        {
            Predicate<int> Even = x => x % 2 == 0;
            Predicate<int> Multiple3 = x => x % 3 == 0;
            Predicate<int> Test = Even.Union(Multiple3);
            Assert.True(Test(2));
            Assert.True(Test(3));
            Assert.True(Test(4));
            Assert.False(Test(5));
        }
    }
}