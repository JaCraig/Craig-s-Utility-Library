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
using Xunit;

using Utilities.DataTypes.ExtensionMethods;
using System.Data;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class IEnumerableExtensions
    {
        [Fact]
        public void ForEachTest3()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Assert.DoesNotThrow(() => Temp.ForEach<int, float>(x => x > 0 ? (float)x : 1.0f));
        }

        [Fact]
        public void IsNullOrEmptyTest()
        {
            List<int> Temp = new List<int>();
            Assert.True(Temp.Is(x => x == null || x.Count == 0));
            Temp = null;
            Assert.True(Temp.Is(x => x == null || x.Count == 0));
            Temp = new int[] { 1, 2, 3 }.ToList();
            Assert.False(Temp.Is(x => x == null || x.Count == 0));
        }

        [Fact]
        public void RemoveDefaultsTest()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            foreach (int Value in Temp.Remove(x => x == 0))
                Assert.NotEqual(0, Value);
        }

        [Fact]
        public void ForEachTest()
        {
            StringBuilder Builder = new StringBuilder();
            int[] Temp = new int[] { 0, 0, 1, 2, 3 };
            Temp.ForEach<int>(x => Builder.Append(x));
            Assert.Equal("00123", Builder.ToString());
        }

        [Fact]
        public void ForEachTest2()
        {
            StringBuilder Builder = new StringBuilder();
            int[] Temp = new int[] { 0, 0, 1, 2, 3 };
            Temp.ForEach(x => x.ToString()).ForEach<string>(x => Builder.Append(x));
            Assert.Equal("00123", Builder.ToString());
        }

        [Fact]
        public void ForEachParallelTest()
        {
            StringBuilder Builder = new StringBuilder(30);
            int[] Temp = new int[] { 0, 0, 1, 2, 3 };
            Temp.ForEachParallel(x => Builder.Append(x));
            Assert.Equal(5, Builder.ToString().Length);
            string OrderedString = new string(Builder.ToString().OrderBy(x => x).ToArray());
            Assert.Equal("00123", OrderedString);
        }

        [Fact]
        public void ForEachParallelTest2()
        {
            StringBuilder Builder = new StringBuilder();
            int[] Temp = new int[] { 0, 0, 1, 2, 3 };
            IEnumerable<string> Values = Temp.ForEachParallel(x => x.ToString());
            Assert.Equal(5, Values.Count());
            Values.ForEach<string>(x => Builder.Append(x));
            string OrderedString = new string(Builder.ToString().OrderBy(x => x).ToArray());
            Assert.Equal("00123", OrderedString);
        }

        [Fact]
        public void ForTest()
        {
            StringBuilder Builder = new StringBuilder();
            int[] Temp = new int[] { 0, 0, 1, 2, 3 };
            Temp.For<int>(0, Temp.Length - 1, x => Builder.Append(x));
            Assert.Equal("00123", Builder.ToString());
        }

        [Fact]
        public void ForParallelTest()
        {
            StringBuilder Builder = new StringBuilder(30);
            int[] Temp = new int[] { 0, 0, 1, 2, 3 };
            Temp.ForParallel(0, Temp.Length - 1, x => Builder.Append(x));
            Assert.Equal(5, Builder.ToString().Length);
            string OrderedString = new string(Builder.ToString().OrderBy(x => x).ToArray());
            Assert.Equal("00123", OrderedString);
        }

        [Fact]
        public void ForParallelTest2()
        {
            StringBuilder Builder = new StringBuilder();
            int[] Temp = new int[] { 0, 0, 1, 2, 3 };
            IEnumerable<string> Values = Temp.ForParallel(0, Temp.Length - 1, x => x.ToString());
            Assert.Equal(5, Values.Count());
            Values.ForEach<string>(x => Builder.Append(x));
            string OrderedString = new string(Builder.ToString().OrderBy(x => x).ToArray());
            Assert.Equal("00123", OrderedString);
        }

        [Fact]
        public void PositionOf()
        {
            Assert.Equal(0, new int[] { 1, 2, 3 }.PositionOf(1));
            Assert.Equal(1, new int[] { 1, 2, 3 }.PositionOf(2));
            Assert.Equal(2, new int[] { 1, 2, 3 }.PositionOf(3));
        }

        [Fact]
        public void ToArray()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Assert.DoesNotThrow(() => Temp.ToArray<int, double>(x => (double)x));
            double[] Temp2 = Temp.ToArray<int, double>(x => (double)x);
            Assert.Equal(0, Temp2[0]);
            Assert.Equal(0, Temp2[1]);
            Assert.Equal(1, Temp2[2]);
            Assert.Equal(2, Temp2[3]);
            Assert.Equal(3, Temp2[4]);
        }

        [Fact]
        public void ToList()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList(x => x + 10);
            Assert.Equal(10, Temp[0]);
            Assert.Equal(10, Temp[1]);
            Assert.Equal(11, Temp[2]);
            Assert.Equal(12, Temp[3]);
            Assert.Equal(13, Temp[4]);
        }

        [Fact]
        public void ThrowIfAll()
        {
            Assert.Throws<Exception>(() => new int[] { 0, 0, 1, 2, 3 }.ThrowIfAll(x => x < 4, new Exception()));
            Assert.DoesNotThrow(() => new int[] { 0, 0, 1, 2, 3 }.ThrowIfAll(x => x < 3, new Exception()));
        }

        [Fact]
        public void ThrowIfAny()
        {
            Assert.Throws<Exception>(() => new int[] { 0, 0, 1, 2, 3 }.ThrowIfAny(x => x < 3, new Exception()));
            Assert.DoesNotThrow(() => new int[] { 0, 0, 1, 2, 3 }.ThrowIfAny(x => x > 3, new Exception()));
        }

        [Fact]
        public void ToDataTable()
        {
            List<PreDataTable> Temp = new PreDataTable[] { new PreDataTable { ID = 1, Value = "A" }, new PreDataTable { ID = 2, Value = "B" }, new PreDataTable { ID = 3, Value = "C" } }.ToList();
            Assert.DoesNotThrow(() => Temp.ToDataTable());
            DataTable Temp2 = Temp.ToDataTable();
            Assert.Equal(1, Temp2.Rows[0].ItemArray[0]);
            Assert.Equal("A", Temp2.Rows[0].ItemArray[1]);
            Assert.Equal(2, Temp2.Rows[1].ItemArray[0]);
            Assert.Equal("B", Temp2.Rows[1].ItemArray[1]);
            Assert.Equal(3, Temp2.Rows[2].ItemArray[0]);
            Assert.Equal("C", Temp2.Rows[2].ItemArray[1]);
        }

        [Fact]
        public void ToStringTest()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Assert.Equal("0,0,1,2,3", Temp.ToString(Seperator: ","));
            Assert.NotEqual("0,0,1,2,3", Temp.ToString());
        }

        [Fact]
        public void TryAll()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            List<int> Results = new List<int>();
            Temp.ForEach(x => Results.Add(x == 0 ? 4 : x));
            Assert.Equal(4, Results[0]);
            Assert.Equal(4, Results[1]);
            Assert.Equal(1, Results[2]);
            Assert.Equal(2, Results[3]);
            Assert.Equal(3, Results[4]);
            List<int?> Temp2 = new int?[] { 0, 0, 1, 2, 3, null }.ToList();
            List<int?> Results2 = new List<int?>();
            Temp2.ForEach(x => Results2.Add(x == 0 ? 4 : x.Value + 1), (x, y) => Results2.Add(5));
            Assert.Equal(4, Results2[0]);
            Assert.Equal(4, Results2[1]);
            Assert.Equal(2, Results2[2]);
            Assert.Equal(3, Results2[3]);
            Assert.Equal(4, Results2[4]);
            Assert.Equal(5, Results2[5]);
        }

        [Fact]
        public void TryAllParallel()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            List<int> Results = new List<int>(10);
            Temp.ForEachParallel(x => Results.Add(x == 0 ? 4 : x));
            Assert.Equal(5, Results.Count);
            Assert.Equal(14, Results.Sum());
            List<int?> Temp2 = new int?[] { 0, 0, 1, 2, 3, null }.ToList();
            List<int?> Results2 = new List<int?>(10);
            Temp2.ForEachParallel(x => Results2.Add(x == 0 ? 4 : x.Value + 1), (x, y) => Results2.Add(5));
            Assert.Equal(6, Results2.Count);
            Assert.Equal(22, Results2.Sum());
        }

        [Fact]
        public void ElementsBetween()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Assert.Equal(new int[] { 0, 0, 1 }.ToList(), Temp.ElementsBetween(0, 3));
        }
        
        [Fact]
        public void Last()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Assert.Equal(new int[] { 1, 2, 3 }.ToList(), Temp.Last(3));
        }
    }

    public class PreDataTable
    {
        public virtual int ID { get; set; }
        public virtual string Value { get; set; }
        public override int GetHashCode()
        {
            return ID.GetHashCode() + Value.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PreDataTable))
                return false;
            return GetHashCode() == obj.GetHashCode();
        }
    }
}