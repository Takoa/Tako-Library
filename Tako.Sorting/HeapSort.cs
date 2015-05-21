using System.Collections.Generic;
using Tako.Collections.Generic;

namespace Tako.Sorting
{
    public class HeapSort<T> : SortBase<T>
    {
        public HeapSort()
            : base(null)
        {
        }

        public HeapSort(IComparer<T> comparer)
            : base(comparer)
        {
        }

        public override void Sort(T[] array)
        {
            MaxHeap<T> heap = new MaxHeap<T>(array, false);

            for (int i = array.Length - 1; 0 <= i; i--)
            {
                array[i] = heap.ExtractRoot();
            }
        }
    }
}
