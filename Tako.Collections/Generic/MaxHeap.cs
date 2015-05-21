using System;
using System.Collections.Generic;

namespace Tako.Collections.Generic
{
    public class MaxHeap<T> : Heap<T>
    {
        private const int defaultCount = 10;

        public MaxHeap()
            : base(null, MaxHeap<T>.defaultCount, false, null)
        {
        }

        public MaxHeap(IComparer<T> comparer)
            : base(null, MaxHeap<T>.defaultCount, false, comparer)
        {
        }

        public MaxHeap(T[] array, bool copies)
            : base(array, array.Length, copies, null)
        {
        }

        public MaxHeap(T[] array, int size, bool copies, IComparer<T> comparer)
            : base(array, size, copies, comparer)
        {
        }

        protected override bool SatisfiesProperty(T parent, T child)
        {
            return 0 < this.Comparer.Compare(parent, child);
        }
    }
}
