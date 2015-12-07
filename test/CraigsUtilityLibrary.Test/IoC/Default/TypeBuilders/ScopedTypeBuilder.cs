using System;
using Xunit;

namespace Utilities.Test.IoC.Default.TypeBuilders
{
    public class ScopedTypeBuilder
    {
        private static int DisposedCount = 0;

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        [InlineData(4332)]
        [InlineData(123)]
        public void Copy(int value)
        {
            using (var Temp = new Utilities.IoC.Default.TypeBuilders.ScopedTypeBuilder<int>(x => value))
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

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        [InlineData(4332)]
        [InlineData(123)]
        public void Creation(int value)
        {
            using (var Temp = new Utilities.IoC.Default.TypeBuilders.ScopedTypeBuilder<int>(x => value))
            {
                Assert.NotNull(Temp);
                Assert.Equal(typeof(int), Temp.ReturnType);
                Assert.Equal(value, Temp.Create(null));
            }
        }

        [Fact]
        public void ScopeTest()
        {
            using (var Temp = new Utilities.IoC.Default.TypeBuilders.ScopedTypeBuilder<ScopedTestClass>(x => new ScopedTestClass()))
            {
                Assert.NotNull(Temp);
                Assert.Equal(typeof(ScopedTestClass), Temp.ReturnType);
                var Value = Temp.Create(null);
                Assert.IsType(typeof(ScopedTestClass), Value);
                var Value2 = Temp.Create(null);
                Assert.Same(Value, Value2);
            }
            Assert.Equal(1, DisposedCount);
        }

        private class ScopedTestClass : IDisposable
        {
            public void Dispose()
            {
                ++DisposedCount;
            }
        }
    }
}