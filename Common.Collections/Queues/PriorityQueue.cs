using System;
using System.Collections.Generic;

using Common.Collections.Lists;

namespace Common.Collections.Queues
{

    public class PriorityQueue<T> 
    {

        public int Count { get { return m_list.Count; } }

        public SortedListNode<T> Root { get { return m_list.Root; } }

        private SortedList<T> m_list;

        public PriorityQueue(IComparer<T> comparer)
        {
            m_list = new SortedList<T>(comparer);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

        public List<T> ToList()
        {
            return m_list.ToList();
        }

        public void Clear()
        {
            m_list.Clear();
        }

        public T Pop()
        {
            if (m_list.Count == 0) return default(T);

            T first = m_list.Root.Item;
            m_list.Remove(first);

            return first;
        }

        public void Push(T item)
        {
            m_list.Add(item);
        }

    }
}
