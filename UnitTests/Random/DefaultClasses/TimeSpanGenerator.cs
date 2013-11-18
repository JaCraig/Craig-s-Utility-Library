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

using System;

using Xunit;

namespace UnitTests.Random.DefaultClasses
{
    public class TimeSpanGenerator
    {
        [Fact]
        public void Next()
        {
            Utilities.Random.DefaultClasses.TimeSpanGenerator Generator = new Utilities.Random.DefaultClasses.TimeSpanGenerator();
            Assert.InRange(Generator.Next(new System.Random()), TimeSpan.MinValue, TimeSpan.MaxValue);
            Assert.InRange(Generator.Next(new System.Random(), new TimeSpan(10, 1, 1), new TimeSpan(12, 2, 3)), new TimeSpan(10, 1, 1), new TimeSpan(12, 2, 3));
        }
    }
}
