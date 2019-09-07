using System;
using System.Collections.Generic;

namespace Common.Collections.Sets
{

    /// <summary>
    /// a disjoint-set data structure is a data
    /// structure that tracks a set of elements
    /// partitioned into a number of disjoint 
    /// (non-overlapping) subsets.
    /// Elements in the set must be integer 
    /// ranging from 0 to Count-1.
    /// </summary>
    public class DisjointSet
    {
        /// <summary>
        /// The parents of the element at each index.
        /// All elements with the same parent belong 
        /// to the same set. A element may have its parent
        /// set to -1 to indicate this element has 
        /// been deleted. If a elements parent is itsself
        /// then this is a root element.
        /// </summary>
        private int[] m_parent;

        private int[] m_rank;

        public DisjointSet(int size)
        {
            m_parent = new int[size];
            m_rank = new int[size];
        }

        public int Count => m_rank.Length;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[DisjointSet: Count={0}]", Count);
        }

        /// <summary>
        /// Clear the set.
        /// </summary>
        public void Clear()
        {
            Array.Clear(m_parent, 0, m_parent.Length);
            Array.Clear(m_rank, 0, m_rank.Length);
        }

        /// <summary>
        /// Added a new element and set what 
        /// the elements parent is.
        /// </summary>
        public void Add(int idx, int parent)
        {
            m_parent[idx] = parent;
            m_rank[idx] = 1;
        }

        /// <summary>
        /// Find the elements parent. 
        /// If the element has been deleted its parent is -1.
        /// If the elements parent is itself it is the root element.
        /// </summary>
        public int FindParent(int id)
        {
            int p = m_parent[id];
            if (p == -1) return -1;

            if (m_parent[id] != m_parent[m_parent[id]])
                m_parent[id] = FindParent(m_parent[id]);

            return m_parent[id];
        }

        /// <summary>
        /// Merge the two sets.
        /// </summary>
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