using System;
using System.Collections.Generic;

namespace Common.Collections.Queues
{

    /// <summary>
    /// A naive implementation of a priority queue
    /// using a list. Used for Debuging and performance tests.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityList<T> : IPriorityQueue<T>
        where T : IComparable<T>
    {

        private List<T> m_list;

        public PriorityList()
        {
            m_list = new List<T>();
        }

        public PriorityList(int  size)
        {
            m_list = new List<T>(size);
        }

        public int Count
        {
            get { return m_list.Count; }
        }

        public int Capacity
        {
            get { return m_list.Capacity;  }
            set { m_list.Capacity = value; }
        }

        public void Add(IEnumerable<T> items)
        {
            m_list.AddRange(items);
            m_list.Sort();
        }

        public void Add(T item)
        {
            m_list.Add(item);
            m_list.Sort();
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public int IndexOf(T item)
        {
            int i = m_list.BinarySearch(item);
            return (i < 0) ? -1 : i;
        }

        public bool FindPredecessor(T item, out T predecessor)
        {
            int i = IndexOf(item);
            if (i <= 0)
            {
                predecessor = default(T);
                return false;
            }
            else
            {
                predecessor = m_list[i - 1];
                return true;
            }
        }

        public bool FindSuccesor(T item, out T succesor)
        {
            int i = IndexOf(item);
            if (i < 0 || i >= Count - 1)
            {
                succesor = default(T);
                return false;
            }
            else
            {
                succesor = m_list[i + 1];
                return true;
            }
        }

        public T Peek()
        {
            return m_list[0];
        }

        public bool Remove(T item)
        {
            int i = IndexOf(item);
            if (i < 0) return false;
            m_list.RemoveAt(i);
            return true;
        }

        public List<T> ToList()
        {
            return new List<T>(m_list);
        }

        public void Clear()
        {
            m_list.Clear();
        }
    }
}
