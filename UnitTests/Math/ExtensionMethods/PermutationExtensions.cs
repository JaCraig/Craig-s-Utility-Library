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
using Utilities.Math.ExtensionMethods;
using Utilities.DataTypes;

namespace UnitTests.Math.ExtensionMethods
{
    /// <summary>
    /// Permutation extensions
    /// </summary>
    public class PermutationExtensions
    {
        [Fact]
        public void BasicTest()
        {
            System.Collections.Generic.List<string> TestObject = new System.Collections.Generic.List<string>();
            TestObject.AddRange(new string[] { "this", "is", "a", "test" });
            ListMapping<int, string> Results = TestObject.Permute();
            Assert.Equal(24, Results.Keys.Count);
            foreach (int Key in Results.Keys)
                foreach (string Item in Results[Key])
                    Assert.True(Item == "this" || Item == "is" || Item == "a" || Item == "test");
        }
    }
}