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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.DataTypes;
using Xunit;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class ICollectionExtensions
    {
        [Fact]
        public void AddAndReturnTest()
        {
            List<int> TestObject = new int[] { 1, 2, 3, 4, 5, 6 }.ToList();
            int Item = 7;
            Assert.Equal(Item, TestObject.AddAndReturn(Item));
        }

        [Fact]
        public void AddIfTest()
        {
            List<int> TestObject = new int[] { 1, 2, 3, 4, 5, 6 }.ToList();
            Assert.False(TestObject.AddIf(x => x > 1, 1));
            Assert.True(TestObject.AddIf(x => x > 1, 7));
            Assert.True(TestObject.AddIf(x => x > 7, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
            Assert.Equal(8, TestObject.Count);
        }

        [Fact]
        public void AddIfUniqueTest()
        {
            List<int> TestObject = new int[] { 1, 2, 3, 4, 5, 6 }.ToList();
            Assert.False(TestObject.AddIfUnique(1));
            Assert.True(TestObject.AddIfUnique(7));
            Assert.True(TestObject.AddIfUnique(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
            Assert.Equal(8, TestObject.Count);
            TestObject = new int[] { 1, 2, 3, 4, 5, 6 }.ToList();
            Assert.False(TestObject.AddIfUnique((x, y) => x == y, 1));
            Assert.True(TestObject.AddIfUnique((x, y) => x == y, 7));
            Assert.True(TestObject.AddIfUnique((x, y) => x == y, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
            Assert.Equal(8, TestObject.Count);
        }

        [Fact]
        public void AddRange()
        {
            List<int> TestObject = new int[] { 1, 2, 3, 4, 5, 6 }.ToList();
            List<int> Results = TestObject.Add(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }).ToList();
            Assert.Equal(14, Results.Count);
            Assert.Equal(14, TestObject.Count);
        }

        [Fact]
        public void ForTest()
        {
            var Builder = new StringBuilder();
            int[] Temp = { 0, 0, 1, 2, 3 };
            Temp.For(0, Temp.Length - 1, (x, y) => Builder.Append(y));
            Assert.Equal("00123", Builder.ToString());
            Builder = new StringBuilder();
            Temp.For(0, Temp.Length - 1, (x, y) => Builder.Append(x));
            Assert.Equal("01234", Builder.ToString());
        }

        [Fact]
        public void ForTest2()
        {
            var Builder = new StringBuilder();
            int[] Temp = { 0, 0, 1, 2, 3 };
            int[] Temp2 = Temp.For(0, Temp.Length - 1, (x, y) => x + y).ToArray();
            Assert.Equal(new int[] { 0, 1, 3, 5, 7 }, Temp2);
        }

        [Fact]
        public void RemoveRange()
        {
            List<int> TestObject = new int[] { 1, 2, 3, 4, 5, 6 }.ToList();
            Assert.Equal(0, TestObject.Remove(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }).Count);
        }

        [Fact]
        public void RemoveRange2()
        {
            int[] TestObject = { 1, 2, 3, 4, 5, 6 };
            Assert.Equal(0, TestObject.Remove(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }).Count);
            TestObject = new int[] { 1, 2, 3, 4, 5, 6 };
            Assert.Equal(1, TestObject.Remove(new int[] { 1, 2, 3, 4, 5 }).Count);
            TestObject = new int[] { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6 };
            Assert.Equal(1, TestObject.Remove(new int[] { 1, 2, 3, 4, 5 }).Count);
        }

        [Fact]
        public void RemoveTest()
        {
            List<int> TestObject = new int[] { 1, 2, 3, 4, 5, 6 }.ToList();
            TestObject = TestObject.Remove((x) => x % 2 == 0).ToList();
            Assert.Equal(3, TestObject.Count());
            foreach (int Item in TestObject)
                Assert.False(Item % 2 == 0);
        }

        [Fact]
        public void RemoveTest2()
        {
            int[] TestObject = { 1, 2, 3, 4, 5, 6 };
            TestObject = TestObject.Remove((x) => x % 2 == 0).ToArray();
            Assert.Equal(3, TestObject.Count());
            foreach (int Item in TestObject)
                Assert.False(Item % 2 == 0);
        }
    }
}