/*
Copyright (c) 2011 <a href="http://www.gutgames.com">James Craig</a>

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
using System.IO;
using MoonUnit;
using MoonUnit.Attributes;
using Utilities.IO.Logging.Enums;
using Utilities.IO.ExtensionMethods;

namespace UnitTests.IO.Logging
{
    public class FileLog : IDisposable
    {
        public FileLog() { Log = new Utilities.IO.Logging.FileLog(@".\Test\File.txt"); }

        [Test]
        public void LogMessage()
        {
            foreach (MessageType Type in Enum.GetValues(typeof(MessageType)))
                Assert.DoesNotThrow<Exception>(() => Log.LogMessage("TestMessage", Type));
            Assert.Contains("\r\nGeneral: TestMessage\r\nDebug: TestMessage\r\nTrace: TestMessage\r\nInfo: TestMessage\r\nWarn: TestMessage\r\nError: TestMessage\r\n", new FileInfo(@".\Test\File.txt").Read());
        }


        public void Dispose() { Log.Dispose(); new DirectoryInfo(@".\Test").DeleteAll(); }

        private Utilities.IO.Logging.FileLog Log { get; set; }
    }
}