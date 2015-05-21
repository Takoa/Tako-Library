using System.Collections.Generic;

namespace Tako.Sorting
{
    public class MergeSort<T> : SortBase<T>
    {
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
            if (array.Length == 1)
            {
                return;
            }

            int firstArrayLength = array.Length / 2;
            int secondArrayLength = array.Length - firstArrayLength;
            T[] firstArray = new T[firstArrayLength];
            T[] secondArray = new T[secondArrayLength];

            for (int i = 0; i < firstArrayLength; i++)
            {
                firstArray[i] = array[i];
            }

            for (int i = 0; i < secondArrayLength; i++)
            {
                secondArray[i] = array[firstArrayLength + i];
            }

            this.Sort(firstArray);
            this.Sort(secondArray);
            this.merge(firstArray, secondArray, array);
        }

        private void merge(T[] firstArray, T[] secondArray, T[] array)
        {
            int i = 0;
            int j = 0;
            int k = 0;

            for (; i < firstArray.Length && j < secondArray.Length; k++)
            {
                array[k] = this.Comparer.Compare(firstArray[i], secondArray[j]) < 0 ? firstArray[i++] : secondArray[j++];
            }

            for (; i < firstArray.Length; i++, k++)
            {
                array[k] = firstArray[i];
            }

            for (; j < secondArray.Length; j++, k++)
            {
                array[k] = secondArray[j];
            }
        }
    }
}
