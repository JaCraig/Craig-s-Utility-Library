using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.IoC.Default
{
    public class TypeBuilder
    {
        [Fact]
        public void Creation()
        {
            Utilities.IoC.Default.TypeBuilder<int> Temp = new Utilities.IoC.Default.TypeBuilder<int>(() => 12);
            Assert.NotNull(Temp);
            Assert.Equal(typeof(int), Temp.ReturnType);
            Assert.Equal(12, Temp.Create());
        }
    }
}
