using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Queues
{

    /// <summary>
    /// A naive implementation of a priority queue
    /// using a list. Used for Debuging and performance tests.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityLinkedList<T> : IPriorityQueue<T>
        where T : IComparable<T>
    {

        private LinkedList<T> m_list;

        private bool m_isDirty = true;

        public PriorityLinkedList()
        {
            m_list = new LinkedList<T>();
        }

        public int Count
        {
            get { return m_list.Count; }
        }

        public int Capacity
        {
            get { return 0; }
            set { ; }
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
            return string.Format("[PriorityLinkedList: Count={0}]", Count);
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
            foreach (var i in m_list)
                if (i.CompareTo(item) == 0) return true;

            return false;
        }

        public bool FindPredecessor(T item, out T predecessor)
        {
            if(Count == 0)
            {
                predecessor = default(T);
                return false;
            }

            Sort();

            for (var node = m_list.First; node != m_list.Last.Next; node = node.Next)
            {
                if (node.Value.CompareTo(item) == 0)
                {
                    if (node.Previous == null)
                    {
                        predecessor = default(T);
                        return false;
                    }
                    else
                    {
                        predecessor = node.Previous.Value;
                        return true;
                    }
                }
            }

            predecessor = default(T);
            return false;
        }

        public bool FindSuccesor(T item, out T succesor)
        {
            if (Count == 0)
            {
                succesor = default(T);
                return false;
            }

            Sort();

            for (var node = m_list.First; node != m_list.Last.Next; node = node.Next)
            {
                if (node.Value.CompareTo(item) == 0)
                {
                    if (node.Next == null)
                    {
                        succesor = default(T);
                        return false;
                    }
                    else
                    {
                        succesor = node.Previous.Value;
                        return true;
                    }
                }
            }

            succesor = default(T);
            return false;
        }

        public T Peek()
        {
            Sort();
            return m_list.First.Value;
        }

        public bool Remove(T item)
        {
            //int i = IndexOf(item);
            //if (i < 0) return false;
            //m_list.RemoveAt(i);
            return true;
        }

        public T RemoveFirst()
        {
            Sort();
            T item = m_list.First.Value;
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

            if (Comparer != null)
                m_list.Sort(Comparer);
            else
                m_list.Sort();

            m_isDirty = false;
        }
    }
}
