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

using Xunit;

namespace UnitTests.DataTypes
{
    public class Fraction
    {
        [Fact]
        public void BasicTest()
        {
            var TestObject = new Utilities.DataTypes.Fraction(9, 27);
            var TestObject2 = new Utilities.DataTypes.Fraction(3, 4);
            TestObject.Reduce();
            Assert.Equal(3, TestObject.Denominator);
            Assert.Equal(1, TestObject.Numerator);
            Assert.Equal(new Utilities.DataTypes.Fraction(1, 4), TestObject * TestObject2);
            Assert.Equal(new Utilities.DataTypes.Fraction(13, 12), TestObject + TestObject2);
            Assert.Equal(new Utilities.DataTypes.Fraction(-5, 12), TestObject - TestObject2);
            Assert.Equal(new Utilities.DataTypes.Fraction(4, 9), TestObject / TestObject2);
            Assert.Equal(new Utilities.DataTypes.Fraction(-1, 3), -TestObject);
            Assert.Equal(new Utilities.DataTypes.Fraction(9, 27), TestObject);
        }
    }
}