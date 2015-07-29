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
using System.ComponentModel.DataAnnotations;
using Utilities.Validation;

using Xunit;

namespace UnitTests.Validation.Rules
{
    public class Between
    {
        [Fact]
        public void Test()
        {
            var Temp = new ClassA();
            Temp.ItemA = 1;
            Temp.ItemB = DateTime.Now;
            Temp.Validate();
            Temp.ItemA = 0;
            Temp.ItemB = new DateTime(1800, 1, 1);
            Assert.Throws<ValidationException>(() => Temp.Validate());
        }
    }

    public class ClassA
    {
        [Between(1, 5)]
        public int ItemA { get; set; }

        [Between("1/1/1900", "1/1/2100")]
        public DateTime ItemB { get; set; }
    }
}