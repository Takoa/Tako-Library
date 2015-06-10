using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tako.Collections.Single;
using Xunit;

namespace Tako.Collections.Single.Tests
{
    public class KdTreeTests
    {
        private int count = 1000000;
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

                Console.WriteLine(this.testPoints[i].ToString() + ", " + point.ToString());

                for (int j = 0; j < point.Length; j++)
                {
                    Assert.Equal(this.testPoints[i][j], point[j]);
                }
            }
        }
    }
}
