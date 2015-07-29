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
using System.Data;
using System.Linq;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.Schema.Default.Database;
using Utilities.ORM.Manager.Schema.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;
using Xunit;

namespace UnitTests.ORM.Manager.SourceProvider
{
    public class Manager
    {
        [Fact]
        public void Create()
        {
            var Temp = new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>());
            Assert.NotNull(Temp);
        }

        [Fact]
        public void GetSource()
        {
            var Temp = new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>());
            ISourceInfo Source = Temp.GetSource("Temp");
            Assert.Equal("Temp", Source.Connection);
            Assert.Equal("Temp", Source.Name);
            Assert.Equal("@", Source.ParameterPrefix);
            Assert.Equal(true, Source.Readable);
            Assert.Equal("System.Data.SqlClient", Source.SourceType);
            Assert.Equal(true, Source.Writable);
        }

        [Fact]
        public void GetSourceAgain()
        {
            var Temp = new Utilities.ORM.Manager.SourceProvider.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>());
            ISourceInfo Source = Temp.GetSource("Temp");
            ISourceInfo Source2 = Temp.GetSource("Temp");
            Assert.Equal(Source, Source2);
            Assert.Equal("Temp", Source.Connection);
            Assert.Equal("Temp", Source.Name);
            Assert.Equal("@", Source.ParameterPrefix);
            Assert.Equal(true, Source.Readable);
            Assert.Equal("System.Data.SqlClient", Source.SourceType);
            Assert.Equal(true, Source.Writable);
            Assert.Equal("Temp", Source2.Connection);
            Assert.Equal("Temp", Source2.Name);
            Assert.Equal("@", Source2.ParameterPrefix);
            Assert.Equal(true, Source2.Readable);
            Assert.Equal("System.Data.SqlClient", Source2.SourceType);
            Assert.Equal(true, Source2.Writable);
        }
    }
}