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
using Utilities.ORM.BaseClasses;
using Utilities.ORM.Interfaces;
using Utilities.ORM.Manager.Mapper.Interfaces;
using Utilities.ORM.Manager.QueryProvider.Interfaces;
using Xunit;

namespace UnitTests.ORM.Manager.QueryProvider.Default.LDAP
{
    public class LDAPGenerator : DatabaseBaseClass
    {
        public LDAPGenerator()
        {
            var Temp = new Utilities.ORM.Manager.Mapper.Manager(Utilities.IoC.Manager.Bootstrapper.ResolveAll<IMapping>());
            QueryProvider = new Utilities.ORM.Manager.QueryProvider.Default.LDAP.LDAPQueryProvider();
            Generator = new Utilities.ORM.Manager.QueryProvider.Default.LDAP.LDAPGenerator<Dynamo>(QueryProvider, LDAPSource, Temp[typeof(Dynamo), LDAPSource]);
        }

        private Utilities.ORM.Manager.QueryProvider.Default.LDAP.LDAPGenerator<Dynamo> Generator { get; set; }

        private Utilities.ORM.Manager.QueryProvider.Default.LDAP.LDAPQueryProvider QueryProvider { get; set; }

        [Fact]
        public void All()
        {
            IBatch Batch = Generator.All();
            Assert.Equal(1, Batch.CommandCount);
        }

        [Fact]
        public void Any()
        {
            IBatch Batch = Generator.Any();
            Assert.Equal(1, Batch.CommandCount);
        }

        [Fact]
        public void Delete()
        {
            IBatch Batch = Generator.Delete(new Dynamo());
            Assert.Equal(0, Batch.CommandCount);
        }

        [Fact]
        public void Delete2()
        {
            IBatch Batch = Generator.Delete(new Dynamo[] { new Dynamo(), new Dynamo(), new Dynamo() });
            Assert.Equal(0, Batch.CommandCount);
        }

        [Fact]
        public void Insert()
        {
            IBatch Batch = Generator.Insert(new Dynamo());
            Assert.Equal(0, Batch.CommandCount);
        }

        [Fact]
        public void PageCount()
        {
            IBatch Batch = Generator.PageCount(25);
            Assert.Equal(0, Batch.CommandCount);
        }

        [Fact]
        public void Paged()
        {
            IBatch Batch = Generator.Paged(25, 0, "");
            Assert.Equal(1, Batch.CommandCount);
        }

        [Fact]
        public void Update()
        {
            IBatch Batch = Generator.Update(new Dynamo());
            Assert.Equal(0, Batch.CommandCount);
        }

        [Fact]
        public void Update2()
        {
            IBatch Batch = Generator.Update(new Dynamo[] { new Dynamo(), new Dynamo(), new Dynamo() });
            Assert.Equal(0, Batch.CommandCount);
        }

        public class DynamoDatabase : IDatabase
        {
            public bool Audit
            {
                get { return false; }
            }

            public string Name
            {
                get { return "LDAP"; }
            }

            public int Order
            {
                get { return 0; }
            }

            public bool Readable
            {
                get { return true; }
            }

            public bool Update
            {
                get { return false; }
            }

            public bool Writable
            {
                get { return false; }
            }
        }

        public class DynamoMapping : MappingBaseClass<Dynamo, DynamoDatabase>
        {
            public DynamoMapping()
            {
                ID(x => x.Count).SetFieldName("Count").SetAutoIncrement();
            }
        }
    }
}