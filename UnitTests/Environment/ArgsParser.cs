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
using Utilities.Environment.ExtensionMethods;
using Utilities.DataTypes.ExtensionMethods;
using System.Security.Cryptography;
using System.Diagnostics;

namespace UnitTests.Environment
{
    public class ArgsParser
    {
        [Fact]
        public void Create()
        {
            Utilities.Environment.ArgsParser TestObject = new Utilities.Environment.ArgsParser();
            List<Utilities.Environment.Option> Options= TestObject.Parse(new string[] { "/TestOption Parameter /TestOption2 \"This is a test\"" });
            Assert.NotNull(TestObject);
            Assert.Equal("Parameter", Options[0].Parameters[0]);
            Assert.Equal("TestOption", Options[0].Command);
            Assert.Equal("\"This is a test\"", Options[1].Parameters[0]);
            Assert.Equal("TestOption2", Options[1].Command);
        }
    }
}
