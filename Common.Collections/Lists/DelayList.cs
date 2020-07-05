using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Common.Collections.Lists
{
    public class DelayList<T> : IDynamicList<T>
    {
        private struct Record<T>
        {
            public T item;
            public bool isRemoved;
        }

        private bool m_isDirty;

        private List<Record<T>> m_listA, m_listB;

        public DelayList()
        {
            m_listA = new List<Record<T>>();
            m_listB = new List<Record<T>>();
        }

        public DelayList(int count)
        {
            m_listA = new List<Record<T>>(count);
            m_listB = new List<Record<T>>(count);
        }

        public DelayList(IEnumerable<T> items)
        {
            m_listA = new List<Record<T>>();
            m_listB = new List<Record<T>>();

            foreach (var item in items)
                Add(item);
            
        }

        public int Count { get; private set; }

        public int Capacity
        {
            get 
            { 
                return m_listA.Capacity; 
            }
            set 
            {
                Sync();
                m_listA.Capacity = value; 
            }
        }

        public override string ToString()
        {
            return string.Format("[DelayList: Count={0}]", Count);
        }

        public T this[int i]
        {
            get 
            {
                Sync();
                return m_listA[i].item; 
            }
            set
            {
                m_listA[i] = new Record<T>() { item = value }; 
            }
        }

        public void Add(T item)
        {
            Count++;
            m_listA.Add(new Record<T>() { item = item });
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public void Clear()
        {
            Count = 0;
            m_isDirty = false;
            m_listA.Clear();
            m_listB.Clear();
        }

        public bool Contains(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < m_listA.Count; i++)
            {
                var record = m_listA[i];
                if (record.isRemoved) continue;

                if (comparer.Equals(m_listA[i].item, item)) 
                    return true;
            }
                
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for(int i = 0; i < m_listA.Count; i++)
            {
                var record = m_listA[i];
                if (record.isRemoved) continue;

                yield return record.item;
            }
                
        }

        public int IndexOf(T item)
        {
            Sync();
            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < m_listA.Count; i++)
            {
                if (comparer.Equals(m_listA[i].item, item))
                    return i;
            }

            return -1;
        }

        public bool Remove(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < m_listA.Count; i++)
            {
                var record = m_listA[i];
                if (record.isRemoved) continue;

                if (comparer.Equals(record.item, item))
                {
                    Count--;
                    m_isDirty = true;
                    m_listA[i] = new Record<T>() { isRemoved = true };
                    return true;
                }
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Sync()
        {
            if (!m_isDirty) return;
            m_listB.Clear();

            for (int i = 0; i < m_listA.Count; i++)
            {
                var record = m_listA[i];
                if (record.isRemoved) continue;
                m_listB.Add(record);
            }

            m_listA.Clear();

            var tmp = m_listA;
            m_listA = m_listB;
            m_listB = tmp;

            Count = m_listA.Count;
            m_isDirty = false;
        }
    }
}
