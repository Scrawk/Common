using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Lists
{

    public class LALinkedList<TYPE> : IEnumerable<TYPE> where TYPE : class
    {

        public int MaxCacheSize { get; set; }

        public int Count { get; private set; }

        public LALinkedListNode<TYPE> First { get; private set; }

        public LALinkedListNode<TYPE> Last { get; private set; }

        internal int Version { get; private set; }

        private List<LALinkedListNode<TYPE>> m_cache;

        private LALinkedListEnumerator<TYPE> m_enumerator;

        public LALinkedList()
        {
            MaxCacheSize = 100;
            m_cache = new List<LALinkedListNode<TYPE>>();
            m_enumerator = new LALinkedListEnumerator<TYPE>(this);
        }

        public IEnumerator<TYPE> GetEnumerator()
        {
            if(m_enumerator.InUse)
                return new LALinkedListEnumerator<TYPE>(this);
            else
            {
                m_enumerator.Reset();
                m_enumerator.InUse = true;
                return m_enumerator;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddFirst(TYPE item)
        {
            Version++;

            var node = NewNode();
            node.Value = item;
            node.Next = First;

            First = node;

            if (Last == null)
                Last = First;

            Count++;
        }

        public void AddLast(TYPE item)
        {
            Version++;

            var node = NewNode();
            node.Value = item;

            if (Last == null)
                First = Last = node;
            else
            {
                Last.Next = node;
                Last = Last.Next;
            }

            Count++;
        }

        public void RemoveFirst()
        {
            Version++;

            if (Count == 0) return;
            else
            {
                var tmp = First;
                First = First.Next;
                if (First == null) Last = null;

                CacheNode(tmp);
                Count--;
            }
        }

        public void RemoveLast()
        {
            Version++;

            if (Count == 0) return;
            else if(Count == 1)
            {
                var tmp = First;
                First = Last = null;

                CacheNode(tmp);
                Count = 0;
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
                Count--;
            }
        }

        public void Remove(TYPE item)
        {
            Version++;

            if (Count == 0) return;

            if(item == First.Value)
                RemoveFirst();
            else if(item == Last.Value)
                RemoveLast();
            {
                LALinkedListNode<TYPE> previous = null;
                LALinkedListNode<TYPE> current = First;

                while (current != null)
                {
                    if (item == current.Value)
                    {
                        previous.Next = current.Next;
                        Count--;

                        CacheNode(current);
                        return;
                    }

                    previous = current;
                    current = current.Next;
                }
            }

        }

        public void Clear()
        {
            Version++;

            var current = First;
            while (current != null)
            {
                var tmp = current;
                current = current.Next;
                CacheNode(tmp);
            }

            First = null;
            Last = null;
            Count = 0;
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
