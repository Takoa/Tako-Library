using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tako.Sorting
{
    public interface ISort<T>
    {
        IComparer<T> Comparer { get; }

        void Sort(T[] array);
    }
}
