using System;
using Xunit;

namespace Utilities.Test.IoC.Default.TypeBuilders
{
    public class SingletonTypeBuilder
    {
        private static int DisposedCount = 0;

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        [InlineData(4332)]
        [InlineData(123)]
        public void Copy(int value)
        {
            using (var Temp = new Utilities.IoC.Default.TypeBuilders.SingletonTypeBuilder<int>(x => value))
            {
                using (var Temp2 = Temp.Copy())
                {
                    Assert.NotNull(Temp2);
                    Assert.Equal(typeof(int), Temp2.ReturnType);
                    Assert.Equal(value, Temp2.Create(null));
                    Assert.Same(Temp, Temp2);
                }
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        [InlineData(4332)]
        [InlineData(123)]
        public void Creation(int value)
        {
            using (var Temp = new Utilities.IoC.Default.TypeBuilders.SingletonTypeBuilder<int>(x => value))
            {
                Assert.NotNull(Temp);
                Assert.Equal(typeof(int), Temp.ReturnType);
                Assert.Equal(value, Temp.Create(null));
            }
        }

        [Fact]
        public void ScopeTest()
        {
            using (var Temp = new Utilities.IoC.Default.TypeBuilders.SingletonTypeBuilder<SingletonTestClass>(x => new SingletonTestClass()))
            {
                Assert.NotNull(Temp);
                Assert.Equal(typeof(SingletonTestClass), Temp.ReturnType);
                var Value = Temp.Create(null);
                Assert.IsType(typeof(SingletonTestClass), Value);
                var Value2 = Temp.Create(null);
                Assert.Same(Value, Value2);
            }
            Assert.Equal(1, DisposedCount);
        }

        private class SingletonTestClass : IDisposable
        {
            public void Dispose()
            {
                ++DisposedCount;
            }
        }
    }
}