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
using System.Linq;
using System.Runtime.Serialization;
using Xunit;

namespace UnitTests.IO.Messaging.Default
{
    public class SMTPSystem
    {
        [Fact]
        public void Creation()
        {
            var Temp = new Utilities.IO.Messaging.Default.SMTPSystem();
            Assert.NotNull(Temp);
            Assert.Equal(0, Temp.Formatters.Count());
            Assert.Equal(typeof(Utilities.IO.EmailMessage), Temp.MessageType);
            Assert.Equal("SMTP", Temp.Name);
        }

        [Fact]
        public void NullSend()
        {
            var Temp = new Utilities.IO.Messaging.Default.SMTPSystem();
            Assert.NotNull(Temp);
            Temp.Send(null, new Temp()).Wait();
        }

        [Serializable]
        [DataContract]
        protected class Temp
        {
            [DataMember(Name = "A", Order = 1)]
            public int A { get; set; }
        }
    }
}