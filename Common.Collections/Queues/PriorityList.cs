using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Queues
{

    /// <summary>
    /// A naive implementation of a priority queue
    /// using a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityList<T> : IPriorityQueue<T>
        where T : IComparable<T>
    {

        private List<T> m_list;

        private bool m_isDirty = true;

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

        /// <summary>
        /// Optional comparer to use.
        /// </summary>
        public IComparer<T> Comparer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[PriorityList: Count={0}]", Count);
        }

        public void Add(IEnumerable<T> items)
        {
            m_isDirty = true;
            m_list.AddRange(items);
        }

        public bool Add(T item)
        {
            m_isDirty = true;
            m_list.Add(item);
            return true;
        }

        /// <summary>
        /// Find if the item is in the list.
        /// This utilizes the type T's Comparer 
        /// and will consider items  the same 
        /// order the same object.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Find the index of the item in the list.
        /// This utilizes the type T's Comparer 
        /// and will consider items  the same 
        /// order the same object.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            Sort();

            int i = 0;
            if(Comparer != null)
                i = m_list.BinarySearch(item, Comparer);
            else
                i = m_list.BinarySearch(item);

            return (i < 0) ? -1 : i;
        }

        public bool FindPredecessor(T item, out T predecessor)
        {
            Sort();
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
            Sort();
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
            Sort();
            return m_list[0];
        }

        public bool Remove(T item)
        {
            int i = IndexOf(item);
            if (i < 0) return false;
            m_list.RemoveAt(i);
            return true;
        }

        public T RemoveFirst()
        {
            Sort();
            T item = m_list[0];
            m_list.RemoveAt(0);
            return item;
        }

        public List<T> ToList()
        {
            Sort();
            return new List<T>(m_list);
        }

        public void Clear()
        {
            m_list.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Sort()
        {
            if (!m_isDirty) return;

            if(Comparer != null)
                m_list.Sort(Comparer);
            else
                m_list.Sort();

            m_isDirty = false;
        }
    }
}
