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
using Utilities.DataTypes;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.Schema.Default.Database;
using Utilities.ORM.Manager.Schema.Interfaces;
using Utilities.ORM.Manager.SourceProvider.Interfaces;
using Xunit;

namespace UnitTests.ORM.Manager
{
    public class ORMManager : DatabaseBaseClass
    {
        [Fact]
        public void Create()
        {
            var Temp = new Utilities.ORM.Manager.ORMManager(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.Mapper.Manager>(),
                Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.QueryProvider.Manager>(),
                Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.Schema.Manager>(),
                Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.SourceProvider.Manager>(),
                Utilities.IoC.Manager.Bootstrapper.ResolveAll<IDatabase>());
            Assert.NotNull(Temp);
            Assert.Equal("ORM Manager\r\n", Temp.ToString());
        }
    }
}