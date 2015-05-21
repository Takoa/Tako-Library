using System.Collections.Generic;

namespace Tako.Collections.Generic
{
    public partial class Llrb23TreeList<T>
    {
        private class Element : IEnumerable<Element>
        {
            public bool IsRed;
            public T Item;
            public Element Left;
            public Element Right;
            public int TreeSize = 1;

            public Element(T item)
            {
                this.IsRed = true;
                this.Item = item;
            }

            public static bool IsNotNilAndRed(Element element)
            {
                return element != null && element.IsRed;
            }

            public static bool IsNilOrBlack(Element element)
            {
                return element == null || !element.IsRed;
            }

            public static void RemoveMin(ref Element from)
            {
                if (from.Left == null)
                {
                    from = null;

                    return;
                }

                if (Element.IsNilOrBlack(from.Left) && Element.IsNilOrBlack(from.Left.Left))
                {
                    Element.MoveRedLeft(ref from);
                }

                Element.RemoveMin(ref from.Left);

                from.TreeSize--;
                Element.FixUp(ref from);
            }

            public static void RotateLeft(ref Element root)
            {
                Element pivot = root.Right;

                root.Right = pivot.Left;
                root.TreeSize -= pivot.TreeSize;
                root.TreeSize += (pivot.Left != null ? pivot.Left.TreeSize : 0);
                pivot.TreeSize -= (pivot.Left != null ? pivot.Left.TreeSize : 0);
                pivot.TreeSize += root.TreeSize;
                pivot.Left = root;
                pivot.IsRed = root.IsRed;
                root.IsRed = true;
                root = pivot;
            }

            public static void RotateRight(ref Element root)
            {
                Element pivot = root.Left;

                root.Left = pivot.Right;
                root.TreeSize -= pivot.TreeSize;
                root.TreeSize += (pivot.Right != null ? pivot.Right.TreeSize : 0);
                pivot.TreeSize -= (pivot.Right != null ? pivot.Right.TreeSize : 0);
                pivot.TreeSize += root.TreeSize;
                pivot.Right = root;
                pivot.IsRed = root.IsRed;
                root.IsRed = true;
                root = pivot;
            }

            public static void MoveRedLeft(ref Element element)
            {
                element.FlipColor();

                if (Element.IsNotNilAndRed(element.Right.Left))
                {
                    Element right = element.Right;

                    Element.RotateRight(ref right);
                    element.Right = right;
                    Element.RotateLeft(ref element);
                    element.FlipColor();
                }
            }

            public static void MoveRedRight(ref Element element)
            {
                element.FlipColor();

                if (Element.IsNotNilAndRed(element.Left.Left))
                {
                    Element.RotateRight(ref element);
                    element.FlipColor();
                }
            }

            public static void FixUp(ref Element element)
            {
                if (Element.IsNotNilAndRed(element.Right))  // Rotate-left right-leaning reds
                {
                    Element.RotateLeft(ref element);
                }

                if (Element.IsNotNilAndRed(element.Left) && Element.IsNotNilAndRed(element.Left.Left))  // Rotate-right red-red pairs
                {
                    Element.RotateRight(ref element);
                }

                if (Element.IsNotNilAndRed(element.Left) && Element.IsNotNilAndRed(element.Right))  // Split 4-elements
                {
                    element.FlipColor();
                }
            }

            public void FlipColor()
            {
                this.IsRed = !this.IsRed;
                this.Left.IsRed = !this.Left.IsRed;
                this.Right.IsRed = !this.Right.IsRed;
            }

            public Element GetMin()
            {
                Element node = this;

                while (node.Left != null)
                {
                    node = node.Left;
                }

                return node;
            }

            public IEnumerator<Element> GetEnumerator()
            {
                if (this.Left != null)
                {
                    foreach (Element element in this.Left)
                    {
                        yield return element;
                    }
                }

                yield return this;

                if (this.Right != null)
                {
                    foreach (Element element in this.Right)
                    {
                        yield return element;
                    }
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
