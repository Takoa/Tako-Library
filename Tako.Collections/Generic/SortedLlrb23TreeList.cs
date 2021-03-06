﻿using System;
using System.Collections.Generic;

namespace Tako.Collections.Generic
{
    public partial class SortedLlrb23TreeList<T> : IList<T>
    {
        private Element root;

        public IComparer<T> Comparer { get; private set; }

        public int Count
        {
            get
            {
                return this.root != null ? this.root.TreeSize : 0;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || this.Count <= index)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return this.FindElementByIndex(this.root, index).Item;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public SortedLlrb23TreeList()
            : this(null)
        {
        }

        public SortedLlrb23TreeList(IComparer<T> comparer)
        {
            if (comparer == null)
            {
                this.Comparer = Comparer<T>.Default;
            }
            else
            {
                this.Comparer = comparer;
            }
        }

        public void Add(T item)
        {
            this.Add(ref this.root, item);
            this.root.IsRed = false;
        }

        public bool Contains(T item)
        {
            return this.FindElementByItem(this.root, item) != null;
        }

        public int IndexOf(T item)
        {
            Element element = this.root;
            int index = 0;

            while (element != null)
            {
                int order = this.Comparer.Compare(item, element.Item);

                if (order == 0)
                {
                    return index += (element.Left != null ? element.Left.TreeSize : 0);
                }
                else if (order < 0)
                {
                    element = element.Left;
                }
                else
                {
                    index += 1 + (element.Left != null ? element.Left.TreeSize : 0);
                    element = element.Right;
                }
            }

            return -1;
        }

        public void Clear()
        {
            this.root = null;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || this.Count <= index)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            this.RemoveAt(ref this.root, index);
        }

        public bool Remove(T item)
        {
            int index = this.IndexOf(item);

            if (0 <= index)
            {
                return this.RemoveAt(ref this.root, index);
            }
            else
            {
                throw new ArgumentException("No such item.");
            }
        }

        public void CopyTo(T[] array, int index)
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

            if (this.root != null)
            {
                foreach (Element element in this.root)
                {
                    array[index++] = element.Item;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (this.root != null)
            {
                foreach (Element element in this.root)
                {
                    yield return element.Item;
                }
            }
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private Element FindElementByIndex(Element element, int index)
        {
            while (element != null)
            {
                int leftTreeSize = element.Left != null ? element.Left.TreeSize : 0;

                if (index == leftTreeSize)
                {
                    return element;
                }
                else if (index < leftTreeSize)
                {
                    element = element.Left;
                }
                else
                {
                    element = element.Right;
                    index -= (leftTreeSize + 1);
                }
            }

            return null;
        }

        private Element FindElementByItem(Element element, T item)
        {
            while (element != null)
            {
                int order = this.Comparer.Compare(item, element.Item);

                if (order == 0)
                {
                    return element;
                }
                else
                {
                    element = order < 0 ? element.Left : element.Right;
                }
            }

            return null;
        }

        private void Add(ref Element element, T item)
        {
            int order;

            if (element == null)
            {
                element = new Element(item);  // Insert at the bottom.

                return;
            }

            order = this.Comparer.Compare(item, element.Item);

            if (order == 0)
            {
                throw new ArgumentException("The item already exists.", "key");
            }
            else if (order < 0)
            {
                this.Add(ref element.Left, item);
            }
            else
            {
                this.Add(ref element.Right, item);
            }

            element.TreeSize++;

            if (Element.IsNilOrBlack(element.Left) && Element.IsNotNilAndRed(element.Right))
            {
                Element.RotateLeft(ref element);  // Fix right-leaning reds on the way up.
            }

            if (Element.IsNotNilAndRed(element.Left) && Element.IsNotNilAndRed(element.Left.Left))
            {
                Element.RotateRight(ref element);  // Fix two reds in a row on the way up.
            }

            if (Element.IsNotNilAndRed(element.Left) && Element.IsNotNilAndRed(element.Right))
            {
                element.FlipColor();  // Split 4-nodes on the way up.
            }
        }

        private bool RemoveAt(ref Element element, int index)
        {
            bool succeeded;

            if (index < (element.Left != null ? element.Left.TreeSize : 0))
            {
                // Removing will never fail if the index is always in range of the tree; thus, currently, No false-returning processes are needed.
                //
                //if (from.Left != null)
                //{
                if (Element.IsNilOrBlack(element.Left) && Element.IsNilOrBlack(element.Left.Left))
                {
                    Element.MoveRedLeft(ref element);  // Push red right if necessary.
                }

                succeeded = this.RemoveAt(ref element.Left, index);  // Move down (left).
                //}
                //else
                //{
                //    return false;
                //}
            }
            else
            {
                int leftTreeSize;

                if (Element.IsNotNilAndRed(element.Left))
                {
                    Element.RotateRight(ref element);  // Rotate to push red right.
                }

                if (element.Right == null && index == (element.Left != null ? element.Left.TreeSize : 0))
                {
                    element = null;  // Delete node.

                    return true;
                }

                //if (from.Right == null)
                //{
                //    if (index == (from.Left != null ? from.Left.TreeSize : 0))
                //    {
                //        from = null;  // Delete node.

                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}

                if (Element.IsNilOrBlack(element.Right) && Element.IsNilOrBlack(element.Right.Left))
                {
                    Element.MoveRedRight(ref element);  // Push red right if necessary.
                }

                leftTreeSize = (element.Left != null ? element.Left.TreeSize : 0);

                if (index == leftTreeSize)
                {
                    element.Item = element.Right.GetMin().Item;  // Replace current node with successor key, value.
                    Element.RemoveMin(ref element.Right);  // Delete successor.
                    succeeded = true;
                }
                else
                {
                    succeeded = this.RemoveAt(ref element.Right, index - (leftTreeSize + 1));  // Move down (right).
                }
            }

            element.TreeSize--;
            Element.FixUp(ref element);  // Fix right-leaning red links and eliminate 4-nodes on the way up.

            return succeeded;
        }
    }
}
