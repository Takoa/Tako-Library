using System;
using System.Collections.Generic;
using Xunit;

namespace Tako.Collections.Generic.Tests
{
    public class SortedLlrb23TreeListTests
    {
        private int count = 100;
        private Random random = new Random();
        private int[] testInts;
        private SortedSet<int> sortedSet = new SortedSet<int>();

        public SortedLlrb23TreeListTests()
        {
            this.testInts = new int[this.count];

            for (int i = 0; i < this.count; i++)
            {
                this.testInts[i] = this.random.Next();
                this.sortedSet.Add(this.testInts[i]);
            }
        }

        [Fact()]
        public void SortedLlrb23TreeListTest()
        {
            SortedLlrb23TreeListTests.CompareWithSortedSet(this.InitializeTree(), this.sortedSet);
        }

        [Fact()]
        public void SortedLlrb23TreeListTest1()
        {
            TestComparer comparer = new TestComparer();
            SortedSet<int> sortedSet = new SortedSet<int>(comparer);
            SortedLlrb23TreeList<int> tree = new SortedLlrb23TreeList<int>(comparer);

            for (int i = 0; i < this.count; i++)
            {
                sortedSet.Add(this.testInts[i]);
                tree.Add(this.testInts[i]);
            }

            SortedLlrb23TreeListTests.CompareWithSortedSet(tree, sortedSet);
        }

        [Fact()]
        public void AddTest()
        {
            SortedLlrb23TreeList<int> tree = this.InitializeTree();
            int i = 0;

            foreach (int item in this.sortedSet)
            {
                Assert.Equal(item, tree[i++]);
            }

            Assert.Throws<ArgumentException>(() => tree.Add(testInts[0]));
        }

        [Fact()]
        public void ContainsTest()
        {
            SortedLlrb23TreeList<int> tree = this.InitializeTree();

            foreach (int item in this.sortedSet)
            {
                Assert.Equal(true, tree.Contains(item));
            }

            Assert.Equal(false, tree.Contains(-1));
        }

        [Fact()]
        public void IndexOfTest()
        {
            SortedLlrb23TreeList<int> tree = this.InitializeTree();
            int i = 0;

            foreach (int item in this.sortedSet)
            {
                Assert.Equal(i++, tree.IndexOf(item));
            }

            Assert.InRange(tree.IndexOf(-1), int.MinValue, -1);
        }

        [Fact()]
        public void ClearTest()
        {
            SortedLlrb23TreeList<int> tree = this.InitializeTree();

            tree.Clear();

            Assert.Equal(0, tree.Count);

            foreach (int item in this.sortedSet)
            {
                Assert.Equal(false, tree.Contains(item));
            }
        }

        [Fact()]
        public void RemoveAtTest()
        {
            SortedLlrb23TreeList<int> tree = this.InitializeTree();
            int i = 0;

            for (int j = 0; j < this.count / 2; j++)
            {
                tree.RemoveAt(j);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => tree.RemoveAt(tree.Count));
            Assert.Equal(this.count / 2, tree.Count);

            foreach (int item in this.sortedSet)
            {
                if (i % 2 != 0)
                {
                    Assert.Equal(item, tree[(i - 1) / 2]);
                }

                i++;
            }
        }

        [Fact()]
        public void RemoveTest()
        {
            SortedLlrb23TreeList<int> tree = this.InitializeTree();
            int i = 0;

            foreach (int item in this.sortedSet)
            {
                if (i % 2 == 0)
                {
                    tree.Remove(item);
                }

                i++;
            }

            Assert.Throws<ArgumentException>(() => tree.Remove(-1));
            Assert.Equal(this.count / 2, tree.Count);

            i = 0;
            foreach (int item in this.sortedSet)
            {
                if (i % 2 != 0)
                {
                    Assert.Equal(item, tree[(i - 1) / 2]);
                }

                i++;
            }
        }

        [Fact()]
        public void CopyToTest()
        {
            SortedLlrb23TreeList<int> tree = this.InitializeTree();
            int[] values = new int[this.count];
            int i = 0;

            tree.CopyTo(values, 0);

            foreach (int item in this.sortedSet)
            {
                Assert.Equal(item, values[i++]);
            }
        }

        [Fact()]
        public void GetEnumeratorTest()
        {
            SortedLlrb23TreeList<int> tree = this.InitializeTree();
            IEnumerator<int> sortedSetEnumerator = this.sortedSet.GetEnumerator();
            IEnumerator<int> treeEnumerator = tree.GetEnumerator();

            while (sortedSetEnumerator.MoveNext() && treeEnumerator.MoveNext())
            {
                Assert.Equal(sortedSetEnumerator.Current, treeEnumerator.Current);
            }
        }

        [Fact]
        public void InsertTest()
        {
            IList<int> tree = this.InitializeTree();

            Assert.Throws<NotSupportedException>(() => tree.Insert(0, 0));
        }

        [Fact]
        public void SetTest()
        {
            IList<int> tree = this.InitializeTree();

            Assert.Throws<NotSupportedException>(() => tree[0] = 0);
        }

        private SortedLlrb23TreeList<int> InitializeTree()
        {
            SortedLlrb23TreeList<int> tree = new SortedLlrb23TreeList<int>();

            for (int i = 0; i < this.count; i++)
            {
                tree.Add(this.testInts[i]);
            }

            return tree;
        }

        private static void CompareWithSortedSet(SortedLlrb23TreeList<int> sortedLlrb23Tree, SortedSet<int> sortedSet)
        {
            int i = 0;

            foreach (int item in sortedSet)
            {
                Assert.Equal(true, sortedLlrb23Tree[i] == item);
                i++;
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
