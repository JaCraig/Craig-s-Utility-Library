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

using System.Linq;
using Utilities.DataTypes;
using Xunit;

namespace UnitTests.DataTypes
{
    public class ListMapping
    {
        [Fact]
        public void ContainsTest()
        {
            var TestObject = new ListMapping<string, int>();
            TestObject.Add("A", 0);
            TestObject.Add("A", 1);
            Assert.True(TestObject.Contains("A", 0));
            Assert.False(TestObject.Contains("A", 2));
        }

        [Fact]
        public void RandomTest()
        {
            var TestObject = new ListMapping<string, int>();
            var Rand = new System.Random();
            for (int x = 0; x < 10; ++x)
            {
                string Name = x.ToString();
                for (int y = 0; y < 5; ++y)
                {
                    int Value = Rand.Next();
                    TestObject.Add(Name, Value);
                    Assert.Equal(y + 1, TestObject[Name].Count());
                    Assert.True(TestObject[Name].Contains(Value));
                }
            }
            Assert.Equal(10, TestObject.Count);
        }

        [Fact]
        public void RemoveTest()
        {
            var TestObject = new ListMapping<string, int>();
            TestObject.Add("A", 0);
            TestObject.Add("A", 1);
            TestObject.Remove("A", 0);
            Assert.Equal(1, TestObject["A"].Count());
            Assert.Equal(1, TestObject["A"].FirstOrDefault());
        }
    }
}