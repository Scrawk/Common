using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Queues
{

    /// <summary>
    /// A naive implementation of a priority queue
    /// using a list. Stores the items in reverse
    /// so removing first item is really removing from 
    /// end which is faster.
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
                return m_list[Count - 1 - i];
            }
            set 
            {
                m_isDirty = true;
                m_list[Count - 1 - i] = value; 
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
            return m_list[Count-1];
        }

        /// <summary>
        /// Return and remove the first item in the list.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            Sort();
            T item = m_list[Count-1];
            m_list.RemoveAt(Count - 1);
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
            var list = new List<T>(m_list);
            list.Reverse();
            return list;
        }

        /// <summary>
        /// Enumerate through the items.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            Sort();
            for (int i = Count - 1; i >= 0; i--)
                yield return m_list[i];
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

            m_list.Sort(ReverseCompare);
            m_isDirty = false;
        }

        /// <summary>
        /// The function used to compare the items.
        /// Will reverse the order so end of list
        /// contains the lowest values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int ReverseCompare(T x, T y)
        {
            int i;

            if (Comparer != null)
                i = Comparer.Compare(x, y);
            else
                i = Comparer<T>.Default.Compare(x, y);

            return i * -1;
        }

    }
}
