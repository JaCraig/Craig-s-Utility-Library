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

using Utilities.DataTypes.Comparison;
using Xunit;

namespace Utilities.Tests.DataTypes.Comparison
{
    public class GenericEqualityComparer
    {
        [Fact]
        public void Compare()
        {
            var Comparer = new GenericEqualityComparer<string>();
            Assert.True(Comparer.Equals("A", "A"));
            Assert.False(Comparer.Equals("A", "B"));
            Assert.False(Comparer.Equals("B", "A"));
        }

        [Fact]
        public void CompareNullNonValueType()
        {
            var Comparer = new GenericEqualityComparer<string>();
            Assert.True(Comparer.Equals(null, null));
            Assert.False(Comparer.Equals(null, "B"));
            Assert.False(Comparer.Equals("B", null));
        }

        [Fact]
        public void CompareValueType()
        {
            var Comparer = new GenericEqualityComparer<int>();
            Assert.True(Comparer.Equals(0, 0));
            Assert.False(Comparer.Equals(0, 1));
            Assert.False(Comparer.Equals(1, 0));
        }

        [Fact]
        public void GetHashCode()
        {
            var Comparer = new GenericEqualityComparer<string>();
            Assert.Equal("A".GetHashCode(), Comparer.GetHashCode("A"));
            Assert.Equal("B".GetHashCode(), Comparer.GetHashCode("B"));
        }
    }
}