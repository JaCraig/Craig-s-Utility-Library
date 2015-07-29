using Xunit;

namespace UnitTests.IoC.Default
{
    public class TypeBuilder
    {
        [Fact]
        public void Creation()
        {
            var Temp = new Utilities.IoC.Default.TypeBuilder<int>(() => 12);
            Assert.NotNull(Temp);
            Assert.Equal(typeof(int), Temp.ReturnType);
            Assert.Equal(12, Temp.Create());
        }
    }
}