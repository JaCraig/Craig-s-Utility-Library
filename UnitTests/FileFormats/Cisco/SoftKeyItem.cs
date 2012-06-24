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
using Utilities.Random.ExtensionMethods;
using Utilities.Random.StringGenerators;

namespace UnitTests.FileFormats.Cisco
{
    public class SoftKeyItem
    {
        protected Utilities.Cisco.SoftKeyItem Entry = null;
        protected Utilities.Random.Random Random = null;

        public SoftKeyItem()
        {
            Entry = new Utilities.Cisco.SoftKeyItem();
            Random = new Utilities.Random.Random();
        }

        [Test]
        public void NullTest()
        {
            Entry.Name = null;
            Entry.Position = 0;
            Entry.URL = null;
            Entry.URLDown = null;
            Assert.NotEmpty(Entry.ToString());
        }

        [Test]
        public void RandomTest()
        {
            Entry.Name = Random.Next<string>(new RegexStringGenerator(30));
            Entry.URL = Random.Next<string>(new RegexStringGenerator(30));
            Entry.URLDown = Random.Next<string>(new RegexStringGenerator(30));
            Entry.Position = Random.Next(int.MinValue, int.MaxValue);
            Assert.Contains(Entry.Name, Entry.ToString());
            Assert.Contains(Entry.URLDown, Entry.ToString());
            Assert.Contains(Entry.URL, Entry.ToString());
            Assert.Contains(Entry.Position.ToString(), Entry.ToString());
        }
    }
}
