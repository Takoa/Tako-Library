using System.Collections.Generic;

namespace Tako.Sorting
{
    public class NonRecursiveQuickSort<T> : SortBase<T>
    {
        public NonRecursiveQuickSort()
            : base(null)
        {
        }

        public NonRecursiveQuickSort(IComparer<T> comparer)
            : base(comparer)
        {
        }

        public override void Sort(T[] array)
        {
            this.Sort(array, 0, array.Length - 1);
        }

        private void Sort(T[] array, int startIndex, int endIndex)
        {
            Stack<int> stack = new Stack<int>(65536);

            stack.Push(endIndex);
            stack.Push(startIndex);

            while (0 < stack.Count)
            {
                startIndex = stack.Pop();
                endIndex = stack.Pop();

                if (startIndex < endIndex)
                {
                    int middleIndex = startIndex;

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        if (this.Comparer.Compare(array[i], array[endIndex]) < 0)
                        {
                            NonRecursiveQuickSort<T>.Swap(array, i, middleIndex++);
                        }
                    }

                    NonRecursiveQuickSort<T>.Swap(array, middleIndex, endIndex);

                    stack.Push(endIndex);
                    stack.Push(middleIndex + 1);
                    stack.Push(middleIndex - 1);
                    stack.Push(startIndex);
                }
            }
        }

        private static void Swap(T[] array, int index1, int index2)
        {
            if (index1 == index2)
            {
                return;
            }

            T temp = array[index1];

            array[index1] = array[index2];
            array[index2] = temp;
        }
    }
}
