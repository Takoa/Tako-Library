using System;
using System.Collections.Generic;

namespace Tako.Collections.Single
{
    public partial class KdTree
    {
        private Node root;
        private int removedNodeCount;

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public int Count
        {
            get
            {
                return this.root != null ? this.root.Count - this.removedNodeCount : 0;
            }
        }

        public int Dimension { get; private set; }

        public KdTree(float[][] points, int dimension)
            : this(points, dimension, true)
        {
        }

        public KdTree(float[][] points, int dimension, bool copies)
        {
            if (points == null)
            {
                throw new ArgumentNullException("points");
            }
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].Length != dimension)
                {
                    throw new ArgumentException("points");
                }
            }
            if (dimension < 0)
            {
                throw new ArgumentOutOfRangeException("dimension");
            }

            float[][] pointsForBuild;

            this.Dimension = dimension;

            if (copies)
            {
                pointsForBuild = new float[points.Length][];

                for (int i = 0; i < pointsForBuild.Length; i++)
                {
                    pointsForBuild[i] = points[i];
                }
            }
            else
            {
                pointsForBuild = points;
            }

            this.root = this.Build(pointsForBuild, 0, 0, points.Length - 1);
        }

        public void Add(float[] point)
        {
            if (point.Length != this.Dimension)
            {
                throw new ArgumentException("point");
            }

            this.Add(ref this.root, point, 0);
        }

        public bool Remove(float[] point)
        {
            if (point.Length != this.Dimension)
            {
                throw new ArgumentException("point");
            }
            if (this.root == null)
            {
                return false;
            }

            Node nearest = null;
            float distance = float.PositiveInfinity;

            this.GetNearest(point, this.root, 0, ref nearest, ref distance);

            for (int i = 0; i < point.Length; i++)
            {
                if (point[i] != nearest.Value[i])
                {
                    return false;
                }
            }

            nearest.IsRemoved = true;
            this.removedNodeCount++;

            if (this.Count < this.removedNodeCount * 2)
            {
                this.Rebuild();
            }

            return true;
        }

        public void Clear()
        {
            this.root = null;
            this.removedNodeCount = 0;
        }

        public void CopyTo(float[][] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
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
                foreach (Node node in this.root)
                {
                    if (!node.IsRemoved)
                    {
                        array[index++] = node.Value;
                    }
                }
            }
        }

        public IEnumerator<float[]> GetEnumerator()
        {
            if (this.root != null)
            {
                foreach (Node node in this.root)
                {
                    if (!node.IsRemoved)
                    {
                        yield return node.Value;
                    }
                }
            }
        }

        public float[] GetNearest(float[] point)
        {
            float squareDistance;

            return this.GetNearest(point, out squareDistance);
        }

        public float[] GetNearest(float[] point, out float squareDistance)
        {
            if (point.Length != this.Dimension)
            {
                throw new ArgumentException("point");
            }

            Node nearest = null;

            squareDistance = float.PositiveInfinity;

            this.GetNearest(point, this.root, 0, ref nearest, ref squareDistance);

            return nearest != null ? nearest.Value : null;
        }

        public void Rebuild()
        {
            if (this.root == null)
            {
                return;
            }

            this.Rebuild(ref this.root, 0, this.Count);
        }

        private static float GetDistance(float[] point1, float[] point2)
        {
            float result = 0f;

            for (int i = 0; i < point1.Length; i++)
            {
                float f = point2[i] - point1[i];

                result += f * f;
            }

            return result;
        }

        private static void Swap<T>(T[] array, int index1, int index2)
        {
            if (index1 == index2)
            {
                return;
            }

            T temp = array[index1];

            array[index1] = array[index2];
            array[index2] = temp;
        }

        private void Rebuild(ref Node node, int depth)
        {
            this.Rebuild(ref node, depth, node.Count);
        }

        private void Rebuild(ref Node node, int depth, int arrayLength)
        {
            float[][] points = new float[arrayLength][];
            int i = 0;

            foreach (Node item in node)
            {
                if (!item.IsRemoved)
                {
                    points[i++] = item.Value;
                }
            }

            this.removedNodeCount -= node.Count - i;
            node = this.Build(points, depth, 0, --i);
        }

        private Node Build(float[][] points, int depth, int startIndex, int endIndex)
        {
            int length = endIndex - startIndex;
            Node result;
            int mid = startIndex + length / 2;

            if (length < 0)
            {
                return null;
            }

            result = new Node(this.GetMedianValue(points, startIndex, endIndex, depth % this.Dimension));
            result.Left = this.Build(points, ++depth, startIndex, mid - 1);
            result.Right = this.Build(points, depth, mid + 1, endIndex);
            result.Count = endIndex - startIndex + 1;

            return result;
        }

        private float[] GetMedianValue(float[][] points, int startIndex, int endIndex, int axis)
        {
            this.Quicksort(points, startIndex, endIndex, axis);

            return points[startIndex + (endIndex - startIndex) / 2];
        }

        private void Quicksort(float[][] points, int startIndex, int endIndex, int axis)
        {
            int length = endIndex - startIndex;

            if (length <= 0)
            {
                return;
            }
            else if (length == 1)
            {
                if (points[endIndex][axis] < points[startIndex][axis])
                {
                    KdTree.Swap(points, startIndex, endIndex);
                }
            }
            else
            {
                int middleIndex = startIndex;

                for (int i = startIndex; i < endIndex; i++)
                {
                    if (points[i][axis] < points[endIndex][axis])
                    {
                        KdTree.Swap(points, i, middleIndex++);
                    }
                }

                KdTree.Swap(points, middleIndex, endIndex);

                this.Quicksort(points, startIndex, middleIndex - 1, axis);
                this.Quicksort(points, middleIndex + 1, endIndex, axis);
            }
        }

        private void GetNearest(float[] point, Node node, int depth, ref Node currentBest, ref float currentBestDistance)
        {
            if (node == null)
            {
                return;
            }

            int axis = depth % this.Dimension;
            Node otherSide;

            if (point[axis] < node.Value[axis])
            {
                this.GetNearest(point, node.Left, ++depth, ref currentBest, ref currentBestDistance);

                otherSide = node.Right;
            }
            else
            {
                this.GetNearest(point, node.Right, ++depth, ref currentBest, ref currentBestDistance);

                otherSide = node.Left;
            }

            if (!node.IsRemoved)
            {
                if (currentBest == null)
                {
                    currentBest = node;
                    currentBestDistance = KdTree.GetDistance(point, node.Value);
                }
                else
                {
                    float f = node.Value[axis] - currentBest.Value[axis];

                    if (f * f < currentBestDistance)
                    {
                        float distance = KdTree.GetDistance(point, node.Value);

                        this.GetNearest(point, otherSide, depth, ref node, ref distance);

                        if (distance < currentBestDistance)
                        {
                            currentBest = node;
                            currentBestDistance = distance;
                        }
                    }
                }
            }
        }

        private void Add(ref Node node, float[] point, int depth)
        {
            if (node == null)
            {
                node = new Node(point);
                node.Count++;

                return;
            }

            int axis = depth % this.Dimension;
            int leftSize;
            int rightSize;

            if (point[axis] < node.Value[axis])
            {
                this.Add(ref node.Left, point, depth + 1);
            }
            else
            {
                this.Add(ref node.Right, point, depth + 1);
            }

            node.Count++;
            leftSize = node.Left != null ? node.Left.Count : 0;
            rightSize = node.Right != null ? node.Right.Count : 0;

            if ((leftSize < rightSize) && (leftSize + 1) * 2 <= rightSize
                || (rightSize < leftSize) && (rightSize + 1) * 2 < leftSize)
            {
                this.Rebuild(ref node, depth);
            }
        }
    }
}
