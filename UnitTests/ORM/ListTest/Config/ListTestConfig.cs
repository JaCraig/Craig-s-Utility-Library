using Utilities.ORM.QueryProviders.Interfaces;

namespace UnitTests.ORM.ListTest.Config
{
    public class ListTestConfig : IDatabase
    {
        public string Name
        {
            get { return "List Test Configuration"; }
        }

        public string ConnectionString
        {
            get { return "Data Source=localhost;Initial Catalog=ORMTestDatabase3;Integrated Security=SSPI;Pooling=false"; }
        }

        public string ParameterStarter
        {
            get { return "@"; }
        }

        public bool Audit
        {
            get { return true; }
        }

        public bool Update
        {
            get { return true; }
        }

        public bool Writable
        {
            get { return true; }
        }

        public bool Readable
        {
            get { return true; }
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
