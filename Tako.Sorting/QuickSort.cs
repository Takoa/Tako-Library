using System.Collections.Generic;

namespace Tako.Sorting
{
    public class QuickSort<T> : SortBase<T>
    {
        public QuickSort()
            : base(null)
        {
        }

        public QuickSort(IComparer<T> comparer)
            : base(comparer)
        {
        }

        public override void Sort(T[] array)
        {
            this.Sort(array, 0, array.Length - 1);
        }

        private void Sort(T[] array, int startIndex, int endIndex)
        {
            int length = endIndex - startIndex;

            if (length <= 0)
            {
                return;
            }
            else if (length == 1)
            {
                if (0 < this.Comparer.Compare(array[startIndex], array[endIndex]))
                {
                    QuickSort<T>.Swap(array, startIndex, endIndex);
                }
            }
            else
            {
                int middleIndex = startIndex;

                for (int i = startIndex; i < endIndex; i++)
                {
                    if (this.Comparer.Compare(array[i], array[endIndex]) < 0)
                    {
                        QuickSort<T>.Swap(array, i, middleIndex++);
                    }
                }

                QuickSort<T>.Swap(array, middleIndex, endIndex);

                this.Sort(array, startIndex, middleIndex - 1);
                this.Sort(array, middleIndex + 1, endIndex);
            }
        }

        private static void Swap(T[] array, int index1, int index2)
        {
            T temp = array[index1];

            array[index1] = array[index2];
            array[index2] = temp;
        }
    }
}
