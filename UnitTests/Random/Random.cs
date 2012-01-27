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
using MoonUnit.Attributes;
using MoonUnit;
using System.Threading.Tasks;

namespace UnitTests.Random
{
    public class Random
    {
        [Test]
        public void NextDateTest()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.Between(Rand.NextDate(new DateTime(1999, 1, 1), new DateTime(2000, 1, 1)), new DateTime(1999, 1, 1), new DateTime(2000, 1, 1));
        }

        [Test]
        public void NextStringTest()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.Equal(10, Rand.NextString(10).Length);
        }

        [Test]
        public void NextLoremIpsumTest()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.NotNull(Rand.NextLoremIpsum(1, 4));
        }

        [Test]
        public void NextBoolTest()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.DoesNotThrow<Exception>(() => Rand.NextBool());
        }

        [Test]
        public void NextEnumTest()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.DoesNotThrow<Exception>(() => Rand.NextEnum<TestEnum>());
        }

        [Test]
        public void NextTimeSpanTest()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.Between(Rand.NextTimeSpan(new TimeSpan(1, 0, 0), new TimeSpan(2, 0, 0)), new TimeSpan(1, 0, 0), new TimeSpan(2, 0, 0));
        }

        [Test]
        public void NextColorTest()
        {
            Utilities.Random.Random Rand = new Utilities.Random.Random();
            Assert.DoesNotThrow<Exception>(() => Rand.NextColor());
        }

        [Test]
        public void ThreadSafeNext()
        {
            Parallel.For(0, 100, x =>
            {
                Assert.DoesNotThrow<Exception>(() => Utilities.Random.Random.ThreadSafeNext(-20, 20));
            });
        }
    }

    public enum TestEnum
    {
        Number1,
        Number2,
        Number3
    }
}
