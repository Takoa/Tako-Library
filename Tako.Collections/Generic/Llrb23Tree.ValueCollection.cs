using System;
using System.Collections.Generic;

namespace Tako.Collections.Generic
{
    public partial class Llrb23Tree<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public class ValueCollection : ICollection<TValue>
        {
            private Llrb23Tree<TKey, TValue> llrb;
            private IEqualityComparer<TValue> equalityComparer;

            public int Count
            {
                get
                {
                    return this.llrb.Count;
                }
            }

            bool ICollection<TValue>.IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            public ValueCollection(Llrb23Tree<TKey, TValue> llrb)
            {
                this.llrb = llrb;
                this.equalityComparer = EqualityComparer<TValue>.Default;
            }

            public void CopyTo(TValue[] array, int index)
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
                        array[i++] = node.Value;
                    }
                }
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                if (this.llrb.root != null)
                {
                    foreach (Node node in this.llrb.root)
                    {
                        yield return node.Value;
                    }
                }
            }

            void ICollection<TValue>.Add(TValue value)
            {
                throw new NotSupportedException();
            }

            void ICollection<TValue>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<TValue>.Contains(TValue value)
            {
                if (this.llrb.root != null)
                {
                    if (value == null)
                    {
                        foreach (Node node in this.llrb.root)
                        {
                            if (node.Value == null)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        foreach (Node node in this.llrb.root)
                        {
                            if (this.equalityComparer.Equals(value, node.Value))
                            {
                                return true;
                            }
                        } 
                    }
                }

                return false;
            }

            bool ICollection<TValue>.Remove(TValue value)
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
