using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tako.Collections.Generic
{
    public abstract class Heap<T> : ICollection<T>
    {
        private const int defaultCount = 10;

        private T[] array;

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public int Count { get; private set; }

        public IComparer<T> Comparer { get; private set; }

        public T this[int index]
        {
            get
            {
                if (index < 0 || this.Count <= index)
                {
                    throw new IndexOutOfRangeException("index");
                }

                return this.array[index];
            }
        }

        public Heap(T[] array, int size, bool copies, IComparer<T> comparer)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            if (comparer == null)
            {
                this.Comparer = Comparer<T>.Default;
            }
            else
            {
                this.Comparer = comparer;
            }

            if (array == null)
            {
                this.array = new T[size];
                this.Count = 0;
            }
            else
            {
                T[] targetArray;

                if (copies)
                {
                    targetArray = new T[size];

                    for (int i = 0; i < size; i++)
                    {
                        targetArray[i] = array[i];
                    }
                }
                else
                {
                    targetArray = array;
                }

                this.array = targetArray;
                this.Count = size;
                this.Build();
            }
        }

        public T GetParent(int index)
        {
            if (index < 0 || this.Count <= index)
            {
                throw new IndexOutOfRangeException("index");
            }

            int parentIndex = (index - 1) / 2;

            return 0 <= parentIndex ? this.array[parentIndex] : default(T);
        }

        public T GetLeft(int index)
        {
            if (index < 0 || this.Count <= index)
            {
                throw new IndexOutOfRangeException("index");
            }

            int leftIndex = index * 2 + 1;

            return leftIndex < this.Count ? this.array[leftIndex] : default(T);
        }

        public T GetRight(int index)
        {
            if (index < 0 || this.Count <= index)
            {
                throw new IndexOutOfRangeException("index");
            }

            int rightIndex = index * 2 + 2;

            return rightIndex < this.Count ? this.array[rightIndex] : default(T);
        }

        public void Insert(T item)
        {
            if (this.Count == this.array.Length)
            {
                T[] newArray = new T[this.Count * 2];

                this.array.CopyTo(newArray, 0);
                this.array = newArray;
            }

            this.array[this.Count] = item;
            this.ShiftUp(this.Count++);
        }

        public T ExtractRoot()
        {
            if (this.Count == 0)
            {
                return default(T);
            }

            if (this.Count == 1)
            {
                return this.array[--this.Count];
            }
            else
            {
                T min = this.array[0];

                this.array[0] = this.array[--this.Count];
                this.Heapify(0);

                return min;
            }
        }

        void ICollection<T>.Add(T item)
        {
            this.Insert(item);
        }

        void ICollection<T>.Clear()
        {
            this.Count = 0;
        }

        bool ICollection<T>.Contains(T item)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (item.Equals(this.array[i]))
                {
                    return true;
                }
            }

            return false;
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }
            if (this.Count < arrayIndex + array.Length)
            {
                throw new ArgumentException("The array given is too small.", "array");
            }

            for (int i = 0; i < this.Count; i++)
            {
                array[arrayIndex + i] = this.array[i];
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return this.array[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        protected abstract bool SatisfiesProperty(T parent, T child);

        private void Build()
        {
            for (int i = (this.Count - 1) / 2; 0 <= i; i--)
            {
                this.Heapify(i);
            }
        }

        private void Heapify(int index)
        {
            int leftIndex = index * 2 + 1;
            int rightIndex = index * 2 + 2;
            int smallestIndex = index;

            if (leftIndex < this.Count)
            {
                if (!this.SatisfiesProperty(this.array[smallestIndex], this.array[leftIndex]))
                {
                    smallestIndex = leftIndex;
                }

                if (rightIndex < this.Count)
                {
                    if (!this.SatisfiesProperty(this.array[smallestIndex], this.array[rightIndex]))
                    {
                        smallestIndex = rightIndex;
                    }
                }
            }

            if (smallestIndex != index)
            {
                T temp = this.array[index];

                this.array[index] = this.array[smallestIndex];
                this.array[smallestIndex] = temp;

                this.Heapify(smallestIndex);
            }
        }

        private void ShiftUp(int index)
        {
            if (index == 0)
            {
                return;
            }

            int parentIndex = (index - 1) / 2;

            if (!this.SatisfiesProperty(this.array[parentIndex], this.array[index]))
            {
                T temp = this.array[index];

                array[index] = array[parentIndex];
                array[parentIndex] = temp;

                this.ShiftUp(parentIndex);
            }
            else
            {
                return;
            }
        }
    }
}
