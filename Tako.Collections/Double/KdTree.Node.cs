using System.Collections.Generic;

namespace Tako.Collections.Double
{
    public partial class KdTree
    {
        private class Node : IEnumerable<Node>
        {
            public double[] Value;
            public Node Left;
            public Node Right;
            public bool IsRemoved;
            public int Count;

            public Node(double[] value)
            {
                this.Value = value;
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
