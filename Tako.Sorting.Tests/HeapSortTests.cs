using Xunit;

namespace Tako.Sorting.Tests
{
    public class HeapSortTests
    {
        [Fact()]
        public void SortTest()
        {
            Assert.True(SortTestHelper.SortTest(new HeapSort<int>()));
        }
    }
}
