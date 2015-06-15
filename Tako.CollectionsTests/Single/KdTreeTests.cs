using System;
using Xunit;

namespace Tako.Collections.Single.Tests
{
    public class KdTreeTests
    {
        private int count = 10000;
        private readonly float[][] testPoints;
        private Random random = new Random();

        public KdTreeTests()
        {
            this.testPoints = new float[this.count][];

            for (int i = 0; i < this.count; i++)
            {
                this.testPoints[i] = new float[] { (float)this.random.NextDouble(), (float)this.random.NextDouble(), (float)this.random.NextDouble() };
            }
        }

        [Fact()]
        public void KdTreeTest()
        {
            float[][] copy = new float[this.count][];
            KdTree kdTree;

            for (int i = 0; i < this.testPoints.Length; i++)
            {
                copy[i] = this.testPoints[i];
            }

            kdTree = new KdTree(this.testPoints, 3);

            for (int i = 0; i < this.testPoints.Length; i++)
            {
                Assert.Equal(this.testPoints[i], copy[i]);
            }

            Assert.Equal(this.count, kdTree.Count);
        }

        [Fact()]
        public void AddTest()
        {
            float[][] points = new float[this.count][];
            KdTree kdTree = new KdTree(this.testPoints, 3);

            for (int i = 0; i < this.count; i++)
            {
                points[i] = new float[] { (float)this.random.NextDouble(), (float)this.random.NextDouble(), (float)this.random.NextDouble() };
                kdTree.Add(points[i]);
            }

            for (int i = 0; i < points.Length; i++)
            {
                float[] point = kdTree.GetNearest(points[i]);

                for (int j = 0; j < point.Length; j++)
                {
                    Assert.Equal(points[i][j], point[j]);
                }
            }

            Assert.Equal(this.count * 2, kdTree.Count);
        }

        [Fact()]
        public void RemoveTest()
        {
            KdTree kdTree = new KdTree(this.testPoints, 3);

            kdTree.Remove(this.testPoints[0]);

            for (int i = 1; i < this.testPoints.Length; i += 2)
            {
                kdTree.Remove(this.testPoints[i]);
            }

            for (int i = 1; i < this.testPoints.Length; i++)
            {
                if (i % 2 == 0)
                {
                    float[] point = kdTree.GetNearest(this.testPoints[i]);

                    for (int j = 0; j < point.Length; j++)
                    {
                        Assert.Equal(this.testPoints[i][j], point[j]);
                    }
                }
                else
                {
                    float[] point = kdTree.GetNearest(this.testPoints[i]);
                    bool isEqual = true;

                    for (int j = 0; j < point.Length; j++)
                    {
                        if (this.testPoints[i][j] != point[j])
                        {
                            isEqual = false;
                        }
                    }

                    Assert.False(isEqual);
                }
            }

            Assert.Equal(this.count / 2 - 1, kdTree.Count);
        }

        [Fact()]
        public void ClearTest()
        {
            KdTree kdTree = new KdTree(this.testPoints, 3);

            kdTree.Clear();

            Assert.Equal(0, kdTree.Count);
            Assert.Equal(null, kdTree.GetNearest(this.testPoints[0]));
        }

        [Fact()]
        public void GetEnumeratorTest()
        {
            KdTree kdTree = new KdTree(this.testPoints, 3);
            int i = 0;

            foreach (float[] item in kdTree)
            {
                i++;
            }

            Assert.Equal(kdTree.Count, i);
        }

        [Fact()]
        public void GetNearestTest()
        {
            KdTree kdTree = new KdTree(this.testPoints, 3);

            for (int i = 0; i < this.testPoints.Length; i++)
            {
                float[] point = kdTree.GetNearest(this.testPoints[i]);

                for (int j = 0; j < point.Length; j++)
                {
                    Assert.Equal(this.testPoints[i][j], point[j]);
                }
            }
        }
    }
}
