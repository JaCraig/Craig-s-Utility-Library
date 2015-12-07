using System;
using Xunit;

namespace Utilities.Test.IoC.Default.TypeBuilders
{
    public class TransientTypeBuilder
    {
        private static int DisposedCount = 0;

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        [InlineData(4332)]
        [InlineData(123)]
        public void Copy(int value)
        {
            using (var Temp = new Utilities.IoC.Default.TypeBuilders.TransientTypeBuilder<int>(x => value))
            {
                using (var Temp2 = Temp.Copy())
                {
                    Assert.NotNull(Temp2);
                    Assert.Equal(typeof(int), Temp2.ReturnType);
                    Assert.Equal(value, Temp2.Create(null));
                    Assert.NotSame(Temp, Temp2);
                }
            }
        }

        [Fact]
        public void ImplementationNotSupplied()
        {
            using (var Temp = new Utilities.IoC.Default.TypeBuilders.TransientTypeBuilder<int>(null))
            {
                Assert.NotNull(Temp);
                Assert.Equal(typeof(int), Temp.ReturnType);
                Assert.Equal(0, Temp.Create(null));
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        [InlineData(4332)]
        [InlineData(123)]
        public void Creation(int value)
        {
            using (var Temp = new Utilities.IoC.Default.TypeBuilders.TransientTypeBuilder<int>(x => value))
            {
                Assert.NotNull(Temp);
                Assert.Equal(typeof(int), Temp.ReturnType);
                Assert.Equal(value, Temp.Create(null));
            }
        }

        [Fact]
        public void ScopeTest()
        {
            using (var Temp = new Utilities.IoC.Default.TypeBuilders.TransientTypeBuilder<TransientTestClass>(x => new TransientTestClass()))
            {
                Assert.NotNull(Temp);
                Assert.Equal(typeof(TransientTestClass), Temp.ReturnType);
                var Value = Temp.Create(null);
                Assert.IsType(typeof(TransientTestClass), Value);
                var Value2 = Temp.Create(null);
                Assert.NotSame(Value, Value2);
            }
            Assert.Equal(0, DisposedCount);
        }

        private class TransientTestClass : IDisposable
        {
            public void Dispose()
            {
                ++DisposedCount;
            }
        }
    }
}