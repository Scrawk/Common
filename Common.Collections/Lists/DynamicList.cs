using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Common.Collections.Lists
{
    public class DynamicList<T> : IDynamicList<T>
    {
        private List<T> m_list;

        public DynamicList()
        {
            m_list = new List<T>();
        }

        public DynamicList(int count)
        {
            m_list = new List<T>(count);
        }

        public DynamicList(IEnumerable<T> items)
        {
            m_list = new List<T>(items);
        }

        public int Count => m_list.Count;

        public int Capacity
        {
            get { return m_list.Capacity; }
            set { m_list.Capacity = value; }
        }

        public override string ToString()
        {
            return string.Format("[DynamicList: Count={0}]", Count);
        }

        public T this[int i]
        {
            get { return m_list[i]; }
            set { m_list[i] = value; }
        }

        public void Add(T item)
        {
            m_list.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            m_list.AddRange(items);
        }

        public void Clear()
        {
            m_list.Clear();
        }

        public bool Contains(T item)
        {
            return m_list.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return m_list.IndexOf(item);
        }

        public bool Remove(T item)
        {
            return m_list.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
