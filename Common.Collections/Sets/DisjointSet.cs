using System;
using System.Collections.Generic;

namespace Common.Collections.Sets
{

    public class DisjointSet
    {

        private int[] m_parent;

        private int[] m_rank;

        public DisjointSet(int size)
        {
            m_parent = new int[size];
            m_rank = new int[size];
        }

        public void Clear()
        {
            Array.Clear(m_parent, 0, m_parent.Length);
            Array.Clear(m_rank, 0, m_rank.Length);
        }

        public void Add(int idx, int parent)
        {
            m_parent[idx] = parent;
            m_rank[idx] = 1;
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