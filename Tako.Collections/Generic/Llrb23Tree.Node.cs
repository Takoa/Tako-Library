using System.Collections.Generic;

namespace Tako.Collections.Generic
{
    public partial class Llrb23Tree<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private class Node : IEnumerable<Node>
        {
            public bool IsRed;
            public TKey Key;
            public TValue Value;
            public Node Left;
            public Node Right;

            public Node(TKey key, TValue value)
            {
                this.IsRed = true;
                this.Key = key;
                this.Value = value;
            }

            public static bool IsNotNilAndRed(Node node)
            {
                return node != null && node.IsRed;
            }

            public static bool IsNilOrBlack(Node node)
            {
                return node == null || !node.IsRed;
            }

            public static void RemoveMin(ref Node from)
            {
                if (from.Left == null)
                {
                    from = null;

                    return;
                }

                if (Node.IsNilOrBlack(from.Left) && Node.IsNilOrBlack(from.Left.Left))
                {
                    Node.MoveRedLeft(ref from);
                }

                Node.RemoveMin(ref from.Left);
                Node.FixUp(ref from);
            }

            public static void RotateLeft(ref Node root)
            {
                Node pivot = root.Right;

                root.Right = pivot.Left;
                pivot.Left = root;
                pivot.IsRed = root.IsRed;
                root.IsRed = true;
                root = pivot;
            }

            public static void RotateRight(ref Node root)
            {
                Node pivot = root.Left;

                root.Left = pivot.Right;
                pivot.Right = root;
                pivot.IsRed = root.IsRed;
                root.IsRed = true;
                root = pivot;
            }

            public static void MoveRedLeft(ref Node node)
            {
                node.FlipColor();

                if (Node.IsNotNilAndRed(node.Right.Left))
                {
                    Node right = node.Right;

                    Node.RotateRight(ref right);
                    node.Right = right;
                    Node.RotateLeft(ref node);
                    node.FlipColor();
                }
            }

            public static void MoveRedRight(ref Node node)
            {
                node.FlipColor();

                if (Node.IsNotNilAndRed(node.Left.Left))
                {
                    Node.RotateRight(ref node);
                    node.FlipColor();
                }
            }

            public static void FixUp(ref Node node)
            {
                if (Node.IsNotNilAndRed(node.Right))  // Rotate-left right-leaning reds
                {
                    Node.RotateLeft(ref node);
                }

                if (Node.IsNotNilAndRed(node.Left) && Node.IsNotNilAndRed(node.Left.Left))  // Rotate-right red-red pairs
                {
                    Node.RotateRight(ref node);
                }

                if (Node.IsNotNilAndRed(node.Left) && Node.IsNotNilAndRed(node.Right))  // Split 4-nodes
                {
                    node.FlipColor();
                }
            }

            public void FlipColor()
            {
                this.IsRed = !this.IsRed;
                this.Left.IsRed = !this.Left.IsRed;
                this.Right.IsRed = !this.Right.IsRed;
            }

            public Node GetMin()
            {
                Node node = this;

                while (node.Left != null)
                {
                    node = node.Left;
                }

                return node;
            }

            public IEnumerator<Node> GetEnumerator()
            {
                if (this.Left != null)
                {
                    foreach (Node node in this.Left)
                    {
                        yield return node;
                    }
                }

                yield return this;

                if (this.Right != null)
                {
                    foreach (Node node in this.Right)
                    {
                        yield return node;
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
