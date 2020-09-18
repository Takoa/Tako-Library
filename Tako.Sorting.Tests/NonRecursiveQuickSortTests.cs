using Xunit;

namespace Tako.Sorting.Tests
{
    public class NonRecursiveQuickSortTests
    {
        [Fact()]
        public void SortTest()
        {
            Assert.True(SortTestHelper.SortTest(new NonRecursiveQuickSort<int>()));
        }
    }
}
