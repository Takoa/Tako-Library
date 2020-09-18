using Xunit;

namespace Tako.Sorting.Tests
{
    public class QuickSortTest
    {
        [Fact]
        public void SortTest()
        {
            Assert.True(SortTestHelper.SortTest(new QuickSort<int>()));
        }
    }
}
