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
    public class ObjectExtensions
    {
        [Fact]
        public void IsTest()
        {
            object TestObject = null;
            Assert.True(TestObject.Is(x => x == null));
            TestObject = 1;
            Assert.False(TestObject.Is(x => x == null));
            int TestObject2 = 1;
            Assert.False(TestObject2.Is(x => x < 2, x => x > 3));
            Assert.True(TestObject2.Is(x => x < 2, x => x > 0));
            Assert.True(TestObject2.Is(1));
            Assert.False(TestObject2.Is(2));
        }

        [Fact]
        public void Check()
        {
            object TestObject = null;
            Assert.Equal(3, TestObject.Check(x => x != null, 3));
            Assert.Equal(null, TestObject.Check(x => x == null, 3));
        }
    }
}
