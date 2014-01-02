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

using System.Collections.Generic;
using Utilities.DataTypes;
using Xunit;

namespace UnitTests.DataTypes.Caching.Default
{
    public class Cache
    {
        [Fact]
        public void Create()
        {
            Utilities.DataTypes.Caching.Default.Cache Temp = new Utilities.DataTypes.Caching.Default.Cache();
            Assert.NotNull(Temp);
            Assert.Equal(0, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(0, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(0, Temp.Values.Count);
        }

        [Fact]
        public void Add()
        {
            Utilities.DataTypes.Caching.Default.Cache Temp = new Utilities.DataTypes.Caching.Default.Cache();
            Temp.Add("A", 1);
            Assert.NotNull(Temp);
            Assert.Equal(1, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(1, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(1, Temp.Values.Count);
            Assert.Equal(1, Temp["A"]);
            Assert.True(Temp.ContainsKey("A"));
            Assert.True(Temp.Contains(new KeyValuePair<string,object>("A",1)));
        }

        [Fact]
        public void Remove()
        {
            Utilities.DataTypes.Caching.Default.Cache Temp = new Utilities.DataTypes.Caching.Default.Cache();
            Temp.Add("A", 1);
            Assert.NotNull(Temp);
            Assert.Equal(1, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(1, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(1, Temp.Values.Count);
            Assert.Equal(1, Temp["A"]);
            Assert.True(Temp.Remove("A"));
            Assert.Equal(0, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(0, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(0, Temp.Values.Count);
            Assert.Equal(null, Temp["A"]);
        }

        [Fact]
        public void TryGetValue()
        {
            Utilities.DataTypes.Caching.Default.Cache Temp = new Utilities.DataTypes.Caching.Default.Cache();
            Temp.Add("A", 1);
            Assert.NotNull(Temp);
            Assert.Equal(1, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(1, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(1, Temp.Values.Count);
            object Value=0;
            Assert.True(Temp.TryGetValue("A", out Value));
            Assert.Equal(1, Value);
        }

        [Fact]
        public void Clear()
        {
            Utilities.DataTypes.Caching.Default.Cache Temp = new Utilities.DataTypes.Caching.Default.Cache();
            Temp.Add("A", 1);
            Assert.NotNull(Temp);
            Assert.Equal(1, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(1, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(1, Temp.Values.Count);
            Temp.Clear();
            Assert.Equal(0, Temp.Count);
            Assert.False(Temp.IsReadOnly);
            Assert.Equal(0, Temp.Keys.Count);
            Assert.Equal("Default", Temp.Name);
            Assert.Equal(0, Temp.Values.Count);
        }
    }
}