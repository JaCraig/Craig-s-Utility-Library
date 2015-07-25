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
using System.Net.Mail;
using System.Runtime.Serialization;
using Xunit;

namespace UnitTests.IO
{
    public class EmailMessage
    {
        [Fact]
        public void Creation()
        {
            using (Utilities.IO.EmailMessage Message = new Utilities.IO.EmailMessage())
            {
                Assert.Equal(0, Message.Attachments.Count);
                Assert.True(string.IsNullOrEmpty(Message.Bcc));
                Assert.True(string.IsNullOrEmpty(Message.Body));
                Assert.True(string.IsNullOrEmpty(Message.CC));
                Assert.Equal(0, Message.EmbeddedResources.Count);
                Assert.True(string.IsNullOrEmpty(Message.From));
                Assert.True(string.IsNullOrEmpty(Message.Password));
                Assert.Equal(25, Message.Port);
                Assert.Equal(MailPriority.Normal, Message.Priority);
                Assert.True(string.IsNullOrEmpty(Message.Server));
                Assert.True(string.IsNullOrEmpty(Message.Subject));
                Assert.True(string.IsNullOrEmpty(Message.To));
                Assert.True(string.IsNullOrEmpty(Message.UserName));
                Assert.False(Message.UseSSL);
            }
        }

        [Fact]
        public void SendWithoutServer()
        {
            using (Utilities.IO.EmailMessage Message = new Utilities.IO.EmailMessage())
            {
                Assert.Throws<AggregateException>(() => Message.Send(new Temp()).Wait());
                Assert.Throws<AggregateException>(() => Message.Send().Wait());
            }
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