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
    {

        /// <summary>
        /// The list that contains the data.
        /// </summary>
        private List<T> m_list;

        /// <summary>
        /// Does the list need to be resorted.
        /// </summary>
        private bool m_isDirty = true;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PriorityList()
        {
            m_list = new List<T>();
        }

        /// <summary>
        /// Construct a list with a capacity size.
        /// </summary>
        /// <param name="size"></param>
        public PriorityList(int  size)
        {
            m_list = new List<T>(size);
        }

        /// <summary>
        /// The number of elements in the list.
        /// </summary>
        public int Count
        {
            get { return m_list.Count; }
        }

        /// <summary>
        /// The capacity of the list.
        /// </summary>
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
        /// Access a element at index i.
        /// </summary>
        public T this[int i]
        {
            get 
            {
                Sort();
                return m_list[i]; 
            }
            set 
            {
                m_isDirty = true;
                m_list[i] = value; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[PriorityList: Count={0}]", Count);
        }

        /// <summary>
        /// Clear the list.
        /// </summary>
        public void Clear()
        {
            m_list.Clear();
        }

        /// <summary>
        /// Add a range of items to the list.
        /// </summary>
        /// <param name="items"></param>
        public void Add(IEnumerable<T> items)
        {
            m_isDirty = true;
            m_list.AddRange(items);
        }

        /// <summary>
        /// Add a item to the list.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Add(T item)
        {
            m_isDirty = true;
            m_list.Add(item);
            return true;
        }

        /// <summary>
        /// Return the first item in the list.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            Sort();
            return m_list[0];
        }

        /// <summary>
        /// Return and remove the first item in the list.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            Sort();
            T item = m_list[0];
            m_list.RemoveAt(0);
            return item;
        }

        /// <summary>
        /// Remove the item from the list.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            return m_list.Remove(item);
        }

        /// <summary>
        /// Return a list of the items.
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            Sort();
            return new List<T>(m_list);
        }

        /// <summary>
        /// Enumerate through the items.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            Sort();
            return m_list.GetEnumerator();
        }

        /// <summary>
        /// Enumerate through the items.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Sort the list.
        /// </summary>
        private void Sort()
        {
            if (!m_isDirty) return;

            if (Comparer != null)
                m_list.Sort(Comparer);
            else
            {
                m_list.Sort();
            }

            m_isDirty = false;
        }
    }
}
