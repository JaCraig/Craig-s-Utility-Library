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
using UnitTests.Fixtures;
using Utilities.IO.Logging.Enums;
using Xunit;

namespace UnitTests.IO.Logging.Default
{
    public class DefaultLogger : TestingDirectoryFixture
    {
        [Fact]
        public void Creation()
        {
            using (Utilities.IO.Logging.Default.DefaultLogger Logger = new Utilities.IO.Logging.Default.DefaultLogger())
            {
                Assert.NotNull(Logger);
                Assert.Equal(0, Logger.Logs.Count);
                Assert.Equal("Default Logger", Logger.Name);
            }
            new Utilities.IO.DirectoryInfo("~/Logs/").Delete();
        }

        [Fact]
        public void LogMessage()
        {
            using (Utilities.IO.Logging.Default.DefaultLogger Logger = new Utilities.IO.Logging.Default.DefaultLogger())
            {
                var File = (Utilities.IO.Logging.Default.DefaultLog)Logger.GetLog();
                Assert.Equal("Default", File.Name);
                foreach (MessageType Type in Enum.GetValues(typeof(MessageType)))
                    File.LogMessage("TestMessage", Type);
                Assert.Contains("\r\nGeneral: TestMessage\r\nDebug: TestMessage\r\nTrace: TestMessage\r\nInfo: TestMessage\r\nWarn: TestMessage\r\nError: TestMessage\r\n", new Utilities.IO.FileInfo(File.FileName).Read());
            }
            new Utilities.IO.DirectoryInfo("~/Logs/").Delete();
        }
    }
}