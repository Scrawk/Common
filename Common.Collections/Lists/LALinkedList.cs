using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Lists
{

    public class LALinkedList<TYPE> : ICollection<TYPE> where TYPE : class
    {

        private int m_count;

        private int m_users;

        private List<LALinkedListNode<TYPE>> m_cache;

        public LALinkedList()
        {
            MaxCacheSize = 100;
            m_cache = new List<LALinkedListNode<TYPE>>();
        }

        public int MaxCacheSize { get; set; }

        public int Count { get { return m_count; } }

        public LALinkedListNode<TYPE> First { get; private set; }

        public LALinkedListNode<TYPE> Last { get; private set; }

        public bool IsReadOnly { get { return false; } }

        public IEnumerator<TYPE> GetEnumerator()
        {
            m_users++;
            var current = First;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }

            m_users--;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddFirst(TYPE item)
        {
            if (m_users != 0)
                throw new InvalidOperationException("Collection was modified after the enumerator was instantiated");

            var node = NewNode();
            node.Value = item;
            node.Next = First;

            First = node;

            if (Last == null)
                Last = First;

            m_count++;
        }

        public void Add(TYPE item)
        {
            AddLast(item);
        }

        public void AddLast(TYPE item)
        {
            if (m_users != 0)
                throw new InvalidOperationException("Collection was modified after the enumerator was instantiated");

            var node = NewNode();
            node.Value = item;

            if (Last == null)
                First = Last = node;
            else
            {
                Last.Next = node;
                Last = Last.Next;
            }

            m_count++;
        }

        public void RemoveFirst()
        {
            if (m_users != 0)
                throw new InvalidOperationException("Collection was modified after the enumerator was instantiated");

            if (Count == 0) return;
            else
            {
                var tmp = First;
                First = First.Next;
                if (First == null) Last = null;

                CacheNode(tmp);
                m_count--;
            }
        }

        public void RemoveLast()
        {
            if (m_users != 0)
                throw new InvalidOperationException("Collection was modified after the enumerator was instantiated");

            if (Count == 0) return;
            else if(Count == 1)
            {
                var tmp = First;
                First = Last = null;

                CacheNode(tmp);
                m_count = 0;
            }
            else
            {
                var current = First;

                for (int i = 0; i < Count - 2; i++)
                    current = current.Next;

                var tmp = Last;
                Last = current;
                Last.Next = null;

                CacheNode(tmp);
                m_count--;
            }
        }

        public bool Remove(TYPE item)
        {
            if (m_users != 0)
                throw new InvalidOperationException("Collection was modified after the enumerator was instantiated");

            if (Count == 0) return false;

            if (item == First.Value)
            {
                RemoveFirst();
                return true;
            }
            else if (item == Last.Value)
            {
                RemoveLast();
                return true;
            }
            else
            {
                LALinkedListNode<TYPE> previous = null;
                LALinkedListNode<TYPE> current = First;

                while (current != null)
                {
                    if (item == current.Value)
                    {
                        previous.Next = current.Next;
                        m_count--;

                        CacheNode(current);
                        return true;
                    }

                    previous = current;
                    current = current.Next;
                }

                return false;
            }
        }

        public void Clear()
        {
            var current = First;
            while (current != null)
            {
                var tmp = current;
                current = current.Next;
                CacheNode(tmp);
            }

            First = null;
            Last = null;
            m_count = 0;
        }

        public bool Contains(TYPE item)
        {
            var current = First;
            while(current != null)
            {
                if (item == current.Value) return true;
                current = current.Next;
            }

            return false;
        }

        public void CopyTo(TYPE[] array, int arrayIndex)
        {
            var current = First;
            while (current != null)
            {
                array[arrayIndex++] = current.Value;
                current = current.Next;
            }
        }

        private LALinkedListNode<TYPE> NewNode()
        {
            int count = m_cache.Count;

            if (count == 0)
                return new LALinkedListNode<TYPE>();
            else
            {
                var tmp = m_cache[count - 1];
                m_cache.RemoveAt(count - 1); //Should be O(1)
                return tmp;
            }
        }

        private void CacheNode(LALinkedListNode<TYPE> node)
        {
            node.Value = null;
            node.Next = null;

            if (m_cache.Count < MaxCacheSize)
                m_cache.Add(node);
        }

    }

}
