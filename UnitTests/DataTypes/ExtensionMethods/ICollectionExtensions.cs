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
using System.Collections;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class ICollectionExtensions
    {
        [Test]
        public void AddIfTest()
        {
            List<int> TestObject = new int[] { 1, 2, 3, 4, 5, 6 }.ToList();
            Assert.False(TestObject.AddIf(1, x => x > 1));
            Assert.True(TestObject.AddIf(7, x => x > 1));
            Assert.True(TestObject.AddIf(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }, x => x > 7));
            Assert.Equal(8, TestObject.Count);
        }

        [Test]
        public void AddIfUniqueTest()
        {
            List<int> TestObject = new int[] { 1, 2, 3, 4, 5, 6 }.ToList();
            Assert.False(TestObject.AddIfUnique(1));
            Assert.True(TestObject.AddIfUnique(7));
            Assert.True(TestObject.AddIfUnique(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }));
            Assert.Equal(8, TestObject.Count);
        }

        [Test]
        public void RemoveTest()
        {
            List<int> TestObject = new int[] { 1, 2, 3, 4, 5, 6 }.ToList();
            TestObject.Remove((x) => x % 2 == 0);
            Assert.Equal(3, TestObject.Count);
            foreach (int Item in TestObject)
                Assert.False(Item % 2 == 0);
        }

        [Test]
        public void AddRange()
        {
            List<int> TestObject = new int[] { 1, 2, 3, 4, 5, 6 }.ToList();
            Assert.Equal(14, TestObject.AddRange<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }).Count);
        }

        [Test]
        public void RemoveRange()
        {
            List<int> TestObject = new int[] { 1, 2, 3, 4, 5, 6 }.ToList();
            Assert.Equal(0, TestObject.RemoveRange<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 }).Count);
        }
    }
}
