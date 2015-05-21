using System;
using Xunit;

namespace Tako.Collections.Generic.Tests
{
    public class Llrb23TreeListTests
    {
        private Random random = new Random();
        private int count = 100;
        private string[] testStrings;

        public Llrb23TreeListTests()
        {
            this.testStrings = new string[this.count];

            for (int i = 0; i < this.count; i++)
            {
                this.testStrings[i] = "Test String " + this.random.Next();
            }
        }

        [Fact()]
        public void AddTest()
        {
            Llrb23TreeList<string> tree = this.InitializeTree();

            Assert.Equal(this.count, tree.Count);

            for (int i = 0; i < this.count; i++)
            {
                Assert.Equal(this.testStrings[i], tree[i]);
            }
        }

        [Fact()]
        void InsertTest()
        {
            Llrb23TreeList<string> tree = this.InitializeTree();
            string inserted = "Inserted String";

            for (int i = 0; i < this.count; i++)
            {
                tree.Insert(i * 2, inserted);
            }

            for (int i = 0; i < this.count; i++)
            {
                int j = i * 2;

                Assert.Equal(inserted, tree[j]);
                Assert.Equal(this.testStrings[i], tree[j + 1]);
            }
        }

        [Fact()]
        public void ContainsTest()
        {
            Llrb23TreeList<string> tree = this.InitializeTree();

            for (int i = 0; i < this.count; i++)
            {
                Assert.Equal(true, tree.Contains(this.testStrings[i]));
            }

            Assert.Equal(false, tree.Contains("Not Contained String"));
        }

        [Fact()]
        void IndexOfTest()
        {
            Llrb23TreeList<string> tree = this.InitializeTree();

            for (int i = 0; i < this.count; i++)
            {
                Assert.Equal(i, tree.IndexOf(this.testStrings[i]));
            }
        }

        [Fact()]
        public void RemoveAtTest()
        {
            Llrb23TreeList<string> tree = this.InitializeTree();

            for (int i = 0; i < this.count / 2; i++)
            {
                tree.RemoveAt(i);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => tree.RemoveAt(tree.Count));
            Assert.Equal(this.count / 2, tree.Count);

            for (int i = 0; i < this.count / 2; i++)
            {
                Assert.Equal(this.testStrings[i * 2 + 1], tree[i]);
            }
        }

        [Fact()]
        public void RemoveTest()
        {
            Llrb23TreeList<string> tree = this.InitializeTree();

            for (int i = 0; i < this.count; i += 2)
            {
                tree.Remove(this.testStrings[i]);
            }

            Assert.Throws<ArgumentException>(() => tree.Remove(this.testStrings[0]));
            Assert.Equal(this.count / 2, tree.Count);

            for (int i = 0; i < this.count / 2; i++)
            {
                Assert.Equal(this.testStrings[i * 2 + 1], tree[i]);
            }
        }

        [Fact()]
        public void CopyToTest()
        {
            Llrb23TreeList<string> tree = this.InitializeTree();
            string[] values = new string[this.count];

            tree.CopyTo(values, 0);

            for (int i = 0; i < this.count; i++)
            {
                Assert.Equal(this.testStrings[i], values[i]);
            }
        }

        [Fact()]
        public void ClearTest()
        {
            Llrb23TreeList<string> tree = this.InitializeTree();

            tree.Clear();

            Assert.Equal(0, tree.Count);

            for (int i = 0; i < this.count; i++)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => tree[i]);
            }
        }

        [Fact()]
        public void GetEnumeratorTest()
        {
            Llrb23TreeList<string> tree = this.InitializeTree();
            int i = 0;

            foreach (string value in tree)
            {
                Assert.Equal(this.testStrings[i], value);
                i++;
            }
        }

        [Fact()]
        public void SetTest()
        {
            Llrb23TreeList<string> tree = this.InitializeTree();
            string changed = "Changed";

            for (int i = 0; i < this.count; i++)
            {
                tree[i] = changed;
                Assert.Equal(changed, tree[i]);
            }
        }

        private Llrb23TreeList<string> InitializeTree()
        {
            Llrb23TreeList<string> tree = new Llrb23TreeList<string>();

            for (int i = 0; i < this.count; i++)
            {
                tree.Add(this.testStrings[i]);
            }

            return tree;
        }
    }
}
