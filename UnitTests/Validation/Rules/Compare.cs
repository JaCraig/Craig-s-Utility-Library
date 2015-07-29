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
    public class Compare
    {
        [Fact]
        public void Test()
        {
            var Temp = new CompareClass();
            Temp.ItemA = 1;
            Temp.ItemB = 2.1f;
            Temp.ItemC = new DateTime(1900, 1, 1);
            Temp.ItemD = "a";
            Temp.ItemE = -1;
            Temp.ItemF = DateTime.Now;
            Temp.NaNTest = 1;
            Temp.Validate();
            Temp.ItemA = 2;
            Temp.NaNTest = double.NaN;
            Assert.Throws<ValidationException>(() => Temp.Validate());
        }
    }

    public class CompareClass
    {
        [Utilities.Validation.Compare(1, ComparisonType.Equal)]
        public int ItemA { get; set; }

        [Utilities.Validation.Compare(2.0f, ComparisonType.GreaterThan)]
        public float ItemB { get; set; }

        [Utilities.Validation.Compare("1/1/1900", ComparisonType.GreaterThanOrEqual)]
        public DateTime ItemC { get; set; }

        [Utilities.Validation.Compare("A", ComparisonType.LessThan)]
        public string ItemD { get; set; }

        [Utilities.Validation.Compare(0, ComparisonType.LessThanOrEqual)]
        public long ItemE { get; set; }

        [Utilities.Validation.Compare("1/1/2100", ComparisonType.NotEqual)]
        public DateTime ItemF { get; set; }

        [Utilities.Validation.Compare(double.NaN, ComparisonType.NotEqual)]
        public double NaNTest { get; set; }
    }
}