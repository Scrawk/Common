using System;
using System.Collections.Generic;

namespace Common.Collections.Lists
{

    public class SortedList<T>
    {

        public int Count { get; private set; }

        public SortedListNode<T> Root { get; private set; }

        private IComparer<T> m_comparer;

        public SortedList(IComparer<T> comparer)
        {
            m_comparer = comparer;
        }

        public IEnumerator<T> GetEnumerator()
        {
            SortedListNode<T> current = Root;

            while (current != null)
            {
                yield return current.Item;
                current = current.Next;
            }

        }

        public List<T> ToList()
        {
            List<T> list = new List<T>(Count);
            SortedListNode<T> current = Root;

            while (current != null)
            {
                list.Add(current.Item);
                current = current.Next;
            }

            return list;
        }

        public void Clear()
        {
            Root = null;
            Count = 0;
        }

        public void Remove(T item)
        {
            if (item == null) return;
            if (Root == null) return;

            SortedListNode<T> parent = null;
            SortedListNode <T> current = Root;

            while (current != null)
            {
                if (current.Item.Equals(item)) break;
                parent = current;
                current = current.Next;
            }

            if (current == null) return;

            if (parent == null)
                Root = current.Next;
            else
                parent.Next = current.Next;

            Count--;
        }

        public void Add(T item)
        {
            if (item == null) return;

            Count++;

            if (Root == null)
            {
                Root = new SortedListNode<T>(item);
                return;
            }

            SortedListNode<T> previous = null;
            SortedListNode<T> current = Root;

            while (true)
            {
                if (current == null)
                {
                    if (previous == null)
                        throw new NullReferenceException("Previous should not be null.");

                    previous.Next = new SortedListNode<T>(item);
                    return;
                }

                if (m_comparer.Compare(item, current.Item) <= 0)
                {
                    SortedListNode<T> node = new SortedListNode<T>(item);
                    node.Next = current;

                    if (previous != null)
                        previous.Next = node;
                    else
                        Root = node;

                    return;
                }

                previous = current;
                current = current.Next;
            }

        }
    }
}
