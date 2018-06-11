using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Lists
{
    public class LALinkedListEnumerator<TYPE> : IEnumerator<TYPE> where TYPE : class
    {

        public TYPE Current { get; private set; }

        object IEnumerator.Current { get { return Current; } }

        internal bool InUse { get; set; }

        private int m_version;

        private LALinkedListNode<TYPE> m_current;

        private LALinkedList<TYPE> m_list;

        public LALinkedListEnumerator(LALinkedList<TYPE> list)
        {
            m_list = list;
            m_current = list.First;
            m_version = list.Version;
        }

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            if (m_version != m_list.Version)
                throw new InvalidOperationException("Collection was modified after the enumerator was insantiated");

            if (m_current == null)
            {
                InUse = false;
                return false;
            }
            else
            {
                Current = m_current.Value;
                m_current = m_current.Next;
                return true;
            }
        }

        public void Reset()
        {
            InUse = false;
            Current = null;
            m_current = m_list.First;
            m_version = m_list.Version;
        }
    }
}
