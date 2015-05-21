using System;
using System.Collections.Generic;
using Xunit;

namespace Tako.Collections.Generic.Tests
{
    public class MaxHeapTests
    {
        private int count = 100;
        private readonly int[] testInts;
        private Random random = new Random();

        public MaxHeapTests()
        {
            this.testInts = new int[this.count];

            for (int i = 0; i < this.count; i++)
            {
                this.testInts[i] = this.random.Next();
            }
        }

        [Fact()]
        public void MaxHeapTest()
        {
            MaxHeap<int> heap = new MaxHeap<int>();

            Assert.True(heap.Count == 0);
            Assert.Throws<IndexOutOfRangeException>(() => heap[0]);
        }

        [Fact()]
        public void MaxHeapTest1()
        {
            MaxHeap<int> heap = new MaxHeap<int>(new TestComparer());

            for (int i = 0; i < heap.Count / 2; i++)
            {
                int left = heap.GetLeft(i);
                int right = heap.GetRight(i);

                if (left != 0)
                {
                    Assert.True(heap[i] < left);
                }

                if (right != 0)
                {
                    Assert.True(heap[i] < right);
                }
            }
        }

        [Fact()]
        public void MaxHeapTest2()
        {
            MaxHeap<int> heap = new MaxHeap<int>(this.testInts, true);

            for (int i = 0; i < heap.Count / 2; i++)
            {
                int left = heap.GetLeft(i);
                int right = heap.GetRight(i);

                if (left != 0)
                {
                    Assert.True(left < heap[i]);
                }

                if (right != 0)
                {
                    Assert.True(right < heap[i]);
                }
            }
        }

        [Fact()]
        public void MaxHeapTest3()
        {
            MaxHeap<int> heap = new MaxHeap<int>(this.testInts, 50, true, null);

            for (int i = 0; i < heap.Count / 2; i++)
            {
                int left = heap.GetLeft(i);
                int right = heap.GetRight(i);

                if (left != 0)
                {
                    Assert.True(left < heap[i]);
                }

                if (right != 0)
                {
                    Assert.True(right < heap[i]);
                }
            }
        }

        [Fact()]
        public void InsertTest()
        {
            MaxHeap<int> heap = new MaxHeap<int>(this.testInts, true);

            for (int i = 0; i < this.count; i++)
            {
                heap.Insert(this.random.Next());
            }

            for (int i = 0; i < heap.Count / 2; i++)
            {
                int left = heap.GetLeft(i);
                int right = heap.GetRight(i);

                if (left != 0)
                {
                    Assert.True(left < heap[i]);
                }

                if (right != 0)
                {
                    Assert.True(right < heap[i]);
                }
            }
        }

        private class TestComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                if (x < y)
                {
                    return 1;
                }
                else if (x == y)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}

