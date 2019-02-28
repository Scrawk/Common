using System;
using System.Collections.Generic;

namespace Common.Collections.Sets
{

    public class DisjointMappedSet
    {

        private Dictionary<int, int> m_parent;

        private Dictionary<int, int> m_rank;

        public DisjointMappedSet()
        {
            m_parent = new Dictionary<int, int>();
            m_rank = new Dictionary<int, int>();
        }

        public int Count => m_rank.Count;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[DisjointMappedSet: Count={0}]", Count);
        }

        public void Clear()
        {
            m_parent.Clear();
            m_rank.Clear();
        }

        public bool Contains(int id)
        {
            return m_parent.ContainsKey(id);
        }

        public void Add(int id, int parent)
        {
            if(m_parent.ContainsKey(id))
                m_parent[id] = parent;
            else
            {
                m_parent.Add(id, parent);
                m_rank.Add(id, 1);
            }
        }

        public int FindParent(int id)
        {
            if (m_parent[id] != m_parent[m_parent[id]])
                m_parent[id] = FindParent(m_parent[id]);

            return m_parent[id];
        }

        public bool Union(int from, int to)
        {

            int x = FindParent(from);
            int y = FindParent(to);

            if (x == y) return false;

            // make sure rank[xx] is smaller
            if (m_rank[x] > m_rank[y]) { int t = x; x = y; y = t; }

            // if both are equal, the combined tree becomes 1 deeper
            if (m_rank[x] == m_rank[y]) m_rank[y]++;

            m_parent[x] = y;

            return true;

        }

    }


}