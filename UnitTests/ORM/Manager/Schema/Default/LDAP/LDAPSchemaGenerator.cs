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

using Utilities.ORM.Manager.Schema.Interfaces;
using Xunit;

namespace UnitTests.ORM.Manager.Schema.Default.LDAP
{
    public class LDAPSchemaGenerator : DatabaseBaseClass
    {
        [Fact]
        public void Create()
        {
            var Temp = new Utilities.ORM.Manager.Schema.Default.LDAP.LDAPSchemaGenerator(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.QueryProvider.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.SourceProvider.Manager>());
            Assert.Equal("LDAP", Temp.ProviderName);
        }

        [Fact]
        public void GenerateSchema()
        {
            var Temp = new Utilities.ORM.Manager.Schema.Default.LDAP.LDAPSchemaGenerator(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.QueryProvider.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.SourceProvider.Manager>());
            ISource Source = Temp.GetSourceStructure(TestDatabaseSource);
            Assert.Null(Source);
        }

        [Fact]
        public void SourceExists()
        {
            var Temp = new Utilities.ORM.Manager.Schema.Default.LDAP.LDAPSchemaGenerator(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.QueryProvider.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.SourceProvider.Manager>());
            Assert.True(Temp.SourceExists("TestDatabase", DatabaseSource));
        }

        [Fact]
        public void TableExists()
        {
            var Temp = new Utilities.ORM.Manager.Schema.Default.LDAP.LDAPSchemaGenerator(Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.QueryProvider.Manager>(), Utilities.IoC.Manager.Bootstrapper.Resolve<Utilities.ORM.Manager.SourceProvider.Manager>());
            Assert.False(Temp.TableExists("TestTable", TestDatabaseSource));
        }
    }
}