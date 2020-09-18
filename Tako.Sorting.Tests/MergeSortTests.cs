using Xunit;

namespace Tako.Sorting.Tests
{
    public class MergeSortTests
    {
        [Fact()]
        public void SortTest()
        {
            Assert.True(SortTestHelper.SortTest(new MergeSort<int>()));
        }
    }
}
