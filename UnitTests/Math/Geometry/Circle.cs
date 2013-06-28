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


using Utilities.Math.ExtensionMethods;
using Xunit;

namespace UnitTests.Math.Geometry
{
    public class Circle
    {
        [Fact]
        public void BasicTest()
        {
            Utilities.Math.Geometry.Circle Circle = new Utilities.Math.Geometry.Circle(10, 10, 10);
            Utilities.Math.Geometry.Circle Circle2 = new Utilities.Math.Geometry.Circle(15, 15, 2);
            Assert.Equal(20, Circle.Diameter);
            Assert.Equal(62.83d, Circle.Circumference.Round());
            Assert.Equal(314.16d, Circle.Area.Round());
            Assert.True(Circle.Overlap(Circle2));
            Assert.True(Circle.Contains(5, 5));
        }
    }
}
