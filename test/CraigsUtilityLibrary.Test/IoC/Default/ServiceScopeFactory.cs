using System.Reflection;
using Xunit;

namespace Utilities.Test.IoC.Default
{
    public class ServiceScopeFactory
    {
        [Fact]
        public void CreateScope()
        {
            var Bootstrapper = GetBootstrapper();
            var TestObject = new Utilities.IoC.Default.ServiceScopeFactory(Bootstrapper);
            var Scope = TestObject.CreateScope();
            Assert.NotNull(Scope);
            Assert.NotSame(Scope, Bootstrapper);
        }

        [Fact]
        public void Creation()
        {
            var TestObject = new Utilities.IoC.Default.ServiceScopeFactory(GetBootstrapper());
            Assert.NotNull(TestObject);
        }

        private Utilities.IoC.Default.DefaultBootstrapper GetBootstrapper()
        {
            return new Utilities.IoC.Default.DefaultBootstrapper(new Assembly[] { typeof(DefaultBootstrapper).GetTypeInfo().Assembly }, typeof(DefaultBootstrapper).GetTypeInfo().Assembly.GetTypes());
        }
    }
}