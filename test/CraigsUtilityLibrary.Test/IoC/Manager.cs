using System.Reflection;
using Utilities.IoC.Default;
using Utilities.IoC.Interfaces;
using Xunit;

namespace Utilities.Test.IoC
{
    public class Manager
    {
        [Fact]
        public void Creation()
        {
            using (var TempManager = new Utilities.IoC.Manager(new Assembly[] { typeof(Manager).GetTypeInfo().Assembly }))
            {
                using (IBootstrapper Temp = TempManager.Bootstrapper)
                {
                    Assert.NotNull(Temp);
                    Assert.IsType(typeof(DefaultBootstrapper), Temp);
                    Assert.Equal("Default bootstrapper", Temp.Name);
                    Temp.ToString();
                    Assert.Equal("Default bootstrapper", TempManager.ToString());
                }
            }
        }

        [Fact]
        public void Destructor()
        {
            var TempManager = new Utilities.IoC.Manager(new Assembly[] { typeof(Manager).GetTypeInfo().Assembly });
        }

        public class TestModule : IModule
        {
            public int Order => 1;

            public void Load(IBootstrapper bootstrapper)
            {
            }
        }
    }
}