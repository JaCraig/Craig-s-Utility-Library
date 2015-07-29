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

using Utilities.DataTypes;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Xunit;

namespace UnitTests.ORM.Manager.QueryProvider.Default.LDAP
{
    public class LDAPQueryProvider : DatabaseBaseClass
    {
        [Fact]
        public void Batch()
        {
            var Temp = new Utilities.ORM.Manager.QueryProvider.Default.LDAP.LDAPQueryProvider();
            IBatch Batch = Temp.Batch(TestDatabaseSource);
            Assert.Equal(0, Batch.CommandCount);
            Assert.Equal(typeof(Utilities.ORM.Manager.QueryProvider.Default.LDAP.LDAPBatch), Batch.GetType());
        }

        [Fact]
        public void Create()
        {
            var Temp = new Utilities.ORM.Manager.QueryProvider.Default.LDAP.LDAPQueryProvider();
            Assert.NotNull(Temp);
            Assert.Equal("LDAP", Temp.ProviderName);
        }

        [Fact]
        public void Generate()
        {
            var Temp = new Utilities.ORM.Manager.QueryProvider.Default.LDAP.LDAPQueryProvider();
            IGenerator<Dynamo> Generator = Temp.Generate<Dynamo>(TestDatabaseSource, new Utilities.ORM.Manager.Mapper.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IMapping>())[typeof(Dynamo), TestDatabaseSource]);
            Assert.Equal(typeof(Utilities.ORM.Manager.QueryProvider.Default.LDAP.LDAPGenerator<Dynamo>), Generator.GetType());
        }
    }
}