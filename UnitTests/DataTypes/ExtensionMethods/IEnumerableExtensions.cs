/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
    public class IEnumerableExtensions
    {
        [Test]
        public void Cast()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Assert.DoesNotThrow<Exception>(() => Temp.Cast<int, float>(x => x > 0 ? (float)x : 1.0f));
        }

        [Test]
        public void IsNullOrEmptyTest()
        {
            List<int> Temp = new List<int>();
            Assert.True(Temp.IsNullOrEmpty());
            Temp = null;
            Assert.True(Temp.IsNullOrEmpty());
            Temp = new int[] { 1, 2, 3 }.ToList();
            Assert.False(Temp.IsNullOrEmpty());
        }

        [Test]
        public void RemoveDefaultsTest()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            foreach (int Value in Temp.RemoveDefaults())
                Assert.NotEqual(0, Value);
        }

        [Test]
        public void ForEachTest()
        {
            StringBuilder Builder = new StringBuilder();
            int[] Temp = new int[] { 0, 0, 1, 2, 3 };
            Temp.ForEach<int>(x => Builder.Append(x));
            Assert.Equal("00123", Builder.ToString());
        }

        [Test]
        public void ForEachTest2()
        {
            StringBuilder Builder = new StringBuilder();
            int[] Temp = new int[] { 0, 0, 1, 2, 3 };
            Temp.ForEach(x => x.ToString()).ForEach<string>(x => Builder.Append(x));
            Assert.Equal("00123", Builder.ToString());
        }

        [Test]
        public void ForEachParallelTest()
        {
            StringBuilder Builder = new StringBuilder(30);
            int[] Temp = new int[] { 0, 0, 1, 2, 3 };
            Temp.ForEachParallel(x => Builder.Append(x));
            Assert.Equal(5, Builder.ToString().Length);
            string OrderedString = new string(Builder.ToString().OrderBy(x => x).ToArray());
            Assert.Equal("00123", OrderedString);
        }

        [Test]
        public void ForTest()
        {
            StringBuilder Builder = new StringBuilder();
            int[] Temp = new int[] { 0, 0, 1, 2, 3 };
            Temp.For<int>(0, Temp.Length - 1, x => Builder.Append(x));
            Assert.Equal("00123", Builder.ToString());
        }

        [Test]
        public void ForParallelTest()
        {
            StringBuilder Builder = new StringBuilder(30);
            int[] Temp = new int[] { 0, 0, 1, 2, 3 };
            Temp.ForParallel(0, Temp.Length - 1, x => Builder.Append(x));
            Assert.Equal(5, Builder.ToString().Length);
            string OrderedString = new string(Builder.ToString().OrderBy(x => x).ToArray());
            Assert.Equal("00123", OrderedString);
        }

        [Test]
        public void ToArray()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Assert.DoesNotThrow<Exception>(() => Temp.ToArray<int, double>(x => (double)x));
            double[] Temp2 = Temp.ToArray<int, double>(x => (double)x);
            Assert.Equal(0, Temp2[0]);
            Assert.Equal(0, Temp2[1]);
            Assert.Equal(1, Temp2[2]);
            Assert.Equal(2, Temp2[3]);
            Assert.Equal(3, Temp2[4]);
        }

        [Test]
        public void ToStringTest()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Assert.Equal("0,0,1,2,3", Temp.ToString(Seperator:","));
            Assert.NotEqual("0,0,1,2,3", Temp.ToString());
        }

        [Test]
        public void TrueForEach()
        {
            IEnumerable<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Assert.True(Temp.TrueForAll(x => x < 4));
        }

        [Test]
        public void TryAll()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            List<int> Results = new List<int>();
            Temp.TryAll(x => Results.Add(x == 0 ? 4 : x));
            Assert.Equal(4, Results[0]);
            Assert.Equal(4, Results[1]);
            Assert.Equal(1, Results[2]);
            Assert.Equal(2, Results[3]);
            Assert.Equal(3, Results[4]);
            List<int?> Temp2 = new int?[] { 0, 0, 1, 2, 3, null }.ToList();
            List<int?> Results2 = new List<int?>();
            Temp2.TryAll(x => Results2.Add(x == 0 ? 4 : x.Value + 1), x => Results2.Add(5));
            Assert.Equal(4, Results2[0]);
            Assert.Equal(4, Results2[1]);
            Assert.Equal(2, Results2[2]);
            Assert.Equal(3, Results2[3]);
            Assert.Equal(4, Results2[4]);
            Assert.Equal(5, Results2[5]);
        }

        [Test]
        public void TryAllParallel()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            List<int> Results = new List<int>(10);
            Temp.TryAllParallel(x => Results.Add(x == 0 ? 4 : x));
            Assert.Equal(5, Results.Count);
            Assert.Equal(14, Results.Sum());
            List<int?> Temp2 = new int?[] { 0, 0, 1, 2, 3, null }.ToList();
            List<int?> Results2 = new List<int?>(10);
            Temp2.TryAllParallel(x => Results2.Add(x == 0 ? 4 : x.Value + 1), x => Results2.Add(5));
            Assert.Equal(6, Results2.Count);
            Assert.Equal(22, Results2.Sum());
        }
    }
}