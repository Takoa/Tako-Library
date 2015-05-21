using System;
using System.Collections.Generic;
using Xunit;

namespace Tako.Collections.Generic.Tests
{
    public class Llrb23TreeTests
    {
        private Random random = new Random();
        private int count = 100;
        private SortedDictionary<int, string> dictionary = new SortedDictionary<int, string>();
        private int[] testInts;
        private string[] testStrings;

        public Llrb23TreeTests()
        {
            this.testInts = new int[this.count];
            this.testStrings = new string[this.count];

            for (int i = 0; i < this.count; i++)
            {
                this.testInts[i] = this.random.Next();
                this.testStrings[i] = "Test String " + this.testInts[i];
                this.dictionary.Add(this.testInts[i], this.testStrings[i]);
            }
        }

        [Fact()]
        public void Llrb23TreeTest()
        {
            TestComparer comparer = new TestComparer();
            SortedDictionary<int, string> sortedDictionary = new SortedDictionary<int, string>(comparer);
            Llrb23Tree<int, string> tree = new Llrb23Tree<int, string>(comparer);
            IEnumerator<KeyValuePair<int, string>> dictionaryEnumerator;
            IEnumerator<KeyValuePair<int, string>> llrbEnumerator;

            for (int i = 0; i < this.count; i++)
            {
                sortedDictionary.Add(this.testInts[i], this.testStrings[i]);
                tree.Add(this.testInts[i], this.testStrings[i]);
            }

            dictionaryEnumerator = sortedDictionary.GetEnumerator();
            llrbEnumerator = tree.GetEnumerator();

            while (dictionaryEnumerator.MoveNext() && llrbEnumerator.MoveNext())
            {
                Assert.Equal(dictionaryEnumerator.Current, llrbEnumerator.Current);
            }
        }

        [Fact()]
        public void AddTest()
        {
            Llrb23Tree<int, string> tree = this.InitializeTree();

            Assert.Equal(this.count, tree.Count);

            for (int i = 0; i < this.count; i++)
            {
                Assert.Equal(this.testStrings[i], tree[this.testInts[i]]);
            }

            Assert.Throws<ArgumentException>(() => tree.Add(this.testInts[0], this.testStrings[0]));
        }

        [Fact()]
        public void RemoveTest()
        {
            Llrb23Tree<int, string> tree = this.InitializeTree();

            for (int i = 0; i < this.count; i += 2)
            {
                tree.Remove(this.testInts[i]);
            }

            Assert.Equal(this.count / 2, tree.Count);

            for (int i = 0; i < this.count; i += 2)
            {
                Assert.Throws<KeyNotFoundException>(() => tree[this.testInts[i]]);
            }

            for (int i = 1; i < this.count / 2; i += 2)
            {
                Assert.Equal(this.testStrings[i], tree[this.testInts[i]]);
            }
        }

        [Fact()]
        public void ContainsKeyTest()
        {
            Llrb23Tree<int, string> tree = this.InitializeTree();

            for (int i = 0; i < this.count; i++)
            {
                Assert.Equal(true, tree.ContainsKey(this.testInts[i]));
            }

            Assert.Equal(false, tree.ContainsKey(this.count));
        }

        [Fact()]
        public void ClearTest()
        {
            Llrb23Tree<int, string> tree = this.InitializeTree();

            tree.Clear();

            Assert.Equal(0, tree.Count);

            for (int i = 0; i < this.count; i++)
            {
                Assert.Throws<KeyNotFoundException>(() => tree[this.testInts[i]]);
            }
        }

        [Fact()]
        public void TryGetValueTest()
        {
            Llrb23Tree<int, string> tree = this.InitializeTree();
            string value;

            for (int i = 0; i < this.count; i++)
            {
                Assert.Equal(true, tree.TryGetValue(this.testInts[i], out value));
                Assert.Equal(this.testStrings[i], value);
            }

            Assert.Equal(false, tree.TryGetValue(-1, out value));
            Assert.Equal(null, value);
        }

        [Fact()]
        public void GetEnumeratorTest()
        {
            Llrb23Tree<int, string> tree = this.InitializeTree();
            IEnumerator<KeyValuePair<int, string>> dictionaryEnumerator = this.dictionary.GetEnumerator();
            IEnumerator<KeyValuePair<int, string>> llrbEnumerator = tree.GetEnumerator();

            while (dictionaryEnumerator.MoveNext() && llrbEnumerator.MoveNext())
            {
                Assert.Equal(dictionaryEnumerator.Current, llrbEnumerator.Current);
            }
        }

        [Fact()]
        public void KeysTest()
        {
            Llrb23Tree<int, string> tree = this.InitializeTree();
            IEnumerator<int> dictionaryKeysEnumerator = this.dictionary.Keys.GetEnumerator();
            IEnumerator<int> treeKeysEnumerator = tree.Keys.GetEnumerator();

            while (dictionaryKeysEnumerator.MoveNext() && treeKeysEnumerator.MoveNext())
            {
                Assert.Equal(dictionaryKeysEnumerator.Current, treeKeysEnumerator.Current);
            }
        }

        [Fact()]
        public void ValuesTest()
        {
            Llrb23Tree<int, string> tree = this.InitializeTree();
            IEnumerator<string> dictionaryKeysEnumerator = this.dictionary.Values.GetEnumerator();
            IEnumerator<string> treeKeysEnumerator = tree.Values.GetEnumerator();

            while (dictionaryKeysEnumerator.MoveNext() && treeKeysEnumerator.MoveNext())
            {
                Assert.Equal(dictionaryKeysEnumerator.Current, treeKeysEnumerator.Current);
            }
        }

        [Fact()]
        public void SetTest()
        {
            Llrb23Tree<int, string> tree = this.InitializeTree();
            string changed = "Changed";

            for (int i = 0; i < this.count; i++)
            {
                tree[this.testInts[i]] = changed;
                Assert.Equal(changed, tree[this.testInts[i]]);
            }
        }

        private Llrb23Tree<int, string> InitializeTree()
        {
            Llrb23Tree<int, string> tree = new Llrb23Tree<int, string>();

            for (int i = 0; i < this.count; i++)
            {
                tree.Add(this.testInts[i], this.testStrings[i]);
            }

            return tree;
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
