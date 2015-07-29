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

using System.ComponentModel.DataAnnotations;
using Utilities.Validation;

using Xunit;

namespace UnitTests.Validation.Rules
{
    public class Is
    {
        [Fact]
        public void Test()
        {
            var Temp = new IsClass();
            Temp.ItemA = "4012888888881881";
            Temp.ItemB = "1234.123";
            Temp.ItemC = "http://www.google.com";
            Temp.ItemD = "1234";
            Temp.Validate();
            Temp.ItemA = "1234567890123";
            Temp.ItemB = "ASD1234";
            Temp.ItemC = "google@somewhere.com";
            Temp.ItemD = "123.4313";
            Assert.Throws<ValidationException>(() => Temp.Validate());
        }
    }

    public class IsClass
    {
        [Is(Utilities.Validation.IsValid.CreditCard)]
        public string ItemA { get; set; }

        [Is(Utilities.Validation.IsValid.Decimal)]
        public string ItemB { get; set; }

        [Is(Utilities.Validation.IsValid.Domain)]
        public string ItemC { get; set; }

        [Is(Utilities.Validation.IsValid.Integer)]
        public string ItemD { get; set; }
    }
}