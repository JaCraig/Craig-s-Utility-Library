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

using Xunit;

namespace UnitTests.IO.Messaging.Default
{
    public class DefaultFormatter
    {
        [Fact]
        public void Create()
        {
            new Utilities.IO.Messaging.Default.DefaultFormatter();
            Assert.Equal("Default", new Utilities.IO.Messaging.Default.DefaultFormatter().Name);
        }

        [Fact]
        public void Format()
        {
            var TestObject = new Utilities.IO.Messaging.Default.DefaultFormatter();
            var TestMessage = new Utilities.IO.EmailMessage { Body = "There are {A} items in the {B}" };
            TestObject.Format(TestMessage, new TestClass { A = 2, B = "class" });
            Assert.Equal("There are 2 items in the class", TestMessage.Body);
            TestObject.Format(TestMessage, "Testing this out");
            Assert.Equal("Testing this out", TestMessage.Body);
        }

        public class TestClass
        {
            public int A { get; set; }

            public string B { get; set; }
        }
    }
}