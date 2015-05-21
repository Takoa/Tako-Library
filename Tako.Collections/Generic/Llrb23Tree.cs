using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tako.Collections.Generic
{
    [DebuggerDisplay("Count = {Count}")]
    public partial class Llrb23Tree<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Node root;

        public int Count { get; private set; }
        public IComparer<TKey> Comparer { get; private set; }

        public ICollection<TKey> Keys
        {
            get
            {
                return new Llrb23Tree<TKey, TValue>.KeyCollection(this);
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return new Llrb23Tree<TKey, TValue>.ValueCollection(this);
            }
        }

        public virtual TValue this[TKey key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                Node node = this.FindNode(this.root, key);

                if (node != null)
                {
                    return node.Value;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            set
            {
                Node node = this.FindNode(this.root, key);

                if (node != null)
                {
                    node.Value = value;
                }
                else
                {
                    this.Add(key, value);
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public Llrb23Tree()
            : this(null)
        {
        }

        public Llrb23Tree(IComparer<TKey> comparer)
        {
            if (comparer == null)
            {
                this.Comparer = Comparer<TKey>.Default;
            }
            else
            {
                this.Comparer = comparer;
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            this.Add(ref this.root, key, value);
            root.IsRed = false;
            this.Count++;
        }

        public bool Remove(TKey key)
        {
            bool result = this.Remove(ref this.root, key);

            if (result)
            {
                this.Count--;
            }

            return result;
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return this.FindNode(this.root, key) != null;
        }

        public void Clear()
        {
            this.root = null;
            this.Count = 0;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            Node node = this.FindNode(this.root, key);

            if (node != null)
            {
                value = node.Value;

                return true;
            }
            else
            {
                value = default(TValue);

                return false;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (this.root != null)
            {
                foreach (Node node in this.root)
                {
                    yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
                }
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.ContainsKey(item.Key);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if (index < 0 || this.Count <= index)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (array.Length - index < this.Count)
            {
                throw new ArgumentException();
            }

            foreach (Node node in this.root)
            {
                array[index++] = new KeyValuePair<TKey,TValue>(node.Key, node.Value);
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private Node FindNode(Node node, TKey key)
        {
            while (node != null)
            {
                int order = this.Comparer.Compare(key, node.Key);

                if (order == 0)
                {
                    return node;
                }
                else
                {
                    node = order < 0 ? node.Left : node.Right;
                }
            }

            return null;
        }

        private void Add(ref Node to, TKey key, TValue value)
        {
            int order;

            if (to == null)
            {
                to = new Node(key, value);  // Insert at the bottom.

                return;
            }

            order = this.Comparer.Compare(key, to.Key);

            if (order == 0)
            {
                throw new ArgumentException("The key already exists.", "key");
            }
            else if (order < 0)
            {
                this.Add(ref to.Left, key, value);
            }
            else
            {
                this.Add(ref to.Right, key, value);
            }

            if (Node.IsNilOrBlack(to.Left) && Node.IsNotNilAndRed(to.Right))
            {
                Node.RotateLeft(ref to);  // Fix right-leaning reds on the way up.
            }

            if (Node.IsNotNilAndRed(to.Left) && Node.IsNotNilAndRed(to.Left.Left))
            {
                Node.RotateRight(ref to);  // Fix two reds in a row on the way up.
            }

            if (Node.IsNotNilAndRed(to.Left) && Node.IsNotNilAndRed(to.Right))
            {
                to.FlipColor();  // Split 4-nodes on the way up.
            }
        }

        private bool Remove(ref Node from, TKey key)
        {
            bool succeeded;

            if (this.Comparer.Compare(key, from.Key) < 0)
            {
                if (from.Left != null)
                {
                    if (Node.IsNilOrBlack(from.Left) && Node.IsNilOrBlack(from.Left.Left))
                    {
                        Node.MoveRedLeft(ref from);  // Push red right if necessary.
                    }

                    succeeded = this.Remove(ref from.Left, key);  // Move down (left).
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (Node.IsNotNilAndRed(from.Left))
                {
                    Node.RotateRight(ref from);  // Rotate to push red right.
                }

                if (from.Right == null)
                {
                    if (this.Comparer.Compare(key, from.Key) == 0)
                    {
                        from = null;  // Delete node.

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (Node.IsNilOrBlack(from.Right) && Node.IsNilOrBlack(from.Right.Left))
                {
                    Node.MoveRedRight(ref from);  // Push red right if necessary.
                }

                if (this.Comparer.Compare(key, from.Key) == 0)
                {
                    Node min = from.Right.GetMin();

                    from.Key = min.Key;  // Replace current node with successor key, value.
                    from.Value = min.Value;
                    Node.RemoveMin(ref from.Right);  // Delete successor.
                    succeeded = true;
                }
                else
                {
                    succeeded = this.Remove(ref from.Right, key);  // Move down (right).
                }
            }

            Node.FixUp(ref from);  // Fix right-leaning red links and eliminate 4-nodes on the way up.

            return succeeded;
        }
    }
}
