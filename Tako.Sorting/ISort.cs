using System.Collections.Generic;

namespace Tako.Sorting
{
    public interface ISort<T>
    {
        IComparer<T> Comparer { get; }

        void Sort(T[] array);
    }
}
