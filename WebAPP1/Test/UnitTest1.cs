using System;
using Xunit;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            P1 p = new P1();
            int count = p.Sum(1, 2);
            Assert.True(count == 3);
        }

        [Theory]
        [InlineData(2, 3, 5)]
        [InlineData(2, 4, 6)]
        [InlineData(2, 3, 7)]
        public void Add_OK_Two(int nb1, int nb2, int result)
        {
            P1 p = new P1();
            var count = p.Sum(nb1, nb2);
            Assert.True(count == result);
        }
    }
}
