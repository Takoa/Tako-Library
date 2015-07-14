using System.Collections.Generic;

namespace Tako.Sorting
{
    public class MergeSort<T> : SortBase<T>
    {
        private T[] temp;

        public MergeSort()
            : base(null)
        {
        }

        public MergeSort(IComparer<T> comparer)
            : base(comparer)
        {
        }

        public override void Sort(T[] array)
        {
            this.temp = new T[array.Length];
            this.Sort(array, 0, array.Length - 1);
        }

        private void Sort(T[] array, int startIndex, int endIndex)
        {
            if (endIndex <= startIndex)
            {
                return;
            }

            int mid = (startIndex + endIndex) / 2;

            this.Sort(array, startIndex, mid);
            this.Sort(array, mid + 1, endIndex);
            this.merge(array, startIndex, mid, endIndex);
        }

        private void merge(T[] array, int startIndex, int mid, int endIndex)
        {
            for (int i = startIndex; i <= mid; i++)
            {
                this.temp[i] = array[i];
            }

            for (int i = mid + 1, j = endIndex; i <= endIndex; i++, j--)
            {
                this.temp[i] = array[j];
            }

            for (int i = startIndex, j = startIndex, k = endIndex; i <= endIndex; i++)
            {
                array[i] = this.Comparer.Compare(this.temp[j], this.temp[k]) < 0 ? this.temp[j++] : this.temp[k--];
            }
        }
    }
}
