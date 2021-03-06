﻿using System;
using System.Collections.Generic;

namespace Tako.Collections.Generic
{
    public partial class Llrb23TreeList<T> : IList<T>
    {
        private Element root;
        private EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;

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

                return this.FindElement(this.root, index).Item;
            }
            set
            {
                if (index < 0 || this.Count <= index)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                this.FindElement(this.root, index).Item = value;
            }
        }

        public Llrb23TreeList()
        {
        }

        public void Add(T item)
        {
            this.Insert(ref this.root, this.Count, item);
            this.root.IsRed = false;
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || this.Count <= index)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            this.Insert(ref this.root, index, item);
            this.root.IsRed = false;
        }

        public bool Contains(T item)
        {
            if (this.root != null)
            {
                if (item == null)
                {
                    foreach (Element element in this.root)
                    {
                        if (element.Item == null)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    foreach (Element element in this.root)
                    {
                        if (this.equalityComparer.Equals(item, element.Item))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public int IndexOf(T item)
        {
            int index = 0;

            if (this.root != null)
            {
                if (item == null)
                {
                    foreach (Element element in this.root)
                    {
                        if (element.Item == null)
                        {
                            return index;
                        }

                        index++;
                    }
                }
                else
                {
                    foreach (Element element in this.root)
                    {
                        if (this.equalityComparer.Equals(item, element.Item))
                        {
                            return index;
                        }

                        index++;
                    }
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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private Element FindElement(Element element, int index)
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

        private void Insert(ref Element element, int index, T item)
        {
            int leftTreeSize;

            if (element == null)
            {
                element = new Element(item);

                return;
            }

            leftTreeSize = element.Left != null ? element.Left.TreeSize : 0;

            if (index <= leftTreeSize)
            {
                this.Insert(ref element.Left, index, item);
            }
            else
            {
                this.Insert(ref element.Right, index - (leftTreeSize + 1), item);
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
                element.FlipColor();  // Split 4-elements on the way up.
            }
        }

        private bool RemoveAt(ref Element to, int index)
        {
            bool succeeded;

            if (index < (to.Left != null ? to.Left.TreeSize : 0))
            {
                // Removing will never fail if the index is always in range of the tree; thus, currently, No false-returning processes are needed.
                //
                //if (from.Left != null)
                //{
                if (Element.IsNilOrBlack(to.Left) && Element.IsNilOrBlack(to.Left.Left))
                {
                    Element.MoveRedLeft(ref to);  // Push red right if necessary.
                }

                succeeded = this.RemoveAt(ref to.Left, index);  // Move down (left).
                //}
                //else
                //{
                //    return false;
                //}
            }
            else
            {
                int leftTreeSize;

                if (Element.IsNotNilAndRed(to.Left))
                {
                    Element.RotateRight(ref to);  // Rotate to push red right.
                }

                if (to.Right == null && index == (to.Left != null ? to.Left.TreeSize : 0))
                {
                    to = null;  // Delete node.

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

                if (Element.IsNilOrBlack(to.Right) && Element.IsNilOrBlack(to.Right.Left))
                {
                    Element.MoveRedRight(ref to);  // Push red right if necessary.
                }

                leftTreeSize = (to.Left != null ? to.Left.TreeSize : 0);

                if (index == leftTreeSize)
                {
                    to.Item = to.Right.GetMin().Item;  // Replace current node with successor key, value.
                    Element.RemoveMin(ref to.Right);  // Delete successor.
                    succeeded = true;
                }
                else
                {
                    succeeded = this.RemoveAt(ref to.Right, index - (leftTreeSize + 1));  // Move down (right).
                }
            }

            to.TreeSize--;
            Element.FixUp(ref to);  // Fix right-leaning red links and eliminate 4-nodes on the way up.

            return succeeded;
        }
    }
}
