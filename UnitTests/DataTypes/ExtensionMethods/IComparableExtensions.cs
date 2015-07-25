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

using Utilities.DataTypes;
using Xunit;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class IComparableExtensions
    {
        [Fact]
        public void BetweenTest()
        {
            int Value = 1;
            Assert.True(Value.Between(0, 2));
            Assert.False(Value.Between(2, 10));
        }

        [Fact]
        public void ClampTest()
        {
            int Value = 10;
            Assert.Equal(9, Value.Clamp(9, 1));
            Assert.Equal(11, Value.Clamp(15, 11));
            Assert.Equal(10, Value.Clamp(11, 1));
        }

        [Fact]
        public void MaxTest()
        {
            int Value = 4;
            Assert.Equal(5, Value.Max(5));
            Assert.Equal(4, Value.Max(1));
        }

        [Fact]
        public void MinTest()
        {
            int Value = 4;
            Assert.Equal(4, Value.Min(5));
            Assert.Equal(1, Value.Min(1));
        }
    }
}