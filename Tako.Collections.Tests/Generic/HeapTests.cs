using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tako.Collections.Generic.Tests
{
    public class HeapTests
    {
        private int count = 100;
        private readonly int[] testInts;
        private Random random = new Random();

        public HeapTests()
        {
            this.testInts = new int[this.count];

            for (int i = 0; i < this.count; i++)
            {
                this.testInts[i] = this.random.Next();
            }
        }

        [Fact()]
        public void GetTest()
        {
            Heap<int> heap = new MaxHeap<int>(this.testInts, true);

            Assert.Throws<IndexOutOfRangeException>(() => heap[-1]);
            Assert.Throws<IndexOutOfRangeException>(() => heap[this.count]);
        }

        [Fact()]
        public void LeftParent()
        {
            Heap<int> heap = new MaxHeap<int>(this.testInts, true);

            Assert.Throws<IndexOutOfRangeException>(() => heap.GetParent(-1));
            Assert.Throws<IndexOutOfRangeException>(() => heap.GetParent(this.count));
        }

        [Fact()]
        public void LeftTest()
        {
            Heap<int> heap = new MaxHeap<int>(this.testInts, true);

            Assert.Throws<IndexOutOfRangeException>(() => heap.GetLeft(-1));
            Assert.Throws<IndexOutOfRangeException>(() => heap.GetLeft(this.count));
        }

        [Fact()]
        public void RightTest()
        {
            Heap<int> heap = new MaxHeap<int>(this.testInts, true);

            Assert.Throws<IndexOutOfRangeException>(() => heap.GetRight(-1));
            Assert.Throws<IndexOutOfRangeException>(() => heap.GetRight(this.count));
        }

        [Fact()]
        public void ExtractRootTest()
        {
            Heap<int> heap = new MaxHeap<int>(this.testInts, true);
            int[] sortedInts = new int[this.count];

            this.testInts.CopyTo(sortedInts, 0);
            Array.Sort(sortedInts);

            for (int i = this.count - 1; 0 <= i; i--)
            {
                Assert.True(sortedInts[i] == heap.ExtractRoot());
            }

            Assert.True(heap.ExtractRoot() == 0);
        }

        [Fact()]
        public void ClearTest()
        {
            Heap<int> heap = new MaxHeap<int>(this.testInts, true);

            ((ICollection<int>)heap).Clear();

            Assert.Throws<IndexOutOfRangeException>(() => heap[0]);
            Assert.True(heap.ExtractRoot() == 0);
        }

        [Fact()]
        public void ContainsTest()
        {
            ICollection<int> heap = new MaxHeap<int>(this.testInts, true);

            for (int i = this.count - 1; 0 <= i; i--)
            {
                Assert.True(heap.Contains(this.testInts[i]));
            }

            Assert.False(heap.Contains(-1));
        }

        [Fact()]
        public void CopyToTest()
        {
            ICollection<int> heap = new MaxHeap<int>(this.testInts, true);
            int[] copy = new int[this.count];

            heap.CopyTo(copy, 0);

            for (int i = 0; i < this.count; i++)
            {
                Assert.True(copy[i] == ((MaxHeap<int>)heap)[i]);
            }

            Assert.Throws<ArgumentNullException>(() => heap.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => heap.CopyTo(copy, -1));
            Assert.Throws<ArgumentException>(() => heap.CopyTo(copy, 1));
        }

        [Fact()]
        public void GetEnumeratorTest()
        {
            MaxHeap<int> heap = new MaxHeap<int>(this.testInts, true);
            int i = 0;

            foreach (int item in (ICollection<int>)heap)
            {
                Assert.True(item == heap[i++]);
            }
        }
    }
}
