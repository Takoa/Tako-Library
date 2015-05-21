using System;
using System.Collections.Generic;

namespace Tako.Collections.Generic
{
    public partial class Llrb23Tree<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public class KeyCollection : ICollection<TKey>
        {
            private Llrb23Tree<TKey, TValue> llrb;

            public int Count
            {
                get
                {
                    return this.llrb.Count;
                }
            }

            bool ICollection<TKey>.IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            public KeyCollection(Llrb23Tree<TKey, TValue> llrb)
            {
                this.llrb = llrb;
            }

            public void CopyTo(TKey[] array, int index)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }

                if (index < 0)
                {
                    throw new ArgumentException();
                }

                if (array.Length - index < this.Count)
                {
                    throw new ArgumentException();
                }

                if (this.llrb.root != null)
                {
                    int i = index;

                    foreach (Node node in this.llrb.root)
                    {
                        array[i++] = node.Key;
                    }
                }
            }

            public IEnumerator<TKey> GetEnumerator()
            {
                if (this.llrb.root != null)
                {
                    foreach (Node node in this.llrb.root)
                    {
                        yield return node.Key;
                    }
                }
            }

            void ICollection<TKey>.Add(TKey key)
            {
                throw new NotSupportedException();
            }

            void ICollection<TKey>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<TKey>.Contains(TKey key)
            {
                return this.llrb.ContainsKey(key);
            }

            bool ICollection<TKey>.Remove(TKey key)
            {
                throw new NotSupportedException();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
