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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonUnit;
using MoonUnit.Attributes;
using Utilities.DataTypes.ExtensionMethods;
using System.Data;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class DateTimeExtension
    {
        [Test]
        public void IsInFutureTest()
        {
            Assert.True(new DateTime(2100, 1, 1).IsInFuture());
        }

        [Test]
        public void IsInPastTest()
        {
            Assert.True(new DateTime(1900, 1, 1).IsInPast());
        }

        [Test]
        public void DaysLeftInMonthTest()
        {
            Assert.Equal(29, new DateTime(1999, 1, 2).DaysLeftInMonth());
        }

        [Test]
        public void DaysLeftInYearTest()
        {
            Assert.Equal(363, new DateTime(1999, 1, 2).DaysLeftInYear());
        }

        [Test]
        public void DaysLeftInWeekTest()
        {
            Assert.Equal(0, new DateTime(1999, 1, 2).DaysLeftInWeek());
        }

        [Test]
        public void IsWeekDayTest()
        {
            Assert.False(new DateTime(1999, 1, 2).IsWeekDay());
        }

        [Test]
        public void IsWeekEndTest()
        {
            Assert.True(new DateTime(1999, 1, 2).IsWeekEnd());
        }

        [Test]
        public void FirstDayOfMonth()
        {
            Assert.Equal(new DateTime(1999, 1, 1), new DateTime(1999, 1, 2).FirstDayOfMonth());
        }

        [Test]
        public void LastDayOfMonth()
        {
            Assert.Equal(new DateTime(1999, 1, 31), new DateTime(1999, 1, 2).LastDayOfMonth());
        }

        [Test]
        public void DaysInMonth()
        {
            Assert.Equal(31, new DateTime(1999, 1, 2).DaysInMonth());
        }

        [Test]
        public void FirstDayOfWeek()
        {
            Assert.Equal(new DateTime(1998, 12, 27), new DateTime(1999, 1, 2).FirstDayOfWeek());
        }

        [Test]
        public void LastDayOfWeek()
        {
            Assert.Equal(new DateTime(1999, 1, 2), new DateTime(1999, 1, 2).LastDayOfWeek());
        }

        [Test]
        public void BeginningOfDay()
        {
            Assert.Equal(new DateTime(1999, 1, 2), new DateTime(1999, 1, 2, 12, 1, 1).BeginningOfDay());
        }

        [Test]
        public void EndOfDay()
        {
            Assert.Equal(new DateTime(1999, 1, 2, 23, 59, 59), new DateTime(1999, 1, 2, 12, 1, 1).EndOfDay());
        }

        [Test]
        public void ToUnix()
        {
            Assert.Equal(915166800, new DateTime(1999, 1, 1).ToUnix());
        }

        [Test]
        public void FromUnix()
        {
            Assert.Equal(new DateTime(2009, 2, 13, 23, 31, 30), 1234567890.FromUnixTime());
        }

        [Test]
        public void UTCOffset()
        {
            Assert.Equal(-5, new DateTime(1999, 1, 2, 23, 1, 1, DateTimeKind.Local).UTCOffset());
        }

        [Test]
        public void Age()
        {
            Assert.Equal(41, new DateTime(1940, 1, 1).Age(new DateTime(1981, 1, 1)));
        }

        [Test]
        public void IsToday()
        {
            Assert.True(DateTime.Now.IsToday());
        }

        [Test]
        public void SetTime()
        {
            Assert.Equal(new DateTime(2009, 1, 1, 14, 2, 12), new DateTime(2009, 1, 1, 2, 3, 4).SetTime(14, 2, 12));
        }

        [Test]
        public void AddWeeks()
        {
            Assert.Equal(new DateTime(2009, 1, 15, 2, 3, 4), new DateTime(2009, 1, 1, 2, 3, 4).AddWeeks(2));
        }
    }
}
