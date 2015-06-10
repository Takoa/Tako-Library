using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tako.Collections.Single
{
    public partial class KdTree
    {
        private Node root;

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public int Count { get; private set; }

        public int Dimention { get; private set; }

        public KdTree(float[][] values, int dimention)
            : this(values, dimention, true)
        {
        }

        public KdTree(float[][] values, int dimention, bool copies)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Length != dimention)
                {
                    throw new ArgumentException("values");
                }
            }
            if (dimention < 0)
            {
                throw new ArgumentOutOfRangeException("dimention");
            }

            float[][] valuesForBuild;

            this.Dimention = dimention;
            this.Count = values.Length;

            if (copies)
            {
                valuesForBuild = new float[this.Count][];

                for (int i = 0; i < valuesForBuild.Length; i++)
                {
                    valuesForBuild[i] = values[i];
                }
            }
            else
            {
                valuesForBuild = values;
            }

            this.root = this.Build(valuesForBuild, 0, 0, values.Length - 1);
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
                    array[index++] = node.Value;
                }
            }
        }

        public IEnumerator<float[]> GetEnumerator()
        {
            if (this.root != null)
            {
                foreach (Node node in this.root)
                {
                    yield return node.Value;
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
            if (point.Length != this.Dimention)
            {
                throw new ArgumentException("point");
            }

            Node nearest = null;
            
            squareDistance = float.PositiveInfinity;

            this.GetNearest(point, this.root, 0, ref nearest, ref squareDistance);

            return nearest != null ? nearest.Value : null;
        }

        private Node Build(float[][] values, int depth, int startIndex, int endIndex)
        {
            int length = endIndex - startIndex;
            Node result;
            int mid = startIndex + length / 2;

            if (length < 0)
            {
                return null;
            }

            result = new Node(this.GetMedianValue(values, startIndex, endIndex, depth % this.Dimention));
            result.Left = this.Build(values, ++depth, startIndex, mid - 1);
            result.Right = this.Build(values, depth, mid + 1, endIndex);

            return result;
        }

        private float[] GetMedianValue(float[][] values, int startIndex, int endIndex, int axis)
        {
            this.Quicksort(values, startIndex, endIndex, axis);

            return values[startIndex + (endIndex - startIndex) / 2];
        }

        private void Quicksort(float[][] values, int startIndex, int endIndex, int axis)
        {
            int length = endIndex - startIndex;

            if (length <= 0)
            {
                return;
            }
            else if (length == 1)
            {
                if (values[endIndex][axis] < values[startIndex][axis])
                {
                    KdTree.Swap(values, startIndex, endIndex);
                }
            }
            else
            {
                int middleIndex = startIndex;

                for (int i = startIndex; i < endIndex; i++)
                {
                    if (values[i][axis] < values[endIndex][axis])
                    {
                        KdTree.Swap(values, i, middleIndex++);
                    }
                }

                if (middleIndex != endIndex)
                {
                    KdTree.Swap(values, middleIndex, endIndex);
                }

                this.Quicksort(values, startIndex, middleIndex - 1, axis);
                this.Quicksort(values, middleIndex + 1, endIndex, axis);
            }
        }

        private static void Swap<T>(T[] array, int index1, int index2)
        {
            T temp = array[index1];

            array[index1] = array[index2];
            array[index2] = temp;
        }

        private void GetNearest(float[] point, Node node, int depth, ref Node currentBest, ref float currentBestDistance)
        {
            if (node == null)
            {
                return;
            }

            int axis = depth % this.Dimention;
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

            if (currentBest == null)
            {
                currentBest = node;
                currentBestDistance = this.GetDistance(point, node.Value);
            }
            else
            {
                float f = node.Value[axis] - currentBest.Value[axis];

                if (f * f < currentBestDistance)
                {
                    float distance = this.GetDistance(point, node.Value);

                    this.GetNearest(point, otherSide, depth, ref node, ref distance);

                    if (distance < currentBestDistance)
                    {
                        currentBest = node;
                        currentBestDistance = distance;
                    }
                }
            }
        }

        private float GetDistance(float[] point1, float[] point2)
        {
            float result = 0f;

            for (int i = 0; i < point1.Length; i++)
            {
                float f = point2[i] - point1[i];

                result += f * f;
            }

            return result;
        }
    }
}
