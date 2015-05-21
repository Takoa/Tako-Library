using System.Collections.Generic;

namespace Tako.Sorting
{
    public abstract class SortBase<T> : ISort<T>
    {
        public IComparer<T> Comparer { get; private set; }

        public SortBase(IComparer<T> comparer)
        {
            if (this.Comparer == null)
            {
                this.Comparer = Comparer<T>.Default;
            }
            else
            {
                this.Comparer = comparer;
            }
        }

        public abstract void Sort(T[] array);
    }
}
