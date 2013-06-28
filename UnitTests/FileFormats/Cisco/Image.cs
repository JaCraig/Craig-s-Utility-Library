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


using Utilities.Random.ExtensionMethods;
using Utilities.Random.StringGenerators;
using Xunit;

namespace UnitTests.FileFormats.Cisco
{
    public class Image
    {
        protected Utilities.FileFormats.Cisco.Image Entry = null;
        protected Utilities.Random.Random Random = null;

        public Image()
        {
            Entry = new Utilities.FileFormats.Cisco.Image();
            Random = new Utilities.Random.Random();
        }

        [Fact]
        public void NullTest()
        {
            Entry.Prompt = null;
            Entry.Title = null;
            Entry.URL = null;
            Entry.X = 0;
            Entry.Y = 0;
            Assert.NotEmpty(Entry.ToString());
        }

        [Fact]
        public void NullItemTest()
        {
            Entry.SoftKeys.Add(null);
            Assert.NotEmpty(Entry.ToString());
        }

        [Fact]
        public void RandomTest()
        {
            Entry.Prompt = Random.Next<string>(new RegexStringGenerator(30));
            Entry.Title = Random.Next<string>(new RegexStringGenerator(30));
            Entry.URL = Random.Next<string>(new RegexStringGenerator(30));
            Entry.X = Random.Next(int.MinValue, int.MaxValue);
            Entry.Y = Random.Next(int.MinValue, int.MaxValue);
            Assert.Contains(Entry.Prompt, Entry.ToString());
            Assert.Contains(Entry.URL, Entry.ToString());
            Assert.Contains(Entry.Title, Entry.ToString());
            Assert.Contains(Entry.X.ToString(), Entry.ToString());
            Assert.Contains(Entry.Y.ToString(), Entry.ToString());
        }
    }
}
