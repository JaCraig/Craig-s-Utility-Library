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
using Utilities.DataTypes;
using Xunit;

namespace UnitTests.DataTypes.ExtensionMethods
{
    public class IDictionaryExtensions
    {
        [Fact]
        public void CopyToTest()
        {
            var Test = new Dictionary<string, int>();
            var Test2 = new Dictionary<string, int>();
            Test.Add("Q", 4);
            Test.Add("Z", 2);
            Test.Add("C", 3);
            Test.Add("A", 1);
            Test.CopyTo(Test2);
            string Value = "";
            int Value2 = 0;
            foreach (string Key in Test2.Keys.OrderBy(x => x))
            {
                Value += Key;
                Value2 += Test2[Key];
            }
            Assert.Equal("ACQZ", Value);
            Assert.Equal(10, Value2);
        }

        [Fact]
        public void GetValue()
        {
            var Test = new Dictionary<string, int>();
            Test.Add("Q", 4);
            Test.Add("Z", 2);
            Test.Add("C", 3);
            Test.Add("A", 1);
            Assert.Equal(4, Test.GetValue("Q"));
            Assert.Equal(0, Test.GetValue("V"));
            Assert.Equal(123, Test.GetValue("B", 123));
        }

        [Fact]
        public void SetValue()
        {
            var Test = new Dictionary<string, int>();
            Test.Add("Q", 4);
            Test.Add("Z", 2);
            Test.Add("C", 3);
            Test.Add("A", 1);
            Assert.Equal(4, Test.GetValue("Q"));
            Test.SetValue("Q", 40);
            Assert.Equal(40, Test.GetValue("Q"));
        }

        [Fact]
        public void SortByValueTest()
        {
            IDictionary<string,int> Test = new Dictionary<string, int>();
            Test.Add("Q", 4);
            Test.Add("Z", 2);
            Test.Add("C", 3);
            Test.Add("A", 1);
            Test = Test.Sort(x => x.Value);
            string Value = "";
            foreach (string Key in Test.Keys)
                Value += Test[Key].ToString();
            Assert.Equal("1234", Value);
        }

        [Fact]
        public void SortTest()
        {
            IDictionary<string, int> Test = new Dictionary<string, int>();
            Test.Add("Q", 4);
            Test.Add("Z", 2);
            Test.Add("C", 3);
            Test.Add("A", 1);
            Test = Test.Sort();
            string Value = "";
            foreach (string Key in Test.Keys)
                Value += Key;
            Assert.Equal("ACQZ", Value);
        }
    }
}