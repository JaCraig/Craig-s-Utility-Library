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
using MoonUnit.Attributes;
using MoonUnit;

namespace UnitTests.FileFormats.Cisco
{
    public class ExecuteItem
    {
        protected Utilities.Cisco.ExecuteItem Entry = null;
        protected Utilities.Random.Random Random = null;

        public ExecuteItem()
        {
            Entry = new Utilities.Cisco.ExecuteItem();
            Random = new Utilities.Random.Random();
        }

        [Test]
        public void NullTest()
        {
            Entry.Priority=0;
            Entry.URL=null;
            Assert.NotEmpty(Entry.ToString());
        }

        [Test]
        public void RandomTest()
        {
            Entry.Priority = Random.Next(int.MinValue,int.MaxValue);
            Entry.URL = Random.NextString(30);
            Assert.Contains(Entry.Priority.ToString(), Entry.ToString());
            Assert.Contains(Entry.URL, Entry.ToString());
        }
    }
}
