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

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class TimeSpanExtensions
    {
        [Fact]
        public void Average()
        {
            Assert.Equal(new TimeSpan(20), new TimeSpan[] { new TimeSpan(10), new TimeSpan(30) }.Average());
        }

        [Fact]
        public void DaysRemainder()
        {
            Assert.Equal(0, (new DateTime(2011, 12, 1) - new DateTime(1977, 1, 1)).DaysRemainder());
        }

        [Fact]
        public void Months()
        {
            Assert.Equal(11, (new DateTime(2011, 12, 1) - new DateTime(1977, 1, 1)).Months());
        }

        [Fact]
        public void ToStringFull()
        {
            Assert.Equal("34 years, 11 months", (new DateTime(2011, 12, 1) - new DateTime(1977, 1, 1)).ToStringFull());
        }

        [Fact]
        public void Years()
        {
            Assert.Equal(34, (new DateTime(2011, 12, 1) - new DateTime(1977, 1, 1)).Years());
        }
    }
}