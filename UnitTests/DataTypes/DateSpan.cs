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


namespace UnitTests.DataTypes
{
    public class DateSpan
    {
        [Fact]
        public void CompareTest()
        {
            Utilities.DataTypes.DateSpan Span1=new Utilities.DataTypes.DateSpan(new DateTime(1999, 1, 1), new DateTime(2009, 1, 1));
            Utilities.DataTypes.DateSpan Span2=new Utilities.DataTypes.DateSpan(new DateTime(1999, 1, 1), new DateTime(2009, 1, 1));
            Utilities.DataTypes.DateSpan Span3=new Utilities.DataTypes.DateSpan(new DateTime(1999, 1, 2), new DateTime(2009, 1, 1));

            Assert.True(Span1 == Span2);
            Assert.False(Span1 == Span3);
        }

        [Fact]
        public void IntersectionTest()
        {
            Utilities.DataTypes.DateSpan Span1 = new Utilities.DataTypes.DateSpan(new DateTime(1999, 1, 1), new DateTime(2003, 1, 1));
            Utilities.DataTypes.DateSpan Span2 = new Utilities.DataTypes.DateSpan(new DateTime(2002, 1, 1), new DateTime(2009, 1, 1));
            Utilities.DataTypes.DateSpan Span3 = Span1.Intersection(Span2);
            Assert.Equal(new DateTime(2002, 1, 1), Span3.Start);
            Assert.Equal(new DateTime(2003, 1, 1), Span3.End);
        }

        [Fact]
        public void OverlapTest()
        {
            Utilities.DataTypes.DateSpan Span1 = new Utilities.DataTypes.DateSpan(new DateTime(1999, 1, 1), new DateTime(2003, 1, 1));
            Utilities.DataTypes.DateSpan Span2 = new Utilities.DataTypes.DateSpan(new DateTime(2002, 1, 1), new DateTime(2009, 1, 1));
            Assert.True(Span1.Overlap(Span2));
        }

        [Fact]
        public void UnionTest()
        {
            Utilities.DataTypes.DateSpan Span1 = new Utilities.DataTypes.DateSpan(new DateTime(1999, 1, 1), new DateTime(2003, 1, 1));
            Utilities.DataTypes.DateSpan Span2 = new Utilities.DataTypes.DateSpan(new DateTime(2002, 1, 1), new DateTime(2009, 1, 1));
            Utilities.DataTypes.DateSpan Span3 = Span1 + Span2;
            Assert.Equal(new DateTime(1999, 1, 1), Span3.Start);
            Assert.Equal(new DateTime(2009, 1, 1), Span3.End);
        }

        [Fact]
        public void DifferenceTest()
        {
            Utilities.DataTypes.DateSpan Span1 = new Utilities.DataTypes.DateSpan(new DateTime(1999, 1, 1), new DateTime(2003, 1, 1));
            Assert.Equal(4, Span1.Years);
            Assert.Equal(0, Span1.Months);
            Assert.Equal(0, Span1.Days);
            Assert.Equal(0, Span1.Hours);
            Assert.Equal(0, Span1.Minutes);
            Assert.Equal(0, Span1.Seconds);
            Assert.Equal(0, Span1.MilliSeconds);
            Utilities.DataTypes.DateSpan Span2 = new Utilities.DataTypes.DateSpan(new DateTime(1999, 1, 1, 2, 3, 4), new DateTime(2003, 11, 15, 6, 45, 32));
            Assert.Equal(4, Span2.Years);
            Assert.Equal(10, Span2.Months);
            Assert.Equal(14, Span2.Days);
            Assert.Equal(4, Span2.Hours);
            Assert.Equal(42, Span2.Minutes);
            Assert.Equal(28, Span2.Seconds);
            Assert.Equal(0, Span2.MilliSeconds);
        }
    }
}
