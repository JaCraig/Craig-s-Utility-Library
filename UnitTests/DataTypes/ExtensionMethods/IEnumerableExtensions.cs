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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Utilities.DataTypes;
using Xunit;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class IEnumerableExtensions
    {
        [Fact]
        public void Distinct()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 1, 3, 5, 2 }.ToList();
            List<int> Results = new int[] { 0, 1, 2, 3, 5 }.ToList();
            Assert.Equal(Results, Temp.Distinct((x, y) => x == y));
        }

        [Fact]
        public void ElementsBetween()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Assert.Equal(new int[] { 0, 0, 1 }.ToList(), Temp.ElementsBetween(0, 3));
        }

        [Fact]
        public void ForEachParallelTest()
        {
            ConcurrentBag<int> Builder = new ConcurrentBag<int>();
            int[] Temp = { 0, 0, 1, 2, 3 };
            Temp.ForEachParallel(x => Builder.Add(x));
            Assert.Equal(5, Builder.Count);
            string OrderedString = new string(Builder.OrderBy(x => x).ToString(x => x.ToString(), "").ToArray());
            Assert.Equal("00123", OrderedString);
        }

        [Fact]
        public void ForEachParallelTest2()
        {
            StringBuilder Builder = new StringBuilder();
            int[] Temp = { 0, 0, 1, 2, 3 };
            IEnumerable<string> Values = Temp.ForEachParallel(x => x.ToString());
            Assert.Equal(5, Values.Count());
            Values.ForEach<string>(x => Builder.Append(x));
            string OrderedString = new string(Builder.ToString().OrderBy(x => x).ToArray());
            Assert.Equal("00123", OrderedString);
        }

        [Fact]
        public void ForEachTest()
        {
            StringBuilder Builder = new StringBuilder();
            int[] Temp = { 0, 0, 1, 2, 3 };
            Temp.ForEach<int>(x => Builder.Append(x));
            Assert.Equal("00123", Builder.ToString());
        }

        [Fact]
        public void ForEachTest2()
        {
            StringBuilder Builder = new StringBuilder();
            int[] Temp = { 0, 0, 1, 2, 3 };
            Temp.ForEach(x => x.ToString()).ForEach<string>(x => Builder.Append(x));
            Assert.Equal("00123", Builder.ToString());
        }

        [Fact]
        public void ForEachTest3()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Temp.ForEach<int, float>(x => x > 0 ? (float)x : 1.0f);
        }

        [Fact]
        public void ForParallelTest()
        {
            StringBuilder Builder = new StringBuilder(30);
            int[] Temp = { 0, 0, 1, 2, 3 };
            Temp.ForParallel(0, Temp.Length - 1, x => Builder.Append(x));
            Assert.Equal(5, Builder.ToString().Length);
            string OrderedString = new string(Builder.ToString().OrderBy(x => x).ToArray());
            Assert.Equal("00123", OrderedString);
        }

        [Fact]
        public void ForParallelTest2()
        {
            StringBuilder Builder = new StringBuilder();
            int[] Temp = { 0, 0, 1, 2, 3 };
            IEnumerable<string> Values = Temp.ForParallel(0, Temp.Length - 1, x => x.ToString());
            Assert.Equal(5, Values.Count());
            Values.ForEach<string>(x => Builder.Append(x));
            string OrderedString = new string(Builder.ToString().OrderBy(x => x).ToArray());
            Assert.Equal("00123", OrderedString);
        }

        [Fact]
        public void ForTest()
        {
            StringBuilder Builder = new StringBuilder();
            int[] Temp = { 0, 0, 1, 2, 3 };
            Temp.For<int>(0, Temp.Length - 1, x => Builder.Append(x));
            Assert.Equal("00123", Builder.ToString());
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
        public void Last()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Assert.Equal(new int[] { 1, 2, 3 }.ToList(), Temp.Last(3));
        }

        [Fact]
        public void LeftJoin()
        {
            var Temp1 = new[]{
                new {A="A",ID=1},
                new {A="B",ID=2},
                new {A="C",ID=3}
            };
            var Temp2 = new[]{
                new {B="D",ID=1},
                new {B="E",ID=2}
            };
            var Result = Temp1.LeftJoin(Temp2, x => x.ID, x => x.ID, (x, y) => new { A = x == null ? "" : x.A, B = y == null ? "" : y.B }).ToList();
            Assert.Equal(3, Result.Count);
            Assert.True(Result.Any(x => x.A == "A" && x.B == "D"));
            Assert.True(Result.Any(x => x.A == "B" && x.B == "E"));
            Assert.True(Result.Any(x => x.A == "C" && x.B == ""));
        }

        [Fact]
        public void OuterJoin()
        {
            var Temp1 = new[]{
                new {A="A",ID=1},
                new {A="B",ID=2},
                new {A="C",ID=3}
            };
            var Temp2 = new[]{
                new {B="D",ID=1},
                new {B="E",ID=2},
                new {B="F",ID=4}
            };
            var Result = Temp1.OuterJoin(Temp2, x => x.ID, x => x.ID, (x, y) => new { A = x == null ? "" : x.A, B = y == null ? "" : y.B }).ToList();
            Assert.Equal(4, Result.Count);
            Assert.True(Result.Any(x => x.A == "A" && x.B == "D"));
            Assert.True(Result.Any(x => x.A == "B" && x.B == "E"));
            Assert.True(Result.Any(x => x.A == "C" && x.B == ""));
            Assert.True(Result.Any(x => x.A == "" && x.B == "F"));
        }

        [Fact]
        public void PositionOf()
        {
            Assert.Equal(0, new int[] { 1, 2, 3 }.PositionOf(1));
            Assert.Equal(1, new int[] { 1, 2, 3 }.PositionOf(2));
            Assert.Equal(2, new int[] { 1, 2, 3 }.PositionOf(3));
        }

        [Fact]
        public void RemoveDefaultsTest()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            foreach (int Value in Temp.Remove(x => x == 0))
                Assert.NotEqual(0, Value);
        }

        [Fact]
        public void RightJoin()
        {
            var Temp2 = new[]{
                new {A="A",ID=1},
                new {A="B",ID=2},
                new {A="C",ID=3}
            };
            var Temp1 = new[]{
                new {B="D",ID=1},
                new {B="E",ID=2}
            };
            var Result = Temp1.RightJoin(Temp2, x => x.ID, x => x.ID, (x, y) => new { A = y == null ? "" : y.A, B = x == null ? "" : x.B }).ToList();
            Assert.Equal(3, Result.Count);
            Assert.True(Result.Any(x => x.A == "A" && x.B == "D"));
            Assert.True(Result.Any(x => x.A == "B" && x.B == "E"));
            Assert.True(Result.Any(x => x.A == "C" && x.B == ""));
        }

        [Fact]
        public void ThrowIfAll()
        {
            Assert.Throws<Exception>(() => new int[] { 0, 0, 1, 2, 3 }.ThrowIfAll(x => x < 4, new Exception()));
            new int[] { 0, 0, 1, 2, 3 }.ThrowIfAll(x => x < 3, new Exception());
        }

        [Fact]
        public void ThrowIfAny()
        {
            Assert.Throws<Exception>(() => new int[] { 0, 0, 1, 2, 3 }.ThrowIfAny(x => x < 3, new Exception()));
            new int[] { 0, 0, 1, 2, 3 }.ThrowIfAny(x => x > 3, new Exception());
        }

        [Fact]
        public void ToArray()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Temp.ToArray<int, double>(x => (double)x);
            double[] Temp2 = Temp.ToArray<int, double>(x => (double)x);
            Assert.Equal(0, Temp2[0]);
            Assert.Equal(0, Temp2[1]);
            Assert.Equal(1, Temp2[2]);
            Assert.Equal(2, Temp2[3]);
            Assert.Equal(3, Temp2[4]);
        }

        [Fact]
        public void ToDataTable()
        {
            List<PreDataTable> Temp = new PreDataTable[] { new PreDataTable { ID = 1, Value = "A" }, new PreDataTable { ID = 2, Value = "B" }, new PreDataTable { ID = 3, Value = "C" } }.ToList();
            Temp.To();
            DataTable Temp2 = Temp.To();
            Assert.Equal(1, Temp2.Rows[0].ItemArray[0]);
            Assert.Equal("A", Temp2.Rows[0].ItemArray[1]);
            Assert.Equal(2, Temp2.Rows[1].ItemArray[0]);
            Assert.Equal("B", Temp2.Rows[1].ItemArray[1]);
            Assert.Equal(3, Temp2.Rows[2].ItemArray[0]);
            Assert.Equal("C", Temp2.Rows[2].ItemArray[1]);
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
        public void ToObservableList()
        {
            ObservableList<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToObservableList(x => x + 10);
            Assert.Equal(10, Temp[0]);
            Assert.Equal(10, Temp[1]);
            Assert.Equal(11, Temp[2]);
            Assert.Equal(12, Temp[3]);
            Assert.Equal(13, Temp[4]);
        }

        [Fact]
        public void ToStringTest()
        {
            List<int> Temp = new int[] { 0, 0, 1, 2, 3 }.ToList();
            Assert.Equal("0,0,1,2,3", Temp.ToString(Seperator: ","));
            Assert.NotEqual("0,0,1,2,3", Temp.ToString());
        }

        [Fact]
        public void Transverse()
        {
            TraverseClass TestObject = new TraverseClass() { Children = new TraverseClass[] { new TraverseClass(), new TraverseClass(), new TraverseClass(), new TraverseClass() }.ToList() };
            Assert.Equal(5, TestObject.Transverse(x => x.Children).Count());
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
    }

    public class PreDataTable
    {
        public virtual int ID { get; set; }

        public virtual string Value { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is PreDataTable))
                return false;
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode() + Value.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class TraverseClass
    {
        public TraverseClass()
        {
            Children = new List<TraverseClass>();
        }

        public List<TraverseClass> Children { get; set; }
    }
}